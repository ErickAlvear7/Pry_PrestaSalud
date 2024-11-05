using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Titulares
{
    public partial class FrmTitularAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        int count = 0;
        string strCodPersona = "", strCodTitular = "", strCodProducto = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                if (Session["SalirAgenda"].ToString() == "NO")
                {
                    if (Session["TipoCita"].ToString() == "CitaMedica")
                        Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                    if (Session["TipoCita"].ToString() == "CitaOdontologica")
                        Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                    return;
                }
                ViewState["codigoProducto"] = 0;
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];            
        }
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["codigoProducto"].ToString());
                objparam[1] = txtCriterio.Text.Trim().ToUpper();
                objparam[2] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularAdmin", objparam);
                if (int.Parse(dt.Tables[0].Rows[0][0].ToString()) > 3000)
                {
                    new Funciones().funShowJSMessage("Demasiados datos, realice otro criterio de búsqueda..!", this);
                    return;
                }
                objparam[2] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularAdmin", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }

        private void FunLlenarClientes(TreeNode treenode)
        {
            try
            {
                int idCampaign = 0;
                if(Session["Perfil"].ToString() == "NOVA")
                {
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 32;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = 0;
                    objparam[7] = 0;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    if (dt != null && treenode != null)
                    {
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            idCampaign = int.Parse(dr[0].ToString());
                            TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                            node = FunAgregarGrupo(node, idCampaign);
                            treenode.ChildNodes.Add(node);
                        }
                        trvPrestadoras.CollapseAll();
                    }

                }
                else
                {
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
                            node = FunAgregarGrupo(node, idCampaign);
                            treenode.ChildNodes.Add(node);
                        }
                        trvPrestadoras.CollapseAll();
                    }
                }
               
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.Message;
            }
        }

        private TreeNode FunAgregarGrupo(TreeNode node, int idCampaign)
        {
            try
            {
                if (Session["Perfil"].ToString() == "CALLCENTER-NOVA")
                {
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 33;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = idCampaign;
                    objparam[7] = 0;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    if (dt != null && dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drFila in dt.Tables[0].Rows)
                        {
                            TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                            node.ChildNodes.Add(unNode);
                        }
                    }

                }
                else
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
                    
                }
                return node;

            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.Message;
                return node;
            }
        }

        private TreeNode FunAgregarProducto(TreeNode node, int idCampaign, string idGrupo)
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
                lbltitulo.Text = ex.Message;
                return node;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (int.Parse(ViewState["codigoProducto"].ToString()) > 0) Response.Redirect("FrmNuevoTitular.aspx?&CodPersona=0" + "&CodTitular=0" + "&CodProducto=" + ViewState["codigoProducto"].ToString(), true);
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            strCodPersona = grdvDatos.DataKeys[gvRow.RowIndex].Values["CodPersona"].ToString();
            strCodTitular = grdvDatos.DataKeys[gvRow.RowIndex].Values["CodTitular"].ToString();
            strCodProducto = grdvDatos.DataKeys[gvRow.RowIndex].Values["CodProducto"].ToString();
            Response.Redirect("FrmNuevoTitular.aspx?CodPersona=" + strCodPersona + "&CodTitular=" + strCodTitular + "&CodProducto=" + ViewState["codigoProducto"].ToString(), true);
        }
        
        protected void trvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarClientes(e.Node);
                    break;
            }
        }

        protected void trvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 1:
                        ViewState["codigoProducto"] = 0;
                        lbltitulo.Text = "";
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        break;
                    case 2:
                        ViewState["codigoProducto"] = 0;
                        lbltitulo.Text = "";
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathProducto = node.ValuePath.Split(new char[] { '/' });
                        node = FunAgregarProducto(node, int.Parse(pathProducto[2].ToString()), pathProducto[3].ToString());
                        break;
                    case 3:
                        grdvDatos.DataSource = null;
                        grdvDatos.DataBind();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["codigoProducto"] = pathRoot[4].ToString();
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(ViewState["codigoProducto"].ToString());
                        objparam[1] = "";
                        objparam[2] = 12;
                        dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        lbltitulo.Text = "Administrar Titulares " + dt.Tables[0].Rows[0][0].ToString();
                        //string strResponse = string.Format("{0}?codProducto={1}", "FrmTitularAdmin.aspx", pathRoot[3].ToString());
                        //Response.Clear();
                        //Response.Write("<script>window.open('" + strResponse + "','DetallePrestadora');</script>");
                        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        //sb.Append(@"<script>window.open('" + strResponse);
                        //sb.Append("','DetallePrestadora');");
                        //sb.Append("</script>");
                        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Producto", sb.ToString(), false);
                        break;
                }
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.Message;
            }
        }        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["codigoProducto"].ToString()) > 0)
                {
                    if (!string.IsNullOrEmpty(txtCriterio.Text.Trim())) FunCargaMantenimiento();
                    else lbltitulo.Text = "Ingrese Criterio Cédula o Nombres/Apellidos";
                }
            }
            catch(Exception ex)
            {
                lbltitulo.Text = ex.ToString();
                ViewState["grdvDatos"] = dt;
            }
        }
        #endregion
    }
}