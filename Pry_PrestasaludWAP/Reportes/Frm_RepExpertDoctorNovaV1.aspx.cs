using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Reportes
{
    public partial class Frm_RepExpertDoctorNovaV1 : System.Web.UI.Page
    {
        #region Variables
        Object[] objparam = new Object[1];
        DataSet ds = new DataSet();
        #endregion
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
                txtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtFechaFinal.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
                FunCascadaCombos(1);

            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];

        }

        #region Procedimientos y Funciones
        private void FunCascadaCombos(int opcion)
        {
            switch (opcion)
            {
               
                case 1:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 57;
                    ddlClienteNova.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlClienteNova.DataTextField = "Descripcion";
                    ddlClienteNova.DataValueField = "Codigo";
                    ddlClienteNova.DataBind();
                    ddlClienteNova.SelectedIndex = 0;
                    break;
            }
        }
        #endregion

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            if (!new Funciones().IsDate(txtFechaInicio.Text))
            {
                new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                return;
            }

            if (!new Funciones().IsDate(txtFechaFinal.Text))
            {
                new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                return;
            }

            if (DateTime.ParseExact(txtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFinal.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
            {
                new Funciones().funShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                return;
            }

            DateTime fecha = DateTime.ParseExact("10/01/2024", "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicio = DateTime.ParseExact(txtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(txtFechaFinal.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            TimeSpan tiempo = fechaFin.Subtract(fechaInicio);
            int dia = tiempo.Days;
            if(dia > 90)
            {
                new Funciones().funShowJSMessage("No existen gestiones a partir de esta fecha", this);
                return;
            }

            //if (fechaInicio <= fecha)
            //{
            //    new Funciones().funShowJSMessage("No existen gestiones a partir de esta fecha", this);
            //    return;
            //}

            Array.Resize(ref objparam, 4);
            System.Threading.Thread.Sleep(500);
            objparam[0] = 0;
            objparam[1] = txtFechaInicio.Text;
            objparam[2] = txtFechaFinal.Text;
            objparam[3] = ddlClienteNova.SelectedValue;
            //ds = new Conexion(2, "").funConsultarSqls("sp_ReportesExpertDoctorNova", objparam);
            ds = new Conexion(2, "").FunConsultarSQLNOVA(objparam);

            grdvDatos.DataSource = ds.Tables[0];
            grdvDatos.DataBind();
            ViewState["grdvDatos"] = ds.Tables[1];
            //ViewState["grdvDatos"] = grdvDatos.DataSource;
            totalreg.InnerHtml = "Total Registro: " + ds.Tables[1].Rows.Count.ToString();
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgExportar.Visible = true;
                lblExportar.Visible = true;
            }
            else
            {
                imgExportar.Visible = false;
                lblExportar.Visible = false;
                new Funciones().funShowJSMessage("No existen datos para mostrar", this);
                return;
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void grdvDatos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            grdvDatos.PageIndex = e.NewPageIndex;
            grdvDatos.DataBind();
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }

        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            string FileName = "ReporteExpertDoctor_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdvDatos.AllowPaging = false;
                grdvDatos.DataSource = (DataTable)ViewState["grdvDatos"];
                grdvDatos.DataBind();
                //grdvDatos.HeaderRow.BackColor = Color.White;
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    //row.BackColor = Color.White;
                    row.Cells[7].Style.Add("mso-number-format", "\\@");
                    //row.Cells[2].Style.Add("mso-number-format", "\\@");
                    //row.Cells[3].Style.Add("mso-number-format", "\\@");
                    //row.Cells[15].Style.Add("mso-number-format", "\\@");
                }
                grdvDatos.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }
    }
}