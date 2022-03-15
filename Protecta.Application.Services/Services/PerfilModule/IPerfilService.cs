using Protecta.Application.Service.Dtos.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Protecta.Domain.Service.SecurityModule.Aggregates.SecurityAgg;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

namespace Protecta.Application.Service.Services.PerfilModule
{
    public interface IPerfilService
    {
        Task<IEnumerable<TipoPerfilDto>> ListarTipoPerfil();
        Task<IEnumerable<PerfilDto>> ListarPerfiles(PerfilConsultaDto perfilConsultaDto);
        Task<IEnumerable<ListaRecursoRespuesta>> ListaRecursoPorPerfilAsync(int idPerfil);
        Task<string> RegistroUsuarioPerfil(string usuario, string firstName, string lastName, string mail, int idPerfil, string usuarioRegistro);
        Task<IEnumerable<Usuario>> ListaUsuariosPorPerfil(int idPerfil);
        Task<string> RegistroPerfilTask(RecursoProcesoDto recursoProcesoDto);
        Task<string> ActualizaPerfilTask(RecursoProcesoDto recursoProcesoDto);
        Task<string> EliminaPerfilTask(int idPerfil, string usuario);
        Task<string> EliminaUsuarioPerfilTask(string usuario, int idPerfil, string usuarioCreacion);
    }
}
