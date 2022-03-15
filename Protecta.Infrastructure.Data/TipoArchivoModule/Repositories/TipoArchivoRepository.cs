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
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;

namespace Protecta.Infrastructure.Data.TipoArchivoModule.Repositories
{
    public class TipoArchivoRepository : ITipoArchivoRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public TipoArchivoRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<TipoArchivo>> ListarTipoArchivo()
        {
            TipoArchivo tipoarchivoEntity;
            List<TipoArchivo> tipoarchivoList = new List<TipoArchivo>();

            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREATIPO_ARCHIVO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    tipoarchivoEntity = new TipoArchivo();
                    tipoarchivoEntity.IdTipoarchivo = Convert.ToInt32(dr["ID_TIPO_ARCHIVO"]);
                    tipoarchivoEntity.VcDescripcion = dr["VC_DESCRIPCION"].ToString();
                    tipoarchivoList.Add(tipoarchivoEntity);
                }
            }

            return Task.FromResult<List<TipoArchivo>>(tipoarchivoList);
        }
    }
}
