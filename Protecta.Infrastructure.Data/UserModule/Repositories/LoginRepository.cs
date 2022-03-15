using System;
using System.Threading.Tasks;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using Protecta.Infrastructure.Data.Extensions;
using Protecta.Infrastructure.Connection;
using Protecta.CrossCuting.Utilities.Configuration;
using Microsoft.Extensions.Options;
using System.Data.Common;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;


namespace Protecta.Infrastructure.Data.UserModule.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public LoginRepository(IOptions<AppSettings> appSettings,
                                IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<PRO_USER> Authenticate(string username, string password)
        {
            PRO_USER entities = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            try { 
            parameters.Add(new OracleParameter("P_SUSER", OracleDbType.NVarchar2, 20, username, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_SPASSWORD", OracleDbType.NVarchar2, 100, password, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_SEL_USUARIO_CREDENCIALES", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                if (dr.Read())
                {
                    //entities = dr.ReadFields<PRO_USER>();

                    entities = new PRO_USER();
                    entities.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"].ToString());
                    entities.VC_COD_USUARIO = dr["VC_COD_USUARIO"].ToString();
                    entities.VC_NOMBRE_USUARIO = dr["VC_NOMBRE_USUARIO"].ToString();
                    entities.VC_APE_PATERNO = dr["VC_APE_PATERNO"].ToString();
                    entities.VC_APE_MATERNO = dr["VC_APE_MATERNO"].ToString();
                }
                else
                {
                    return null;
                }
            }
            }catch(Exception ex)
            {

            }

            return Task.FromResult<PRO_USER>(entities);
        }

        public Task<Respuesta> LecturaRecursos(string username)
        {
            var respuesta = new Respuesta();

            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_VC_USUARIO", OracleDbType.NVarchar2, username, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.OBTENER_RECURSOS_USUARIO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    var a = dr["JSON"].ToString() +
                        "{!state!:!cobranzas!,!name!:!Gestión de Cobranzas!,!type!:" +
                        "!sub!,!icon!:!library_books!," +
                        "!children!: [{!state!: !generartrama!,!name!:" +
                        "!Generar Trama!} , {!state!: !cargarlote!,!name!:" +
                        "!Cargar Lote!},{!state!: !pagorecibos!,!name!:" +
                        "!Pago Recibos!},{!state!: !seguimientolote!,!name!:" +
                          "!Seguimiento de Lote de Cobranza!} ]}||";

                    //"!Forma de Pago NC/PCP!}]}||";
                    

                    var b = dr["JSON"].ToString();
                    respuesta.Mensaje = Convert.ToString(a.Replace("!", "\""));
                    //respuesta.Mensaje = Convert.ToString(dr["JSON"].ToString());
                    respuesta.Resultado = 1;
                }
            }

            return Task.FromResult<Respuesta>(respuesta);
        }

        public Task<IEnumerable<Recursos>> LecturaRecursos(int idPerfil)
        {

            IEnumerable<Recursos> entities = null;
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_IDPERFIL", OracleDbType.Decimal, idPerfil, ParameterDirection.Input));
            parameters.Add(new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_LISTA_RECURSOS_POR_PERFIL", parameters))
            {
                entities = dr.ReadRows<Recursos>();
            }

            return Task.FromResult<IEnumerable<Recursos>>(entities);
        }

        public Task<Recursos> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recursos>> All()
        {
            throw new NotImplementedException();
        }

        public Task<string> Create(Recursos _dato)
        {
            throw new NotImplementedException();
        }

        public Task<string> Update(Recursos _dato)
        {
            throw new NotImplementedException();
        }

        public Task<string> Remove(Recursos _dato)
        {
            throw new NotImplementedException();
        }
    }
}