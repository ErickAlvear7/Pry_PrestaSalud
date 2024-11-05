using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Departamento_FrmNuevoDepartamento : System.Web.UI.Page
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
            Session["Tipo"] = Request["Tipo"];
            if (Request["Tipo"] == "N")
            {
                Session["Tipo"] = Request["Tipo"].ToString();
                lbltitulo.Text = "Agregar Nuevo departamento";
            }
            else
            {
                Session["CodigDepar"] = Request["Codigo"];
                Label3.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Departamento";
                funCargaMantenimiento(Session["CodigDepar"].ToString());
            }
        }
    }
    #endregion

    #region Procedimientos y Funciones
    protected void funCargaMantenimiento(String strCodigoDepar)
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = int.Parse(strCodigoDepar);
        dt = new Conexion(2, "").funConsultarSqls("sp_DeparEditRead", objparam);
        txtDepartamento.Text = dt.Tables[0].Rows[0][1].ToString();
        chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
        chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
        Session["DepartamentoAnte"] = txtDepartamento.Text;
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtDepartamento.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre del Departamente..!";
            return;
        }
        try
        {
            Array.Resize(ref objparam, 6);
            objparam[0] = Session["Tipo"].ToString() == "N" ? 0 : int.Parse(Session["CodigDepar"].ToString());
            objparam[1] = txtDepartamento.Text.ToUpper();
            objparam[2] = Session["DepartamentoAnte"] == null ? "" : Session["DepartamentoAnte"].ToString();
            objparam[3] = chkEstado.Checked == true ? 1 : 0;
            objparam[4] = int.Parse(Session["usuCodigo"].ToString());
            objparam[5] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_DeparEditUpdate", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Departamento ya existe, por favor cree otro..!"; //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe Departamento Creado, por favor Cree otro');", true);
            else Response.Redirect("FrmDepartamentoAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch(Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmDepartamentoAdmin.aspx");
    }
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
    }
    #endregion
}