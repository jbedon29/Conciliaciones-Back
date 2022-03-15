using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Protecta.Application.Service.Dtos.Cobranzas;
using Protecta.Application.Service.Models;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg;
using Microsoft.Extensions.Configuration;
using Protecta.CrossCuting.Utilities.Configuration;
using System.Text;
using System.Net;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Data;
using Protecta.CrossCuting.Utilities.Files;
using Protecta.CrossCuting.Utilities.Files.IFiles;
using Protecta.CrossCuting.Utilities.ManageFiles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Syncfusion.XlsIO;
using System.Transactions;
using Detalles = Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg.Detalles;

namespace Protecta.Application.Service.Services.CobranzasModule
{
    public class CobranzasService : ICobranzasService
    {
        private readonly ICobranzaRepository _cobranzasRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly Ldap _ldapSettings;
        private string NumeroCuenta { get; set; }
        private string IdMoneda { get; set; }
        private int FlagExtorno { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CobranzasService));

        public CobranzasService(ICobranzaRepository cobranzaRepository, ILoggerManager logger, IMapper mapper, IOptions<Ldap> ldapSettings)
        {

            this._cobranzasRepository = cobranzaRepository;
            _logger = logger;
            //_mapper = mapper;
            _mapper = new MapperConfiguration(Config => { Config.CreateMissingTypeMaps = true; }).CreateMapper();
            _ldapSettings = ldapSettings.Value;
        }
        public async Task<IEnumerable<BancoDto>> ListarBancos()
        {
            IEnumerable<BancoDto> BancoDtos = null;

            try
            {
                var bancoResult = await _cobranzasRepository.ListarBancos();
                if (bancoResult == null) return null;
                BancoDtos = _mapper.Map<IEnumerable<BancoDto>>(bancoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return BancoDtos;
        }

        public void ValidateProforma(ref ConciliacionDto conciliacionDto, ref List<string> ListProforma, string Line, int segmento, int idbanco)
        {
            if (conciliacionDto.IsValido && (((idbanco == 1 || idbanco == 2) && segmento == 2) || (idbanco == 3 && segmento == 1)))
            {
                if (ListProforma.Contains(conciliacionDto.NumeroRecibo.Trim()))
                {
                    conciliacionDto.IsValido = false;
                    conciliacionDto.Mensaje = "Excepción en la Ln " + Line + " : El código de proforma ya existe en la trama.";
                }
                else
                {
                    ListProforma.Add(conciliacionDto.NumeroRecibo.Trim());
                }
            }
        }



        public async Task<TramaDto> ValidarTrama(string usercode, string base64String, int idbanco, int idproducto, string idproceso, string fechaInicio, string fechaFinal, string CodProforma)
        {


            int CantidadCab = 0;
            string MontoCab = string.Empty;
            decimal MontoDet = 0;
            int CantidadDet = 0;


            string MontoOrigen = string.Empty;
            decimal MontoDetOrigen = 0;


            Boolean ExistCab = true, ExistPie = true;
            Boolean ExistDet = false;
            List<string> ListProforma = new List<string>();


            TramaDto tramaDto = new TramaDto();
            ConciliacionDto conciliacionDto = new ConciliacionDto();
            DateTime tinicial = DateTime.Now;
            tramaDto.listado = new List<ListadoConciliacionDto>();
            int totalfilas = 1;
            int indice = 1;
            bool ProcesoValido = true;
            idproceso = string.IsNullOrEmpty(idproceso) ? ObtenerIdProceso(idbanco, idproducto, usercode) : idproceso;


            if (idbanco != 0)
            {
                Stream stream = new MemoryStream(Convert.FromBase64String(base64String));
                StreamReader file = new StreamReader(stream);

                string line;
                int numeroFilas = 1;
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        totalfilas++;
                    }
                }
                stream = new MemoryStream(Convert.FromBase64String(base64String));
                file = new StreamReader(stream);

                while ((line = file.ReadLine()) != null)
                {

                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        tramaDto.IdProducto = idproducto;
                        tramaDto.StringTrama = line.Trim();
                        tramaDto.IdBanco = idbanco;
                        tramaDto.Fila = numeroFilas.ToString();
                        tramaDto.Segmento = ObtenerSegmento(numeroFilas, totalfilas, idbanco);
                        tramaDto.TipoIngreso = "R";

                        var tramaResult = await _cobranzasRepository.ValidarTrama(_mapper.Map<Trama>(tramaDto));
                        conciliacionDto = _mapper.Map<ConciliacionDto>(tramaResult);



                        if (tramaDto.Segmento == 1 && conciliacionDto.IsValido == false) { ExistCab = false; }
                        if (tramaDto.Segmento == 3 && conciliacionDto.IsValido == false) { ExistPie = false; }
                        ValidateProforma(ref conciliacionDto, ref ListProforma, tramaDto.Fila, tramaDto.Segmento, idbanco);
                        if (((idbanco == 1 || idbanco == 2) && tramaDto.Segmento == 2) || (idbanco == 3 && tramaDto.Segmento == 1)) { ExistDet = true; }

                        if (conciliacionDto.IsValido)
                        {
                            if (((idbanco == 1) && tramaDto.Segmento == 1) || (idbanco == 2 && tramaDto.Segmento == 3))
                            {
                                CantidadCab = Int32.Parse(tramaResult.CantTotal);
                                MontoCab = FormatoImporte(tramaResult.MontoTotal);
                                if (idbanco == 2)
                                {
                                    MontoOrigen = FormatoImporte(tramaResult.MontoTotalOrigen);
                                }
                            }

                            conciliacionDto.IdBanco = idbanco;
                            conciliacionDto.IdProducto = idproducto;
                            conciliacionDto.IdProceso = idproceso;
                            conciliacionDto.UserCode = usercode;

                            if (((idbanco == 1 || idbanco == 2) && tramaDto.Segmento == 2) || (idbanco == 3 && tramaDto.Segmento == 1))
                            {
                                CantidadDet++;
                                MontoDet = MontoDet + Convert.ToDecimal(FormatoImporte(conciliacionDto.Importe));
                                if (idbanco == 2)
                                {
                                    MontoDetOrigen = MontoDetOrigen + Convert.ToDecimal(FormatoImporte(conciliacionDto.ImporteOrigen));
                                }
                                tramaDto.listado.Add(ObtenerListadoConciliacion(conciliacionDto));
                            }
                            else if (idbanco == 2 && tramaDto.Segmento == 1)
                            {
                                this.NumeroCuenta = conciliacionDto.NumeroCuenta;
                                this.IdMoneda = conciliacionDto.IdMoneda;
                            }

                            tramaDto.EsValido = conciliacionDto.IsValido;
                            tramaDto.Mensaje = conciliacionDto.Mensaje;

                            indice++;
                        }
                        else
                        {
                            tramaDto.listado.Add(new ListadoConciliacionDto { IsValido = conciliacionDto.IsValido, Mensaje = conciliacionDto.Mensaje, TipoOperacion = "GP" });
                            ProcesoValido = false;
                            tramaDto.EsValido = conciliacionDto.IsValido;
                            //break;
                        }
                        numeroFilas++;
                        //if (!conciliacionDto.IsValido)
                        //    {
                        //        ProcesoValido = false;
                        //        tramaDto.EsValido = conciliacionDto.IsValido;
                        //        break;
                        //    }
                    }
                    else
                    {
                        ProcesoValido = false;
                        break;
                    }

                }
                try
                {
                    if ((idbanco == 1 && ExistCab && ExistDet) || (idbanco == 2 && tramaDto.Segmento == 3 && ExistPie && ExistDet))
                    {
                        if (CantidadCab != CantidadDet)
                        {
                            tramaDto.listado.Add(new ListadoConciliacionDto { IsValido = false, Mensaje = "Excepción " + ((idbanco == 2) ? "Pie" : "Cabecera") + " : La cantidad total de registros enviados no corresponde con el detalle.", TipoOperacion = "GP" });
                        }
                        if (MontoDet.ToString("N2") != Convert.ToDecimal(MontoCab).ToString("N2"))
                        {
                            tramaDto.listado.Add(new ListadoConciliacionDto { IsValido = false, Mensaje = "Excepción " + ((idbanco == 2) ? "Pie" : "Cabecera") + " : El monto total de los importes pagados no corresponde con el detalle.", TipoOperacion = "GP" });
                        }
                        if (idbanco == 2)
                        {
                            if (MontoDetOrigen.ToString("N2") != Convert.ToDecimal(MontoOrigen).ToString("N2"))
                            {
                                tramaDto.listado.Add(new ListadoConciliacionDto { IsValido = false, Mensaje = "Excepción " + ((idbanco == 2) ? "Pie" : "Cabecera") + " : El monto total de pagos no corresponde con la suma de importes de origen.", TipoOperacion = "GP" });
                            }
                        }
                    }
                    if (!ExistDet)
                    {
                        tramaDto.listado.Add(new ListadoConciliacionDto { IsValido = false, Mensaje = "Excepción Detalle : No se encontró ningún detalle en la trama.", TipoOperacion = "GP" });
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                try
                {
                    var LiqManual = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.ObtenerLiquidacionManual(idproceso, idproducto, idbanco, CodProforma, fechaInicio, fechaFinal, usercode));
                    if (LiqManual.Code != "1")
                    {
                        var LstConciliacion = _mapper.Map<List<ConciliacionDto>>(LiqManual.Data);
                        foreach (ConciliacionDto conciliacion in LstConciliacion)
                        {
                            conciliacion.UserCode = usercode;
                            var ListaConciliacion = ObtenerListadoConciliacionManual(_mapper.Map<ConciliacionDto>(conciliacion));
                            tramaDto.listado.Add(ListaConciliacion);
                        }
                        tramaDto.EsValido = true;
                        return tramaDto;
                    }
                }
                catch (Exception ex)
                {
                    tramaDto.EsValido = false;
                    tramaDto.Mensaje = ex.Message;
                }

            }
            //if (ProcesoValido)
            if (tramaDto.listado.Where(x => x.IsValido == true).Count() > 0)
            {
                var result = await _cobranzasRepository.InsertarProceso(_mapper.Map<List<ListaConciliacion>>(tramaDto.listado));
                if (result)
                {
                    tramaDto.EsValido = true;
                    tramaDto.Mensaje = "Procesado con éxito";
                }
                else
                {
                    tramaDto.EsValido = false;
                    tramaDto.Mensaje = "No se pudo realizar la insercion de los documentos";
                }
            }
            else
            {
                tramaDto.EsValido = true;
                tramaDto.Mensaje = "No se pudo Encontraron documentos a insertar";
            }
            DateTime tfinal = DateTime.Now;
            TimeSpan totaltiempo = new TimeSpan(tfinal.Ticks - tinicial.Ticks);
            tramaDto.TiempoTranscurrido = totaltiempo.TotalSeconds.ToString();


            return tramaDto;
        }


