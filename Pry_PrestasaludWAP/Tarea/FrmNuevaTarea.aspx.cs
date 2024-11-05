using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Tarea_FrmNuevaTarea : System.Web.UI.Page
{
    #region Variables
    DataSet ds = new DataSet();
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
            Session["Tipo"] = Request["Tipo"];
            if (Request["Tipo"] == "N")
            {
                Session["Tipo"] = Request["Tipo"].ToString();
                lbltitulo.Text = "Agregar Nueva Tarea";
            }
            else
            {
                Session["CodigTarea"] = Request["Codigo"];
                Label3.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Tarea";
                funCargaMantenimiento(Session["CodigTarea"].ToString());
            }
        }
    }
    #endregion

    #region Procedimientos y Funciones
    protected void funCargaMantenimiento(String strCodigoTarea)
    {
        Array.Resize(ref objparam, 2);
        objparam[0] = 0;
        objparam[1] = int.Parse(strCodigoTarea);
        ds = new Conexion(2, "").funConsultarSqls("sp_TareaEditRead", objparam);
        txtTarea.Text = ds.Tables[0].Rows[0][1].ToString();
        txtRuta.Text = ds.Tables[0].Rows[0][2].ToString();
        chkEstado.Text = ds.Tables[0].Rows[0][3].ToString();
        chkEstado.Checked = ds.Tables[0].Rows[0][3].ToString() == "Activo" ? true : false;
        Session["TareaAnterior"] = txtTarea.Text;
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtTarea.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre de la Tarea..!";
            return;
        }
        if (string.IsNullOrEmpty(txtRuta.Text.Trim()))
        {
            lblerror.Text = "Ingrese Ruta..!";
            return;
        }
        try
        {
            if (fun.ruta_bien_escrita(txtRuta.Text))
            {
                Array.Resize(ref objparam, 7);
                objparam[0] = Session["Tipo"].ToString() == "N" ? 0 : int.Parse(Session["CodigTarea"].ToString());
                objparam[1] = txtTarea.Text;
                objparam[2] = Session["TareaAnterior"] == null ? "" : Session["TareaAnterior"].ToString();
                objparam[3] = txtRuta.Text;
                objparam[4] = chkEstado.Checked;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                if (Session["Tipo"].ToString() == "N") ds = new Conexion(2, "").funConsultarSqls("sp_NuevaTarea", objparam);
                else ds = new Conexion(2, "").funConsultarSqls("sp_TareaEditUpdate", objparam);
                if (ds.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Tarea ya existe, por favor cree otra..!"; //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe la Tarea Creada, por favor Cree otra');", true);
                else Response.Redirect("FrmTareaAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.opener.location='frmtarAdmin.aspx?mensajeRetornado=Guardado Con éxito" + "';window.close();", true);
                //}
            }
            else lblerror.Text = "Nombre de la página mal escrito (eje: ruta/nombre.aspx)"; //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Nombre de la página mal escrito (eje: nombre.aspx)');", true);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmTareaAdmin.aspx");
    }
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
    }
    #endregion
}