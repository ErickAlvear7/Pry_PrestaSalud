using System;
using System.Data;
public partial class Regiones_FrmNuevaCiudad : System.Web.UI.Page
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
            objparam[0] = 3;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargarRegion", objparam);
            ddlProvincia.DataSource = dt;
            ddlProvincia.DataTextField = "Provincia";
            ddlProvincia.DataValueField = "Codigo";
            ddlProvincia.DataBind();
            Session["Tipo"] = Request["Tipo"];
            if (Request["Tipo"] == "N")
            {
                Session["Tipo"] = Request["Tipo"].ToString();
                lbltitulo.Text = "Agregar Nueva ciudad";
            }
            else
            {
                var prov = Request["Provincia"];
                Session["CodigCiudad"] = Request["Codigo"];
                Session["CodigProv"] = Request["Provincia"];
                Session["Ciudad"] = Request["Ciudad"];
                Session["Estado"] = Request["Estado"];
                Session["CiudadAnterior"] = Request["Ciudad"];
                Label3.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Ciudad";
                funCargaMantenimiento();
            }
            //Session["usuCodigo"] = "1";
        }
    }
    #endregion

    #region Procedimientos y Funciones
    protected void funCargaMantenimiento()
    {
        ddlProvincia.SelectedValue = Session["CodigProv"].ToString();
        txtCiudad.Text = Session["Ciudad"].ToString();
        chkEstado.Text = Session["Estado"].ToString();
        chkEstado.Checked = Session["Estado"].ToString() == "Activo" ? true : false;
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlProvincia.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Provincia..!", this);
                return;
            }

            if (string.IsNullOrEmpty(txtCiudad.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Nombre de la Ciudad..!", this);
                return;
            }

            Array.Resize(ref objparam, 8);
            objparam[0] = Session["Tipo"].ToString() == "N" ? 0 : int.Parse(Session["CodigCiudad"].ToString());
            objparam[1] = int.Parse(ddlProvincia.SelectedValue);
            objparam[2] = Session["CodigProv"] == null ? 0 : int.Parse(Session["CodigProv"].ToString());
            objparam[3] = txtCiudad.Text.ToUpper();
            objparam[4] = Session["CiudadAnterior"] == null ? "" : Session["CiudadAnterior"].ToString().ToUpper();
            objparam[5] = chkEstado.Checked == true ? 1 : 0;
            objparam[6] = int.Parse(Session["usuCodigo"].ToString());
            objparam[7] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_NuevaCiudad", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Ya existe ciudad creada, por favor ingrese una nueva..!";//ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ya existe Ciudad Creada, por favor Cree otra');", true);
            else Response.Redirect("FrmCiudadAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch (Exception ex)
        {
            new Funciones().funCrearLogsAuditoria("ExpertWeb_Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", "ERROR", "Frm_Login", "Evento: " + ex.Message);
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmCiudadAdmin.aspx", false);
    }
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
    }
    #endregion
}