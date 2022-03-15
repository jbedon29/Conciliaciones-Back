using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Protecta.CrossCuting.Utilities.Configuration;

namespace Protecta.Infrastructure.Connection
{
    public class ConnectionBase : IConnectionBase
    {
        private string strConexionOracle = null;
        private string strConexionOracleVTime = null;
        private string strConexionOracleConciliacion = null;

        OracleConnection DataConnectionOracle = new OracleConnection();
        OracleConnection DataConnectionOracleTIME = new OracleConnection();
        OracleConnection DataConnectionOracleConciliacion = new OracleConnection();

        private readonly AppSettings _appSettings;

        public enum enuTypeDataBase
        {
            OracleCanalP,
            OracleVTime,
            OracleConciliacion
        }

        public enum enuTypeExecute
        {
            ExecuteNonQuery,
            ExecuteReader
        }

        //public DbParameterCollection ParamsCollectionResult;

        public DbParameterCollection ParamsCollectionResult { get; set; }

        //Constructor de la clase 
        public ConnectionBase(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            this.strConexionOracle = _appSettings.ConnectionStringORA;
            DataConnectionOracle.ConnectionString = this.strConexionOracle;

            this.strConexionOracleVTime = _appSettings.ConnectionStringTimeP;
            DataConnectionOracleTIME.ConnectionString = this.strConexionOracleVTime;

            this.strConexionOracleConciliacion = _appSettings.ConnectionStringConciliacion;
            DataConnectionOracleConciliacion.ConnectionString = this.strConexionOracleConciliacion;
        }

        public DbConnection ConnectionGet(enuTypeDataBase typeDataBase = enuTypeDataBase.OracleCanalP)
        {
            DbConnection DataConnection = null;
            switch (typeDataBase)
            {
                case enuTypeDataBase.OracleCanalP:
                    DataConnection = DataConnectionOracle;
                    break;
                case enuTypeDataBase.OracleVTime:
                    DataConnection = DataConnectionOracleTIME;
                    break;
                case enuTypeDataBase.OracleConciliacion:
                    DataConnection = DataConnectionOracleConciliacion;
                    break;
                default:
                    break;
            }
            return DataConnection;
        }

        public DbDataReader ExecuteByStoredProcedure(string nameStore,
                IEnumerable<DbParameter> parameters = null,
                enuTypeDataBase typeDataBase = enuTypeDataBase.OracleCanalP,
                enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
                )
        {
            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            cmdCommand.CommandText = nameStore;
            cmdCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    cmdCommand.Parameters.Add(parameter);
                }
            }

            DataConnection.Open();
            DbDataReader myReader;

            try { 
            if (((cmdCommand.Parameters.Contains("C_TABLE") || IsOracleReader(cmdCommand))) && typeExecute == enuTypeExecute.ExecuteReader)
            {
                myReader = cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
            {
                cmdCommand.ExecuteNonQuery();
                ParamsCollectionResult = cmdCommand.Parameters;
                //z = ParamsCollectionResult;
                cmdCommand.Connection.Close();
                myReader = null;
            }
            }catch(Exception ex)
            {
                throw ex;
            }
            finally{
           
            }
            return myReader;
        }

        public DbDataReader ExecuteByStoredProcedureCupon(string nameStore,
              IEnumerable<DbParameter> parameters = null,
              enuTypeDataBase typeDataBase = enuTypeDataBase.OracleCanalP,
              enuTypeExecute typeExecute = enuTypeExecute.ExecuteReader
              )
        {
            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            cmdCommand.CommandText = nameStore;
            cmdCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    cmdCommand.Parameters.Add(parameter);
                }
            }

            DataConnection.Open();
            DbDataReader myReader;

            if (((cmdCommand.Parameters.Contains("CUR_TOUT") ||  cmdCommand.Parameters.Contains("CUR_COUPONS") ||  cmdCommand.Parameters.Contains("CUR_CUPONERA") ||  cmdCommand.Parameters.Contains("CUR_TREPORTOUT") ||   cmdCommand.Parameters.Contains("RC1") || IsOracleReader(cmdCommand))) && typeExecute == enuTypeExecute.ExecuteReader)
            {
                myReader = cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
            {
                cmdCommand.ExecuteNonQuery();
                ParamsCollectionResult = cmdCommand.Parameters;
                //z = ParamsCollectionResult;
                cmdCommand.Connection.Close();
                myReader = null;
            }
            return myReader;
        }

        public DbParameterCollection ExecuteByStoredProcedureNonQuery(string nameStore,
              IEnumerable<DbParameter> parameters = null,
              enuTypeDataBase typeDataBase = enuTypeDataBase.OracleCanalP,
              enuTypeExecute typeExecute = enuTypeExecute.ExecuteNonQuery
              )
        {
            DbConnection DataConnection = ConnectionGet(typeDataBase);
            DbCommand cmdCommand = DataConnection.CreateCommand();
            cmdCommand.CommandText = nameStore;
            cmdCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    cmdCommand.Parameters.Add(parameter);
                }
            }

            DataConnection.Open();
            DbParameterCollection myReader = null;

            if (typeExecute == enuTypeExecute.ExecuteNonQuery)
            {   
                cmdCommand.ExecuteNonQuery();
                myReader = cmdCommand.Parameters;               
                cmdCommand.Connection.Close();              
            }
            return myReader;
        }

        /// <summary>
        /// </summary>
        /// <param name="cmdCommand"></param>
        /// <returns></returns>
        private bool IsOracleReader(DbCommand cmdCommand)
        {
            bool isOracleReader = false;
            foreach (DbParameter item in cmdCommand.Parameters)
            {
                if (item is OracleParameter)
                {
                    if ((item as OracleParameter).OracleDbType == OracleDbType.RefCursor)
                    {
                        isOracleReader = true;
                        break;
                    }
                }
            }
            return isOracleReader;
        }
    }
}
