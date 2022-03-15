using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.MonedaModule.Aggregates.MonedaAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.MonedaModule.Repositories
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public MonedaRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Moneda>> ListarMoneda()
        {
            Moneda entities = null;
            List<Moneda> listaEntidades = new List<Moneda>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_MONEDA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Moneda();
                    entities.IdMoneda = Convert.ToInt64(dr["ID_MONEDA"]);
                    entities.Codigo = Convert.ToString(dr["VC_CODIGO"]);
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult<List<Moneda>>(listaEntidades);
        }
    }
}
