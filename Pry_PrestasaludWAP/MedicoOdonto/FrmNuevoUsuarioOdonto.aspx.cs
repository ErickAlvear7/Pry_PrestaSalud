using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.MedicoOdonto
{
    public partial class FrmNuevoUsuarioOdonto : System.Web.UI.Page
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
                ViewState["CodigoMedico"] = Request["CodigoMedico"];
                ViewState["CodigoUsuario"] = Request["CodigoUsuario"];
                //funCargarCombos();
                funCargarMantenimiento();
                lbltitulo.Text = "Asignar Usuario a:" + txtNombres.InnerText + " " + txtApellidos.InnerText;
            }
        }
        #endregion

        #region Funciones y Procedimientos
        private void funCargarCombos()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = txtCriterio.Text.Trim();
            objparam[2] = 125;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0) new Funciones().funShowJSMessage("Datos Encontrados..!", this);
            ddlAsignarUsuario.DataSource = dt;
            ddlAsignarUsuario.DataTextField = "Descripcion";
            ddlAsignarUsuario.DataValueField = "Codigo";
            ddlAsignarUsuario.DataBind();
        }
        private void funCargarMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[1] = "";
                objparam[2] = 47;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    txtNombres.InnerText = dt.Tables[0].Rows[0][0].ToString();
                    txtApellidos.InnerText = dt.Tables[0].Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ddlAsignarUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            lblNombres.Visible = false;
            lblApellidos.Visible = false;
            txtApellidos.Visible = false;
            txtNombres.Visible = false;
            txtLogin.Visible = false;
            lbllogin.Visible = false;
            lblpassword.Visible = false;
            txtPassword.Visible = false;

            if (ddlAsignarUsuario.SelectedValue == "0")
            {
                lblNombres.Visible = true;
                lblApellidos.Visible = true;
                txtApellidos.Visible = true;
                txtNombres.Visible = true;
                lbllogin.Visible = true;
                lblpassword.Visible = true;
                txtLogin.Visible = true;
                txtPassword.Visible = true;
            }
            if (int.Parse(ddlAsignarUsuario.SelectedValue) > 0)
            {
                //CONSULTAR NOMBRES Y APELLIDOS DEL USUARIO ASIGNADO
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ddlAsignarUsuario.SelectedValue);
                objparam[1] = "";
                objparam[2] = 37;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    txtNombres.InnerText = dt.Tables[0].Rows[0][0].ToString();
                    txtApellidos.InnerText = dt.Tables[0].Rows[0][1].ToString();
                }
                lblNombres.Visible = true;
                lblApellidos.Visible = true;
                txtApellidos.Visible = true;
                txtNombres.Visible = true;
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAsignarUsuario.SelectedValue == "-1")
                {
                    //lblerror.Text = "Seleccione Usuario o Creee uno Nuevo..!";
                    new Funciones().funShowJSMessage("Seleccione Usuario o Creee uno Nuevo..!", this);
                    return;
                }
                if (ddlAsignarUsuario.SelectedValue == "0")
                {
                    if (string.IsNullOrEmpty(txtLogin.Text))
                    {
                        //lblerror.Text = "Ingrese Login para crear usuario..!";
                        new Funciones().funShowJSMessage("Ingrese Login para crear usuario..!", this);
                        return;
                    }
                    if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        //lblerror.Text = "Ingrese password..!";
                        new Funciones().funShowJSMessage("Ingrese password..!", this);
                        return;
                    }
                }
                Array.Resize(ref objparam, 9);
                objparam[0] = ddlAsignarUsuario.SelectedValue == "0" ? 4 : 3;
                objparam[1] = int.Parse(ddlAsignarUsuario.SelectedValue);
                objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[3] = txtNombres.InnerText;
                objparam[4] = txtApellidos.InnerText;
                objparam[5] = txtLogin.Text.Trim();
                objparam[6] = new Funciones().EncriptaMD5(txtPassword.Text);
                objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                objparam[8] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_CrearUsuarioMedico", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmOdontoUsuarioAdmin.aspx?MensajeRetornado='Usuario Asignado con Éxito'", true);
                    else new Funciones().funShowJSMessage(dt.Tables[0].Rows[0][0].ToString(), this);
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmOdontoUsuarioAdmin.aspx", true);
        }
        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            funCargarCombos();
        }
        #endregion
    }
}