        public async Task<PlanillaDto> ObtenerTrama(TramaDto trama)
        {
            PlanillaDto planillaDto = new PlanillaDto();
            var tramaResult = await _cobranzasRepository.ObtenerTrama(_mapper.Map<Trama>(trama));
            if (tramaResult == null) return null;
            planillaDto = _mapper.Map<PlanillaDto>(tramaResult);
            //   var nombreRuta = planillaDto.RutaTrama.Replace('','*').
            planillaDto.RutaTrama = Base64StringEncode(planillaDto.RutaTrama);
            //planillaDto.RutaTrama = Base64StringEncode(@"D:\Kevin\Develop\APPCONCILACIONES\Doc. Protecta\CARGA DE PLANOS AL BANCO\BCP\19-02\crep.txt");
            return planillaDto;
        }
        public async Task<Models.ResponseControl> InsertarFacturaFormaPago(List<ListadoConciliacionDto> listadoConciliacionDtos)
        {
            //var mapper = new MapperConfiguration(config => { config.CreateMissingTypeMaps = true; }).CreateMapper();
            Models.ResponseControl Rpt = new Models.ResponseControl();
            string idproceso = string.Empty, tipooperacion = string.Empty;
            int idproducto, idbanco, usercode;
            var response = await _cobranzasRepository.InsertarProceso(_mapper.Map<List<ListaConciliacion>>(listadoConciliacionDtos));
            if (listadoConciliacionDtos.Count > 0 && response)
            {

                idproceso = listadoConciliacionDtos[0].IdProceso;
                idproducto = int.Parse(listadoConciliacionDtos[0].IdProducto);
                idbanco = int.Parse(listadoConciliacionDtos[0].IdBanco);
                //tipooperacion = listadoConciliacionDtos[0].TipoOperacion;
                tipooperacion = (listadoConciliacionDtos[0].TipoOperacion == "FP") ? "GP" : listadoConciliacionDtos[0].TipoOperacion;
                usercode = int.Parse(listadoConciliacionDtos[0].UserCode);
                Rpt = _mapper.Map<Models.ResponseControl>((await _cobranzasRepository.GeneraPlanillaFactura(idproceso, idproducto, idbanco, tipooperacion, usercode)));

                if (Rpt.Data != null)
                {
                    SendVouchers((List<State_voucher>)Rpt.Data);
                }

                //Rpt = mapper.Map(Obj, Rpt);
                // =  (Obj);

                //cobranzasRepository.GeneraPlanillaFactura(idproceso, idproducto, idbanco, tipooperacion, usercode));
            }
            return Rpt;
        }

        private void SendVouchers(List<State_voucher> state_Voucher)
        {

            foreach (State_voucher voucher in state_Voucher)
            {
                if (voucher.SVALOR == "1") //Se genero la factura
                {
                    State_voucher Response = new State_voucher();
                    var ObjVoucher = _mapper.Map<CobranzaVoucher>(voucher);
                    string JsonRequest = JsonConvert.SerializeObject(ObjVoucher);
                    for (int i = 1; i <= 3; i++)
                    {
                        try
                        {
                            Response = JsonConvert.DeserializeObject<State_voucher>(SendFE(JsonRequest));
                            voucher.Resultado = Response.Resultado;
                            InsertarLogFE(voucher);
                            if (Response.Resultado == "ok") { break; }
                        }
                        catch (Exception ex)
                        {
                            voucher.Resultado = ex.Message;
                            InsertarLogFE(voucher);
                        }
                    }

                }
            }
        }


        public async void InsertarLogFE(State_voucher voucher)
        {
            voucher.status = (voucher.Resultado == "ok" ? "1" : "0");
            voucher.Application = "CONCILIACIONES";
            Boolean RptLog = await _cobranzasRepository.Insertar_Respuesta_FE(voucher);
        }

        private string SendFE(string Json_request)
        {
            string response;


            var Url = _ldapSettings.UrlFE;
            byte[] data = UTF8Encoding.UTF8.GetBytes(Json_request);

            HttpWebRequest request;
            request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/json; charset=utf-8";
            //request.Headers.Add("Usuario", user);
            //request.Headers.Add("Clave", password);
            request.Proxy = new WebProxy() { UseDefaultCredentials = true };

            Stream postTorrente = request.GetRequestStream();
            postTorrente.Write(data, 0, data.Length);
            postTorrente.Close();


            HttpWebResponse respuesta = request.GetResponse() as HttpWebResponse;
            StreamReader read = new StreamReader(respuesta.GetResponseStream(), Encoding.UTF8);
            response = read.ReadToEnd();
            return response;


        }


