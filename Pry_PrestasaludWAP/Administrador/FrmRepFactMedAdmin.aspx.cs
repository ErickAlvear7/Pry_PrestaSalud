namespace Pry_PrestasaludWAP.Administrador
{
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;

    public partial class FrmRepFactMedAdmin : Page
    {
        #region Variables
        DataSet _dtsFactura = new DataSet();
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        string _prestador = "", _sql = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                TxtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                pnlFacturacion.Visible = false;
                Lbltitulo.Text = "Reporte Facturación MEDICO";
            }
        }
        #endregion

        #region Botones y Eventos
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

                _sql = "SELECT CodigoCita = ISNULL((SELECT coge_secuencial FROM Expert_GENERAR_SECUENCIAL WHERE coge_hicicodigo=(select HIx.HICI_CODIGO FROM Expert_HISTORIAL_CITAMEDICA HIx WHERE HIx.CITA_CODIGO=CI.CITA_CODIGO AND HIx.hici_estadocita='A')),CI.CITA_CODIGO),Producto = PR.prod_nombre,Tipo = CASE CI.cita_tipocliente WHEN 'T' THEN 'TITULAR' ELSE 'BENEFICIARIO' END,Paciente = CASE CI.cita_tipocliente WHEN 'T' THEN (SELECT pers_nombrescompletos from Expert_PERSONA PE WHERE PE.PERS_CODIGO in(SELECT PERS_CODIGO FROM Expert_TITULAR TI WHERE TI.TITU_CODIGO=CI.cita_titucodigo)) ELSE (SELECT bene_nombrescompletos FROM Expert_BENEFICIARIO BE WHERE BE.BENE_CODIGO=CI.cita_benecodigo) END,Cedula = PE.pers_numerodocumento,Agenda = (SELECT pade_nombre from Expert_PARAMETRO_DETALLE where pade_valor=(select HIx.hici_descripcioncita from Expert_HISTORIAL_CITAMEDICA HIx where HIx.hici_estadocita='A' AND HIx.CITA_CODIGO=CI.CITA_CODIGO) and PARA_CODIGO in(select PARA_CODIGO from Expert_PARAMETRO_CABECERA where para_nombre='MOTIVOS CITA')),Atencion = isnull((SELECT PD.pade_nombre from Expert_PARAMETRO_DETALLE PD where pade_valor=(select top 1 hix.hici_descripcioncita from Expert_HISTORIAL_CITAMEDICA HIx where HIx.CITA_CODIGO=CI.CITA_CODIGO and HIx.hici_estadocita='T') and PD.PARA_CODIGO in(select PC.PARA_CODIGO from Expert_PARAMETRO_CABECERA PC where PC.para_nombre='CIE10V2')),'NO ENCUENTRA CIE10'),Pvp = (select convert(decimal(12,2),case PX.pree_auxi1 when ' ' then '0.00' else PX.pree_auxi1 end) from Expert_PRESTADORAESPECI PX where PX.PRES_CODIGO=CI.PRES_COD and PX.EXPR_CODIGO=CI.EXPR_CODIGO),Costo = (select convert(decimal(12,2),case PX.pree_auxi2 when ' ' then '0.00' else PX.pree_auxi2 end) from Expert_PRESTADORAESPECI PX where PX.PRES_CODIGO=CI.PRES_COD and PX.EXPR_CODIGO=CI.EXPR_CODIGO),FechaCita = (select top 1 convert(varchar(10),HIx.hici_fechacita,121) from Expert_HISTORIAL_CITAMEDICA HIx where HIx.CITA_CODIGO=CI.CITA_CODIGO and HIx.hici_estadocita='A'),FechaRegistro = (select top 1 convert(varchar(10),HIx.hici_fechacita,121) from Expert_HISTORIAL_CITAMEDICA Hix where Hix.CITA_CODIGO=CI.CITA_CODIGO and HIx.hici_estadocita='T'),Prestadora = (select pres_nombre from Expert_PRESTADORA where PRES_COD=CI.PRES_COD),Medico = (select medi_nombre + ' '+ medi_apellido from Expert_MEDICO where MEDI_COD=CI.MEDI_COD) FROM Expert_CITAMEDICA CI INNER JOIN Expert_HISTORIAL_CITAMEDICA HI ON CI.CITA_CODIGO=HI.CITA_CODIGO INNER JOIN Expert_TITULAR TI ON CI.cita_titucodigo=TI.TITU_CODIGO INNER JOIN Expert_PERSONA PE ON PE.PERS_CODIGO=TI.PERS_CODIGO INNER JOIN Expert_PRODUCTOS PR (nolock) ON PR.PROD_CODIGO=TI.PROD_CODIGO WHERE CI.cita_estatuscita in('T') AND HI.hici_estadocita='A' AND CONVERT(DATE,HI.hici_fechacita,101) BETWEEN CONVERT(DATE,'" + TxtFechaDesde.Text + "',101) AND CONVERT(DATE,'" + TxtFechaHasta.Text + "',101) AND ";

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[1] = "";
                _objparam[2] = 167;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        _prestador += _fila["CodigoPRES"].ToString() + ",";
                    }

                    _prestador = _prestador.Remove(_prestador.Length - 1);
                }

                _sql = _sql + "CI.PRES_COD IN(" + _prestador + ")";

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
                _dtsFactura = new Conexion(2, "").FunConsultaDatos1(_objparam);

                if (_dtsFactura.Tables[0].Rows.Count > 0)
                {
                    pnlFacturacion.Visible = true;
                    RptFacturacion.Reset();
                    RptFacturacion.SizeToReportContent = true;
                    RptFacturacion.LocalReport.ReportPath = Server.MapPath("~/Reports/RptFactMedAdmin.rdlc");
                    RptFacturacion.LocalReport.EnableExternalImages = true;
                    RptFacturacion.LocalReport.DataSources.Clear();
                    ReportDataSource dtrFactura = new ReportDataSource("DtsFactMedAdmin", _dtsFactura.Tables[0]);
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