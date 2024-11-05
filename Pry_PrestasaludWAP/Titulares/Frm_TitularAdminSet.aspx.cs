using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Titulares
{
    public partial class Frm_TitularAdminSet : Page
    {
        #region Variables
        DataSet dts = new DataSet();
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
                ViewState["CodigoPROD"] = 0;
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else GrdvDatos.DataSource = ViewState["GrdvDatos"];
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPROD"].ToString());
                objparam[1] = TxtCriterio.Text.Trim().ToUpper();
                objparam[2] = 1;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarTitularAdmin", objparam);
                if (int.Parse(dts.Tables[0].Rows[0][0].ToString()) > 3000)
                {
                    new Funciones().funShowJSMessage("Demasiados datos, realice otro criterio de búsqueda..!", this);
                    return;
                }
                objparam[2] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarTitularAdmin", objparam);
                GrdvDatos.DataSource = dts;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        private void FunLlenarClientes(TreeNode treenode)
        {
            try
            {
                int idCampaign = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 15;
                objparam[1] = int.Parse(Session["usuCodigo"].ToString());
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dts.Tables[0].Rows)
                    {
                        idCampaign = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = FunAgregarGrupo(node, idCampaign);
                        treenode.ChildNodes.Add(node);
                    }
                    TrvPrestadoras.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.Message;
            }
        }

        private TreeNode FunAgregarGrupo(TreeNode node, int idCampaign)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 16;
                objparam[1] = idCampaign;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dts.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }

            catch (Exception ex)
            {
                Lbltitulo.Text = ex.Message;
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
                objparam[2] = 130;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts != null && dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dts.Tables[0].Rows)
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
                Lbltitulo.Text = ex.Message;
                return node;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void TrvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarClientes(e.Node);
                    break;
            }
        }

        protected void TrvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 1:
                        ViewState["CodigoPROD"] = 0;
                        Lbltitulo.Text = "";
                        GrdvDatos.DataSource = null;
                        GrdvDatos.DataBind();
                        break;
                    case 2:
                        ViewState["CodigoPROD"] = 0;
                        Lbltitulo.Text = "";
                        GrdvDatos.DataSource = null;
                        GrdvDatos.DataBind();
                        TrvPrestadoras.SelectedNode.Expand();
                        string[] pathProducto = node.ValuePath.Split(new char[] { '/' });
                        node = FunAgregarProducto(node, int.Parse(pathProducto[2].ToString()), pathProducto[3].ToString());
                        break;
                    case 3:
                        GrdvDatos.DataSource = null;
                        GrdvDatos.DataBind();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPROD"] = pathRoot[4].ToString();
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(ViewState["CodigoPROD"].ToString());
                        objparam[1] = "";
                        objparam[2] = 12;
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        Lbltitulo.Text = "Administrar Titulares " + dts.Tables[0].Rows[0][0].ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.Message;
            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frm_NuevoTitularSet.aspx?&CodPersona=0" + "&CodTitular=0" + "&CodProducto=" + ViewState["CodigoPROD"].ToString(), true);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["CodigoPROD"].ToString()) > 0)
                {
                    if (!string.IsNullOrEmpty(TxtCriterio.Text.Trim())) FunCargaMantenimiento();
                    else Lbltitulo.Text = "Ingrese Criterio Cédula o Nombres/Apellidos";
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
                ViewState["GrdvDatos"] = dts;
            }
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            strCodPersona = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodPersona"].ToString();
            strCodTitular = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodTitular"].ToString();
            strCodProducto = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodProducto"].ToString();
            Response.Redirect("Frm_NuevoTitularSet.aspx?CodPersona=" + strCodPersona + "&CodTitular=" + strCodTitular + "&CodProducto=" + ViewState["CodigoPROD"].ToString(), true);

        }
        #endregion

    }
}