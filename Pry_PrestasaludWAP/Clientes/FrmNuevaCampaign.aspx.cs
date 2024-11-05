using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Clientes
{
    public partial class FrmNuevaCampaign : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtb = new DataTable();
        Object[] objparam = new Object[1];
        DataTable tbProducto = new DataTable();
        int maxCodigo = 0, codigo = 0;
        string gerencial = "", response = "";
        CheckBox chkgeren = new CheckBox();
        DataRow resultado;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            txtCosto.Attributes.Add("onchange", "ValidarDecimales();");
            if (!IsPostBack)
            {
                tbProducto.Columns.Add("Producto");
                tbProducto.Columns.Add("Costo");
                tbProducto.Columns.Add("Estado");
                tbProducto.Columns.Add("Codigo");
                tbProducto.Columns.Add("Detalle");
                tbProducto.Columns.Add("Grupo");
                tbProducto.Columns.Add("CodigoGrupo");
                tbProducto.Columns.Add("Tipo");
                tbProducto.Columns.Add("Gerencial");
                ViewState["tblProducto"] = tbProducto;
                ViewState["Tipo"] = Request["Tipo"];
                ViewState["CodigoCliente"] = Request["Codigo"];
                funCargarCombos();
                if (ViewState["Tipo"].ToString() == "N")
                {
                    lbltitulo.Text = "Agregar Nuevo Cliente/Producto";
                }
                else
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["CodigoCliente"].ToString());
                    objparam[1] = "";
                    objparam[2] = 83;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    txtCliente.Text = dt.Tables[0].Rows[0][0].ToString();
                    txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                    Label3.Visible = true;
                    chkEstadoCliente.Visible = true;
                    chkEstadoCliente.Text = dt.Tables[0].Rows[0][2].ToString();
                    chkEstadoCliente.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
                    txtCabecera.Text = dt.Tables[0].Rows[0][3].ToString();
                    txtPie.Text = dt.Tables[0].Rows[0][4].ToString();
                    ddlTipoCli.SelectedValue = dt.Tables[0].Rows[0][5].ToString();
                    lbltitulo.Text = "Editar Cliente/Producto";
                    funCargaMantenimiento(int.Parse(ViewState["CodigoCliente"].ToString()));
                }
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimientos
        protected void funCargaMantenimiento(int intCodCliente)
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = intCodCliente;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarProducto", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["tblProducto"] = dt.Tables[0];
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void funCargarCombos()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 34;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlGrupoProducto.DataSource = dt;
                ddlGrupoProducto.DataTextField = "Descripcion";
                ddlGrupoProducto.DataValueField = "Codigo";
                ddlGrupoProducto.DataBind();
            }
            catch(Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void funConsultarEventos()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ddlGrupoProducto.SelectedValue);
                objparam[1] = "Mx";
                objparam[2] = 127;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    txtAsisAnual.Text = dt.Tables[0].Rows[0]["AsisAnual"].ToString();
                    txtAsisMensual.Text = dt.Tables[0].Rows[0]["AsisMes"].ToString();
                    if (dt.Tables[0].Rows[0]["TipoFecha"].ToString() == "FC") chkFechaCobertura.Checked = true;
                    if (dt.Tables[0].Rows[0]["TipoFecha"].ToString() == "FS") chkFechaSistema.Checked = true;
                }
                else
                {
                    txtAsisMensual.Text = "1";
                    txtAsisAnual.Text = "1";
                    chkFechaCobertura.Checked = false;
                    chkFechaSistema.Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void chkEstadoCliente_CheckedChanged(object sender, EventArgs e)
        {
            chkEstadoCliente.Text = chkEstadoCliente.Checked == true ? "Activo" : "Inactivo";
        }

        protected void chkEstadoProducto_CheckedChanged(object sender, EventArgs e)
        {
            chkEstadoProducto.Text = chkEstadoProducto.Checked == true ? "Activo" : "Inactivo";
        }
        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            bool lexiste = false;

            lblerror.Text = "";
            if (txtProducto.Text == "")
            {
                new Funciones().funShowJSMessage("Ingrese nombre del Producto..!", this);
                return;
            }
            if (ddlGrupoProducto.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Grupo del Producto..!", this);
                return;
            }

            try
            {
                if (ViewState["tblProducto"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblProducto"];
                    if (tblbuscar.Rows.Count > 0)
                    {
                        maxCodigo = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    }
                    else maxCodigo = 0;
                    DataRow result = tblbuscar.Select("Producto='" + txtProducto.Text.ToUpper() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "Codigo";
                    if (result != null) lexiste = true;
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe definido un Producto..!", this);
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProducto"];
                DataRow filagre = tblagre.NewRow();
                filagre["Producto"] = txtProducto.Text.ToUpper();
                filagre["Costo"] = txtCosto.Text == "" ? "0.00" : txtCosto.Text;
                filagre["Estado"] = "Activo";
                filagre["Codigo"] = maxCodigo + 1;
                filagre["Detalle"] = txtDetalle.Text.ToUpper();
                filagre["Grupo"] = ddlGrupoProducto.SelectedItem.ToString();
                filagre["CodigoGrupo"] = ddlGrupoProducto.SelectedValue;
                filagre["Tipo"] = ddlTipoPro.SelectedValue;
                filagre["Gerencial"] = "SI";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Producto";
                ViewState["tblProducto"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                
                txtProducto.Text = "";
                txtCosto.Text = "";
                txtDetalle.Text = "";
                ddlGrupoProducto.SelectedIndex = 0;
                ddlTipoPro.SelectedIndex = 0;
                imgCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCampaign", ex.ToString(), frame.GetFileLineNumber());
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool lexiste = false;
                lblerror.Text = "";
                if (txtProducto.Text == "")
                {
                    new Funciones().funShowJSMessage("Ingrese nombre del Producto..!", this);
                    return;
                }
                if (ddlGrupoProducto.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Grupo del Producto..!", this);
                    return;
                }

                if (ViewState["tblProducto"] != null)
                    {
                    DataTable tblbuscar = (DataTable)ViewState["tblProducto"];
                    if (ViewState["ProductoAnterior"].ToString() != txtProducto.Text.ToUpper())
                    {
                        DataRow[] result = tblbuscar.Select("Producto='" + txtProducto.Text.ToUpper() + "'");
                        lexiste = result.Length == 0 ? false : true;
                    }
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe definido un Producto..!", this);
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProducto"];
                tblagre.Rows.RemoveAt(int.Parse(ViewState["index"].ToString()));
                DataRow filagre = tblagre.NewRow();
                filagre["Producto"] = txtProducto.Text.ToUpper();
                filagre["Costo"] = txtCosto.Text == "" ? "0.00" : txtCosto.Text;                
                filagre["Estado"] = chkEstadoProducto.Checked ? "Activo" : "Inactivo";
                filagre["Codigo"] = ViewState["CodigoProducto"].ToString();
                filagre["Detalle"] = txtDetalle.Text.ToUpper();
                filagre["Grupo"] = ddlGrupoProducto.SelectedItem.ToString();
                filagre["CodigoGrupo"] = ddlGrupoProducto.SelectedValue;
                filagre["Tipo"] = ddlTipoPro.SelectedValue;
                filagre["Gerencial"] = ViewState["Gerencial"].ToString();
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Producto";
                ViewState["tblProducto"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                txtProducto.Text = "";
                txtDetalle.Text = "";
                txtCosto.Text = "";
                chkEstadoProducto.Checked = true;
                chkEstadoProducto.Text = "Activo";
                imgModificar.Enabled = false;
                imgAgregar.Enabled = true;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                ViewState["index"] = intIndex;
                ViewState["CodigoProducto"] = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtb = (DataTable)ViewState["tblProducto"];
                resultado = dtb.Select("Codigo='" + ViewState["CodigoProducto"].ToString() + "'").FirstOrDefault();
                ddlGrupoProducto.SelectedValue = resultado["CodigoGrupo"].ToString();
                ddlTipoCli.SelectedValue = resultado["Tipo"].ToString();
                txtProducto.Text = resultado["Producto"].ToString();
                ViewState["ProductoAnterior"] = txtProducto.Text;
                txtCosto.Text = resultado["Costo"].ToString();
                txtDetalle.Text = resultado["Detalle"].ToString();
                chkEstadoProducto.Text = resultado["Estado"].ToString();
                chkEstadoProducto.Checked = resultado["Estado"].ToString() == "Activo" ? true : false;
                ViewState["Gerencial"] = resultado["Gerencial"].ToString();
                ddlTipoPro.SelectedValue = resultado["Tipo"].ToString();
                funConsultarEventos();
                Label6.Visible = true;
                chkEstadoProducto.Visible = true;
                imgModificar.Enabled = true;
                imgAgregar.Enabled = false;
                imgCancelar.Enabled = true;
            }
            catch(Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            txtProducto.Text = "";
            txtCosto.Text = "";
            txtDetalle.Text = "";
            imgAgregar.Enabled = true;
            imgCancelar.Enabled = false;
            imgModificar.Enabled = false;
            funCargaMantenimiento(int.Parse(ViewState["CodigoCliente"].ToString()));
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCliente.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre del Cliente..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtAsisMensual.Text.Trim()) || txtAsisMensual.Text.Trim() == "0")
                {
                    new Funciones().funShowJSMessage("Ingrese Valor de Asistencias por Mes..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtAsisAnual.Text.Trim()) || txtAsisAnual.Text.Trim() == "0")
                {
                    new Funciones().funShowJSMessage("Ingrese Valor de Asistencia por Año..!", this);
                    return;
                }
                if (!chkFechaCobertura.Checked && !chkFechaSistema.Checked)
                {
                    new Funciones().funShowJSMessage("Seleccione Fecha Cobertura o Fecha Sistema..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(300);
                Array.Resize(ref objparam, 21);
                objparam[0] = int.Parse(ViewState["CodigoCliente"].ToString());
                objparam[1] = txtCliente.Text.ToUpper();
                objparam[2] = txtDescripcion.Text.ToUpper();
                objparam[3] = txtCabecera.Text;
                objparam[4] = txtPie.Text;
                objparam[5] = chkEstadoCliente.Checked;
                objparam[6] = int.Parse(ddlGrupoProducto.SelectedValue);
                objparam[7] = txtAsisAnual.Text.Trim();
                objparam[8] = txtAsisMensual.Text.Trim();
                objparam[9] = chkFechaCobertura.Checked ? "FC" : "FS";
                objparam[10] = "";
                objparam[11] = "";
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = 0;
                objparam[15] = 0;
                objparam[16] = 0;
                objparam[17] = 0;
                objparam[18] = int.Parse(Session["usuCodigo"].ToString());
                objparam[19] = Session["MachineName"].ToString();
                objparam[20] = ddlTipoCli.SelectedValue;

                dt = new Conexion(2, "").FunInsertarClienteProducto(objparam, (DataTable)ViewState["tblProducto"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    if (ViewState["Tipo"].ToString() == "N") Response.Redirect("FrmCampaignAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
                    else
                    {
                        response = string.Format("{0}?Tipo={1}&Codigo={2}&MensajeRetornado={3}", Request.Url.AbsolutePath, "E", ViewState["CodigoCliente"].ToString(), "Guardado con Éxito");
                        Response.Redirect(response, true);
                    }
                    
                }
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCampaignAdmin.aspx", true);
        }
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblest = new Label();
            CheckBox chkest = new CheckBox();
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkest = (CheckBox)(e.Row.Cells[4].FindControl("chkGerencial"));
                    gerencial = grdvDatos.DataKeys[e.Row.RowIndex].Values["Gerencial"].ToString();
                    if (gerencial == "SI") chkest.Checked = true;
                    else chkest.Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkFechaCobertura_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaSistema.Checked = false;
        }

        protected void chkFechaSistema_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaCobertura.Checked = false;
        }

        protected void chkGerencial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkgeren = (CheckBox)(gvRow.Cells[4].FindControl("chkGerencial"));
                tbProducto = (DataTable)ViewState["tblProducto"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                DataRow[] result = tbProducto.Select("Codigo=" + codigo);
                result[0]["Gerencial"] = chkgeren.Checked ? "SI" : "NO";
                tbProducto.AcceptChanges();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void ddlGrupoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            funConsultarEventos();
        }
        #endregion
    }
}