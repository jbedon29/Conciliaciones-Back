using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.CobranzaModule.Aggregates;
using Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.CobranzasModule.Repositories
{
    public class CobranzaRepository : ICobranzaRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public CobranzaRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }


        public Task<List<Banco>> ListarBancos()
        {
            Banco entities = null;
            List<Banco> listaBancos = new List<Banco>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_PAYROLL.PA_SEL_BANK", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Banco();
                    entities.IdBanco = Convert.ToInt32(dr["NIDBANK"]);
                    entities.DescripcionBanco = Convert.ToString(dr["SDESCRIPTION"]);
                    listaBancos.Add(entities);
                }
            }

            return Task.FromResult<List<Banco>>(listaBancos);
        }


        public Task<Conciliacion> ValidarTrama(Trama trama)
        {

            Conciliacion _conciliacion = new Conciliacion();
            string response = string.Empty, valid = string.Empty, mensaje = string.Empty;
            List<OracleParameter> parameters = new List<OracleParameter>();
            try
            {
                parameters.Add(new OracleParameter("P_TRAMA", OracleDbType.Varchar2, 4000, trama.StringTrama, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NFILA", OracleDbType.Varchar2, trama.Fila, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NID_BANK", OracleDbType.Int32, trama.IdBanco, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NSEGMEN", OracleDbType.Int32, trama.Segmento, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_STIPOINGRESO", OracleDbType.Varchar2, trama.TipoIngreso, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, trama.IdProducto, ParameterDirection.Input));
                //Parámetro de Salida
                //var pRecibo = new OracleParameter("P_NRECEIPT", OracleDbType.Varchar2, ParameterDirection.Output)
                //{
                //    Size = 2000
                //};
                var pRecibo = new OracleParameter("P_SPROFORMA", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pImporte = new OracleParameter("P_NPREMIUM", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pImporteOrigen = new OracleParameter("P_NPREMIUM_ORIG", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };

                var pFechaPago = new OracleParameter("P_FECHAPAGO", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pNombreCliente = new OracleParameter("P_NOMCLIENTE", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pFechaVencimiento = new OracleParameter("P_FECHAVENC", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pNumeroCuenta = new OracleParameter("P_NUMCUENTA", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pNumeroDocumento = new OracleParameter("P_NUMDOCUMENTO", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pMoneda = new OracleParameter("P_NIDCURRENCY", OracleDbType.Int32, ParameterDirection.Output);

                var pcantidad = new OracleParameter("P_CANT_REGIST", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var Fechaoperation = new OracleParameter("P_OPERATION_DATE", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var numeroOperation = new OracleParameter("P_OPERATION_NUMBER", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var reference = new OracleParameter("P_REFERENCE", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pmonto = new OracleParameter("P_MONTO_TOTAL", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };
                var pmontoOrigen = new OracleParameter("P_ORIG_TOTAL", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };


                var pExtorno = new OracleParameter("P_EXTORNO", OracleDbType.Varchar2, ParameterDirection.Output)
                {
                    Size = 1
                };
                var pValid = new OracleParameter("P_VALID", OracleDbType.Int32, ParameterDirection.Output);
                var pMensaje = new OracleParameter("P_MENSAJE", OracleDbType.NVarchar2, ParameterDirection.Output)
                {
                    Size = 2000
                };


                parameters.Add(pRecibo);
                parameters.Add(pImporte);
                parameters.Add(pImporteOrigen);
                parameters.Add(pFechaPago);
                parameters.Add(pNombreCliente);
                parameters.Add(pFechaVencimiento);
                parameters.Add(pNumeroCuenta);
                parameters.Add(pNumeroDocumento);
                parameters.Add(pMoneda);
                parameters.Add(Fechaoperation);
                parameters.Add(numeroOperation);
                parameters.Add(reference);
                parameters.Add(pcantidad);
                parameters.Add(pmonto);
                parameters.Add(pmontoOrigen);
                parameters.Add(pExtorno);
                parameters.Add(pValid);
                parameters.Add(pMensaje);

                OracleParameterCollection dr = (OracleParameterCollection)_connectionBase.ExecuteByStoredProcedureNonQuery("PKG_TRAMA_CONFIG.VAL_TRAMA", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);

                _conciliacion.IsValido = dr["P_VALID"].Value == DBNull.Value ? false : int.Parse(dr["P_VALID"].Value.ToString()) == 0 ? false : true;
                _conciliacion.Mensaje = dr["P_MENSAJE"].Value.ToString().Equals("null") ? string.Empty : dr["P_MENSAJE"].Value.ToString();
                _conciliacion.NumeroRecibo = dr["P_SPROFORMA"].Value.ToString().Equals("null") ? string.Empty : dr["P_SPROFORMA"].Value.ToString();
                _conciliacion.Importe = dr["P_NPREMIUM"].Value.ToString().Equals("null") ? string.Empty : dr["P_NPREMIUM"].Value.ToString();
                _conciliacion.NombreCliente = dr["P_NOMCLIENTE"].Value.ToString().Equals("null") ? string.Empty : dr["P_NOMCLIENTE"].Value.ToString();
                _conciliacion.NumeroCuenta = dr["P_NUMCUENTA"].Value.ToString().Equals("null") ? string.Empty : dr["P_NUMCUENTA"].Value.ToString();
                _conciliacion.FechaVencimiento = dr["P_FECHAVENC"].Value.ToString().Equals("null") ? string.Empty : dr["P_FECHAVENC"].Value.ToString();
                _conciliacion.FechaPago = dr["P_FECHAPAGO"].Value.ToString().Equals("null") ? string.Empty : dr["P_FECHAPAGO"].Value.ToString();
                _conciliacion.IdMoneda = dr["P_NIDCURRENCY"].Value.ToString().Equals("null") ? string.Empty : dr["P_NIDCURRENCY"].Value.ToString();
                _conciliacion.FlagExtorno = dr["P_EXTORNO"].Value.ToString().Equals("null") || dr["P_EXTORNO"].Value.ToString().Trim().Equals("") ? "2" : "1";
                _conciliacion.NumeroDocuento = dr["P_NUMDOCUMENTO"].Value.ToString().Equals("null") ? string.Empty : dr["P_NUMDOCUMENTO"].Value.ToString();
                _conciliacion.CantTotal = dr["P_CANT_REGIST"].Value.ToString().Equals("null") ? string.Empty : dr["P_CANT_REGIST"].Value.ToString();
                _conciliacion.MontoTotal = dr["P_MONTO_TOTAL"].Value.ToString().Equals("null") ? string.Empty : dr["P_MONTO_TOTAL"].Value.ToString();
                _conciliacion.NumeroOperacion = dr["P_OPERATION_NUMBER"].Value.ToString().Equals("null") ? string.Empty : dr["P_OPERATION_NUMBER"].Value.ToString();
                _conciliacion.Referencia = dr["P_REFERENCE"].Value.ToString().Equals("null") ? string.Empty : dr["P_REFERENCE"].Value.ToString();
                _conciliacion.FechaOperacion = dr["P_FECHAPAGO"].Value.ToString().Equals("null") ? string.Empty : dr["P_FECHAPAGO"].Value.ToString();
                _conciliacion.ImporteOrigen = dr["P_NPREMIUM_ORIG"].Value.ToString().Equals("null") ? string.Empty : dr["P_NPREMIUM_ORIG"].Value.ToString();
                _conciliacion.MontoTotalOrigen = dr["P_ORIG_TOTAL"].Value.ToString().Equals("null") ? string.Empty : dr["P_ORIG_TOTAL"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return Task.FromResult<Conciliacion>(_conciliacion);
        }
        public Task<Planilla> ObtenerTrama(Trama trama)
        {
            Planilla planilla = new Planilla();
            Proforma proforma;
            planilla.ListaProforma = new List<Proforma>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            CultureInfo cultureInfo = new CultureInfo("es-PE");
            parameters.Add(new OracleParameter("P_NIDBANK", OracleDbType.Int32, trama.IdBanco, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, trama.IdProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_DFECHAINI", OracleDbType.Date, DateTime.Parse(trama.FechaInicial, cultureInfo), ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_DFECHAFIN", OracleDbType.Date, DateTime.Parse(trama.FechaFinal, cultureInfo), ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, trama.CodigoUsuario, ParameterDirection.Input));

            //Parámetro de Salida
            var pValid = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            var pRuta = new OracleParameter("P_SRUTA", OracleDbType.NVarchar2, ParameterDirection.Output);
            pRuta.Size = 2500;


            // var pRuta = new OracleParameter("P_SRUTA", OracleDbType.Varchar2,4000, ParameterDirection.Output);
            var pMensaje = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, 4000, ParameterDirection.Output);
            parameters.Add(pRuta);
            parameters.Add(new OracleParameter("LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            parameters.Add(pValid);
            parameters.Add(pMensaje);

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_TRAMA_ENVIO_BANCO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    proforma = new Proforma();
                    //  proforma.NumeroCuenta = dr["NRECEIPT"] == DBNull.Value ? string.Empty : dr["NRECEIPT"].ToString();
                    proforma.NombreContratante = dr["SCLIENAME"] == DBNull.Value ? string.Empty : dr["SCLIENAME"].ToString();
                    proforma.Documento = dr["SIDDOC"] == DBNull.Value ? string.Empty : dr["SIDDOC"].ToString();
                    proforma.DescTipoDoc = dr["DESC_TIPODOC"] == DBNull.Value ? string.Empty : dr["DESC_TIPODOC"].ToString();
                    proforma.CodigoProforma = dr["SPROFORMA"] == DBNull.Value ? string.Empty : dr["SPROFORMA"].ToString();
                    proforma.NumeroRecibo = dr["NRECEIPT"] == DBNull.Value ? string.Empty : dr["NRECEIPT"].ToString();
                    proforma.FechaEmision = dr["FE_EMISION"] == DBNull.Value ? string.Empty : Convert.ToDateTime(dr["FE_EMISION"].ToString()).ToString("dd/MM/yyyy");
                    proforma.FechaVencimiento = dr["FE_VENCIMIENTO"] == DBNull.Value ? string.Empty : Convert.ToDateTime(dr["FE_VENCIMIENTO"].ToString()).ToString("dd/MM/yyyy");
                    proforma.Importe = dr["NPREMIUM"] == DBNull.Value ? "0.00" : Convert.ToDouble(dr["NPREMIUM"].ToString()).ToString("N2");
                    proforma.Ramo = dr["RAMO"] == DBNull.Value ? string.Empty : dr["RAMO"].ToString();
                    proforma.Producto = dr["PRODUCTO"] == DBNull.Value ? string.Empty : dr["PRODUCTO"].ToString();


                    //  proforma.ImporteMora = dr["MORA"] == DBNull.Value ? "0.00" : Convert.ToDouble(dr["MORA"].ToString()).ToString("N2");
                    //  proforma.ImporteMontoMinimo = dr["MONTO_MINIMO"] == DBNull.Value ? "0.00" : Convert.ToDouble(dr["MONTO_MINIMO"].ToString()).ToString("N2");

                    planilla.ListaProforma.Add(proforma);
                }
                planilla.Resultado = int.Parse(pValid.Value.ToString()) == 0 ? true : false;
                planilla.MensajeError = pMensaje.Value.ToString();
                planilla.RutaTrama = pRuta.Value.ToString();
            }
            return Task.FromResult<Planilla>(planilla);
        }
        public Task<bool> InsertarProceso(List<ListaConciliacion> listaConciliacions)
        {
            CultureInfo cultureInfo = new CultureInfo("es-PE");
            int count = 1;
            bool exito = true;
            try
            {

                foreach (var item in listaConciliacions)
                {
                    if (item.IsValido || item.TipoOperacion != "GP")
                    {
                        List<OracleParameter> parameters = new List<OracleParameter>();
                        parameters.Add(new OracleParameter("P_SIDPROCESS", OracleDbType.Varchar2, item.IdProceso ?? string.Empty, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NIDBANK", OracleDbType.Int32, item.IdBanco, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_STYPEOPE", OracleDbType.Varchar2, item.TipoOperacion ?? string.Empty, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, item.IdProducto, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_SPROFORMA", OracleDbType.Varchar2, item.NumeroRecibo ?? count.ToString(), ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NPREMIUM", OracleDbType.Decimal, item.Importe, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NIDCURRENCY", OracleDbType.Int32, item.IdMoneda, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NAMOUNT", OracleDbType.Decimal, item.MontoFormaPago, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NIDPAIDTYPE", OracleDbType.Int32, item.IdTipoPago, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NIDACCOUNTBANK", OracleDbType.Int64, item.IdCuentaBanco, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("PNPM_SOPERATION_NUMBER", OracleDbType.Varchar2, item.NumeroOperacion ?? string.Empty, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_DOPERATION_DATE", OracleDbType.Date, DateTime.Parse(item.FechaOperacion, cultureInfo), ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_SREFERENCE", OracleDbType.Varchar2, item.Referencia ?? string.Empty, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_SINDEXTORNO", OracleDbType.Varchar2, item.FlagExtorno, ParameterDirection.Input));
                        parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, item.UserCode, ParameterDirection.Input));


                        var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                        var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                        p_Message.Size = 2500;

                        parameters.Add(pCode);
                        parameters.Add(p_Message);
                        _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_GET_TRAMA_INS_PAYROLLBILLS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                        count++;
                        bool flag = pCode.Value.ToString() == "0" ? true : false;
                        if (!flag)
                        {
                            exito = false;
                            throw new Exception(p_Message.Value.ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Task.FromResult<bool>(exito);
        }
        public Task<ResponseControl> GeneraPlanillaFactura(string idproceso, int idproducto, int idbanco, string tipooperacion, int usercode)
        {
            bool exito = true;
            List<OracleParameter> parameters = new List<OracleParameter>();
            ResponseControl Response = new ResponseControl(ResponseControl.Status.Ok);
            List<State_voucher> state_Vouchers = new List<State_voucher>();
            State_voucher voucher;
            try
            {

                parameters.Add(new OracleParameter("P_SIDPROCESS", OracleDbType.Varchar2, idproceso, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Varchar2, idproducto, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NIDBANK", OracleDbType.Varchar2, idbanco, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_STYPEOPE", OracleDbType.Varchar2, tipooperacion, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Varchar2, usercode, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                var table = new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output);
                p_Message.Size = 2500;
                table.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                parameters.Add(table);
                //_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_GENERA_PLANILLA_DOCUMENTO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_GENERA_PLANILLA_DOCUMENTO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        voucher = new State_voucher
                        {
                            SCAMPO = (dr["SCAMPO"] != null ? dr["SCAMPO"].ToString() : ""),
                            SGRUPO = (dr["SGRUPO"] != null ? dr["SGRUPO"].ToString() : ""),
                            SMENSAJE = (dr["SMENSAJE"] != null ? dr["SMENSAJE"].ToString() : ""),
                            SVALOR = (dr["SVALOR"] != null ? dr["SVALOR"].ToString() : ""),
                            BILLTYPE = (dr["SBILLTYPE"] != null ? dr["SBILLTYPE"].ToString() : ""),
                            SBILING = (dr["SBILLING"] != null ? dr["SBILLING"].ToString() : ""),
                            NINSUR_AREA = (dr["NINSUR_AREA"] != null ? dr["NINSUR_AREA"].ToString() : ""),
                            NBILLNUM = (dr["NBILLNUM"] != null ? dr["NBILLNUM"].ToString() : ""),
                            OPERADOR = (dr["SCORREO_OPE"] != null ? dr["SCORREO_OPE"].ToString() : "")
                        };
                        state_Vouchers.Add(voucher);
                    }
                    Response.Data = state_Vouchers;
                    Response.message = p_Message.Value.ToString();
                    if (!(pCode.Value.ToString() == "0"))
                    {
                        Response.Code = pCode.Value.ToString();
                        throw new Exception(p_Message.Value.ToString());

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Task.FromResult<ResponseControl>(Response);
        }
        public Task<ResponseControl> Validar_Planilla_Factura(ListaConciliacion listaConciliacions)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_SIDPROCESS", OracleDbType.Varchar2, listaConciliacions.IdProceso ?? string.Empty, ParameterDirection.Input));
                var nnoBills = new OracleParameter("P_NNOBILLS", OracleDbType.Int16, ParameterDirection.Output);
                parameters.Add(nnoBills);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_VALIDA_PLANILLA", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                response.Data = nnoBills.Value.ToString() == "1" ? true : false;

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.Code = "1";
            }
            return Task.FromResult<ResponseControl>(response);
        }

        public Task<ResponseControl> ObtenerLiquidacionManual(string idproceso, int idproducto, int idbanco, string StrProforma, string fechaInicio, string fechaFin, string usercode)
        {
            bool exito = true;
            CultureInfo cultureInfo = new CultureInfo("es-PE");
            List<OracleParameter> parameters = new List<OracleParameter>();
            ResponseControl Response = new ResponseControl(ResponseControl.Status.Ok);
            List<Conciliacion> ObjListaConciliacion = new List<Conciliacion>();
            State_voucher voucher;
            try
            {

                parameters.Add(new OracleParameter("P_SIDPROCESS", OracleDbType.Varchar2, idproceso, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NIDBANK", OracleDbType.Int32, idbanco, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, idproducto, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_SPROFORMA", OracleDbType.Varchar2, StrProforma, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DFECHAINI", OracleDbType.Date, DateTime.Parse(fechaInicio, cultureInfo), ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DFECHAFIN", OracleDbType.Date, DateTime.Parse(fechaFin, cultureInfo), ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Varchar2, usercode, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                var table = new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output);
                p_Message.Size = 2500;
                table.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                parameters.Add(table);
                //_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_GENERA_PLANILLA_DOCUMENTO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_LIQUIDACION_MANUAL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime
                    ))
                {
                    while (dr.Read())
                    {
                        var _conciliacion = new Conciliacion();
                        _conciliacion.IsValido = true;
                        _conciliacion.IdProceso = idproceso;
                        _conciliacion.IdProducto = idproducto;
                        _conciliacion.NumeroRecibo = dr["SPROFORMA"] == DBNull.Value ? string.Empty : dr["SPROFORMA"].ToString();
                        _conciliacion.Importe = dr["NPREMIUM"] == DBNull.Value ? string.Empty : dr["NPREMIUM"].ToString();
                        _conciliacion.FechaVencimiento = dr["DEXPIRDAT"] == DBNull.Value ? string.Empty : dr["DEXPIRDAT"].ToString().Substring(0, 10);
                        _conciliacion.FechaPago = dr["DCOMPDATE"] == DBNull.Value ? string.Empty : dr["DCOMPDATE"].ToString().Substring(0, 10);
                        _conciliacion.IdMoneda = dr["NCURRENCY"] == DBNull.Value ? string.Empty : dr["NCURRENCY"].ToString();
                        _conciliacion.NumeroDocuento = dr["SIDDOC"] == DBNull.Value ? string.Empty : dr["SIDDOC"].ToString();
                        ObjListaConciliacion.Add(_conciliacion);
                    }
                    Response.Data = ObjListaConciliacion;
                    Response.message = p_Message.Value.ToString();
                    if (!(pCode.Value.ToString() == "0"))
                    {
                        Response.Code = pCode.Value.ToString();
                        throw new Exception(p_Message.Value.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Task.FromResult<ResponseControl>(Response);
        }
        public Task<ResponseControl> ObtenerFormaPago(int idBanco, string idProceso)
        {

            List<OracleParameter> parameters = new List<OracleParameter>();
            ResponseControl Response = new ResponseControl(ResponseControl.Status.Ok);
            List<ListaConciliacion> ListFormaPago = new List<ListaConciliacion>();
            ListaConciliacion conciliacion;
            try
            {

                parameters.Add(new OracleParameter("P_SIDPROCESS", OracleDbType.Varchar2, idProceso, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NIDBANK", OracleDbType.Int16, idBanco, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                var table = new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output);
                p_Message.Size = 2500;
                table.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                parameters.Add(table);
                //_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_GENERA_PLANILLA_DOCUMENTO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_PLANILLA_AUTOMATICA", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        conciliacion = new ListaConciliacion
                        {
                            IdMoneda = (dr["NIDCURRENCY"] != null ? dr["NIDCURRENCY"].ToString() : string.Empty),
                            IdTipoPago = (dr["NIDPAIDTYPE"] != null ? dr["NIDPAIDTYPE"].ToString() : string.Empty),
                            IdCuentaBanco = (dr["NIDACCOUNTBANK"] != null ? dr["NIDACCOUNTBANK"].ToString() : string.Empty),
                            IdBanco = (dr["NIDBANK"] != null ? dr["NIDBANK"].ToString() : string.Empty),
                            NumeroOperacion = (dr["SOPERATION_NUMBER"] != null ? dr["SOPERATION_NUMBER"].ToString() : string.Empty),
                            FechaOperacion = (dr["DOPERATION_DATE"] != null ? dr["DOPERATION_DATE"].ToString() : string.Empty),
                            Referencia = (dr["SREFERENCE"] != null ? dr["SREFERENCE"].ToString() : string.Empty),
                            MontoFormaPago = (dr["NAMOUNT"] != null ? dr["NAMOUNT"].ToString() : string.Empty),
                        };
                        ListFormaPago.Add(conciliacion);
                    }
                    Response.Data = ListFormaPago;
                    Response.message = p_Message.Value.ToString();
                    if (!(pCode.Value.ToString() == "0"))
                    {
                        Response.Code = pCode.Value.ToString();
                        throw new Exception(p_Message.Value.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Task.FromResult<ResponseControl>(Response);
        }

        public Task<List<Cuenta>> ListarCuenta(int idBanco)
        {
            Cuenta entities = null;
            List<Cuenta> ListCuentas = new List<Cuenta>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_IDBANK", OracleDbType.Int16, idBanco, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.PA_SEL_ACCOUNTBANK", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Cuenta();
                    entities.idCuenta = Convert.ToInt32(dr["NIDACCOUNTBANK"]);
                    entities.DescripcionCuenta = Convert.ToString(dr["SDESCRIPT"]);
                    ListCuentas.Add(entities);
                }
            }

            return Task.FromResult<List<Cuenta>>(ListCuentas);
        }




        public Task<bool> Insertar_Respuesta_FE(State_voucher _Voucher)
        {
            CultureInfo cultureInfo = new CultureInfo("es-PE");
            bool exito = true;
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_NINSUR_AREA", OracleDbType.Int64, _Voucher.NINSUR_AREA ?? string.Empty, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_SBILLTYPE", OracleDbType.Varchar2, _Voucher.BILLTYPE, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_SBILLING", OracleDbType.Varchar2, _Voucher.SBILING ?? string.Empty, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NBILLNUM", OracleDbType.Int64, _Voucher.NBILLNUM, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ADDRESS_SUBMIT", OracleDbType.Varchar2, _Voucher.OPERADOR, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_RESPONSE", OracleDbType.Varchar2, _Voucher.Resultado, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_STATUS", OracleDbType.Varchar2, _Voucher.status, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_APPLICATION", OracleDbType.Varchar2, _Voucher.Application, ParameterDirection.Input));


                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 2500;

                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_INS_RESPONSE_FE", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                bool flag = pCode.Value.ToString() == "0" ? true : false;
                if (!flag)
                {
                    exito = false;
                    throw new Exception(p_Message.Value.ToString());

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Task.FromResult<bool>(exito);
        }

        public Task<List<Tipo_Pago>> ListarTipoPago()
        {
            Tipo_Pago entities = null;
            List<Tipo_Pago> ListarTipopago = new List<Tipo_Pago>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_PAYROLL.PA_SEL_TYPE_PAY", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Tipo_Pago();
                    entities.id = Convert.ToInt32(dr["NIDPAY"]);
                    entities.name = Convert.ToString(dr["SDESCRIPTION"]);
                    ListarTipopago.Add(entities);
                }
            }

            return Task.FromResult<List<Tipo_Pago>>(ListarTipopago);
        }



        public Task<ResponseControl> ValidarDepositoArchivo(Loadtransaction _request)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_VC_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VC_COD_TRANS", OracleDbType.Varchar2, _request.IdLoadTransaction, ParameterDirection.Input));
                // parameters.Add(new OracleParameter("P_VC_COD_TRANS", OracleDbType.Int32, _request.IdLoadTransaction, ParameterDirection.Input));


                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_VAL_DEPOSITO_ARCHIVO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }

        public Task<ResponseControl> DeleteDepositosArchivo(Loadtransaction _request)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_VC_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_TIPO_MEDIO_PAGO", OracleDbType.Varchar2, _request.IdLoadTransaction, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_DEL_CARGA_TABLA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }

        /*INSERTA  DEPOSITO EN PASARELA VISA*/
        public Task<ResponseControl> InsertDepositosArchivoVisa(Loadtransaction _request)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_TIPO_TRANSAC", OracleDbType.Varchar2, _request.IdLoadTransaction, ParameterDirection.Input));

                // var pCode = new OracleParameter("P_ID_DEPOSITO_ARCHIVO", OracleDbType.Int16, ParameterDirection.Output);
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);

                //var pArchivo = new OracleParameter("P_ID_DEPOSITO_ARCHIVO", OracleDbType.NVarchar2, ParameterDirection.Output);

                //var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                //parameters.Add(pArchivo);

                _connectionBase.ExecuteByStoredProcedure("SP_PASARELA_VISA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }

        /*INSERTA DEPOSITO PSARELA PE*/
        public Task<ResponseControl> InsertDepositosArchivoPE(Loadtransaction _request)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);

                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);

                _connectionBase.ExecuteByStoredProcedure("SP_PASARELA_PE", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }


        ///*delete nuevo idtran=3*/
        //public Task<ResponseControl> InsertDepositosArchivoPE(Loadtransaction _request)
        //{
        //    ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
        //    try
        //    {
        //        List<OracleParameter> parameters = new List<OracleParameter>();
        //        parameters.Add(new OracleParameter("P_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));
        //        parameters.Add(new OracleParameter("P_TIPO_TRANSAC", OracleDbType.Varchar2, _request.IdLoadTransaction, ParameterDirection.Input));

        //        var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
        //        var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
        //        p_Message.Size = 4000;
        //        parameters.Add(pCode);
        //        parameters.Add(p_Message);
        //        _connectionBase.ExecuteByStoredProcedure("SP_PASARELA_PE", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

        //        response.Code = pCode.Value.ToString();
        //        response.message = p_Message.Value.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        response.message = ex.Message;
        //    }
        //    return Task.FromResult<ResponseControl>(response);
        //}

        public Task<ResponseControl> InsertarDepositosArchivo(DepositTransaccion _request, Loadtransaction _CabRequest)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_VC_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _CabRequest.FileName, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_TIPO_MEDIO_PAGO", OracleDbType.Int32, _CabRequest.IdLoadTransaction, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VC_ID_OPERACION", OracleDbType.Varchar2, _request.numOperaccion, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DT_FECHA_DEPOSITO", OracleDbType.Varchar2, _request.FechaOperacion, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DC_MONTO", OracleDbType.Varchar2, _request.monto, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VC_USUARIO_CREACION", OracleDbType.Varchar2, _request.usuario, ParameterDirection.Input));


                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_INS_CARGA_DEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();


            }


            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }


        public Task<ResponseControl> InsertarDepositosArchivoByVisa(DepositTransaccionVisa _request, Loadtransaction _CabRequest)
        {

            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {

                /*para visa*/
                {
                    List<OracleParameter> parameters = new List<OracleParameter>();
                    parameters.Add(new OracleParameter("P_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _CabRequest.FileName, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_RUC", OracleDbType.Varchar2, _request.ruc, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_RAZON_SOCIAL", OracleDbType.Varchar2, _request.razon_social, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_COD_COMERCIO", OracleDbType.Varchar2, _request.cod_comercio, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_NOMBRE_COMERCIAL", OracleDbType.Varchar2, _request.nombre_comercial, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_FECHA_OPERACION", OracleDbType.Varchar2, _request.fecha_operacion, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_FECHA_DEPOSITO", OracleDbType.Varchar2, _request.fecha_deposito, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_PRODUCTO", OracleDbType.Varchar2, _request.producto, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_TIPO_OPERACION", OracleDbType.Varchar2, _request.tipo_operacion, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_TARJETA", OracleDbType.Varchar2, _request.tarjeta, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_ORIGEN_TARJETA", OracleDbType.Varchar2, _request.origen_tarjeta, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_TIPO_TARJETA", OracleDbType.Varchar2, _request.tipo_tarjeta, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_MARCO_TARJETA", OracleDbType.Varchar2, _request.marco_tarjeta, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_MONEDA", OracleDbType.Varchar2, _request.moneda, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_IMPORTE_OPERACION", OracleDbType.Decimal, Convert.ToDecimal(_request.importe_operacion), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_DCC", OracleDbType.Varchar2, _request.dcc, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_MONTO_DCC", OracleDbType.Decimal, Convert.ToDecimal(_request.monto_dcc), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_COMISION_TOTAL", OracleDbType.Decimal, Convert.ToDecimal(_request.comision_total), ParameterDirection.Input)); //18col
                    parameters.Add(new OracleParameter("P_COMISION_NIUBIZ", OracleDbType.Decimal, Convert.ToDecimal(_request.comision_niubiz), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_IGV", OracleDbType.Decimal, Convert.ToDecimal(_request.igv), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_SUMA_DEPOSITADA", OracleDbType.Decimal, Convert.ToDecimal(_request.suma_depositada), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Varchar2, _request.estado, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_ID_OPERACION", OracleDbType.Varchar2, _request.id_operacion, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_CUENTA_BANCO_PAGADOR", OracleDbType.Varchar2, _request.cuenta_banco_pagador, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_BANCO_PAGADOR", OracleDbType.Varchar2, _request.banco_pagador, ParameterDirection.Input));//25col
                    parameters.Add(new OracleParameter("P_TIPO_TRANSAC", OracleDbType.Varchar2, _CabRequest.IdLoadTransaction, ParameterDirection.Input));

                    // parameters.Add(new OracleParameter("P_TIPO_TRANSAC", OracleDbType.Int32, _request.tipo_transac, ParameterDirection.Input)); //26
                    parameters.Add(new OracleParameter("P_CIP", OracleDbType.Varchar2, _request.cip, ParameterDirection.Input)); //27
                    parameters.Add(new OracleParameter("P_FECHA_CREACION_CIP", OracleDbType.Varchar2, _request.fecha_creacion_cip, ParameterDirection.Input));//28

                    parameters.Add(new OracleParameter("P_VC_USUARIO_CREACION", OracleDbType.Varchar2, _request.vc_usuario_creacion, ParameterDirection.Input));


                    var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                    var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                    p_Message.Size = 4000;
                    parameters.Add(pCode);
                    parameters.Add(p_Message);
                    //_connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_INS_CARGA_VISA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                    using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_INS_CARGA_ARCHIVO_TRANSAC", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
                    {
                        response.Code = pCode.Value.ToString();
                        response.message = p_Message.Value.ToString();
                    }


                    Console.WriteLine("el response.Code : " + response.Code);
                    Console.WriteLine("el response.message : " + response.message);
                }

            }


            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);
                Console.WriteLine("el ex : " + ex);
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }


        public Task<ResponseControl> DeleteCargaDeposito(Loadtransaction _request)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_ID_DEPOSITO_ARCHIVO", OracleDbType.Int32, _request.idArchivo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_TIPO_MEDIO_PAGO", OracleDbType.Int32, _request.IdLoadTransaction, ParameterDirection.Input));
                parameters.Add(new OracleParameter("p_NOMBRE_ARCHIVO", OracleDbType.Varchar2, _request.FileName, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_RAC_CONCILIACION.SP_DEL_CARGA_DEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }
        //
        public Task<List<RamoPay>> ListarRamoPay()
        {
            RamoPay entities = null;
            List<RamoPay> listaRamoPay = new List<RamoPay>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.RAMO_PAY_PASS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new RamoPay();
                    entities.IdRamoPay = Convert.ToInt32(dr["NBRANCH"]);
                    entities.DescripcionRamoPay = Convert.ToString(dr["SDESCRIPT"]);
                    listaRamoPay.Add(entities);
                }
            }

            return Task.FromResult<List<RamoPay>>(listaRamoPay);
        }
        public Task<List<EstadoRecibo>> ListarEstadoRecibo()
        {
            EstadoRecibo entities = null;
            List<EstadoRecibo> listaEstadoRecibo = new List<EstadoRecibo>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.ESTADO_RECIBO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new EstadoRecibo();
                    entities.IdEstadoRecibo = Convert.ToInt32(dr["NSTATUS_PRE"]);
                    entities.DescripcionEstadoRecibo = Convert.ToString(dr["SDESCRIPT"]);
                    listaEstadoRecibo.Add(entities);
                }
            }

            return Task.FromResult<List<EstadoRecibo>>(listaEstadoRecibo);
        }
        public Task<List<EstadoEnvio>> ListarEstadoEnvioCE()
        {
            EstadoEnvio entities = null;
            List<EstadoEnvio> listaEstadoEnvio = new List<EstadoEnvio>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_LOT_STATE_CAB", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new EstadoEnvio();
                    entities.IdEstadoEnvio = Convert.ToInt32(dr["ID_ESTADO"]);
                    entities.DescripcionEstadoEnvio = Convert.ToString(dr["ESTADO"]);
                    listaEstadoEnvio.Add(entities);
                }
            }

            return Task.FromResult<List<EstadoEnvio>>(listaEstadoEnvio);
        }

        public Task<List<DatosRespuestaProductoPay>> ListarProductoPay(DatosConsultaProductoPay datosConsultaProductoPay)
        {
            DatosRespuestaProductoPay entities = null;
            List<DatosRespuestaProductoPay> ListProductoPay = new List<DatosRespuestaProductoPay>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            long _idRamo = datosConsultaProductoPay.IdRamoPay;

            parameters.Add(new OracleParameter("RAMO", OracleDbType.Int16, _idRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.PRODUCTO_PAY_PASS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new DatosRespuestaProductoPay();
                    entities.id_productoPay = Convert.ToString(dr["NBRANCH"]);
                    entities.productoPay_descripccion = Convert.ToString(dr["SDESCRIPT"]);
                    ListProductoPay.Add(entities);
                }
            }

            return Task.FromResult<List<DatosRespuestaProductoPay>>(ListProductoPay);
        }

        public Task<ReporteContratantePay> ListarContratantePay(DatosConsultaContratantePay datosConsultaContratantePay)
        {
            ReporteContratantePay entities = null;
            List<ReporteContratantePay> listaContratantePay = new List<ReporteContratantePay>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idRamoPay = datosConsultaContratantePay.IdRamoPay;
            long _idProductoPay = datosConsultaContratantePay.IdProductoPay;
            long _idPoliza = datosConsultaContratantePay.IdPoliza;


            parameters.Add(new OracleParameter("P_RAMO", OracleDbType.Long, _idRamoPay, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Long, _idProductoPay, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_POLIZA", OracleDbType.Long, _idPoliza, ParameterDirection.Input));

            parameters.Add(new OracleParameter("P_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.CONTRATANTE_PAY", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new ReporteContratantePay();

                    entities.contratante = dr["SCLIENT"] == DBNull.Value ? string.Empty : dr["SCLIENT"].ToString();
                    entities.nom_contratante = dr["SCLIENAME"] == DBNull.Value ? string.Empty : dr["SCLIENAME"].ToString();
                    entities.cant_polizas = dr["CANTIDAD_POLIZAS"] == DBNull.Value ? string.Empty : dr["CANTIDAD_POLIZAS"].ToString();
                    //listaContratantePay.Add(entities);
                }
            }
            return Task.FromResult<ReporteContratantePay>(entities);
        }

        public Task<List<ReporteReciboPay>> ListarReciboPay(DatosConsultaReciboPay datosConsultaReciboPay)
        {

            ReporteReciboPay entities = null;
            List<ReporteReciboPay> listaReciboPay = new List<ReporteReciboPay>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                string _Contratante = datosConsultaReciboPay.contratante;
                long _Poliza = datosConsultaReciboPay.IdPoliza;
                long _ProductoPay = datosConsultaReciboPay.IdProductoPay;
                long _Ramo = datosConsultaReciboPay.idRamo;
                int _Estado = datosConsultaReciboPay.Estado;
                int _Moneda = datosConsultaReciboPay.moneda;
                long _Recibo = datosConsultaReciboPay.recibo;
                long _NroEnvio = datosConsultaReciboPay.nroEnvio;
                int _Comprobante = datosConsultaReciboPay.idComprobante;



                string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaReciboPay.FechaDesde);
                string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaReciboPay.FechaHasta);
                if (_fechaDesde == "01/01/0001")
                {
                    _fechaDesde = "";

                }
                if (_fechaHasta == "01/01/0001")
                {
                    _fechaHasta = "";

                }



                parameters.Add(new OracleParameter("P_SCLIENT", OracleDbType.NVarchar2, _Contratante, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Long, _Poliza, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_FECHA_INI", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Long, _ProductoPay, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Long, _Ramo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Int32, _Estado, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_MONEDA", OracleDbType.Int32, _Moneda, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_RECIBO", OracleDbType.Long, _Recibo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NRO_ENVIO", OracleDbType.Long, _NroEnvio, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_COMPROBANTE", OracleDbType.Int32, _Comprobante, ParameterDirection.Input));
                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.NRECEIPT_PAY", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {

                    while (dr.Read())
                    {
                        entities = new ReporteReciboPay();
                        entities.sContratante = dr["CONTRATANTE"] == DBNull.Value ? string.Empty : dr["CONTRATANTE"].ToString();
                        entities.sProducto = dr["PRODUCTO"] == DBNull.Value ? string.Empty : dr["PRODUCTO"].ToString();
                        entities.nPoliza = dr["POLIZA"] == DBNull.Value ? 0 : Convert.ToInt32(dr["POLIZA"].ToString());
                        entities.nRecibo = dr["RECIBO"] == DBNull.Value ? 0 : Convert.ToDouble(dr["RECIBO"].ToString());
                        entities.ini_vigencia = dr["INICIO_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INICIO_VIGENCIA"].ToString();
                        entities.fin_vigencia = dr["FIN_VIGENCIA"] == DBNull.Value ? string.Empty : dr["FIN_VIGENCIA"].ToString();
                        entities.sMoneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();
                        entities.nImporte = dr["IMPORTE"] == DBNull.Value ? 0 : Decimal.Parse(dr["IMPORTE"].ToString());
                        entities.nroEnvio = dr["NRO_ENVIO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_ENVIO"].ToString());
                        entities.sComprobante = dr["COMPROBANTE"] == DBNull.Value ? string.Empty : dr["COMPROBANTE"].ToString();
                        entities.sEstado = dr["ESTADO"] == DBNull.Value ? string.Empty : dr["ESTADO"].ToString();
                        entities.dlimite_pago = dr["LIMITE_PAGO"] == DBNull.Value ? string.Empty : dr["LIMITE_PAGO"].ToString();
                        entities.NEstadoDetLot = dr["ESTADO_DET_LOT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ESTADO_DET_LOT"].ToString());
                        entities.NStatusPre = dr["NSTATUS_PRE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NSTATUS_PRE"].ToString());
                        entities.nPrimerRecibo = dr["PRIMER_RECIBO"] == DBNull.Value ? 0 : Convert.ToDouble(dr["PRIMER_RECIBO"].ToString());
                        entities.nIdStatus = dr["ID_ESTADO"] == DBNull.Value ? 0 : Convert.ToDouble(dr["ID_ESTADO"].ToString());

                        entities.checkDes = false;
                        entities.isCheck = true;


                        if (entities.nIdStatus == 4 && entities.nroEnvio > 0 && entities.NEstadoDetLot != 1)
                        {
                            entities.checkDes = false;
                        }

                        else if (entities.nIdStatus == 2 && entities.nroEnvio > 0 && entities.NEstadoDetLot != 1)
                        {
                            entities.checkDes = false;
                        }

                        else if (entities.nRecibo == entities.nPrimerRecibo)
                        {
                            entities.checkDes = true;
                            entities.isCheck = false;

                        }
                        else if (entities.NEstadoDetLot == 1)
                        {
                            entities.checkDes = true;
                            entities.isCheck = false;
                        }

                        else if (entities.NEstadoDetLot != 1 && entities.nroEnvio > 0)
                        {
                            entities.checkDes = true;
                            entities.isCheck = false;
                        }

                        //nIdStatus ==4 =>CERRADO 2=> conciliado

                        listaReciboPay.Add(entities);
                    }

                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteReciboPay>>(listaReciboPay);
        }

        public Task<List<ReporteEnvioPay>> ListarEnvioPay(DatosConsultaEnvioPay datosConsultaEnvioPay)
        {

            ReporteEnvioPay entities = null;
            List<ReporteEnvioPay> listaEnvioPay = new List<ReporteEnvioPay>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                long _Poliza = datosConsultaEnvioPay.IdPoliza;
                long _ProductoPay = datosConsultaEnvioPay.IdProductoPay;
                long _Ramo = datosConsultaEnvioPay.idRamo;
                int _Estado = datosConsultaEnvioPay.Estado;
                int _NroEnvio = datosConsultaEnvioPay.nroEnvio;
                string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaEnvioPay.FechaDesde);
                string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaEnvioPay.FechaHasta);
                if (_fechaDesde == "01/01/0001" && _fechaHasta == "01/01/0001")
                {
                    _fechaDesde = null;
                    _fechaHasta = null;

                }


                parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Long, _Poliza, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_FECHA_INI", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Long, _ProductoPay, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Long, _Ramo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Int32, _Estado, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NRO_ENVIO", OracleDbType.Int32, _NroEnvio, ParameterDirection.Input));

                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_LOT_PAYPASS_CAB", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {

                    while (dr.Read())
                    {
                        entities = new ReporteEnvioPay();
                        entities.nroEnvio = dr["NRO_ENVIO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_ENVIO"].ToString());
                        entities.sFechaEnvio = dr["FECHA_ENVIO"] == DBNull.Value ? string.Empty : (dr["FECHA_ENVIO"].ToString());
                        entities.sEstado = dr["ESTADO"] == DBNull.Value ? string.Empty : dr["ESTADO"].ToString();
                        entities.nroRegistro = dr["NRO_REG"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_REG"].ToString());
                        entities.nroCobro = dr["NRO_COB"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_COB"].ToString());
                        entities.nroSinCobro = dr["NRO_SIN_COB"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_SIN_COB"].ToString());
                        entities.nroError = dr["NRO_ERROR"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_ERROR"].ToString());
                        entities.glosa = dr["GLOSA"] == DBNull.Value ? string.Empty : dr["GLOSA"].ToString();
                        entities.idEstado = dr["ID_ESTADO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ID_ESTADO"].ToString());

                        listaEnvioPay.Add(entities);
                    }

                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteEnvioPay>>(listaEnvioPay);
        }

        public Task<List<ReporteReciboDetallePay>> ListarReciboDetalle(DatosConsultaReciboDetallePay datosConsultaEnvioPay)
        {

            ReporteReciboDetallePay entities = null;
            List<ReporteReciboDetallePay> listaReciboDetalle = new List<ReporteReciboDetallePay>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                long nroEnvio = datosConsultaEnvioPay.nroEnvio;


                parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Long, nroEnvio, ParameterDirection.Input));
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);

                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_LOT_PAYPASS_RECEIPT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {

                    while (dr.Read())
                    {
                        entities = new ReporteReciboDetallePay();
                        entities.nroPoliza = dr["POLIZA"] == DBNull.Value ? 0 : long.Parse(dr["POLIZA"].ToString());
                        entities.nroRecibo = dr["RECIBO"] == DBNull.Value ? 0 : long.Parse(dr["RECIBO"].ToString());
                        entities.sEstado = dr["ESTADO"] == DBNull.Value ? string.Empty : dr["ESTADO"].ToString();
                        entities.sFechaIniVigencia = dr["INI_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        entities.sFechaFinVigencia = dr["FIN_VIGENCIA"] == DBNull.Value ? string.Empty : dr["FIN_VIGENCIA"].ToString();
                        entities.sMotivo = dr["MOTIVO"] == DBNull.Value ? string.Empty : dr["MOTIVO"].ToString();
                        entities.sComprobante = dr["COMPROBANTE"] == DBNull.Value ? string.Empty : dr["COMPROBANTE"].ToString();


                        listaReciboDetalle.Add(entities);
                    }

                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteReciboDetallePay>>(listaReciboDetalle);
        }

        //public Task<ResponseControl> ActualizarMotivos(long nroEnvio, DatosConsultaReciboActualizar datosConsultaReciboActualizar)

        public Task<ResponseControl> ActualizarMotivos(long nroEnvio, DatosConsultaReciboActualizar datosConsultaReciboActualizar)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            List<OracleParameter> parameters = new List<OracleParameter>();
            nroEnvio = datosConsultaReciboActualizar.nrosEnvio;

            try
            {

                long nUserCode = datosConsultaReciboActualizar.nUserCode;
                string sMotivo = datosConsultaReciboActualizar.sMotivo;


                //parameters.Add(new OracleParameter("P_ENVIO", OracleDbType.Long, nroEnvio, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ENVIO", OracleDbType.Long, nroEnvio, ParameterDirection.Input));

                parameters.Add(new OracleParameter("P_GLOSA", OracleDbType.NVarchar2, sMotivo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Long, nUserCode, ParameterDirection.Input));
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.GEN_LOT_PAY_PASS_CLOSE", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<ResponseControl>(response);
        }
        public Task<ResponseControl> ins_user_cabecera(long nUserCode)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Long, nUserCode, ParameterDirection.Input));
                var pEnvio = new OracleParameter("P_ENVIO", OracleDbType.Int32, ParameterDirection.Output);
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 2000;
                parameters.Add(pEnvio);
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.GEN_LOT_PAY_PASS_CAB", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                response.Data = pEnvio.Value.ToString();
                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<ResponseControl>(response);
        }

        public Task<ResponseControl> ins_detalle(Detalles det, DatosConsultaReciboCobro datosConsultaEntity, int nEstado, DatosRespuestaServicio datosRespuestaServicio, object data)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {

                parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Int32, datosConsultaEntity.idRamo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, datosConsultaEntity.idProductoPay, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Int32, det.nroPoliza, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ENVIO", OracleDbType.Int32, long.Parse(data.ToString()), ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Long, det.nroRecibo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, datosConsultaEntity.nUserCode, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Int32, nEstado, ParameterDirection.Input));

                parameters.Add(new OracleParameter("P_SERV_CODIGO", OracleDbType.NVarchar2, datosRespuestaServicio.code, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_SERV_RESULT", OracleDbType.NVarchar2, datosRespuestaServicio.success ? '1' : '0', ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_SERV_ID_PLANILLA", OracleDbType.Int32, datosRespuestaServicio.idPlanilla, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DESC_ERROR", OracleDbType.NVarchar2, datosRespuestaServicio.message, ParameterDirection.Input));
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);

                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 2000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.GEN_LOT_PAY_PASS_DET", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                response.Code = pCode.Value.ToString();

                response.message = p_Message.Value.ToString();

            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<ResponseControl>(response);
        }
        public Task<ResponseControl> ins_cabecera(object nroEnvio)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_ENVIO", OracleDbType.Long, nroEnvio, ParameterDirection.Input));
                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.UPG_LOT_PAYPASS_CAB", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

                response.Data = nroEnvio.ToString();

            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<ResponseControl>(response);
        }
        public Task<ResponseControl> StatusCierre(Detalles det, DatosConsultaReciboCobro datosConsultaEntity)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Int32, det.nroPoliza, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, datosConsultaEntity.idProductoPay, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Int32, datosConsultaEntity.idRamo, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_ENVIO", OracleDbType.Int32, long.Parse(data.ToString()), ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Long, det.nroRecibo, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, datosConsultaEntity.nUserCode, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Int32, nEstado, ParameterDirection.Input));

                //parameters.Add(new OracleParameter("P_SERV_CODIGO", OracleDbType.NVarchar2, datosRespuestaServicio.code, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_SERV_RESULT", OracleDbType.NVarchar2, datosRespuestaServicio.success ? '1' : '0', ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_SERV_ID_PLANILLA", OracleDbType.Int32, datosRespuestaServicio.idPlanilla, ParameterDirection.Input));
                //parameters.Add(new OracleParameter("P_DESC_ERROR", OracleDbType.NVarchar2, datosRespuestaServicio.message, ParameterDirection.Input));
                var pCode = new OracleParameter("P_CODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 2000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.VAL_LOT_SINCIERRE", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                //response.Code = pCode.Value.ToString();
                response.Code = pCode.Value.ToString();

                response.message = p_Message.Value.ToString();

            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<ResponseControl>(response);
        }

        public Task<DatosRespuestaContratanteNC> ListarContratanteNC(DatosConsultaContratanteNC datosConsultaContratanteNC)
        {
            DatosRespuestaContratanteNC entities = null;
            List<DatosRespuestaContratanteNC> listaContratantePay = new List<DatosRespuestaContratanteNC>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            string _docContratante = datosConsultaContratanteNC.doc_contratante;

            parameters.Add(new OracleParameter("P_DOC", OracleDbType.NVarchar2, _docContratante, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_NC_CONTRATANTE", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new DatosRespuestaContratanteNC();

                    entities.cod_contratante = dr["SCLIENT"] == DBNull.Value ? string.Empty : dr["SCLIENT"].ToString();
                    entities.nom_contratante = dr["SCLIENAME"] == DBNull.Value ? string.Empty : dr["SCLIENAME"].ToString();
                    //listaContratantePay.Add(entities);
                }
            }
            return Task.FromResult<DatosRespuestaContratanteNC>(entities);
        }


        public Task<List<ReporteReciboPendienteNC>> ListarReciboPendienteNC(DatosConsultaReciboPendienteNC datosConsultaReciboPendienteNC)
        {

            ReporteReciboPendienteNC entities = null;
            List<ReporteReciboPendienteNC> listaReciboPendienteNC = new List<ReporteReciboPendienteNC>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_CONTRATANTE", OracleDbType.NVarchar2, datosConsultaReciboPendienteNC.cod_contratante, ParameterDirection.Input));
                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_NC_RECPEND", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        entities = new ReporteReciboPendienteNC();
                        entities.ramo = dr["RAMO"] == DBNull.Value ? string.Empty : dr["RAMO"].ToString();
                        entities.producto = dr["PRODUCTO"] == DBNull.Value ? string.Empty : dr["PRODUCTO"].ToString();
                        entities.recibo = dr["RECIBO"] == DBNull.Value ? string.Empty : dr["RECIBO"].ToString();
                        entities.estado = dr["ESTADO"] == DBNull.Value ? string.Empty : dr["ESTADO"].ToString();
                        entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();
                        entities.monto = dr["MONTO"] == DBNull.Value ? string.Empty : dr["MONTO"].ToString();
                        entities.tipo = dr["TIPO"] == DBNull.Value ? string.Empty : dr["TIPO"].ToString();
                        entities.nota_credito = dr["NOTA_CREDITO"] == DBNull.Value ? string.Empty : dr["NOTA_CREDITO"].ToString();
                        entities.ini_vigen = dr["INI_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        entities.fin_vigen = dr["FIN_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        listaReciboPendienteNC.Add(entities);
                    }
                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteReciboPendienteNC>>(listaReciboPendienteNC);
        }

        public Task<List<ReporteFormaPago>> ReporteFormaPago(DatosConsultaFormaPago DatosConsultaFormaPago)
        {

            ReporteFormaPago entities = null;
            List<ReporteFormaPago> listaReporteFormaPago = new List<ReporteFormaPago>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_CONTRATANTE", OracleDbType.NVarchar2, DatosConsultaFormaPago.doc_contratante, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_REC_NC", OracleDbType.NVarchar2, DatosConsultaFormaPago.rec_nc, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_TIPO", OracleDbType.Int32, DatosConsultaFormaPago.tipo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_NC_DOCCLIENTES", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        entities = new ReporteFormaPago();
                        entities.documento = dr["DOCUMENTO"] == DBNull.Value ? string.Empty : dr["DOCUMENTO"].ToString();
                        entities.contratante = dr["CONTRATANTE"] == DBNull.Value ? string.Empty : dr["CONTRATANTE"].ToString();
                        entities.ini_vigencia = dr["INI_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        entities.fin_vigencia = dr["FIN_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();
                        entities.monto = dr["MONTO"] == DBNull.Value ? string.Empty : dr["MONTO"].ToString();
                        entities.recibo = dr["NRECEIPT"] == DBNull.Value ? string.Empty : dr["NRECEIPT"].ToString();
                        entities.Fecha_ope = dr["DISSUEDAT"] == DBNull.Value ? string.Empty : dr["DISSUEDAT"].ToString();
                        entities.sel_auto = dr["SEL_AUTOMATICA"] == DBNull.Value ? string.Empty : dr["SEL_AUTOMATICA"].ToString();
                        entities.forma_pago = dr["FORMA_PAGO"] == DBNull.Value ? string.Empty : dr["FORMA_PAGO"].ToString();
                        entities.tipo = dr["TIPO"] == DBNull.Value ? 0 : int.Parse(dr["TIPO"].ToString());




                        listaReporteFormaPago.Add(entities);
                    }
                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteFormaPago>>(listaReporteFormaPago);
        }

        public Task<List<TipoFP>> ListarTipoFP()
        {
            TipoFP entities = null;
            List<TipoFP> ListarTipoFP = new List<TipoFP>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_TYPE_FP", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new TipoFP();
                    entities.id_tipoFP = Convert.ToString(dr["NIDPAIDTYPE"]);
                    entities.des_tipoFP = Convert.ToString(dr["SDESCRIPT"]);
                    ListarTipoFP.Add(entities);
                }
            }
            return Task.FromResult<List<TipoFP>>(ListarTipoFP);
        }
        public Task<ResponseControl> InsertarRecSelFormaPagoTemp(List<ConsultaInsertListaFPTemp> listaAgregadaFPTemp)
        {
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                int i = 0;
                foreach (ConsultaInsertListaFPTemp item in listaAgregadaFPTemp)
                {
                    if (i == 0)
                    {
                        {
                            List<OracleParameter> parametersD = new List<OracleParameter>();

                            parametersD.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, int.Parse(item.nUserCode), ParameterDirection.Input));
                            parametersD.Add(new OracleParameter("P_DOCUMENTO", OracleDbType.Varchar2, item.Documento ?? string.Empty, ParameterDirection.Input));

                            var pCodeD = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                            var p_MessageD = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                            p_MessageD.Size = 4000;
                            parametersD.Add(pCodeD);
                            parametersD.Add(p_MessageD);
                            _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.DEL_SEL_FP", parametersD, ConnectionBase.enuTypeDataBase.OracleVTime);

                            response.Code = pCodeD.Value.ToString();
                            response.message = p_MessageD.Value.ToString();
                        }
                    }


                    string _fechaOpe = string.Format("{0:dd/MM/yyyy}", (Convert.ToString(Convert.ToDateTime(item.Fecha_ope))).Substring(0, 10));
                    decimal _monto = Convert.ToDecimal(item.Monto);
                    List<OracleParameter> parameters = new List<OracleParameter>();
                    parameters.Add(new OracleParameter("P_DOCUMENTO", OracleDbType.Varchar2, item.Documento ?? string.Empty, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_CONTRATANTE", OracleDbType.Varchar2, item.Contratante ?? string.Empty, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_FECHA_OPE", OracleDbType.Varchar2, _fechaOpe ?? string.Empty, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_MONEDA", OracleDbType.Int32, item.Moneda, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_MONTO", OracleDbType.Decimal, _monto, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_TIPO", OracleDbType.Int32, item.Tipo_forma_pago, ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Long, long.Parse(item.Recibo), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_ID_BANCO", OracleDbType.Int32, int.Parse(item.Banco), ParameterDirection.Input)); // si va para voucher, para pcp  nc va 0 
                    parameters.Add(new OracleParameter("P_ID_CUENTA", OracleDbType.Int32, long.Parse(item.Cuenta), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_NUM_OPE", OracleDbType.Varchar2, item.Documento ?? string.Empty, ParameterDirection.Input));// documento
                    parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.Int32, int.Parse(item.nUserCode), ParameterDirection.Input));
                    parameters.Add(new OracleParameter("P_REFERENCIA", OracleDbType.Varchar2, item.Referencia ?? string.Empty, ParameterDirection.Input));

                    var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                    var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                    p_Message.Size = 4000;
                    parameters.Add(pCode);
                    parameters.Add(p_Message);
                    _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.INS_SEL_FP", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);

                    response.Code = pCode.Value.ToString();
                    response.message = p_Message.Value.ToString();

                    i++;
                    //}

                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }



        public Task<List<ReporteListaFPSelTemp>> GetListaSelFPTemp(ConsultaListaFPSelTemp consultaListaFPSelTemp)
        {

            ReporteListaFPSelTemp entities = null;
            List<ReporteListaFPSelTemp> listaAgregadaFPTemp = new List<ReporteListaFPSelTemp>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.NVarchar2, consultaListaFPSelTemp.userCode, ParameterDirection.Input));
                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_FORMAPAGO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        entities = new ReporteListaFPSelTemp();
                        entities.forma_pago = dr["FORMA_PAGO"] == DBNull.Value ? string.Empty : dr["FORMA_PAGO"].ToString();//5
                        entities.operacion = dr["OPERACIÓN"] == DBNull.Value ? string.Empty : dr["OPERACIÓN"].ToString();//1
                        entities.monto = dr["MONTO"] == DBNull.Value ? string.Empty : dr["MONTO"].ToString();//4
                        entities.fecha_ope = dr["FECHA_OPE"] == DBNull.Value ? string.Empty : dr["FECHA_OPE"].ToString();//2
                        entities.recibo = dr["NRECEIPT"] == DBNull.Value ? string.Empty : dr["NRECEIPT"].ToString();
                        entities.banco = dr["ID_BANCO"] == DBNull.Value ? string.Empty : dr["ID_BANCO"].ToString();
                        entities.cuenta = dr["ID_CUENTA"] == DBNull.Value ? string.Empty : dr["ID_CUENTA"].ToString();
                        entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();
                        entities.tipo = dr["TIPO"] == DBNull.Value ? 0 : int.Parse(dr["TIPO"].ToString());

                        listaAgregadaFPTemp.Add(entities);
                    }
                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteListaFPSelTemp>>(listaAgregadaFPTemp);
        }
        public Task<ResponseControl> ProcessGenerarPlanillaFP(PlanillaFP _request)
        {
            //String _recibo = _request.recibo.Replace("",");
            ResponseControl response = new ResponseControl(ResponseControl.Status.Ok);
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.NVarchar2, _request.recibo, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NAMOUNT", OracleDbType.Decimal, Convert.ToDecimal(_request.monto), ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.NVarchar2, _request.userCode, ParameterDirection.Input));

                var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
                var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
                p_Message.Size = 4000;
                parameters.Add(pCode);
                parameters.Add(p_Message);
                _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.GEN_PAYROLL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);

                response.Code = pCode.Value.ToString();
                response.message = p_Message.Value.ToString();

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return Task.FromResult<ResponseControl>(response);
        }

        public Task<List<ReporteSeguimientoLote>> ReporteSeguimientoLote(DatosConsultaSeguimientoLote datosConsulta)
        {

            ReporteSeguimientoLote entities = null;
            List<ReporteSeguimientoLote> listaEnvioPay = new List<ReporteSeguimientoLote>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            try
            {
                string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsulta.FechaDesde);
                string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsulta.FechaHasta);
                //if (_fechaDesde == "01/01/0001" && _fechaHasta == "01/01/0001")
                //{
                //    _fechaDesde = null;
                //    _fechaHasta = null;

                //}

                parameters.Add(new OracleParameter("P_INI_FECHA", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_FIN_FECHA", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));

                parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.LIST_LOT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {

                    while (dr.Read())
                    {

                        entities = new ReporteSeguimientoLote();
                        //entities.nroEnvio = dr["NRO_ENVIO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NRO_ENVIO"].ToString());
                        entities.doc_contratante = dr["DOC_CONTRATANTE"] == DBNull.Value ? string.Empty : (dr["DOC_CONTRATANTE"].ToString());
                        entities.contratante = dr["CONTRATANTE"] == DBNull.Value ? string.Empty : dr["CONTRATANTE"].ToString();
                        entities.nrocel_contratante = dr["NROCELCONTRANTE"] == DBNull.Value ? string.Empty : dr["NROCELCONTRANTE"].ToString();//
                        entities.correo_contratante = dr["CORREOCONTRANTE"] == DBNull.Value ? string.Empty : dr["CORREOCONTRANTE"].ToString();//
                        entities.poliza = dr["POLIZA"] == DBNull.Value ? string.Empty : dr["POLIZA"].ToString();
                        entities.recibo = dr["RECIBO"] == DBNull.Value ? string.Empty : dr["RECIBO"].ToString();
                        entities.per_poliza = dr["PERIODO_POLIZA"] == DBNull.Value ? string.Empty : dr["PERIODO_POLIZA"].ToString();
                        entities.comprobante = dr["COMPROBANTE"] == DBNull.Value ? string.Empty : dr["COMPROBANTE"].ToString();
                        entities.estado_comprobante = dr["DESCESTCOMP"] == DBNull.Value ? string.Empty : dr["DESCESTCOMP"].ToString();      //                                        
                        entities.fecha_emision = dr["FECHA_EMISION"] == DBNull.Value ? string.Empty : dr["FECHA_EMISION"].ToString();
                        entities.ini_vigen_comp = dr["INI_VIGENCIA"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA"].ToString();
                        entities.fin_vigen_comp = dr["FIN_VIGENCIA"] == DBNull.Value ? string.Empty : dr["FIN_VIGENCIA"].ToString();
                        entities.ini_vigen_poli = dr["INI_VIGENCIA_POL"] == DBNull.Value ? string.Empty : dr["INI_VIGENCIA_POL"].ToString();
                        entities.fin_vigen_poli = dr["FIN_VIGENCIA_POL"] == DBNull.Value ? string.Empty : dr["FIN_VIGENCIA_POL"].ToString();
                        entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();
                        entities.importe = dr["IMPORTE"] == DBNull.Value ? string.Empty : dr["IMPORTE"].ToString();
                        entities.estado_envio = dr["ESTADO_COBRO"] == DBNull.Value ? string.Empty : dr["ESTADO_COBRO"].ToString();
                        entities.estado_cobro = dr["DET_ESTADO"] == DBNull.Value ? string.Empty : dr["DET_ESTADO"].ToString();
                        entities.detalle_estado = dr["DETALLE_ESTADO"] == DBNull.Value ? string.Empty : dr["DETALLE_ESTADO"].ToString();


                        entities.fecha_estado = dr["FECHA_ESTADO"] == DBNull.Value ? string.Empty : dr["FECHA_ESTADO"].ToString();
                        entities.num_envio = dr["NUMERO_ENVIO"] == DBNull.Value ? string.Empty : dr["NUMERO_ENVIO"].ToString();
                        entities.fecha_envio = dr["FECHA_ENVIO"] == DBNull.Value ? string.Empty : dr["FECHA_ENVIO"].ToString();
                        entities.agen_vendedor = dr["AGENTE_VENDEDOR"] == DBNull.Value ? string.Empty : dr["AGENTE_VENDEDOR"].ToString();
                        entities.agen_seguimiento = dr["AGENTE_SEGUIMIENTO"] == DBNull.Value ? string.Empty : dr["AGENTE_SEGUIMIENTO"].ToString();
                        entities.tipo_comision = dr["TIPO_COMISION"] == DBNull.Value ? string.Empty : dr["TIPO_COMISION"].ToString();




                        listaEnvioPay.Add(entities);
                    }

                }
            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
            return Task.FromResult<List<ReporteSeguimientoLote>>(listaEnvioPay);
        }


    }
}

