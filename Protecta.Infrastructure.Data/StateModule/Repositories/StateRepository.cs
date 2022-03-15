using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.StateModule.Aggregates.StateAgg;
using Protecta.Infrastructure.Connection;
using Protecta.Infrastructure.Data.Extensions;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Protecta.Infrastructure.Data.StateModule.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public StateRepository(IOptions<AppSettings> appSettings,
                               IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<IEnumerable<State>> GetState()
        {
            IEnumerable<State> entities = null;
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("PKG_PAYROLL.PA_SEL_STATE", parameters))
            {
                entities = dr.ReadRows<State>();
            }

            return Task.FromResult<IEnumerable<State>>(entities);

        }
    }
}
