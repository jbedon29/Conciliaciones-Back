using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Planilla
{
    public class DetallePlanillaCobroDto
    {
        public int IdDetallePlanillaCobro
        {
            get; set;
        }

        public int IdPlanilla
        {
            get;
            set;
        }

        public int IdRamo
        {
            get;
            set;
        }

        public int IdProducto
        {
            get;
            set;
        }

        public long VcNumeropoliza
        {
            get;
            set;
        }

        public long VcNumerocertificado
        {
            get;
            set;
        }

        public long IdProforma
        {
            get;
            set;
        }

        public decimal DcMonto
        {
            get;
            set;
        }

        public string IddgEstado
        {
            get;
            set;
        }

        public string IddgEstadoDetPlanilla
        {
            get;
            set;
        }

        public DateTime DtFechacreacion
        {
            get;
            set;
        }

        public string VcUsuariocreacion
        {
            get;
            set;
        }

        public DateTime DtFechamodificacion
        {
            get;
            set;
        }

        public string VcUsuariomodificacion
        {
            get;
            set;
        }

        public int IndicaComprobante
        {
            get;
            set;
        }

        public int IndicaComision
        {
            get;
            set;
        }
    }
}
