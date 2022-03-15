using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Transactions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg;

namespace Protecta.Infrastructure.Data.ProcesoModule.Repositories
{
    public class ProcesoRepository : IProceso
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public ProcesoRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public int RegistrarLog(ProcesoGeneral procesoGeneral)
        {
            int _pValor = 0;

            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter("ID_PROCESO", OracleDbType.Int32, procesoGeneral.IdProceso, ParameterDirection.Input));
                parameters.Add(new OracleParameter("VC_DESCRIPCION", OracleDbType.NVarchar2, 50, procesoGeneral.VcDescripcion, ParameterDirection.Input));
                parameters.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, 20, procesoGeneral.VcUsuariocreacion, ParameterDirection.Input));
                parameters.Add(new OracleParameter("VC_MENSAJE", OracleDbType.NVarchar2, 255, procesoGeneral.VcMensaje, ParameterDirection.Input));
                parameters.Add(new OracleParameter("VC_AMBITO", OracleDbType.NVarchar2, 10, procesoGeneral.VcAmbito, ParameterDirection.Input));
                //Parámetro de Salida
                var pNextVal = new OracleParameter("P_NEXTVAL", OracleDbType.Int32, ParameterDirection.Output);
                parameters.Add(pNextVal);

                _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCPROCESO.CCLPOSTPROCESO_GENERAL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);

                _pValor = Convert.ToInt32(pNextVal.Value.ToString());
            }
            catch (Exception ex)
            {
                _pValor = 0;
            }

            return _pValor;
        }

        public void RegistrarLog(LogProcesoGeneral logProcesoGeneral)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("ID_PROCESO_GENERAL", OracleDbType.Int32, logProcesoGeneral.IdProcesoGeneral, ParameterDirection.Input));
            parameters.Add(new OracleParameter("VC_MENSAJE", OracleDbType.NVarchar2, 255, logProcesoGeneral.VcMensaje, ParameterDirection.Input));
            parameters.Add(new OracleParameter("VC_AMBITO", OracleDbType.NVarchar2, 10, logProcesoGeneral.VcAmbito, ParameterDirection.Input));
            parameters.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, 20, logProcesoGeneral.VcUsuarioCreacion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output));

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCPROCESO.CCLUPDLOGPROCESO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
        }
    }
}
