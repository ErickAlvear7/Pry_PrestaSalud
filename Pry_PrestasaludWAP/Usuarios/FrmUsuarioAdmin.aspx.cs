using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usuarios_FrmUsuarioAdmin : Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    string strCodigoUsu = "", redirect = "";
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["usuCodigo"] = "1";
        //Session["MachineName"] = "PC";
        //Session["SalirAgenda"] = "SI";
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
            lbltitulo.Text = "Administrar Usuarios";
            FunCargaMantenimiento();
            if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
        }
    }
    #endregion

    #region Funciones y Procedimiento
    protected void FunCargaMantenimiento()
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = 0;
        dt = new Conexion(2, "").funConsultarSqls("sp_UsurioAdminReadAll", objparam);
        grdvDatos.DataSource = dt;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    #endregion

    #region Botones y Eventos
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmNuevoUsuario.aspx?Codigo=0", true);
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        strCodigoUsu = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "Editar_Usuario", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('frmEditarUsuario.aspx?usuCodigo=" + strCodigoUsu + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=500px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        Response.Redirect("FrmNuevoUsuario.aspx?Codigo=" + strCodigoUsu, true);
    }
    protected void imgReset_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        strCodigoUsu = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
        Array.Resize(ref objparam, 7);
        objparam[0] = 0;
        objparam[1] = int.Parse(strCodigoUsu);
        objparam[2] = new Funciones().EncriptaMD5("prestasalud");
        objparam[3] = "";
        objparam[4] = "";
        objparam[5] = 0;
        objparam[6] = 0;
        dt = new Conexion(2, "").funConsultarSqls("sp_ChangePass", objparam);
        redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Contraseña Reseteada con Exito..!");
        Response.Redirect(redirect, true);
    }
    #endregion
}