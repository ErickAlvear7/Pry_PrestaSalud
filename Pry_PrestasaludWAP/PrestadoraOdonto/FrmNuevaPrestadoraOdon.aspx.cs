using System;
using System.Data;

namespace Pry_PrestasaludWAP.PrestadoraOdonto
{
    public partial class FrmNuevaPrestadoraOdon : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Funciones fun = new Funciones();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                try
                {
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 4;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargarRegion", objparam);
                    ddlCiudad.DataSource = dt;
                    ddlCiudad.DataTextField = "Ciudad";
                    ddlCiudad.DataValueField = "Codigo";
                    ddlCiudad.DataBind();
                    funCargarCombos();
                    ViewState["Tipo"] = Request["Tipo"];
                    if (Request["Tipo"] == "N")
                    {
                        ViewState["Tipo"] = Request["Tipo"].ToString();
                        lbltitulo.Text = "Agregar Nueva Red/Ondontológica";
                    }
                    else
                    {
                        ViewState["codigopresta"] = Request["Codigo"];
                        Label11.Visible = true;
                        chkEstado.Visible = true;
                        lbltitulo.Text = "Editar Red/Odontológica";
                        funCargaMantenimiento();
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = int.Parse(ViewState["codigopresta"].ToString());
                objparam[1] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarPrestadoraEdit", objparam);
                txtPretadora.Text = dt.Tables[0].Rows[0][0].ToString();
                txtDireccion.Text = dt.Tables[0].Rows[0][1].ToString();
                txtFono1.Text = dt.Tables[0].Rows[0][2].ToString();
                txtFono2.Text = dt.Tables[0].Rows[0][3].ToString();
                txtFono3.Text = dt.Tables[0].Rows[0][4].ToString();
                txtCelular.Text = dt.Tables[0].Rows[0][5].ToString();
                txtFax.Text = dt.Tables[0].Rows[0][6].ToString();
                txtEmail1.Text = dt.Tables[0].Rows[0][7].ToString();
                txtEmail2.Text = dt.Tables[0].Rows[0][8].ToString();
                txtEmail3.Text = dt.Tables[0].Rows[0][9].ToString();
                txtURL.Text = dt.Tables[0].Rows[0][10].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][11].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][11].ToString() == "Activo" ? true : false;
                ddlProvincia.SelectedValue = dt.Tables[0].Rows[0][13].ToString();
                chkEnviar1.Checked = dt.Tables[0].Rows[0][14].ToString() == "Si" ? true : false;
                chkEnviar2.Checked = dt.Tables[0].Rows[0][15].ToString() == "Si" ? true : false;
                chkEnviar3.Checked = dt.Tables[0].Rows[0][16].ToString() == "Si" ? true : false;
                funCascadaCombos(0);
                ddlCiudad.SelectedValue = dt.Tables[0].Rows[0][12].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funCargarCombos()
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = 6;
            ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlProvincia.DataTextField = "Descripcion";
            ddlProvincia.DataValueField = "Codigo";
            ddlProvincia.DataBind();

            Array.Resize(ref objparam, 3);
            objparam[0] = ddlProvincia.SelectedValue;
            objparam[1] = "";
            objparam[2] = 4;
            ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            ddlCiudad.DataTextField = "Descripcion";
            ddlCiudad.DataValueField = "Codigo";
            ddlCiudad.DataBind();
        }

        private void funCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    ddlCiudad.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlProvincia.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 4;
                    ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlCiudad.DataTextField = "Descripcion";
                    ddlCiudad.DataValueField = "Codigo";
                    ddlCiudad.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";
                if (string.IsNullOrEmpty(txtPretadora.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre de la Prestadora..!", this);
                    return;
                }
                if (ddlCiudad.SelectedItem.ToString() == "--Seleccione Ciudad--")
                {
                    new Funciones().funShowJSMessage("Seleccione Ciudad..!", this);
                    return;
                }
                if (new Funciones().email_bien_escrito(txtEmail1.Text.Trim()) == false)
                {
                    new Funciones().funShowJSMessage("Dirección de e-mail incorrecto..!", this);
                    return;
                }
                if (new Funciones().email_bien_escrito(txtEmail2.Text.Trim()) == false)
                {
                    new Funciones().funShowJSMessage("Dirección de e-mail incorrecto..!", this);
                    return;
                }
                if (new Funciones().email_bien_escrito(txtEmail3.Text.Trim()) == false)
                {
                    new Funciones().funShowJSMessage("Dirección de e-mail incorrecto..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(100);
                Array.Resize(ref objparam, 21);
                objparam[0] = ViewState["Tipo"].ToString() == "N" ? 0 : int.Parse(ViewState["codigopresta"].ToString());
                objparam[1] = int.Parse(ddlCiudad.SelectedValue);
                objparam[2] = txtPretadora.Text.Trim().ToUpper();
                objparam[3] = txtDireccion.Text.Trim().ToUpper();
                objparam[4] = txtFono1.Text;
                objparam[5] = txtFono2.Text;
                objparam[6] = txtFono3.Text;
                objparam[7] = txtCelular.Text;
                objparam[8] = txtFax.Text;
                objparam[9] = txtEmail1.Text.Trim().ToLower();
                objparam[10] = chkEnviar1.Checked;
                objparam[11] = txtEmail2.Text.Trim().ToLower();
                objparam[12] = chkEnviar2.Checked;
                objparam[13] = txtEmail3.Text.Trim().ToLower();
                objparam[14] = chkEnviar3.Checked;
                objparam[15] = txtURL.Text;
                objparam[16] = chkEstado.Checked == true ? 1 : 0;
                objparam[17] = int.Parse(Session["usuCodigo"].ToString());
                objparam[18] = Session["MachineName"].ToString();
                objparam[19] = 1;
                objparam[20] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevaPretadora", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Ya existe prestadora creada, ingrese una nueva..!";//ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe Ciudad Creada, por favor Cree otra');", true);
                else Response.Redirect("FrmPrestadoraOdonAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmPrestadoraOdonAdmin.aspx", false);
        }

        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(0);
        }

        protected void chkEnviar1_CheckedChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (chkEnviar1.Checked)
            {
                if (new Funciones().email_bien_escrito(txtEmail1.Text.Trim()) == false || txtEmail1.Text.Trim() == "")
                {
                    lblerror.Text = "Ingrese un mail válido por favor..!";
                    chkEnviar1.Checked = false;
                }
            }
        }

        protected void chkEnviar2_CheckedChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (chkEnviar2.Checked)
            {
                if (new Funciones().email_bien_escrito(txtEmail2.Text.Trim()) == false || txtEmail2.Text.Trim() == "")
                {
                    lblerror.Text = "Ingrese un mail válido por favor..!";
                    chkEnviar2.Checked = false;
                }
            }
        }

        protected void chkEnviar3_CheckedChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (chkEnviar3.Checked)
            {
                if (new Funciones().email_bien_escrito(txtEmail3.Text.Trim()) == false || txtEmail3.Text.Trim() == "")
                {
                    lblerror.Text = "Ingrese un mail válido por favor..!";
                    chkEnviar3.Checked = false;
                }
            }
        }
        #endregion
    }
}