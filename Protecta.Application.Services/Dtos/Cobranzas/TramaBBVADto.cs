using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class TramaBBVADto
    {
        public TramaBBVAResultado Respuesta { get; set; }
        public TramaBBVAEnvio Envio { get; set; }
    }
    #region Envio
    public class TramaBBVAEnvio
    {
        public TramaBBVAEnvioCabecera Cabecera { get; set; }
        public TramaBBVAEnvioDetalle Detalle { get; set; }
        public TramaBBVAEnvioTotales Totales { get; set; }
    }
    public class TramaBBVAEnvioCabecera
    {
        public string TipoRegistro { get; set; }
        public string Ruc { get; set; }
        public string NumeroClase { get; set; }
        public string Moneda { get; set; }
        public string FechaFacturacion { get; set; }
        public string Version { get; set; }
        public string TipoActualizacion { get; set; }
    }
    public class TramaBBVAEnvioDetalle
    {
        public string TipoRegistro { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionPago { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaBloqueo { get; set; }
        public string ImpMaximoCobrar { get; set; }
        public string ImpMinimoCobrar { get; set; }
        public string InformacionAdicional { get; set; }
        public string CodigoSubconcepto1 { get; set; }
        public string ValorSubconcepto1 { get; set; }
        public string CodigoSubconcepto2 { get; set; }
        public string ValorSubconcepto2 { get; set; }
        public string CodigoSubconcepto3 { get; set; }
        public string ValorSubconcepto3 { get; set; }
        public string CodigoSubconcepto4 { get; set; }
        public string ValorSubconcepto4 { get; set; }
        public string CodigoSubconcepto5 { get; set; }
        public string ValorSubconcepto5 { get; set; }
        public string CodigoSubconcepto6 { get; set; }
        public string ValorSubconcepto6 { get; set; }
        public string CodigoSubconcepto7 { get; set; }
        public string ValorSubconcepto7 { get; set; }
        public string CodigoSubconcepto8 { get; set; }
        public string ValorSubconcepto8 { get; set; }

    }
    public class TramaBBVAEnvioTotales
    {
        public string TipoRegistro { get; set; }
        public string CantidadRegistrosContables { get; set; }
        public string TotalImpMaximos { get; set; }
        public string TotalImpMinimos { get; set; }
        public string DatosAdicionales { get; set; }
    }
    #endregion
    #region Resultado
    public class TramaBBVAResultado
    {
        public TramaBBVAResultadoCabecera Cabecera { get; set; }
        public TramaBBVAResultadoEnvioDetalle Detalle { get; set; }
        public TramaBBVAResultadoTotales Totales { get; set; }
    }
    public class TramaBBVAResultadoCabecera : TramaBBVAEnvioCabecera
    {
        public string FechaProceso { get; set; }
        public string CuentaRecaudadora { get; set; }

    }
    public class TramaBBVAResultadoEnvioDetalle
    {
        public string TipoRegistro { get; set; }
        public string NombreCliente { get; set; }
        public string Referencia { get; set; }
        public string ImporteOrigen { get; set; }
        public string ImporteDepositado { get; set; }
        public string ImporteMora { get; set; }
        public string Oficina { get; set; }
        public string NumeroMovimiento { get; set; }
        public string FechaPago { get; set; }
        public string TipoValor { get; set; }
        public string CanalEntrada { get; set; }
    }

    public class TramaBBVAResultadoTotales
    {
        public string TipoRegistro { get; set; }
        public string TotalRegistroGrabado { get; set; }
        public string TotalPagos { get; set; }
        public string TotalDepositos { get; set; }
        public string TotalMora { get; set; }
    }



    #endregion


}
