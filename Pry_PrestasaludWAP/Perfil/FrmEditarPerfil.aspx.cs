using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Perfil_FrmEditarPerfil : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
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
                lbltitulo.Text = "Editar Perfil";
                Session["CodPerfil"] = Request["perfCodigo"].ToString();
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
        int contar = 0;
        Array.Resize(ref objparam, 1);
        objparam[0] = int.Parse(Session["CodPerfil"].ToString());
        dt = new Conexion(2, "").funConsultarSqls("sp_CargarPerfil", objparam);
        txtPerfil.Text = dt.Tables[0].Rows[0][0].ToString();
        txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
        chkestado.Text = dt.Tables[0].Rows[0][2].ToString();
        chkestado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
        chkCrear.Text = dt.Tables[0].Rows[0][3].ToString();
        chkCrear.Checked = dt.Tables[0].Rows[0][3].ToString() == "Si" ? true : false;
        chkModificar.Text = dt.Tables[0].Rows[0][4].ToString();
        chkModificar.Checked = dt.Tables[0].Rows[0][4].ToString() == "Si" ? true : false;
        chkEliminar.Text = dt.Tables[0].Rows[0][5].ToString();
        chkEliminar.Checked = dt.Tables[0].Rows[0][5].ToString() == "Si" ? true : false;
        Session["NomPerfil"] = txtPerfil.Text;
        Array.Resize(ref objparam, 1);
        objparam[0] = int.Parse(Session["CodPerfil"].ToString());
        dt = new Conexion(2, "").funConsultarSqls("sp_PerfilEditRead", objparam);
        grdvDatos.DataSource = dt;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        Session["grdvDatos"] = grdvDatos.DataSource;
        foreach (GridViewRow row in grdvDatos.Rows)
        {
            chkAgregar = row.FindControl("chkAgregar") as CheckBox;
            if (dt.Tables[0].Rows[contar][0].ToString() == "Activo") chkAgregar.Checked = true;
            else chkAgregar.Checked = false;
            contar++;
        }
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
        try
        {
            String ok = "Ok";
            Array.Resize(ref objparam, 10);
            objparam[0] = int.Parse(Session["CodPerfil"].ToString());
            objparam[1] = txtPerfil.Text.ToUpper();
            objparam[2] = Session["NomPerfil"].ToString();
            objparam[3] = txtDescripcion.Text.ToUpper();
            objparam[4] = chkestado.Checked;
            objparam[5] = chkCrear.Checked;
            objparam[6] = chkModificar.Checked;
            objparam[7] = chkEliminar.Checked;
            objparam[8] = int.Parse(Session["usuCodigo"].ToString());
            objparam[9] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_PerfilUpdate", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El Nombre del Perfil ingresado ya existe, por favor ingrese otro ');", true);
                lblerror.Text = "El Nombre del Perfil ingresado ya existe, por favor ingrese otro..!";
                return;
            }
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["CodPerfil"].ToString());
            objparam[1] = int.Parse(Session["usuCodigo"].ToString());
            foreach (GridViewRow row in grdvDatos.Rows)
            {
                chkAgregar = row.FindControl("chkAgregar") as CheckBox;
                if (chkAgregar.Checked == true)
                {
                    objparam[2] = int.Parse(grdvDatos.DataKeys[row.RowIndex].Values["Codigo"].ToString());
                    dt = new Conexion(2, "").funConsultarSqls("sp_PerfilEditUpdateRow", objparam);
                    if (dt.Tables[0].Rows[0][0].ToString() != "Exito")
                    {
                        ok = "ERR";
                        break;
                    }
                }
            }
            if (ok == "Ok") Response.Redirect("FrmPerfilAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            else throw new Exception("Error al grabar Regitros");
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
    protected void chkestado_CheckedChanged(object sender, EventArgs e)
    {
        chkestado.Text = chkestado.Checked == true ? "Activo" : "Inactivo";
    }
    #endregion
}