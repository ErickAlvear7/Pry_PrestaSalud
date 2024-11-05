using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Configuracion
{
    public partial class FrmSMSAdmin : System.Web.UI.Page
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
                if (Session["SalirAgenda"].ToString() == "NO")
                {
                    if (Session["TipoCita"].ToString() == "CitaMedica")
                        Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                    if (Session["TipoCita"].ToString() == "CitaOdontologica")
                        Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                    return;
                }
                lbltitulo.Text = "Administrar Configurar-SMS";
                funCargaMantenimiento();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else
            {
                grdvDatos.DataSource = ViewState["grdvDatos"];
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            Array.Resize(ref objparam, 20);
            objparam[0] = 0;
            objparam[1] = 0;
            objparam[2] = 0;
            objparam[3] = "";
            objparam[4] = "";
            objparam[5] = "";
            objparam[6] = "";
            objparam[7] = "";
            objparam[8] = "";
            objparam[9] = "";
            objparam[10] = "";
            objparam[11] = "";
            objparam[12] = 0;
            objparam[13] = "";
            objparam[14] = "";
            objparam[15] = "";
            objparam[16] = 0;
            objparam[17] = 0;
            objparam[18] = Session["usuCodigo"].ToString();
            objparam[19] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_GenerarTextoSMS", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                grdvDatos.UseAccessibleHeader = true;
                grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            ViewState["grdvDatos"] = grdvDatos.DataSource;
        }
        #endregion

        #region Botones y Eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNuevoAprobarSMS.aspx?Codigo=0" + "&CodigoCamp=0");
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            string strCodigoSMS = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            string strCodigoCam = grdvDatos.DataKeys[intIndex].Values["CodigoCamp"].ToString();
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "Editar_Usuario", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('frmEditarUsuario.aspx?usuCodigo=" + strCodigoUsu + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=500px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            Response.Redirect("FrmNuevoAprobarSMS.aspx?Codigo=" + strCodigoSMS + "&CodigoCamp=" + strCodigoCam);
        }
        #endregion
    }
}