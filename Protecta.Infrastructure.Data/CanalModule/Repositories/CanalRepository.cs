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
using Protecta.Domain.Service.CanalModule.Aggregates.CanalAgg;

namespace Protecta.Infrastructure.Data.CanalModule.Repositories
{
    public class CanalRepository : ICanalRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public CanalRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Canal>> ListarCanal()
        {
            Canal canalEntity;
            List<Canal> canalList = new List<Canal>();

            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.PKG_CCL_GENERAL.INSREAPV_BACK_CHANNEL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    canalEntity = new Canal();
                    canalEntity.IdCanal = Convert.ToInt64(dr["NCODCHANNEL"]);
                    canalEntity.DescripcionCanal = Convert.ToString(dr["NCODCHANNEL"]) + " - " + dr["SDESCHANNEL"].ToString();
                    canalList.Add(canalEntity);
                }
            }

            return Task.FromResult<List<Canal>>(canalList);
        }
    }
}
