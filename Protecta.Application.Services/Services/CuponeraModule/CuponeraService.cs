using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Dtos.Cuponera;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.CrossCuting.Utilities.service;
using Protecta.Domain.Service.CuponeraModule.Aggregates;
using Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg;

namespace Protecta.Application.Service.Services.CuponeraModule
{
    public class CuponeraService : ICuponeraService
    {

        private readonly ICuponeraRepository _cuponeraRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly Ldap _ldapSettings;


        public CuponeraService(ICuponeraRepository cuponeraRepository, ILoggerManager logger, IMapper mapper, IOptions<Ldap> ldapSettings)
        {

            this._cuponeraRepository = cuponeraRepository;
            _logger = logger;
            _mapper = new MapperConfiguration(Config => { Config.CreateMissingTypeMaps = true; }).CreateMapper();
            _ldapSettings = ldapSettings.Value;
        }


        public async Task<IEnumerable<TransacionDto>> ListarTransaciones()
        {
            IEnumerable<TransacionDto> transacionDtos = null;

            try
            {
                var TransacionResult = await _cuponeraRepository.ListarTransaciones();
                if (TransacionResult == null) return null;
                transacionDtos = _mapper.Map<IEnumerable<TransacionDto>>(TransacionResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }

            return transacionDtos;
        }

        public async Task<ReciboDto> GetInfoRecibo(ParametersReciboDto parameters)
        {
            ReciboDto recibo = new ReciboDto();
            try
            {
                GenerateResponse response = new GenerateResponse();
                response = await _cuponeraRepository.ValidateRecibo(_mapper.Map<ParametersRecibo>(parameters));

                if (response.P_NCODE == 0) {

                    var ReciboResult = await _cuponeraRepository.GetInfoRecibo(_mapper.Map<ParametersRecibo>(parameters));
                    if (ReciboResult == null) return null;
                    recibo = _mapper.Map<ReciboDto>(ReciboResult);
                }
                else
                {
                    recibo.P_NCODE = response.P_NCODE;
                    recibo.P_SMESSAGE = response.P_SMESSAGE;
                }


            } catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return recibo;
        }

        public async Task<List<CuponDto>> GetInfoCuponPreview(ParametersReciboDto parameters)
        {
            List<CuponDto> ListCupon = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.GetInfoCuponPreview(_mapper.Map<ParametersRecibo>(parameters));
                if (ReciboResult == null) return null;
                ListCupon = _mapper.Map<List<CuponDto>>(ReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return ListCupon;
        }

        public async Task<List<CuponDto>> GetInfoCuponeraDetail(ParametersReciboDto parameters)
        {
            List<CuponDto> ListCupon = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.GetInfoCuponeraDetail(_mapper.Map<ParametersRecibo>(parameters));
                if (ReciboResult == null) return null;
                ListCupon = _mapper.Map<List<CuponDto>>(ReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return ListCupon;
        }

        public async Task<GenerateResponse> GenerateCupon(ParametersReciboDto parametersRecibo)
        {
            GenerateResponse reponse = null;
            try
            {
                var ResponseResult = await _cuponeraRepository.GenerateCupon(_mapper.Map<ParametersRecibo>(parametersRecibo));
                if (ResponseResult == null) return null;
                reponse = _mapper.Map<GenerateResponse>(ResponseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return reponse;
        }




        public async Task<ReciboDto> GetInfoCuponera(ParametersReciboDto parametersRecibo)
        {
            ReciboDto Cupon = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.GetInfoCuponera(_mapper.Map<ParametersRecibo>(parametersRecibo));
                if (ReciboResult == null) return null;
                Cupon = _mapper.Map<ReciboDto>(ReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return Cupon;
        }

        public async Task<DetalleReciboDto> GetInfoMovimiento(ParametersReciboDto parametersRecibo)
        {
            DetalleReciboDto Cupon = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.GetInfoMovimiento(_mapper.Map<ParametersRecibo>(parametersRecibo));
                if (ReciboResult == null) return null;
                Cupon = _mapper.Map<DetalleReciboDto>(ReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return Cupon;
        }

        public async Task<GenerateResponse> AnnulmentCupon(ParametersReciboDto parametersRecibo)
        {
            GenerateResponse Cupon = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.AnnulmentCupon(_mapper.Map<ParametersRecibo>(parametersRecibo));
                if (ReciboResult == null) return null;
                Cupon = _mapper.Map<GenerateResponse>(ReciboResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return Cupon;
        }

        public async Task<GenerateResponse> PrintCupon(PrintCupon paramPrint)
        {
            GenerateResponse Cupon = null;
            List<TemplateCupon1> template = null;
            try
            {
                var ReciboResult = await _cuponeraRepository.PrintCupon(_mapper.Map<PrintCupon>(paramPrint));
                if (ReciboResult == null) return null;
                template = _mapper.Map<List<TemplateCupon1>>(ReciboResult);

                Cupon = 
                ConsumeService<GenerateResponse, List<TemplateCupon1>>.PostRequest(_ldapSettings.UrlServicioGestor, "Report/GenerateReport", template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
            return Cupon;
        }




    }
}