        private int ObtenerSegmento(int lineaactual, int totallineas, int idbanco)
        {
            int segmento = 0;
            switch (idbanco)
            {
                case 1:
                    segmento = lineaactual == 1 ? 1 : 2;
                    break;
                case 2:
                    segmento = lineaactual == 1 ? 1 : ((lineaactual < totallineas - 1) ? 2 : 3);
                    break;
                case 3:
                    segmento = 1;
                    break;
            }
            return segmento;
        }
        private string ObtenerIdProceso(int idbanco, int idproducto, string usercode)//ConciliacionDto conciliacionDto) //ListadoConciliacionDto
        {
            string fecha = DateTime.Now.ToString("ddMMyyyy");
            string hora = DateTime.Now.ToString("HHmmssms");
            return string.Format("{0}{1}{2}{3}{4}", usercode, idbanco.ToString(), idproducto.ToString(), fecha, hora);

        }
        private ListadoConciliacionDto ObtenerListadoConciliacion(ConciliacionDto conciliacionDto)
        {

            return new ListadoConciliacionDto()
            {
                IsValido = conciliacionDto.IsValido,
                Mensaje = conciliacionDto.Mensaje,
                IdProceso = conciliacionDto.IdProceso,
                IdBanco = conciliacionDto.IdBanco.ToString(),
                TipoOperacion = "GP",
                IdProducto = conciliacionDto.IdProducto.ToString(),
                NumeroRecibo = FormatoNumeroRecibo(conciliacionDto.NumeroRecibo, conciliacionDto.IdBanco),
                Importe = FormatoImporte(conciliacionDto.Importe),
                IdCuentaBanco = conciliacionDto.NumeroCuenta == string.Empty ? this.NumeroCuenta : conciliacionDto.NumeroCuenta,
                NombreCliente = conciliacionDto.NombreCliente,
                DocumentoCliente = conciliacionDto.NumeroDocuento.ToString(),
                FechaVencimiento = FormatoFecha(conciliacionDto.FechaVencimiento),
                FechaCargaArchivo = DateTime.Now.ToString("dd/MM/yyyy"),
                FechaOperacion = FormatoFecha(conciliacionDto.FechaPago),
                NumeroOperacion = conciliacionDto.NumeroOperacion,
                Referencia = conciliacionDto.Referencia,
                IdMoneda = conciliacionDto.IdMoneda == string.Empty ? this.IdMoneda : conciliacionDto.IdMoneda,
                //FlagExtorno = int.Parse(conciliacionDto.FlagExtorno)==2 || this.FlagExtorno == 2 ? 2 :  1
                FlagExtorno = int.Parse(conciliacionDto.FlagExtorno) == 2 ? 2 : 1,
                UserCode = conciliacionDto.UserCode
            };


        }
        private ListadoConciliacionDto ObtenerListadoConciliacionManual(ConciliacionDto conciliacionDto)
        {
            var retornar = new ListadoConciliacionDto();


            retornar.IsValido = conciliacionDto.IsValido;
            retornar.Mensaje = conciliacionDto.Mensaje;
            retornar.IdProceso = conciliacionDto.IdProceso;
            retornar.IdBanco = conciliacionDto.IdBanco.ToString();
            retornar.TipoOperacion = "GP";
            retornar.IdProducto = conciliacionDto.IdProducto.ToString();
            retornar.NumeroRecibo = FormatoNumeroRecibo(conciliacionDto.NumeroRecibo, conciliacionDto.IdBanco);
            retornar.Importe = conciliacionDto.Importe;
            retornar.IdCuentaBanco = conciliacionDto.NumeroCuenta == string.Empty ? this.NumeroCuenta : conciliacionDto.NumeroCuenta;
            retornar.NombreCliente = conciliacionDto.NombreCliente;
            retornar.DocumentoCliente = conciliacionDto.NumeroDocuento.ToString();
            retornar.FechaVencimiento = (conciliacionDto.FechaVencimiento);
            retornar.FechaCargaArchivo = DateTime.Now.ToString("dd/MM/yyyy");
            retornar.FechaOperacion = conciliacionDto.FechaPago;
            retornar.IdMoneda = conciliacionDto.IdMoneda == string.Empty ? this.IdMoneda : conciliacionDto.IdMoneda;
            //FlagExtorno = int.Parse(conciliacionDto.FlagExtorno)==2 || this.FlagExtorno == 2 ? 2 :  1
            retornar.FlagExtorno = 2;
            retornar.UserCode = conciliacionDto.UserCode;
            return retornar;
        }
        private string FormatoNumeroRecibo(string numero, int idBanco)
        {
            string numeroRecibo = string.Empty;
            string numeroplanilla = string.Empty;
            if (!string.IsNullOrEmpty(numero))
            {
                numero = numero.Trim();
                numeroplanilla = numero.Substring(0, 2);


                switch (idBanco)
                {
                    case 1:
                        if (numeroplanilla == "PL")
                        {
                            numeroRecibo = numero;
                        }
                        else
                            numeroRecibo = numero.Substring(numero.IndexOf('0'), 9);
                        break;
                    case 2:
                        numeroRecibo = numero.Substring(numero.Length - 18, 18);
                        break;
                    case 3:
                        numeroRecibo = numero;
                        break;
                }

            }
            return (idBanco == 2) ? numeroRecibo : numero;
        }
        private string FormatoImporte(string numero)
        {
            string importe = string.Empty;
            int entero = 0;
            string parteEntera, parteDecimal;
            if (!string.IsNullOrEmpty(numero))
            {
                entero = int.Parse(numero.Trim());
                //this.FlagExtorno = entero > 0 ? 2 : 1;
                parteEntera = entero.ToString().Substring(0, entero.ToString().Length - 2);
                parteDecimal = entero.ToString().Substring(entero.ToString().Length - 2, 2);
                importe = string.Format("{0}.{1}", parteEntera, parteDecimal);
            }
            return importe;
        }
        private string FormatoFecha(string fecha)
        {
            if (!string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            }
            return fecha;
        }

        private string Base64StringEncode(string ruta)
        {

            // var rt = "";
            //var credential = new SimpleImpersonation.UserCredentials( _ldapSettings.Dominio, _ldapSettings.usernameServidor, _ldapSettings.PasswordServidor);
            //SimpleImpersonation.Impersonation.RunAsUser(credential, SimpleImpersonation.LogonType.NewCredentials, ()=> {
            // byte[] AsBytes = File.ReadAllBytes(ruta);
            //rt = Convert.ToBase64String(AsBytes);
            //}); 

            //using (var impersonation = new ImpersonateUser(_ldapSettings.usernameServidor, _ldapSettings.Dominio, _ldapSettings.PasswordServidor))
            //{
            //    WindowsIdentity.RunImpersonated(impersonation.Identity.AccessToken, () =>
            //    {
            //        WindowsIdentity useri = WindowsIdentity.GetCurrent();
            //        
            //    });
            //}
            // return rt;

            var rt = "";
            bool isIP = false;
            string dominio = "";
            IPAddress ip;
            string[] _url = ruta.Split(@"\");
            for (int i = 0; i < _url.Length; i++)
            {
                isIP = IPAddress.TryParse(_url[i].ToString(), out ip);
                if (isIP)
                {
                    dominio = _url[i].ToString();
                    break;
                }
            }
            using (var impersonation = new ImpersonateUser(_ldapSettings.usernameServidor, dominio, _ldapSettings.PasswordServidor))
            {
                WindowsIdentity.RunImpersonated(impersonation.Identity.AccessToken, () =>
                {
                    WindowsIdentity useri = WindowsIdentity.GetCurrent();
                    byte[] AsBytes = File.ReadAllBytes(ruta);
                    rt = Convert.ToBase64String(AsBytes);
                });
            }
            return rt;
        }

        public async Task<Models.ResponseControl> Validar_Planilla_FacturaAsync(ListadoConciliacionDto Listado)
        {
            return _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.Validar_Planilla_Factura(_mapper.Map<ListaConciliacion>(Listado)));
        }

