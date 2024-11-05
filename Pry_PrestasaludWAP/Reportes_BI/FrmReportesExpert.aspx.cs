using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Globalization;
public partial class Reportes_BI_FrmReportesExpert : Page
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
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.imgExportar);
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
            ddlCampain.DataSource = dt;
            ddlCampain.DataTextField = "Campain";
            ddlCampain.DataValueField = "Codigo";
            ddlCampain.DataBind();

        }
        else
        {
            grdvDatos.DataSource = Session["grdvDatos"];
            grdvDatos1.DataSource = Session["grdvDatos1"];
        }
    }
    #endregion

    #region Botones y Eventos
    protected void imgExportar_Click(object sender, ImageClickEventArgs e)
    {

        Response.Clear();
        Response.Buffer = true;
        string FileName = rdbReporFTP.Checked == true ? "Reporte_" + rdbReporFTP.Text + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls" :
                "Reporte_" + rdbReporABF.Text + "_" + ddlTipoCliente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            if (rdbReporABF.Checked)
            {
                switch (ddlTipoCliente.SelectedItem.ToString())
                {
                    case "TITULAR":
                        grdvDatos1.AllowPaging = false;
                        grdvDatos1.DataSource = (DataSet)Session["grdvDatos1"];
                        grdvDatos1.DataBind();
                        grdvDatos1.HeaderRow.BackColor = Color.White;
                        foreach (TableCell cell in grdvDatos1.HeaderRow.Cells)
                        {
                            cell.BackColor = grdvDatos1.HeaderStyle.BackColor;
                        }
                        foreach (GridViewRow row in grdvDatos1.Rows)
                        {
                            row.BackColor = Color.White;
                            row.Cells[3].Style.Add("mso-number-format", "\\@");
                            row.Cells[8].Style.Add("mso-number-format", "\\@");
                            foreach (TableCell cell in row.Cells)
                            {
                                cell.ToString();
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = grdvDatos1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    cell.BackColor = grdvDatos1.RowStyle.BackColor;
                                }
                                cell.CssClass = "textmode";
                            }
                        }
                        grdvDatos1.RenderControl(hw);
                        break;

                    case "BENEFICIARIO":
                        grdvDatos.AllowPaging = false;
                        grdvDatos.DataSource = (DataSet)Session["grdvDatos"];
                        grdvDatos.DataBind();
                        grdvDatos.HeaderRow.BackColor = Color.White;
                        foreach (TableCell cell in grdvDatos.HeaderRow.Cells)
                        {
                            cell.BackColor = grdvDatos.HeaderStyle.BackColor;
                        }
                        foreach (GridViewRow row in grdvDatos.Rows)
                        {
                            row.BackColor = Color.White;
                            row.Cells[3].Style.Add("mso-number-format", "\\@");
                            row.Cells[8].Style.Add("mso-number-format", "\\@");
                            row.Cells[21].Style.Add("mso-number-format", "\\@");
                            row.Cells[26].Style.Add("mso-number-format", "\\@");
                            foreach (TableCell cell in row.Cells)
                            {
                                cell.ToString();
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = grdvDatos1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    cell.BackColor = grdvDatos1.RowStyle.BackColor;
                                }
                                cell.CssClass = "textmode";
                            }
                        }
                        grdvDatos.RenderControl(hw);
                        break;
                }


            }
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    protected void btnProcesar_Click(object sender, EventArgs e)
    {
        try
        {
            lblerror.Text = "";
            imgExportar.Visible = false;
            Label6.Visible = false;
            divBeneficiario.Visible = false;
            divTitular.Visible = false;
            string inCodigo = "(";
            if (lstProdSelec.Items.Count == 0)
            //if (ddlCampain.SelectedItem.ToString() == "--Seleccione Cliente--")
            {
                lblerror.Text = "Seleccione Producto..!";
                return;
            }

            if (!new Funciones().IsDate(txtFechaIni.Text))
            {
                lblerror.Text = "No es una fecha válida..!";
                return;
            }
            if (!new Funciones().IsDate(txtFechaFin.Text))
            {
                lblerror.Text = "No es una fecha válida..!";
                return;
            }
            if (DateTime.ParseExact(txtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
            {
                lblerror.Text = "La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!";
                return;
            }

            foreach (ListItem item in lstProdSelec.Items)
            {
                inCodigo += item.Value.ToString() + ","; 
            }
            inCodigo = inCodigo.Remove(inCodigo.Length - 1) + ")";
            Array.Resize(ref objparam, 6);
            objparam[0] = ddlTipoCliente.SelectedItem.ToString();
            objparam[1] = txtFechaIni.Text;
            objparam[2] = txtFechaFin.Text;
            objparam[3] = inCodigo;
            objparam[4] = ddlMovimiento.SelectedItem.ToString().Substring(0, 1);
            if (rdbReporFTP.Checked == true) objparam[5] = 0;
            if (rdbReporABF.Checked == true) objparam[5] = 1;
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporteABF", objparam);

            System.Threading.Thread.Sleep(3000);

            if (dt.Tables[0].Rows.Count > 0)
            {
                imgExportar.Visible = true;
                Label6.Visible = true;
                if (rdbReporABF.Checked)
                {
                    switch (ddlTipoCliente.SelectedItem.ToString())
                    {
                        case "TITULAR":
                            divTitular.Visible = true;
                            grdvDatos1.DataSource = dt;
                            Session["grdvDatos1"] = grdvDatos1.DataSource;
                            grdvDatos1.DataBind();
                            break;
                        case "BENEFICIARIO":
                            divBeneficiario.Visible = true;
                            grdvDatos.DataSource = dt;
                            Session["grdvDatos"] = grdvDatos.DataSource;
                            grdvDatos.DataBind();
                            break;
                    }
                }
                //grdvDatos.Visible = true;
                //grdvDatos.UseAccessibleHeader = true;
                //grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            //UpdatePanel2.Update();
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
    protected void grdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDatos.PageIndex = e.NewPageIndex;
        grdvDatos.DataBind();

    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mantenedor/FrmDetalle.aspx");
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        bool agregar = true;
        if (ddlCampain.SelectedItem.ToString() != "" || ddlCampain.SelectedItem.ToString() != "--Seleccione Producto--")
        {
            btnQuitar.Visible = true;
            if (lstProdSelec.Items.Count == 0)
            {
                lstProdSelec.Items.Add(new ListItem(ddlCampain.SelectedItem.ToString(), ddlCampain.SelectedValue));
            }
            foreach (ListItem item in lstProdSelec.Items)
            {
                if (ddlCampain.SelectedValue == item.Value)
                {
                    agregar = true;
                    break;
                }
                else agregar = false;
            }
            if (agregar == false) lstProdSelec.Items.Add(new ListItem(ddlCampain.SelectedItem.ToString(), ddlCampain.SelectedValue));
        }
        new Funciones().funOrdenar(lstProdSelec);
    }
    protected void btnQuitar_Click(object sender, EventArgs e)
    {
        if (lstProdSelec.SelectedIndex >= 0) lstProdSelec.Items.RemoveAt(lstProdSelec.SelectedIndex);
        if (lstProdSelec.Items.Count == 0) btnQuitar.Visible = true;

    }
    
    protected void grdvDatos1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDatos1.PageIndex = e.NewPageIndex;
        grdvDatos1.DataBind();
    }
    #endregion

}