using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Transactions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.GeneralModule.Aggregates.GeneralAgg;
using Protecta.Infrastructure.Data.ProcesoModule.Repositories;

namespace Protecta.Infrastructure.Data.GeneracionExactusModule.Repositories
{
    public class GeneracionExactusRepository : IGeneracionExactusRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;
        private readonly ProcesoRepository _procesoRepository;

        public GeneracionExactusRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;

            _procesoRepository = new ProcesoRepository(appSettings, ConnectionBase);
        }     

        #region Métodos Generación de Interfaces

        public List<PlanillaEstado> ListarPlanillaConciliada(DatosConsultaArchivos datosConsultaArchivos)
        {
            int _indFecPlanilla = 0;
            string _fechaDesde = string.Empty;
            string _fechaHasta = string.Empty;
            string _usuario = string.Empty;

            PlanillaEstado planillaEstadoEntity = null;
            List<PlanillaEstado> planillaEstadoList = new List<PlanillaEstado>();

            if (datosConsultaArchivos.FechaDesde != "" || datosConsultaArchivos.FechaHasta != "")
            {
                _indFecPlanilla = 1;
                _fechaDesde = Convert.ToDateTime(datosConsultaArchivos.FechaDesde).ToShortDateString();
                _fechaHasta = Convert.ToDateTime(datosConsultaArchivos.FechaHasta).ToShortDateString();
                _usuario = datosConsultaArchivos.Usuario.ToString();
            }
            else
            {
                _indFecPlanilla = 0;
                _fechaDesde = string.Empty;
                _fechaHasta = string.Empty;
                _usuario = datosConsultaArchivos.Usuario.ToString();
            }

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("INDFECPLA", OracleDbType.Int32, _indFecPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECDESDE", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECHASTA", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREAPLANILLACONCILIADA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    planillaEstadoEntity = new PlanillaEstado();

                    if (dr["ID_PLANILLA"] != null && dr["ID_PLANILLA"].ToString() != "")
                    {
                        planillaEstadoEntity.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"]);
                    }

                    planillaEstadoEntity.VcUsuariocreacion = _usuario.ToString(); //Agregado 16/07/2018

                    planillaEstadoList.Add(planillaEstadoEntity);
                }
            }

            return planillaEstadoList;
        }

        public List<PlanillaEstado> ListarPlanillaDocumentoLiquidado(DatosConsultaArchivos datosConsultaArchivos)
        {
            int _indFecPlanilla = 0;
            string _fechaDesde = string.Empty;
            string _fechaHasta = string.Empty;
            string _usuario = string.Empty;

            PlanillaEstado planillaEstadoEntity = null;
            List<PlanillaEstado> planillaEstadoList = new List<PlanillaEstado>();

            if (datosConsultaArchivos.FechaDesde != "" || datosConsultaArchivos.FechaHasta != "")
            {
                _indFecPlanilla = 1;
                _fechaDesde = Convert.ToDateTime(datosConsultaArchivos.FechaDesde).ToShortDateString();
                _fechaHasta = Convert.ToDateTime(datosConsultaArchivos.FechaHasta).ToShortDateString();
                _usuario = datosConsultaArchivos.Usuario.ToString();
            }
            else
            {
                _indFecPlanilla = 0;
                _fechaDesde = string.Empty;
                _fechaHasta = string.Empty;
                _usuario = datosConsultaArchivos.Usuario.ToString();
            }

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("INDFECPLA", OracleDbType.Int32, _indFecPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECDESDE", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECHASTA", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREAPLANILLACONDOCLIQ", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    planillaEstadoEntity = new PlanillaEstado();

                    if (dr["ID_PLANILLA"] != null && dr["ID_PLANILLA"].ToString() != "")
                    {
                        planillaEstadoEntity.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"]);
                    }

                    planillaEstadoEntity.VcUsuariocreacion = _usuario.ToString(); //Agregado 16/07/2018

                    planillaEstadoList.Add(planillaEstadoEntity);
                }
            }

            return planillaEstadoList;
        }

        public string RegistrarPlanillaDetalle(PlanillaEstado planillaEstado)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_ID_PLANILLA", OracleDbType.Int32, planillaEstado.IdPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_VC_USUARIO", OracleDbType.NVarchar2, planillaEstado.VcUsuariocreacion, ParameterDirection.Input));
            //Parámetro de Salida
            var pResult01 = new OracleParameter("P_IN_RESPUESTA", OracleDbType.Int32, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult01);

            var pResult02 = new OracleParameter("P_VC_RESPUESTA_MENSAJE", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(pResult02);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLAPLICA_LIQUIDACIONES", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult01.Value.ToString() + pResult02.Value.ToString();

            return _sMessage;
        }

        public List<DetallePlanillaCobro> ListarPlanillaCertificado(PlanillaEstado planillaEstado)
        {
            DetallePlanillaCobro detallePlanillaCobroEntity = null;
            List<DetallePlanillaCobro> detallePlanillaCobroList = new List<DetallePlanillaCobro>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("ID_PLANILLA", OracleDbType.Int32, planillaEstado.IdPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREAPLANILLACERTIFICADO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    detallePlanillaCobroEntity = new DetallePlanillaCobro();

                    if (dr["ID_PLANILLA"] != null && dr["ID_PLANILLA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"]);
                    }

                    if (dr["ID_PRODUCTO"] != null && dr["ID_PRODUCTO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdProducto = Convert.ToInt32(dr["ID_PRODUCTO"]);
                    }

                    if (dr["ID_RAMO"] != null && dr["ID_RAMO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdRamo = Convert.ToInt32(dr["ID_RAMO"]);
                    }

                    if (dr["VC_NUMERO_POLIZA"] != null && dr["VC_NUMERO_POLIZA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.VcNumeropoliza = Convert.ToInt64(dr["VC_NUMERO_POLIZA"]);
                    }

                    if (dr["VC_NUMERO_CERTIFICADO"] != null && dr["VC_NUMERO_CERTIFICADO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.VcNumerocertificado = Convert.ToInt64(dr["VC_NUMERO_CERTIFICADO"]);
                    }

                    if (dr["ID_PROFORMA"] != null && dr["ID_PROFORMA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdProforma = Convert.ToInt64(dr["ID_PROFORMA"]);
                    }

                    detallePlanillaCobroEntity.VcUsuariocreacion = planillaEstado.VcUsuariocreacion.ToString(); //Agregado 16/07/2018

                    detallePlanillaCobroList.Add(detallePlanillaCobroEntity);
                }
            }

            return detallePlanillaCobroList;
        }

        public List<PREMIUM_MO> ListarReciboAbonado(PlanillaDetalle planillaDetalle)
        {
            PREMIUM_MO premium_mo = null;
            List<PREMIUM_MO> premium_moList = new List<PREMIUM_MO>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int32, planillaDetalle.IdProforma, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSREAPREMIUM_MO_LIQ", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    premium_mo = new PREMIUM_MO();

                    if (dr["NRECEIPT"] != null && dr["NRECEIPT"].ToString() != "")
                    {
                        premium_mo.NRECEIPT = Convert.ToInt64(dr["NRECEIPT"]);
                    }

                    if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                    {
                        premium_mo.NPRODUCT = Convert.ToInt32(dr["NPRODUCT"]);
                    }

                    if (dr["NBRANCH"] != null && dr["NBRANCH"].ToString() != "")
                    {
                        premium_mo.NBRANCH = Convert.ToInt32(dr["NBRANCH"]);
                    }

                    if (dr["SCERTYPE"] != null && dr["SCERTYPE"].ToString() != "")
                    {
                        premium_mo.SCERTYPE = dr["SCERTYPE"].ToString();
                    }
                    else
                    {
                        premium_mo.SCERTYPE = "";
                    }

                    if (dr["NDIGIT"] != null && dr["NDIGIT"].ToString() != "")
                    {
                        premium_mo.NDIGIT = Convert.ToInt32(dr["NDIGIT"]);
                    }

                    if (dr["NPAYNUMBE"] != null && dr["NPAYNUMBE"].ToString() != "")
                    {
                        premium_mo.NPAYNUMBE = Convert.ToInt32(dr["NPAYNUMBE"]);
                    }

                    if (dr["NTRANSAC"] != null && dr["NTRANSAC"].ToString() != "")
                    {
                        premium_mo.NTRANSAC = Convert.ToInt32(dr["NTRANSAC"]);
                    }

                    if (dr["DC_MONTO"] != null && dr["DC_MONTO"].ToString() != "")
                    {
                        premium_mo.NAMOUNT = Convert.ToInt64(dr["DC_MONTO"]);
                    }

                    if (dr["NBALANCE"] != null && dr["NBALANCE"].ToString() != "")
                    {
                        premium_mo.NBALANCE = Convert.ToInt64(dr["NBALANCE"]);
                    }

                    if (dr["NBORDEREAUX"] != null && dr["NBORDEREAUX"].ToString() != "")
                    {
                        premium_mo.NBORDEREAUX = Convert.ToInt32(dr["NBORDEREAUX"]);
                    }

                    if (dr["SCESSICOI"] != null && dr["SCESSICOI"].ToString() != "")
                    {
                        premium_mo.SCESSICOI = dr["SCESSICOI"].ToString();
                    }
                    else
                    {
                        premium_mo.SCESSICOI = "";
                    }

                    if (dr["DCOMPDATE"] != null && dr["DCOMPDATE"].ToString() != "")
                    {
                        premium_mo.DCOMPDATE = Convert.ToDateTime(dr["DCOMPDATE"]);
                    }

                    if (dr["ID_MONEDA"] != null && dr["ID_MONEDA"].ToString() != "")
                    {
                        premium_mo.NCURRENCY = Convert.ToInt32(dr["ID_MONEDA"]);
                    }

                    if (dr["SIND_REVER"] != null && dr["SIND_REVER"].ToString() != "")
                    {
                        premium_mo.SIND_REVER = dr["SIND_REVER"].ToString();
                    }
                    else
                    {
                        premium_mo.SIND_REVER = "";
                    }

                    if (dr["NINT_MORA"] != null && dr["NINT_MORA"].ToString() != "")
                    {
                        premium_mo.NINT_MORA = Convert.ToInt32(dr["NINT_MORA"]);
                    }                   

                    if (dr["SINTERMEI"] != null && dr["SINTERMEI"].ToString() != "")
                    {
                        premium_mo.SINTERMEI = dr["SINTERMEI"].ToString();
                    }
                    else
                    {
                        premium_mo.SINTERMEI = "";
                    }

                    if (dr["ID_TIPO_MEDIO_PAGO"] != null && dr["ID_TIPO_MEDIO_PAGO"].ToString() != "")
                    {
                        premium_mo.SPAY_FORM = dr["ID_TIPO_MEDIO_PAGO"].ToString();
                    }
                    else
                    {
                        premium_mo.SPAY_FORM = "";
                    }

                    if (dr["DPOSTED"] != null && dr["DPOSTED"].ToString() != "")
                    {
                        premium_mo.DPOSTED = Convert.ToDateTime(dr["DPOSTED"]);
                    }

                    if (dr["NPREMIUM"] != null && dr["NPREMIUM"].ToString() != "")
                    {
                        premium_mo.NPREMIUM = Convert.ToInt64(dr["NPREMIUM"]);
                    }

                    if (dr["DSTATDATE"] != null && dr["DSTATDATE"].ToString() != "")
                    {
                        premium_mo.DSTATDATE = Convert.ToDateTime(dr["DSTATDATE"]);
                    }

                    if (dr["SSTATISI"] != null && dr["SSTATISI"].ToString() != "")
                    {
                        premium_mo.SSTATISI = dr["SSTATISI"].ToString();
                    }
                    else
                    {
                        premium_mo.SSTATISI = "";
                    }

                    if (dr["NUSERCODE"] != null && dr["NUSERCODE"].ToString() != "")
                    {
                        premium_mo.NUSERCODE = Convert.ToInt32(dr["NUSERCODE"]);
                    }

                    if (dr["DLEDGERDAT"] != null && dr["DLEDGERDAT"].ToString() != "")
                    {
                        premium_mo.DLEDGERDAT = Convert.ToDateTime(dr["DLEDGERDAT"]);
                    }

                    if (dr["NTYPE"] != null && dr["NTYPE"].ToString() != "")
                    {
                        premium_mo.NTYPE = Convert.ToInt32(dr["NTYPE"]);
                    }

                    if (dr["NEXCHANGE"] != null && dr["NEXCHANGE"].ToString() != "")
                    {
                        premium_mo.NEXCHANGE = Convert.ToInt32(dr["NEXCHANGE"]);
                    }

                    if (dr["SINDASSOCPRO"] != null && dr["SINDASSOCPRO"].ToString() != "")
                    {
                        premium_mo.SINDASSOCPRO = dr["SINDASSOCPRO"].ToString();
                    }
                    else
                    {
                        premium_mo.SINDASSOCPRO = "";
                    }

                    if (dr["NID"] != null && dr["NID"].ToString() != "")
                    {
                        premium_mo.NID = Convert.ToInt32(dr["NID"]);
                    }

                    /*Agregado*/
                    if (dr["NBANK_CODE"] != null && dr["NBANK_CODE"].ToString() != "")
                    {
                        premium_mo.NBANK_CODE = Convert.ToInt32(dr["NBANK_CODE"]);
                    }

                    if (dr["NCASH_MOV"] != null && dr["NCASH_MOV"].ToString() != "")
                    {
                        premium_mo.NCASH_MOV = Convert.ToInt64(dr["NCASH_MOV"]);
                    }

                    if (dr["NBILLNUM"] != null && dr["NBILLNUM"].ToString() != "")
                    {
                        premium_mo.NBILLNUM = Convert.ToInt32(dr["NBILLNUM"]);
                    }

                    if (dr["SBILLTYPE"] != null && dr["SBILLTYPE"].ToString() != "")
                    {
                        premium_mo.SBILLTYPE = dr["SBILLTYPE"].ToString();
                    }
                    else
                    {
                        premium_mo.SBILLTYPE = "";
                    }

                    premium_moList.Add(premium_mo);
                }
            }

            return premium_moList;
        }

        public string RegistrarReciboAbonado(PREMIUM_MO premiun_mo)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, premiun_mo.NRECEIPT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, premiun_mo.NPRODUCT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, premiun_mo.NBRANCH, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SCERTYPE", OracleDbType.NVarchar2, premiun_mo.SCERTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NDIGIT", OracleDbType.Int32, premiun_mo.NDIGIT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPAYNUMBE", OracleDbType.Int32, premiun_mo.NPAYNUMBE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NTRANSAC", OracleDbType.Int32, premiun_mo.NTRANSAC, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NAMOUNT", OracleDbType.Int64, premiun_mo.NAMOUNT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBALANCE", OracleDbType.Int64, premiun_mo.NBALANCE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBORDEREAUX", OracleDbType.Int32, premiun_mo.NBORDEREAUX, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SCESSICOI", OracleDbType.NVarchar2, premiun_mo.SCESSICOI, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DCOMPDATE", OracleDbType.Date, premiun_mo.DCOMPDATE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCURRENCY", OracleDbType.Int32, premiun_mo.NCURRENCY, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SIND_REVER", OracleDbType.NVarchar2, premiun_mo.SIND_REVER, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NINT_MORA", OracleDbType.Int32, premiun_mo.NINT_MORA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SINTERMEI", OracleDbType.NVarchar2, premiun_mo.SINTERMEI, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("SPAY_FORM", OracleDbType.NVarchar2, premiun_mo.SPAY_FORM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DPOSTED", OracleDbType.Date, premiun_mo.DPOSTED, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NPREMIUM", OracleDbType.Int64, premiun_mo.NPREMIUM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DSTATDATE", OracleDbType.Date, premiun_mo.DSTATDATE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SSTATISI", OracleDbType.NVarchar2, premiun_mo.SSTATISI, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, premiun_mo.NUSERCODE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DLEDGERDAT", OracleDbType.Date, premiun_mo.DLEDGERDAT, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NTYPE", OracleDbType.Int32, premiun_mo.NTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NEXCHANGE", OracleDbType.Int32, premiun_mo.NEXCHANGE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SINDASSOCPRO", OracleDbType.NVarchar2, premiun_mo.SINDASSOCPRO, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NID", OracleDbType.Int32, premiun_mo.NID, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBANK_CODE", OracleDbType.Int32, premiun_mo.NBANK_CODE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCASH_MOV", OracleDbType.Int64, premiun_mo.NCASH_MOV, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBILLNUM", OracleDbType.Int32, premiun_mo.NBILLNUM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SBILLTYPE", OracleDbType.NVarchar2, premiun_mo.SBILLTYPE, ParameterDirection.Input));

            //Parámetro de Salida
            var pResult = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSPOSTPREMIUM_MO_LIQ", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return _sMessage;
        }
        
        public List<COLFORMREF> ListarCobroAbonado(PlanillaDetalle planillaDetalle)
        {
            COLFORMREF colformref = null;
            List<COLFORMREF> colformrefList = new List<COLFORMREF>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int32, planillaDetalle.IdProforma, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSREACOLFORMREF_LIQ", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    colformref = new COLFORMREF();

                    if (dr["NBORDEREAUX"] != null && dr["NBORDEREAUX"].ToString() != "")
                    {
                        colformref.NBORDEREAUX = Convert.ToInt32(dr["NBORDEREAUX"]);
                    }

                    if (dr["STYPE"] != null && dr["STYPE"].ToString() != "")
                    {
                        colformref.STYPE = dr["STYPE"].ToString();
                    }
                    else
                    {
                        colformref.STYPE = "";
                    }

                    if (dr["NBANK"] != null && dr["NBANK"].ToString() != "")
                    {
                        colformref.NBANK = Convert.ToInt32(dr["NBANK"]);
                    }

                    if (dr["NINTERMED"] != null && dr["NINTERMED"].ToString() != "")
                    {
                        colformref.NINTERMED = Convert.ToInt32(dr["NINTERMED"]);
                    }

                    if (dr["NREL_AMOUN"] != null && dr["NREL_AMOUN"].ToString() != "")
                    {
                        colformref.NREL_AMOUN = Convert.ToInt64(dr["NREL_AMOUN"]);
                    }

                    if (dr["NCURRENCY"] != null && dr["NCURRENCY"].ToString() != "")
                    {
                        colformref.NCURRENCY = Convert.ToInt32(dr["NCURRENCY"]);
                    }

                    if (dr["NPOLICY"] != null && dr["NPOLICY"].ToString() != "")
                    {
                        colformref.NPOLICY = Convert.ToInt64(dr["NPOLICY"]);
                    }

                    if (dr["DCOLLECT"] != null && dr["DCOLLECT"].ToString() != "")
                    {
                        colformref.DCOLLECT = Convert.ToDateTime(dr["DCOLLECT"]); 
                    }

                    if (dr["NBRANCH"] != null && dr["NBRANCH"].ToString() != "")
                    {
                        colformref.NBRANCH = Convert.ToInt32(dr["NBRANCH"]);
                    }

                    if (dr["NOFFICE"] != null && dr["NOFFICE"].ToString() != "")
                    {
                        colformref.NOFFICE = Convert.ToInt32(dr["NOFFICE"]);
                    }

                    if (dr["DCOMPDATE"] != null && dr["DCOMPDATE"].ToString() != "")
                    {
                        colformref.DCOMPDATE = Convert.ToDateTime(dr["DCOMPDATE"]);
                    }

                    if (dr["NUSERCODE"] != null && dr["NUSERCODE"].ToString() != "")
                    {
                        colformref.NUSERCODE = Convert.ToInt32(dr["NUSERCODE"]);
                    }

                    if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                    {
                        colformref.NPRODUCT = Convert.ToInt32(dr["NPRODUCT"]);
                    }

                    if (dr["SREL_TYPE"] != null && dr["SREL_TYPE"].ToString() != "")
                    {
                        colformref.SREL_TYPE = dr["SREL_TYPE"].ToString();
                    }
                    else
                    {
                        colformref.SREL_TYPE = "";
                    }

                    if (dr["SSTATUS"] != null && dr["SSTATUS"].ToString() != "")
                    {
                        colformref.SSTATUS = dr["SSTATUS"].ToString();
                    }
                    else
                    {
                        colformref.SSTATUS = "";
                    }

                    if (dr["NCERTIF"] != null && dr["NCERTIF"].ToString() != "")
                    {
                        colformref.NCERTIF = Convert.ToInt32(dr["NCERTIF"]);
                    }

                    if (dr["SCONWIN"] != null && dr["SCONWIN"].ToString() != "")
                    {
                        colformref.SCONWIN = dr["SCONWIN"].ToString();
                    }
                    else
                    {
                        colformref.SCONWIN = "";
                    }

                    if (dr["SCLIENT"] != null && dr["SCLIENT"].ToString() != "")
                    {
                        colformref.SCLIENT = dr["SCLIENT"].ToString();
                    }
                    else
                    {
                        colformref.SCLIENT = "";
                    }

                    if (dr["NUSER_AMEND"] != null && dr["NUSER_AMEND"].ToString() != "")
                    {
                        colformref.NUSER_AMEND = Convert.ToInt32(dr["NUSER_AMEND"]);
                    }

                    if (dr["NINPUTTYP"] != null && dr["NINPUTTYP"].ToString() != "")
                    {
                        colformref.NINPUTTYP = Convert.ToInt32(dr["NINPUTTYP"]);
                    }

                    if (dr["NINSUR_AREA"] != null && dr["NINSUR_AREA"].ToString() != "")
                    {
                        colformref.NINSUR_AREA = Convert.ToInt32(dr["NINSUR_AREA"]);
                    }

                    if (dr["DVALUEDATE"] != null && dr["DVALUEDATE"].ToString() != "")
                    {
                        colformref.DVALUEDATE = Convert.ToDateTime(dr["DVALUEDATE"]);
                    }

                    if (dr["SIND_ANNUITY"] != null && dr["SIND_ANNUITY"].ToString() != "")
                    {
                        colformref.SIND_ANNUITY = dr["SIND_ANNUITY"].ToString();
                    }
                    else
                    {
                        colformref.SIND_ANNUITY = "";
                    }

                    if (dr["SRELORIGI"] != null && dr["SRELORIGI"].ToString() != "")
                    {
                        colformref.SRELORIGI = dr["SRELORIGI"].ToString();
                    }
                    else
                    {
                        colformref.SRELORIGI = "";
                    }

                    /*Agregado*/
                    if (dr["NUMERO_OPERACION"] != null && dr["NUMERO_OPERACION"].ToString() != "")
                    {
                        colformref.NUMERO_OPERACION = dr["NUMERO_OPERACION"].ToString();
                    }
                    else
                    {
                        colformref.NUMERO_OPERACION = "";
                    }

                    if (dr["NUMERO_CUENTA"] != null && dr["NUMERO_CUENTA"].ToString() != "")
                    {
                        colformref.NUMERO_CUENTA = dr["NUMERO_CUENTA"].ToString();
                    }
                    else
                    {
                        colformref.NUMERO_CUENTA = "";
                    }

                    colformrefList.Add(colformref);
                }
            }

            return colformrefList;
        }

        public string RegistrarCobroAbonado(COLFORMREF colformref)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();           
            parameters.Add(new OracleParameter("STYPE", OracleDbType.NVarchar2, colformref.STYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBORDEREAUX", OracleDbType.Int32, colformref.NBORDEREAUX, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBANK", OracleDbType.Int32, colformref.NBANK, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINTERMED", OracleDbType.Int32, colformref.NINTERMED, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NREL_AMOUN", OracleDbType.Int64, colformref.NREL_AMOUN, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCURRENCY", OracleDbType.Int32, colformref.NCURRENCY, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPOLICY", OracleDbType.Int64, colformref.NPOLICY, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DCOLLECT", OracleDbType.Date, colformref.DCOLLECT, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, colformref.NBRANCH, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NOFFICE", OracleDbType.Int32, colformref.NOFFICE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DCOMPDATE", OracleDbType.Date, colformref.DCOMPDATE, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, colformref.NUSERCODE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, colformref.NPRODUCT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SREL_TYPE", OracleDbType.NVarchar2, colformref.SREL_TYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SSTATUS", OracleDbType.NVarchar2, colformref.SSTATUS, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCERTIF", OracleDbType.Int32, colformref.NCERTIF, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SCONWIN", OracleDbType.NVarchar2, colformref.SCONWIN, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SCLIENT", OracleDbType.NVarchar2, colformref.SCLIENT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSER_AMEND", OracleDbType.Int32, colformref.NUSER_AMEND, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINPUTTYP", OracleDbType.Int32, colformref.NINPUTTYP, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINSUR_AREA", OracleDbType.Int32, colformref.NINSUR_AREA, ParameterDirection.Input));           
            parameters.Add(new OracleParameter("DVALUEDATE", OracleDbType.Date, colformref.DVALUEDATE, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("SIND_ANNUITY", OracleDbType.NVarchar2, colformref.SIND_ANNUITY, ParameterDirection.Input));            
            parameters.Add(new OracleParameter("SRELORIGI", OracleDbType.NVarchar2, colformref.SRELORIGI, ParameterDirection.Input));           
            parameters.Add(new OracleParameter("NUMERO_OPERACION", OracleDbType.NVarchar2, colformref.NUMERO_OPERACION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUMERO_CUENTA", OracleDbType.NVarchar2, colformref.NUMERO_CUENTA, ParameterDirection.Input));

            //Parámetro de Salida
            var pResult = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSPOSTCOLFORMREF_LIQ", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return _sMessage;
        }

        public List<COMM_VISANET> ListarComisionCabeceraAbonado(long idProforma)
        {
            COMM_VISANET comm_visanet = null;
            List<COMM_VISANET> comm_visanetList = new List<COMM_VISANET>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, idProforma, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSREACOMM_VISANET_LIQ", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    comm_visanet = new COMM_VISANET();

                    if (dr["SCERTYPE"] != null && dr["SCERTYPE"].ToString() != "")
                    {
                        comm_visanet.SCERTYPE = dr["SCERTYPE"].ToString();
                    }
                    else
                    {
                        comm_visanet.SCERTYPE = "";
                    }

                    if (dr["NBRANCH"] != null && dr["NBRANCH"].ToString() != "")
                    {
                        comm_visanet.NBRANCH = Convert.ToInt32(dr["NBRANCH"]);
                    }

                    if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                    {
                        comm_visanet.NPRODUCT = Convert.ToInt32(dr["NPRODUCT"]);
                    }

                    if (dr["NPOLICY"] != null && dr["NPOLICY"].ToString() != "")
                    {
                        comm_visanet.NPOLICY = Convert.ToInt64(dr["NPOLICY"]);
                    }

                    if (dr["NCOMERCIO"] != null && dr["NCOMERCIO"].ToString() != "")
                    {
                        comm_visanet.NCOMERCIO = Convert.ToInt32(dr["NCOMERCIO"]);
                    }

                    if (dr["NOMBRE_COMERCIAL"] != null && dr["NOMBRE_COMERCIAL"].ToString() != "")
                    {
                        comm_visanet.NOMBRE_COMERCIAL = dr["NOMBRE_COMERCIAL"].ToString();
                    }
                    else
                    {
                        comm_visanet.NOMBRE_COMERCIAL = "";
                    }

                    if (dr["DEFFECDATE"] != null && dr["DEFFECDATE"].ToString() != "")
                    {
                        comm_visanet.DEFFECDATE = Convert.ToDateTime(dr["DEFFECDATE"]);
                    }

                    if (dr["FEC_TRANSACCION"] != null && dr["FEC_TRANSACCION"].ToString() != "")
                    {
                        comm_visanet.FEC_TRANSACCION = Convert.ToDateTime(dr["FEC_TRANSACCION"]);
                    }

                    if (dr["FEC_PROCESO"] != null && dr["FEC_PROCESO"].ToString() != "")
                    {
                        comm_visanet.FEC_PROCESO = Convert.ToDateTime(dr["FEC_PROCESO"]);
                    }

                    if (dr["TIPO_OPERACION"] != null && dr["TIPO_OPERACION"].ToString() != "")
                    {
                        comm_visanet.TIPO_OPERACION = dr["TIPO_OPERACION"].ToString();
                    }
                    else
                    {
                        comm_visanet.TIPO_OPERACION = "";
                    }

                    if (dr["DESC_COD_CONTABLE"] != null && dr["DESC_COD_CONTABLE"].ToString() != "")
                    {
                        comm_visanet.DESC_COD_CONTABLE = dr["DESC_COD_CONTABLE"].ToString();
                    }
                    else
                    {
                        comm_visanet.DESC_COD_CONTABLE = "";
                    }

                    if (dr["NUM_TARJETA"] != null && dr["NUM_TARJETA"].ToString() != "")
                    {
                        comm_visanet.NUM_TARJETA = dr["NUM_TARJETA"].ToString();
                    }
                    else
                    {
                        comm_visanet.NUM_TARJETA = "";
                    }

                    if (dr["ORI_TARJETA"] != null && dr["ORI_TARJETA"].ToString() != "")
                    {
                        comm_visanet.ORI_TARJETA = dr["ORI_TARJETA"].ToString();
                    }
                    else
                    {
                        comm_visanet.ORI_TARJETA = "";
                    }


                    if (dr["TIPO_TARJETA"] != null && dr["TIPO_TARJETA"].ToString() != "")
                    {
                        comm_visanet.TIPO_TARJETA = dr["TIPO_TARJETA"].ToString();
                    }
                    else
                    {
                        comm_visanet.TIPO_TARJETA = "";
                    }

                    if (dr["TIPO_CAPTURA"] != null && dr["TIPO_CAPTURA"].ToString() != "")
                    {
                        comm_visanet.TIPO_CAPTURA = dr["TIPO_CAPTURA"].ToString();
                    }
                    else
                    {
                        comm_visanet.TIPO_CAPTURA = "";
                    }

                    if (dr["MONEDA"] != null && dr["MONEDA"].ToString() != "")
                    {
                        comm_visanet.MONEDA = dr["MONEDA"].ToString();
                    }
                    else
                    {
                        comm_visanet.MONEDA = "";
                    }

                    if (dr["IMPORTE_TRANSAC"] != null && dr["IMPORTE_TRANSAC"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_TRANSAC = Convert.ToDecimal(dr["IMPORTE_TRANSAC"]);
                    }

                    if (dr["IMPORTE_CASHBACK"] != null && dr["IMPORTE_CASHBACK"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_CASHBACK = Convert.ToDecimal(dr["IMPORTE_CASHBACK"]);
                    }

                    if (dr["IMPORTE_COMISION_TOTAL"] != null && dr["IMPORTE_COMISION_TOTAL"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_COMISION_TOTAL = Convert.ToDecimal(dr["IMPORTE_COMISION_TOTAL"]);
                    }
                    
                    if (dr["IMPORTE_COMISION_GRAVABLE"] != null && dr["IMPORTE_COMISION_GRAVABLE"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_COMISION_GRAVABLE = Convert.ToDecimal(dr["IMPORTE_COMISION_GRAVABLE"]);
                    }

                    if (dr["IMPORTE_COMISION_GRAVABLE"] != null && dr["IMPORTE_COMISION_GRAVABLE"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_COMISION_GRAVABLE = Convert.ToDecimal(dr["IMPORTE_COMISION_GRAVABLE"]);
                    }

                    if (dr["NPORCENT_COMISION"] != null && dr["NPORCENT_COMISION"].ToString() != "")
                    {
                        comm_visanet.NPORCENT_COMISION = Convert.ToDecimal(dr["NPORCENT_COMISION"]);
                    }

                    if (dr["IMPORTE_IGV"] != null && dr["IMPORTE_IGV"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_IGV = Convert.ToDecimal(dr["IMPORTE_IGV"]);
                    }

                    if (dr["IMPORTE_ABONAR"] != null && dr["IMPORTE_ABONAR"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_ABONAR = Convert.ToDecimal(dr["IMPORTE_ABONAR"]);
                    }

                    if (dr["ESTADO"] != null && dr["ESTADO"].ToString() != "")
                    {
                        comm_visanet.ESTADO = dr["ESTADO"].ToString();
                    }
                    else
                    {
                        comm_visanet.ESTADO = "";
                    }

                    if (dr["STYP_COMM"] != null && dr["STYP_COMM"].ToString() != "")
                    {
                        comm_visanet.STYP_COMM = dr["STYP_COMM"].ToString();
                    }
                    else
                    {
                        comm_visanet.STYP_COMM = "";
                    }

                    if (dr["FEC_ABONO"] != null && dr["FEC_ABONO"].ToString() != "")
                    {
                        comm_visanet.FEC_ABONO = Convert.ToDateTime(dr["FEC_ABONO"]);
                    }

                    if (dr["AUTORIZACION"] != null && dr["AUTORIZACION"].ToString() != "")
                    {
                        comm_visanet.AUTORIZACION = dr["AUTORIZACION"].ToString();
                    }
                    else
                    {
                        comm_visanet.AUTORIZACION = "";
                    }

                    if (dr["ID_UNICO"] != null && dr["ID_UNICO"].ToString() != "")
                    {
                        comm_visanet.ID_UNICO = dr["ID_UNICO"].ToString();
                    }
                    else
                    {
                        comm_visanet.ID_UNICO = "";
                    }
                    
                    if (dr["NUM_TERMINAL"] != null && dr["NUM_TERMINAL"].ToString() != "")
                    {
                        comm_visanet.NUM_TERMINAL = dr["NUM_TERMINAL"].ToString();
                    }
                    else
                    {
                        comm_visanet.NUM_TERMINAL = "";
                    }

                    if (dr["NLOTE"] != null && dr["NLOTE"].ToString() != "")
                    {
                        comm_visanet.NLOTE = Convert.ToInt32(dr["NLOTE"]);
                    }

                    if (dr["NUM_REFERENCIA"] != null && dr["NUM_REFERENCIA"].ToString() != "")
                    {
                        comm_visanet.NUM_REFERENCIA = dr["NUM_REFERENCIA"].ToString();
                    }
                    else
                    {
                        comm_visanet.NUM_REFERENCIA = "";
                    }

                    if (dr["NUM_CUOTAS"] != null && dr["NUM_CUOTAS"].ToString() != "")
                    {
                        comm_visanet.NUM_CUOTAS = Convert.ToInt32(dr["NUM_CUOTAS"]);
                    }

                    if (dr["CUENTA_BANCARIA"] != null && dr["CUENTA_BANCARIA"].ToString() != "")
                    {
                        comm_visanet.CUENTA_BANCARIA = dr["CUENTA_BANCARIA"].ToString();
                    }
                    else
                    {
                        comm_visanet.CUENTA_BANCARIA = "";
                    }

                    if (dr["NBANK_CODE"] != null && dr["NBANK_CODE"].ToString() != "")
                    {
                        comm_visanet.NBANK_CODE = Convert.ToInt32(dr["NBANK_CODE"]);
                    }

                    if (dr["TRANSAC_CUOTAS"] != null && dr["TRANSAC_CUOTAS"].ToString() != "")
                    {
                        comm_visanet.TRANSAC_CUOTAS = dr["TRANSAC_CUOTAS"].ToString();
                    }
                    else
                    {
                        comm_visanet.TRANSAC_CUOTAS = "";
                    }

                    if (dr["NOMBRE_PROGRAMA"] != null && dr["NOMBRE_PROGRAMA"].ToString() != "")
                    {
                        comm_visanet.NOMBRE_PROGRAMA = dr["NOMBRE_PROGRAMA"].ToString();
                    }
                    else
                    {
                        comm_visanet.NOMBRE_PROGRAMA = "";
                    }

                    if (dr["IMPORTE_DESCONT"] != null && dr["IMPORTE_DESCONT"].ToString() != "")
                    {
                        comm_visanet.IMPORTE_DESCONT = Convert.ToDecimal(dr["IMPORTE_DESCONT"]);
                    }

                    if (dr["NOPERACION_BANCO"] != null && dr["NOPERACION_BANCO"].ToString() != "")
                    {
                        comm_visanet.NOPERACION_BANCO = Convert.ToInt32(dr["NOPERACION_BANCO"]);
                    }

                    if (dr["NRECEIPT"] != null && dr["NRECEIPT"].ToString() != "")
                    {
                        comm_visanet.NRECEIPT = Convert.ToInt64(dr["NRECEIPT"]);
                    }

                    if (dr["DCOMPDATE"] != null && dr["DCOMPDATE"].ToString() != "")
                    {
                        comm_visanet.DCOMPDATE = Convert.ToDateTime(dr["DCOMPDATE"]);
                    }

                    if (dr["NUSERCODE"] != null && dr["NUSERCODE"].ToString() != "")
                    {
                        comm_visanet.NUSERCODE = Convert.ToInt32(dr["NUSERCODE"]);
                    }

                    comm_visanetList.Add(comm_visanet);
                }
            }

            return comm_visanetList;
        }       
        
        public List<DET_COMM_VISANET> ListarComisionDetalleAbonado(long idProforma)
        {
            DET_COMM_VISANET det_comm_visanet = null;
            List<DET_COMM_VISANET> det_comm_visanetList = new List<DET_COMM_VISANET>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, idProforma, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSREADET_COMM_VISANET", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    det_comm_visanet = new DET_COMM_VISANET();

                    if (dr["SCERTYPE"] != null && dr["SCERTYPE"].ToString() != "")
                    {
                        det_comm_visanet.SCERTYPE = dr["SCERTYPE"].ToString();
                    }
                    else
                    {
                        det_comm_visanet.SCERTYPE = "";
                    }

                    if (dr["NBRANCH"] != null && dr["NBRANCH"].ToString() != "")
                    {
                        det_comm_visanet.NBRANCH = Convert.ToInt32(dr["NBRANCH"]);
                    }

                    if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                    {
                        det_comm_visanet.NPRODUCT = Convert.ToInt32(dr["NPRODUCT"]);
                    }

                    if (dr["NPOLICY"] != null && dr["NPOLICY"].ToString() != "")
                    {
                        det_comm_visanet.NPOLICY = Convert.ToInt64(dr["NPOLICY"]);
                    }

                    if (dr["NBRANCH_LED"] != null && dr["NBRANCH_LED"].ToString() != "")
                    {
                        det_comm_visanet.NBRANCH_LED = Convert.ToInt32(dr["NBRANCH_LED"]);
                    }

                    if (dr["NCERTIF"] != null && dr["NCERTIF"].ToString() != "")
                    {
                        det_comm_visanet.NCERTIF = Convert.ToInt32(dr["NCERTIF"]);
                    }

                    if (dr["NRECEIPT"] != null && dr["NRECEIPT"].ToString() != "")
                    {
                        det_comm_visanet.NRECEIPT = Convert.ToInt64(dr["NRECEIPT"]);
                    }

                    if (dr["NINSUR_AREA"] != null && dr["NINSUR_AREA"].ToString() != "")
                    {
                        det_comm_visanet.NINSUR_AREA = Convert.ToInt32(dr["NINSUR_AREA"]);
                    }

                    if (dr["SBILLTYPE"] != null && dr["SBILLTYPE"].ToString() != "")
                    {
                        det_comm_visanet.SBILLTYPE = dr["SBILLTYPE"].ToString();
                    }
                    else
                    {
                        det_comm_visanet.SBILLTYPE = "";
                    }

                    if (dr["NBILLNUM"] != null && dr["NBILLNUM"].ToString() != "")
                    {
                        det_comm_visanet.NBILLNUM = Convert.ToInt32(dr["NBILLNUM"]);
                    }

                    if (dr["NCURRENCY"] != null && dr["NCURRENCY"].ToString() != "")
                    {
                        det_comm_visanet.NCURRENCY = Convert.ToInt32(dr["NCURRENCY"]);
                    }

                    if (dr["NPREMIUMN"] != null && dr["NPREMIUMN"].ToString() != "")
                    {
                        det_comm_visanet.NPREMIUMN = Convert.ToInt64(dr["NPREMIUMN"]);
                    }

                    if (dr["NINTERTYP"] != null && dr["NINTERTYP"].ToString() != "")
                    {
                        det_comm_visanet.NINTERTYP = Convert.ToInt32(dr["NINTERTYP"]);
                    }

                    if (dr["NINTERMED"] != null && dr["NINTERMED"].ToString() != "")
                    {
                        det_comm_visanet.NINTERMED = Convert.ToInt32(dr["NINTERMED"]);
                    }

                    if (dr["NPERCENT"] != null && dr["NPERCENT"].ToString() != "")
                    {
                        det_comm_visanet.NPERCENT = Convert.ToDecimal(dr["NPERCENT"]);
                    }

                    if (dr["NCOMMISSION"] != null && dr["NCOMMISSION"].ToString() != "")
                    {
                        det_comm_visanet.NCOMMISSION = Convert.ToDecimal(dr["NCOMMISSION"]);
                    }

                    if (dr["DEFFECDATE"] != null && dr["DEFFECDATE"].ToString() != "")
                    {
                        det_comm_visanet.DEFFECDATE = Convert.ToDateTime(dr["DEFFECDATE"]);
                    }

                    if (dr["NBANK_CODE"] != null && dr["NBANK_CODE"].ToString() != "")
                    {
                        det_comm_visanet.NBANK_CODE = Convert.ToInt32(dr["NBANK_CODE"]);
                    }

                    if (dr["NOPERACION_BANCO"] != null && dr["NOPERACION_BANCO"].ToString() != "")
                    {
                        det_comm_visanet.NOPERACION_BANCO = Convert.ToInt32(dr["NOPERACION_BANCO"]);
                    }

                    if (dr["NTYPE"] != null && dr["NTYPE"].ToString() != "")
                    {
                        det_comm_visanet.NTYPE = Convert.ToInt32(dr["NTYPE"]);
                    }

                    if (dr["STYP_COMM"] != null && dr["STYP_COMM"].ToString() != "")
                    {
                        det_comm_visanet.STYP_COMM = dr["STYP_COMM"].ToString();
                    }
                    else
                    {
                        det_comm_visanet.STYP_COMM = "";
                    }

                    if (dr["ID_UNICO"] != null && dr["ID_UNICO"].ToString() != "")
                    {
                        det_comm_visanet.ID_UNICO = Convert.ToInt64(dr["ID_UNICO"]);
                    }

                    if (dr["DCOMPDATE"] != null && dr["DCOMPDATE"].ToString() != "")
                    {
                        det_comm_visanet.DCOMPDATE = Convert.ToDateTime(dr["DCOMPDATE"]);
                    }

                    if (dr["NUSERCODE"] != null && dr["NUSERCODE"].ToString() != "")
                    {
                        det_comm_visanet.NUSERCODE = Convert.ToInt32(dr["NUSERCODE"]);
                    }

                    if (dr["SKEY"] != null && dr["SKEY"].ToString() != "")
                    {
                        det_comm_visanet.SKEY = dr["SKEY"].ToString();
                    }
                    else
                    {
                        det_comm_visanet.SKEY = "";
                    }

                    det_comm_visanetList.Add(det_comm_visanet);
                }
            }

            return det_comm_visanetList;
        }

        public string RegistrarComisionCabeceraAbonado(COMM_VISANET comm_visanet)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("SCERTYPE", OracleDbType.NVarchar2, comm_visanet.SCERTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, comm_visanet.NBRANCH, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, comm_visanet.NPRODUCT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPOLICY", OracleDbType.Int64, comm_visanet.NPOLICY, ParameterDirection.Input)); //
            parameters.Add(new OracleParameter("NCOMERCIO", OracleDbType.Int64, comm_visanet.NCOMERCIO, ParameterDirection.Input)); //
            parameters.Add(new OracleParameter("NOMBRE_COMERCIAL", OracleDbType.NVarchar2, comm_visanet.NOMBRE_COMERCIAL, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DEFFECDATE", OracleDbType.Date, comm_visanet.DEFFECDATE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("FEC_TRANSACCION", OracleDbType.Date, comm_visanet.FEC_TRANSACCION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("FEC_PROCESO", OracleDbType.Date, comm_visanet.FEC_PROCESO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("TIPO_OPERACION", OracleDbType.NVarchar2, comm_visanet.TIPO_OPERACION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DESC_COD_CONTABLE", OracleDbType.NVarchar2, comm_visanet.DESC_COD_CONTABLE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUM_TARJETA", OracleDbType.NVarchar2, comm_visanet.NUM_TARJETA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ORI_TARJETA", OracleDbType.NVarchar2, comm_visanet.ORI_TARJETA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("TIPO_TARJETA", OracleDbType.NVarchar2, comm_visanet.TIPO_TARJETA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("TIPO_CAPTURA", OracleDbType.NVarchar2, comm_visanet.TIPO_CAPTURA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("MONEDA", OracleDbType.NVarchar2, comm_visanet.MONEDA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_TRANSAC", OracleDbType.Decimal, comm_visanet.IMPORTE_TRANSAC, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_CASHBACK", OracleDbType.Decimal, comm_visanet.IMPORTE_CASHBACK, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_COMISION_TOTAL", OracleDbType.Decimal, comm_visanet.IMPORTE_COMISION_TOTAL, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_COMISION_GRAVABLE", OracleDbType.Decimal, comm_visanet.IMPORTE_COMISION_GRAVABLE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPORCENT_COMISION", OracleDbType.Decimal, comm_visanet.NPORCENT_COMISION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_IGV", OracleDbType.Decimal, comm_visanet.IMPORTE_IGV, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_ABONAR", OracleDbType.Decimal, comm_visanet.IMPORTE_ABONAR, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ESTADO", OracleDbType.NVarchar2, comm_visanet.ESTADO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("STYP_COMM", OracleDbType.NVarchar2, comm_visanet.STYP_COMM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("FEC_ABONO", OracleDbType.Date, comm_visanet.FEC_ABONO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("AUTORIZACION", OracleDbType.NVarchar2, comm_visanet.AUTORIZACION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_UNICO", OracleDbType.NVarchar2, comm_visanet.ID_UNICO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUM_TERMINAL", OracleDbType.NVarchar2, comm_visanet.NUM_TERMINAL, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NLOTE", OracleDbType.Int64, comm_visanet.NLOTE, ParameterDirection.Input));//
            parameters.Add(new OracleParameter("NUM_REFERENCIA", OracleDbType.NVarchar2, comm_visanet.NUM_REFERENCIA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUM_CUOTAS", OracleDbType.Int32, comm_visanet.NUM_CUOTAS, ParameterDirection.Input));
            parameters.Add(new OracleParameter("CUENTA_BANCARIA", OracleDbType.NVarchar2, comm_visanet.CUENTA_BANCARIA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBANK_CODE", OracleDbType.Int64, comm_visanet.NBANK_CODE, ParameterDirection.Input));//
            parameters.Add(new OracleParameter("TRANSAC_CUOTAS", OracleDbType.NVarchar2, comm_visanet.TRANSAC_CUOTAS, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NOMBRE_PROGRAMA", OracleDbType.NVarchar2, comm_visanet.NOMBRE_PROGRAMA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("IMPORTE_DESCONT", OracleDbType.Decimal, comm_visanet.IMPORTE_DESCONT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NOPERACION_BANCO", OracleDbType.Int64, comm_visanet.NOPERACION_BANCO, ParameterDirection.Input));//
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, comm_visanet.NRECEIPT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DCOMPDATE", OracleDbType.Date, comm_visanet.DCOMPDATE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, comm_visanet.NUSERCODE, ParameterDirection.Input));  

            //Parámetro de Salida
            var pResult = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSPOSTCOMM_VISANET", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return _sMessage;
        }
        
        public string RegistrarComisionDetalleAbonado(DET_COMM_VISANET det_comm_visanet)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("SCERTYPE", OracleDbType.NVarchar2, det_comm_visanet.SCERTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBRANCH", OracleDbType.Int32, det_comm_visanet.NBRANCH, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, det_comm_visanet.NPRODUCT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPOLICY", OracleDbType.Int32, det_comm_visanet.NPOLICY, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBRANCH_LED", OracleDbType.Int32, det_comm_visanet.NBRANCH_LED, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCERTIF", OracleDbType.Int32, det_comm_visanet.NCERTIF, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NRECEIPT", OracleDbType.Int64, det_comm_visanet.NRECEIPT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINSUR_AREA", OracleDbType.Int32, det_comm_visanet.NINSUR_AREA, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SBILLTYPE", OracleDbType.NVarchar2, det_comm_visanet.SBILLTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NBILLNUM", OracleDbType.Int32, det_comm_visanet.NBILLNUM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCURRENCY", OracleDbType.Int32, det_comm_visanet.NCURRENCY, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPREMIUMN", OracleDbType.Int64, det_comm_visanet.NPREMIUMN, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINTERTYP", OracleDbType.Int32, det_comm_visanet.NINTERTYP, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NINTERMED", OracleDbType.Int32, det_comm_visanet.NINTERMED, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NPERCENT", OracleDbType.Decimal, det_comm_visanet.NPERCENT, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NCOMMISSION", OracleDbType.Decimal, det_comm_visanet.NCOMMISSION, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DEFFECDATE", OracleDbType.Date, det_comm_visanet.DEFFECDATE, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("NBANK_CODE", OracleDbType.Int32, det_comm_visanet.NBANK_CODE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NOPERACION_BANCO", OracleDbType.Int32, det_comm_visanet.NOPERACION_BANCO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NTYPE", OracleDbType.Int32, det_comm_visanet.NTYPE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("STYP_COMM", OracleDbType.NVarchar2, det_comm_visanet.STYP_COMM, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_UNICO", OracleDbType.Int64, det_comm_visanet.ID_UNICO, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DCOMPDATE", OracleDbType.Date, det_comm_visanet.DCOMPDATE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("NUSERCODE", OracleDbType.Int32, det_comm_visanet.NUSERCODE, ParameterDirection.Input));
            parameters.Add(new OracleParameter("SKEY", OracleDbType.NVarchar2, det_comm_visanet.SKEY, ParameterDirection.Input));

            //Parámetro de Salida
            var pResult = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCLIQABONOS.INSPOSTDET_COMM_VISANET", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return _sMessage;
        }

        #endregion

    }
}
