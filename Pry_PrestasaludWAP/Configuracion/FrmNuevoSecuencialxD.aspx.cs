using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Configuracion
{
    public partial class FrmNuevoSecuencialxD : System.Web.UI.Page
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
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                if (!IsPostBack)
                {
                    funCargarCombos();
                    ViewState["Codigo"] = Request["Codigo"];
                    if (int.Parse(ViewState["Codigo"].ToString()) == 0) lbltitulo.Text = "Nuevo Secuencial";
                    else
                    {
                        ddlProducto.Enabled = false;
                        lbltitulo.Text = "Editar Secuencial";
                        funCargaMantenimiento();
                    }
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funCargarCombos()
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = 45;
            ddlProducto.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlProducto.DataTextField = "Descripcion";
            ddlProducto.DataValueField = "Codigo";
            ddlProducto.DataBind();
        }
        private void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["Codigo"].ToString());
                objparam[1] = "";
                objparam[2] = 104;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlProducto.SelectedValue = dt.Tables[0].Rows[0][0].ToString();
                txtSecuencial.Text = dt.Tables[0].Rows[0][1].ToString();
                ddlMedicamento.SelectedValue = dt.Tables[0].Rows[0][2].ToString();
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
                if (ddlProducto.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Producto..!";
                    return;
                }
                if (string.IsNullOrEmpty(txtSecuencial.Text.Trim()))
                {
                    lblerror.Text = "Ingrese valor del secuencial..!";
                    return;
                }
                if (ddlMedicamento.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Medicamento (S/N)..!";
                    return;
                }
                Array.Resize(ref objparam, 11);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["Codigo"].ToString());
                objparam[2] = int.Parse(ddlProducto.SelectedValue);
                objparam[3] = int.Parse(txtSecuencial.Text);
                objparam[4] = ddlMedicamento.SelectedValue;
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = 0;
                objparam[8] = 0;
                objparam[9] = int.Parse(Session["usuCodigo"].ToString());
                objparam[10] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevoSecuencialxDxO", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmSecuencialxDAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch(Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSecuencialxDAdmin.aspx", true);
        }
        #endregion
    }
}