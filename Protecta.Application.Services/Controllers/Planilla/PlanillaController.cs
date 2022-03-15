using System;
using AutoMapper;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Application.Service.Dtos.Planilla;
using Protecta.Application.Service.Services.PlanillaModule;
using Protecta.Application.Service.Dtos.Consulta;

namespace Protecta.Application.Service.Controllers.Planilla
{
    [Route("api/[controller]")]
    public class PlanillaController : Controller
    {
        private IPlanillaService _PlanillaService;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public PlanillaController(ILoggerManager logger, IPlanillaService _planillaService, 
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _PlanillaService = _planillaService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportarPlanilla([FromBody] DatosConsultaPlanillaDto datosConsultaPlanillaDto) 
        {
            _logger.LogInfo("Metodo Importar Planilla Venta");

            var planillaResult = await _PlanillaService.ImportarPlanilla(datosConsultaPlanillaDto);

            return Ok(planillaResult);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> NotificarPlanillaConciliada(DatosNotificacionDto datosNotificacionDto) 
        {
            //DatosNotificacionDto datosNotificacionDto = new DatosNotificacionDto();
            //datosNotificacionDto.IndPlanilla = 1;
            //datosNotificacionDto.Planilla = "184";
            //datosNotificacionDto.Usuario = "UsrConci";

            var notificacionResult = await _PlanillaService.NotificarPlanillaConciliada(datosNotificacionDto);

            return Ok(notificacionResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NotificarPlanillaNoLiquidada(DatosNotificacionDto datosNotificacionDto) 
        {
            //DatosNotificacionDto datosNotificacionDto = new DatosNotificacionDto();
            //datosNotificacionDto.IndPlanilla = 0;
            //datosNotificacionDto.Planilla = "";

            var notificacionResult = await _PlanillaService.NotificarPlanillaNoLiquidada(datosNotificacionDto);

            return Ok(notificacionResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConsultarPlanillasProcesadas([FromBody] DatosConsultaPlanillaDto datosConsultaPlanillaDto) 
        //public async Task<IActionResult> ConsultarPlanillasProcesadas()
        {
            //DatosConsultaPlanillaDto datosConsultaPlanillaDto = new DatosConsultaPlanillaDto();
            //datosConsultaPlanillaDto.FechaDesde = "01/07/2018";
            //datosConsultaPlanillaDto.FechaHasta = "26/07/2018";
            //datosConsultaPlanillaDto.IdCanal = 0;
            //datosConsultaPlanillaDto.IdProducto = 1000;
            if (datosConsultaPlanillaDto.IdProducto != 71)
            {
                datosConsultaPlanillaDto.IdTipoCobro = 0;
            }
            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaPlanillaDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaPlanillaDto.FechaHasta);
            datosConsultaPlanillaDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaPlanillaDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var planillaResult = await _PlanillaService.ConsultarPlanillasProcesadas(datosConsultaPlanillaDto);
            return Ok(planillaResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConsultarPlanillasProcesadasLog([FromBody] DatosConsultaPlanillaDto datosConsultaPlanillaDto)       
        {

            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaPlanillaDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaPlanillaDto.FechaHasta);
            datosConsultaPlanillaDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaPlanillaDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var planillaResult = await _PlanillaService.ConsultarPlanillasProcesadasLog(datosConsultaPlanillaDto);
            return Ok(planillaResult);
        }

        [HttpPost("[action]")]
        //public async Task<IActionResult> ConsultarPlanillasProcesadas(long idProducto, string fechaInicio, string FechaFin)
        public async Task<IActionResult> ConsultarPlanillasPendientes([FromBody] DatosConsultaPlanillaDto datosConsultaPlanillaDto) 
        {
            _logger.LogInfo("Metodo Listar Planillas pendientes");

            ////DatosConsultaPlanillaDto datosConsultaPlanillaDto = new DatosConsultaPlanillaDto();
            ////datosConsultaPlanillaDto.IdProducto = 1000;
            ////datosConsultaPlanillaDto.IdCanal = 0;
            ////datosConsultaPlanillaDto.FechaDesde = "01/07/2018";
            ////datosConsultaPlanillaDto.FechaHasta = "26/07/2018";

            DateTime dfechaDesde = Convert.ToDateTime(datosConsultaPlanillaDto.FechaDesde);
            DateTime dfechaHasta = Convert.ToDateTime(datosConsultaPlanillaDto.FechaHasta);
            datosConsultaPlanillaDto.FechaDesde = dfechaDesde.ToString("dd/MM/yyyy");
            datosConsultaPlanillaDto.FechaHasta = dfechaHasta.ToString("dd/MM/yyyy");

            var planillaResult = await _PlanillaService.ConsultarPlanillasPendientes(datosConsultaPlanillaDto);
            
            return Ok(planillaResult);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ValidarFacturaDePlanilla([FromBody] long idPlanilla)
        {
            var pCount= await _PlanillaService.ValidarFacturaDePlanilla(idPlanilla);

            return Ok(new { cantidad = pCount });
        }


    }
}