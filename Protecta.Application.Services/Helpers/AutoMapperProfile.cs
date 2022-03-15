using System;
using AutoMapper;

using Protecta.Application.Service.Dtos;
using Protecta.Application.Service.Dtos.Entidad;

using Protecta.Application.Service.Dtos.State;
using Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg;

using Protecta.Domain.Service.StateModule.Aggregates.StateAgg;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

using Protecta.Application.Service.Dtos.Planilla;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;

using Protecta.Application.Service.Dtos.Consulta;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;

using Protecta.Application.Service.Dtos.Producto;
using Protecta.Domain.Service.ProductoModule.Aggregates.ProductoAgg;

using Protecta.Application.Service.Dtos.TipoArchivo;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;

using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;
using Protecta.Application.Service.Dtos.Deposito;

using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;
using Protecta.Application.Service.Dtos.Reporte;

using Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg;
using Protecta.Application.Service.Dtos.GeneracionExactus;

using Protecta.Domain.Service.CanalModule.Aggregates.CanalAgg;
using Protecta.Application.Service.Dtos.Canal;

using Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg;
using Protecta.Application.Service.Dtos.Factura;

namespace Protecta.Application.Service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PRO_USER, UserDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ID_USUARIO))
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.VC_COD_USUARIO))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.VC_NOMBRE_USUARIO))
                .ForMember(dest => dest.LastName,opts => opts.MapFrom(src => src.VC_APE_PATERNO));

            CreateMap<UserDto, PRO_USER>()
                .ForMember(dest => dest.ID_USUARIO, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.VC_COD_USUARIO, opts => opts.MapFrom(src => src.Username))
                .ForMember(dest => dest.VC_NOMBRE_USUARIO, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.VC_APE_PATERNO, opts => opts.MapFrom(src => src.LastName));

            CreateMap<PRO_RESOURCES, PRO_RESOURCESDto>();

            CreateMap<StateDto, State>();

            //Planillas
            CreateMap<PlanillaDto, Planilla>()
               .ForMember(dest => dest.DtFechaPlanilla, opts => opts.MapFrom(src => Convert.ToDateTime(src.DtFechaPlanilla)))
              ;
            CreateMap<Planilla, PlanillaDto>()
               .ForMember(dest => dest.DtFechaPlanilla, opts => opts.MapFrom(src => src.DtFechaPlanilla.ToString("dd/MM/yyyy")))
              ;

            CreateMap<DetallePlanillaCobro, DetallePlanillaCobroDto>();
            CreateMap<DetallePlanillaPago, DetallePlanillaPagoDto>();
            CreateMap<Producto, ProductoDto>();
            CreateMap<TipoArchivo, TipoArchivoDto>();
            CreateMap<DatosConsultaPlanilla, DatosConsultaPlanillaDto>();
            CreateMap<DatosConsultaArchivos, DatosConsultaArchivosDto>();

            //Control Bancario
            CreateMap<PlanillaEstado, PlanillaEstadoDto>();
            CreateMap<PlanillaDetalle, PlanillaDetalleDto>();
            CreateMap<PV_PAIDWAY, PV_PAIDWAYDto>();
            CreateMap<PREMIUM_MO, PREMIUM_MODto>();
            CreateMap<COLFORMREF, COLFORMREFDto>();

            //Canal
            CreateMap<Canal, CanalDto>();

            //Entidad
            CreateMap<EntidadDto, Entidad>()
              .ForMember(dest => dest.ID_ENTIDAD, opts => opts.MapFrom(src => src.IdEntidad))
              .ForMember(dest => dest.VC_NOMBRE, opts => opts.MapFrom(src => src.DescEntidad))
             ;
            CreateMap<Entidad, EntidadDto>()
              .ForMember(dest => dest.IdEntidad, opts => opts.MapFrom(src => src.ID_ENTIDAD))
              .ForMember(dest => dest.DescEntidad, opts => opts.MapFrom(src => src.VC_NOMBRE))
              ;

            //Cuenta
            CreateMap<CuentaDto, Cuenta>()
              .ForMember(dest => dest.ID_CUENTA, opts => opts.MapFrom(src => src.IdCuenta))
              .ForMember(dest => dest.ID_ENTIDAD, opts => opts.MapFrom(src => src.IdEntidad))
              .ForMember(dest => dest.NUMERO_CUENTA, opts => opts.MapFrom(src => src.NumeroCuenta))
              .ForMember(dest => dest.ID_MONEDA, opts => opts.MapFrom(src => src.IdMoneda))
               .ForMember(dest => dest.CODIGO_MONEDA, opts => opts.MapFrom(src => src.CodigoMoneda))
             ;
            CreateMap<Cuenta, CuentaDto>()
              .ForMember(dest => dest.IdCuenta, opts => opts.MapFrom(src => src.ID_CUENTA))
              .ForMember(dest => dest.IdEntidad, opts => opts.MapFrom(src => src.ID_ENTIDAD))
              .ForMember(dest => dest.NumeroCuenta, opts => opts.MapFrom(src => src.NUMERO_CUENTA))
              .ForMember(dest => dest.IdMoneda, opts => opts.MapFrom(src => src.ID_MONEDA))
              .ForMember(dest => dest.CodigoMoneda, opts => opts.MapFrom(src => src.CODIGO_MONEDA))
              ;

            //Factura
            CreateMap<DatosConsultaDocumento, DatosConsultaDocumentoDto>();
            CreateMap<FacturaDeposito, FacturaDepositoDto>();
            CreateMap<DetalleFacturaDeposito, DetalleFacturaDepositoDto>();
            CreateMap<DocumentoAbono, DocumentoAbonoDto>();

            //Consulta Planillas Procesadas
            CreateMap<PlanillaConsultaProcesadaDto, PlanillaConsultaProcesada>()
               .ForMember(
                    dest => dest.IdPlanilla,
                    opts => opts.MapFrom(src => long.Parse(src.IdPlanilla))
                )
               .ForMember(dest => dest.TotalPlanilla, opts => opts.MapFrom(src => decimal.Parse(src.TotalPlanilla)))
               .ForMember(dest => dest.FechaPlanilla, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaPlanilla)))
               //.ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => long.Parse(src.IdDeposito)))
               .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaDeposito)))
               .ForMember(dest => dest.TotalDeposito, opts => opts.MapFrom(src => decimal.Parse(src.TotalDeposito)))
               .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
               .ForMember(dest => dest.Usuario, opts => opts.MapFrom(src => src.Usuario))
               .ForMember(dest => dest.FechaConciliacion, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaConciliacion)))
            ;
            CreateMap<PlanillaConsultaProcesada, PlanillaConsultaProcesadaDto>()
              .ForMember(dest => dest.IdPlanilla, opts => opts.MapFrom(src => src.IdPlanilla.ToString()))
               .ForMember(dest => dest.TotalPlanilla, opts => opts.MapFrom(src => src.TotalPlanilla.ToString()))
               .ForMember(dest => dest.FechaPlanilla, opts => opts.MapFrom(src => src.FechaPlanilla.ToString("dd/MM/yyyy")))
               //.ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => src.IdDeposito.ToString()))
               .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => src.FechaDeposito.ToString("dd/MM/yyyy")))
               .ForMember(dest => dest.TotalDeposito, opts => opts.MapFrom(src => src.TotalDeposito.ToString()))
               .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
               .ForMember(dest => dest.Usuario, opts => opts.MapFrom(src => src.Usuario))
               .ForMember(dest => dest.FechaConciliacion, opts => opts.MapFrom(src => src.FechaConciliacion.ToString("dd/MM/yyyy")))
               ;

            CreateMap<DepositoDto, Deposito>()
            .ForMember(dest => dest.IdDepositArchivo, opts => opts.MapFrom(src => src.IdDepositArchivo))
            .ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => src.IdDeposito))
            .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaDeposito)))
            .ForMember(dest => dest.IdEstado, opts => opts.MapFrom(src => src.IdEstado))
            .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
            .ForMember(dest => dest.Saldo, opts => opts.MapFrom(src => src.Saldo))
            .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
            .ForMember(dest => dest.IdMoneda, opts => opts.MapFrom(src => src.IdMoneda))
            ;

            CreateMap<Deposito, DepositoDto>()
                .ForMember(dest => dest.IdDepositArchivo, opts => opts.MapFrom(src => src.IdDepositArchivo))
                .ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => src.IdDeposito))
                .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => src.FechaDeposito.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.IdEstado, opts => opts.MapFrom(src => src.IdEstado))
                .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
                .ForMember(dest => dest.Saldo, opts => opts.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
                .ForMember(dest => dest.IdMoneda, opts => opts.MapFrom(src => src.IdMoneda))
                ;

            CreateMap<ReportePlanillaPendienteDto, ReportePlanillaPendiente>()
                .ForMember(dest => dest.IdPlanilla, opts => opts.MapFrom(src => src.IdPlanilla))
                .ForMember(dest => dest.FechaPlanilla, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaPlanilla)))
                .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
                .ForMember(dest => dest.DescripcionMedioPago, opts => opts.MapFrom(src => src.DescripcionMedioPago))
                .ForMember(dest => dest.FechaProceso, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaProceso)))
                .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
            ;

            CreateMap<ReportePlanillaPendiente, ReportePlanillaPendienteDto>()
                .ForMember(dest => dest.IdPlanilla, opts => opts.MapFrom(src => src.IdPlanilla))
                .ForMember(dest => dest.FechaPlanilla, opts => opts.MapFrom(src => src.FechaPlanilla))
                //.ForMember(dest => dest.FechaPlanilla, opts => opts.MapFrom(src => (src.FechaPlanilla != null && src.FechaPlanilla != DateTime.MinValue && src.FechaPlanilla.ToString("dd/MM/yyyy") != "01/01/0001") ? src.FechaPlanilla.ToString("dd/MM/yyyy") : ""))
                .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
                .ForMember(dest => dest.DescripcionMedioPago, opts => opts.MapFrom(src => src.DescripcionMedioPago))
                .ForMember(dest => dest.FechaProceso, opts => opts.MapFrom(src => src.FechaProceso))
                //.ForMember(dest => dest.FechaProceso, opts => opts.MapFrom(src => (src.FechaProceso != null && src.FechaProceso != DateTime.MinValue && src.FechaProceso.ToString("dd/MM/yyyy") != "01/01/0001") ? src.FechaProceso.ToString("dd/MM/yyyy") : ""))
                .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
                ;


            CreateMap<ReporteDepositoPendienteDto, ReporteDepositoPendiente>()
               .ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => src.IdDeposito))             
               .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
               .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
               .ForMember(dest => dest.Saldo, opts => opts.MapFrom(src => src.Saldo))
               .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => Convert.ToDateTime(src.FechaDeposito)))
               .ForMember(dest => dest.NombreArchivo, opts => opts.MapFrom(src => src.NombreArchivo))
               .ForMember(dest => dest.Cuenta, opts => opts.MapFrom(src => src.Cuenta))
               .ForMember(dest => dest.Banco, opts => opts.MapFrom(src => src.Banco))
             
            ;

            CreateMap<ReporteDepositoPendiente, ReporteDepositoPendienteDto>()
                .ForMember(dest => dest.IdDeposito, opts => opts.MapFrom(src => src.IdDeposito))
                .ForMember(dest => dest.NumeroOperacion, opts => opts.MapFrom(src => src.NumeroOperacion))
                .ForMember(dest => dest.Monto, opts => opts.MapFrom(src => src.Monto))
                .ForMember(dest => dest.Saldo, opts => opts.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.FechaDeposito, opts => opts.MapFrom(src => (src.FechaDeposito != null && src.FechaDeposito != DateTime.MinValue && src.FechaDeposito.ToString("dd/MM/yyyy") != "01/01/0001") ? src.FechaDeposito.ToString("dd/MM/yyyy") : ""))
                .ForMember(dest => dest.NombreArchivo, opts => opts.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.Cuenta, opts => opts.MapFrom(src => src.Cuenta))
                .ForMember(dest => dest.Banco, opts => opts.MapFrom(src => src.Banco))

            ;
        }
    }
}
