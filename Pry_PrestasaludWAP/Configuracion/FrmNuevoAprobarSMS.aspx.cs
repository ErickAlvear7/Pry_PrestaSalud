using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Configuracion
{
    public partial class FrmNuevoAprobarSMS : System.Web.UI.Page
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
                    ViewState["CodigoCamp"] = Request["CodigoCamp"];
                    if (int.Parse(ViewState["Codigo"].ToString()) == 0)
                    {
                        lbltitulo.Text = "Nuevo Text SMS";
                    }
                    else
                    {
                        lblEstado.Visible = true;
                        chkEstado.Visible = true;
                        lbltitulo.Text = "Editar Texto SMS";
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
            objparam[0] = 5;
            ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            ddlCliente.DataTextField = "Descripcion";
            ddlCliente.DataValueField = "Codigo";
            ddlCliente.DataBind();
            objparam[0] = 48;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                lstVariableA.Items.Add(dr[0].ToString());
            }
        }

        private void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["Codigo"].ToString());
                objparam[1] = "";
                objparam[2] = 108;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlCliente.SelectedValue = dt.Tables[0].Rows[0][0].ToString();
                txtSMS.Text = dt.Tables[0].Rows[0][1].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
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
            chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void lstVariableA_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSMS.Text += lstVariableA.Text;
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if (ddlCliente.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Cliente..!";
                return;
            }
            if (string.IsNullOrEmpty(txtSMS.Text.Trim()))            
            {
                lblerror.Text = "Ingrese texto SMS..!";
                return;
            }
            Array.Resize(ref objparam, 20);
            objparam[0] = 1;
            objparam[1] = int.Parse(ViewState["Codigo"].ToString());
            objparam[2] = int.Parse(ddlCliente.SelectedValue);
            objparam[3] = "";
            objparam[4] = "";
            objparam[5] = "";
            objparam[6] = "";
            objparam[7] = "";
            objparam[8] = "";
            objparam[9] = "";
            objparam[10] = "";
            objparam[11] = "";
            objparam[12] = chkEstado.Checked;
            objparam[13] = txtSMS.Text.Trim();
            objparam[14] = "";
            objparam[15] = "";
            objparam[16] = 0;
            objparam[17] = 0;
            objparam[18] = Session["usuCodigo"].ToString();
            objparam[19] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_GenerarTextoSMS", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe")
            {
                lblerror.Text = "Txt SMS ingresado para Cliente ya existe..!";
                return;
            }
            else Response.Redirect("FrmSMSAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSMSAdmin.aspx", true);
        }
        #endregion
    }
}