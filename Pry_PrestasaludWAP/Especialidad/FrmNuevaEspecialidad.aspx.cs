using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
public partial class Especialidad_FrmNuevaEspecialidad : System.Web.UI.Page
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
            ViewState["CodigoEspecialidad"] = Request["Codigo"];
            ViewState["Tipo"] = Request["Tipo"];
            if (Request["Tipo"] == "N")
            {
                lbltitulo.Text = "Agregar Nueva Especialidad Odontológica";
            }
            else
            {                
                Label7.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Especialidad Odontológica";
                funCargaMantenimiento(int.Parse(ViewState["CodigoEspecialidad"].ToString()));
            }
        }
    }
    #endregion

    #region Funciones y Procedimientos
    protected void funCargaMantenimiento(int intCodEspe)
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = intCodEspe;
            dt = new Conexion(2, "").funConsultarSqls("sp_EspeEditRead", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtEspecialidad.Text = dt.Tables[0].Rows[0][0].ToString();
                txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
            }
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
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtEspecialidad.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre de la Especialidad..!";
            return;
        }
        try
        {
            Array.Resize(ref objparam, 6);
            objparam[0] = int.Parse(ViewState["CodigoEspecialidad"].ToString()); 
            objparam[1] = txtEspecialidad.Text.ToUpper();
            objparam[2] = txtDescripcion.Text.ToUpper();
            objparam[3] = chkEstado.Checked;
            objparam[4] = int.Parse(Session["usuCodigo"].ToString());
            objparam[5] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_NuevaEspecialidad", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Especialidad ya existe, ingrese una nueva por favor..!";                
            else Response.Redirect("FrmEspecialidadAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch(Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmEspecialidadAdmin.aspx");
    }
    #endregion
}