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
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedureCupon("PKG_REA_CUPONERA.REA_TABLE340", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
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
                        //NroRecibo = (dr["NRECEIPT"] != null ? Convert.ToString(dr["NRECEIPT"]) : string.Empty),
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

        public Task<List<TemplateCupon1>> PrintCupon(PrintCupon paramPrint)
        {
            List<TemplateCupon1> Template = new List<TemplateCupon1>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NCUPONERA", OracleDbType.Int32, paramPrint.cuponera, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_INI", OracleDbType.Int32, paramPrint.cuponInicial, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCUOTA_FIN", OracleDbType.Int32, paramPrint.cuponFinal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCOPY", OracleDbType.Int32, paramPrint.copias, ParameterDirection.Input));
            parameters.Add(new OracleParameter("CUR_TOUT", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_CRE_CUPONERA.PRINTCOUPONBOOK1", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    TemplateCupon1 item= new TemplateCupon1()
                    {
                        IdRamo= (dr[":B17"] != null ? Convert.ToInt32(dr[":B17"]) : 0),
                        DesRamo = (dr[":B16"] != null ? Convert.ToString(dr[":B16"]) : string.Empty),
                        IdMoneda = (dr[":B15"] != null ? Convert.ToInt32(dr[":B15"]) : 0),
                        DesMoneda = (dr[":B14"] != null ? Convert.ToString(dr[":B14"]) : string.Empty),
                        Policy= (dr[":B13"] != null ? Convert.ToString(dr[":B13"]) : string.Empty),
                        VigenciaDesde = (dr[":B11"] != null ? Convert.ToString(dr[":B11"]) : string.Empty),
                        VigenciaHasta = (dr[":B10"] != null ? Convert.ToString(dr[":B10"]) : string.Empty),
                        Asegurado= (dr[ ":B8"] != null ? Convert.ToString(dr[":B8"]) : string.Empty),
                        Documento = (dr[":B7"] != null ? Convert.ToString(dr[":B7"]) : string.Empty),
                        Direccion = (dr[":B6"] != null ? Convert.ToString(dr[":B6"]) : string.Empty),
                        Convenio= (dr[":B5"] != null ? Convert.ToInt32(dr[":B5"]) : 0),
                        Intermediario= (dr[":B4"] != null ? Convert.ToString(dr[":B4"]) : string.Empty),
                        Cuponera = (dr["NCUPONERA"] != null ? Convert.ToString(dr["NCUPONERA"]) : string.Empty),
                        Cupon= (dr["NCOUPON"] != null ? Convert.ToString(dr["NCOUPON"]) : string.Empty),
                        FechaVencimiento = (dr["DPAYDATE"] != null ? Convert.ToString(dr["DPAYDATE"]) : string.Empty),
                        Importe = (dr["NPREMIUM"] != null ? Convert.ToDecimal(dr["NPREMIUM"]) : 0)
                    };
                    Template.Add(item);
                }
            }

            return Task.FromResult(Template);
        }

      
    }
}