        public async Task<Models.ResponseControl> ObtenerFormaPago(int idbanco, string idproceso)
        {

            try
            {
                var response = _mapper.Map<Models.ResponseControl>((await _cobranzasRepository.ObtenerFormaPago(idbanco, idproceso)));
                if (response.Code == "0")
                {
                    List<ListaConciliacion> ListFormaPago = (List<ListaConciliacion>)response.Data;
                }
                return response;
            }
            catch (Exception ex)
            {
                return new Models.ResponseControl
                {
                    Code = "1",
                    message = ex.Message
                };
            }
        }

        public async Task<IEnumerable<CuentaDto>> ListarCuenta(int Idbanco)
        {
            IEnumerable<CuentaDto> CuentaDtos = null;

            try
            {
                var CuentaResult = await _cobranzasRepository.ListarCuenta(Idbanco);
                if (CuentaResult == null) return null;
                CuentaDtos = _mapper.Map<IEnumerable<CuentaDto>>(CuentaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return CuentaDtos;
        }

        public async Task<IEnumerable<Tipo_PagoDto>> ListarTipoPago()
        {
            IEnumerable<Tipo_PagoDto> TipoPagoDto = null;

            try
            {
                var TipoPagoResult = await _cobranzasRepository.ListarTipoPago();
                if (TipoPagoResult == null) return null;
                TipoPagoDto = _mapper.Map<IEnumerable<Tipo_PagoDto>>(TipoPagoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return TipoPagoDto;
        }
        public async Task<Models.ResponseControl> ProcessDelCargaDeposito(IFormFile file, int idArchivo, int IdTransc)
        {
            Models.ResponseControl response = new Models.ResponseControl();
            Loadtransaction loadTran = new Loadtransaction() { idArchivo = idArchivo, IdLoadTransaction = IdTransc };
            response = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.DeleteCargaDeposito(loadTran));
            if (response.Code == "0")
            {
                var memoryStream = new MemoryStream();

                await file.CopyToAsync(memoryStream);

                var route = _ldapSettings.RutaFTP + file.FileName;
                using (var fs = new FileStream(route, FileMode.Create, FileAccess.Write))
                    memoryStream.Close();
                memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                using (var fs = new FileStream(route, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }

                if (IdTransc == 2)
                {
                    loadTran.FileName = file.FileName;

                    response = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertDepositosArchivoVisa(loadTran));

                    Console.WriteLine("TranscResponse " + response.Code);
                    Console.WriteLine("Mensaje del if - 1" + response.message);
                    Console.WriteLine("valor del code - 1" + response.Code);

                    if (response.Code == "0")

                    {
                        response.Code = "0";
                        response.message = "Se inserto corectamenteeee";
                    }
                    else
                    {
                        response.message = "No se realizo la insercion del deposito";
                    }
                }
                if (IdTransc == 3)
                {
                    loadTran.FileName = file.FileName;

                    response = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertDepositosArchivoPE(loadTran));

                    //if (response.Code == "0")
                    //{
                    //    response.Code = "0";
                    //    response.message = "Se inserto corectamente";  //codigo d earchivo
                    //    Console.WriteLine("Mensaje del if - 2" + response.message);
                    //    Console.WriteLine("valor del code - 2" + response.Code);
                    //}
                    //else
                    //{
                    //    response.message = "No se realizo la insercion del deposito";
                    //}
                }
            }

            return response;
        }

        private dynamic GetConfigProcess(int idTransac)
        {
            dynamic getValuesFile = new Newtonsoft.Json.Linq.JObject();

            switch (idTransac)
            {
                case 1:
                    getValuesFile.NumFila = 6;
                    getValuesFile.NumColumnCodigo = 6;
                    getValuesFile.NumColumnFecha = 0;
                    getValuesFile.NumColumnMonto = 3;
                    break;
                //case 2:
                //    getValuesFile.NumFila = 4;
                //    getValuesFile.NumColumnCodigo = 22;
                //    getValuesFile.NumColumnFecha = 21;
                //    getValuesFile.NumColumnMonto = 14;
                //    break;
                //case 3:
                //    getValuesFile.NumFila = 1;
                //    getValuesFile.NumColumnCodigo = 4;
                //    getValuesFile.NumColumnFecha = 8;
                //    getValuesFile.NumColumnMonto = 7;
                //    break;

                //PE NUEVO
                case 3:
                    getValuesFile.NumFila = 1;
                    getValuesFile.NumColumnCip = 3;
                    getValuesFile.NumColumnComision_total = 6;
                    getValuesFile.NumColumnSuma_depositada = 7;
                    getValuesFile.NumColumnFecha_operacion = 8;
                    getValuesFile.NumColumnFecha_deposito = 9;
                    getValuesFile.NumColumnFecha_creacion_cip = 12;
                    getValuesFile.NumColumnBanco_pagador = 13;
                    break;
                case 4:
                    getValuesFile.NumFila = 2;
                    getValuesFile.NumColumnCodigo = 10;
                    getValuesFile.NumColumnFecha = 22;
                    getValuesFile.NumColumnMonto = 16;
                    break;
                case 5:
                    getValuesFile.NumFila = 2;
                    getValuesFile.NumColumnCodigo = 10;
                    getValuesFile.NumColumnFecha = 23;
                    getValuesFile.NumColumnMonto = 16;
                    break;
                //VISA NUEVO
                case 2:
                    getValuesFile.NumFila = 9;
                    getValuesFile.NumColumnRuc = 0;
                    getValuesFile.NumColumnRazon_social = 1;
                    getValuesFile.NumColumnCod_comercio = 2;
                    getValuesFile.NumColumnNombre_comercial = 3;
                    getValuesFile.NumColumnFecha_operacion = 4;
                    getValuesFile.NumColumnFecha_deposito = 5;
                    getValuesFile.NumColumnProducto = 6;
                    getValuesFile.NumColumnTipo_operacion = 7;
                    getValuesFile.NumColumnTarjeta = 8;
                    getValuesFile.NumColumnOrigen_tarjeta = 9;
                    getValuesFile.NumColumnTipo_tarjeta = 10;
                    getValuesFile.NumColumnMarco_tarjeta = 11;
                    getValuesFile.NumColumnMoneda = 12;
                    getValuesFile.NumColumnImporte_operacion = 13;
                    getValuesFile.NumColumnDcc = 14;
                    getValuesFile.NumColumnMonto_dcc = 15;
                    getValuesFile.NumColumnComision_total = 16;
                    getValuesFile.NumColumnComision_niubiz = 17;
                    getValuesFile.NumColumnIgv = 18;
                    getValuesFile.NumColumnSuma_depositada = 19;
                    getValuesFile.NumColumnEstado = 20;
                    getValuesFile.NumColumnId_operacion = 21;
                    getValuesFile.NumColumnCuenta_banco_pagador = 22;
                    getValuesFile.NumColumnBanco_pagador = 23;
                    break;
            }

            return getValuesFile;
        }

        private DataTable ConvertToDataTable(IFormFile file, int numberOfColumns, char split)
        {
            DataTable tbl = new DataTable();

            for (int col = 0; col < numberOfColumns; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));


            List<string> listLine = new List<string>();


            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listLine.Add(line);
                }
            }
            foreach (string line in listLine)
            {
                var cols = line.Split(split);

                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < numberOfColumns; cIndex++)
                {
                    dr[cIndex] = cols[cIndex];
                }

