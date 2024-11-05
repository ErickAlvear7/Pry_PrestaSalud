using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Usuarios
{
    public partial class FrmChangePassword : System.Web.UI.Page
    {
        #region Variables
        string redirect = "";
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbltitulo.Text = "Administrar Contraseñas";
                if (bool.Parse(Session["CambiarPass"].ToString()) == false)
                {
                    txtContraAnterior.Enabled = false;
                    txtConfirmaContra.Enabled = false;
                    txtNuevaContra.Enabled = false;
                    btnGrabar.Enabled = false;
                    lblerror.Text = "Usuario no tiene permisos suficientes..!";
                }
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtContraAnterior.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Contraseña Anterior..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtNuevaContra.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Contraseña Nueva..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtConfirmaContra.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Confirmar Contraseña..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(300);
                if (new Funciones().funDesencripta(Session["Clave"].ToString()) != txtContraAnterior.Text.Trim())
                {
                    new Funciones().funShowJSMessage("Contraseña anterior incorrecta..!", this);
                    return;
                }
                if (txtNuevaContra.Text.Trim() != txtConfirmaContra.Text.Trim())
                {
                    new Funciones().funShowJSMessage("Contraseñas no Coinciden..!", this);
                    return;
                }
                Array.Resize(ref objparam, 7);
                objparam[0] = 0;
                objparam[1] = int.Parse(Session["usuCodigo"].ToString());
                objparam[2] = new Funciones().EncriptaMD5(txtNuevaContra.Text.Trim());
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = 0;
                objparam[6] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_ChangePass", objparam);
                if (dts.Tables[0].Rows[0][0].ToString() == "OK")
                {
                    redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Contraseña cambiada con Exito..!");
                    Response.Redirect(redirect, true);
                }
                else lblerror.Text = "Error en servidor SQL..!";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}