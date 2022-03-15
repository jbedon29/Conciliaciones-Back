using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.Application.Service.Dtos.Factura;
using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.CrossCuting.Utilities.Configuration;

namespace Protecta.Application.Service.Services.FacturaModule
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;       

        public FacturaService(IFacturaRepository facturaRepository, ILoggerManager logger, IMapper mapper)
        {
            this._facturaRepository = facturaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentoAbonoDto>> ListarDocumentosAbonos(DatosConsultaDocumentoDto datosConsultaDocumentoDto)
        {
            IEnumerable<DocumentoAbonoDto> documento_abono_dtos = null;

            try
            {
                if (datosConsultaDocumentoDto.FechaDesde != null && datosConsultaDocumentoDto.FechaHasta != null)
                {
                    datosConsultaDocumentoDto.FechaDesde = Convert.ToDateTime(datosConsultaDocumentoDto.FechaDesde).ToShortDateString();
                    datosConsultaDocumentoDto.FechaHasta = Convert.ToDateTime(datosConsultaDocumentoDto.FechaHasta).ToShortDateString();
                }

                var consulta = _mapper.Map<DatosConsultaDocumento>(datosConsultaDocumentoDto);

                var result = await _facturaRepository.ListarDocumentosAbonos(consulta);

                if (result == null)
                    return null;

                documento_abono_dtos = _mapper.Map<IEnumerable<DocumentoAbonoDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return documento_abono_dtos;
        }

        public async Task<string> ValidarExisteFacturaDeposito()
        {
            string mensaje = string.Empty;

            try
            {
                var result = await _facturaRepository.ValidarExisteFacturaDeposito();

                if (result.IndGeneracion == "0")
                {
                    mensaje = "La fecha permitida para ejecutar el proceso es: " + string.Format("{0:dd/MM/yyyy}", result.DtFechaCreacion);
                }
                else 
                {
                    mensaje = result.IndGeneracion;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                mensaje = "Hubo error al validar el proceso.";
            }

            return mensaje;
        }

        public async Task<string> GenerarFacturaAbonos(DatosFacturaAbonosDto datosFacturaAbonos)
        {
            var result = string.Empty;

            try
            {
                var consulta = _mapper.Map<DatosFacturaAbonos>(datosFacturaAbonos);

                var facturadeposito = await _facturaRepository.ObtenerFacturaDeposito(consulta);

                if (facturadeposito != null)
                {
                    result = await _facturaRepository.RegistrarFacturaDeposito(facturadeposito);

                    foreach (DetalleFacturaDeposito d in facturadeposito.DetalleFacturaDeposito)
                    {
                        await _facturaRepository.ActualizarEstadoDeposito(d);
                    }

                    return result;                    
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                result = "1";
            }

            return result;
        }

        public async Task<string> GenerarNotaCredito()
        {
            NotaCredito nota_credito = null;

            var result = string.Empty;

            try
            {
                var facturadeposito = await _facturaRepository.ListarFacturaDeposito();

                if (facturadeposito.Count > 0)
                {
                    foreach (FacturaDeposito c in facturadeposito)
                    {
                        nota_credito = new NotaCredito();
                        nota_credito.IdProducto = c.IdProducto;
                        nota_credito.NumeroNotaCredito = " ";
                        nota_credito.IdFacturaDeposito = c.IdFacturaDeposito;
                        nota_credito.DcMonto = c.MontoTotal;
                        nota_credito.VcUsuarioCreacion = "AppConciliacion";
                        nota_credito.IddgEstado = 1; //Generado

                        await _facturaRepository.RegistrarNotaCredito(nota_credito);

                        await _facturaRepository.AnularFacturaDeposito(c);

                        foreach (DetalleFacturaDeposito d in c.DetalleFacturaDeposito)
                        {
                            d.IddgEstadoDeposito = 1101; //Ingresado
                            await _facturaRepository.ActualizarEstadoDeposito(d);
                        }
                    }

                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                result = "2";
            }

            return result;
        }
    }
}
