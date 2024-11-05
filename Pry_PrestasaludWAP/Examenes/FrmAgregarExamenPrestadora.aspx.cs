using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmAgregarExamenPrestadora : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        Object[] objparam = new Object[1];
        DataTable dtbgrupoexamen = new DataTable();
        DataTable dtbexamenes = new DataTable();
        DataTable dtbexamenestmp = new DataTable();
        DataTable dtbdatos = new DataTable();
        DataRow result, filagre, filagretem;
        DataRow[] drtemp, drexamenes;
        int maxcodigo = 0, codigogrex = 0;
        bool lexiste;
        ImageButton imgexamen;
        ImageButton imgeliminar;
        CheckBox chkestado;
        string estado, conexamen, codigo, response, codigoexeanterior;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                TxtPvp.Attributes.Add("onchange", "ValidarDecimales();");
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Asignar Procedimientos a Red/Prestadora";
                    dtbgrupoexamen.Columns.Add("Codigo");
                    dtbgrupoexamen.Columns.Add("GrupoExamen");
                    dtbgrupoexamen.Columns.Add("Descripcion");
                    dtbgrupoexamen.Columns.Add("Estado");
                    dtbgrupoexamen.Columns.Add("ConExamen");
                    ViewState["GrupoExamen"] = dtbgrupoexamen;

                    dtbexamenes.Columns.Add("Codigo");
                    dtbexamenes.Columns.Add("CodigoGREX");
                    dtbexamenes.Columns.Add("CodigoEXSE");
                    dtbexamenes.Columns.Add("Categoria");
                    dtbexamenes.Columns.Add("Examen");
                    dtbexamenes.Columns.Add("Valor");
                    dtbexamenes.Columns.Add("Pvp");
                    dtbexamenes.Columns.Add("Estado");
                    ViewState["Examenes"] = dtbexamenes;
                    ViewState["ExamenesTmp"] = dtbexamenes.Copy();

                    ViewState["CodigoPRES"] = "0";

                    FunCargarCombos(0);
                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
                }            
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 0;
                    objparam[1] = "";
                    objparam[2] = 137;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    DdlExamen.DataSource = dts;
                    DdlExamen.DataTextField = "Descripcion";
                    DdlExamen.DataValueField = "Codigo";
                    DdlExamen.DataBind();
                    break;
            }
        }

        protected void FunCargaMantenimiento()
        {
            try
            {
                dtbgrupoexamen.Columns.Add("Codigo");
                dtbgrupoexamen.Columns.Add("GrupoExamen");
                dtbgrupoexamen.Columns.Add("Descripcion");
                dtbgrupoexamen.Columns.Add("Estado");
                dtbgrupoexamen.Columns.Add("ConExamen");
                ViewState["GrupoExamen"] = dtbgrupoexamen;

                dtbexamenes.Columns.Add("Codigo");
                dtbexamenes.Columns.Add("CodigoGREX");
                dtbexamenes.Columns.Add("CodigoEXSE");
                dtbexamenes.Columns.Add("Categoria");
                dtbexamenes.Columns.Add("Examen");
                dtbexamenes.Columns.Add("Valor");
                dtbexamenes.Columns.Add("Pvp");
                dtbexamenes.Columns.Add("Estado");
                ViewState["Examenes"] = dtbexamenes;
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPRES"].ToString());
                objparam[1] = "";
                objparam[2] = 136;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                dtbexamenes = (DataTable)ViewState["Examenes"];
                if (dts.Tables[0].Rows.Count > 0) TxtPrestadora.Text = dts.Tables[0].Rows[0]["pres_nombre"].ToString();
                if (dts.Tables[1].Rows.Count > 0)
                {
                    ViewState["GrupoExamen"] = dts.Tables[1];
                    GrdvDatos.DataSource = dts.Tables[1];
                    GrdvDatos.DataBind();
                    PnlGrupoExamenes.Visible = true;
                    Array.Resize(ref objparam, 3);                    
                    objparam[1] = "";
                    objparam[2] = 136;
                    foreach (DataRow drfila in dts.Tables[1].Rows)
                    {
                        objparam[0] = int.Parse(drfila["Codigo"].ToString());
                        dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        foreach (DataRow drexam in dtx.Tables[0].Rows)
                        {
                            filagre = dtbexamenes.NewRow();
                            filagre["Codigo"] = drexam["Codigo"].ToString();
                            filagre["CodigoGREX"] = drfila["Codigo"].ToString();
                            filagre["CodigoEXSE"] = drexam["CodigoEXSE"].ToString();
                            filagre["Categoria"] = drexam["Categoria"].ToString();
                            filagre["Examen"] = drexam["Examen"].ToString();
                            filagre["Valor"] = drexam["Valor"].ToString();
                            filagre["Pvp"] = drexam["Pvp"].ToString();
                            filagre["Estado"] = drexam["Estado"].ToString();
                            dtbexamenes.Rows.Add(filagre);
                        }
                    }
                    ViewState["Examenes"] = dtbexamenes;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunClearObjects(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    dtbgrupoexamen.Clear();
                    ImgModificar.Enabled = false;
                    PnlGrupoExamenes.Visible = false;
                    PnlExamenes.Visible = false;
                    break;
            }
        }

        private void FunLlenarPrestador(TreeNode treenode)
        {
            try
            {
                int idCiudad = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 11;
                objparam[1] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dts != null && treenode != null)
                {
                    foreach (DataRow dr in dts.Tables[0].Rows)
                    {
                        idCiudad = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = FunAgregarPrestadora(node, idCiudad);
                        treenode.ChildNodes.Add(node);
                    }
                    TrvPrestadoras.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private TreeNode FunAgregarPrestadora(TreeNode node, int idCiudad)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 1;
                objparam[1] = idCiudad;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dts != null && dts.Tables[0].Rows.Count > 0)
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
                Lblerror.Text = ex.ToString();
                return node;
            }
        }

        protected void TrvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    FunLlenarPrestador(e.Node);
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void TrvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                switch (node.Depth)
                {
                    case 2:
                        TrvPrestadoras.SelectedNode.Expand();
                        string[] pathPrestadora = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPRES"] = pathPrestadora[3].ToString();
                        FunClearObjects(0);
                        FunCargaMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPrestadora.Text))
                {
                    new Funciones().funShowJSMessage("Seleccione Prestadora..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtGrupoExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingres Nombre Grupo Exámenes..!", this);
                    return;
                }
                if (ViewState["GrupoExamen"] != null)
                {
                    dtbdatos = (DataTable)ViewState["GrupoExamen"];
                    if (dtbdatos.Rows.Count > 0)
                        maxcodigo = dtbdatos.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else maxcodigo = 0;
                    result = dtbdatos.Select("GrupoExamen='" + TxtGrupoExamen.Text.Trim().ToUpper() + "'").FirstOrDefault();
                    if (result != null) lexiste = true;
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Grupo Examen ya se encuentra Agreagaod..!", this);
                    return;
                }
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                filagre = dtbgrupoexamen.NewRow();
                filagre["Codigo"] = maxcodigo + 1;
                filagre["GrupoExamen"] = TxtGrupoExamen.Text.Trim().ToUpper();
                filagre["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                filagre["Estado"] = "Activo";
                filagre["ConExamen"] = "NO";
                dtbgrupoexamen.Rows.Add(filagre);
                dtbgrupoexamen.DefaultView.Sort = "GrupoExamen";
                ViewState["GrupoExamen"] = dtbgrupoexamen;
                GrdvDatos.DataSource = dtbgrupoexamen;
                GrdvDatos.DataBind();
                TxtGrupoExamen.Text = "";
                TxtDescripcion.Text = "";
                ImgModificar.Enabled = false;
                PnlGrupoExamenes.Visible = true;
                PnlExamenes.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lexiste = false;
                if (string.IsNullOrEmpty(TxtGrupoExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingres Nombre Grupo Exámenes..!", this);
                    return;
                }                
                if (ViewState["GrupoExamen"] != null)
                {
                    if(ViewState["GrupoAnterior"].ToString()!=TxtGrupoExamen.Text.Trim().ToUpper())
                    {
                        dtbdatos = (DataTable)ViewState["GrupoExamen"];
                        result = dtbdatos.Select("GrupoExamen='" + TxtGrupoExamen.Text.Trim().ToUpper() + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Grupo Examen ya se encuentra Agreagaod..!", this);
                    return;
                }
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                result = dtbgrupoexamen.Select("Codigo='" + ViewState["CodigoGREX"].ToString() + "'").FirstOrDefault();
                result["GrupoExamen"] = TxtGrupoExamen.Text.Trim().ToUpper();
                result["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                GrdvDatos.DataSource = dtbgrupoexamen;
                GrdvDatos.DataBind();
                ImgModificar.Enabled = false;
                PnlExamenes.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgExamenes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoGREX"] = codigo;
                dtbexamenes = (DataTable)ViewState["Examenes"];
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoGREX='" + codigo + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoGREX"] = fila["CodigoGREX"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Valor"] = fila["Valor"].ToString();
                    filagretem["Pvp"] = fila["Pvp"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbexamenestmp.Rows.Add(filagretem);
                }
                dtbexamenestmp.DefaultView.Sort = "Examen";
                dtbexamenestmp = dtbexamenestmp.DefaultView.ToTable();
                GrdvExamenes.DataSource = dtbexamenestmp;
                GrdvExamenes.DataBind();
                PnlExamenes.Visible = true;
                ImgModificar.Enabled = false;
                TxtGrupoExamen.Text = "";
                TxtDescripcion.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                //Buscar si no existe Efecto agregado
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("CodigoGREX='" + codigo + "'").FirstOrDefault();
                if (result == null)
                {
                    PnlExamenes.Visible = false;
                    dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                    result = dtbgrupoexamen.Select("Codigo='" + codigo + "'").FirstOrDefault();
                    result.Delete();
                    dtbgrupoexamen.AcceptChanges();
                    ViewState["GrupoExamen"] = dtbgrupoexamen;
                    GrdvDatos.DataSource = dtbgrupoexamen;
                    GrdvDatos.DataBind();
                }
                else new Funciones().funShowJSMessage("Elimine antes Examenes asociados..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkestado = (CheckBox)(gvRow.Cells[2].FindControl("ChkEstado"));
                codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                result = dtbgrupoexamen.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result["Estado"] = chkestado.Checked ? "Activo" : "Inactivo";
                dtbgrupoexamen.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccGrupo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                PnlExamenes.Visible = false;
                ImgModificar.Enabled = true;
                codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();                
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                result = dtbgrupoexamen.Select("Codigo='" + codigo + "'").FirstOrDefault();
                TxtGrupoExamen.Text = result["GrupoExamen"].ToString();
                ViewState["GrupoAnterior"] = result["GrupoExamen"].ToString();
                ViewState["CodigoGREX"] = codigo;
                TxtDescripcion.Text = result["Descripcion"].ToString();
                PnlExamenes.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lexiste = false;
                if (DdlExamen.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Examen..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtPvp.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Pvp..!", this);
                    return;
                }
                if (TxtPvp.Text.Trim() == "0" || TxtPvp.Text.Trim() == "0.0" || TxtPvp.Text.Trim() == "0.00")
                {
                    new Funciones().funShowJSMessage("Ingrese Pvp..!", this);
                    return;
                }
                if (ViewState["Examenes"] != null)
                {
                    if (DdlExamen.SelectedValue != ViewState["CodiExeAnterior"].ToString())
                    {
                        dtbexamenes = (DataTable)ViewState["Examenes"];
                        result = dtbexamenes.Select("CodigoGREX='" + ViewState["CodigoGREX"].ToString() +
                            "' and Codigo='" + DdlExamen.SelectedValue + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Examen ya se encuentra Agregado..!", this);
                    return;
                }
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + ViewState["CodigoExamen"].ToString() + "'").FirstOrDefault();
                result["CodigoEXSE"] = DdlExamen.SelectedValue;
                result["Examen"] = DdlExamen.SelectedItem.ToString();
                result["Valor"] = TxtValor.Text;
                result["Categoria"] = TxtCategoria.Text.Trim();
                result["Pvp"] = TxtPvp.Text;
                ViewState["Examenes"] = dtbexamenes;
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoGREX='" + ViewState["CodigoGREX"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoGREX"] = fila["CodigoGREX"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Valor"] = fila["Valor"].ToString();
                    filagretem["Pvp"] = fila["Pvp"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbexamenestmp.Rows.Add(filagretem);
                }
                dtbexamenestmp.DefaultView.Sort = "Examen";
                dtbexamenestmp = dtbexamenestmp.DefaultView.ToTable();
                GrdvExamenes.DataSource = dtbexamenestmp;
                GrdvExamenes.DataBind();
                DdlExamen.SelectedValue = "0";
                TxtCategoria.Text = "";
                TxtSubCategoria.Text = "";
                TxtValor.Text = "0.00";
                TxtPvp.Text = "0.00";
                ImgModificar.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccExa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvExamenes.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvExamenes.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ImgModExamen.Enabled = true;
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                codigoexeanterior = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + codigo + "'").FirstOrDefault();
                DdlExamen.SelectedValue = result["CodigoEXSE"].ToString();
                TxtCategoria.Text = result["Categoria"].ToString();
                TxtValor.Text = result["Valor"].ToString();
                TxtPvp.Text = result["Pvp"].ToString();
                ViewState["CodiExeAnterior"] = result["CodigoEXSE"].ToString();
                ViewState["CodigoExamen"] = codigo;
                dtbgrupoexamen.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlExamen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(DdlExamen.SelectedValue);
                objparam[1] = "";
                objparam[2] = 138;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    TxtCategoria.Text = dts.Tables[0].Rows[0]["Categoria"].ToString();
                    TxtSubCategoria.Text = dts.Tables[0].Rows[0]["SubCategoria"].ToString();
                    TxtValor.Text = dts.Tables[0].Rows[0]["Valor"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstExamen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkestado = (CheckBox)(gvRow.Cells[2].FindControl("ChkEstExamen"));
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result["Estado"] = chkestado.Checked ? "Activo" : "Inactivo";
                dtbexamenes.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvExamenes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkestado = (CheckBox)(e.Row.Cells[4].FindControl("ChkEstExamen"));
                    imgeliminar = (ImageButton)(e.Row.Cells[5].FindControl("ImgDelExamen"));                    
                    estado = GrdvExamenes.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    codigo = GrdvExamenes.DataKeys[e.Row.RowIndex].Values["CodigoEXSE"].ToString();
                    if (estado == "Activo") chkestado.Checked = true;
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 20;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(ViewState["CodigoGREX"].ToString());
                    objparam[7] = int.Parse(codigo);
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dtx = new Conexion(2, "").FunConsultaDatos1(objparam);
                    if (dtx.Tables[0].Rows.Count > 0)
                    {
                        imgeliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        imgeliminar.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlExamen.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Examen..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtPvp.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Pvp..!", this);
                    return;
                }
                if (TxtPvp.Text.Trim() == "0" || TxtPvp.Text.Trim() == "0.0" || TxtPvp.Text.Trim() == "0.00")
                {
                    new Funciones().funShowJSMessage("Ingrese Pvp..!", this);
                    return;
                }
                if (ViewState["Examenes"] != null)
                {
                    dtbexamenes = (DataTable)ViewState["Examenes"];
                    if (dtbexamenes.Rows.Count > 0)
                        maxcodigo = dtbexamenes.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else maxcodigo = 0;
                    result = dtbexamenes.Select("CodigoGREX='" + ViewState["CodigoGREX"].ToString() +
                        "' and CodigoEXSE='" + DdlExamen.SelectedValue + "'").FirstOrDefault();
                    if (result != null) lexiste = true;
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Examen ya se encuentra Agregado..!", this);
                    return;
                }

                dtbexamenes = (DataTable)ViewState["Examenes"];
                filagre = dtbexamenes.NewRow();
                filagre["Codigo"] = maxcodigo + 1;
                filagre["CodigoGREX"] = ViewState["CodigoGREX"].ToString();
                filagre["CodigoEXSE"] = DdlExamen.SelectedValue;
                filagre["Categoria"] = TxtCategoria.Text.Trim();
                filagre["Examen"] = DdlExamen.SelectedItem.ToString();
                filagre["Valor"] = TxtValor.Text;
                filagre["Pvp"] = TxtPvp.Text;
                filagre["Estado"] = "Activo";
                dtbexamenes.Rows.Add(filagre);
                dtbexamenes.DefaultView.Sort = "Examen";
                ViewState["Examenes"] = dtbexamenes;
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoGREX='" + ViewState["CodigoGREX"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoGREX"] = fila["CodigoGREX"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Valor"] = fila["Valor"].ToString();
                    filagretem["Pvp"] = fila["Pvp"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbexamenestmp.Rows.Add(filagretem);
                }
                dtbexamenestmp.DefaultView.Sort = "Examen";
                dtbexamenestmp = dtbexamenestmp.DefaultView.ToTable();
                GrdvExamenes.DataSource = dtbexamenestmp;
                GrdvExamenes.DataBind();
                DdlExamen.SelectedValue = "0";
                TxtCategoria.Text = "";
                TxtSubCategoria.Text = "";
                TxtValor.Text = "0.00";
                TxtPvp.Text = "0.00";
                ImgModificar.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                //Buscar si no existe Efecto agregado
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                dtbexamenes.AcceptChanges();
                ViewState["Examenes"] = dtbexamenes;
                GrdvExamenes.DataSource = dtbexamenes;
                GrdvExamenes.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    imgexamen = (ImageButton)(e.Row.Cells[1].FindControl("ImgExamenes"));
                    imgeliminar = (ImageButton)(e.Row.Cells[3].FindControl("ImgEliminar"));
                    chkestado = (CheckBox)(e.Row.Cells[2].FindControl("ChkEstado"));
                    estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    codigo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString();
                    conexamen = GrdvDatos.DataKeys[e.Row.RowIndex].Values["ConExamen"].ToString();
                    if (estado == "Activo") chkestado.Checked = true;
                    if (conexamen == "SI") imgexamen.ImageUrl = "~/Botones/notepadcolor.png";
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 19;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(ViewState["CodigoPRES"].ToString());
                    objparam[7] = int.Parse(codigo);
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dtx = new Conexion(2, "").FunConsultaDatos1(objparam);
                    if (dtx.Tables[0].Rows.Count > 0)
                    {
                        imgeliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        imgeliminar.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["CodigoPRES"].ToString() == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Prestadora..!", this);
                    return;
                }
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                if (dtbgrupoexamen.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Ingrese al menos un Grupo de Examen..!", this);
                    return;
                }
                else
                {
                    dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                    dtbexamenes = (DataTable)ViewState["Examenes"];
                    Array.Resize(ref objparam, 22);                    
                    objparam[1] = ViewState["CodigoPRES"].ToString();
                    objparam[3] = 0;
                    objparam[4] = 0;
                    objparam[8] = "0";
                    objparam[9] = "0";
                    objparam[20] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[21] = Session["MachineName"].ToString();
                    foreach (DataRow drGrupo in dtbgrupoexamen.Rows)
                    {
                        objparam[0] = 0;
                        objparam[2] = drGrupo["Codigo"].ToString();
                        objparam[5] = drGrupo["GrupoExamen"].ToString();
                        objparam[6] = drGrupo["Descripcion"].ToString();
                        objparam[7] = drGrupo["Estado"].ToString();
                        objparam[10] = "";
                        objparam[11] = "";
                        objparam[12] = "";
                        objparam[13] = "";
                        objparam[14] = "";
                        objparam[15] = 0;
                        objparam[16] = 0;
                        objparam[17] = 0;
                        objparam[18] = 0;
                        objparam[19] = 0;
                        dts = new Conexion(2, "").FunInsertGrupoExamen(objparam);
                        if (dts != null || dts.Tables[0].Rows.Count > 0)
                        {
                            codigogrex = int.Parse(dts.Tables[0].Rows[0]["CodigoGREX"].ToString());
                            drexamenes = dtbexamenes.Select("CodigoGREX='" + drGrupo["Codigo"].ToString() + "'");
                            foreach (DataRow drExamen in drexamenes)
                            {
                                objparam[0] = 1;
                                objparam[2] = codigogrex;
                                objparam[3] = drExamen["Codigo"];
                                objparam[4] = drExamen["CodigoEXSE"];
                                objparam[7] = drExamen["Estado"];
                                objparam[8] = drExamen["Valor"];
                                objparam[9] = drExamen["Pvp"];
                                dtx = new Conexion(2, "").FunInsertGrupoExamen(objparam);
                            }
                        }
                    }
                    response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(response, false);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Matenedor/FrmDetalle.aspx");
        }
        #endregion
    }
}