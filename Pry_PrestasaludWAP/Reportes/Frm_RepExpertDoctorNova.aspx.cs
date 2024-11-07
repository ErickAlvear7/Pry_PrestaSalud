using System;
using System.Data;
using System.Web.UI;

namespace Pry_PrestasaludWAP.Reportes
{
    public partial class Frm_RepExpertDoctorNova : System.Web.UI.Page
    {

        #region Variables
        Object[] objparam = new Object[1];
        DataSet ds = new DataSet();
        //string sentencia1 = "", sentencia2 = "", motivoAgenda = "";
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

                txtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtFechaFinal.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
                FunCascadaCombos(1);
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 57;
                    ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlCliente.DataTextField = "Descripcion";
                    ddlCliente.DataValueField = "Codigo";
                    ddlCliente.DataBind();
                    ddlCliente.SelectedIndex = 1;
                    break;
            }
        }
        #endregion

        #region Botones y Eventos

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

            System.Threading.Thread.Sleep(500);
            objparam[1] = txtFechaInicio.Text;
            objparam[2] = txtFechaFinal.Text;
            objparam[3] = ddlCliente.SelectedValue;
            ds = new Conexion(2, "").funConsultarSqls("sp_ReportesExpertDoctorNova", objparam);

            grdvDatos.DataSource = ds;
            grdvDatos.DataBind();
            ViewState["grdvDatos"] = grdvDatos.DataSource;
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgExportar.Visible = true;
                lblExportar.Visible = true;
            }
            else
            {
                imgExportar.Visible = false;
                lblExportar.Visible = false;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        protected void btnSalir_Click(object sender, EventArgs e)
        {

        }

        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void grdvDatos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            grdvDatos.PageIndex = e.NewPageIndex;
            grdvDatos.DataBind();
        }
        #endregion
    }
}