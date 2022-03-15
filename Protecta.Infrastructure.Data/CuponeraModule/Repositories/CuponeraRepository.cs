using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.CuponeraModule.Repositories
{
    public class CuponeraRepository : ICuponeraRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public CuponeraRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<Recibo> GetInfoRecibo(ParametersRecibo parametersRecibo)
        {
            Recibo entities = null;
            List<Cupon> ListCupon= new List<Cupon>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_REA_CUPONERA.REA_RECEIPT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Recibo
                    {
                        NroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty),
                        IdRamo = (dr["NBRANCH"] != null ? Convert.ToString(dr["NBRANCH"]) : string.Empty),
                        Ramo = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty),
                        NroPoliza = (dr["NPOLICY"] != null ? Convert.ToString(dr["NPOLICY"]) : string.Empty),
                        IdProducto = (dr["NPRODUCT"] != null ? Convert.ToString(dr["NPRODUCT"]) : string.Empty),
                        Producto = (dr["DES_PRODUCT"] != null ? Convert.ToString(dr["DES_PRODUCT"]) : string.Empty),
                        SClient = (dr["SCLIENT"] != null ? Convert.ToString(dr["SCLIENT"]) : string.Empty),
                        ClientName = (dr["SCLIENAME"] != null ? Convert.ToString(dr["SCLIENAME"]) : string.Empty),
                        NroCertificado = (dr["NCERTIF"] != null ? Convert.ToString(dr["NCERTIF"]) : string.Empty),
                        InicioVigencia = (dr["DEFFECDATE"] != null ? Convert.ToString(dr["DEFFECDATE"]) : string.Empty),
                        FinVigencia = (dr["DEXPIRDAT"] != null ? Convert.ToString(dr["DEXPIRDAT"]) : string.Empty),
                        MontoPrima = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty)
                    };
                }
            }

            return Task.FromResult<Recibo>(entities);
        }

        public Task<GenerateResponse> ValidateRecibo(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>
            {
                new OracleParameter("P_NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input),
                new OracleParameter("P_NTRANS_CUP", OracleDbType.Int32, parametersRecibo.idTransacion, ParameterDirection.Input)
            };

            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_NCODE.Size = 100;
            P_SMESSAGE.Size = 4000;
            parameters.Add(P_NCODE);
            parameters.Add(P_SMESSAGE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_RECEIPT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();

            }

            return Task.FromResult<GenerateResponse>(response);
        }

        public Task<List<Transacion>> ListarTransaciones()

        {
            Transacion entities = null;
            List<Transacion> listarTransaciones = new List<Transacion>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_REA_CUPONERA.REA_TABLE341", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Transacion();
                    entities.idTransacion = Convert.ToInt32(dr["NTRANS_CUP"].ToString());
                    entities.descripcion = dr["SDESCRIPT"] == null ? string.Empty : dr["SDESCRIPT"].ToString();
                    listarTransaciones.Add(entities);
                }
            }

            return Task.FromResult<List<Transacion>>(listarTransaciones);

        }

        public Task<List<Cupon>> GetInfoCuponPreview(ParametersRecibo parametersRecibo)
        {
            Recibo entities = new Recibo();
            List<Cupon> ListCupon = new List<Cupon>();
            Cupon cupon = null;
            
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPREMIUM", OracleDbType.Decimal   , parametersRecibo.Monto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCINICIAL", OracleDbType.Decimal, parametersRecibo.MontoInicial, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTAS", OracleDbType.Int32, parametersRecibo.NroCupones, ParameterDirection.Input));

            var P_NCODE = new OracleParameter("NERROR", OracleDbType.Int32, ParameterDirection.Output);
            var KEY = new OracleParameter("VAR_RETVALOUT", OracleDbType.Varchar2, ParameterDirection.Output);
            P_NCODE.Size = 100;
            KEY.Size = 4000;
            parameters.Add(P_NCODE);
            parameters.Add(KEY);


            parameters.Add(new OracleParameter("CUR_TREPORTOUT", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_CRE_CUPONERA.CALCULATION", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    cupon = new Cupon
                    {
                        skey = KEY.Value.ToString(),
                        nroCupon = (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty),
                        mroRecibo = parametersRecibo.NroRecibo,
                        fechaDesde = (dr["DEFFECDATE"] != null ? (Convert.ToDateTime(dr["DEFFECDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaHasta = (dr["DEXPIRDAT"] != null ? (Convert.ToDateTime(dr["DEXPIRDAT"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaPago = (dr["DPAYDATE"] != null ? (Convert.ToDateTime(dr["DPAYDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        montoCupon = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty),
                    };

                    ListCupon.Add(cupon);
                }
                entities.ListCupones = ListCupon;
            }

            return Task.FromResult(ListCupon);
        }

        public Task<GenerateResponse> GenerateCupon(ParametersRecibo parametersRecibo)
        {
            
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SKEY", OracleDbType.NVarchar2, parametersRecibo.Key, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, parametersRecibo.UserCode, ParameterDirection.Input));


            var P_NCODE = new OracleParameter("NERROR", OracleDbType.Int32, ParameterDirection.Output);
            var NUM_CUPON = new OracleParameter("NCUPONERA", OracleDbType.Int64, ParameterDirection.Output);
            P_NCODE.Size = 100;
            NUM_CUPON.Size = 100;
            parameters.Add(P_NCODE);
            parameters.Add(NUM_CUPON);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.CRECOUPON", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.data = NUM_CUPON.Value.ToString();

            }

            return Task.FromResult(response);
        }

        public Task<Recibo> GetInfoCuponera(ParametersRecibo parametersRecibo)
        {
            Recibo entities = null;
            List<Cupon> ListCupon = new List<Cupon>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int64, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("CUR_CUPONERA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_CRE_CUPONERA.REACOUPON_BOOK", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new Recibo
                    {
                        NroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty),
                        IdRamo = (dr["NBRANCH"] != null ? Convert.ToString(dr["NBRANCH"]) : string.Empty),
                        Ramo = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty),
                        NroPoliza = (dr["NPOLICY"] != null ? Convert.ToString(dr["NPOLICY"]) : string.Empty),
                        IdProducto = (dr["NPRODUCT"] != null ? Convert.ToString(dr["NPRODUCT"]) : string.Empty),
                        Producto = (dr["DES_PRODUCT"] != null ? Convert.ToString(dr["DES_PRODUCT"]) : string.Empty),
                        SClient = (dr["SCLIENT"] != null ? Convert.ToString(dr["SCLIENT"]) : string.Empty),
                        ClientName = (dr["SCLIENAME"] != null ? Convert.ToString(dr["SCLIENAME"]) : string.Empty),
                        NroCertificado = (dr["NCERTIF"] != null ? Convert.ToString(dr["NCERTIF"]) : string.Empty),
                        InicioVigencia = (dr["DEFFECDATE"] != null ? Convert.ToString(dr["DEFFECDATE"]).Substring(0,10) : string.Empty),
                        FinVigencia = (dr["DEXPIRDAT"] != null ? Convert.ToString(dr["DEXPIRDAT"]).Substring(0, 10) : string.Empty),
                        CantCupones = (dr["NQUANTITY_CUPON"] != null ? Convert.ToString(dr["NQUANTITY_CUPON"]) : string.Empty),
                        MontoPrima = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty),
                        MontoInicial = (dr["NCUOTA_INI"] != null ? Convert.ToString(dr["NCUOTA_INI"]) : string.Empty),
                        MontoCupon = (dr["NCUOTA"] != null ? Convert.ToString(dr["NCUOTA"]) : string.Empty),
                        PorcentajeInteres = (dr["NINTERES"] != null ? Convert.ToString(dr["NINTERES"]) : string.Empty),
                        FechaPago = (dr["DEFFECDATE"] != null ? Convert.ToString(dr["DEFFECDATE"]).Substring(0, 10) : string.Empty),
                        //MontoPrima = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty)
                    };
                }
            }

            return Task.FromResult<Recibo>(entities);
        }
        public Task<List<Cupon>> GetInfoCuponeraDetail(ParametersRecibo parametersRecibo)
        {
            Recibo entities = new Recibo();
            List<Cupon> ListCupon = new List<Cupon>();
            Cupon cupon = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));


            parameters.Add(new OracleParameter("CUR_CUPONERA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_CRE_CUPONERA.REACOUPONS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    cupon = new Cupon
                    {
                        nrocuponera = (dr["NCUPONERA"] != null ? Convert.ToString(dr["NCUPONERA"]) : string.Empty),
                        nroCupon = (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty),
                        mroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty),
                        fechaDesde = (dr["DEFFECDATE"] != null ? (Convert.ToDateTime(dr["DEFFECDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaHasta = (dr["DEXPIRDAT"] != null ? (Convert.ToDateTime(dr["DEXPIRDAT"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaPago = (dr["DPAYDATE"] != null ? (Convert.ToDateTime(dr["DPAYDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        montoCupon = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty),
                        estado = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty)
                    };

                    ListCupon.Add(cupon);
                }
                entities.ListCupones = ListCupon;
            }

            return Task.FromResult(ListCupon);
        }



        public Task<Recibo> GetInfoCupon(ParametersRecibo parametersRecibo)
        {
            Recibo entities = null;
            List<Cupon> ListCupon = new List<Cupon>();
            Cupon cupon = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_IDTRANSACION", OracleDbType.NVarchar2, parametersRecibo.idTransacion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NROCUPON", OracleDbType.NVarchar2, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NRORECIBO", OracleDbType.Long, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_PAYROLL.PA_SEL_BANK", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Recibo
                    {
                        // Client = (dr["CLIENT"] != null ? Convert.ToString(dr["CLIENT"]) : string.Empty),
                        Fecha = (dr["FECHA"] != null ? Convert.ToString(dr["FECHA"]) : string.Empty),
                        Ramo = (dr["DES_RAMO"] != null ? Convert.ToString(dr["DES_RAMO"]) : string.Empty),
                        Producto = (dr["DES_PRODUCTO"] != null ? Convert.ToString(dr["DES_PRODUCTO"]) : string.Empty),
                        NroPoliza = (dr["NRO_POLIZA"] != null ? Convert.ToString(dr["NRO_POLIZA"]) : string.Empty),
                        NroCertificado = (dr["NRO_CERTIFICADO"] != null ? Convert.ToString(dr["NRO_CERTIFICADO"]) : string.Empty),
                        Moneda = (dr["DES_MONEDA"] != null ? Convert.ToString(dr["DES_MONEDA"]) : string.Empty),
                        InicioVigencia = (dr["INCIO_VIGENCIA"] != null ? Convert.ToString(dr["INCIO_VIGENCIA"]) : string.Empty),
                        FinVigencia = (dr["FIN_VIGENCIA"] != null ? Convert.ToString(dr["FIN_VIGENCIA"]) : string.Empty),
                        CantCupones = (dr["CANT_CUPONES"] != null ? Convert.ToString(dr["CANT_CUPONES"]) : string.Empty),
                        MontoPrima = (dr["MONTO_PRIMA"] != null ? Convert.ToString(dr["MONTO_PRIMA"]) : string.Empty),
                        MontoInicial = (dr["MONTO_INICIAL"] != null ? Convert.ToString(dr["MONTO_INICIAL"]) : string.Empty),
                        MontoCupon = (dr["MONTO_CUPON"] != null ? Convert.ToString(dr["MONTO_CUPON"]) : string.Empty),
                        PorcentajeInteres = (dr["PORC_INTERES"] != null ? Convert.ToString(dr["PORC_INTERES"]) : string.Empty),
                        FechaPago = (dr["FECHA_PAGO"] != null ? Convert.ToString(dr["FECHA_PAGO"]) : string.Empty)
                    };
                }
                dr.NextResult();
                while (dr.Read())
                {
                    cupon = new Cupon
                    {
                        estado = (dr["ESTADO"] != null ? Convert.ToString(dr["ESTADO"]) : string.Empty),
                        fechaDesde = (dr["FECHA_DESDE"] != null ? Convert.ToString(dr["FECHA_DESDE"]) : string.Empty),
                        fechaHasta = (dr["FECHA_HASTA"] != null ? Convert.ToString(dr["FECHA_HASTA"]) : string.Empty),
                        nroCupon = (dr["NRO_DOCUMENTO"] != null ? Convert.ToString(dr["NRO_DOCUMENTO"]) : string.Empty),
                        mroRecibo = (dr["NRO_RECIBO"] != null ? Convert.ToString(dr["NRO_RECIBO"]) : string.Empty),
                        fechaPago = (dr["FECHA_PAGO"] != null ? Convert.ToString(dr["FECHA_PAGO"]) : string.Empty),
                        montoCupon = (dr["MONTO_CUPON"] != null ? Convert.ToString(dr["MONTO_CUPON"]) : string.Empty),
                    };

                    ListCupon.Add(cupon);
                }
                entities.ListCupones = ListCupon;
            }

            return Task.FromResult<Recibo>(entities);
        }




        public Task<DetalleRecibo> GetInfoMovimiento(ParametersRecibo parametersRecibo)
        {
            DetalleRecibo DetailCupon = new DetalleRecibo();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_PAYROLL.PA_SEL_BANK", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    DetailCupon = new DetalleRecibo
                    {
                       NroCupon = (dr["NRO_CUPON"] != null ? Convert.ToString(dr["NRO_CUPON"]) : string.Empty),
                       Movimiento = (dr["MOVIMIENTO"] != null ? Convert.ToString(dr["MOVIMIENTO"]) : string.Empty),
                       NroRecibo = (dr["NRO_RECIBO"] != null ? Convert.ToString(dr["NRO_RECIBO"]) : string.Empty),
                       Fecha = (dr["FECHA"] != null ? Convert.ToString(dr["FECHA"]) : string.Empty),
                       FechaPago = (dr["FECHA_PAGO"] != null ? Convert.ToString(dr["FECHA_PAGO"]) : string.Empty),
                       IdTransacion = (dr["ID_TRANSACION"] != null ? Convert.ToString(dr["ID_TRANSACION"]) : string.Empty),
                       DescTransacion = (dr["DESC_TRANSACION"] != null ? Convert.ToString(dr["DESC_TRANSACION"]) : string.Empty),
                       MontoCupon = (dr["MONTO_CUPON"] != null ? Convert.ToString(dr["MONTO_CUPON"]) : string.Empty),
                       IdUsuario = (dr["ID_USUARIO"] != null ? Convert.ToString(dr["ID_USUARIO"]) : string.Empty),
                       DescUsuario = (dr["DESC_USUARIO"] != null ? Convert.ToString(dr["DESC_USUARIO"]) : string.Empty),

                    };
                }
            }

            return Task.FromResult<DetalleRecibo>(DetailCupon);
        }

        public Task<GenerateResponse> AnnulmentCupon(ParametersRecibo parametersRecibo)
        {

            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, parametersRecibo.UserCode, ParameterDirection.Input));


            var P_NCODE = new OracleParameter("NERROR", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.ANULCOUPON", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());

            }

            return Task.FromResult<GenerateResponse>(response);
        }
        public Task<GenerateResponse> ValidarPrintCupon(PrintCupon paramPrint)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NCUPONERA", OracleDbType.Int64, paramPrint.cuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCUOTA_INI", OracleDbType.Int32, paramPrint.cuponInicial, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCUOTA_FIN", OracleDbType.Int32, paramPrint.cuponFinal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCOPY", OracleDbType.Int64, paramPrint.copias, ParameterDirection.Input));
            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            try {
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_PRINTER", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                    response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                    
                    response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }
            }
            catch (Exception ex)
            {
                response.P_NCODE = 1;
                response.P_SMESSAGE = ex.Message;
            }
            return Task.FromResult<GenerateResponse>(response);
        }

        public Task<List<TemplateCupon1>> PrintCupon(PrintCupon paramPrint)
        {
            List<TemplateCupon1> Template = new List<TemplateCupon1>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, paramPrint.cuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_INI", OracleDbType.Int32, paramPrint.cuponInicial, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_FIN", OracleDbType.Int32, paramPrint.cuponFinal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCOPY", OracleDbType.Int32, paramPrint.copias, ParameterDirection.Input));
            parameters.Add(new OracleParameter("CUR_TOUT", OracleDbType.RefCursor, ParameterDirection.Output));
            try
            {
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.PRINTCOUPONBOOK1", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    TemplateCupon1 item = new TemplateCupon1();

                    item.IdRamo = (dr["NBRANCH"] != null ? Convert.ToInt32(dr["NBRANCH"]) : 0);
                    item.DesRamo = (dr["SBRANCH"] != null ? Convert.ToString(dr["SBRANCH"]) : string.Empty);
                    item.IdMoneda = (dr["NCURRENCY"] != null ? Convert.ToInt32(dr["NCURRENCY"]) : 0);
                    item.DesMoneda = (dr["SCURRENCY"] != null ? Convert.ToString(dr["SCURRENCY"]) : string.Empty);
                    item.Policy = (dr["NPOLICY"] != null ? Convert.ToString(dr["NPOLICY"]) : string.Empty);
                    item.VigenciaDesde = (dr["DSTARTDATE"] != null ? Convert.ToString(dr["DSTARTDATE"]) : string.Empty);
                    item.VigenciaHasta = (dr["DEXPIRDAT"] != null ? Convert.ToString(dr["DEXPIRDAT"]) : string.Empty);
                    item.Asegurado = (dr["SCLIENAME"] != null ? Convert.ToString(dr["SCLIENAME"]) : string.Empty);
                    item.Documento = (dr["SIDDOC"] != null ? Convert.ToString(dr["SIDDOC"]) : string.Empty);
                    item.Direccion = (dr["SSTREET_FULL"] != null ? Convert.ToString(dr["SSTREET_FULL"]) : string.Empty);
                    item.Convenio = (dr["NCUPONERA"] != null ? Convert.ToInt32(dr["NCUPONERA"]) : 0);
                    item.Intermediario = (dr["SINTERNAME"] != null ? Convert.ToString(dr["SINTERNAME"]) : string.Empty);
                    item.Cuponera = (dr["NCUPONERA"] != null ? Convert.ToString(dr["NCUPONERA"]) : string.Empty);
                    item.Cupon = (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty);
                    item.FechaVencimiento = (dr["DPAYDATE"] != null ? Convert.ToString(dr["DPAYDATE"]) : string.Empty);
                    item.Importe = (dr["NPREMIUM"] != null ? Convert.ToDecimal(dr["NPREMIUM"]) : 0);
                    item.Cupones = (dr["NQUANTITY_CUPON"] != null ? Convert.ToString(dr["NQUANTITY_CUPON"]) : string.Empty);

                    
                    Template.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                var mens = string.Empty;
                mens = ex.Message;
            }
            return Task.FromResult(Template);
        }

        public Task<List<TemplateCupon2>> PrintCuponCrono(PrintCupon paramPrint)
                    {
            List<TemplateCupon2> Template = new List<TemplateCupon2>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, paramPrint.cuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_INI", OracleDbType.Int32, paramPrint.cuponInicial, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_FIN", OracleDbType.Int32, paramPrint.cuponFinal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCOPY", OracleDbType.Int32, paramPrint.copias, ParameterDirection.Input));
            parameters.Add(new OracleParameter("CUR_TOUT", OracleDbType.RefCursor, ParameterDirection.Output));
            try
            {
                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.PRINTCOUPONBOOK2", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        TemplateCupon2 item = new TemplateCupon2();

                        item.DesRamo = (dr["SBRANCH"] != null ? Convert.ToString(dr["SBRANCH"]) : string.Empty);
                        item.Poliza = (dr["NPOLICY"] != null ? Convert.ToString(dr["NPOLICY"]) : string.Empty);
                        item.VigenciaDesde = (dr["DSTARTDATE"] != null ? Convert.ToString(dr["DSTARTDATE"]) : string.Empty);
                        item.VigenciaHasta = (dr["DEXPIRDAT"] != null ? Convert.ToString(dr["DEXPIRDAT"]) : string.Empty);
                        item.Moneda = (dr["SCURRENCY"] != null ? Convert.ToString(dr["SCURRENCY"]) : string.Empty);
                        item.ModalidadPago = (dr["SMOD_PAGO"] != null ? Convert.ToString(dr["SMOD_PAGO"]) : string.Empty);
                        item.Fecha = (dr["DPAYDATE"] != null ? Convert.ToDateTime(dr["DPAYDATE"]).ToString("dd/MM/yyyy") : string.Empty);
                        item.Nombres = (dr["SCLIENAME"] != null ? Convert.ToString(dr["SCLIENAME"]) : string.Empty);
                        item.NroDocumento = (dr["SIDDOC"] != null ? Convert.ToString(dr["SIDDOC"]) : string.Empty);
                        item.Direccion = (dr["SSTREET_FULL"] != null ? Convert.ToString(dr["SSTREET_FULL"]) : string.Empty);
                        item.NroCupon = (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty);
                        item.NroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty);
                        item.Vencimiento = (dr["DPAYDATE"] != null ? Convert.ToDateTime(dr["DPAYDATE"]).ToString("dd/MM/yyyy") : string.Empty);
                        item.Interes = "0";
                        item.Importe = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty);
                        item.NroPago = (dr["NCUPONERA"] != null ? Convert.ToString(dr["NCUPONERA"]) : string.Empty);

                        
                    Template.Add(item);
                }
            }
            }catch(Exception ex)
            {
                var mens = string.Empty;
                mens = ex.Message;
            }

            return Task.FromResult(Template);
        }
        public Task<GenerateResponse> ValidateCouponBook(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NRECEIPT_ORI", OracleDbType.Int64, null, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NTRANS_CUP", OracleDbType.Int32, null, ParameterDirection.Input));


            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_REFINAN_COUPON_BOOK", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }

            return Task.FromResult<GenerateResponse>(response);

        }
        public Task<GenerateResponse> ValidateCoupon(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCOUPON", OracleDbType.Int32, parametersRecibo.NroCupon, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_DPAYDATE", OracleDbType.Date, DateTime.Parse(parametersRecibo.FechaPago), ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPREMIUM", OracleDbType.Decimal, Double.Parse(parametersRecibo.Monto), ParameterDirection.Input));


            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_REFINAN_COUPONS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }
            return Task.FromResult<GenerateResponse>(response);
        }

        public Task<GenerateResponse> RecalculateCouponBook(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<Cupon> ListCupon = new List<Cupon>();
            Cupon cupon = null;
            if (string.IsNullOrEmpty(parametersRecibo.Key)) { parametersRecibo.Key = null; }
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SKEY", OracleDbType.Char, parametersRecibo.Key, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCOUPON", OracleDbType.Int64, parametersRecibo.NroCupon, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPREMIUMNEW", OracleDbType.Decimal, Double.Parse(parametersRecibo.Monto), ParameterDirection.Input));
            parameters.Add(new OracleParameter("DPAYDATENEW", OracleDbType.Date, DateTime.Parse(parametersRecibo.FechaPago), ParameterDirection.Input));            

            var P_NCODE = new OracleParameter("NERROR", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("VAR_RETVALOUT", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            parameters.Add(new OracleParameter("CUR_CUPONERA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_CRE_CUPONERA.RECALCULATION", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    cupon = new Cupon
                    {
                        nroCupon = (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty),
                        mroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty),
                        fechaDesde = (dr["DEFFECDATE"] != null ? (Convert.ToDateTime(dr["DEFFECDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaHasta = (dr["DEXPIRDAT"] != null ? (Convert.ToDateTime(dr["DEXPIRDAT"]).ToString("dd/MM/yyyy")) : string.Empty),
                        fechaPago = (dr["DPAYDATE"] != null ? (Convert.ToDateTime(dr["DPAYDATE"]).ToString("dd/MM/yyyy")) : string.Empty),
                        montoCupon = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty),
                    };

                    ListCupon.Add(cupon);
                }
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
                response.Cupones = ListCupon;
            }

            return Task.FromResult(response);
        }
        public Task<GenerateResponse> RefactorCoupon(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int32, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SKEY", OracleDbType.Varchar2, parametersRecibo.Key, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, parametersRecibo.UserCode, ParameterDirection.Input));
      


            var P_NCODE = new OracleParameter("NERROR", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.MODCOUPON", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }
            return Task.FromResult<GenerateResponse>(response);
        }

        public Task<GenerateResponse> ValCantCoupons(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Int64, parametersRecibo.NroRecibo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCANTCOUP", OracleDbType.Int32, parametersRecibo.NroCupones, ParameterDirection.Input));


            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_CANTCOUPONS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }
            return Task.FromResult<GenerateResponse>(response);
        }

        public Task<GenerateResponse> ValCouponbook(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            string rec = parametersRecibo.NroRecibo == "" ? null : parametersRecibo.NroRecibo;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NCUPONERA", OracleDbType.Int32, parametersRecibo.NroCuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NRECEIPT_ORI", OracleDbType.Int32, rec, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NTRANS_CUP", OracleDbType.Int32, parametersRecibo.idTransacion, ParameterDirection.Input));


            var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
            P_NCODE.Size = 100;
            parameters.Add(P_NCODE);
            var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
            P_SMESSAGE.Size = 10000;
            parameters.Add(P_SMESSAGE);
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_VAL_CUPONERA.VAL_COUPON_BOOK", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                response.P_NCODE = Int32.Parse(P_NCODE.Value.ToString());
                response.P_SMESSAGE = P_SMESSAGE.Value.ToString();
            }
            return Task.FromResult<GenerateResponse>(response);
        }


        public Task<List<Ramo>> ListRamos()
        {
            List<Ramo> response = new List<Ramo>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_REA_CUPONERA.REABRANCHS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    Ramo ramo = new Ramo();
                    ramo.idRamo = (dr["NBRANCH"] != null ? Convert.ToString(dr["NBRANCH"]) : string.Empty);
                    ramo.descripcion = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty);
                    response.Add(ramo);
                }
            }
            return Task.FromResult<List<Ramo>>(response);
        }
        public Task<List<Producto>> ListProductos(ParametersRecibo parametersRecibo)
        { 
            List<Producto> response = new List<Producto>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, parametersRecibo.idRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_REA_CUPONERA.REAPRODUCTS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    Producto producto = new Producto();
                    producto.idProducto = (dr["NPRODUCT"] != null ? Convert.ToString(dr["NPRODUCT"]) : string.Empty);
                    producto.descripcion = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty);
                    response.Add(producto);
                }
            }
            return Task.FromResult<List<Producto>>(response);
        }
        public Task<GenerateResponse> ValPolFact(ParametersRecibo parametersRecibo)
        {
            GenerateResponse response = new GenerateResponse();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, parametersRecibo.idRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, parametersRecibo.idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPOLICY", OracleDbType.Int32, parametersRecibo.idPoliza, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_REA_CUPONERA.REAPOLICYFACT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    response.P_NCODE = dr["SCOLINVOT"] != null ? Int32.Parse(Convert.ToString(dr["SCOLINVOT"])) : 0;
                }
            }
            return Task.FromResult<GenerateResponse>(response); 
        }
        public Task<List<Recibo>> ListRecibo(ParametersRecibo parametersRecibo)
        {
            List<Recibo> response = new List<Recibo>();
            List<OracleParameter> parameters = new List<OracleParameter>();
      
            parameters.Add(new OracleParameter("SCERTYPE", OracleDbType.Varchar2, "2", ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, parametersRecibo.idRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, parametersRecibo.idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPOLICY", OracleDbType.Int32, parametersRecibo.idPoliza, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCERTIF", OracleDbType.Int32, parametersRecibo.idCertificado, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_REA_CUPONERA.REARECEIPTS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    Recibo recibo = new Recibo();
                    recibo.NroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty);
                    recibo.Moneda = (dr["SDESCRIPT"] != null ? Convert.ToString(dr["SDESCRIPT"]) : string.Empty);
                    recibo.InicioVigencia= (dr["DEFFECDATE"] != null ? Convert.ToString(dr["DEFFECDATE"]).Substring(0,10) : string.Empty);
                    recibo.FinVigencia = (dr["DEXPIRDAT"] != null ? Convert.ToString(dr["DEXPIRDAT"]).Substring(0, 10) : string.Empty);
                    recibo.MontoPrima = (dr["NPREMIUM"] != null ? Convert.ToString(dr["NPREMIUM"]) : string.Empty);
                    response.Add(recibo);
                }
            }
            return Task.FromResult<List<Recibo>>(response); ;
        }


    }
}


