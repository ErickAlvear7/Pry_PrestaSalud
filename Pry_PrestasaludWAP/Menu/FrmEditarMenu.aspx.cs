using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Menu_FrmEditarMenu : System.Web.UI.Page
{
    #region Variables
    DataSet ds = new DataSet();
    Object[] objparam = new Object[1];
    ImageButton imgSubir = new ImageButton();
    ImageButton imgBajar = new ImageButton();
    CheckBox chkAgregar;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");
        try
        {
            if (!IsPostBack)
            {
                lbltitulo.Text = "Editar Menú";                
                ViewState["menuCodigo"] = Request["codigo"].ToString();
                funCargarCombos();
                funCargaMantenimiento(int.Parse(ViewState["menuCodigo"].ToString()));
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Procedimientos y Funciones
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
    protected void funCargaMantenimiento(int strCodigoMenu)
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = strCodigoMenu;
        ds = new Conexion(2, "").funConsultarSqls("sp_CargMenu", objparam);
        txtNombreMenu.Text = ds.Tables[0].Rows[0][0].ToString();
        chkEstado.Text = ds.Tables[0].Rows[0][1].ToString() == "True" ? "Activo" : "Inactivo";
        chkEstado.Checked = ds.Tables[0].Rows[0][1].ToString() == "True" ? true : false;
        ddlMenuPadre.SelectedValue = ds.Tables[0].Rows[0][2].ToString();
        //Session["nommenu"] = txtNombreMenu.Text;

        Int16 fila = 0, contar = 0;
        Array.Resize(ref objparam, 2);
        objparam[0] = strCodigoMenu;
        objparam[1] = int.Parse(Session["usuPerfil"].ToString());
        ds = new Conexion(2, "").funConsultarSqls("sp_MenuEditRead", objparam);
        grdvDatos.DataSource = ds;
        grdvDatos.DataBind();
        Session["grdvDatos"] = grdvDatos.DataSource;

        imgSubir = (ImageButton)grdvDatos.Rows[0].Cells[6].FindControl("imgSubirNivel");
        imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
        imgSubir.Enabled = false;
        foreach (GridViewRow row in grdvDatos.Rows)
        {
            imgSubir = row.FindControl("imgSubirNivel") as ImageButton;
            imgBajar = row.FindControl("imgBajarNivel") as ImageButton;
            chkAgregar = row.FindControl("chkAgregar") as CheckBox;
            if (ds.Tables[0].Rows[contar][0].ToString() == "Check") chkAgregar.Checked = true;
            else chkAgregar.Checked = false;
            if (chkAgregar.Checked == false)
            {
                imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
                imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
                imgSubir.Enabled = false;
                imgBajar.Enabled = false;
            }
            else fila = Convert.ToInt16(row.RowIndex);
            contar++;
        }
        imgBajar = (ImageButton)grdvDatos.Rows[fila].FindControl("imgBajarNivel");
        imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
        imgBajar.Enabled = false;
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
        String strOk = "OK";
        try
        {
            Array.Resize(ref objparam, 7);
            objparam[0] = int.Parse(ViewState["menuCodigo"].ToString());
            objparam[1] = txtNombreMenu.Text;
            objparam[2] = int.Parse(ddlMenuPadre.SelectedValue);
            objparam[3] = txtNombreMenuPadre.Text.Trim().ToUpper();
            objparam[4] = chkEstado.Checked;
            objparam[5] = Session["usuCodigo"];
            objparam[6] = Session["MachineName"].ToString();
            ds = new Conexion(2, "").funConsultarSqls("sp_MenuEditDeleteRows", objparam);
            if (ds.Tables[0].Rows[0][0].ToString() != "")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Nombre de Menú ya existe..!');", true);
                lblerror.Text = ds.Tables[0].Rows[0][0].ToString();
                return;
            }

            Array.Resize(ref objparam, 5);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = int.Parse(ViewState["menuCodigo"].ToString());

            foreach (GridViewRow row in grdvDatos.Rows)
            {
                chkAgregar = row.FindControl("chkAgregar") as CheckBox;
                objparam[2] = chkAgregar.Checked == true ? "S" : "N";
                objparam[3] = grdvDatos.DataKeys[row.RowIndex].Values["CodigoTarea"];
                objparam[4] = grdvDatos.Rows[row.RowIndex].Cells[3].Text;
                ds = new Conexion(2, "").funConsultarSqls("sp_MenuEditUpdate", objparam);
                if (ds.Tables[0].Rows[0][0].ToString().Substring(0, 2) != "OK")
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Hubo un error en la asignación del menú..!');", true);
                    lblerror.Text = "Hubo un error en la asignación del menú..!";
                    strOk = "ERR";
                    break;
                }
                else strOk = "OK";
            }
            if (strOk == "OK") Response.Redirect("FrmMenuAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmMenuAdmin.aspx");
    }
    protected void imgSubirNivel_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;

        Array.Resize(ref objparam, 3);
        objparam[0] = int.Parse(Session["menuCodigo"].ToString());
        objparam[1] = int.Parse(grdvDatos.DataKeys[intIndex].Values["CodigoTarea"].ToString());
        objparam[2] = 0;
        ds = new Conexion(2, "").funConsultarSqls("sp_CamOrdenMenTarea", objparam);
        funCargaMantenimiento(int.Parse(Session["menuCodigo"].ToString()));
    }
    protected void imgBajarNivel_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;

        //var strCodTarea = grdvDatos.DataKeys[intIndex].Values["CodigoTarea"];
        Array.Resize(ref objparam, 3);
        objparam[0] = int.Parse(Session["menuCodigo"].ToString());
        objparam[1] = int.Parse(grdvDatos.DataKeys[intIndex].Values["CodigoTarea"].ToString());
        objparam[2] = 1;
        ds = new Conexion(2, "").funConsultarSqls("sp_CamOrdenMenTarea", objparam);
        funCargaMantenimiento(int.Parse(Session["menuCodigo"].ToString()));
    }
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
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