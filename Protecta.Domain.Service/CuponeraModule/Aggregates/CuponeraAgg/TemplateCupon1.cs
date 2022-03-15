using System;
using System.Collections.Generic;
using System.Text;

namespace Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg
{
    public class TemplateCupon1
    {
        public int IdRamo { get; set; }
        public string DesRamo { get; set; }
        public int IdMoneda { get; set; }
        public string DesMoneda { get; set; }
        public string Policy { get; set; }
        public string VigenciaDesde { get; set; }
        public string VigenciaHasta { get; set; }
        public string Documento { get; set; }
        public string Asegurado { get; set; }
        public string Direccion { get; set; }
        public string Intermediario { get; set; }
        public int Convenio { get; set; }
        public string Cuponera { get; set; }
        public string Cupon { get; set; }
        public string FechaVencimiento { get; set; }
        public Decimal Importe { get; set; }


    }
}
