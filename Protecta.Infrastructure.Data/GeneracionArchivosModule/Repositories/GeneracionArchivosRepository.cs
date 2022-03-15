using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Protecta.Infrastructure.Data.GeneracionArchivosModule.Repositories
{
    public class GeneracionArchivosRepository : IGeneracionArchivosRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public GeneracionArchivosRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            this._connectionBase = ConnectionBase;
        }

        public Task<List<DatosRespuestaGeneracionArchivo>> ConsultarArchivos(DatosConsultaGenerarArchivos datosConsulta)
        {
            var result = new DatosProcesoGeneracionArchivo();
            var listaInformacion = new List<DatosRespuestaGeneracionArchivo>();

            ConsultaProcesaArchivos(datosConsulta, false, out listaInformacion, out result);

            return Task.FromResult<List<DatosRespuestaGeneracionArchivo>>(listaInformacion);
        }

        public Task<int> ObtenerDiasPermitidos()
        {
            List<OracleParameter> parameters = new List<OracleParameter>();

            //var P_DC_DIAS = new OracleParameter("P_DC_DIAS", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            var P_DC_DIAS = new OracleParameter("P_DC_DIAS", OracleDbType.Decimal, ParameterDirection.Output);
            parameters.Add(P_DC_DIAS);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.PKG_GENERAR_ARCHIVOS.SP_SEL_DIAS_PERMITIDOS", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

            var oDecimal = (OracleDecimal)P_DC_DIAS.Value;

            return Task.FromResult<int>(oDecimal.ToInt32());
        }

        public Task<DatosProcesoGeneracionArchivo> ProcesarArchivos(DatosConsultaGenerarArchivos datosConsulta)
        {
            var result = new DatosProcesoGeneracionArchivo();
            var listaInformacion = new List<DatosRespuestaGeneracionArchivo>();

            ConsultaProcesaArchivos(datosConsulta, true, out listaInformacion, out result);

            return Task.FromResult<DatosProcesoGeneracionArchivo>(result);
        }

        private void ConsultaProcesaArchivos(
            DatosConsultaGenerarArchivos datosConsulta,
            bool processData,
            out List<DatosRespuestaGeneracionArchivo> listaInformacion,
            out DatosProcesoGeneracionArchivo result)
        {
            string NameProcedure = "INSUDB.PKG_CONCILIACION_PROCESAR_AGD.USP_VistaPreviaConciliacion"; 
            listaInformacion = new List<DatosRespuestaGeneracionArchivo>();
            result = new DatosProcesoGeneracionArchivo();

            List<OracleParameter> parameters = new List<OracleParameter>();
            DatosRespuestaGeneracionArchivo item = null;

            int sendCommit = processData ? 1 : 0;

            parameters.Add(new OracleParameter("P_VC_FECHA_DESDE", OracleDbType.NVarchar2, datosConsulta.fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_VC_FECHA_HASTA", OracleDbType.NVarchar2, datosConsulta.fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_VC_FECHA_GENERACION", OracleDbType.NVarchar2, datosConsulta.fechaGeneracion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_IN_COMMIT", OracleDbType.Decimal, sendCommit, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_VC_USUARIO", OracleDbType.NVarchar2, "", ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Int32, datosConsulta.idProducto, ParameterDirection.Input));

            if (sendCommit == 1)
            {               
                parameters.Add(new OracleParameter("P_ID_PLANILLAS", OracleDbType.NVarchar2, datosConsulta.id_planillas, ParameterDirection.Input));
                NameProcedure = "INSUDB.PKG_CONCILIACION_PROCESAR_AGD.USP_ProcesarConciliacion";
            }

            var P_ID_RESULTADO = new OracleParameter("P_ID_RESULTADO", OracleDbType.Decimal, ParameterDirection.Output);
            parameters.Add(P_ID_RESULTADO);
            var P_VC_MENSAJE = new OracleParameter("P_VC_MENSAJE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(P_VC_MENSAJE);
            
            parameters.Add(new OracleParameter("C_LIQTABLE", OracleDbType.RefCursor, ParameterDirection.Output));            
            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure(NameProcedure, parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                if (processData)
                {
                    result.success = P_ID_RESULTADO.Value.ToString() == "null" ? false : Convert.ToInt32(P_ID_RESULTADO.Value.ToString()) == 1;
                    //result.message = P_VC_MENSAJE.Value.ToString() == "null" ? String.Empty : P_VC_MENSAJE.Value.ToString();
                    result.message = result.success == true ? "se procesó correctamente" : "ERROR: !!!No se procesó correctamente!!!";
                    //result.success = ((OracleDecimal)parameters[6].Value).ToInt32() == 1;                    
                    //result.message = result.success == true ? "se procesó correctamente" : "ERROR: !!!No se procesó correctamente!!!";
                }
                else
                {
                    int i_id_planilla= dr.GetOrdinal("id_planilla");
                    int i_vc_numero_operacion = dr.GetOrdinal("Numero_Operacion");//
                    int i_dt_fecha_conciliacion = dr.GetOrdinal("Dt_fecha_deposito");
                    int i_dc_monto_bruto_dep = dr.GetOrdinal("Monto_bruto");//dc_monto_bruto_dep
                    int i_dc_monto_neto_dep = dr.GetOrdinal("Monto_neto");//dc_monto_neto_dep
                    //int i_dc_monto_comis_dep = dr.GetOrdinal("Comision");//
                    //int i_dc_monto_ocargo_dep = dr.GetOrdinal("Prima_Neto");

                    int i_dc_monto_comis_dep = dr.GetOrdinal("Comision_directa");//
                    int i_dc_monto_ocargo_dep = dr.GetOrdinal("Comision_indirecta");

                    int i_id_deposito = dr.GetOrdinal("id_deposito");//
                    int i_id_deposito_archivo = dr.GetOrdinal("Id_deposito_archivo");//
                    int i_dc_monto = dr.GetOrdinal("Dc_monto_deposito");//
                    int i_dc_saldo = dr.GetOrdinal("Dc_saldo_deposito");//
                    //int i_vc_nombre_archivo = dr.GetOrdinal("Vc_nombre_archivo");//
                    int i_dt_fecha_deposito = dr.GetOrdinal("Dt_fecha_deposito");//
                    int i_dif = dr.GetOrdinal("Diferencia");//

                    int i_tipo_movimiento = dr.GetOrdinal("Tipo_movimiento");//                    
                    int i_banco = dr.GetOrdinal("Banco");
                    int i_numero_cuenta = dr.GetOrdinal("Numero_cuenta");
                    int i_id_dg_estado_planilla = dr.GetOrdinal("id_dg_estado_planilla");

                    while (dr.Read())
                    {
                        item = new DatosRespuestaGeneracionArchivo();

                        if (!dr.IsDBNull(i_id_planilla)) item.id_planilla = dr.GetInt32(i_id_planilla);
                        if (!dr.IsDBNull(i_vc_numero_operacion)) item.numeroOperacion = dr.GetString(i_vc_numero_operacion);
                        if (!dr.IsDBNull(i_dt_fecha_conciliacion)) item.fechaConciliacion = dr.GetDateTime(i_dt_fecha_conciliacion).ToString("dd/MM/yyyy");
                        if (!dr.IsDBNull(i_dc_monto_bruto_dep)) item.montoBruto = dr.GetDecimal(i_dc_monto_bruto_dep);
                        if (!dr.IsDBNull(i_dc_monto_neto_dep)) item.montoNeto = dr.GetDecimal(i_dc_monto_neto_dep);
                        if (!dr.IsDBNull(i_dc_monto_comis_dep)) item.comisionDirecta = dr.GetDecimal(i_dc_monto_comis_dep);
                        if (!dr.IsDBNull(i_dc_monto_ocargo_dep)) item.comisionIndirecta = dr.GetDecimal(i_dc_monto_ocargo_dep);
                        if (!dr.IsDBNull(i_id_deposito)) item.idDeposito = dr.GetString(i_id_deposito);
                        if (!dr.IsDBNull(i_id_deposito_archivo)) item.idDepositoArchivo = dr.GetString(i_id_deposito_archivo);
                        if (!dr.IsDBNull(i_dc_monto)) item.montoDeposito = dr.GetDecimal(i_dc_monto);
                        if (!dr.IsDBNull(i_dc_saldo)) item.saldoDeposito = dr.GetDecimal(i_dc_saldo);
                        //if (!dr.IsDBNull(i_vc_nombre_archivo)) item.nombreArchivo = dr.GetString(i_vc_nombre_archivo);
                        if (!dr.IsDBNull(i_dt_fecha_deposito)) item.fechaDeposito = dr.GetDateTime(i_dt_fecha_deposito).ToString("dd/MM/yyyy");

                        if (!dr.IsDBNull(i_tipo_movimiento)) item.tipoMovimiento = dr.GetString(i_tipo_movimiento);
                        if (!dr.IsDBNull(i_banco)) item.banco = dr.GetString(i_banco);
                        if (!dr.IsDBNull(i_numero_cuenta)) item.numeroCuenta = dr.GetString(i_numero_cuenta);
                        if (!dr.IsDBNull(i_id_dg_estado_planilla)) item.id_dg_estado_planilla = dr.GetString(i_id_dg_estado_planilla);

                        listaInformacion.Add(item);
                    }
                }
                               
            }
        }
    }
}
