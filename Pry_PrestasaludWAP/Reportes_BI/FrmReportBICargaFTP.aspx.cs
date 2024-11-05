using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reportes_BI_FrmReportBICargaFTP : Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");
        if (!IsPostBack)
        {
            if (Session["SalirAgenda"].ToString() == "NO")
            {
                if (Session["TipoCita"].ToString() == "CitaMedica")
                    Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                if (Session["TipoCita"].ToString() == "CitaOdontologica")
                    Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                return;
            }
            txtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lbltitulo.Text = "Reportes BI por cargas FTP";
            Array.Resize(ref objparam, 6);
            objparam[0] = 0;
            objparam[1] = 0;
            objparam[2] = "";
            objparam[3] = "";
            objparam[4] = 2;
            objparam[5] = "";
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporCargaFTP", objparam);
            ddlCampainIni.DataSource = dt;
            ddlCampainIni.DataTextField = "Campain";
            ddlCampainIni.DataValueField = "Codigo";
            ddlCampainIni.DataBind();

            objparam[0] = 1;
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporCargaFTP", objparam);
            ddlCampainFin.DataSource = dt;
            ddlCampainFin.DataTextField = "Campain";
            ddlCampainFin.DataValueField = "Codigo";
            ddlCampainFin.DataBind();
        }
    }
    #endregion

    #region Botones y Eventos
    protected void btnProcesar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCampainIni.SelectedItem.ToString() == "--Seleccione Campain--")
            {
                lblerror.Text = "Seleccione Campain de Inicio..!";
                return;
            }
            if (ddlCampainFin.SelectedItem.ToString() == "--Seleccione Campain--")
            {
                lblerror.Text = "Seleccione Campain Comparación..!";
                return;
            }
            if (!new Funciones().IsDate(txtFechaIni.Text))
            {
                lblerror.Text = "Fecha Inicio, No es una fecha válida..!";
                return;
            }
            if (!new Funciones().IsDate(txtFechaFin.Text))
            {
                lblerror.Text = "Fecha Fin, No es una fecha válida..!";
                return;
            }
            if (DateTime.ParseExact(txtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
            {
                lblerror.Text = "La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!";
                return;
            }
            //DateTime strFechaIni = DateTime.Parse(txtFechaIni.Text);
            //DateTime strFechaFin = DateTime.Parse(txtFechaFin.Text);
            Array.Resize(ref objparam, 6);
            objparam[0] = int.Parse(ddlCampainIni.SelectedValue);
            objparam[1] = int.Parse(ddlCampainFin.SelectedValue);
            objparam[2] = txtFechaIni.Text;
            objparam[3] = txtFechaIni.Text;
            objparam[4] = 1;
            objparam[5] = ddlCondicion.SelectedValue;
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporCargaFTP", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                imgExpCsv.Visible = true;
                imgExportar.Visible = true;
                Label6.Visible = true;
                Label7.Visible = true;
            }
            else
            {
                imgExpCsv.Visible = false;
                imgExportar.Visible = false;
                Label6.Visible = false;
                Label7.Visible = false;
            }
            grdvDatos.DataSource = dt;
            grdvDatos.DataBind();

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mantenedor/FrmDetalle.aspx");
    }
    protected void imgExportar_Click(object sender, ImageClickEventArgs e)
    {        
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Reporte_BI_" + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        grdvDatos.GridLines = GridLines.Both;
        grdvDatos.HeaderStyle.Font.Bold = true;
        grdvDatos.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        Response.End();  
    }
    protected void imgExpCsv_Click(object sender, ImageClickEventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        string FileName = "Reporte_BI_" + DateTime.Now + ".csv";
        Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
        Response.Charset = "";
        Response.ContentType = "application/text";
        StringBuilder sBuilder = new System.Text.StringBuilder();
        for (int index = 0; index < grdvDatos.Columns.Count; index++)
        {
            sBuilder.Append(grdvDatos.Columns[index].HeaderText + ',');
        }
        sBuilder.Append("\r\n");
        for (int i = 0; i < grdvDatos.Rows.Count; i++)
        {
            for (int k = 0; k < grdvDatos.HeaderRow.Cells.Count; k++)
            {
                sBuilder.Append(grdvDatos.Rows[i].Cells[k].Text.Replace(",", "") + ",");
            }
            sBuilder.Append("\r\n");
        }
        Response.Output.Write(sBuilder.ToString());
        Response.Flush();
        Response.End();
    }
    #endregion
}