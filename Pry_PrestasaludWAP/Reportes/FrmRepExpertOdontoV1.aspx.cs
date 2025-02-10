namespace Pry_PrestasaludWAP.Reportes
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmRepExpertOdontoV1 : Page
    {
        #region Variables
        Object[] objparam = new Object[1];
        DataSet ds = new DataSet();
        int opcion = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.imgExportar);
            if (!IsPostBack)
            {
                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
                funCascadaCombos(0);
                funCascadaCombos(1);
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Procedimientos y Funciones
        private void funCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 38;
                    //ddlTipoAgenda.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    //ddlTipoAgenda.DataTextField = "Descripcion";
                    //ddlTipoAgenda.DataValueField = "Codigo";
                    //ddlTipoAgenda.DataBind();
                    break;
                case 1:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 51;
                    ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlCliente.DataTextField = "Descripcion";
                    ddlCliente.DataValueField = "Codigo";
                    ddlCliente.DataBind();
                    break;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        #endregion

        #region Botones y Eventos
        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
  
            if (!new Funciones().IsDateNewx(txtFechaIni.Text))
            {
                lblerror.Text = "No es una fecha válida..!";
                return;
            }
            if (!new Funciones().IsDateNewx(txtFechaFin.Text))
            {
                lblerror.Text = "No es una fecha válida..!";
                return;
            }
            if (DateTime.ParseExact(txtFechaIni.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            {
                lblerror.Text = "La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!";
                return;
            }
            System.Threading.Thread.Sleep(300);
            Array.Resize(ref objparam, 6);
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 0;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 1;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 2;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 3;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 4;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 5;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 6;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 7;

            imgExportar.Visible = true;
            lblExportar.Visible = true;

            if (ddlCliente.SelectedValue == "0") objparam[0] = 0;
            if (ddlCliente.SelectedValue != "0") objparam[0] = 1;


            objparam[1] = int.Parse(ddlCliente.SelectedValue.ToString());
            objparam[2] = "";
            objparam[3] = "";
            objparam[4] = txtFechaIni.Text;
            objparam[5] = txtFechaFin.Text;
            ds = new Conexion(2, "").funConsultarSqls("sp_ReportesExpertOdonto", objparam);
 
            if (ds.Tables[0].Rows.Count > 0 || ds.Tables[0] != null)
            {
                grdvDatos.DataSource = ds.Tables[0];
                ViewState["grdvDatos"] = ds.Tables[0];
                grdvDatos.DataBind();
                
            }
            else
            {
                new Funciones().funShowJSMessage("No se encontraron datos..!", this);
            }
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
                    row.Cells[5].Style.Add("mso-number-format", "\\@");
                }
                grdvDatos.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}