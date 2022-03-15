using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg;
using Protecta.Infrastructure.Connection;
using Protecta.Infrastructure.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.EntidadModule.Repositories
{
    public class EntidadRepository : IEntidadRepository
    {

        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public EntidadRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Entidad>> ListarEntidades()
        {
            Entidad entities = null;
            List<Entidad> listaEntidades = new List<Entidad>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_BANCO", parameters,ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Entidad();
                    //entities = dr.ReadFields<Entidad>();
                    entities.ID_ENTIDAD = Convert.ToInt64(dr["ID_ENTIDAD"]);
                    entities.VC_NOMBRE = Convert.ToString(dr["VC_NOMBRE"]);
                    listaEntidades.Add(entities);
                }                
            }

            return Task.FromResult<List<Entidad>>(listaEntidades);
        }

        public Task<List<Cuenta>> ListarCuentaxEntidad(long idEntidad)
        {
            Cuenta entities = null;
            List<Cuenta> listaCuentas = new List<Cuenta>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_ID_ENTIDAD", OracleDbType.Long, idEntidad, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_CUENTA_X_ENTIDAD", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Cuenta();
                    //entities = dr.ReadFields<Cuenta>();
                    entities.ID_CUENTA = Convert.ToInt64(dr["ID_CUENTA"]);
                    entities.ID_ENTIDAD = Convert.ToInt64(dr["ID_ENTIDAD"]);
                    entities.ID_MONEDA = Convert.ToInt64(dr["ID_MONEDA"]);
                    entities.NUMERO_CUENTA = Convert.ToString(dr["NUMERO_CUENTA"]);
                    entities.CODIGO_MONEDA = Convert.ToString(dr["VC_CODIGO"]);

                    listaCuentas.Add(entities);
                }               
            }

            return Task.FromResult<List<Cuenta>>(listaCuentas);
        }        
    }
}
