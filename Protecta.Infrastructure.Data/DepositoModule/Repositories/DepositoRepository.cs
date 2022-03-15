using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;
using Protecta.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Infrastructure.Data.DepositoModule.Repositories
{
    public class DepositoRepository : IDepositoRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public DepositoRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Deposito>> ListarDeposito(DatosConsultaDeposito deposito)
        {
            Deposito entities = null;
            List<Deposito> listaEntidades = new List<Deposito>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idEstadoDeposito = 1101;
            long _idEntidad = deposito.IdBanco; // 0;
            long _idProducto = 0;
            long _idCuenta = deposito.IdCuenta;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", deposito.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", deposito.FechaHasta);

            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ESTADO_DEPOSITO", OracleDbType.Long, _idEstadoDeposito, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_ENTIDAD", OracleDbType.Long, _idEntidad, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CUENTA", OracleDbType.Long, _idCuenta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_DEPOSITO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Deposito();
                    entities.IdDeposito = Convert.ToInt64(dr["ID_DEPOSITO"]);
                    entities.NumeroOperacion = Convert.ToString(dr["VC_NUMERO_OPERACION"]);
                    entities.Monto = Convert.ToDecimal(dr["DC_MONTO"]);
                    entities.Saldo = Convert.ToDecimal(dr["DC_SALDO"]);
                    entities.FechaDeposito = Convert.ToDateTime(dr["dt_fecha_deposito"]);
                    entities.Extorno = Convert.ToString(dr["extorno"]);
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult<List<Deposito>>(listaEntidades);
        }

        public Task<List<Deposito>> ListarDepositoExtornado(DatosConsultaDepositoExtorno deposito)
        {
            Deposito entities = null;
            List<Deposito> listaEntidades = new List<Deposito>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_NRO_OPERACION", OracleDbType.Varchar2, deposito.NumeroOperacion, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_DEPOSITO", OracleDbType.Varchar2, deposito.IdDeposito, ParameterDirection.Input));

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_DEPOSITO_EXTORNO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
#if DEBUG
                //listaEntidades.Add(new Deposito
                //{ IdDeposito = 1, NumeroOperacion = "987982", Monto = 50, Saldo = 5, FechaDeposito = DateTime.Now });
                //listaEntidades.Add(new Deposito
                //{ IdDeposito = 2, NumeroOperacion = "807982", Monto = 60, Saldo = 8, FechaDeposito = DateTime.Now });
#endif
                while (dr.Read())
                {
                    entities = new Deposito();
                    entities.IdDeposito = Convert.ToInt64(dr["ID_DEPOSITO"]);
                    entities.NumeroOperacion = Convert.ToString(dr["VC_NUMERO_OPERACION"]);
                    entities.Monto = Convert.ToDecimal(dr["DC_MONTO"]);
                    entities.Saldo = Convert.ToDecimal(dr["DC_SALDO"]);
                    entities.FechaDeposito = Convert.ToDateTime(dr["dt_fecha_deposito"]);
                    entities.IdDepositoAsociado = deposito.IdDeposito;
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult<List<Deposito>>(listaEntidades);
        }

    }
}
