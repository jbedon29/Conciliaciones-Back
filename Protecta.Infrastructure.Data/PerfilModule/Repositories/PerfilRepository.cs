using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Domain.Service.SecurityModule.Aggregates.SecurityAgg;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using Protecta.Infrastructure.Connection;

namespace Protecta.Infrastructure.Data.PerfilModule.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;

        public PerfilRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
        }

        public Task<List<Perfil>> ListarPerfiles(DatosConsultaPerfil datosConsultaPerfil)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();

            List<Perfil> perfilList = new List<Perfil>();

            parameters.Add(new OracleParameter("P_NOMBREPERFIL", OracleDbType.NVarchar2, datosConsultaPerfil.NombrePerfil, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_TIPOPERFIL", OracleDbType.Int32, datosConsultaPerfil.IdTipoPerfil, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));


            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_LISTA_PERFILES", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    var perfilEntity = new Perfil
                    {
                        IdPerfil = Convert.ToInt32(dr["id_perfil"]),
                        TipoPerfil = dr["tipo_perfil"].ToString(),
                        VcNombrePerfil = dr["vc_nombre_perfil"].ToString(),
                        VcDescripcion = dr["vc_descripcion"].ToString(),
                        VcUsuariocreacion = dr["vc_usuario_creacion"].ToString(),
                        DtFechacreacion = dr["dt_fecha_creacion"] == null
                            ? ""
                            : string.Format("{0:dd/MM/yyyy}", dr["dt_fecha_creacion"].ToString()),
                        DtFechamodificacion = null,
                        Estado = 1,
                        VcUsuariomodificacion = string.Empty
                    };

                    perfilList.Add(perfilEntity);
                }
            }

            return Task.FromResult<List<Perfil>>(perfilList);
        }

        Task<List<TipoPerfil>> IPerfilRepository.ListarTipoPerfil()
        {
            var listaEntidades = new List<TipoPerfil>();
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            using (var dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SEL_TIPO_PERFIL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    var entities = new TipoPerfil
                    {
                        IdTipoPerfil = Convert.ToInt32(dr["id_tipo_perfil"]),
                        VcDescripcion = Convert.ToString(dr["vc_descripcion"])
                    };
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult<List<TipoPerfil>>(listaEntidades);
        }

        public Task<string> RegistrarPerfil(RecursoProceso perfil)
        {
            string _sMessage = string.Empty;

            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>
                {
                    new OracleParameter("P_IDPERFIL", OracleDbType.Int32, perfil.IdPerfil, ParameterDirection.Input),
                    new OracleParameter("P_TIPOPERFIL", OracleDbType.Int32, perfil.IdTipoPerfil, ParameterDirection.Input),
                    new OracleParameter("P_DESCRIPCION", OracleDbType.NVarchar2, perfil.VcDescripcion, ParameterDirection.Input),
                    new OracleParameter("P_NOMBREPERFIL", OracleDbType.NVarchar2, perfil.VcNombrePerfil, ParameterDirection.Input),
                    new OracleParameter("P_USUARIOCREACION", OracleDbType.NVarchar2, perfil.VcUsuariocreacion, ParameterDirection.Input),
                    new OracleParameter("P_IDRECURSOS", OracleDbType.NVarchar2, perfil.IdRecursos, ParameterDirection.Input)
                };

                var pResult = new OracleParameter
                {
                    Size = 500,
                    ParameterName = "P_MESSAGE",
                    Direction = ParameterDirection.Output
                };
                parameters.Add(pResult);

                _connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_REGISTRO_PERFIL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _sMessage = pResult.Value == null ? string.Empty : pResult.Value.ToString();
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<List<ListaRecursoRespuesta>> ListarRecursosPorPerfil(int idPerfil)
        {
            var listaEntidades = new List<ListaRecursoRespuesta>();
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("P_IDPERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input),
                new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output)

            };

            using (var dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_LISTA_RECURSOS_POR_PERFIL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    var entities = new ListaRecursoRespuesta
                    {
                        IdRecurso = Convert.ToInt32(dr["ID_RECURSO"]),
                        Flag = Convert.ToString(dr["FLAG"]),
                        Modulo = Convert.ToString(dr["MODULO"]),
                        Opcion = Convert.ToString(dr["OPCION"]),
                        Descripcion = Convert.ToString(dr["VC_DESCRIPCION"])
                    };
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult(listaEntidades);
        }

        public Task<string> RegistroUsuarioPerfil(string usuario, string firstName, string lastName, string mail, int idPerfil,
            string usuarioRegistro)
        {

            string _sMessage = string.Empty;

            try
            {

                var parameters = new List<OracleParameter>
                {
                    new OracleParameter("P_USER", OracleDbType.NVarchar2, usuario, ParameterDirection.Input),
                    new OracleParameter("P_FIRSTNAME", OracleDbType.NVarchar2, firstName, ParameterDirection.Input),
                    new OracleParameter("P_LASTNAME", OracleDbType.NVarchar2, lastName, ParameterDirection.Input),
                    new OracleParameter("P_MAIL", OracleDbType.NVarchar2, mail, ParameterDirection.Input),
                    new OracleParameter("P_IDPERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input),
                    new OracleParameter("P_USUARIOCREACION", OracleDbType.NVarchar2, usuarioRegistro, ParameterDirection.Input),
                };

                var pResult = new OracleParameter
                {
                    Size = 500,
                    ParameterName = "P_VC_RESPUESTA_MENSAJE",
                    Direction = ParameterDirection.Output
                };
                parameters.Add(pResult);

                _connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_REGISTRO_USUARIO_PERFIL",
                    parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _sMessage = pResult.Value == null ? string.Empty : pResult.Value.ToString();
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<List<Usuario>> ListarUsuarioPorPerfil(int idPerfil)
        {
            var listaEntidades = new List<Usuario>();
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("P_IDPERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input),
                new OracleParameter("C_TABLE", OracleDbType.RefCursor, ParameterDirection.Output)

            };

            using (var dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_LISTA_USUARIOS_POR_PERFIL", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    var entities = new Usuario
                    {
                        CodUsuario = Convert.ToString(dr["VC_COD_USUARIO"]),
                        CorreoUsuario = Convert.ToString(dr["VC_CORREO_USUARIO"]),
                        Nombre = Convert.ToString(dr["VC_NOMBRE_USUARIO"]),
                        ApellidoPaterno = Convert.ToString(dr["VC_APE_PATERNO"]),
                    };
                    listaEntidades.Add(entities);
                }
            }

            return Task.FromResult(listaEntidades);
        }

        public Task<string> ActualizarPerfil(RecursoProceso perfil)
        {
            string _sMessage;

            try
            {
                var parameters = new List<OracleParameter>
                {
                    new OracleParameter("P_IDPERFIL", OracleDbType.Int32, perfil.IdPerfil, ParameterDirection.Input),
                    new OracleParameter("P_TIPOPERFIL", OracleDbType.Int32, perfil.IdTipoPerfil, ParameterDirection.Input),
                    new OracleParameter("P_DESCRIPCION", OracleDbType.NVarchar2, perfil.VcDescripcion, ParameterDirection.Input),
                    new OracleParameter("P_NOMBREPERFIL", OracleDbType.NVarchar2, perfil.VcNombrePerfil, ParameterDirection.Input),
                    new OracleParameter("P_USUARIOMODIFICA", OracleDbType.NVarchar2, perfil.VcUsuariocreacion, ParameterDirection.Input),
                    new OracleParameter("P_IDRECURSOS", OracleDbType.NVarchar2, perfil.IdRecursos, ParameterDirection.Input)
                };

                var pResult = new OracleParameter
                {
                    Size = 500,
                    ParameterName = "P_MESSAGE",
                    Direction = ParameterDirection.Output
                };
                parameters.Add(pResult);
                _connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_ACTUALIZA_PERFIL", parameters,
                    ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _sMessage = pResult.Value == null ? string.Empty : pResult.Value.ToString();
            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> EliminarPerfil(int idPerfil, string usuario)
        {
            string _sMessage;
            try
            {
                var parameters = new List<OracleParameter>
                {
                    new OracleParameter("P_IDPERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input),
                    new OracleParameter("P_USUARIO", OracleDbType.NVarchar2, usuario, ParameterDirection.Input)
                };

                var pResult = new OracleParameter
                {
                    Size = 500,
                    ParameterName = "P_MESSAGE",
                    Direction = ParameterDirection.Output
                };
                parameters.Add(pResult);

                _connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_ELIMINA_PERFIL", parameters,
                    ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _sMessage = pResult.Value == null ? string.Empty : pResult.Value.ToString();

            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> EliminaUsuarioPerfil(string usuario, int idPerfil, string usuarioCreacion)
        {
            string _sMessage;
            try
            {
                var parameters = new List<OracleParameter>
                {
                    new OracleParameter("P_USER", OracleDbType.NVarchar2, usuario, ParameterDirection.Input),
                    new OracleParameter("P_IDPERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input),
                    new OracleParameter("P_USUARIO", OracleDbType.NVarchar2, usuarioCreacion, ParameterDirection.Input)
                };

                var pResult = new OracleParameter
                {
                    Size = 500,
                    ParameterName = "P_MESSAGE",
                    Direction = ParameterDirection.Output
                };
                parameters.Add(pResult);

                _connectionBase.ExecuteByStoredProcedure("RAC_ADMINIST_SEGURIDAD.SP_ELIMINA_USUARIO_PERFIL", parameters,
                    ConnectionBase.enuTypeDataBase.OracleConciliacion);
                _sMessage = pResult.Value == null ? string.Empty : pResult.Value.ToString();

            }
            catch (Exception ex)
            {
                _sMessage = string.Format("Error: {0}", ex.ToString());
            }

            return Task.FromResult<string>(_sMessage);
        }
    }
}