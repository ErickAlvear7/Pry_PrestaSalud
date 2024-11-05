using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Configuracion
{
    public partial class FrmConfigInfAdicional : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        DataTable tbProcedimiento = new DataTable();
        int count = 0, maxCodigo = 0, codigo = 0;
        DataRow resultado;
        CheckBox chk1 = new CheckBox();
        CheckBox chk2 = new CheckBox();
        CheckBox chk3 = new CheckBox();
        string ver = "", body = "", estado = "", strResponse = "";
        DataTable dtbDatos = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                //ViewState["codigoProducto"] = 0;
                //tbProcedimiento.Columns.Add("Codigo");
                //tbProcedimiento.Columns.Add("CodigoProducto");
                //tbProcedimiento.Columns.Add("CodigoProcedimiento");
                //tbProcedimiento.Columns.Add("Procedimiento");
                //tbProcedimiento.Columns.Add("Cobertura");
                //tbProcedimiento.Columns.Add("Asistencia");
                //tbProcedimiento.Columns.Add("Eventos");
                //tbProcedimiento.Columns.Add("Estado");
                //ViewState["tblProcedimiento"] = tbProcedimiento;
                lbltitulo.Text = "Configuración Información Adicional";
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
                objparam[1] = ViewState["CodigoGrupo"].ToString();
                objparam[2] = 59;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "Configurar Inf. Adicional al Producto: " + dt.Tables[0].Rows[0][0].ToString();

                Array.Resize(ref objparam, 11);
                objparam[0] = 6;
                objparam[1] = "";
                objparam[2] = "";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[7] = 0;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["tblInfAdicional"] = dt.Tables[0];
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
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
                        ViewState["CodigoGrupo"] = 0;
                        lbltitulo.Text = "";
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        break;
                    case 2:
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoGrupo"] = pathRoot[3].ToString();
                        funCargaMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chk1 = (CheckBox)(e.Row.Cells[2].FindControl("chkVer"));
                    chk2 = (CheckBox)(e.Row.Cells[3].FindControl("chkBody"));
                    chk3 = (CheckBox)(e.Row.Cells[4].FindControl("chkEstado"));
                    ver = grdvDatos.DataKeys[e.Row.RowIndex].Values["Ver"].ToString();
                    body = grdvDatos.DataKeys[e.Row.RowIndex].Values["Body"].ToString();
                    estado = grdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    if (ver == "SI") chk1.Checked = true;
                    if (body == "SI") chk2.Checked = true;
                    if (estado == "Activo") chk3.Checked = true;
                }
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
                if (string.IsNullOrEmpty(txtCabecera.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre de la Cabecera..!", this);
                    return;
                }
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                resultado = dtbDatos.Select("Codigo='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                resultado["Cabecera"] = txtCabecera.Text.Trim();
                dtbDatos.AcceptChanges();
                ViewState["tblInfAdicional"] = dtbDatos;
                grdvDatos.DataSource = dtbDatos;
                grdvDatos.DataBind();
                txtCabecera.Text = "";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkVer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chk1 = (CheckBox)(gvRow.Cells[2].FindControl("chkVer"));
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                resultado = dtbDatos.Select("Codigo='" + codigo + "'").FirstOrDefault();
                resultado["Ver"] = chk1.Checked ? "SI" : "NO";
                dtbDatos.AcceptChanges();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkBody_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chk1 = (CheckBox)(gvRow.Cells[3].FindControl("chkBody"));
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                resultado = dtbDatos.Select("Codigo='" + codigo + "'").FirstOrDefault();
                resultado["Body"] = chk1.Checked ? "SI" : "NO";
                dtbDatos.AcceptChanges();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chk1 = (CheckBox)(gvRow.Cells[4].FindControl("Estado"));
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                resultado = dtbDatos.Select("Codigo='" + codigo + "'").FirstOrDefault();
                resultado["Estado"] = chk1.Checked ? "Activo" : "Inactivo";
                dtbDatos.AcceptChanges();
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
                foreach (GridViewRow fr in grdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                ViewState["Codigo"] = codigo;
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                resultado = dtbDatos.Select("Codigo='" + codigo + "'").FirstOrDefault();
                txtCabecera.Text = resultado["Cabecera"].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dtbDatos = (DataTable)ViewState["tblInfAdicional"];
                Array.Resize(ref objparam, 14);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = "";
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = 0;
                foreach (DataRow dr in dtbDatos.Rows)
                {
                    objparam[2] = int.Parse(dr["Codigo"].ToString());
                    objparam[3] = dr["Campo"].ToString();
                    objparam[4] = dr["Cabecera"].ToString();
                    objparam[5] = dr["Ver"].ToString();
                    objparam[6] = dr["Body"].ToString();
                    objparam[7] = dr["Estado"].ToString();
                    new Conexion(2, "").funConsultarSqls("sp_NuevaConfigInfAdic", objparam);
                }
                strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(strResponse, true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}