namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmMenuAdministrador : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[11];
        string _response = "";
        string[] pathroot;
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

                ViewState["PrestadorMedico"] = "0";
                ViewState["PrestadorOdonto"] = "0";
                //PARA TIPO MEDICO
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 169;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows[0]["Valor"].ToString() != "0") ViewState["PrestadorMedico"] = "1";
                objparam[2] = 170;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows[0]["Valor"].ToString() != "0") ViewState["PrestadorOdonto"] = "1";
            }
        }
        #endregion

        #region Botones y Eventos
        protected void TrvMenu_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarMenu(e.Node);
                    TrvMenu.CollapseAll();
                    break;
            }
        }

        private void FunLlenarMenu(TreeNode node)
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 1:
                        if (ViewState["PrestadorMedico"].ToString() == "1")
                        {
                            TreeNode unnode = new TreeNode("Citas Médicas", "1");
                            unnode = FunLLenarMedico(unnode);
                            node.ChildNodes.Add(unnode);
                        }
                        break;
                    case 2:
                        if (ViewState["PrestadorOdonto"].ToString() == "1")
                        {
                            TreeNode dosnode = new TreeNode("Citas Odontológicas", "2");
                            dosnode = FunLLenarOdonto(dosnode);
                            node.ChildNodes.Add(dosnode);
                        }
                        break;
                    case 3:
                        TreeNode tresnode = new TreeNode("Reportes", "3");
                        tresnode = FunLLenarReportes(tresnode);
                        node.ChildNodes.Add(tresnode);
                        break;
                }
            }
        }

        private TreeNode FunLLenarMedico(TreeNode node)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    TreeNode unNode = new TreeNode("Agendadas", "4");
                    node.ChildNodes.Add(unNode);
                }
                if (i == 2)
                {
                    TreeNode unNode = new TreeNode("Gestionadas", "5");
                    node.ChildNodes.Add(unNode);
                }
            }
            return node;
        }

        private TreeNode FunLLenarOdonto(TreeNode node)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    TreeNode unNode = new TreeNode("Agendadas", "6");
                    node.ChildNodes.Add(unNode);
                }
                if (i == 2)
                {
                    TreeNode unNode = new TreeNode("Gestionadas", "7");
                    node.ChildNodes.Add(unNode);
                }
            }
            return node;
        }

        private TreeNode FunLLenarReportes(TreeNode node)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    if (ViewState["PrestadorMedico"].ToString() == "1")
                    {
                        TreeNode unNode = new TreeNode("Médicos", "8");
                        node.ChildNodes.Add(unNode);
                    }
                }
                if (i == 2)
                {
                    if (ViewState["PrestadorOdonto"].ToString() == "1")
                    {
                        TreeNode unNode = new TreeNode("Odontólogos", "9");
                        node.ChildNodes.Add(unNode);
                    }
                }
            }
            return node;
        }

        protected void TrvMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeView treepress = (TreeView)(sender);
                TreeNode node = treepress.SelectedNode;
                switch (node.Depth)
                {
                    case 2: //LISTA DE CLIENTES PARA ATENCION MEDICA
                        pathroot = node.ValuePath.Split(new char[] { '/' });
                        switch (pathroot[2].ToString())
                        {
                            case "4":
                                _response = string.Format("{0}", "FrmCitaAdministrador.aspx");
                                break;
                            case "5":
                                _response = string.Format("{0}", "FrmGesReaMedAdmin.aspx");
                                break;
                            case "6":
                                _response = string.Format("{0}", "FrmCitaOdontoAdministrador.aspx");
                                break;
                            case "7":
                                _response = string.Format("{0}", "FrmPresuRealizadoAdministrador.aspx");
                                break;
                            case "8":
                                _response = string.Format("{0}", "FrmRepFactMedAdmin.aspx");
                                break;
                            case "9":
                                _response = string.Format("{0}", "FrmRepFactOdonAdmin.aspx");
                                break;
                        }
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script>window.open('" + _response);
                        sb.Append("','DetalleAdmin');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CitasMedicas", sb.ToString(), false);
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        } 
        #endregion
    }
}