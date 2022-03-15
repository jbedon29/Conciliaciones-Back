using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg
{
    public interface IGeneracionArchivosRepository
    {
        Task<int> ObtenerDiasPermitidos();

        Task<List<DatosRespuestaGeneracionArchivo>> ConsultarArchivos(DatosConsultaGenerarArchivos datosConsulta);

        Task<DatosProcesoGeneracionArchivo> ProcesarArchivos(DatosConsultaGenerarArchivos datosConsulta);
    }
}
