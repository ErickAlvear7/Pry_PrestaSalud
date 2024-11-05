using System;
using System.Data;

public partial class Regiones_FrmNuevaProvincia : System.Web.UI.Page
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
            Array.Resize(ref objparam, 1);
            objparam[0] = 0;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargarRegion", objparam);
            ddlRegion.DataSource = dt;
            ddlRegion.DataTextField = "Region";
            ddlRegion.DataValueField = "Codigo";
            ddlRegion.DataBind();
            Session["Tipo"] = Request["Tipo"];
            if (Request["Tipo"] == "N")
            {
                Session["Tipo"] = Request["Tipo"].ToString();
                lbltitulo.Text = "Agregar Nueva Provincia";
            }
            else
            {

                Session["CodigProvi"] = Request["Codigo"];
                Session["CodRegion"] = Request["Region"];
                Session["CodMarca"] = Request["CodMarca"];
                Session["Estado"] = Request["Estado"];
                Session["ProvAnterior"] = Request["Provincia"];
                Label3.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Provincia";
                funCargaMantenimiento();
            }
            Session["usuCodigo"] = "1";
        }
    }
    #endregion

    #region Procedimientos y Funciones
    protected void funCargaMantenimiento()
    {
        txtProvincia.Text = Session["ProvAnterior"].ToString();
        ddlRegion.SelectedValue = Session["CodRegion"].ToString();
        txtCodMarca.Text = Session["CodMarca"].ToString();
        chkEstado.Text = Session["Estado"].ToString();
        chkEstado.Checked = Session["Estado"].ToString() == "Activo" ? true : false;
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRegion.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Región..!", this);
                return;
            }
            if (string.IsNullOrEmpty(txtProvincia.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Nombre de la Provincia..!", this);
                return;
            }
            Array.Resize(ref objparam, 10);
            objparam[0] = Session["Tipo"].ToString() == "N" ? 0 : int.Parse(Session["CodigProvi"].ToString());
            objparam[1] = 1;
            objparam[2] = int.Parse(ddlRegion.SelectedValue);
            objparam[3] = Session["CodRegion"] == null ? int.Parse(ddlRegion.SelectedValue) : int.Parse(Session["CodRegion"].ToString());
            objparam[4] = txtProvincia.Text.ToUpper();
            objparam[5] = Session["ProvAnterior"] == null ? "" : Session["ProvAnterior"].ToString().ToUpper();            
            objparam[6] = txtCodMarca.Text;
            objparam[7] = chkEstado.Checked;
            objparam[8] = int.Parse(Session["usuCodigo"].ToString());
            objparam[9] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_NuevaProvincia", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Ya existe provincia creada, por favor ingrese una nueva..!";//ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe la Provincia Creada, por favor Cree otra');", true);
            else Response.Redirect("FrmProvinciaAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch(Exception ex)
        {
            new Funciones().funCrearLogsAuditoria("ExpertWeb_Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", "ERROR", "Frm_Login", "Evento: " + ex.Message);
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmProvinciaAdmin.aspx", false);
    }
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
    }
    #endregion
}