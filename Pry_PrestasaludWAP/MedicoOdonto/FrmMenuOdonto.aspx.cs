namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmMenuOdonto : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        string strResponse = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Array.Resize(ref objparam, 11);
                objparam[0] = 4;
                objparam[1] = "LOGOSI";
                objparam[2] = "PATH LOGOS";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = 0;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                if (dts.Tables[0].Rows.Count > 0) ImgLogo.ImageUrl = dts.Tables[0].Rows[0]["Valorv"].ToString();
                LblUsuario.Text = Session["usuNombres"].ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void trvLista_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    funLlenarCiudadAsignada(e.Node);
                    //trvLista.CollapseAll();
                    break;
            }
        }
        #endregion

        #region Procedimientos y Funciones
        //private void funLlenarListaTrabajo(TreeNode node)
        //{
        //    contar = node.ChildNodes.Count;
        //    for (int i = 0; i < contar; i++)
        //    {
        //        node.ChildNodes.RemoveAt(0);
        //    }
        //    string nombre = "Citas Agendadas";
        //    for (int i = 0; i < 2; i++)
        //    {
        //        TreeNode unNode;
        //        unNode = new TreeNode(nombre, i.ToString());
        //        nombre = "Gestiones Realizadas";
        //        node.ChildNodes.Add(unNode);
        //    }
        //}

        private void funLlenarCiudadAsignada(TreeNode node)
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = "";
            objparam[2] = 98;
            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            foreach (DataRow dr in dts.Tables[0].Rows)
            {
                TreeNode unnode = new TreeNode(dr[0].ToString(), dr[1].ToString());
                unnode = funLlenarPrestadora(unnode, int.Parse(dr[1].ToString()));
                node.ChildNodes.Add(unnode); 
            }
            trvLista.CollapseAll();
        }
        private TreeNode funLlenarPrestadora(TreeNode node, int codigoCiudad)
        {
            try
            {
                //contar = node.ChildNodes.Count;
                //for (int i = 0; i < contar; i++)
                //{
                //    node.ChildNodes.RemoveAt(0);
                //}
                Array.Resize(ref objparam, 7);
                objparam[0] = 12;
                objparam[1] = int.Parse(Session["usuCodigo"].ToString());
                objparam[2] = codigoCiudad;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                foreach (DataRow dr in dts.Tables[0].Rows)
                {
                    //TreeNode unNode;
                    //unNode = new TreeNode(dr[0].ToString(), dr[1].ToString());
                    //node.ChildNodes.Add(unNode);
                    TreeNode unnode = new TreeNode(dr[0].ToString(), dr[1].ToString());
                    unnode = funLlenarMedicosCitasOdonto(unnode, codigoCiudad, int.Parse(dr[1].ToString()));
                    node.ChildNodes.Add(unnode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return node;
        }

        private TreeNode funLlenarMedicosCitasOdonto(TreeNode node, int codigoCiudad, int codigoPrestadora)
        {
            try
            {
                //contar = node.ChildNodes.Count;
                //for (int i = 0; i < contar; i++)
                //{
                //    node.ChildNodes.RemoveAt(0);
                //}
                Array.Resize(ref objparam, 7);
                objparam[0] = 13;
                objparam[1] = int.Parse(Session["usuCodigo"].ToString());
                objparam[2] = codigoCiudad;
                objparam[3] = codigoPrestadora;
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (dts != null && dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dts.Tables[0].Rows)
                    {
                        //TreeNode unNode;
                        TreeNode unNode = new TreeNode(dr[0].ToString(), dr[1].ToString());
                        unNode.Text = "<img alt='' src='../Botones/medico.png' width=20px height=20px />" + dr[0].ToString();
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void trvLista_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    //case 1:
                    //    string[] pathRoot1 = node.ValuePath.Split(new char[] { '/' });
                    //    trvLista.SelectedNode.Expand();
                    //    funLlenarPrestadora(node, int.Parse(pathRoot1[1].ToString()));
                    //    break;
                    //case 2:
                    //    string[] pathRoot2 = node.ValuePath.Split(new char[] { '/' });
                    //    trvLista.SelectedNode.Expand();
                    //    funLlenarMedicosCitasOdonto(node, int.Parse(pathRoot2[1].ToString()), int.Parse(pathRoot2[2].ToString()));
                    //    break;
                    //case 3:
                    //    trvLista.SelectedNode.Expand();
                    //    funLlenarListaTrabajo(node);
                    //    break;
                    case 4:
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        if (pathRoot[4].ToString() == "0") strResponse = string.Format("{0}?CodigoMedico={1}", "FrmOdontoCitasAdmin.aspx", pathRoot[3].ToString());
                        else strResponse = string.Format("{0}?CodigoMedico={1}", "FrmPrespuestoRealizadoAdmin.aspx", pathRoot[3].ToString());                            
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script>window.open('" + strResponse);
                        sb.Append("','DetalleOdonto');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CitasOdonto", sb.ToString(), false);

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}