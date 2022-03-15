using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Transactions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg;
using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;

namespace Protecta.Infrastructure.Data.FacturaModule.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public FacturaRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<DocumentoAbono>> ListarDocumentosAbonos(DatosConsultaDocumento datosConsultaDocumento)
        {
            DocumentoAbono documento_abono = null;
            List<DocumentoAbono> lista_documento_abono = new List<DocumentoAbono>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NUMERO_FACTURA", OracleDbType.NVarchar2, datosConsultaDocumento.NumeroFactura, ParameterDirection.Input));
            parameters.Add(new OracleParameter("FECHADESDE", OracleDbType.NVarchar2, datosConsultaDocumento.FechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("FECHAHASTA", OracleDbType.NVarchar2, datosConsultaDocumento.FechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLREADOCUMENTOABONO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    documento_abono = new DocumentoAbono();

                    if (dr["NUMERO_FACTURA"] != null && dr["NUMERO_FACTURA"].ToString() != "")
                    {
                        documento_abono.NumeroFactura = Convert.ToString(dr["NUMERO_FACTURA"]);
                    }
                    else
                    {
                        documento_abono.NumeroFactura = "";
                    }

                    if (dr["FECHA_FACTURA"] != null && dr["FECHA_FACTURA"].ToString() != "")
                    {
                        documento_abono.FechaFactura = Convert.ToDateTime(dr["FECHA_FACTURA"]).ToShortDateString();
                    }
                    else
                    {
                        documento_abono.FechaFactura = "";
                    }

                    if (dr["MONTO_FACTURA"] != null && dr["MONTO_FACTURA"].ToString() != "")
                    {
                        documento_abono.MontoFactura = Convert.ToDecimal(dr["MONTO_FACTURA"]);
                    }

                    if (dr["ID_NOTA_CREDITO"] != null && dr["ID_NOTA_CREDITO"].ToString() != "")
                    {
                        documento_abono.IdNotaCredito = Convert.ToString(dr["ID_NOTA_CREDITO"]);
                    }
                    else
                    {
                        documento_abono.IdNotaCredito = "";
                    }

                    if (dr["FECHA_NOTA_CREDITO"] != null && dr["FECHA_NOTA_CREDITO"].ToString() != "")
                    {
                        documento_abono.FechaNotaCredito = Convert.ToDateTime(dr["FECHA_NOTA_CREDITO"]).ToShortDateString();
                    }
                    else
                    {
                        documento_abono.FechaNotaCredito = "";
                    }

                    documento_abono.Estado = Convert.ToString(dr["ESTADO"]);                   
                    lista_documento_abono.Add(documento_abono);
                }
            }

            return Task.FromResult<List<DocumentoAbono>>(lista_documento_abono);
        }

        public Task<FacturaDeposito> ValidarExisteFacturaDeposito()
        {
            FacturaDeposito factura_deposito = new FacturaDeposito();            

            List<OracleParameter> parameters = new List<OracleParameter>();
            var PARAM_INDICA_GENERAR = new OracleParameter("INDICA_GENERAR", OracleDbType.Char, 1, OracleDbType.Char, ParameterDirection.Output);
            parameters.Add(PARAM_INDICA_GENERAR); 

            var PARAM_FECHA_FACTURA = new OracleParameter("FECHA_FACTURA", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(PARAM_FECHA_FACTURA);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLVALFACTURADEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            if (PARAM_INDICA_GENERAR.Value != null && PARAM_INDICA_GENERAR.Value.ToString() != "")
            {
                factura_deposito.IndGeneracion = PARAM_INDICA_GENERAR.Value.ToString();
            }
            else
            {
                factura_deposito.IndGeneracion = "";
            }

            if (PARAM_FECHA_FACTURA.Value != null && PARAM_FECHA_FACTURA.Value.ToString() != "")
            {
                factura_deposito.DtFechaCreacion = Convert.ToString(PARAM_FECHA_FACTURA.Value);
            }
            else
            {
                factura_deposito.DtFechaCreacion = "";
            }

            return Task.FromResult<FacturaDeposito>(factura_deposito);
        }

        public Task<FacturaDeposito> ObtenerFacturaDeposito(DatosFacturaAbonos datosFacturaAbonos)
        {
            string numero_serie = string.Empty;

            FacturaDeposito factura_deposito = null;
            DetalleFacturaDeposito detalle_factura_deposito = null;

            List<Deposito> lista_deposito = new List<Deposito>();            
            lista_deposito = ListarDepositoNoConciliado();

            numero_serie = ObtenerNumeroSerie();

            if (lista_deposito.Count > 0 && numero_serie != "")
            {
                factura_deposito = new FacturaDeposito();
                factura_deposito.IdProducto = datosFacturaAbonos.IdProducto;
                factura_deposito.NumeroFactura = numero_serie;
                factura_deposito.MontoTotal = lista_deposito.Sum(x=>x.Monto);
                factura_deposito.VcUsuarioCreacion = datosFacturaAbonos.Usuario;
                factura_deposito.IddgEstado = 1; //Generado
                factura_deposito.DetalleFacturaDeposito = new List<DetalleFacturaDeposito>();

                foreach (Deposito d in lista_deposito)
                {
                    detalle_factura_deposito = new DetalleFacturaDeposito();
                    detalle_factura_deposito.IdDeposito = d.IdDeposito;
                    detalle_factura_deposito.Monto = d.Monto;
                    detalle_factura_deposito.IdTipoMedioPago = d.IdTipoMedioPago;
                    detalle_factura_deposito.VcUsuarioCreacion = datosFacturaAbonos.Usuario;
                    detalle_factura_deposito.IddgEstado = 1; //Generado
                    detalle_factura_deposito.IddgEstadoDeposito = 1108; //Con Factura de Abono
                    factura_deposito.DetalleFacturaDeposito.Add(detalle_factura_deposito);
                }

            }

            return Task.FromResult<FacturaDeposito>(factura_deposito);
        }

        public Task<string> RegistrarFacturaDeposito(FacturaDeposito factura_deposito)
        {
            string result = string.Empty;
            List<OracleParameter> paramfac = null;
            List<OracleParameter> paramdet = null;

            paramfac = new List<OracleParameter>();
            paramfac.Add(new OracleParameter("ID_PRODUCTO", OracleDbType.Int32, factura_deposito.IdProducto, ParameterDirection.Input));
            paramfac.Add(new OracleParameter("NUMERO_FACTURA", OracleDbType.NVarchar2, factura_deposito.NumeroFactura, ParameterDirection.Input));
            paramfac.Add(new OracleParameter("DC_MONTO_TOTAL", OracleDbType.Decimal, factura_deposito.MontoTotal, ParameterDirection.Input));
            paramfac.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, factura_deposito.VcUsuarioCreacion, ParameterDirection.Input));
            paramfac.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, factura_deposito.IddgEstado, ParameterDirection.Input));

            var pnewId = new OracleParameter("P_RESULTADO", OracleDbType.Int32, ParameterDirection.Output);
            paramfac.Add(pnewId);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLPOSTFACTURA_DEPOSITO", paramfac, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            foreach (DetalleFacturaDeposito d in factura_deposito.DetalleFacturaDeposito)
            {
                paramdet = new List<OracleParameter>();
                paramdet.Add(new OracleParameter("ID_FACTURA_DEPOSITO", OracleDbType.Int32, pnewId.Value, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("ID_DEPOSITO", OracleDbType.Int32, d.IdDeposito, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("DC_MONTO", OracleDbType.Decimal, d.Monto, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("ID_TIPO_MEDIO_PAGO", OracleDbType.Int32, d.IdTipoMedioPago, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, d.VcUsuarioCreacion, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, d.IddgEstado, ParameterDirection.Input));
                paramdet.Add(new OracleParameter("P_RESULTADO", OracleDbType.Int32, ParameterDirection.Output));

                _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLPOSTDETFACTURA_DEPOSITO", paramdet, ConnectionBase.enuTypeDataBase.OracleConciliacion);
            }

            result = factura_deposito.NumeroFactura;

            return Task.FromResult<string>(result);
        }

        public List<Deposito> ListarDepositoNoConciliado()
        {
            Deposito deposito = null;
            List<Deposito> lista_deposito = new List<Deposito>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLREADEPOSITONOCONCILIADO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    deposito = new Deposito();
                    deposito.IdDeposito = Convert.ToInt64(dr["ID_DEPOSITO"]);
                    deposito.Monto = Convert.ToDecimal(dr["MONTO_DEPOSITO"]);
                    deposito.IdTipoMedioPago = Convert.ToInt32(dr["ID_TIPO_MEDIO_PAGO"]);
                    lista_deposito.Add(deposito);
                }
            }

            return lista_deposito;
        }

        public string ObtenerNumeroSerie()
        {
            string numero_serie = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();

            var PARAM_NUMERO_FACTURA = new OracleParameter("NUMERO_FACTURA", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(PARAM_NUMERO_FACTURA);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.CCLGENERAFACTURAABONO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);

            if (PARAM_NUMERO_FACTURA.Value != null && PARAM_NUMERO_FACTURA.Value.ToString() != "")
            {
                numero_serie = Convert.ToString(PARAM_NUMERO_FACTURA.Value);
            }
            else
            {
                numero_serie = "";
            }

            return numero_serie;
        }

        public Task<string> ActualizarEstadoDeposito(DetalleFacturaDeposito detalle_factura_deposito)
        {
            string result = string.Empty;

            List<OracleParameter>  parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("ID_DEPOSITO", OracleDbType.Int32, detalle_factura_deposito.IdDeposito, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_DG_ESTADO_DEPOSITO", OracleDbType.Int32, detalle_factura_deposito.IddgEstadoDeposito, ParameterDirection.Input));

            var pResult = new OracleParameter("P_RESULTADO", OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLUPDSTATEDEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            result = pResult.Value.ToString();

            return Task.FromResult<string>(result);
        }

        public Task<List<FacturaDeposito>> ListarFacturaDeposito()
        {
            FacturaDeposito factura_deposito = null;            
            List<FacturaDeposito> lista_factura_deposito = new List<FacturaDeposito>();    

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLREAFACTURA_DEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    factura_deposito = new FacturaDeposito();
                    factura_deposito.IdProducto = Convert.ToInt32(dr["ID_PRODUCTO"]);
                    factura_deposito.IdFacturaDeposito = Convert.ToInt32(dr["ID_FACTURA_DEPOSITO"]);
                    factura_deposito.MontoTotal = Convert.ToDecimal(dr["DC_MONTO_TOTAL"]);
                    lista_factura_deposito.Add(factura_deposito);                   
                }
            }

            ObtenerDetalleFacturaDeposito(ref lista_factura_deposito);

            return Task.FromResult<List<FacturaDeposito>>(lista_factura_deposito);
        }

        public void ObtenerDetalleFacturaDeposito(ref List<FacturaDeposito> factura_deposito)
        {
            DetalleFacturaDeposito detalle_factura_deposito = null;
            List<DetalleFacturaDeposito> lista_detalle_factura = null;
            List<OracleParameter> parameters = null;

            foreach (FacturaDeposito c in factura_deposito)
            {
                parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("ID_FACTURA_DEPOSITO", OracleDbType.Int32, c.IdFacturaDeposito, ParameterDirection.Input));
                parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLREADETFACTURA_DEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
                {
                    lista_detalle_factura = new List<DetalleFacturaDeposito>();

                    while (dr.Read())
                    {
                        detalle_factura_deposito = new DetalleFacturaDeposito();
                        detalle_factura_deposito.IdDeposito = Convert.ToInt32(dr["ID_DEPOSITO"]);
                        lista_detalle_factura.Add(detalle_factura_deposito);
                    }
                }

                c.DetalleFacturaDeposito = new List<DetalleFacturaDeposito>();
                c.DetalleFacturaDeposito = lista_detalle_factura;
            }            
        }

        public Task<string> RegistrarNotaCredito(NotaCredito nota_credito)
        {
            string result = string.Empty;
            List<OracleParameter> param = new List<OracleParameter>();        
                        
            param.Add(new OracleParameter("ID_PRODUCTO", OracleDbType.Int32, nota_credito.IdProducto, ParameterDirection.Input));
            param.Add(new OracleParameter("NUMERO_NOTA_CREDITO", OracleDbType.NVarchar2, nota_credito.NumeroNotaCredito, ParameterDirection.Input));
            param.Add(new OracleParameter("ID_FACTURA_DEPOSITO", OracleDbType.Int32, nota_credito.IdFacturaDeposito, ParameterDirection.Input));
            param.Add(new OracleParameter("DC_MONTO", OracleDbType.Decimal, nota_credito.DcMonto, ParameterDirection.Input));
            param.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, nota_credito.VcUsuarioCreacion, ParameterDirection.Input));
            param.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, nota_credito.IddgEstado, ParameterDirection.Input));

            var pResult = new OracleParameter("P_RESULTADO", OracleDbType.Int32, ParameterDirection.Output);
            param.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLPOSTNOTA_CREDITO", param, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            result = pResult.Value.ToString();

            return Task.FromResult<string>(result);
        }

        public Task<string> AnularFacturaDeposito(FacturaDeposito factura_deposito)
        {
            string result = string.Empty;
            List<OracleParameter> param = new List<OracleParameter>();

            param.Add(new OracleParameter("ID_FACTURA_DEPOSITO", OracleDbType.Int32, factura_deposito.IdFacturaDeposito, ParameterDirection.Input));
            var pResult = new OracleParameter("P_RESULTADO", OracleDbType.Int32, ParameterDirection.Output);
            param.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCDOCABONO.CCLUPDSTATEFACTURA", param, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            result = pResult.Value.ToString();

            return Task.FromResult<string>(result);
        }
    }
}
