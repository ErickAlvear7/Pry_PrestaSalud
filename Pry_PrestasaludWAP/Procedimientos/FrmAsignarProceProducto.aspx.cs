using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Procedimientos
{
    public partial class FrmAsignarProceProducto : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        DataTable tbProcedimiento = new DataTable();
        int count = 0, maxCodigo = 0;
        DataRow resultado;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                ViewState["codigoProducto"] = 0;
                tbProcedimiento.Columns.Add("Codigo");
                tbProcedimiento.Columns.Add("CodigoProducto");
                tbProcedimiento.Columns.Add("CodigoProcedimiento");
                tbProcedimiento.Columns.Add("Procedimiento");
                tbProcedimiento.Columns.Add("Cobertura");
                tbProcedimiento.Columns.Add("Asistencia");
                tbProcedimiento.Columns.Add("Eventos");
                tbProcedimiento.Columns.Add("Estado");
                ViewState["tblProcedimiento"] = tbProcedimiento;
                funCargarCombos();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }   
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = ViewState["codigoProducto"].ToString();
                objparam[2] = 59;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "Asignar Procedimientos al Producto: " + dt.Tables[0].Rows[0][0].ToString();

                objparam[2] = 33;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                DataTable dtp = dt.Tables[0];
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["tblProcedimiento"] = dtp;

                objparam[0] = int.Parse(ViewState["codigoProducto"].ToString());
                objparam[1] = "Ox";
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
                    txtAsisAnual.Text = "1";
                    txtAsisMensual.Text = "1";
                    chkFechaCobertura.Checked = false;
                    chkFechaSistema.Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funCargarCombos()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 9;
                ddlProcedimientos.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlProcedimientos.DataTextField = "Descripcion";
                ddlProcedimientos.DataValueField = "Codigo";
                ddlProcedimientos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funClearObject()
        {
            ddlProcedimientos.SelectedValue = "0";
            txtCobertura.Text = "0";
            txtAsisAnual.Text = "1";
            txtAsisMensual.Text = "1";
            imgAgregar.Enabled = true;
            imgModificar.Enabled = false;
            imgCancelar.Enabled = false;
            tbProcedimiento.Columns.Add("Codigo");
            tbProcedimiento.Columns.Add("CodigoProducto");
            tbProcedimiento.Columns.Add("CodigoProcedimiento");
            tbProcedimiento.Columns.Add("Procedimiento");
            tbProcedimiento.Columns.Add("Cobertura");
            tbProcedimiento.Columns.Add("Asistencia");
            tbProcedimiento.Columns.Add("Eventos");
            tbProcedimiento.Columns.Add("Estado");
            ViewState["tblProcedimiento"] = tbProcedimiento;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
        }
        private void funLlenarClientes(TreeNode treenode)
        {
            try
            {
                int idCampaign = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 4;
                objparam[1] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && treenode != null)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        idCampaign = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = funAgregarGrupo(node, idCampaign);
                        treenode.ChildNodes.Add(node);
                    }
                    trvClientes.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        private TreeNode funAgregarGrupo(TreeNode node, int idCampaign)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 13;
                objparam[1] = idCampaign;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }

            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                return node;
            }
        }

        private TreeNode funAgregarProducto(TreeNode node, int idCampaign, string idGrupo)
        {
            try
            {
                count = node.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    node.ChildNodes.RemoveAt(0);
                }
                Array.Resize(ref objparam, 3);
                objparam[0] = idCampaign;
                objparam[1] = idGrupo;
                objparam[2] = 45;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        unNode.Text = "<img alt='' src='../Botones/productos.png' width=20px height=20px />" + drFila[1].ToString();
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }

            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                return node;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void trvClientes_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    funLlenarClientes(e.Node);
                    break;
            }
        }
        protected void trvClientes_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                lblerror.Text = "";
                switch (node.Depth)
                {
                    case 1:
                        ViewState["codigoProducto"] = 0;
                        lbltitulo.Text = "";
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        break;
                    case 2:
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["codigoProducto"] = pathRoot[3].ToString();
                        funCargaMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                lblerror.Text = "";
                bool lexiste = false;
                if (int.Parse(ViewState["codigoProducto"].ToString()) == 0)
                {
                    new Funciones().funShowJSMessage("Seleccione Producto para Asignar..!", this);
                    return;
                }
                if (ddlProcedimientos.SelectedValue == "0")
                {                    
                    new Funciones().funShowJSMessage("Seleccione Procedimiento..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtCobertura.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese % de Cobertura..!", this);
                    return;
                }
                if (ViewState["tblProcedimiento"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblProcedimiento"];
                    if (tblbuscar.Rows.Count > 0)
                    {
                        maxCodigo = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    }
                    else maxCodigo = 0;
                    DataRow result = tblbuscar.Select("CodigoProcedimiento='" + ddlProcedimientos.SelectedValue + "'").FirstOrDefault();
                    if (result != null) lexiste = true;
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Procedimiento ya está agregado..!", this);
                    return;
                }
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProcedimiento"];
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["CodigoProducto"] = ViewState["codigoProducto"].ToString();
                filagre["CodigoProcedimiento"] = int.Parse(ddlProcedimientos.SelectedValue);
                filagre["Procedimiento"] = ddlProcedimientos.SelectedItem.ToString();
                filagre["Cobertura"] = txtCobertura.Text.Trim();
                filagre["Asistencia"] = "1";
                filagre["Eventos"] = "1";
                filagre["Estado"] = "Activo";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Procedimiento";
                ViewState["tblProcedimiento"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                imgModificar.Enabled = false;
                ddlProcedimientos.SelectedValue = "0";
                txtCobertura.Text = "0";
                txtAsisAnual.Text = "1";
                txtAsisMensual.Text = "1";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblest = new Label();
            CheckBox chkest = new CheckBox();
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkest = (CheckBox)(e.Row.Cells[2].FindControl("chkSelecc"));
                    lblest = (Label)(e.Row.Cells[3].FindControl("lblEstado"));
                    string codigoProducto = grdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoProducto"].ToString();
                    int codigoProcedim = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoProcedimiento"].ToString());
                    Array.Resize(ref objparam, 3);
                    objparam[0] = codigoProcedim;
                    objparam[1] = codigoProducto;
                    objparam[2] = 46;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        chkest.Checked = dt.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                        lblest.Text = dt.Tables[0].Rows[0]["Estado"].ToString();
                    }
                    else
                    {
                        chkest.Enabled = false;
                        chkest.Checked = true;
                        lblest.Text = "Activo";
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            lblerror.Text = "";
            try
            {
                tbProcedimiento = (DataTable)ViewState["tblProcedimiento"];
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                //int intIndex = gvRow.RowIndex;
                ViewState["index"] = gvRow.RowIndex;
                ViewState["Codigo"] = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                resultado = tbProcedimiento.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                ViewState["CodigoProcedimiento"] = resultado["CodigoProcedimiento"].ToString();
                ddlProcedimientos.SelectedValue = resultado["CodigoProcedimiento"].ToString();
                ViewState["Estado"] = resultado["Estado"].ToString();
                txtCobertura.Text = resultado["Cobertura"].ToString();
                //txtAsisAnual.Text = resultado["Asistencia"].ToString();
                //txtAsisMensual.Text = resultado["Eventos"].ToString();
                imgModificar.Enabled = true;
                imgAgregar.Enabled = false;
                imgCancelar.Enabled = true;
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[gvRow.RowIndex].BackColor = System.Drawing.Color.DarkGray;
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
                bool lexiste = false;
                if (ddlProcedimientos.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Procedimiento..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtCobertura.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese % de Cobertura..!", this);
                    return;
                }
                if (ViewState["tblProcedimiento"] != null)
                {
                    if (ViewState["CodigoProcedimiento"].ToString() == ddlProcedimientos.SelectedValue)
                        lexiste = false;
                    else
                    {
                        DataTable tblbuscar = (DataTable)ViewState["tblProcedimiento"];
                        DataRow result = tblbuscar.Select("CodigoProcedimiento='" + ddlProcedimientos.SelectedValue + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Procedimiento ya está agregado..!", this);
                    return;
                }
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProcedimiento"];
                tblagre.Rows.RemoveAt(int.Parse(ViewState["index"].ToString()));
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = ViewState["Codigo"].ToString();
                filagre["CodigoProducto"] = ViewState["codigoProducto"].ToString();
                filagre["CodigoProcedimiento"] =ddlProcedimientos.SelectedValue;
                filagre["Procedimiento"] = ddlProcedimientos.SelectedItem.ToString();
                filagre["Cobertura"] = txtCobertura.Text;
                filagre["Asistencia"] = "1";
                filagre["Eventos"] = "1";
                filagre["Estado"] = ViewState["Estado"].ToString();
                tblagre.Rows.Add(filagre);
                ViewState["tblProcedimiento"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                imgModificar.Enabled = false;
                imgCancelar.Enabled = false;
                imgAgregar.Enabled = true;
                ddlProcedimientos.SelectedValue = "0";
                txtCobertura.Text = "0";
                //txtAsisAnual.Text = "1";
                //txtAsisMensual.Text = "1";
                chkFechaCobertura.Checked = true;
                chkFechaSistema.Checked=false;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            Label lblest = new Label();
            CheckBox chkest = new CheckBox();

            chkest = (CheckBox)(gvRow.Cells[4].FindControl("chkSelecc"));
            lblest = (Label)(gvRow.Cells[5].FindControl("lblEstado"));
            lblest.Text = chkest.Checked ? "Activo" : "Inactivo";
            tbProcedimiento = (DataTable)ViewState["tblProcedimiento"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            DataRow[] result = tbProcedimiento.Select("Codigo='" + codigo + "'");
            result[0]["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
            tbProcedimiento.AcceptChanges();
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            lblerror.Text = "";
            funClearObject();
            funCargaMantenimiento();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdvDatos.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Agregue Procedimientos..!", this);
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
                Array.Resize(ref objparam, 15);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = Session["MachineName"].ToString();
                objparam[2] = ViewState["codigoProducto"].ToString();
                objparam[3] = txtAsisAnual.Text.Trim();
                objparam[4] = txtAsisMensual.Text.Trim();
                objparam[5] = "Ox";
                objparam[6] = chkFechaCobertura.Checked ? "FC" : "FS";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = "";
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = 0;
                objparam[14] = 0;
                dt = new Conexion(2, "").FunInsertarProductoProcedimiento(objparam, (DataTable)ViewState["tblProcedimiento"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    grdvDatos.DataSource = null;
                    grdvDatos.DataBind();
                    string strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(strResponse, true);
                }
                else
                {
                    lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
                    funClearObject();
                    funCargaMantenimiento();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        protected void chkFechaCobertura_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaSistema.Checked = false;
            if (chkFechaCobertura.Checked == false) chkFechaSistema.Checked = true;
        }
        protected void chkFechaSistema_CheckedChanged(object sender, EventArgs e)
        {
            chkFechaCobertura.Checked = false;
            if (chkFechaSistema.Checked == false) chkFechaCobertura.Checked = true;
        }
        #endregion
    }
}