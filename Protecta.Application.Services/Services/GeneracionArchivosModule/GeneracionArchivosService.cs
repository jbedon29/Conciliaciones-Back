using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Protecta.Application.Service.Dtos.Consulta;
using DTO = Protecta.Application.Service.Dtos.GeneracionArchivos;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg;
using System;
using System.Linq;
using Protecta.Application.Service.Dtos.GeneracionArchivos;

namespace Protecta.Application.Service.Services.GeneracionArchivosModule
{
    public class GeneracionArchivosService : IGeneracionArchivosService
    {
        private readonly IGeneracionArchivosRepository _generacionArchivosRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public GeneracionArchivosService(
            IGeneracionArchivosRepository generacionArchivosRepository,
            ILoggerManager logger,
            IMapper mapper
            )
        {
            this._generacionArchivosRepository = generacionArchivosRepository;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<List<DTO.DatosRespuestaGeneracionArchivo>> ConsultarArchivos(DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto)
        {
            List<DTO.DatosRespuestaGeneracionArchivo> itemsRespuesta = null;

            try
            {
                var datosConsulta = _mapper.Map<DatosConsultaGenerarArchivos>(datosConsultaArchivosDto);

                var planillasResult = await _generacionArchivosRepository.ConsultarArchivos(datosConsulta);

                if (planillasResult == null)
                    return null;

                itemsRespuesta = _mapper.Map<List<DTO.DatosRespuestaGeneracionArchivo>>(planillasResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return itemsRespuesta;
        }

        public async Task<DTO.DatosRespuestaFechaGeneracionDto> FechaGeneracion()
        {
            var respuesta = new DTO.DatosRespuestaFechaGeneracionDto() { fechaGeneracion = DateTime.Now.ToString("dd/MM/yyyy") };

            try
            {
                var dias = await _generacionArchivosRepository.ObtenerDiasPermitidos();

                if (dias > 0)
                {
                    var fechaHoy = DateTime.Today;
                    var fechaBase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var res = fechaHoy.Subtract(fechaBase).Days;

                    respuesta.editable = (res <= dias);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return respuesta;
        }

        public async Task<DTO.DatosProcesoGeneracionArchivo> ProcesarArchivos(DatosConsultaGeneracionArchivoDto datosConsultaArchivosDto)
        {
            DTO.DatosProcesoGeneracionArchivo infoRespuesta = null;

            try
            {
                var datosConsulta = _mapper.Map<DatosConsultaGenerarArchivos>(datosConsultaArchivosDto);
                var repositoryResult = await _generacionArchivosRepository.ProcesarArchivos(datosConsulta);

                if (repositoryResult == null)
                    return null;

                infoRespuesta = _mapper.Map<DTO.DatosProcesoGeneracionArchivo>(repositoryResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return infoRespuesta;
        }
    }
}
