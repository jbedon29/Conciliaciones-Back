using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Protecta.Application.Service.Dtos.Perfil;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg;
using Protecta.Domain.Service.SecurityModule.Aggregates.SecurityAgg;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

namespace Protecta.Application.Service.Services.PerfilModule
{
    public class PerfilService : IPerfilService
    {

        private readonly IPerfilRepository _perfilRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public PerfilService(IPerfilRepository perfilRepository, ILoggerManager logger, IMapper mapper)
        {
            this._perfilRepository = perfilRepository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TipoPerfilDto>> ListarTipoPerfil()
        {
            //Traer datos de los productos de base de datos
            var tipoPerfilResult = await _perfilRepository.ListarTipoPerfil();

            //Verifica que el resultado no sea nulo
            if (tipoPerfilResult == null)
                return null;
            ////Mapeo
            var tipoPerfilDto = _mapper.Map<IEnumerable<TipoPerfilDto>>(tipoPerfilResult);
            return tipoPerfilDto;
        }

        public async Task<IEnumerable<PerfilDto>> ListarPerfiles(PerfilConsultaDto perfilConsultaDto)
        {
            var datosConsultaEntity = _mapper.Map<DatosConsultaPerfil>(perfilConsultaDto);
            var perfilResult = await _perfilRepository.ListarPerfiles(datosConsultaEntity);

            var perfilDto = _mapper.Map<IEnumerable<PerfilDto>>(perfilResult);
            return perfilDto;
        }

        public async Task<IEnumerable<ListaRecursoRespuesta>> ListaRecursoPorPerfilAsync(int idPerfil)
        {
            return await _perfilRepository.ListarRecursosPorPerfil(idPerfil);
        }

        public async Task<string> RegistroUsuarioPerfil(string usuario, string firstName, string lastName, string mail, int idPerfil,
            string usuarioRegistro)
        {
            var lowerUsuario = usuario.ToLower();
            return await _perfilRepository.RegistroUsuarioPerfil(lowerUsuario, firstName, lastName, mail, idPerfil,
                usuarioRegistro);
        }

        public async Task<IEnumerable<Usuario>> ListaUsuariosPorPerfil(int idPerfil)
        {
            return await _perfilRepository.ListarUsuarioPorPerfil(idPerfil);
        }

        public async Task<string> RegistroPerfilTask(RecursoProcesoDto recursoProcesoDto)
        {
            var datosRecursoEntity = _mapper.Map<RecursoProceso>(recursoProcesoDto);
            var result = await _perfilRepository.RegistrarPerfil(datosRecursoEntity);
            return result;
        }

        public async Task<string> ActualizaPerfilTask(RecursoProcesoDto recursoProcesoDto)
        {
            var datosRecursoEntity = _mapper.Map<RecursoProceso>(recursoProcesoDto);
            var result = await _perfilRepository.ActualizarPerfil(datosRecursoEntity);
            return result;
        }

        public async Task<string> EliminaPerfilTask(int idPerfil, string usuario)
        {
            return await _perfilRepository.EliminarPerfil(idPerfil, usuario);
        }

        public async Task<string> EliminaUsuarioPerfilTask(string usuario, int idPerfil, string usuarioCreacion)
        {
            return await _perfilRepository.EliminaUsuarioPerfil(usuario, idPerfil, usuarioCreacion);
        }
    }
}
