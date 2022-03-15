using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;

namespace Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg
{
    public interface IGeneracionExactusRepository
    {
        List<PlanillaEstado> ListarPlanillaConciliada(DatosConsultaArchivos datosConsultaArchivos);

        List<PlanillaEstado> ListarPlanillaDocumentoLiquidado(DatosConsultaArchivos datosConsultaArchivos);

        string RegistrarPlanillaDetalle(PlanillaEstado planillaEstado);

        List<DetallePlanillaCobro> ListarPlanillaCertificado(PlanillaEstado planillaEstado);

        List<PREMIUM_MO> ListarReciboAbonado(PlanillaDetalle planillaDetalle);

        string RegistrarReciboAbonado(PREMIUM_MO premiun_mo);

        List<COLFORMREF> ListarCobroAbonado(PlanillaDetalle planillaDetalle);

        string RegistrarCobroAbonado(COLFORMREF colformref);

        List<COMM_VISANET> ListarComisionCabeceraAbonado(long idProforma);

        List<DET_COMM_VISANET> ListarComisionDetalleAbonado(long idProforma);

        string RegistrarComisionCabeceraAbonado(COMM_VISANET comm_visanet);

        string RegistrarComisionDetalleAbonado(DET_COMM_VISANET det_comm_visanet);

    }
}
