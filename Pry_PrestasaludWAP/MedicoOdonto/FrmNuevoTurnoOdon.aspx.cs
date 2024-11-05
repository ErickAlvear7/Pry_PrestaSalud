using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.MedicoOdonto
{
    public partial class FrmNuevoTurnoOdon : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Funciones fun = new Funciones();
        DataTable tbTurnoMedico = new DataTable();
        int maxCodigo = 0, count = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                tbTurnoMedico.Columns.Add("Codigo");
                tbTurnoMedico.Columns.Add("CodigoHora");
                tbTurnoMedico.Columns.Add("CodigoDia");
                tbTurnoMedico.Columns.Add("Dia");
                tbTurnoMedico.Columns.Add("Horario");
                tbTurnoMedico.Columns.Add("Intervalo");
                tbTurnoMedico.Columns.Add("Estado");
                ViewState["tblTurnoMedico"] = tbTurnoMedico;
                lbltitulo.Text = "Asignar Turnos Odontológicos";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());

                funCargarCombos();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funLlenarPrestador(TreeNode treenode)
        {
            try
            {
                int idCiudad = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 6;
                objparam[1] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && treenode != null)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        idCiudad = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = funAgregarPrestadora(node, idCiudad);
                        treenode.ChildNodes.Add(node);
                    }
                    trvPrestadoras.CollapseAll();
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
                objparam[0] = 4;
                ddlHorario.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlHorario.DataTextField = "Descripcion";
                ddlHorario.DataValueField = "Codigo";
                ddlHorario.DataBind();

                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = ddlHorario.SelectedItem.ToString();
                objparam[2] = 10;
                ddlIntervalo.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlIntervalo.DataTextField = "Descripcion";
                ddlIntervalo.DataValueField = "Codigo";
                ddlIntervalo.DataBind();

                Array.Resize(ref objparam, 1);
                objparam[0] = 8;
                ddlDia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlDia.DataTextField = "Descripcion";
                ddlDia.DataValueField = "Codigo";
                ddlDia.DataBind();

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void funCargaMantenimiento(int codigoMedico, int codigoPrestadora)
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 20;
                objparam[1] = codigoPrestadora;
                objparam[2] = codigoMedico;
                objparam[3] = 0;
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = 0;
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = "";
                objparam[16] = "";
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt.Tables[0].Rows.Count > 0) txtNombres.Text = dt.Tables[0].Rows[0][0].ToString();
                if (dt.Tables[1].Rows.Count > 0)
                {
                    DataTable dtTurno = dt.Tables[1];
                    ViewState["tblTurnoMedico"] = dtTurno;
                    grdvDatos.DataSource = dtTurno;
                    grdvDatos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funClearObject()
        {
            ddlHorario.SelectedIndex = 0;
            ddlDia.SelectedIndex = 0;
            ddlIntervalo.Items.Clear();
            ListItem item = new ListItem("--Seleccione Intervalo--", "0");
            ddlIntervalo.Items.Add(item);
            imgAgregar.Enabled = true;
            imgCancelar.Enabled = false;
            tbTurnoMedico.Columns.Add("Codigo");
            tbTurnoMedico.Columns.Add("CodigoHora");
            tbTurnoMedico.Columns.Add("CodigoDia");
            tbTurnoMedico.Columns.Add("Dia");
            tbTurnoMedico.Columns.Add("Horario");
            tbTurnoMedico.Columns.Add("Intervalo");
            tbTurnoMedico.Columns.Add("Estado");
            ViewState["tblTurnoMedico"] = tbTurnoMedico;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void trvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    funLlenarPrestador(e.Node);
                    break;
            }
        }
        private TreeNode funAgregarPrestadora(TreeNode node, int idCiudad)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 7;
                objparam[1] = idCiudad;
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
                lblerror.Text = ex.ToString();
                return node;
            }
        }
        protected void trvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                lblerror.Text = "";
                switch (node.Depth)
                {
                    case 2:
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathRoot = node.ValuePath.Split(new char[] { '/' });
                        node = funListarMedicos(node, int.Parse(pathRoot[3].ToString()));
                        break;
                    case 3:
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathMedico = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPrestadora"] = pathMedico[3].ToString();
                        ViewState["CodigoMedico"] = pathMedico[4].ToString();
                        funClearObject();
                        funCargaMantenimiento(int.Parse(pathMedico[4].ToString()), int.Parse(pathMedico[3].ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        private TreeNode funListarMedicos(TreeNode node, int idPres)
        {
            try
            {
                count = node.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    node.ChildNodes.RemoveAt(0);
                }
                Array.Resize(ref objparam, 2);
                objparam[0] = 8;
                objparam[1] = idPres;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode;
                        unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        //unNode.ImageUrl = @"~/Botones/medico.png";
                        unNode.Text = "<img alt='' src='../Botones/medico.png' width=20px height=20px />" + drFila[1].ToString();
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                return node;
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
                    chkest = (CheckBox)(e.Row.Cells[3].FindControl("chkEstadoDet"));
                    lblest = (Label)(e.Row.Cells[4].FindControl("lblEstado"));

                    int codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    Array.Resize(ref objparam, 3);
                    objparam[0] = codigo;
                    objparam[1] = "";
                    objparam[2] = 24;
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
        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            bool lexiste = false;
            lblerror.Text = "";
            try
            {
                if (txtNombres.Text == "")
                {
                    lblerror.Text = "Seleccione Médico..!";
                    return;
                }
                if (ddlHorario.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Horario..!";
                    return;
                }
                if (ddlIntervalo.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Intervalo..!";
                    return;
                }
                if (ddlDia.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Día..!";
                    return;
                }

                if (ViewState["tblTurnoMedico"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblTurnoMedico"];
                    DataRow result = tblbuscar.Select("Dia='" + ddlDia.SelectedItem.ToString() + "' and Horario='" + ddlHorario.SelectedItem.ToString() + "' and CodigoHora='" + ddlIntervalo.SelectedValue.ToString() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "Codigo";
                    if (result != null) lexiste = true;
                    foreach (DataRow dr in tblbuscar.Rows)
                    {
                        maxCodigo = int.Parse(dr[0].ToString());
                    }
                }

                if (lexiste)
                {
                    lblerror.Text = "Ya existe turno..!";
                    return;
                }
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblTurnoMedico"];
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["CodigoHora"] = ddlIntervalo.SelectedValue;
                filagre["CodigoDia"] = ddlDia.SelectedValue;
                filagre["Dia"] = ddlDia.SelectedItem.ToString();
                filagre["Horario"] = ddlHorario.SelectedItem.ToString();
                filagre["Intervalo"] = ddlIntervalo.SelectedItem.ToString();
                filagre["Estado"] = "Activo";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "CodigoDia";
                ViewState["tblTurnoMedico"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                imgCancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEstadoDet_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            Label lblest = new Label();
            CheckBox chkest = new CheckBox();

            chkest = (CheckBox)(gvRow.Cells[3].FindControl("chkEstadoDet"));
            lblest = (Label)(gvRow.Cells[4].FindControl("lblEstado"));
            lblest.Text = chkest.Checked ? "Activo" : "Inactivo";
            tbTurnoMedico = (DataTable)ViewState["tblTurnoMedico"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            DataRow[] result = tbTurnoMedico.Select("Codigo=" + codigo);
            result[0]["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
            tbTurnoMedico.AcceptChanges();
            ViewState["tblTurnoMedico"] = tbTurnoMedico;
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            funClearObject();
            funCargaMantenimiento(int.Parse(ViewState["CodigoMedico"].ToString()), int.Parse(ViewState["CodigoPrestadora"].ToString()));
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                Array.Resize(ref objparam, 4);
                objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[1] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                objparam[2] = int.Parse(Session["usuCodigo"].ToString());
                objparam[3] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertarTurnoOdonto(objparam, (DataTable)ViewState["tblTurnoMedico"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    string strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(strResponse, false);
                }
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void ddlHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlIntervalo.Items.Clear();
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = ddlHorario.SelectedItem.ToString();
            objparam[2] = 10;
            ddlIntervalo.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            ddlIntervalo.DataTextField = "Descripcion";
            ddlIntervalo.DataValueField = "Codigo";
            ddlIntervalo.DataBind();
        }
        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = "";
                objparam[2] = 7;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    DataTable tblagre = new DataTable();
                    tblagre = (DataTable)ViewState["tblTurnoMedico"];
                    DataRow[] rows;
                    rows = tblagre.Select("Codigo=" + codigo + "");
                    foreach (DataRow row in rows)
                    {
                        tblagre.Rows.Remove(row);
                    }
                    ViewState["tblTurnoMedico"] = tblagre;
                    grdvDatos.DataSource = tblagre;
                    grdvDatos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        #endregion
    }
}