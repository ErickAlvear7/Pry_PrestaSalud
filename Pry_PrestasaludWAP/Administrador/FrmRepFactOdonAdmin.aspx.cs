namespace Pry_PrestasaludWAP.Administrador
{
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;

    public partial class FrmRepFactOdonAdmin : Page
    {
        #region Variables
        DataSet _dtsfactura = new DataSet();
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        string _sql = "", _prestador = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                pnlFacturacion.Visible = false;
                TxtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Reporte Facturación ODONTOLOGICO ";
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFacturacion.Visible = false;
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                pnlFacturacion.Visible = false;

                if (!new Funciones().IsDate(TxtFechaDesde.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new Funciones().IsDate(TxtFechaHasta.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (DateTime.ParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new Funciones().funShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    return;
                }

                _sql = "SELECT CodigoCita = isnull((select cogo_secuencial from Expert_GENERAR_SECUENCIALO where cogo_hicocodigo=(select HIx.HICO_CODIGO from Expert_HISTORIAL_CITAODONTO HIx where HIx.CIOD_CODIGO=CI.CIOD_CODIGO and HIx.hico_estadocita='A')),CI.CIOD_CODIGO),Producto = PR.prod_nombre,Tipo = case CI.ciod_tipocliente when 'T' then 'TITULAR' else 'BENEFICIARIO' end,Paciente = case CI.ciod_tipocliente when 'T' then (select pers_primernombre+' '+pers_segundonombre+' '+pers_primerapellido+' '+ pers_segundoapellido from Expert_PERSONA PE where PE.PERS_CODIGO in(select PERS_CODIGO from Expert_TITULAR TI where TI.TITU_CODIGO=CI.ciod_titucodigo)) else (select bene_primernombre+' '+bene_segundonombre+' '+bene_primerapellido+' '+bene_segundoapellido from Expert_BENEFICIARIO BE where BE.BENE_CODIGO=CI.ciod_benecodigo) end,Identificacion = PE.pers_numerodocumento,Procedimiento = (select PO.pcod_procedimiento from Expert_PROCEDIMIENTOODONTO PO where PO.PCOD_CODIGO in(select PD.psde_pcodcodigo from Expert_PRESUPUESTO_DETALLE PD where PD.PSDE_CODIGO=PV.PSDE_CODIGO)),Pieza = PD.psde_piezadental,Pvp = PD.psde_pvp,Costo = PD.psde_costo,Cobertura = PD.psde_cobertura,Total = case PD.psde_cobertura when 100 then PD.psde_costo else (PD.psde_costo-PD.psde_total) end,FechaCita = (select top 1 convert(varchar(10),HIx.hico_fechacita,121) from Expert_HISTORIAL_CITAODONTO HIx where HIx.CIOD_CODIGO=CI.CIOD_CODIGO and HIx.hico_estadocita='A'),FechaRegistro = (select top 1 convert(varchar(10),HIx.hico_fechacita,121) from Expert_HISTORIAL_CITAODONTO Hix where Hix.CIOD_CODIGO=CI.CIOD_CODIGO and HIx.hico_estadocita='T'),Prestadora = (select prod_nombre from Expert_PRESTADORA_ODONTO where PPRO_CODIGO=CI.PPRO_CODIGO),Medico = (select MED.meod_nombre+' '+MED.meod_apellido from Expert_MEDICO_ODONTO MED WHERE MED.MEOD_CODIGO=CI.MEOD_CODIGO) from Expert_CITAODONTO CI INNER JOIN Expert_HISTORIAL_CITAODONTO HI ON CI.CIOD_CODIGO=HI.CIOD_CODIGO INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO INNER JOIN Expert_PERSONA PE ON PE.PERS_CODIGO=TI.PERS_CODIGO INNER JOIN Expert_PRODUCTOS PR (nolock) ON PR.PROD_CODIGO=TI.PROD_CODIGO INNER JOIN Expert_PRESUPUESTO_EVENTOS PV ON PV.psev_hicocodigo = HI.HICO_CODIGO INNER JOIN Expert_PRESUPUESTO_DETALLE PD ON PD.PSDE_CODIGO=PV.PSDE_CODIGO where convert(date,HI.hico_fechacita,101) between convert(date,'" + TxtFechaDesde.Text + "',101) and convert(date,'" + TxtFechaHasta.Text + "',101) and CI.ciod_estatuscita not in('S','C') AND ";

                if (DdlTipoReporte.SelectedValue == "1") _sql += "PD.psde_cobertura>0 AND ";
                else _sql += "PD.psde_cobertura=0 AND ";

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[1] = "";
                _objparam[2] = 168;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        _prestador += _fila["CodigoPRES"].ToString() + ",";
                    }

                    _prestador = _prestador.Remove(_prestador.Length - 1);
                }

                _sql = _sql + "CI.PPRO_CODIGO IN(" + _prestador + ")";

                Array.Resize(ref _objparam, 11);
                _objparam[0] = 30;
                _objparam[1] = _sql;
                _objparam[2] = "";
                _objparam[3] = "";
                _objparam[4] = "";
                _objparam[5] = "";
                _objparam[6] = 0;
                _objparam[7] = 0;
                _objparam[8] = 0;
                _objparam[9] = 0;
                _objparam[10] = 0;
                _dtsfactura = new Conexion(2, "").FunConsultaDatos1(_objparam);

                if (_dtsfactura.Tables[0].Rows.Count > 0)
                {
                    pnlFacturacion.Visible = true;
                    RptFacturacion.Reset();
                    RptFacturacion.SizeToReportContent = true;
                    RptFacturacion.LocalReport.ReportPath = Server.MapPath("~/Reports/RptFactOdonAdmin.rdlc");
                    RptFacturacion.LocalReport.EnableExternalImages = true;
                    RptFacturacion.LocalReport.DataSources.Clear();
                    ReportDataSource dtrFactura = new ReportDataSource("DtsFactOdonAdmin", _dtsfactura.Tables[0]);
                    RptFacturacion.LocalReport.DataSources.Add(dtrFactura);
                    RptFacturacion.LocalReport.Refresh();
                }
                else
                {
                    pnlFacturacion.Visible = false;
                    new Funciones().funShowJSMessage("No Existen Datos Para Mostrar..!", this);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        } 
        #endregion
    }
}