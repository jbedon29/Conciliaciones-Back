using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg;

namespace Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg
{
    public interface IProceso
    {
        int RegistrarLog(ProcesoGeneral procesoGeneral);

        void RegistrarLog(LogProcesoGeneral logProcesoGeneral);
    }
}
