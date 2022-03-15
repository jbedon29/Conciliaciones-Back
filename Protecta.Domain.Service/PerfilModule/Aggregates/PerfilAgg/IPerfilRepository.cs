using System.Collections.Generic;
using System.Threading.Tasks;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.SecurityModule.Aggregates.SecurityAgg;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

namespace Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg
{
    public interface IPerfilRepository
    {
        Task<List<Perfil>> ListarPerfiles(DatosConsultaPerfil datosConsultaPerfil);
        Task<List<TipoPerfil>> ListarTipoPerfil();
        Task<string> RegistrarPerfil(RecursoProceso perfil);
        Task<List<ListaRecursoRespuesta>> ListarRecursosPorPerfil(int idPerfil);
        Task<string> RegistroUsuarioPerfil(string usuario, string firstName, string lastName, string mail, int idPerfil, string usuarioRegistro);
        Task<List<Usuario>> ListarUsuarioPorPerfil(int idPerfil);
        Task<string> ActualizarPerfil(RecursoProceso perfil);
        Task<string> EliminarPerfil(int idPerfil, string usuario);
        Task<string> EliminaUsuarioPerfil(string usuario, int idPerfil, string usuarioCreacion);
    }
}