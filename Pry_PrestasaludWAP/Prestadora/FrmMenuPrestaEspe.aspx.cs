using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Prestadora
{
    public partial class FrmMenuPrestaEspe : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Funciones fun = new Funciones();
        DataTable tbTurnoMedico = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbltitulo.Text = "PRESTADORAS / CLINICAS";
            }
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
                int idCiudad = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 0;
                objparam[1] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && treenode != null)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        idCiudad = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = funAgregarPrestadora(node, idCiudad);
                        node.CollapseAll();
                        treenode.ChildNodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmMenuPrestaEspe", ex.ToString(), frame.GetFileLineNumber());
            }
        }

        private TreeNode funAgregarPrestadora(TreeNode node, int idCiudad)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 1;
                objparam[1] = idCiudad;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        unNode.Text = "<img alt='' src='../../Botones/casa.png' width=25px height=25px />" + drFila[1].ToString();
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }

            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmMenuPrestaEspe", ex.ToString(), frame.GetFileLineNumber());
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
                    case 2:
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        string strResponse = string.Format("{0}?codPrestadora={1}", "FrmAsignarEspePrestadora.aspx", pathRoot[3].ToString());
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script>window.open('" + strResponse);
                        sb.Append("','DetallePrestaEspe');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Prestadoras", sb.ToString(), false);
                        break;
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmMenuPrestaEspe", ex.ToString(), frame.GetFileLineNumber());
            }
        }
        #endregion
    }
}