using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Prestadora
{
    public partial class FrmAsignarEspePrestadora : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        DataTable tbEspeci = new DataTable();
        int maxCodigo = 0, codigo = 0;
        bool lexiste = false;
        DataRow result = null;
        CheckBox chkest = new CheckBox();
        ImageButton imgDel = new ImageButton();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            txtPvp.Attributes.Add("onchange", "ValidarDecimales();");
            txtCosto.Attributes.Add("onchange", "ValidarDecimales();");
            if (!IsPostBack)
            {
                tbEspeci.Columns.Add("Codigo");
                tbEspeci.Columns.Add("CodigoEspe");
                tbEspeci.Columns.Add("Especialidad");
                tbEspeci.Columns.Add("Pvp");
                tbEspeci.Columns.Add("Costo");
                tbEspeci.Columns.Add("Estado");
                ViewState["tblEspeci"] = tbEspeci;
                ViewState["CodigoPrestadora"] = Request["codPrestadora"];
                ViewState["Tipo"] = Request["Tipo"];
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                objparam[1] = "";
                objparam[2] = 6;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "Agregar Especialidad PRESTADORA: " + dt.Tables[0].Rows[0][0].ToString();

                funCargaMantenimiento(int.Parse(ViewState["CodigoPrestadora"].ToString()));
                funLlenarCombos();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Funciones y Procedimientos
        protected void funCargaMantenimiento(int codigoPrestadora)
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = codigoPrestadora;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarEspeciEdit", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    DataTable dtEspeci = dt.Tables[0];
                    ViewState["tblEspeci"] = dtEspeci;
                    grdvDatos.DataSource = dtEspeci;
                    grdvDatos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        private void funLlenarCombos()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 7;
                ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam).Tables[0];
                ddlEspecialidad.DataTextField = "Descripcion";
                ddlEspecialidad.DataValueField = "Codigo";
                ddlEspecialidad.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblerror.Text = "";
                if (ddlEspecialidad.SelectedItem.ToString() == "--Seleccione Especialidad--")
                {
                    new Funciones().funShowJSMessage("Seleccione Especialidad..!", this);
                    return;
                }
                if (ViewState["tblEspeci"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblEspeci"];
                    DataRow result = tblbuscar.Select("CodigoEspe='" + ddlEspecialidad.SelectedValue + "'").FirstOrDefault();
                    if (result != null) lexiste = true;
                    if (tblbuscar.Rows.Count > 0)
                    {
                        maxCodigo = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe ingresada Especialidad..!", this);
                    return;
                }
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblEspeci"];
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["CodigoEspe"] = int.Parse(ddlEspecialidad.SelectedValue.ToString());
                filagre["Especialidad"] = ddlEspecialidad.SelectedItem.ToString();
                filagre["Pvp"] = txtPvp.Text.Trim() == "" ? "0.00" : txtPvp.Text.Trim();
                filagre["Costo"] = txtCosto.Text.Trim() == "" ? "0.00" : txtCosto.Text.Trim();
                filagre["Estado"] = "Activo";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Especialidad";
                ViewState["tblEspeci"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
                if (e.Row.RowIndex >= 0)
                {
                    chkest = (CheckBox)(e.Row.Cells[3].FindControl("chkEstadoDet"));
                    imgDel = (ImageButton)(e.Row.Cells[4].FindControl("imgEliminar"));
                    codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoEspe"].ToString());
                    Array.Resize(ref objparam, 7);
                    objparam[0] = 1;
                    objparam[1] = 0;
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                    objparam[6] = codigo;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ChangePass", objparam);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        chkest.Checked = dt.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        imgDel.Enabled = false;
                        imgDel.ImageUrl = "~/Botones/eliminaroff.jpg";
                    }
                    else
                    {
                        chkest.Enabled = false;
                        chkest.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEstadoDet_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                CheckBox chkest = new CheckBox();
                chkest = (CheckBox)(gvRow.Cells[3].FindControl("chkEstadoDet"));
                tbEspeci = (DataTable)ViewState["tblEspeci"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                result = tbEspeci.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
                tbEspeci.AcceptChanges();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdvDatos.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Agregue Especialidades..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(100);
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                objparam[1] = int.Parse(Session["usuCodigo"].ToString());
                objparam[2] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertarPrestadoraEspeci(objparam, (DataTable)ViewState["tblEspeci"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmAsignarEspePrestadora.aspx?MensajeRetornado='Guardado con Éxito'" + "&codPrestadora=" + ViewState["CodigoPrestadora"].ToString(), false);
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updOpciones, updOpciones.GetType(), "Cerrar", "CloseFrame();", true);
        }
        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ddlEspecialidad.SelectedValue);
            objparam[1] = "";
            objparam[2] = 123;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0) txtPvp.Text = dt.Tables[0].Rows[0]["Pvp"].ToString();
        }

        protected void imgSeleec_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in grdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                tbEspeci = (DataTable)ViewState["tblEspeci"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                result = tbEspeci.Select("Codigo='" + codigo + "'").FirstOrDefault();
                ddlEspecialidad.SelectedValue = result["CodigoEspe"].ToString();
                ViewState["CodigoEspecialidad"] = result["CodigoEspe"].ToString();
                ViewState["EstadoEspe"] = result["Estado"].ToString();
                txtPvp.Text = result["Pvp"].ToString() == "" ? "0.00" : result["Pvp"].ToString();
                txtCosto.Text = result["Costo"].ToString() == "" ? "0.00" : result["Costo"].ToString();
                imgAgregar.Enabled = false;
                imgModificar.Enabled = true;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                tbEspeci = (DataTable)ViewState["tblEspeci"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                result = tbEspeci.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                tbEspeci.AcceptChanges();
                grdvDatos.DataSource = tbEspeci;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblerror.Text = "";
                if (ddlEspecialidad.SelectedItem.ToString() == "--Seleccione Especialidad--")
                {
                    new Funciones().funShowJSMessage("Seleccione Especialidad..!", this);
                    return;
                }
                if (ViewState["tblEspeci"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblEspeci"];
                    if (ddlEspecialidad.SelectedValue != ViewState["CodigoEspecialidad"].ToString())
                    {
                        result = tblbuscar.Select("CodigoEspe='" + ddlEspecialidad.SelectedValue + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe ingresada Especialidad..!", this);
                    return;
                }
                tbEspeci = (DataTable)ViewState["tblEspeci"];
                result = tbEspeci.Select("CodigoEspe='" + ViewState["CodigoEspecialidad"].ToString() + "'").FirstOrDefault();
                result["CodigoEspe"] = ddlEspecialidad.SelectedValue;
                result["Especialidad"] = ddlEspecialidad.SelectedItem.ToString();
                result["Pvp"] = txtPvp.Text.Trim() == "" ? "0.00" : txtPvp.Text.Trim();
                result["Costo"] = txtCosto.Text.Trim() == "" ? "0.00" : txtCosto.Text.Trim();
                result["Estado"] = ViewState["EstadoEspe"].ToString();
                tbEspeci.AcceptChanges();
                grdvDatos.DataSource = tbEspeci;
                grdvDatos.DataBind();
                imgAgregar.Enabled = true;
                imgModificar.Enabled = false;
                ddlEspecialidad.SelectedValue = "0";
                txtPvp.Text = "0.00";
                txtCosto.Text = "0.00";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}