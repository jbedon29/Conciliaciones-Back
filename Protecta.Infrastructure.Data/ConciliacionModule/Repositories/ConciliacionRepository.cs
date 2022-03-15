using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.Infrastructure.Data.Extensions;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;

namespace Protecta.Infrastructure.Data.ConciliacionModule.Repositories
{
    public class ConciliacionRepository : IConciliacionRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public ConciliacionRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<Respuesta> AplicarConciliacionAutomatica(DatosAplicaConciliacion datosAplicacion)
        {
            Respuesta respuesta = new Respuesta();
            string _sMessage = string.Empty;
            int _iRespuesta = 0;

            try
            {
                //Llama al procedimiento            
                List<OracleParameter> parameters = new List<OracleParameter>();
                _sMessage = string.Empty;
                respuesta.Mensaje = _sMessage;

                //int respuesta = 0;
                long _idProducto = datosAplicacion.IdProducto;

                DateTime dfechaDesde = Convert.ToDateTime(datosAplicacion.FechaDesde);
                DateTime dfechaHasta = Convert.ToDateTime(datosAplicacion.FechaHasta);
                //datosConsultaPlanillaDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
                //datosConsultaPlanillaDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

                //string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosAplicacion.FechaDesde);
                //string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosAplicacion.FechaHasta);              
                string _usuario = datosAplicacion.Usuario;
                long _idTipoCobro = datosAplicacion.IdTipoCobro;


                parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DT_FECHA_INICIO", OracleDbType.Date, dfechaDesde, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DT_FECHA_FIN", OracleDbType.Date, dfechaHasta, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_PLANILLAS", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_DEPOSITOS", OracleDbType.NVarchar2, DBNull.Value, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VC_USUARIO", OracleDbType.NVarchar2, _usuario, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VDP_TIPO", OracleDbType.Long, _idTipoCobro, ParameterDirection.Input));



                //Parámetro de Salida
                var pResult = new OracleParameter("P_IN_RESPUESTA", OracleDbType.Int32, ParameterDirection.Output);
                var pResult1 = new OracleParameter("P_VC_RESPUESTA_MENSAJE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                parameters.Add(pResult);
                parameters.Add(pResult1);

                _connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION.CONCILIAR_AUTOMATICA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _iRespuesta = pResult.Value == null ? -1 : Convert.ToInt32(pResult.Value.ToString());
                _sMessage = pResult1.Value == null ? string.Empty : pResult1.Value.ToString();

                respuesta.Resultado = _iRespuesta;
                respuesta.Mensaje = _sMessage;
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
                respuesta.Resultado = -1;
                respuesta.Mensaje = _sMessage;
            }
            
            return Task.FromResult<Respuesta>(respuesta);
        }
        
        public Task<Respuesta> AplicarConciliacionManual(DatosAplicacionManual datosAplicacion)
        {

            Respuesta respuesta = new Respuesta();
            string _sMessage = string.Empty;
            int _iRespuesta = 0;

            try
            {
                //Llama al procedimiento            
                List<OracleParameter> parameters = new List<OracleParameter>();
                _sMessage = string.Empty;
                respuesta.Mensaje = _sMessage;

                long _idProducto = datosAplicacion.IdProducto;
                DateTime _dfechaDesde = Convert.ToDateTime(datosAplicacion.FechaDesde);
                DateTime _dfechaHasta = Convert.ToDateTime(datosAplicacion.FechaHasta);
                string _idPlanillas = datosAplicacion.IdPlanillas;
                string _idDepositos = datosAplicacion.IdDepositos;
                string _usuario = datosAplicacion.Usuario;

                parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DT_FECHA_INICIO", OracleDbType.Date, _dfechaDesde, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_DT_FECHA_FIN", OracleDbType.Date, _dfechaHasta, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_PLANILLAS", OracleDbType.NVarchar2, _idPlanillas, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_ID_DEPOSITOS", OracleDbType.NVarchar2, _idDepositos, ParameterDirection.Input));
                parameters.Add(new OracleParameter("P_VC_USUARIO", OracleDbType.NVarchar2, _usuario, ParameterDirection.Input));                
                //Parámetro de Salida
                var pResult = new OracleParameter("P_IN_RESPUESTA", OracleDbType.Int32, ParameterDirection.Output);
                var pResult1 = new OracleParameter("P_VC_RESPUESTA_MENSAJE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                parameters.Add(pResult);
                parameters.Add(pResult1);

                _connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION.CONCILIAR_MANUAL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _iRespuesta = pResult.Value == null ? -1 : Convert.ToInt32(pResult.Value.ToString());
                _sMessage = pResult1.Value == null ? string.Empty : pResult1.Value.ToString();

                respuesta.Resultado = _iRespuesta;
                respuesta.Mensaje = _sMessage;
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
                respuesta.Resultado = -1;
                respuesta.Mensaje = _sMessage;
            }
            
            return Task.FromResult<Respuesta>(respuesta);
        }

        
        public Task<Respuesta> RevertirConciliacion(long idPlanilla)
        {

            Respuesta respuesta = new Respuesta();
            string _sMessage = string.Empty;
            int _iRespuesta = 0;

            try
            {
                //Llama al procedimiento            
                List<OracleParameter> parameters = new List<OracleParameter>();
                _sMessage = string.Empty;
                respuesta.Mensaje = _sMessage;                                
                                
                parameters.Add(new OracleParameter("P_ID_PLANILLAS", OracleDbType.Int64, idPlanilla, ParameterDirection.Input));

                //Parámetro de Salida
                var pResult = new OracleParameter("P_IN_RESPUESTA", OracleDbType.Int32, ParameterDirection.Output);
                var pResult1 = new OracleParameter("P_VC_RESPP_IN_RESPUESTAUESTA_MENSAJE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                parameters.Add(pResult);
                parameters.Add(pResult1);

                _connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION.REVERTIR_CONCILIACION", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _iRespuesta = pResult.Value == null ? -1 : Convert.ToInt32(pResult.Value.ToString());
                _sMessage = pResult1.Value == null ? string.Empty : pResult1.Value.ToString();

                respuesta.Resultado = _iRespuesta;
                respuesta.Mensaje = _sMessage;
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
                respuesta.Resultado = -1;
                respuesta.Mensaje = _sMessage;
            }

            return Task.FromResult<Respuesta>(respuesta);
        }
    }
}
