using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Protecta.Application.Service.Dtos.Cobranzas
{
    public class TramaBCPDto
    {
        public TramaBCPResultado Respuesta { get; set; }
        public TramaBCPEnvio Envio { get; set; }
    }
    public class TramaBCPCabecera
    {
        //Tipo de registro(CC=Cabecera,DD= Detalle)
        public string TipoRegistro { get; set; }
        //Código de la Sucursal(de la cta.de la Empresa Afiliada)
        public string CodigoSucursal { get; set; }
        //Código de la moneda(de la cta.de la Empresa Afiliada)
        public string CodigoMoneda { get; set; }
        //Número de cuenta de la Empresa Afiliada
        public string NumeroCuenta { get; set; }
    }
    #region Envio
    public class TramaBCPEnvio
    {
        public TramaBCPEnvioCabecera Cabecera { get; set; }
        public TramaBCPEnvioDetalle Detalle { get; set; }
    }
    public class TramaBCPEnvioCabecera : TramaBCPCabecera
    {
        //Tipo de validación
        public string TipoValidacion { get; set; }
        //Nombre de la Empresa Afiliada
        public string NombreEmpresaAfiliada { get; set; }
        //Fecha de transmisión
        public string FechaTransmision { get; set; }
        //Cantidad total de registros enviados
        public string CantidadTotalRegistros { get; set; }
        //Monto total enviado
        public string MontoTotalEnviado { get; set; }
        //Tipo de Archivo
        public string TipoArchivo { get; set; }
        //Código Servicio
        public string CodigoServicio { get; set; }


    }
    public class TramaBCPEnvioDetalle : TramaBCPCabecera
    {
        //Código de identificación del Depositante
        public string CodigoDepositante { get; set; }
        //Nombre del Depositante
        public string NombreDepositante { get; set; }
        //Campo con información de retorno
        public string InformacionRetorno { get; set; }
        //Fecha de emisión del cupón
        public string FechaEmisionCupon { get; set; }
        //Fecha de vencimiento del cupón
        public string FechaVencimientoCupon { get; set; }
        //Monto del cupón
        public string MontoCupon { get; set; }
        //Monto del mora
        public string MontoMora { get; set; }
        //Monto mínimo
        public string MontoMinimo { get; set; }
        //Tipo de registro de actualización
        public string TipoRegistroActualizacion { get; set; }
        //Nro.Documento de Pago
        public string NumeroDocumentoPago { get; set; }
        //Nro.Documento de Identidad
        public string NumeroDocumentoIdentidad { get; set; }

    }
    #endregion
    #region Resultado
    public class TramaBCPResultado
    {
        public TramaBCPResultadoCabecera Cabecera { get; set; }
        public TramaBCPResultadoDetalle Detalle { get; set; }


    }
    public class TramaBCPResultadoCabecera : TramaBCPCabecera
    {
        //Tipo de validación
        public string TipoValidacion { get; set; }
        //Fecha de proceso
        public string FechaProceso { get; set; }
        //Cantidad total de registros enviados
        public long CantidadTotalRegistros { get; set; }
        //Monto total de los importes pagados
        public double MontoTotalImportePagado { get; set; }
        //Código Interno BCP
        public string CodigoInternoBCP { get; set; }
        //Casilla(usuario Teletransfer de la cuenta recaudadora)
        public string Casilla { get; set; }
        //Hora en que se realizo el corte de información
        public string HoraCorte { get; set; }

    }
    public class TramaBCPResultadoDetalle : TramaBCPCabecera
    {
        //Código de identificación del depositante
        public string CodigoDepositante { get; set; }
        //Dato adicional del depositante
        public string DatoAdicionalDepositante { get; set; }
        //Fecha en que realizó el pago
        public string FechaPago { get; set; }
        //Fecha de vencimiento
        public string FechaVencimiento { get; set; }
        //Monto pagado
        public double MontoPagado { get; set; }
        //Monto de mora pagado
        public double MontoMoraPagada { get; set; }
        //Monto total pagado
        public double MontoTotalPagado { get; set; }
        //Sucursal / Agencia operativa
        public string CodigoOficina { get; set; }
        //Número de operación
        public long NumeroOperacion { get; set; }
        //Referencia
        public string Referencia { get; set; }
        //Identificación del Terminal
        public string IdTerminal { get; set; }
        //Medio de atención
        public string MedioAtencion { get; set; }
        //Hora de atención
        public string HoraAtencion { get; set; }
        //Número de cheque
        public long NumeroCheque { get; set; }
        //Código del Banco
        public int CodigoBanco { get; set; }
        //Cargo fijo pagado
        public double CargoFijo { get; set; }
        //Indicar valor “E”, si el registro fue extornado
        public string Retornado { get; set; }
    }
    #endregion
}
