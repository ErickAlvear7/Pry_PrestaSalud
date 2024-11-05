using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
public partial class Menu_FrmNuevoMenu : System.Web.UI.Page
{
    #region Variables
    DataSet ds = new DataSet();
    Object[] objparam = new Object[1];
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");
        if (!IsPostBack)
        {
            lbltitulo.Text = "Agregar Nuevo Menú";
            lblMenuPadre.Visible = false;
            funCargaMantenimiento();
            funCargarCombos();
        }
    }
    #endregion

    #region Funciones y Procedimiento
    protected void funCargaMantenimiento()
    {
        objparam[0] = 0;
        ds = new Conexion(2, "").funConsultarSqls("sp_MenuTarea", objparam);
        grdvDatos.DataSource = ds;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        Session["grdvDatos"] = grdvDatos.DataSource;
    }

    protected void funCargarCombos()
    {
        Array.Resize(ref objparam, 3);
        objparam[0] = 0;
        objparam[1] = "";
        objparam[2] = 18;
        ddlMenuPadre.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        ddlMenuPadre.DataTextField = "Descripcion";
        ddlMenuPadre.DataValueField = "Codigo";
        ddlMenuPadre.DataBind();
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombreMenu.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre del Menú..!";
            return;
        }
        if (ddlMenuPadre.SelectedValue == "0")
        {
            if (string.IsNullOrEmpty(txtNombreMenuPadre.Text.Trim()))
            {
                lblerror.Text = "Ingrese Nombre del Menú Padre..!";
                return;
            }
        }
        try
        {
            String strmenuCodigo = "", strOk = "Ok";
            CheckBox chkAgregar;
            Array.Resize(ref objparam, 5);
            objparam[0] = int.Parse(ddlMenuPadre.SelectedValue);
            objparam[1] = txtNombreMenuPadre.Text.Trim().ToUpper();
            objparam[2] = txtNombreMenu.Text.Trim();
            objparam[3] = int.Parse(Session["usuCodigo"].ToString());
            objparam[4] = Session["MachineName"].ToString();
            ds = new Conexion(2, "").funConsultarSqls("sp_MenuNewMax", objparam);
            strmenuCodigo = ds.Tables[0].Rows[0][0].ToString();
            if (strmenuCodigo == "Existe")
            {
                lblerror.Text = "Menú ya existe, por favor cree otro..!";
                return;
            }
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = int.Parse(strmenuCodigo);
            foreach (GridViewRow row in grdvDatos.Rows)
            {
                chkAgregar = row.FindControl("chkAgregar") as CheckBox;
                if (chkAgregar.Checked == true)
                {
                    objparam[2] = int.Parse(grdvDatos.Rows[row.RowIndex].Cells[0].Text);
                    ds = new Conexion(2, "").funConsultarSqls("sp_NuevoMenu", objparam);
                    if (ds.Tables[0].Rows[0][0].ToString() != "Exito")
                    {
                        strOk = "ERR";
                        break;
                    }
                }
            }
            if (strOk == "Ok") Response.Redirect("FrmMenuAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            else lblerror.Text = "Hubo un error en la inserción del menú..!";
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            StackTrace st = new StackTrace(ex, true);
            StackFrame frame = st.GetFrame(0);
            new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), frame.GetMethod().Name, ex.ToString(), frame.GetFileLineNumber());
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmMenuAdmin.aspx");
    }
    protected void ddlMenuPadre_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMenuPadre.Visible = false;
        txtNombreMenuPadre.Visible = false;
        if (ddlMenuPadre.SelectedValue == "0")
        {
            lblMenuPadre.Visible = true;
            txtNombreMenuPadre.Visible = true;
        }

    }
    #endregion

}