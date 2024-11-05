using System;
using System.Data;

namespace Pry_PrestasaludWAP.Prestadora
{
    public partial class FrmNuevaPrestaEspe : System.Web.UI.Page
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            txtPvp.Attributes.Add("onchange", "ValidarDecimales();");
            if (!IsPostBack)
            {
                ViewState["Tipo"] = Request["Tipo"];
                ViewState["CodigoEspePres"] = Request["Codigo"];
                if (Request["Tipo"] == "N")
                {
                    lbltitulo.Text = "Agregar Nueva Especialidad Prestadora";
                }
                else
                {
                    Label11.Visible = true;
                    chkEstado.Visible = true;
                    lbltitulo.Text = "Editar Especialidad Prestadora";
                    funCargaMantenimiento();
                }
            }
            
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = int.Parse(ViewState["CodigoEspePres"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarPrestaEspeEdit", objparam);
                txtEspecialidad.Text = dt.Tables[0].Rows[0][0].ToString();
                txtDetalle.Text = dt.Tables[0].Rows[0][1].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
                txtPvp.Text = dt.Tables[0].Rows[0][3].ToString();
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
            try
            {
                lblerror.Text = "";
                if (string.IsNullOrEmpty(txtEspecialidad.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre Especialidad..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(100);
                Array.Resize(ref objparam, 8);
                objparam[0] = int.Parse(ViewState["CodigoEspePres"].ToString());
                objparam[1] = txtEspecialidad.Text;
                objparam[2] = txtDetalle.Text.ToUpper();
                objparam[3] = chkEstado.Checked == true ? 1 : 0;
                objparam[4] = txtPvp.Text.Trim() == "" ? "0.00" : txtPvp.Text.Trim();
                objparam[5] = "";
                objparam[6] = int.Parse(Session["usuCodigo"].ToString());
                objparam[7] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevaEspePrestadora", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmPrestaEspeAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmPrestaEspeAdmin.aspx", false);
        }
        #endregion
    }
}