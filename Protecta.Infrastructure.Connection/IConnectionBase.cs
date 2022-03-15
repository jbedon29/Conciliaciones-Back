using System.Collections.Generic;
using System.Data.Common;

namespace Protecta.Infrastructure.Connection
{
    public interface IConnectionBase
    {
        DbParameterCollection ParamsCollectionResult
        {
            get;
            set;
        }

        DbConnection ConnectionGet(ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.OracleCanalP);

        DbDataReader ExecuteByStoredProcedure(
            string nameStore, 
            //ref DbParameterCollection z, 
            IEnumerable<DbParameter> parameters = null,
            ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.OracleCanalP,
            ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteReader
            );
        DbDataReader ExecuteByStoredProcedureCupon(
            string nameStore,
            //ref DbParameterCollection z, 
            IEnumerable<DbParameter> parameters = null,
            ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.OracleCanalP,
            ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteReader
            );
        
        DbParameterCollection ExecuteByStoredProcedureNonQuery(
           string nameStore,
           //ref DbParameterCollection z, 
           IEnumerable<DbParameter> parameters = null,
           ConnectionBase.enuTypeDataBase typeDataBase = ConnectionBase.enuTypeDataBase.OracleCanalP,
           ConnectionBase.enuTypeExecute typeExecute = ConnectionBase.enuTypeExecute.ExecuteNonQuery
           );
    }
}
