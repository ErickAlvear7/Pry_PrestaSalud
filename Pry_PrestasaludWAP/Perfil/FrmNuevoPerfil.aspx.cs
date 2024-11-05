using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Perfil_FrmNuevoPerfil : System.Web.UI.Page
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
        try
        {
            if (!IsPostBack)
            {
                lbltitulo.Text = "Agregar Nuevo Perfil";
                funCargaMantenimiento();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Procedimientos y Funciones
    protected void funCargaMantenimiento()
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = 0;
        dt = new Conexion(2, "").funConsultarSqls("sp_PerfilRead", objparam);
        grdvDatos.DataSource = dt;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        Session["grdvDatos"] = grdvDatos.DataSource;
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtPerfil.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre del Perfil..!";
            return;
        }
        String strOk = "Ok";
        try
        {
            if(string.IsNullOrEmpty(txtPerfil.Text.Trim()))
            {
                lblerror.Text = "Ingrese Nombre del Perfil..!";
                return;
            }
            CheckBox chkAgregar;
            Array.Resize(ref objparam, 8);
            objparam[0] = txtPerfil.Text.ToUpper();
            objparam[1] = txtDescripcion.Text.ToUpper();
            objparam[2] = 1;
            objparam[3] = chkCrear.Checked;
            objparam[4] = chkModificar.Checked;
            objparam[5] = chkEliminar.Checked;
            objparam[6] = int.Parse(Session["usuCodigo"].ToString());
            objparam[7] = Session["MachineName"].ToString();            
            dt = new Conexion(2, "").funConsultarSqls("sp_PerfilNewMax", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El Nombre del Perfil ingresado ya existe');", true);
                lblerror.Text = "Perfil ya existe, ingrese uno nuevo por favor..!";
                return;
            }
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = int.Parse(dt.Tables[0].Rows[0][0].ToString());
            foreach (GridViewRow row in grdvDatos.Rows)
            {
                chkAgregar = row.FindControl("chkAgregar") as CheckBox;
                if (chkAgregar.Checked == true)
                {
                    //objparam[2] = grdvDatos.Rows[row.RowIndex].Cells[1].Text;
                    objparam[2] = int.Parse(grdvDatos.DataKeys[row.RowIndex].Values["Codigo"].ToString());
                    dt = new Conexion(2, "").funConsultarSqls("sp_NuevoPerfil", objparam);
                    if (dt.Tables[0].Rows[0][0].ToString() != "Exito")
                    {
                        strOk = "ERR";
                        break;
                    }
                }
            }
            if (strOk == "Ok") Response.Redirect("FrmPerfilAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmPerfilAdmin.aspx");
    }

    protected void chkCrear_CheckedChanged(object sender, EventArgs e)
    {
        chkCrear.Text = chkCrear.Checked == true ? "Si" : "No";
    }
    protected void chkModificar_CheckedChanged(object sender, EventArgs e)
    {
        chkModificar.Text = chkModificar.Checked == true ? "Si" : "No";
    }
    protected void chkEliminar_CheckedChanged(object sender, EventArgs e)
    {
        chkEliminar.Text = chkEliminar.Checked == true ? "Si" : "No";
    }
    #endregion

}