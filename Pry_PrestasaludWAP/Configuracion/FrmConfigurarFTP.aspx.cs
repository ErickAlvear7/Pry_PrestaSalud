using System;
using System.Data;

namespace Pry_PrestasaludWAP.Configuracion
{
    public partial class FrmConfigurarFTP : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FunCargarCombos();
                ViewState["CodigoCamp"] = Request["CodigoCamp"];
                if (int.Parse(ViewState["CodigoCamp"].ToString()) == 0)
                {
                    lbltitulo.Text = "Nueva Configuración FTP";
                }
                else
                {
                    lblEstado.Visible = true;
                    chkEstado.Visible = true;
                    lbltitulo.Text = "Editar Configuración FTP";
                    FunCargaMantenimiento();
                }

            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos()
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = 5;
            ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlCliente.DataTextField = "Descripcion";
            ddlCliente.DataValueField = "Codigo";
            ddlCliente.DataBind();

            objparam[0] = 40;
            ddlExtension.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlExtension.DataTextField = "Descripcion";
            ddlExtension.DataValueField = "Codigo";
            ddlExtension.DataBind();

            objparam[0] = 41;
            ddlFormato.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlFormato.DataTextField = "Descripcion";
            ddlFormato.DataValueField = "Codigo";
            ddlFormato.DataBind();

            objparam[0] = 42;
            ddlDelimitado.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlDelimitado.DataTextField = "Descripcion";
            ddlDelimitado.DataValueField = "Codigo";
            ddlDelimitado.DataBind();

            objparam[0] = 43;
            ddlHoraInicio.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlHoraInicio.DataTextField = "Descripcion";
            ddlHoraInicio.DataValueField = "Codigo";
            ddlHoraInicio.DataBind(); 
        }

        private void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoCamp"].ToString());
                objparam[1] = "";
                objparam[2] = 101;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlCliente.SelectedValue = dt.Tables[0].Rows[0][0].ToString();
                txtRuta.Text = dt.Tables[0].Rows[0][1].ToString();
                txtNombre.Text = dt.Tables[0].Rows[0][2].ToString();
                ddlExtension.SelectedValue = dt.Tables[0].Rows[0][3].ToString();
                ddlFormato.SelectedValue = dt.Tables[0].Rows[0][4].ToString();
                ddlDelimitado.SelectedValue = dt.Tables[0].Rows[0][5].ToString();
                ddlHoraInicio.SelectedValue = dt.Tables[0].Rows[0][6].ToString();
                txtUsuarioFTP.Text = dt.Tables[0].Rows[0][7].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][8].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][8].ToString() == "Activo" ? true : false;
                chkEnviar.Text = dt.Tables[0].Rows[0][9].ToString();
                chkEnviar.Checked = dt.Tables[0].Rows[0][9].ToString() == "SI" ? true : false;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
 
            }
 
        }
        #endregion

        #region Botones y Eventos
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            try
            {
                if (ddlCliente.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Cliente para FTP..!";
                    return;
                }
                if (string.IsNullOrEmpty(txtRuta.Text.Trim()))
                {
                    lblerror.Text = "Ingrese ruta del archivo..!";
                    return;
                }
                if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
                {
                    lblerror.Text = "Ingrese Nombre del Archivo..!";
                    return;
                }
                if (ddlExtension.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione extensión del archivo..!";
                    return;
                }
                if (ddlFormato.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione formato del archivo..!";
                    return;
                }
                if (ddlDelimitado.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione delimitador del archivo..!";
                    return;
                }
                if (ddlHoraInicio.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Hora de inicio del FTP..!";
                    return;
                }
                if (string.IsNullOrEmpty(txtUsuarioFTP.Text.Trim()))
                {
                    lblerror.Text = "Ingrese usuario FTP..!";
                    return;
                }
                Array.Resize(ref objparam, 13);
                objparam[0] = int.Parse(ViewState["CodigoCamp"].ToString());
                objparam[1] = int.Parse(ddlCliente.SelectedValue);
                objparam[2] = txtRuta.Text.Trim();
                objparam[3] = txtNombre.Text.Trim();
                objparam[4] = ddlExtension.SelectedValue;
                objparam[5] = ddlFormato.SelectedValue;
                objparam[6] = ddlDelimitado.SelectedValue;
                objparam[7] = chkEstado.Checked;
                objparam[8] = ddlHoraInicio.SelectedValue;
                objparam[9] = chkEnviar.Checked == true ? "S" : "N";
                objparam[10] = txtUsuarioFTP.Text.Trim();
                objparam[11] = int.Parse(Session["usuCodigo"].ToString());
                objparam[12] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevaConfiguracionFTP", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmProgramarFTPAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEnviar_CheckedChanged(object sender, EventArgs e)
        {
            chkEnviar.Text = chkEnviar.Checked == true ? "SI" : "NO";
        }
        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmProgramarFTPAdmin.aspx", false);
        }
        #endregion

    }
}