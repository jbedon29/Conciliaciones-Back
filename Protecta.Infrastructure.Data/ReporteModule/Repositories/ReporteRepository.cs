using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.ReporteModule.Repositories
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public ReporteRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<ReporteDepositoPendiente>> ReporteDepositosPendientes(DatosReporteConciliacionPendiente datosConsulta)
        {
            ReporteDepositoPendiente entities = null;
            List<ReporteDepositoPendiente> listaEntidades = new List<ReporteDepositoPendiente>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsulta.IdProducto;
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_REPORTES.REP_DEPOSITOS_PENDIENTES", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReporteDepositoPendiente();
                    entities.IdDeposito = Convert.ToInt64(dr["ID_DEPOSITO"]);
                    entities.NumeroOperacion = Convert.ToString(dr["VC_NUMERO_OPERACION"]);
                    entities.Monto = Convert.ToDecimal(dr["DC_MONTO"]);
                    entities.Saldo = Convert.ToDecimal(dr["DC_SALDO"]);
                    entities.FechaDeposito = Convert.ToDateTime(dr["dt_fecha_deposito"]);
                    entities.NombreArchivo = Convert.ToString(dr["VC_NOMBRE_ARCHIVO"]);
                    entities.Cuenta = Convert.ToString(dr["NUMERO_CUENTA"]);
                    entities.Banco = Convert.ToString(dr["VC_NOMBRE"]);
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult<List<ReporteDepositoPendiente>>(listaEntidades);
        }

        public Task<List<ReportePlanillaPendiente>> ReportePlanillasPendientes(DatosReporteConciliacionPendiente datosConsulta)
        {
            ReportePlanillaPendiente entities = null;
            List<ReportePlanillaPendiente> listaPlanillas = new List<ReportePlanillaPendiente>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsulta.IdProducto;

            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_REPORTES.REP_PLANILLAS_PENDIENTES", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReportePlanillaPendiente();
                    entities.IdPlanilla = dr["ID_PLANILLA"] == DBNull.Value ? string.Empty : dr["ID_PLANILLA"].ToString();
                    entities.FechaPlanilla = dr["DT_FECHA_PLANILLA"] == DBNull.Value ? string.Empty : dr["DT_FECHA_PLANILLA"].ToString();
                    entities.Monto = dr["DC_MONTO"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO"].ToString());
                    entities.DescripcionMedioPago = dr["ID_TIPO_MEDIO_PAGO"] == DBNull.Value ? string.Empty : dr["ID_TIPO_MEDIO_PAGO"].ToString();
                    entities.DescripcionMedioPago = dr["DESC_MEDIO_PAGO"] == DBNull.Value ? string.Empty : dr["DESC_MEDIO_PAGO"].ToString();
                    entities.NumeroOperacion = dr["VC_NUMERO_OPERACION"] == DBNull.Value ? string.Empty : dr["VC_NUMERO_OPERACION"].ToString();
                    entities.FechaProceso = dr["dt_fecha_proceso"] == DBNull.Value ? string.Empty : dr["dt_fecha_proceso"].ToString();
                    entities.saldo = dr["DC_SALDO"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_SALDO"].ToString());
                    entities.fechaAbono = dr["DT_FECHA_ABONO"] == DBNull.Value ? string.Empty : dr["DT_FECHA_ABONO"].ToString();
                    entities.comisionDirecta = dr["DC_COMISION_DIRECTA"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_COMISION_DIRECTA"].ToString());
                    entities.comisionIndirecta = dr["DC_COMISION_INDIRECTA"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_COMISION_INDIRECTA"].ToString());
                    entities.comisionTotal = dr["DC_COMISION_TOTAL"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_COMISION_TOTAL"].ToString());
                    entities.IdProducto = dr["ID_PRODUCTO"] == DBNull.Value ? string.Empty : dr["ID_PRODUCTO"].ToString();
                    entities.CodigoCanal = dr["CODIGO_CANAL"] == DBNull.Value ? string.Empty : dr["CODIGO_CANAL"].ToString();
                    entities.DescripcionCanal = dr["DESCRIPCION_CANAL"] == DBNull.Value ? string.Empty : dr["DESCRIPCION_CANAL"].ToString();
                    entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();

                    listaPlanillas.Add(entities);
                }
            }

            return Task.FromResult<List<ReportePlanillaPendiente>>(listaPlanillas);
        }

        public Task<List<ReporteConsultaProcesada>> ConsultarReporteMacro(DatosConsultaReporte datosConsultaReporteMacro)
        {
            ReporteConsultaProcesada entities = null;
            List<ReporteConsultaProcesada> listaReportesMacro = new List<ReporteConsultaProcesada>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsultaReporteMacro.IdProducto;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteMacro.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteMacro.FechaHasta);

            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PLANILLA_MACRO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReporteConsultaProcesada();

                    entities.numero_operacion = dr["VC_NUMERO_OPERACION"] == DBNull.Value ? string.Empty : dr["VC_NUMERO_OPERACION"].ToString();
                    entities.fecha_conciliacion = dr["DT_FECHA_CONCILIACION"] == DBNull.Value ? string.Empty : dr["DT_FECHA_CONCILIACION"].ToString();
                    //entities.numero_operacion = Convert.ToString(dr["VC_NUMERO_OPERACION"].ToString());
                    //entities.fecha_conciliacion = Convert.ToString(dr["DT_FECHA_CONCILIACION"].ToString());
                    entities.monto_bruto_dep = dr["DC_MONTO_BRUTO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_BRUTO_DEP"].ToString());
                    entities.monto_neto_dep = dr["DC_MONTO_NETO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_NETO_DEP"].ToString());
                    entities.monto_comis_dep = dr["DC_MONTO_COMIS_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_COMIS_DEP"].ToString());
                    entities.monto_ocargo_dep = dr["DC_MONTO_OCARGO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_OCARGO_DEP"].ToString());
                    entities.deposito = dr["ID_DEPOSITO"] == DBNull.Value ? string.Empty : dr["ID_DEPOSITO"].ToString();
                    entities.deposito_archivo = dr["ID_DEPOSITO_ARCHIVO"] == DBNull.Value ? string.Empty : dr["ID_DEPOSITO_ARCHIVO"].ToString();
                    entities.monto = dr["DC_MONTO"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO"].ToString());
                    entities.saldo = dr["DC_SALDO"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_SALDO"].ToString());
                    entities.nombre_archivo = dr["VC_NOMBRE_ARCHIVO"] == DBNull.Value ? string.Empty : dr["VC_NOMBRE_ARCHIVO"].ToString();
                    entities.fecha_deposito = dr["DT_FECHA_DEPOSITO"] == DBNull.Value ? string.Empty : dr["DT_FECHA_DEPOSITO"].ToString();
                    entities.diferencia = dr["DIF"] == DBNull.Value ? string.Empty : dr["DIF"].ToString();
                    entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();

                    listaReportesMacro.Add(entities);
                }
            }

            return Task.FromResult<List<ReporteConsultaProcesada>>(listaReportesMacro);
        }

        public Task<List<ReportePendienteImportarProcesada>> ReportePendientesPorImportar()
        {
            ReportePendienteImportarProcesada entities = null;
            List<ReportePendienteImportarProcesada> listaReportePendientesPorImportar = new List<ReportePendienteImportarProcesada>();
            List<OracleParameter> parameters = new List<OracleParameter>();


            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PLANILLA_IMPORTAR_MACRO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReportePendienteImportarProcesada();
                    entities.planilla = Convert.ToInt32(dr["PLANILLA"]);
                    entities.fecha_registro = dr["FECHA_REGISTRO"] == DBNull.Value ? string.Empty : dr["FECHA_REGISTRO"].ToString();
                    entities.cantidad = Convert.ToInt32(dr["NQUANTITY"]);
                    entities.monto_total = dr["NAMOUNTTOTAL"] == DBNull.Value ? 0 : Decimal.Parse(dr["NAMOUNTTOTAL"].ToString());
                    entities.error_planilla = dr["ERROR_EXISTE_PLANILLA"] == DBNull.Value ? string.Empty : dr["ERROR_EXISTE_PLANILLA"].ToString();
                    entities.error_plataforma_digital = dr["ERROR_PLATAFORMA_DIGITAL"] == DBNull.Value ? string.Empty : dr["ERROR_PLATAFORMA_DIGITAL"].ToString();
                    entities.fecha_error_plataforma = dr["FECHA_ERROR_PLATAFORMA"] == DBNull.Value ? string.Empty : dr["FECHA_ERROR_PLATAFORMA"].ToString();
                    entities.cod_producto = Convert.ToInt32(dr["COD_PRODUCTO"]);
                    entities.des_producto = dr["DES_PRODUCTO"] == DBNull.Value ? string.Empty : dr["DES_PRODUCTO"].ToString();
                    entities.medio_pago = dr["MEDIO_PAGO"] == DBNull.Value ? string.Empty : dr["MEDIO_PAGO"].ToString();
                    entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();



                    listaReportePendientesPorImportar.Add(entities);
                }
            }

            return Task.FromResult<List<ReportePendienteImportarProcesada>>(listaReportePendientesPorImportar);
        }


        public Task<List<ReporteComprobantePlanillaProcesada>> ReporteComprobantesPorPlanilla(DatosConsultaReporteComprobante datosConsultaReporteComprobantesPorPlanilla)
        {
            ReporteComprobantePlanillaProcesada entities = null;
            List<ReporteComprobantePlanillaProcesada> listaReportesComprobantesPorPlanilla = new List<ReporteComprobantePlanillaProcesada>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsultaReporteComprobantesPorPlanilla.IdProducto;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteComprobantesPorPlanilla.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteComprobantesPorPlanilla.FechaHasta);

            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_COMP_X_PLANILLA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReporteComprobantePlanillaProcesada();

                    entities.poliza = dr["POLIZA"] == DBNull.Value ? string.Empty : dr["POLIZA"].ToString();

                    entities.nro_planilla = dr["NRO_PLANILLA"] == DBNull.Value ? string.Empty : dr["NRO_PLANILLA"].ToString();
                    entities.estado_planilla = dr["ID_DG_ESTADO_PLANILLA"] == DBNull.Value ? string.Empty : dr["ID_DG_ESTADO_PLANILLA"].ToString();
                    entities.monto_total_planilla = dr["MONTO_TOTAL_PLANILLA"] == DBNull.Value ? 0 : Decimal.Parse(dr["MONTO_TOTAL_PLANILLA"].ToString());
                    entities.comision = dr["COMISION"] == DBNull.Value ? 0 : Decimal.Parse(dr["COMISION"].ToString());
                    entities.cod_producto = dr["COD_PRODUCTO"] == DBNull.Value ? string.Empty : dr["COD_PRODUCTO"].ToString();
                    entities.des_producto = dr["DES_PRODUCTO"] == DBNull.Value ? string.Empty : dr["DES_PRODUCTO"].ToString();
                    entities.nro_operacion = dr["NRO_OPERACION"] == DBNull.Value ? string.Empty : dr["NRO_OPERACION"].ToString();
                    entities.recibo = dr["RECIBO"] == DBNull.Value ? string.Empty : dr["RECIBO"].ToString();
                    entities.tipo_comprobante = dr["TIPO_COMPROBANTE"] == DBNull.Value ? string.Empty : dr["TIPO_COMPROBANTE"].ToString();
                    entities.nro_comprobante = dr["NRO_COMPROBANTE"] == DBNull.Value ? string.Empty : dr["NRO_COMPROBANTE"].ToString();
                    entities.monto_bruto_comp = dr["MONTO_BRUTO_COMPROBANTE"] == DBNull.Value ? 0 : Decimal.Parse(dr["MONTO_BRUTO_COMPROBANTE"].ToString());
                    entities.monto_neto_comp = dr["MONTO_NETO_COMPROBANTE"] == DBNull.Value ? 0 : Decimal.Parse(dr["MONTO_NETO_COMPROBANTE"].ToString());
                    entities.igv_comp = dr["IGV_COMPROBANTE"] == DBNull.Value ? 0 : Decimal.Parse(dr["IGV_COMPROBANTE"].ToString());
                    entities.derecho_emision = dr["DERECHO_EMISION_COMPROBANTE"] == DBNull.Value ? 0 : Decimal.Parse(dr["DERECHO_EMISION_COMPROBANTE"].ToString());
                    entities.id_tipo_medio_pago = dr["ID_TIPO_MEDIO_PAGO"] == DBNull.Value ? string.Empty : dr["ID_TIPO_MEDIO_PAGO"].ToString();
                    entities.des_medio_pago = dr["DES_MEDIO_PAGO"] == DBNull.Value ? string.Empty : dr["DES_MEDIO_PAGO"].ToString();
                    entities.factura = dr["NBILLSTAT"] == DBNull.Value ? string.Empty : dr["NBILLSTAT"].ToString();
                    entities.estado_comp = dr["ESTADO_COMPROBANTE"] == DBNull.Value ? string.Empty : dr["ESTADO_COMPROBANTE"].ToString();
                    entities.fecha_liquidacion_comp = dr["FECHA_LIQUDIACION_COMPROBANTE"] == DBNull.Value ? string.Empty : dr["FECHA_LIQUDIACION_COMPROBANTE"].ToString();
                    entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();

                    listaReportesComprobantesPorPlanilla.Add(entities);
                }
            }

            return Task.FromResult<List<ReporteComprobantePlanillaProcesada>>(listaReportesComprobantesPorPlanilla);
        }
        // add for PA 
        public Task<List<ReporteListadoError>> ReporteListadoConError(DatosConsultaReporteListado datosConsultaReporteListado)
        {
            ReporteListadoError entities = null;
            List<ReporteListadoError> listaReportesListado = new List<ReporteListadoError>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteListado.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaReporteListado.FechaHasta);

            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, datosConsultaReporteListado.idProducto, ParameterDirection.Input));

            parameters.Add(new OracleParameter("P_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_REPORTES.REP_REGLA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new ReporteListadoError();
                    entities.planilla = dr["PLANILLA"] == DBNull.Value ? string.Empty : dr["PLANILLA"].ToString();
                    entities.numero_operacion = dr["OPERACION"] == DBNull.Value ? string.Empty : dr["OPERACION"].ToString();
                    entities.monto = dr["MONTO"] == DBNull.Value ? 0 : Decimal.Parse(dr["MONTO"].ToString());
                    entities.fecha_deposito = dr["FECHA_OPERACION"] == DBNull.Value ? string.Empty : dr["FECHA_OPERACION"].ToString();
                    entities.banco = dr["BANCO"] == DBNull.Value ? string.Empty : dr["BANCO"].ToString();
                    entities.numero_cuenta = dr["NRO_CUENTA"] == DBNull.Value ? string.Empty : dr["NRO_CUENTA"].ToString();
                    entities.valor = dr["MENSAJE"] == DBNull.Value ? string.Empty : dr["MENSAJE"].ToString();



                    //entities.fecha_conciliacion = dr["DT_FECHA_CONCILIACION"] == DBNull.Value ? string.Empty : dr["DT_FECHA_CONCILIACION"].ToString();
                    ////entities.numero_operacion = Convert.ToString(dr["VC_NUMERO_OPERACION"].ToString());
                    ////entities.fecha_conciliacion = Convert.ToString(dr["DT_FECHA_CONCILIACION"].ToString());
                    //entities.monto_bruto_dep = dr["DC_MONTO_BRUTO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_BRUTO_DEP"].ToString());
                    //entities.monto_neto_dep = dr["DC_MONTO_NETO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_NETO_DEP"].ToString());
                    //entities.monto_comis_dep = dr["DC_MONTO_COMIS_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_COMIS_DEP"].ToString());
                    //entities.monto_ocargo_dep = dr["DC_MONTO_OCARGO_DEP"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_MONTO_OCARGO_DEP"].ToString());
                    //entities.deposito = dr["ID_DEPOSITO"] == DBNull.Value ? string.Empty : dr["ID_DEPOSITO"].ToString();
                    //entities.deposito_archivo = dr["ID_DEPOSITO_ARCHIVO"] == DBNull.Value ? string.Empty : dr["ID_DEPOSITO_ARCHIVO"].ToString();
                    //entities.saldo = dr["DC_SALDO"] == DBNull.Value ? 0 : Decimal.Parse(dr["DC_SALDO"].ToString());
                    //entities.nombre_archivo = dr["VC_NOMBRE_ARCHIVO"] == DBNull.Value ? string.Empty : dr["VC_NOMBRE_ARCHIVO"].ToString();
                    //entities.diferencia = dr["DIF"] == DBNull.Value ? string.Empty : dr["DIF"].ToString();
                    listaReportesListado.Add(entities);
                }
            }

            return Task.FromResult<List<ReporteListadoError>>(listaReportesListado);
        }

        public Task<List<ReporteListadoError>> OperacionDetalle(DatosDetallePlanilla datosDetallePlanilla)
        {
            ReporteListadoError entities = null;
            List<ReporteListadoError> listReportes = new List<ReporteListadoError>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_OPERACION", OracleDbType.NVarchar2, datosDetallePlanilla.numeroOperacion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_INI", OracleDbType.NVarchar2, datosDetallePlanilla.fecha_ini, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, datosDetallePlanilla.fecha_fin, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_MONTO", OracleDbType.NVarchar2, datosDetallePlanilla.monto, ParameterDirection.Input));

            //int x = datosDetallePlanilla.valor.IndexOf("cuenta");//mg
            //bool nmonto1 = false;
            //decimal smonto1 = 0;

            bool nmonto2 = false;
            decimal smonto2 = 0;
            /* string vvalor2 = "cuenta";
             bool svalor2 = entities.valor.Contains(vvalor2);
             */
            nmonto2 = decimal.TryParse(datosDetallePlanilla.monto, out smonto2);

            parameters.Add(new OracleParameter("P_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_REPORTES.LIST_BUSCA_POR_OPE", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {

                while (dr.Read())
                {

                    entities = new ReporteListadoError();
                    entities.numero_operacion = dr["NRO_OPERACION"] == DBNull.Value ? string.Empty : dr["NRO_OPERACION"].ToString();
                    entities.monto = dr["MONTO"] == DBNull.Value ? 0 : Decimal.Parse(dr["MONTO"].ToString());
                    entities.saldo = dr["SALDO"] == DBNull.Value ? 0 : Decimal.Parse(dr["SALDO"].ToString());

                    entities.fecha_deposito = dr["FECHA"] == DBNull.Value ? string.Empty : dr["FECHA"].ToString();
                    entities.banco = dr["BANCO"] == DBNull.Value ? string.Empty : dr["BANCO"].ToString();
                    entities.numero_cuenta = dr["NRO_CUENTA"] == DBNull.Value ? string.Empty : dr["NRO_CUENTA"].ToString();
                    // entities.valor = dr["MENSAJE"] == DBNull.Value ? string.Empty : dr["MENSAJE"].ToString();
                    entities.isValid = true;
                    bool svalor = datosDetallePlanilla.valor.Contains("monto");
                    bool svalor2 = datosDetallePlanilla.valor.Contains("cuenta ");

                    if (svalor)
                        entities.isValid = false;

                    if (svalor2) entities.isValid = false;
                    /* string vvalor = "monto";
                     *///nmonto1 = decimal.TryParse(entities.monto, out smonto1);

                    //if ((entities.fecha_deposito == datosDetallePlanilla.fechadeposito) && (entities.numero_operacion == datosDetallePlanilla.numeroOperacion))
                    // if (entities.numero_cuenta == datosDetallePlanilla.numeroCuenta && entities.monto == smonto2)
                    //  if (svalor)

                    //if (x > 0) entities.isValid = false;
                    //
                    listReportes.Add(entities);
                }
            }

            return Task.FromResult<List<ReporteListadoError>>(listReportes);
        }

        public Task<Mensaje> ActualizarPlanilla(DatosPlanilla datosPlanilla)
        {
            Mensaje datos = new Mensaje();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_PLANILLA", OracleDbType.Long, datosPlanilla.Planilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_PROCESO", OracleDbType.NVarchar2, datosPlanilla.FechaDeposito, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NRO_OPERACION", OracleDbType.NVarchar2, datosPlanilla.NumeroOperacion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NRO_OPERACION_D", OracleDbType.NVarchar2, datosPlanilla.NumeroOperacionDetalle, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.NVarchar2, datosPlanilla.Usuario, ParameterDirection.Input));
            //Parámetro de Salida
            var pCode = new OracleParameter("P_NCODE", OracleDbType.Int64, 32767, OracleDbType.Int64, ParameterDirection.Output);
            var p_Message = new OracleParameter("P_SMESSAGE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            //p_Message.Size = 2500;

            parameters.Add(pCode);
            parameters.Add(p_Message);

            _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_MOD_PLANILLA", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            datos.ncode = pCode.Value.ToString();
            datos.mensaje = p_Message.Value.ToString() == "null" ? "" : p_Message.Value.ToString();
            return Task.FromResult<Mensaje>(datos);
        }
        public Task<Mensaje> ComunicarPlanilla(DatosPlanillaRechazar datosPlanilla)
        {
            Mensaje datos = new Mensaje();
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_PLANILLA", OracleDbType.Long, datosPlanilla.Planilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NUSERCODE", OracleDbType.NVarchar2, datosPlanilla.Usuario, ParameterDirection.Input));
            var P_EMAIL = new OracleParameter("P_EMAIL", OracleDbType.NVarchar2, ParameterDirection.Output);
            var pCode = new OracleParameter("P_NCODE", OracleDbType.Int16, ParameterDirection.Output);
            var p_Message = new OracleParameter("P_MESSAGE", OracleDbType.NVarchar2, ParameterDirection.Output);
            p_Message.Size = 2500;
            P_EMAIL.Size = 2500;
            parameters.Add(P_EMAIL);
            //parameters.Add(P_ERROR_OPE);
            //parameters.Add(P_ERROR_FECH);
            //parameters.Add(P_ERROR_OTRO);
            parameters.Add(pCode);
            parameters.Add(p_Message);
            _connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.SPS_RECHAZAR_PLANILLA", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            datos.ncode = pCode.Value.ToString();
            datos.mensaje = p_Message.Value.ToString() == "null" ? "" : p_Message.Value.ToString();
            datos.nemail = P_EMAIL.Value.ToString() == "null" ? null : P_EMAIL.Value.ToString();
            //datos.val_ope = int.Parse(P_ERROR_OPE.Value.ToString());
            return Task.FromResult<Mensaje>(datos);
        }

        public Task<List<ReporteExtracto>> ReporteExtractoBancario(DatosConsultaExtracto datosConsultaExtracto)
        {
            ReporteExtracto entities = null;
            List<ReporteExtracto> listaReportesExtracto = new List<ReporteExtracto>();
            List<OracleParameter> parameters = new List<OracleParameter>();
            long _idBanco = datosConsultaExtracto.IdBanco;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaExtracto.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaExtracto.FechaHasta);

            parameters.Add(new OracleParameter("P_BANCO", OracleDbType.Long, _idBanco, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, datosConsultaExtracto.idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_LISTA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_SCTR_COBRANZAS.REP_EXTRACTO_BANCARIO", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    entities = new ReporteExtracto();
                    //entities.fecha_abon = dr["FECHA_ABONO"] == DBNull.Value ? string.Empty : dr["FECHA_ABONO"].ToString();
                    entities.fecha_abon = dr["FECHA_ABONO"] == DBNull.Value ? string.Empty : dr["FECHA_ABONO"].ToString();
                    entities.concepto = dr["CONCEPTO"] == DBNull.Value ? string.Empty : dr["CONCEPTO"].ToString();
                    entities.banco = dr["BANCO"] == DBNull.Value ? string.Empty : dr["BANCO"].ToString();
                    entities.numero_cuenta = dr["NRO_CUENTA"] == DBNull.Value ? string.Empty : dr["NRO_CUENTA"].ToString();
                    entities.operacion = dr["NRO_OPERACION"] == DBNull.Value ? string.Empty : dr["NRO_OPERACION"].ToString();
                    entities.monto_xaplicar = dr["POR_APLICAR"] == DBNull.Value ? 0 : Decimal.Parse(dr["POR_APLICAR"].ToString());
                    entities.monto_aplicado = dr["APLICADO"] == DBNull.Value ? 0 : Decimal.Parse(dr["APLICADO"].ToString());
                    entities.saldo = dr["SALDO"] == DBNull.Value ? 0 : Decimal.Parse(dr["SALDO"].ToString());
                    entities.moneda = dr["MONEDA"] == DBNull.Value ? string.Empty : dr["MONEDA"].ToString();

                    listaReportesExtracto.Add(entities);
                }
            }
            return Task.FromResult<List<ReporteExtracto>>(listaReportesExtracto);
        }
    }
}
