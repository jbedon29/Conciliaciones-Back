using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Transactions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Protecta.Infrastructure.Connection;
using Protecta.CrossCuting.Utilities.Configuration;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Domain.Service.ConsultaModule.Aggregates.ConsultaAgg;
using Protecta.Domain.Service.ProcesoModule.Aggregates.ProcesoAgg;
using Protecta.Infrastructure.Data.ProcesoModule.Repositories;

namespace Protecta.Infrastructure.Data
{
    public class PlanillaRepository : IPlanillaRepository
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IConnectionBase _connectionBase;
        private readonly ProcesoRepository _procesoRepository;

        public PlanillaRepository(IOptions<AppSettings> appSettings, IConnectionBase ConnectionBase)
        {
            this.appSettings = appSettings;
            _connectionBase = ConnectionBase;
            
            _procesoRepository = new ProcesoRepository(appSettings, ConnectionBase);
        }

        public Task<List<Planilla>> ListarPlanilla(DatosConsultaPlanilla datosConsultaPlanilla)
        {
            int _indFecPlanilla = 0;

            string _fechaDesde = string.Empty;
            string _fechaHasta = string.Empty;
            string _sMessage = string.Empty;

            //Agregado 16/07/2018
            string _usuario = string.Empty;
            int _idProducto = 0;

            Planilla planillaEntity = null;
            List<Planilla> planillaList = new List<Planilla>();

            if (datosConsultaPlanilla.FechaDesde != "" || datosConsultaPlanilla.FechaHasta != "")
            {
                _indFecPlanilla = 1;
                _idProducto = datosConsultaPlanilla.IdProducto;
                _fechaDesde = Convert.ToDateTime(datosConsultaPlanilla.FechaDesde).ToShortDateString();
                _fechaHasta = Convert.ToDateTime(datosConsultaPlanilla.FechaHasta).ToShortDateString();
                _usuario = datosConsultaPlanilla.Usuario.ToString();
            }
            else
            {
                _indFecPlanilla = 0;
                _idProducto = datosConsultaPlanilla.IdProducto;
                _fechaDesde = string.Empty;
                _fechaHasta = string.Empty;
                _usuario = datosConsultaPlanilla.Usuario.ToString();
            }

            List<OracleParameter> parameters = new List<OracleParameter>();

            planillaList = new List<Planilla>();

            parameters.Add(new OracleParameter("NPRODUCT", OracleDbType.Int32, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("INDFECPLA", OracleDbType.Int32, _indFecPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECDESDE", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("DFECHASTA", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input)); 
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCIMPORTACION.INSREAPV_PAYROLL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
            {
                while (dr.Read())
                {
                    planillaEntity = new Planilla();

                    if (dr["NIDPAYROLL"] != null && dr["NIDPAYROLL"].ToString() != "")
                    {
                        planillaEntity.IdPlanilla = Convert.ToInt32(dr["NIDPAYROLL"]);
                    }

                    if (dr["DREGPAYROLL"] != null && dr["DREGPAYROLL"].ToString() != "")
                    {
                        planillaEntity.DtFechaPlanilla = Convert.ToDateTime(dr["DREGPAYROLL"]);
                    }

                    if (dr["NAMOUNTTOTAL"] != null && dr["NAMOUNTTOTAL"].ToString() != "")
                    {
                        planillaEntity.DcTotal = dr["NAMOUNTTOTAL"].ToString();
                    }                  

                    if (dr["DUPDATE"] != null && dr["DUPDATE"].ToString() != "")
                    {
                        planillaEntity.DtFechamodificacion = Convert.ToDateTime(dr["DUPDATE"]);
                    }

                    if (dr["NUSERUPDATE"] != null && dr["NUSERUPDATE"].ToString() != "")
                    {
                        planillaEntity.VcUsuariomodificacion = dr["NUSERUPDATE"].ToString();
                    }

                    if (dr["SCODCHANNEL"] != null && dr["SCODCHANNEL"].ToString() != "")
                    {
                        planillaEntity.IdCanal = Convert.ToInt32(dr["SCODCHANNEL"]);
                    }

                    if (dr["NCODSALEPOINT"] != null && dr["NCODSALEPOINT"].ToString() != "")
                    {
                        planillaEntity.IdPuntoventa = dr["NCODSALEPOINT"].ToString();
                    }

                    if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                    {
                        planillaEntity.IdProducto = Convert.ToInt32(dr["NPRODUCT"]);
                    }

                    planillaEntity.IddgEstado = "1001"; //Estado de Registro - Activo
                    planillaEntity.IddgEstadoPlanilla = "1101"; //Estado de Planilla - Ingresado

                    planillaEntity.VcUsuariocreacion = _usuario.ToString(); //Agregado 16/07/2018

                    planillaList.Add(planillaEntity);
                }
            }

            ObtenerDetallePlanillaCobro(ref planillaList);

            ObtenerDetallePlanillaPago(ref planillaList);

            return Task.FromResult<List<Planilla>>(planillaList);
        }

        public void ObtenerDetallePlanillaCobro(ref List<Planilla> planillaList)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = null;
            DetallePlanillaCobro detallePlanillaCobroEntity = null;
            List<DetallePlanillaCobro> detallePlanillaCobroList = null;            

            foreach (Planilla p in planillaList)
            {
                parameters = new List<OracleParameter>();
                detallePlanillaCobroList = new List<DetallePlanillaCobro>();

                parameters.Add(new OracleParameter("NIDPAYROLL", OracleDbType.Int32, p.IdPlanilla, ParameterDirection.Input));
                parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCIMPORTACION.INSREAPV_PAYROLL_DETAIL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        detallePlanillaCobroEntity = new DetallePlanillaCobro();

                        if (dr["NIDPAYROLLDETAIL"] != null && dr["NIDPAYROLLDETAIL"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.IdDetallePlanillaCobro = Convert.ToInt32(dr["NIDPAYROLLDETAIL"]);
                        }

                        if (dr["NIDPAYROLL"] != null && dr["NIDPAYROLL"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.IdPlanilla = Convert.ToInt32(dr["NIDPAYROLL"]);
                        }

                        if (dr["NBRANCH"] != null && dr["NBRANCH"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.IdRamo = Convert.ToInt32(dr["NBRANCH"]);
                        }

                        if (dr["NPRODUCT"] != null && dr["NPRODUCT"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.IdProducto = Convert.ToInt32(dr["NPRODUCT"]);
                        }

                        if (dr["NPOLICY"] != null && dr["NPOLICY"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.VcNumeropoliza = Convert.ToInt64(dr["NPOLICY"]);
                        }

                        if (dr["NCERTIF"] != null && dr["NCERTIF"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.VcNumerocertificado = Convert.ToInt64(dr["NCERTIF"]);
                        }

                        if (dr["NRECEIPT"] != null && dr["NRECEIPT"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.IdProforma = Convert.ToInt64(dr["NRECEIPT"]);
                        }

                        if (dr["NPREMIUM"] != null && dr["NPREMIUM"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.DcMonto = Convert.ToDecimal(dr["NPREMIUM"]);
                        }                     

                        if (dr["DUPDATE"] != null && dr["DUPDATE"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.DtFechamodificacion = Convert.ToDateTime(dr["DUPDATE"]);
                        }

                        if (dr["NUSERUPDATE"] != null && dr["NUSERUPDATE"].ToString() != "")
                        {
                            detallePlanillaCobroEntity.VcUsuariomodificacion = dr["NUSERUPDATE"].ToString();
                        }

                        detallePlanillaCobroEntity.IddgEstado = "1001"; //Estado de Registro - Activo
                        detallePlanillaCobroEntity.IddgEstadoDetPlanilla = "1101"; //Estado de Planilla - Ingresado

                        detallePlanillaCobroEntity.VcUsuariocreacion = p.VcUsuariocreacion.ToString(); //Agregado 16/07/2018

                        detallePlanillaCobroList.Add(detallePlanillaCobroEntity);
                    }

                    p.DetallePlanillacobro = detallePlanillaCobroList;
                }
            }
        }    

        public void ObtenerDetallePlanillaPago(ref List<Planilla> planillaList)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = null;
            DetallePlanillaPago detallePlanillaPagoEntity = null;
            List<DetallePlanillaPago> detallePlanillaPagoList = null;

            foreach (Planilla p in planillaList)
            {
                parameters = new List<OracleParameter>();
                detallePlanillaPagoList = new List<DetallePlanillaPago>();

                parameters.Add(new OracleParameter("NIDPAYROLL", OracleDbType.Int32, p.IdPlanilla, ParameterDirection.Input));
                parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

                using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCIMPORTACION.INSREAPV_PAYROLL_PAYMENT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime))
                {
                    while (dr.Read())
                    {
                        detallePlanillaPagoEntity = new DetallePlanillaPago();

                        if (dr["NIDPAYROLLDETAIL"] != null && dr["NIDPAYROLLDETAIL"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdDetallePlanillaPago = Convert.ToInt32(dr["NIDPAYROLLDETAIL"]);
                        }

                        if (dr["NIDPAYROLL"] != null && dr["NIDPAYROLL"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdPlanilla = Convert.ToInt32(dr["NIDPAYROLL"]);
                        }

                        if (dr["NCURRENCY"] != null && dr["NCURRENCY"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdMoneda = Convert.ToInt32(dr["NCURRENCY"]);
                        }

                        if (dr["NAMOUNT"] != null && dr["NAMOUNT"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.DcMonto = dr["NAMOUNT"].ToString();
                        }

                        if (dr["NIDPAIDTYPE"] != null && dr["NIDPAIDTYPE"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdTipomediopago = Convert.ToInt32(dr["NIDPAIDTYPE"]);
                        }

                        if (dr["NBANK"] != null && dr["NBANK"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdBanco = Convert.ToInt32(dr["NBANK"]);
                        }

                        if (dr["NBANK_ACCOUNT"] != null && dr["NBANK_ACCOUNT"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.IdCuentaBanco = Convert.ToInt32(dr["NBANK_ACCOUNT"]);
                        }

                        if (dr["SOPERATION_NUMBER"] != null && dr["SOPERATION_NUMBER"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.VcNumerooperacion = dr["SOPERATION_NUMBER"].ToString();
                        }                       

                        if (dr["DUPDATE"] != null && dr["DUPDATE"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.DtFechamodificacion = Convert.ToDateTime(dr["DUPDATE"]);
                        }

                        if (dr["NUSERUPDATE"] != null && dr["NUSERUPDATE"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.VcUsuariomodificacion = dr["NUSERUPDATE"].ToString();
                        }

                        //Agregado 17/07/2018
                        if (dr["DOPERATION_DATE"] != null && dr["DOPERATION_DATE"].ToString() != "")
                        {
                            detallePlanillaPagoEntity.DtFechaoperacion = Convert.ToDateTime(dr["DOPERATION_DATE"]);
                        }

                        detallePlanillaPagoEntity.IddgEstado = "1001"; //Estado de Registro - Activo

                        detallePlanillaPagoEntity.IddgEstadoDetPlanilla = "1101"; //Estado de Planilla - Ingresado

                        detallePlanillaPagoEntity.VcUsuariocreacion = p.VcUsuariocreacion.ToString(); //Agregado 16/07/2018

                        detallePlanillaPagoList.Add(detallePlanillaPagoEntity);
                    }

                    p.DetallePlanillapago = detallePlanillaPagoList;
                }
            }
        }

        public Task<string> RegistrarPlanilla(List<Planilla> planillaList)
        {
            int _nIdProceso = 0;
            string _sMessage = string.Empty;

            List<OracleParameter> parameters1 = null;
            ProcesoGeneral _procesoGeneral = null;
            LogProcesoGeneral _procesoLogGeneral = null;
            PlanillaEstado _planillaEstado = new PlanillaEstado();

            _procesoGeneral = new ProcesoGeneral();
            _procesoGeneral.IdProceso = 3; //Importacion
            _procesoGeneral.VcUsuariocreacion = planillaList[0].VcUsuariocreacion.ToString(); //Agregado 16/07/2018
            _procesoGeneral.VcDescripcion = "";
            _procesoGeneral.VcMensaje = "Inicio Registro de Importación de Planillas";
            _procesoGeneral.VcAmbito = "BD";
            _nIdProceso = _procesoRepository.RegistrarLog(_procesoGeneral);

            foreach (Planilla p in planillaList)
            {
                try
                {
                    parameters1 = new List<OracleParameter>();
                    parameters1.Add(new OracleParameter("ID_PLANILLA", OracleDbType.Int32, p.IdPlanilla, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("VC_DESCRIPCION", OracleDbType.Varchar2, 255, p.VcDescripcion, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("DC_TOTAL", OracleDbType.NVarchar2, 10, p.DcTotal, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("DT_FECHA_PLANILLA", OracleDbType.Date, p.DtFechaPlanilla, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_CANAL", OracleDbType.Int32, p.IdCanal, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_PRODUCTO", OracleDbType.Int32, p.IdProducto, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_PUNTO_VENTA", OracleDbType.NVarchar2, p.IdPuntoventa, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_DG_ESTADO_PLANILLA", OracleDbType.NVarchar2, p.IddgEstadoPlanilla, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_DG_ESTADO_PROENV", OracleDbType.NVarchar2, p.IddgEstadoProenv, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.NVarchar2, p.IddgEstado, ParameterDirection.Input));                    
                    parameters1.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, 20, p.VcUsuariocreacion, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("DT_FECHA_MODIFICACION", OracleDbType.Date, p.DtFechamodificacion, ParameterDirection.Input));
                    parameters1.Add(new OracleParameter("VC_USUARIO_MODIFICACION", OracleDbType.NVarchar2, 20, p.VcUsuariomodificacion, ParameterDirection.Input));

                    //Parámetro de Salida
                    var pResult1 = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                    parameters1.Add(pResult1);

                    _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCIMPORTACION.CCLPOSTPLANILLA", parameters1, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                    _sMessage = pResult1.Value.ToString() == null ? String.Empty : pResult1.Value.ToString();

                    foreach (DetallePlanillaCobro d in p.DetallePlanillacobro)
                    {
                        List<OracleParameter> parameters2 = new List<OracleParameter>();

                        parameters2.Add(new OracleParameter("ID_DETALLE_PLANILLA_COBRO", OracleDbType.Int32, d.IdDetallePlanillaCobro, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_PLANILLA", OracleDbType.Int32, d.IdPlanilla, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_RAMO", OracleDbType.Int32, d.IdRamo, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_PRODUCTO", OracleDbType.Int32, d.IdProducto, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("VC_NUMERO_POLIZA", OracleDbType.NVarchar2, 20, d.VcNumeropoliza, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("VC_NUMERO_CERTIFICADO", OracleDbType.NVarchar2, 20, d.VcNumerocertificado, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_PROFORMA", OracleDbType.Long, d.IdProforma, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("DC_MONTO", OracleDbType.Decimal, d.DcMonto, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, d.IddgEstado, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("ID_DG_ESTADO_DET_PLANILLA", OracleDbType.Int32, d.IddgEstadoDetPlanilla, ParameterDirection.Input));                        
                        parameters2.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, 20, d.VcUsuariocreacion, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("DT_FECHA_MODIFICACION", OracleDbType.Date, d.DtFechamodificacion, ParameterDirection.Input));
                        parameters2.Add(new OracleParameter("VC_USUARIO_MODIFICACION", OracleDbType.NVarchar2, 20, d.VcUsuariomodificacion, ParameterDirection.Input));
                        //Parámetro de Salida
                        var pResult2 = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                        parameters2.Add(pResult2);

                        _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCIMPORTACION.CCLPOSTDETPLANILLACOBRO", parameters2, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                        _sMessage = pResult2.Value.ToString() == null ? String.Empty : pResult2.Value.ToString();
                    }

                    foreach (DetallePlanillaPago e in p.DetallePlanillapago)
                    {
                        List<OracleParameter> parameters3 = new List<OracleParameter>();
                       
                        parameters3.Add(new OracleParameter("ID_DETALLE_PLANILLA_PAGO", OracleDbType.Int32, e.IdDetallePlanillaPago, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_PLANILLA", OracleDbType.Int32, e.IdPlanilla, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_MONEDA", OracleDbType.Int32, e.IdMoneda, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("DC_MONTO", OracleDbType.Decimal, e.DcMonto, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_TIPO_MEDIO_PAGO", OracleDbType.Int32, e.IdTipomediopago, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_BANCO", OracleDbType.Int32, e.IdBanco, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_CUENTA_BANCO", OracleDbType.NVarchar2, 20, Convert.ToString(e.IdCuentaBanco), ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("VC_NUMERO_OPERACION", OracleDbType.NVarchar2, 20, e.VcNumerooperacion, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, Convert.ToInt32(e.IddgEstado), ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("ID_DG_ESTADO_DET_PLANILLA", OracleDbType.Int32, Convert.ToInt32(e.IddgEstadoDetPlanilla), ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("DT_FECHA_OPERACION", OracleDbType.Date, e.DtFechaoperacion, ParameterDirection.Input)); //Agregado 17/07/2018
                        parameters3.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, 20, e.VcUsuariocreacion, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("DT_FECHA_MODIFICACION", OracleDbType.Date, e.DtFechamodificacion, ParameterDirection.Input));
                        parameters3.Add(new OracleParameter("VC_USUARIO_MODIFICACION", OracleDbType.NVarchar2, 20, e.VcUsuariomodificacion, ParameterDirection.Input));                          
                        //Parámetro de Salida
                        var pResult3 = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                        parameters3.Add(pResult3);

                        _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCIMPORTACION.CCLPOSTDETPLANILLAPAGO", parameters3, ConnectionBase.enuTypeDataBase.OracleConciliacion);
                        _sMessage = pResult3.Value.ToString() == null ? String.Empty : pResult3.Value.ToString();
                    }

                }
                catch (Exception ex)
                {                    
                    _sMessage = string.Format("Error: {0}", ex.ToString());

                    _procesoLogGeneral = new LogProcesoGeneral();
                    _procesoLogGeneral.IdProcesoGeneral = _nIdProceso;
                    _procesoLogGeneral.VcMensaje = _sMessage.ToString();
                    _procesoLogGeneral.VcAmbito = _procesoGeneral.VcAmbito;
                    _procesoLogGeneral.VcUsuarioCreacion = _procesoGeneral.VcUsuariocreacion;
                    _procesoRepository.RegistrarLog(_procesoLogGeneral);
                }

                //Registra estados
                _planillaEstado = new PlanillaEstado();
                _planillaEstado.IdPlanilla = p.IdPlanilla;
                _planillaEstado.IddgEstadoplanilla = 1101; //Ingresado
                _planillaEstado.IddgEstado = 1001; //Registro activo
                _planillaEstado.VcUsuariocreacion = _procesoGeneral.VcUsuariocreacion; //Agregado 16/07/2018
                RegistrarEstadoPlanilla(_planillaEstado);
            }

            _procesoLogGeneral = new LogProcesoGeneral();
            _procesoLogGeneral.IdProcesoGeneral = _nIdProceso;
            _procesoLogGeneral.VcMensaje = "Fin Registro de Importación de Planillas";
            _procesoLogGeneral.VcAmbito = _procesoGeneral.VcAmbito;
            _procesoLogGeneral.VcUsuarioCreacion = _procesoGeneral.VcUsuariocreacion;
            _procesoRepository.RegistrarLog(_procesoLogGeneral);

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> ActualizarEstadoImportacion(List<Planilla> planillaList)
        {
            string _sMessage = string.Empty;

            foreach (Planilla p in planillaList)
            {
                try
                {
                    List<OracleParameter> parameters = new List<OracleParameter>();

                    parameters.Add(new OracleParameter("NIDPAYROLL", OracleDbType.Int32, p.IdPlanilla, ParameterDirection.Input));                   
                    //Parámetro de Salida
                    var pResult = new OracleParameter("P_RESULT", OracleDbType.NVarchar2, 32767, OracleDbType.NVarchar2, ParameterDirection.Output);
                    parameters.Add(pResult);

                    _connectionBase.ExecuteByStoredProcedure("INSUDB.INSPROCIMPORTACION.INSUPDPV_PAYROLL_CONCILIAR", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
                    _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();
                }
                catch (Exception ex)
                {
                    _sMessage = string.Format("Error: {0}", ex.ToString());
                }
            }          

            return Task.FromResult<string>(_sMessage);
        }
        
        public Task<List<DetallePlanillaCobro>> ListarPlanillasConciliadas(DatosNotificacion datosNotificacion)
        {
            DetallePlanillaCobro detallePlanillaCobroEntity = null;
            List<DetallePlanillaCobro> detallePlanillaCobroList = new List<DetallePlanillaCobro>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("INDPLANILLA", OracleDbType.Int32, datosNotificacion.IndPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_PLANILLA", OracleDbType.NVarchar2, datosNotificacion.Planilla, ParameterDirection.Input));            
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREAPLANILLALIQUIDADA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    detallePlanillaCobroEntity = new DetallePlanillaCobro();

                    if (dr["ID_PLANILLA"] != null && dr["ID_PLANILLA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"]);
                    }

                    if (dr["ID_RAMO"] != null && dr["ID_RAMO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdRamo = Convert.ToInt32(dr["ID_RAMO"]);
                    }

                    if (dr["ID_PRODUCTO"] != null && dr["ID_PRODUCTO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdProducto = Convert.ToInt32(dr["ID_PRODUCTO"]);
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



                    detallePlanillaCobroEntity.VcUsuariocreacion = datosNotificacion.Usuario.ToString(); //Agregado 16/07/2018

                    detallePlanillaCobroList.Add(detallePlanillaCobroEntity);
                }
            }

            return Task.FromResult<List<DetallePlanillaCobro>>(detallePlanillaCobroList);
        }

        public Task<List<DetallePlanillaCobro>> ListarPlanillasNoConciliadas(DatosNotificacion datosNotificacion)
        {
            DetallePlanillaCobro detallePlanillaCobroEntity = null;
            List<DetallePlanillaCobro> detallePlanillaCobroList = new List<DetallePlanillaCobro>();

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("INDPLANILLA", OracleDbType.Int32, datosNotificacion.IndPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_PLANILLA", OracleDbType.NVarchar2, datosNotificacion.Planilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPROCLIQUIDACION.CCLREAPLANILLANOLIQUIDADA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    detallePlanillaCobroEntity = new DetallePlanillaCobro();

                    if (dr["ID_PLANILLA"] != null && dr["ID_PLANILLA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"]);
                    }

                    if (dr["ID_RAMO"] != null && dr["ID_RAMO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdRamo = Convert.ToInt32(dr["ID_RAMO"]);
                    }

                    if (dr["ID_PRODUCTO"] != null && dr["ID_PRODUCTO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.IdProducto = Convert.ToInt32(dr["ID_PRODUCTO"]);
                    }

                    if (dr["VC_NUMERO_POLIZA"] != null && dr["VC_NUMERO_POLIZA"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.VcNumeropoliza = Convert.ToInt64(dr["VC_NUMERO_POLIZA"]);
                    }

                    if (dr["VC_NUMERO_CERTIFICADO"] != null && dr["VC_NUMERO_CERTIFICADO"].ToString() != "")
                    {
                        detallePlanillaCobroEntity.VcNumerocertificado = Convert.ToInt64(dr["VC_NUMERO_CERTIFICADO"]);
                    }

                    detallePlanillaCobroEntity.VcUsuariocreacion = datosNotificacion.Usuario.ToString(); //Agregado 16/07/2018

                    detallePlanillaCobroList.Add(detallePlanillaCobroEntity);
                }
            }

            return Task.FromResult<List<DetallePlanillaCobro>>(detallePlanillaCobroList);
        }

        public Task<string> ValidarExisteContratante(DetallePlanillaCobro planillaCertificado)
        {
            string _sExists = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Int32, planillaCertificado.IdRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.NVarchar2, planillaCertificado.IdProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.NVarchar2, planillaCertificado.VcNumeropoliza, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCERTIF", OracleDbType.NVarchar2, planillaCertificado.VcNumerocertificado, ParameterDirection.Input));
            //Parámetro de Salida
            var pResult = new OracleParameter("P_EXISTS", OracleDbType.Int32, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.CCL_VALCLIENT", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sExists = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return Task.FromResult<string>(_sExists);
        }

        public Task<string> ValidaFechaEnvioComprobante()
        {
            string _sEnvia = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            //Parámetro de Salida
            var pResult = new OracleParameter("P_ENVIA", OracleDbType.Int32, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.CCL_VALFECCOMP", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sEnvia = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return Task.FromResult<string>(_sEnvia);
        }

        public Task<string> RegistrarComprobantePendiente(DetallePlanillaCobro planillaCertificado)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NIDPAYROLL", OracleDbType.Int32, planillaCertificado.IdPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NBRANCH", OracleDbType.Int32, planillaCertificado.IdRamo, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPRODUCT", OracleDbType.Int32, 10, planillaCertificado.IdProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NPOLICY", OracleDbType.Int32, 255, planillaCertificado.VcNumeropoliza, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NCERTIF", OracleDbType.Int32, planillaCertificado.VcNumerocertificado, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_IND_BILLS", OracleDbType.Int32, planillaCertificado.IndicaComprobante, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_IND_COMMISSION", OracleDbType.Int32, planillaCertificado.IndicaComision, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NSTATE", OracleDbType.Int32, planillaCertificado.IddgEstadoDetPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_NUSERREGISTER", OracleDbType.NVarchar2, planillaCertificado.VcUsuariocreacion, ParameterDirection.Input)); //Modificado 16/07/2018
            parameters.Add(new OracleParameter("P_NRECEIPT", OracleDbType.Int64, planillaCertificado.IdProforma, ParameterDirection.Input)); //Proyecto Kuntur
            
            //Parámetro de Salida
            var pResult = new OracleParameter("P_MESSAGE", OracleDbType.Varchar2, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.CCL_INSGENERATION_BILLS", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return Task.FromResult<string>(_sMessage);
        }

        public Task<string> RegistrarEstadoPlanilla(PlanillaEstado planillaEstado)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("ID_PLANILLA", OracleDbType.Int32, planillaEstado.IdPlanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_DG_ESTADO_PLANILLA", OracleDbType.Int32, planillaEstado.IddgEstadoplanilla, ParameterDirection.Input));
            parameters.Add(new OracleParameter("ID_DG_ESTADO", OracleDbType.Int32, planillaEstado.IddgEstado, ParameterDirection.Input));
            parameters.Add(new OracleParameter("VC_USUARIO_CREACION", OracleDbType.NVarchar2, planillaEstado.VcUsuariocreacion, ParameterDirection.Input));
            //Parámetro de Salida
            var pResult = new OracleParameter("P_RESULT", OracleDbType.Varchar2, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("CONCILIACION.CCLPOSTPLANILLAESTADO", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return Task.FromResult<string>(_sMessage);
        }

        public Task<List<PlanillaConsultaProcesada>> ConsultarPlanillasProcesadas(DatosConsultaPlanilla datosConsultaPlanilla)
        {
            PlanillaConsultaProcesada entities = null;
            List<PlanillaConsultaProcesada> listaPlanillas = new List<PlanillaConsultaProcesada>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsultaPlanilla.IdProducto;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaHasta);            
                        
            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CANAL", OracleDbType.Long, datosConsultaPlanilla.IdCanal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_ENTIDAD", OracleDbType.Long, datosConsultaPlanilla.IdBanco, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CUENTA", OracleDbType.Long, datosConsultaPlanilla.IdCuenta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_VDP_TIPO", OracleDbType.Long, datosConsultaPlanilla.IdTipoCobro, ParameterDirection.Input));


            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PLANILLA_CONCILIADA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new PlanillaConsultaProcesada();
                    entities.CodigoCanal = Convert.ToString(dr["CODIGO_CANAL"].ToString());
                    entities.DescripcionCanal = Convert.ToString(dr["DESCRIPCION_CANAL"].ToString());
                    entities.IdPlanilla = Convert.ToInt64(dr["ID_PLANILLA"].ToString());
                    entities.FechaPlanilla = Convert.ToDateTime(dr["DT_FECHA_PLANILLA"].ToString());
                    entities.TotalPlanilla = Convert.ToDecimal(dr["DC_TOTAL_PLANILLA"].ToString());                                        
                    entities.NumeroOperacion = Convert.ToString(dr["VC_NUMERO_OPERACION"].ToString());
                    //entities.IdDeposito = Convert.ToInt64(dr["ID_DEPOSITO"].ToString());
                    entities.FechaDeposito = Convert.ToDateTime(dr["DT_FECHA_DEPOSITO"].ToString());
                    entities.TotalDeposito = Convert.ToDecimal(dr["DC_TOTAL_DEPOSITO"].ToString());
                    entities.SaldoDeposito = Convert.ToDecimal(dr["DC_SALDO_DEPOSITO"].ToString());
                    entities.ImporteDeposito = Convert.ToDecimal(dr["DC_IMPORTE_DEPOSITO"].ToString());
                    entities.Usuario = Convert.ToString(dr["VC_USUARIO_CREACION"].ToString());
                    entities.EstadoPlanilla = dr["VC_ESTADO_PLANILLA"].ToString();//Conciliaciones 1.3.1.8.5
                    entities.IdEstadoPlanilla = dr["ESTADOPLANILLA"].ToString();//Conciliaciones 1.3.1.8.5
                    entities.Banco = Convert.ToString(dr["BANCO"].ToString());
                    entities.FechaConciliacion = Convert.ToDateTime(dr["DT_FECHA_CONCILIACION"].ToString());
                    entities.comision = Convert.ToDecimal(dr["COMISION"].ToString());
                    entities.MedioDePago = Convert.ToString(dr["MEDIO_PAGO"].ToString());
                    entities.moneda = Convert.ToString(dr["MONEDA"].ToString());


                    listaPlanillas.Add(entities);
                }
            }

            return Task.FromResult<List<PlanillaConsultaProcesada>>(listaPlanillas);
        }


        public Task<List<PlanillaConsultaProcesadaLog>> ConsultarPlanillasProcesadasLog(DatosConsultaPlanilla datosConsultaPlanilla)
        {
            PlanillaConsultaProcesadaLog entities = null;
            List<PlanillaConsultaProcesadaLog> listaPlanillasLog = new List<PlanillaConsultaProcesadaLog>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsultaPlanilla.IdProducto;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaHasta);

            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CANAL", OracleDbType.Long, datosConsultaPlanilla.IdCanal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_ENTIDAD", OracleDbType.Long, datosConsultaPlanilla.IdBanco, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CUENTA", OracleDbType.Long, datosConsultaPlanilla.IdCuenta, ParameterDirection.Input));

            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PLANILLA_LOG", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new PlanillaConsultaProcesadaLog();               
                    entities.planilla = Convert.ToInt64(dr["ID_PLANILLA"].ToString());
                    entities.procesoGeneral = Convert.ToInt64(dr["ID_PROCESO_GENERAL"].ToString());
                    entities.fechaProceso = Convert.ToString(dr["DT_FECHA_PROCESO"].ToString());
                    entities.logError = Convert.ToString(dr["LOG_ERROR"].ToString());
                    listaPlanillasLog.Add(entities);
                }
            }

            return Task.FromResult<List<PlanillaConsultaProcesadaLog>>(listaPlanillasLog);
        }




        public Task<List<Planilla>> ConsultarPlanillasPendientes(DatosConsultaPlanilla datosConsultaPlanilla)
        {
            Planilla entities = null;
            List<Planilla> listaPlanillas = new List<Planilla>();
            List<OracleParameter> parameters = new List<OracleParameter>();

            long _idProducto = datosConsultaPlanilla.IdProducto;
            string _fechaDesde = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaDesde);
            string _fechaHasta = string.Format("{0:dd/MM/yyyy}", datosConsultaPlanilla.FechaHasta);
                   
            parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.NVarchar2, _fechaDesde, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.NVarchar2, _fechaHasta, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PRODUCTO", OracleDbType.Long, _idProducto, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_CANAL", OracleDbType.Long, datosConsultaPlanilla.IdCanal, ParameterDirection.Input));
            parameters.Add(new OracleParameter("P_ID_PLANILLA", OracleDbType.Long, string.IsNullOrEmpty(datosConsultaPlanilla.IdPlanilla) ? (long)0:Convert.ToInt64(datosConsultaPlanilla.IdPlanilla), ParameterDirection.Input));
            
            parameters.Add(new OracleParameter("P_CU_SALIDA", OracleDbType.RefCursor, ParameterDirection.Output));

            using (OracleDataReader dr = (OracleDataReader)_connectionBase.ExecuteByStoredProcedure("RAC_CONCILIACION_CONSULTAS.SEL_PLANILLA", parameters, ConnectionBase.enuTypeDataBase.OracleConciliacion))
            {
                while (dr.Read())
                {
                    entities = new Planilla();
                    entities.IdPlanilla = Convert.ToInt32(dr["ID_PLANILLA"].ToString());
                    entities.DcTotal = Convert.ToDecimal(dr["DC_TOTAL"].ToString()).ToString("00.00");
                    entities.DtFechaPlanilla = Convert.ToDateTime(dr["DT_FECHA_PLANILLA"]);
                    entities.CodigoCanal = dr["CODIGO_CANAL"].ToString();
                    entities.DescripcionCanal = dr["DESCRIPCION_CANAL"].ToString();
                    //entities.DtFechaPlanilla = Convert.ToDateTime(dr["DT_FECHA_PLANILLA"].ToString()).ToShortDateString();
                    //entities.IdMoneda = Convert.ToInt64(dr["ID_MONEDA"].ToString());                   
                    listaPlanillas.Add(entities);
                }
            }

            return Task.FromResult<List<Planilla>>(listaPlanillas);
        }

        public Task<string> EliminarFacturaDePlanilla(long idPlanilla)
        {
            string _sMessage = string.Empty;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NIDPAYROLL", OracleDbType.Int64, idPlanilla, ParameterDirection.Input));
            var pResult = new OracleParameter("P_MESSAGE", OracleDbType.Varchar2, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.PKG_PV_PAYROLL.SP_DELETE_BILL_POLICY", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _sMessage = pResult.Value.ToString() == null ? String.Empty : pResult.Value.ToString();

            return Task.FromResult<string>(_sMessage);
        }


        public Task<int> ValidarFacturaDePlanilla(long idPlanilla) {
            int _cantidad = 0;

            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("P_NIDPAYROLL", OracleDbType.Int64, idPlanilla, ParameterDirection.Input));
            var pResult = new OracleParameter("P_COUNT", OracleDbType.Int32, 32767, OracleDbType.Int32, ParameterDirection.Output);
            parameters.Add(pResult);

            _connectionBase.ExecuteByStoredProcedure("INSUDB.PKG_PV_PAYROLL.SP_VALIDAR_EXISTE_BILL", parameters, ConnectionBase.enuTypeDataBase.OracleVTime);
            _cantidad = pResult.Value == null ? (int)0 : Convert.ToInt32(pResult.Value.ToString());

            return Task.FromResult<int>(_cantidad);
        }



    }
}
