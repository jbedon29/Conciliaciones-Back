using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg
{
    public interface ICobranzaRepository
    {
        Task<List<Banco>> ListarBancos();
        Task<List<Cuenta>> ListarCuenta(int idBanco);
        Task<Conciliacion> ValidarTrama(Trama trama);
        Task<Planilla> ObtenerTrama(Trama trama);
        Task<bool> InsertarProceso(List<ListaConciliacion> listaConciliacions);
        Task<ResponseControl> GeneraPlanillaFactura(string idproceso, int idproducto, int idbanco, string tipooperacion, int usercode);
        Task<ResponseControl> Validar_Planilla_Factura(ListaConciliacion listaConciliacions);
        Task<ResponseControl> ObtenerLiquidacionManual(string idproceso, int idproducto, int idbanco, string StrProforma, string fechaInicio, string fechaFin, string usercode);
        Task<ResponseControl> ObtenerFormaPago(int idBanco, string idProceso);
        Task<bool> Insertar_Respuesta_FE(State_voucher _Voucher);
        Task<List<Tipo_Pago>> ListarTipoPago();
        Task<ResponseControl> ValidarDepositoArchivo(Loadtransaction _request);
        Task<ResponseControl> DeleteDepositosArchivo(Loadtransaction _request);

        Task<ResponseControl> InsertDepositosArchivoVisa(Loadtransaction _request);

        Task<ResponseControl> InsertDepositosArchivoPE(Loadtransaction _request);

        Task<ResponseControl> InsertarDepositosArchivo(DepositTransaccion _request, Loadtransaction _CabRequest);

        Task<ResponseControl> InsertarDepositosArchivoByVisa(DepositTransaccionVisa _request, Loadtransaction _CabRequest);
        Task<ResponseControl> DeleteCargaDeposito(Loadtransaction _request);

        Task<List<RamoPay>> ListarRamoPay();
        Task<List<EstadoRecibo>> ListarEstadoRecibo();

        Task<List<EstadoEnvio>> ListarEstadoEnvioCE();

        Task<List<DatosRespuestaProductoPay>> ListarProductoPay(DatosConsultaProductoPay datosConsultaProducto);
        Task<ReporteContratantePay> ListarContratantePay(DatosConsultaContratantePay datosConsulta);
        Task<List<ReporteReciboPay>> ListarReciboPay(DatosConsultaReciboPay datosConsulta);

        Task<List<ReporteEnvioPay>> ListarEnvioPay(DatosConsultaEnvioPay datosConsulta);

        Task<List<ReporteReciboDetallePay>> ListarReciboDetalle(DatosConsultaReciboDetallePay datosConsulta);
        Task<ResponseControl> ActualizarMotivos(long nroEnvio, DatosConsultaReciboActualizar DatosConsultaReciboDetallePay);

        Task<ResponseControl> ins_user_cabecera(long nUserCode);
        Task<ResponseControl> ins_detalle(Detalles det, DatosConsultaReciboCobro datosConsultaEntity, int nEstado, DatosRespuestaServicio datosRespuestaServicio, object data);
        Task<ResponseControl> ins_cabecera(object data);
        Task<ResponseControl> StatusCierre(Detalles det, DatosConsultaReciboCobro datosConsultaEntity);
        Task<DatosRespuestaContratanteNC> ListarContratanteNC(DatosConsultaContratanteNC datosConsulta);
        Task<List<ReporteReciboPendienteNC>> ListarReciboPendienteNC(DatosConsultaReciboPendienteNC datosConsulta);
        Task<List<ReporteFormaPago>> ReporteFormaPago(DatosConsultaFormaPago datosConsulta);


        Task<List<TipoFP>> ListarTipoFP();

        //12/10/21
        //Task<bool> InsertarPlanilla(List<ListaConciliacion> listaConciliacions);
        //Task<ResponseControl> GeneraPlanillaFactura(string idproceso, int idproducto, int idbanco, string tipooperacion, int usercode);


        Task<List<ReporteSeguimientoLote>> ReporteSeguimientoLote(DatosConsultaSeguimientoLote datosConsulta);





    }
}
