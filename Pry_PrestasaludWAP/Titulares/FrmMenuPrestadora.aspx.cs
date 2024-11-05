using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Titulares
{
    public partial class FrmMenuPrestadora : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        int count = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region Procedimientos y Funciones
        protected void trvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    funLlenarPrestador(e.Node);
                    break;
            }
        }

        private void funLlenarPrestador(TreeNode treenode)
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
                    trvPrestadoras.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.Message;
            }
        }

        private TreeNode funAgregarGrupo(TreeNode node, int idCampaign)
        {
            try
            {
                count = node.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    node.ChildNodes.RemoveAt(0);
                }
                Array.Resize(ref objparam, 2);
                objparam[0] = 13;
                objparam[1] = idCampaign;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[2].ToString(), drFila[1].ToString());
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
        private TreeNode funAgregarProducto(TreeNode node, int idCampaign, string idGrupo)
        {
            try
            {
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
        protected void trvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 1:
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathGrupo = node.ValuePath.Split(new char[] { '/' });
                        node = funAgregarGrupo(node, int.Parse(pathGrupo[2].ToString()));
                        break;
                    case 2:
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathProducto = node.ValuePath.Split(new char[] { '/' });
                        node = funAgregarProducto(node, int.Parse(pathProducto[2].ToString()), pathProducto[3].ToString());
                        break;                    
                    case 3:
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        string strResponse = string.Format("{0}?codProducto={1}", "FrmTitularAdmin.aspx", pathRoot[3].ToString());
                        //Response.Clear();
                        //Response.Write("<script>window.open('" + strResponse + "','DetallePrestadora');</script>");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script>window.open('" + strResponse);
                        sb.Append("','DetallePrestadora');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Producto", sb.ToString(), false);
                        break;
                }
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.Message;
            }
        }
        #endregion
    }
}