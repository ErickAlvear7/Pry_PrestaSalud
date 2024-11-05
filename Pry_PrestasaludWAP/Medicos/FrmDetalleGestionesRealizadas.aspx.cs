namespace Pry_PrestasaludWAP.Medicos
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmDetalleGestionesRealizadas : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        string codigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["Regresar"] = Request["Regresar"];
                funCargaMantenimiento(int.Parse(ViewState["CodigoCita"].ToString()));
            }
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento(int codigoCita)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = codigoCita;
                objparam[1] = "";
                objparam[2] = 117;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvTitular.DataSource = dt.Tables[0];
                    grdvTitular.DataBind();

                    grdvPaciente.DataSource = dt.Tables[1];
                    grdvPaciente.DataBind();

                    grdvHistorial.DataSource = dt.Tables[2];
                    grdvHistorial.DataBind();
                }
                lbltitulo.Text = "Citas-Registradas " + ViewState["NombreMedico"];
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = grdvHistorial.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ScriptManager.RegisterStartupScript(this.updHistorial, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDetalleObservaGestiones.aspx?Codigo=" + codigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            switch (ViewState["Regresar"].ToString())
            {
                case "0":
                    Response.Redirect("FrmGestionesRealizadasAdmin.aspx", true);
                    break;
                case "1":
                    Response.Redirect("../Administrador/FrmGesReaMedAdmin.aspx", true);
                    break;
            }
        }
        #endregion

    }
}