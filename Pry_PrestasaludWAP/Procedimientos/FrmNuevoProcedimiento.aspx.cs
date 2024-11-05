using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Procedimientos
{
    public partial class FrmNuevoProcedimiento : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            txtCosto.Attributes.Add("onchange", "ValidarDecimales();");
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                try
                {
                    ViewState["codigoprocedimiento"] = Request["Codigo"];
                    funCargarCombos();
                    if (Request["Tipo"] == "N")
                    {
                        lbltitulo.Text = "Agregar Nuevo Procedimiento";
                    }
                    else
                    {
                        Label11.Visible = true;
                        chkEstado.Visible = true;
                        lbltitulo.Text = "Editar Procedimiento";
                        funCargaMantenimiento(int.Parse(ViewState["codigoprocedimiento"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Funciones y Procedimientos
        private void funCargaMantenimiento(int codigo)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = 0;
                objparam[2] = 26;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    txtProcedimiento.Text = dt.Tables[0].Rows[0][0].ToString();
                    txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                    txtCosto.Text = dt.Tables[0].Rows[0][2].ToString().Replace(",",".");
                    ddlAplica.SelectedValue = dt.Tables[0].Rows[0][4].ToString();
                    chkEstado.Text = dt.Tables[0].Rows[0][5].ToString();                    
                    chkEstado.Checked = dt.Tables[0].Rows[0][5].ToString() == "Activo" ? true : false;
                    ddlPorDefetco.SelectedValue = dt.Tables[0].Rows[0][6].ToString();
                    ddlTipo.SelectedValue = dt.Tables[0].Rows[0][7].ToString();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funCargarCombos()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 22;
                ddlAplica.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlAplica.DataTextField = "Descripcion";
                ddlAplica.DataValueField = "Codigo";
                ddlAplica.DataBind();

                objparam[0] = 24;
                ddlTipo.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlTipo.DataTextField = "Descripcion";
                ddlTipo.DataValueField = "Codigo";
                ddlTipo.DataBind();

                objparam[0] = 26;
                ddlPorDefetco.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlPorDefetco.DataTextField = "Descripcion";
                ddlPorDefetco.DataValueField = "Codigo";
                ddlPorDefetco.DataBind();

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            try
            {
                if (string.IsNullOrEmpty(txtProcedimiento.Text.Trim()))
                {
                    lblerror.Text = "Ingrese Nombre del Procedimiento..!";
                    return;
                }

                if (ddlAplica.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Donde Aplica el Procedimiento..!";
                    return;
                }

                if (ddlTipo.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Tipo de Procedimiento..!";
                    return;
                }

                Array.Resize(ref objparam, 12);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["codigoprocedimiento"].ToString());
                objparam[2] = txtProcedimiento.Text.Trim().ToUpper();
                objparam[3] = txtDescripcion.Text.Trim().ToUpper();
                objparam[4] = txtCosto.Text == "" ? "0.00" : Math.Round(Convert.ToDecimal(txtCosto.Text, CultureInfo.InvariantCulture), 2).ToString("0.00").Replace(",", ".");
                objparam[5] = ddlAplica.SelectedValue;
                objparam[6] = ddlTipo.SelectedValue;
                objparam[7] = 0;
                objparam[8] = chkEstado.Checked;
                objparam[9] = ddlPorDefetco.SelectedValue;
                objparam[10] = int.Parse(Session["usuCodigo"].ToString());
                objparam[11] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevoProcedimiento", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Ya Existe Procedimiento Creado..!";//ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe Ciudad Creada, por favor Cree otra');", true);
                else Response.Redirect("FrmProcedimientosAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmProcedimientosAdmin.aspx", true);
        }
        #endregion
    }
}