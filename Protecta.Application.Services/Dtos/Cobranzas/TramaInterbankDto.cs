using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class TramaInterbankDto
    {
        public TramaInterbankResultado Respuesta { get; set; }
        public TramaInterbankEnvio Envio { get; set; }
    }
    public class TramaInterbankCabecera
    {
        public string TipoRegistro { get; set; }
        public string TipoFormato { get; set; }
        public string CodigoFijo { get; set; }
    }
    #region Envio
    public class TramaInterbankEnvio
    {
        public TramaInterbankEnvioCabecera Cabecera { get; set; }
        public TramaInterbankEnvioCuota Cuota { get; set; }
        public TramaInterbankDetalle Detalle { get; set; }
    }
    public class TramaInterbankEnvioCabecera : TramaInterbankCabecera
    {
        public string CodigoGrupo { get; set; }
        public string CodigoRubro { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string CodigoSolicitud { get; set; }
        public string DescripcionSolicitud { get; set; }
        public string OrigenSolicitud { get; set; }
        public string CodigoRequerimiento { get; set; }
        public string CanalEnvio { get; set; }
        public string TipoInformacion { get; set; }
        public string NumeroRegistros { get; set; }
        public string CodigoUnico { get; set; }
        public string FechaProceso { get; set; }
        public string FechaInicioCargos { get; set; }
        public string Moneda { get; set; }
        public string ImporteTotal1 { get; set; }
        public string ImporteTotal2 { get; set; }
        public string TipoGlosa { get; set; }
        public string GlosaGeneral { get; set; }
    }

    public class TramaInterbankEnvioCuota : TramaInterbankCabecera
    {
        public string CodigoCuota { get; set; }
        public string NumeroConceptos { get; set; }
        public string DescripcionConcepto1 { get; set; }
        public string DescripcionConcepto2 { get; set; }
        public string DescripcionConcepto3 { get; set; }
        public string DescripcionConcepto4 { get; set; }
        public string DescripcionConcepto5 { get; set; }
        public string DescripcionConcepto6 { get; set; }
        public string DescripcionConcepto7 { get; set; }
    }

    public class TramaInterbankDetalle : TramaInterbankCabecera
    {
        public string CodigoDeudor { get; set; }
        public string NombreDeudor { get; set; }
        public string Referencia1 { get; set; }
        public string Referencia2 { get; set; }
        public string TipoOperacion { get; set; }
        public string CodigoCuota { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MonedaDeuda { get; set; }
        public string ImporteConcepto1 { get; set; }
        public string ImporteConcepto2 { get; set; }
        public string ImporteConcepto3 { get; set; }
        public string ImporteConcepto4 { get; set; }
        public string ImporteConcepto5 { get; set; }
        public string ImporteConcepto6 { get; set; }
        public string ImporteConcepto7 { get; set; }
        public string TipoCuentaPrincipal { get; set; }
        public string ProductoCuentaPrincipal { get; set; }
        public string MonedaCuentaPrincipal { get; set; }
        public string NumeroCuentaPrincipal { get; set; }
        public string ImporteAbonarCuenta2 { get; set; }
        public string GlosaParticular { get; set; }
    }


    #endregion
    #region Resultado
    public class TramaInterbankResultado
    {
        public string CodigoRubro { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string Moneda { get; set; }
        public string CodigoDeudor { get; set; }
        public string CodigoCuota { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreDeudor { get; set; }
        public string FechaPago { get; set; }
        public string HoraPago { get; set; }
        public string ImportePagado { get; set; }
        public string ImporteMora { get; set; }
        public string ImporteDescuento { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string NumeroOperacion { get; set; }
        public string NumeroCheque { get; set; }
        public string IndicadorCheque { get; set; }
    }
    #endregion
}
