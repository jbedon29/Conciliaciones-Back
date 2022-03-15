using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Protecta.CrossCuting.Log.Contracts;
using AutoMapper;
using System.Threading.Tasks;
using Protecta.Application.Service.Services.PlanillaModule;
using EN = Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;

namespace Protecta.Application.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ILoggerManager _logger;
        private readonly IPlanillaService _PlanillaService;
        private IMapper _mapper;

        public ValuesController(ILoggerManager logger,
                    IPlanillaService _planillaService)
        {
            _logger = logger;
            _PlanillaService = _planillaService;
        }

        // GET api/values
        //[HttpGet]
        //public IEnumerable<EN.Planilla> Get()
        //{
        //    _logger.LogInfo("Metodo Get Planilla");
        //    try
        //    {
        //        return new EN.Planilla[] {
        //            new EN.Planilla() { NIDCHANNEL = 1000, NIDUSER = 9999 },
        //            new EN.Planilla() { NIDCHANNEL = 1000, NIDUSER = 8888 },
        //            new EN.Planilla() { NIDCHANNEL = 1000, NIDUSER = 7777 }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw ex;
        //    }
        //}
    }
}