                tbl.Rows.Add(dr);
            }

            return tbl;
        }


        private DataTable ConvertToDataTableR(IFormFile file, int numberOfColumns, char split)
        {
            DataTable tbl = new DataTable();

            for (int col = 0; col < numberOfColumns; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));


            List<string> listLine = new List<string>();

            return tbl;
        }
        public async Task<Models.ResponseControl> ProcessConciliaciones(IFormFile file, int IdTransc)
        {

            Models.ResponseControl response = new Models.ResponseControl();

            _ManageFile work = new ManageExcel();

            var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            Stream FileRead = memoryStream;

            DataSet data = null;

            try
            {
                if (IdTransc == 2)
                {
                    //data = await work.Process(FileRead, file.FileName);
                    //METODO
                    //var resp = ProcessConciliacionesVisa(file, IdTransc);
                    data = await work.Process(FileRead, file.FileName);
                }
                else if (IdTransc == 1)

                //if ((new int[] { 1, 2 }).Contains(IdTransc))
                {
                    data = await work.Process(FileRead, file.FileName);
                }
                else if (IdTransc == 3)
                {
                    data = new DataSet();
                    data.Tables.Add(this.ConvertToDataTable(file, 14, ','));
                    //data.Tables.Add(this.ConvertToDataTable(file, 12, ','));

                }
                else
                {
                    data = new DataSet();
                    data.Tables.Add(this.ConvertToDataTable(file, 24, ';'));
                }

                if (data != null && data.Tables.Count > 0)
                {

                    if (IdTransc != 2 && IdTransc != 3) // Para todos menos VISA NI PE DESCOMENTAR LUEGO

                    //if (IdTransc != 3)
                    {
                        if (data != null && data.Tables.Count > 0)
                        {
                            List<DepositTransaccion> Listdeposit = new List<DepositTransaccion>();
                            DataTable table = data.Tables[0];
                            int indice = 1;

                            dynamic getValuesFile = this.GetConfigProcess(IdTransc);

                            foreach (DataRow row in table.Rows)
                            {
                                if (getValuesFile.NumFila <= indice)
                                {
                                    DepositTransaccion deposit = new DepositTransaccion();
                                    deposit.numOperaccion = (row[string.Format("Column{0}", getValuesFile.NumColumnCodigo)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnCodigo)].ToString() : string.Empty);
                                    deposit.FechaOperacion = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha)].ToString() : string.Empty);
                                    deposit.monto = (row[string.Format("Column{0}", getValuesFile.NumColumnMonto)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnMonto)].ToString() : string.Empty);
                                    deposit.usuario = string.Empty;
                                    Listdeposit.Add(deposit);
                                }
                                indice++;
                            }

                            Loadtransaction loadTran = new Loadtransaction() { FileName = file.FileName, IdLoadTransaction = IdTransc };

                            Models.ResponseControl TranscResponse = null;

                            TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.DeleteDepositosArchivo(loadTran));
                            if (TranscResponse.Code == "0")
                            {
                                foreach (DepositTransaccion deposit in Listdeposit)
                                {
                                    TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertarDepositosArchivo(deposit, loadTran));
                                }
                            }

                            TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.ValidarDepositoArchivo(loadTran));

                            if (TranscResponse.Code == "0")
                            {
                                memoryStream.Close();
                                memoryStream = new MemoryStream();
                                await file.CopyToAsync(memoryStream);

                                var route = _ldapSettings.RutaFTP + file.FileName;

                                using (var fs = new FileStream(route, FileMode.Create, FileAccess.Write))
                                {
                                    memoryStream.WriteTo(fs);
                                }
                            }
                            response = TranscResponse;
                        }

                        else
                        {
                            response.Code = "1";
                            response.message = "No se encontro información";
                        }
                    }

                    else
                    {
                        if (data != null && data.Tables.Count > 0)
                        {
                            List<DepositTransaccionVisa> Listdeposit = new List<DepositTransaccionVisa>();
                            DataTable table = data.Tables[0];
                            int indice = 1;

                            dynamic getValuesFile = this.GetConfigProcess(IdTransc);

                            if (IdTransc == 2)
                            {

                                foreach (DataRow row in table.Rows)
                                {
                                    if (getValuesFile.NumFila <= indice)
                                    {
                                        DepositTransaccionVisa deposit = new DepositTransaccionVisa();
                                        deposit.ruc = (row[string.Format("Column{0}", getValuesFile.NumColumnRuc)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnRuc)].ToString() : string.Empty);
                                        deposit.razon_social = (row[string.Format("Column{0}", getValuesFile.NumColumnRazon_social)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnRazon_social)].ToString() : string.Empty);
                                        deposit.cod_comercio = (row[string.Format("Column{0}", getValuesFile.NumColumnCod_comercio)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnCod_comercio)].ToString() : string.Empty);
                                        deposit.nombre_comercial = (row[string.Format("Column{0}", getValuesFile.NumColumnNombre_comercial)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnNombre_comercial)].ToString() : string.Empty);
                                        deposit.fecha_operacion = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha_operacion)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha_operacion)].ToString() : string.Empty);
                                        deposit.fecha_deposito = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha_deposito)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha_deposito)].ToString() : string.Empty);
                                        deposit.producto = (row[string.Format("Column{0}", getValuesFile.NumColumnProducto)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnProducto)].ToString() : string.Empty);
                                        deposit.tipo_operacion = (row[string.Format("Column{0}", getValuesFile.NumColumnTipo_operacion)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnTipo_operacion)].ToString() : string.Empty);
                                        deposit.tarjeta = (row[string.Format("Column{0}", getValuesFile.NumColumnTarjeta)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnTarjeta)].ToString() : string.Empty);
                                        deposit.origen_tarjeta = (row[string.Format("Column{0}", getValuesFile.NumColumnOrigen_tarjeta)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnOrigen_tarjeta)].ToString() : string.Empty);
                                        deposit.tipo_tarjeta = (row[string.Format("Column{0}", getValuesFile.NumColumnTipo_tarjeta)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnTipo_tarjeta)].ToString() : string.Empty);
                                        deposit.marco_tarjeta = (row[string.Format("Column{0}", getValuesFile.NumColumnMarco_tarjeta)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnMarco_tarjeta)].ToString() : string.Empty);
                                        deposit.moneda = (row[string.Format("Column{0}", getValuesFile.NumColumnMoneda)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnMoneda)].ToString() : string.Empty);
                                        deposit.importe_operacion = (row[string.Format("Column{0}", getValuesFile.NumColumnImporte_operacion)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnImporte_operacion)].ToString() : string.Empty);
                                        deposit.dcc = (row[string.Format("Column{0}", getValuesFile.NumColumnDcc)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnDcc)].ToString() : string.Empty);
                                        deposit.monto_dcc = (row[string.Format("Column{0}", getValuesFile.NumColumnMonto_dcc)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnMonto_dcc)].ToString() : string.Empty);
                                        deposit.comision_total = (row[string.Format("Column{0}", getValuesFile.NumColumnComision_total)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnComision_total)].ToString() : string.Empty);
                                        deposit.comision_niubiz = (row[string.Format("Column{0}", getValuesFile.NumColumnComision_niubiz)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnComision_niubiz)].ToString() : string.Empty);
                                        deposit.igv = (row[string.Format("Column{0}", getValuesFile.NumColumnIgv)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnIgv)].ToString() : string.Empty);
                                        deposit.suma_depositada = (row[string.Format("Column{0}", getValuesFile.NumColumnSuma_depositada)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnSuma_depositada)].ToString() : string.Empty);
                                        deposit.estado = (row[string.Format("Column{0}", getValuesFile.NumColumnEstado)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnEstado)].ToString() : string.Empty);
                                        deposit.id_operacion = (row[string.Format("Column{0}", getValuesFile.NumColumnId_operacion)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnId_operacion)].ToString() : string.Empty);
                                        deposit.cuenta_banco_pagador = (row[string.Format("Column{0}", getValuesFile.NumColumnCuenta_banco_pagador)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnCuenta_banco_pagador)].ToString() : string.Empty);
                                        deposit.banco_pagador = (row[string.Format("Column{0}", getValuesFile.NumColumnBanco_pagador)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnBanco_pagador)].ToString() : string.Empty);
                                        Listdeposit.Add(deposit);
                                    }  
                                    indice++;
                                }

                                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                                objRespuesta["code"] = "0";
                                bool bolRespuesta = false;
                                foreach (DepositTransaccionVisa deposit in Listdeposit)
                                {
                                    if (!bolRespuesta && (deposit.fecha_operacion == null || (deposit.fecha_operacion + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna Fecha y Hora de Operación";
                                    }

                                    else if (!bolRespuesta && (deposit.suma_depositada == null || (deposit.suma_depositada + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna Suma Depositada";
                                    }

                                    else if (!bolRespuesta && (deposit.id_operacion == null || (deposit.suma_depositada + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna ID Operación";
                                    }

                                    else if (!bolRespuesta && (deposit.banco_pagador == null || (deposit.banco_pagador + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna Banco Pagador";
                                    }

                                    else if (!bolRespuesta && (deposit.fecha_deposito == null || (deposit.fecha_deposito + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna Fecha de depósito";
                                    }
                                    else if (!bolRespuesta && (deposit.importe_operacion == null || (deposit.importe_operacion + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna Importe de Operación";
                                    }
                                }

                                if (objRespuesta["code"] == "3")
                                {
                                    response.Code = "1";
                                    response.message = objRespuesta["message"];
                                    return response;
                                }
                            }

                            if (IdTransc == 3)
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    if (getValuesFile.NumFila <= indice)
                                    {
                                        DepositTransaccionVisa deposit = new DepositTransaccionVisa();
                                        deposit.cip = (row[string.Format("Column{0}", getValuesFile.NumColumnCip)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnCip)].ToString() : string.Empty);
                                        deposit.comision_total = (row[string.Format("Column{0}", getValuesFile.NumColumnComision_total)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnComision_total)].ToString() : string.Empty);
                                        deposit.suma_depositada = (row[string.Format("Column{0}", getValuesFile.NumColumnSuma_depositada)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnSuma_depositada)].ToString() : string.Empty);
                                        deposit.fecha_operacion = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha_operacion)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha_operacion)].ToString() : string.Empty);
                                        deposit.fecha_deposito = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha_deposito)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha_deposito)].ToString() : string.Empty);
                                        deposit.fecha_creacion_cip = (row[string.Format("Column{0}", getValuesFile.NumColumnFecha_creacion_cip)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnFecha_creacion_cip)].ToString() : string.Empty);
                                        deposit.banco_pagador = (row[string.Format("Column{0}", getValuesFile.NumColumnBanco_pagador)] != null ? row[string.Format("Column{0}", getValuesFile.NumColumnBanco_pagador)].ToString() : string.Empty);
                                        deposit.usuario = string.Empty;
                                        Listdeposit.Add(deposit);
                                    }
                                    indice++;
                                }

                                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                                objRespuesta["code"] = "0";
                                bool bolRespuesta = false;
                                foreach (DepositTransaccionVisa deposit in Listdeposit)
                                {
                                    if (!bolRespuesta && (deposit.cip == null || (deposit.cip + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna N°3, que pertenece a columna CIP";
                                    }

                                    else if (!bolRespuesta && (deposit.suma_depositada == null || (deposit.suma_depositada + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna N°7, que pertenece a columna suma depositada";
                                    }

                                    else if (!bolRespuesta && (deposit.fecha_deposito == null || (deposit.fecha_deposito + " ").Trim() == ""))
                                    {
                                        bolRespuesta = true;
                                        objRespuesta["code"] = "3";
                                        objRespuesta["message"] = "Hay datos vacios en la columna N°9, que pertenece a columna Fecha deposito";
                                    }
                                }

                                if (objRespuesta["code"] == "3")
                                {
                                    response.Code = "1";
                                    response.message = objRespuesta["message"];
                                    return response;
                                }
                            }

                            Loadtransaction loadTran = new Loadtransaction() { FileName = file.FileName, IdLoadTransaction = IdTransc };

                            Models.ResponseControl TranscResponse = null;

                            TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.DeleteDepositosArchivo(loadTran));

                            if (TranscResponse.Code == "0")
                            {
                                foreach (DepositTransaccionVisa deposit in Listdeposit)
                                {
                                    TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertarDepositosArchivoByVisa(deposit, loadTran));
                                }
                            }

                            TranscResponse = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.ValidarDepositoArchivo(loadTran));
                            if (TranscResponse.Code == "0")
                            {
                                memoryStream.Close();
                                memoryStream = new MemoryStream();
                                await file.CopyToAsync(memoryStream);

                                var route = _ldapSettings.RutaFTP + file.FileName;
                                if (IdTransc != 2 && IdTransc != 3)
                                {
                                    using (var fs = new FileStream(route, FileMode.Create, FileAccess.Write))
                                    {
                                        memoryStream.WriteTo(fs);
                                    }
                                }
                                if (IdTransc == 2)
                                {
                                    response = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertDepositosArchivoVisa(loadTran));
                                    Console.WriteLine("TranscResponse " + response.Code);
                                    Console.WriteLine("Mensaje del if - 1" + response.message);
                                    Console.WriteLine("valor del code - 1" + response.Code);

                                    if (response.Code == "0")
                                    {
                                        response.Code = "0";
                                        response.message = "Se inserto corectamente";  //codigo d earchivo
                                        Console.WriteLine("Mensaje del if - 2" + response.message);
                                        Console.WriteLine("valor del code - 2" + response.Code);
                                    }
                                    else
                                    {
                                        response.message = "No se realizo la insercion del deposito";
                                    }
                                }
                                if (IdTransc == 3)
                                {
                                    response = _mapper.Map<Models.ResponseControl>(await _cobranzasRepository.InsertDepositosArchivoPE(loadTran));

                                    //if (response.Code == "0")
                                    //{
                                    //    response.Code = "0";
                                    //    response.message = "Se inserto corectamente";  //codigo d earchivo
                                    //    Console.WriteLine("Mensaje del if - 2" + response.message);
                                    //    Console.WriteLine("valor del code - 2" + response.Code);
                                    //}
                                    //else
                                    //{
                                    //    response.message = "No se realizo la insercion del deposito";
                                    //}
                                }

                            }
                            response = TranscResponse;
                        }

                        else
                        {
                            response.Code = "1";
                            response.message = "No se encontro información";
                        }
                    }

                }
                else
                {
                    response.Code = "1";
                    response.message = "No se encontro información";
                }

            }
            catch (Exception ex)
            {
                response.Code = "1";
                response.message = ex.Message;
            }
            return response;
        }

        // NUEVOS listar PAY
        public async Task<IEnumerable<RamoPayDto>> ListarRamoPay()
        {
            IEnumerable<RamoPayDto> RamoPayDtos = null;

            try
            {
                var ramoPayResult = await _cobranzasRepository.ListarRamoPay();
                if (ramoPayResult == null) return null;
                RamoPayDtos = _mapper.Map<IEnumerable<RamoPayDto>>(ramoPayResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return RamoPayDtos;
        }
        public async Task<IEnumerable<EstadoReciboDto>> ListarEstadoRecibo()
        {
            IEnumerable<EstadoReciboDto> EstadoReciboDtos = null;

            try
            {
                var estadoReciboResult = await _cobranzasRepository.ListarEstadoRecibo();
                if (estadoReciboResult == null) return null;
                EstadoReciboDtos = _mapper.Map<IEnumerable<EstadoReciboDto>>(estadoReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return EstadoReciboDtos;
        }

        public async Task<IEnumerable<EstadoEnvioDto>> ListarEstadoEnvioCE()
        {
            IEnumerable<EstadoEnvioDto> EstadoEnvioDtos = null;

            try
            {
                var estadoEnvioResult = await _cobranzasRepository.ListarEstadoEnvioCE();
                if (estadoEnvioResult == null) return null;
                EstadoEnvioDtos = _mapper.Map<IEnumerable<EstadoEnvioDto>>(estadoEnvioResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return EstadoEnvioDtos;
        }

        public async Task<IEnumerable<DatosRespuestaProductoPayDto>> ListarProductoPay(DatosConsultaProductoPayDto datosConsultaProductoPayDto)
        {
            IEnumerable<DatosRespuestaProductoPayDto> datosRespuestaProductoPayDto = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaProductoPay>(datosConsultaProductoPayDto);

                var ProductoPayResult = await _cobranzasRepository.ListarProductoPay(datosConsultaEntity);

                if (ProductoPayResult == null)
                    return null;

                datosRespuestaProductoPayDto = _mapper.Map<IEnumerable<DatosRespuestaProductoPayDto>>(ProductoPayResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return datosRespuestaProductoPayDto;
        }

        public async Task<ReporteContratantePayDto> ListarContratantePay(DatosConsultaContratantePayDto datosConsultaContratantePayDto)
        {
            ReporteContratantePayDto reporteContratantePayDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaContratantePay>(datosConsultaContratantePayDto);

                var ContratantePayResult = await _cobranzasRepository.ListarContratantePay(datosConsultaEntity);

                if (ContratantePayResult == null)
                    return null;

                reporteContratantePayDtos = _mapper.Map<ReporteContratantePayDto>(ContratantePayResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteContratantePayDtos;
        }

        public async Task<IEnumerable<RamoPayDto>> Estado()
        {
            IEnumerable<RamoPayDto> RamoPayDtos = null;

            try
            {
                var ramoPayResult = await _cobranzasRepository.ListarRamoPay();
                if (ramoPayResult == null) return null;
                RamoPayDtos = _mapper.Map<IEnumerable<RamoPayDto>>(ramoPayResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return RamoPayDtos;
        }


        public async Task<IEnumerable<ReporteReciboPayDto>> ListarReciboPay(DatosConsultaReciboPayDto datosConsultaReciboPay)
        {
            IEnumerable<ReporteReciboPayDto> reporteReciboPayDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReciboPay>(datosConsultaReciboPay);

                var reportesReciboResult = await _cobranzasRepository.ListarReciboPay(datosConsultaEntity);

                if (reportesReciboResult == null)
                    return null;

                reporteReciboPayDtos = _mapper.Map<IEnumerable<ReporteReciboPayDto>>(reportesReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteReciboPayDtos;
        }

        public async Task<IEnumerable<ReporteEnvioPayDto>> ListarEnvioPay(DatosConsultaEnvioPayDto datosConsultaEnvioPay)
        {
            IEnumerable<ReporteEnvioPayDto> reporteEnvioPayDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaEnvioPay>(datosConsultaEnvioPay);

                var reportesEnvioResult = await _cobranzasRepository.ListarEnvioPay(datosConsultaEntity);

                if (reportesEnvioResult == null)
                    return null;

                reporteEnvioPayDtos = _mapper.Map<IEnumerable<ReporteEnvioPayDto>>(reportesEnvioResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteEnvioPayDtos;
        }

        public async Task<IEnumerable<ReporteReciboDetallePayDto>> ListarReciboDetalle(DatosConsultaReciboDetalleDto datosConsultaReciboDetallePay)
        {
            IEnumerable<ReporteReciboDetallePayDto> reporteReciboDetallePayDto = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReciboDetallePay>(datosConsultaReciboDetallePay);

                var reportesResult = await _cobranzasRepository.ListarReciboDetalle(datosConsultaEntity);

                if (reportesResult == null)
                    return null;

                reporteReciboDetallePayDto = _mapper.Map<IEnumerable<ReporteReciboDetallePayDto>>(reportesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteReciboDetallePayDto;
        }
        public async Task<Models.ResponseControl> ActualizarMotivos(DatosReciboActualizarDto datosReciboActualizarDto)
        {
            Models.ResponseControl result = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReciboActualizar>(datosReciboActualizarDto);
                //List<long> _envios = new List<long>();
                //_envios.AddRange(datosConsultaEntity.nrosEnvio);
                long _envios = 0;
                //_envios.AddRange(datosConsultaEntity.nrosEnvio);
                //for (int i = 0; i < _envios.Count; i++)

                //    for (int i = 0; i < _envios; i++)
                //{
                    //var reportesResult = await _cobranzasRepository.ActualizarMotivos(_envios[i], datosConsultaEntity);

                    var reportesResult = await _cobranzasRepository.ActualizarMotivos(_envios, datosConsultaEntity);
                    result = _mapper.Map<Models.ResponseControl>(reportesResult);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return result;
        }
        public async Task<Models.ResponseControl> CobroRecibo(DatosReciboCobroDto datosReciboCobroDto)
        {
            Models.ResponseControl result = null;
            DatosRespuestaServicio datosRespuestaServicio = new DatosRespuestaServicio();
            int nEstado = 0;
            string sEstado = "";
            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReciboCobro>(datosReciboCobroDto);

                Detalles detval = datosConsultaEntity.detalles[0];
                var estadoCierreResult = await _cobranzasRepository.StatusCierre(detval, datosConsultaEntity);
                                                              
            
                if (estadoCierreResult.Code == "0")
                {
                    var reportesCabecera = await _cobranzasRepository.ins_user_cabecera(datosReciboCobroDto.nUserCode);
                    for (int i = 0; i < datosConsultaEntity.detalles.Count; i++)
                    {
                        Detalles det = datosConsultaEntity.detalles[i];
                        try
                        {
                            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://190.216.170.173/apidesarrollo/api/Ecommerce/debitobancario/" + det.nroRecibo);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://servicios.protectasecurity.pe/wsplataformadigitalstg/api/Ecommerce/debitobancario/" + det.nroRecibo);// QA
                            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://servicios.protectasecurity.pe/wsplataformadigital/api/Ecommerce/debitobancario//" + det.nroRecibo); // PROD
                        
                            request.MaximumAutomaticRedirections = 4;
                            request.MaximumResponseHeadersLength = 4;
                            request.Credentials = CredentialCache.DefaultCredentials;
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Stream receiveStream = response.GetResponseStream();
                            //StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                            //Console.WriteLine(readStream.ReadToEnd());
                            //dynamic responseServ = readStream.ReadToEnd();

                            var encoding = ASCIIEncoding.UTF8;
                            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                            {
                                string responseText = reader.ReadToEnd();
                                datosRespuestaServicio = JsonConvert.DeserializeObject<DatosRespuestaServicio>(responseText);
                                if (datosRespuestaServicio.code.ToString() == "DBT000" && datosRespuestaServicio.success)
                                {
                                    nEstado = 1;// cobrado
                                }
                                else if (datosRespuestaServicio.code.ToString() == "DBT004")
                                {
                                    nEstado = 2; // sin cobro
                                }
                                else
                                {
                                    nEstado = 3; // con error
                                }
                            }

                            response.Close();
                            //readStream.Close();
                        }
                        catch (Exception)
                        {
                            nEstado = 3;
                        }

                        var reportesDetalle = await _cobranzasRepository.ins_detalle(det, datosConsultaEntity, nEstado, datosRespuestaServicio, reportesCabecera.Data);

                    }

                    var reportResult = await _cobranzasRepository.ins_cabecera(reportesCabecera.Data);
                    //for (int i = 0; i < _envios.Count; i++)
                    //{
                    //    var reportesResult = await _cobranzasRepository.ActualizarMotivos(_envios[i], datosConsultaEntity);
                    result = _mapper.Map<Models.ResponseControl>(reportResult);
                }
                else
                {
                   //ar reportResult = await _cobranzasRepository.StatusCierre(detval, datosConsultaEntity);

                    //var reportResult = await _cobranzasRepository.ins_cabecera(reportesCabecera.Data);

                    result = _mapper.Map<Models.ResponseControl>(estadoCierreResult);
                }

                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return result;
        }

        public async Task<DatosRespuestaContratanteNCDto> ListarContratanteNC(DatosConsultaContratanteNCDto datosConsultaContratanteNCDto)
        {
            DatosRespuestaContratanteNCDto datosRespuestaContratanteNCDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaContratanteNC>(datosConsultaContratanteNCDto);

                var ContratanteNCResult = await _cobranzasRepository.ListarContratanteNC(datosConsultaEntity);

                if (ContratanteNCResult == null)
                    return null;

                datosRespuestaContratanteNCDtos = _mapper.Map<DatosRespuestaContratanteNCDto>(ContratanteNCResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return datosRespuestaContratanteNCDtos;
        }
        public async Task<IEnumerable<ReporteReciboPendienteNCDto>> ListarReciboPendienteNC(DatosConsultaReciboPendienteNCDto datosConsultaReciboPendienteNCDto)
        {
            IEnumerable<ReporteReciboPendienteNCDto> reporteReciboPendienteNCDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaReciboPendienteNC>(datosConsultaReciboPendienteNCDto);

                var reportesReciboResult = await _cobranzasRepository.ListarReciboPendienteNC(datosConsultaEntity);

                if (reportesReciboResult == null)
                    return null;
                reporteReciboPendienteNCDtos = _mapper.Map<IEnumerable<ReporteReciboPendienteNCDto>>(reportesReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteReciboPendienteNCDtos;
        }
        public async Task<IEnumerable<ReporteFormaPagoDto>> ReporteFormaPago(DatosConsultaFormaPagoDto datosConsultaFormaPagoDto)
        {
            IEnumerable<ReporteFormaPagoDto> reporteFormaPagoDto = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaFormaPago>(datosConsultaFormaPagoDto);

                var reporteFormaPagoResult = await _cobranzasRepository.ReporteFormaPago(datosConsultaEntity);

                if (reporteFormaPagoResult == null)
                    return null;

                reporteFormaPagoDto = _mapper.Map<IEnumerable<ReporteFormaPagoDto>>(reporteFormaPagoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteFormaPagoDto;
        }
        
            public async Task<IEnumerable<TipoFPDto>> ListarTipoFP()
        {
            IEnumerable<TipoFPDto> ListaTipoFPDtos = null;

            try
            {
                var listaTipoFPResult = await _cobranzasRepository.ListarTipoFP();
                if (listaTipoFPResult == null) return null;
                ListaTipoFPDtos = _mapper.Map<IEnumerable<TipoFPDto>>(listaTipoFPResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return ListaTipoFPDtos;
        }

        public async Task<Models.ResponseControl> InsertarRecSelFormaPagoTemp(List<ConsultaInsertListaFPTempDto> listaAgregadaFPTempDTo)
        {
            Models.ResponseControl result = null;
            try
            {
                var request = _mapper.Map<List<ConsultaInsertListaFPTemp>>(listaAgregadaFPTempDTo);

                var reportesResult = await _cobranzasRepository.InsertarRecSelFormaPagoTemp(request);
                result = _mapper.Map<Models.ResponseControl>(reportesResult);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.InnerException.ToString());
            }
            return result;

        }

        public async Task<IEnumerable<ReporteListaFPSelTempDto>> GetListaSelFPTemp(ConsultaListaFPSelTempDto consultaListaFPSelTempDto)
        {
            IEnumerable<ReporteListaFPSelTempDto> responseListaAgregadaFPTempDtos = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<ConsultaListaFPSelTemp>(consultaListaFPSelTempDto);

                var response = await _cobranzasRepository.GetListaSelFPTemp(datosConsultaEntity);

                if (response == null)
                    return null;

                responseListaAgregadaFPTempDtos = _mapper.Map<IEnumerable<ReporteListaFPSelTempDto>>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return responseListaAgregadaFPTempDtos;
        }
        //06/10/2021
        // se continua 15/11/2021
        public async Task<Models.ResponseControl> ProcessGenerarPlanillaFP(PlanillaFPDto planillaFPDto)

        {

            //Models.ResponseControl Rpt = new Models.ResponseControl();
            Models.ResponseControl resultProcessGenerarPlanillaFP = null;

            try
            {
                var request = _mapper.Map<PlanillaFP>(planillaFPDto);
                var result = await _cobranzasRepository.ProcessGenerarPlanillaFP(request);


                //var response = await _cobranzasRepository.ProcessGenerarPlanillaFP(_mapper.Map<PlanillaFP>(planillaFPDto));
                resultProcessGenerarPlanillaFP = _mapper.Map<Models.ResponseControl>(result);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.InnerException.ToString());
            }


            return resultProcessGenerarPlanillaFP;
        }

        public async Task<IEnumerable<ReporteSeguimientoLoteDto>> ReporteSeguimientoLote(DatosConsultaSeguimientoLoteDto datosConsultaSeguimientoLoteDto)
        {
            IEnumerable<ReporteSeguimientoLoteDto> reporteSeguimientoLoteDto = null;

            try
            {
                var datosConsultaEntity = _mapper.Map<DatosConsultaSeguimientoLote>(datosConsultaSeguimientoLoteDto);

                var reportesResult = await _cobranzasRepository.ReporteSeguimientoLote(datosConsultaEntity);

                if (reportesResult == null)
                    return null;

                reporteSeguimientoLoteDto = _mapper.Map<IEnumerable<ReporteSeguimientoLoteDto>>(reportesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return reporteSeguimientoLoteDto;
        }
        //06/10/2021

        //public async Task<Models.ResponseControl> ProcessGenerarPlanillaNC(List<DatosGenerarPlanillaNCDto> datosGenerarPlanillaNCDto)
        //public async Task<Models.ResponseControl> ProcessGenerarPlanillaNC(List<DatosGenerarPlanillaNCDto> datosGenerarPlanillaNCDto)

        //{

        //    //Models.ResponseControl Rpt = new Models.ResponseControl();
        //    //string idproceso = string.Empty, tipooperacion = string.Empty;
        //    //int idproducto, idbanco, usercode;
        //    //var response = await _cobranzasRepository.InsertarPlanilla(_mapper.Map<List<ListaConciliacion>>(listadoConciliacionDtos));
        //    //if (listadoConciliacionDtos.Count > 0 && response)
        //    //{

        //    //    idproceso = listadoConciliacionDtos[0].IdProceso;
        //    //    idproducto = int.Parse(listadoConciliacionDtos[0].IdProducto);
        //    //    idbanco = int.Parse(listadoConciliacionDtos[0].IdBanco);
        //    //    //tipooperacion = listadoConciliacionDtos[0].TipoOperacion;
        //    //    tipooperacion = (listadoConciliacionDtos[0].TipoOperacion == "FP") ? "GP" : listadoConciliacionDtos[0].TipoOperacion;
        //    //    usercode = int.Parse(listadoConciliacionDtos[0].UserCode);
        //    //    Rpt = _mapper.Map<Models.ResponseControl>((await _cobranzasRepository.GeneraPlanillaFactura(idproceso, idproducto, idbanco, tipooperacion, usercode)));

        //    //    if (Rpt.Data != null)
        //    //    {
        //    //        SendVouchers((List<State_voucher>)Rpt.Data);
        //    //    }

        //    //    //Rpt = mapper.Map(Obj, Rpt);
        //    //    // =  (Obj);

        //    //    //cobranzasRepository.GeneraPlanillaFactura(idproceso, idproducto, idbanco, tipooperacion, usercode));
        //    //}
        //    //return Rpt;
        //}

    }

}
