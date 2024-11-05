using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmNuevoExamenCliente : Page
    {
        #region Variables
        Object[] Objparam = new Object[1];
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbgrupoexamen = new DataTable();
        DataTable dtbvariables = new DataTable();
        DataTable dtbvariablestmp = new DataTable();
        DataTable dtbexamenes = new DataTable();
        DataTable dtbexamenestmp = new DataTable();
        int codigoclus = 0, codigocamp = 0, maxcodigo = 0, codigoexgc = 0, codvarexa = 0;
        DataTable dtbdatos = new DataTable();
        DataRow result, filagre, filagretem;
        DataRow[] drtemp, drvariables, drexamenes;
        DataTable tblbuscar = new DataTable();
        bool lexiste;
        ImageButton imgvariables;
        ImageButton imgexamen;
        ImageButton imgeliminar;
        CheckBox chkestado;
        Object[] objparam = new Object[1];
        string estado, conexamen, convariables, textoValor, codigo, codigoexeanterior, response;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                //TxtMonto.Attributes.Add("onchange", "ValidarDecimales();");
                TxtValorExamen.Attributes.Add("onchange", "ValidarDecimales();");
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Configuración Clientes - Grupo Exámenes";
                    dtbgrupoexamen.Columns.Add("Codigo");
                    dtbgrupoexamen.Columns.Add("CodigoPROD");
                    dtbgrupoexamen.Columns.Add("GrupoExamen");
                    dtbgrupoexamen.Columns.Add("Observacion");
                    dtbgrupoexamen.Columns.Add("Estado");
                    dtbgrupoexamen.Columns.Add("ConVariables");
                    dtbgrupoexamen.Columns.Add("ConExamen");
                    ViewState["GrupoExamen"] = dtbgrupoexamen;

                    dtbvariables.Columns.Add("Codigo");
                    dtbvariables.Columns.Add("CodigoEXGC");
                    dtbvariables.Columns.Add("Campo");
                    dtbvariables.Columns.Add("Field");
                    dtbvariables.Columns.Add("Operador");
                    dtbvariables.Columns.Add("Valor");
                    dtbvariables.Columns.Add("Estado");
                    ViewState["Variables"] = dtbvariables;
                    ViewState["VariablesTmp"] = dtbvariables.Copy();

                    dtbexamenes.Columns.Add("Codigo");
                    dtbexamenes.Columns.Add("CodigoEXGC");
                    dtbexamenes.Columns.Add("CodigoEXSE");
                    dtbexamenes.Columns.Add("Categoria");
                    dtbexamenes.Columns.Add("Examen");
                    dtbexamenes.Columns.Add("Costo");
                    dtbexamenes.Columns.Add("Pvp");
                    dtbexamenes.Columns.Add("Estado");
                    ViewState["Examenes"] = dtbexamenes;
                    ViewState["ExamenesTmp"] = dtbexamenes.Copy();

                    FunCargarCombos(0);
                    FunCargarCombos(1);
                    FunCargarCombos(2);
                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::MENSAJE::", Request["MensajeRetornado"].ToString());
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                dtbgrupoexamen.Columns.Add("Codigo");
                dtbgrupoexamen.Columns.Add("CodigoPROD");
                dtbgrupoexamen.Columns.Add("GrupoExamen");
                dtbgrupoexamen.Columns.Add("Observacion");
                dtbgrupoexamen.Columns.Add("Estado");
                dtbgrupoexamen.Columns.Add("ConVariables");
                dtbgrupoexamen.Columns.Add("ConExamen");
                ViewState["GrupoExamen"] = dtbgrupoexamen;

                dtbvariables.Columns.Add("Codigo");
                dtbvariables.Columns.Add("CodigoEXGC");
                dtbvariables.Columns.Add("Campo");
                dtbvariables.Columns.Add("Field");
                dtbvariables.Columns.Add("Operador");
                dtbvariables.Columns.Add("Valor");
                dtbvariables.Columns.Add("Estado");
                ViewState["Variables"] = dtbvariables;
                ViewState["VariablesTmp"] = dtbvariables.Copy();

                dtbexamenes.Columns.Add("Codigo");
                dtbexamenes.Columns.Add("CodigoEXGC");
                dtbexamenes.Columns.Add("CodigoEXSE");
                dtbexamenes.Columns.Add("Categoria");
                dtbexamenes.Columns.Add("Examen");
                dtbexamenes.Columns.Add("Costo");
                dtbexamenes.Columns.Add("Pvp");
                dtbexamenes.Columns.Add("Estado");
                ViewState["Examenes"] = dtbexamenes;
                ViewState["Examenestmp"] = dtbexamenes.Copy();

                dtbgrupoexamen.Clear();
                dtbvariables.Clear();
                dtbexamenes.Clear();

                TrGrupoExamen.Visible = false;
                TrVariables.Visible = false;
                TrExamenes.Visible = false;

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(DdlProducto.SelectedValue);
                objparam[1] = "";
                //objparam[2] = 148;
                //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                //TxtMonto.Text = dts.Tables[0].Rows[0]["Costo"].ToString().Replace(",",".");

                objparam[2] = 135;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0) TrGrupoExamen.Visible = true;
                ViewState["GrupoExamen"] = dts.Tables[0];
                GrdvGrupoExamen.DataSource = dts;
                GrdvGrupoExamen.DataBind();
                dtbexamenes = (DataTable)ViewState["Examenes"];
                Array.Resize(ref objparam, 3);
                objparam[1] = "";
                objparam[2] = 136;                
                foreach (DataRow drfila in dts.Tables[0].Rows)
                {
                    objparam[0] = int.Parse(drfila["Codigo"].ToString());
                    dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    foreach (DataRow drvariable in dtx.Tables[0].Rows)
                    {
                        filagre = dtbvariables.NewRow();
                        filagre["Codigo"] = drvariable["Codigo"].ToString();
                        filagre["CodigoEXGC"] = drfila["Codigo"].ToString();
                        filagre["Campo"] = drvariable["Campo"].ToString();
                        filagre["Field"] = drvariable["Field"].ToString();
                        filagre["Operador"] = drvariable["Operador"].ToString();
                        filagre["Valor"] = drvariable["Valor"].ToString();
                        filagre["Estado"] = drvariable["Estado"].ToString();
                        dtbvariables.Rows.Add(filagre);
                    }
                    ViewState["Variables"] = dtbvariables;
                    foreach (DataRow drexamen in dtx.Tables[1].Rows)
                    {
                        filagre = dtbexamenes.NewRow();
                        filagre["Codigo"] = drexamen["Codigo"].ToString();
                        filagre["CodigoEXGC"] = drfila["Codigo"].ToString();
                        filagre["CodigoEXSE"] = drexamen["CodigoEXSE"].ToString();
                        filagre["Categoria"] = drexamen["Categoria"].ToString();
                        filagre["Examen"] = drexamen["Examen"].ToString();
                        filagre["Costo"] = drexamen["Costo"].ToString();
                        filagre["Pvp"] = drexamen["Pvp"].ToString();
                        filagre["Estado"] = drexamen["Estado"].ToString();
                        dtbexamenes.Rows.Add(filagre);
                    }
                    ViewState["Examenes"] = dtbexamenes;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref Objparam, 3);
                    Objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                    Objparam[1] = "";
                    Objparam[2] = 145;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
                    codigoclus = int.Parse(dts.Tables[0].Rows[0]["CodigoCLUS"].ToString());
                    codigocamp = int.Parse(dts.Tables[0].Rows[0]["CodigoCAMP"].ToString());

                    Objparam[2] = 146;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
                    DdlCampos.DataSource = dts;
                    DdlCampos.DataTextField = "Descripcion";
                    DdlCampos.DataValueField = "Codigo";
                    DdlCampos.DataBind();

                    Array.Resize(ref Objparam, 11);
                    Objparam[0] = 14;
                    Objparam[1] = "";
                    Objparam[2] = "";
                    Objparam[3] = "";
                    Objparam[4] = "";
                    Objparam[5] = "";
                    Objparam[6] = codigocamp;
                    Objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                    Objparam[8] = 0;
                    Objparam[9] = 0;
                    Objparam[10] = 0;
                    dts = new Conexion(2, "").FunConsultaDatos1(Objparam);
                    DdlProducto.DataSource = dts;
                    DdlProducto.DataTextField = "Descripcion";
                    DdlProducto.DataValueField = "Codigo";
                    DdlProducto.DataBind();
                    break;
                case 1:
                    List<KeyValuePair<string, string>> listOper = new List<KeyValuePair<string, string>>();
                    listOper.Add(new KeyValuePair<string, string>("0", "--<Seleccion Operación>--"));
                    listOper.Add(new KeyValuePair<string, string>("=", "Igual (=)"));
                    listOper.Add(new KeyValuePair<string, string>(">", "Mayor que (>)"));
                    listOper.Add(new KeyValuePair<string, string>(">=", "Mayor Igual que (>=)"));
                    listOper.Add(new KeyValuePair<string, string>("<", "Menor que (<)"));
                    listOper.Add(new KeyValuePair<string, string>("<=", "Menor Igual que (<=)"));
                    //listOper.Add(new KeyValuePair<string, string>("like", "Contiene Caracteres (Like)"));
                    //listOper.Add(new KeyValuePair<string, string>("between", "Entre"));
                    DdlOperador.DataSource = listOper;
                    DdlOperador.DataTextField = "Value";
                    DdlOperador.DataValueField = "Key";
                    DdlOperador.DataBind();
                    break;
                case 2:
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

        private void FunSetearCampos(string campo)
        {
            try
            {
                TxtValor.Text = "";
                Array.Resize(ref Objparam, 3);
                Objparam[0] = 0;
                Objparam[1] = campo;
                Objparam[2] = 147;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
                ViewState["Tipo"] = dts.Tables[0].Rows[0]["Tipo"].ToString();
                TxtValor.Attributes.Clear();
                switch (ViewState["Tipo"].ToString())
                {
                    case "date":
                    case "datetime":
                        CalendarExtender calExtender = new CalendarExtender();
                        calExtender.Format = "MM/dd/yyyy";
                        calExtender.TargetControlID = TxtValor.ID;
                        PlaceTxt.Controls.Add(calExtender);
                        break;
                    case "int":
                    case "smallint":
                    case "tinyint":
                        FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
                        filter.FilterType = FilterTypes.Numbers;
                        filter.TargetControlID = TxtValor.ID;
                        PlaceTxt.Controls.Add(filter);
                        break;
                    case "decimal":
                    case "numeric":
                    case "float":
                    case "money":
                    case "real":
                        TxtValor.Attributes.Add("onkeypress", "return NumeroDecimal(this.form.txtValor, event)");
                        TxtValor.Attributes.Add("onchange", "ValidarDecimales();");
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private bool FunValidarCondiciones()
        {
            if (DdlCampos.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Debe seleccionar un Campo para la condición", this);
                return false;
            }
            if (DdlOperador.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Debe seleccionar tipo de operación", this);
                return false;
            }
            if (TxtValor.Text == "")
            {
                new Funciones().funShowJSMessage("Debe ingresar un valor de comparación para la condición", this);
                return false;
            }
            if (ViewState["Variables"] != null)
            {
                dtbvariables = (DataTable)ViewState["Variables"];
                result = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "' and Campo='"+
                    DdlCampos.SelectedItem.ToString() + "' and Operador='" + DdlOperador.SelectedItem.ToString() + "'").FirstOrDefault();
                lexiste = result != null ? true : false;
            }            
            lexiste = result != null ? true : false;
            if (lexiste)
            {
                new Funciones().funShowJSMessage("Ya existe un registro con esta condición", this);
                return false;
            }
            //FunSetearCampos(DdlCampos.SelectedValue);
            switch (ViewState["Tipo"].ToString())
            {
                case "numeric":
                case "double":
                case "decimal":
                case "int":
                    if (DdlOperador.SelectedValue.ToString() == "like")
                    {
                        new Funciones().funShowJSMessage("No puede utilizar la operacion like para datos numéricos", this);
                        return false;
                    }
                    break;
                case "date":
                case "smalldatetime":
                case "datetime":
                    if (DdlOperador.SelectedValue.ToString() == "like")
                    {
                        new Funciones().funShowJSMessage("No puede utilizar la operacion like para datos tipo fecha", this);
                        return false;
                    }
                    break;
            }
            return true;
        }
        #endregion

        #region Botones y Eventos
        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlProducto.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Producto..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtGrupoExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingres Nombre Grupo Exámenes..!", this);
                    return;
                }
                //if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                //{
                //    new Funciones().funShowJSMessage("Ingrese Monto del Producto..!", this);
                //    return;
                //}
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
                    new Funciones().funShowJSMessage("Grupo Examen ya se encuentra Agregado..!", this);
                    return;
                }
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                filagre = dtbgrupoexamen.NewRow();
                filagre["Codigo"] = maxcodigo + 1;
                filagre["CodigoPROD"] = DdlProducto.SelectedValue;
                filagre["GrupoExamen"] = TxtGrupoExamen.Text.Trim().ToUpper();
                filagre["Observacion"] = TxtObservacion.Text.Trim().ToUpper();
                filagre["Estado"] = "Activo";
                filagre["ConVariables"] = "NO";
                filagre["ConExamen"] = "NO";
                dtbgrupoexamen.Rows.Add(filagre);
                dtbgrupoexamen.DefaultView.Sort = "GrupoExamen";
                ViewState["GrupoExamen"] = dtbgrupoexamen;
                GrdvGrupoExamen.DataSource = dtbgrupoexamen;
                GrdvGrupoExamen.DataBind();
                TxtGrupoExamen.Text = "";
                TxtObservacion.Text = "";
                ImgModificar.Enabled = false;
                TrGrupoExamen.Visible = true;
                TrVariables.Visible = false;
                TrExamenes.Visible = false;
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
                if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                {
                    new Funciones().funShowJSMessage("Ingrese Monto del Producto..!", this);
                    return;
                }
                if (ViewState["GrupoExamen"] != null)
                {
                    dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                    if (ViewState["GrupoAnterior"].ToString() != TxtGrupoExamen.Text.Trim().ToUpper())
                    {
                        result = dtbgrupoexamen.Select("GrupoExamen='" + TxtGrupoExamen.Text.Trim().ToUpper() + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Grupo Examen ya se encuentra Agregado..!", this);
                    return;
                }
                result = dtbgrupoexamen.Select("Codigo='" + ViewState["CodigoEXGC"].ToString() + "'").FirstOrDefault();
                result["GrupoExamen"] = TxtGrupoExamen.Text.Trim().ToUpper();
                result["Observacion"] = TxtObservacion.Text.Trim().ToUpper();
                result["Estado"] = ChkEstadoGrupo.Checked ? "Activo" : "Inactivo";
                dtbgrupoexamen.AcceptChanges();
                ViewState["GrupoExamen"] = dtbgrupoexamen;
                GrdvGrupoExamen.DataSource = dtbgrupoexamen;
                GrdvGrupoExamen.DataBind();
                TxtGrupoExamen.Text = "";
                TxtObservacion.Text = "";
                ImgModificar.Enabled = false;
                ImgAgregar.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgVariables_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvGrupoExamen.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvGrupoExamen.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Beige;
                codigo = GrdvGrupoExamen.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoEXGC"] = codigo;
                dtbvariables = (DataTable)ViewState["Variables"];
                dtbvariablestmp = (DataTable)ViewState["VariablesTmp"];
                dtbvariablestmp.Clear();
                drtemp = dtbvariables.Select("CodigoEXGC='" + codigo + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbvariablestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["Campo"] = fila["Campo"].ToString();
                    filagretem["Field"] = fila["Field"].ToString();
                    filagretem["Operador"] = fila["Operador"].ToString();
                    filagretem["Valor"] = fila["Valor"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbvariablestmp.Rows.Add(filagretem);
                }
                dtbvariablestmp.DefaultView.Sort = "Campo";
                dtbvariablestmp = dtbvariablestmp.DefaultView.ToTable();
                GrdvVariables.DataSource = dtbvariablestmp;
                GrdvVariables.DataBind();
                TrVariables.Visible = true;
                TrExamenes.Visible = false;
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
                foreach (GridViewRow fr in GrdvGrupoExamen.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }
                GrdvGrupoExamen.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Beige;
                codigo = GrdvGrupoExamen.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoEXGC"] = codigo;
                dtbexamenes = (DataTable)ViewState["Examenes"];
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoEXGC='" + codigo + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Costo"] = fila["Costo"].ToString();
                    filagretem["Pvp"] = fila["Pvp"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbexamenestmp.Rows.Add(filagretem);
                }
                dtbexamenestmp.DefaultView.Sort = "Examen";
                dtbexamenestmp = dtbexamenestmp.DefaultView.ToTable();
                GrdvExamenes.DataSource = dtbexamenestmp;
                GrdvExamenes.DataBind();
                TrVariables.Visible = false;
                TrExamenes.Visible = true;
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
                foreach (GridViewRow fr in GrdvGrupoExamen.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }
                GrdvGrupoExamen.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                TrVariables.Visible = false;
                TrExamenes.Visible = false;
                ImgModificar.Enabled = true;
                codigo = GrdvGrupoExamen.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                result = dtbgrupoexamen.Select("Codigo='" + codigo + "'").FirstOrDefault();
                TxtGrupoExamen.Text = result["GrupoExamen"].ToString();
                TxtObservacion.Text = result["Observacion"].ToString();
                ViewState["GrupoAnterior"] = result["GrupoExamen"].ToString();
                ViewState["CodigoEXGC"] = codigo;                
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkestado = (CheckBox)(gvRow.Cells[4].FindControl("ChkEstado"));
                codigo = GrdvGrupoExamen.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
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

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = GrdvGrupoExamen.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                //Buscar si no existe Efecto agregado
                dtbvariables = (DataTable)ViewState["Variables"];
                result = dtbvariables.Select("CodigoEXGC='" + codigo + "'").FirstOrDefault();
                if (result != null)
                {
                    new Funciones().funShowJSMessage("Elimine antes Variables..!", this);
                    return;
                }
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("CodigoEXGC='" + codigo + "'").FirstOrDefault();
                if (result != null)
                {
                    new Funciones().funShowJSMessage("Elimine antes Examenes asociados..!", this);
                    return;
                }
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                result = dtbgrupoexamen.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                dtbgrupoexamen.AcceptChanges();
                ViewState["GrupoExamen"] = dtbgrupoexamen;
                GrdvGrupoExamen.DataSource = dtbgrupoexamen;
                GrdvGrupoExamen.DataBind();
                if (dtbgrupoexamen.Rows.Count == 0)
                {
                    TrGrupoExamen.Visible = false;
                    TxtMonto.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void GrdvGrupoExamen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    imgvariables = (ImageButton)(e.Row.Cells[2].FindControl("ImgVariables"));
                    imgexamen = (ImageButton)(e.Row.Cells[2].FindControl("ImgExamenes"));
                    chkestado = (CheckBox)(e.Row.Cells[3].FindControl("ChkEstado"));
                    imgeliminar = (ImageButton)(e.Row.Cells[4].FindControl("ImgEliminar"));                    
                    estado = GrdvGrupoExamen.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    codigo = GrdvGrupoExamen.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString();
                    conexamen = GrdvGrupoExamen.DataKeys[e.Row.RowIndex].Values["ConExamen"].ToString();
                    convariables = GrdvGrupoExamen.DataKeys[e.Row.RowIndex].Values["ConVariables"].ToString();
                    if (estado == "Activo") chkestado.Checked = true;
                    if (conexamen == "SI") imgexamen.ImageUrl = "~/Botones/notepadcolor.png";
                    if (convariables == "SI") imgvariables.ImageUrl = "~/Botones/variablescolor.png";
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 19;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(DdlProducto.SelectedValue);
                    objparam[7] = 0;
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

        protected void ImgAddCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lexiste = false;
                if (FunValidarCondiciones())
                {
                    textoValor = TxtValor.Text;
                    switch (ViewState["Tipo"].ToString())
                    {
                        case "date":
                        case "datetime":
                        case "varchar":
                        case "char":
                            if (DdlOperador.SelectedValue == "like") textoValor = "'" + TxtValor.Text + "%'";
                            //else textoValor = "'" + TxtValor.Text + "'";
                            break;
                    }
                    if (ViewState["Variables"] != null)
                    {
                        dtbvariables = (DataTable)ViewState["Variables"];
                        if (dtbvariables.Rows.Count > 0)
                            maxcodigo = dtbvariables.AsEnumerable()
                                .Max(row => int.Parse((string)row["Codigo"]));
                        else maxcodigo = 0;
                        result = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() +
                            "' and Field='" + DdlCampos.SelectedValue + "' and Operador='" + DdlOperador.SelectedValue + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                    if (lexiste)
                    {
                        new Funciones().funShowJSMessage("Definición de Variable ya está Agregada..!", this);
                        return;
                    }
                    filagre = dtbvariables.NewRow();
                    filagre["Codigo"] = maxcodigo + 1;
                    filagre["CodigoEXGC"] = ViewState["CodigoEXGC"].ToString();
                    filagre["Campo"] = DdlCampos.SelectedItem.ToString();
                    filagre["Field"] = DdlCampos.SelectedValue;
                    filagre["Operador"] = DdlOperador.SelectedValue;
                    filagre["Valor"] = textoValor;
                    filagre["Estado"] = "Activo";
                    dtbvariables.Rows.Add(filagre);
                    dtbvariablestmp = (DataTable)ViewState["VariablesTmp"];
                    dtbvariablestmp.Clear();
                    drtemp = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                    foreach (DataRow fila in drtemp)
                    {
                        filagretem = dtbvariablestmp.NewRow();
                        filagretem["Codigo"] = fila["Codigo"].ToString();
                        filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                        filagretem["Campo"] = fila["Campo"].ToString();
                        filagretem["Field"] = fila["Field"].ToString();
                        filagretem["Operador"] = fila["Operador"].ToString();
                        filagretem["Valor"] = fila["Valor"].ToString();
                        filagretem["Estado"] = fila["Estado"].ToString();
                        dtbvariablestmp.Rows.Add(filagretem);
                    }
                    dtbvariablestmp.DefaultView.Sort = "Campo";
                    dtbvariablestmp = dtbvariablestmp.DefaultView.ToTable();
                    ViewState["Variables"] = dtbvariables;
                    GrdvVariables.DataSource = dtbvariablestmp;
                    GrdvVariables.DataBind();
                    DdlCampos.SelectedValue = "0";
                    DdlOperador.SelectedValue = "0";
                    TxtValor.Text = "";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstadoCampo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkestado = (CheckBox)(gvRow.Cells[3].FindControl("ChkEstadoCampo"));
                dtbvariables = (DataTable)ViewState["Variables"];
                codigo = GrdvVariables.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                result = dtbvariables.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result["Estado"] = chkestado.Checked ? "Activo" : "Inactivo";
                dtbvariables.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvVariables.Rows)
                {
                    fr.Cells[0].BackColor = Color.White;
                }
                GrdvVariables.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                codigo = GrdvVariables.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["CodigoVar"] = codigo;
                dtbvariables = (DataTable)ViewState["Variables"];
                result = dtbvariables.Select("Codigo='" + codigo + "'").FirstOrDefault();
                ViewState["Estado"] = result["Estado"].ToString();
                DdlCampos.SelectedValue = result["Field"].ToString();
                FunSetearCampos(DdlCampos.SelectedItem.ToString());
                DdlOperador.SelectedValue = result["Operador"].ToString();
                TxtValor.Text = result["Valor"].ToString().Replace("'", "");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgDelCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = GrdvVariables.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                Array.Resize(ref objparam, 11);
                objparam[0] = 22;
                objparam[1] = "";
                objparam[2] = "";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = int.Parse(ViewState["CodigoEXGC"].ToString());
                objparam[7] = codigo;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                new Conexion(2, "").FunConsultaDatos1(objparam);
                dtbvariables = (DataTable)ViewState["Variables"];
                result = dtbvariables.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                dtbvariables.AcceptChanges();
                dtbvariablestmp = (DataTable)ViewState["VariablesTmp"];
                dtbvariablestmp.Clear();
                drtemp = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbvariablestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["Campo"] = fila["Campo"].ToString();
                    filagretem["Field"] = fila["Field"].ToString();
                    filagretem["Operador"] = fila["Operador"].ToString();
                    filagretem["Valor"] = fila["Valor"].ToString();
                    filagretem["Estado"] = fila["Estado"].ToString();
                    dtbvariablestmp.Rows.Add(filagretem);
                }
                dtbvariablestmp.DefaultView.Sort = "Campo";
                dtbvariablestmp = dtbvariablestmp.DefaultView.ToTable();
                GrdvVariables.DataSource = dtbvariablestmp;
                GrdvVariables.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgAddExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lexiste = false;
                if (DdlExamen.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Examen..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtValorExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Costo Examen..!", this);
                    return;
                }
                if (TxtValorExamen.Text.Trim() == "0" || TxtValorExamen.Text.Trim() == "0.0" || TxtValorExamen.Text.Trim() == "0.00")
                {
                    new Funciones().funShowJSMessage("Ingrese Costo Examen..!", this);
                    return;
                }
                if (ViewState["Examenes"] != null)
                {
                    dtbexamenes = (DataTable)ViewState["Examenes"];
                    if (dtbexamenes.Rows.Count > 0)
                        maxcodigo = dtbexamenes.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else maxcodigo = 0;
                    result = dtbexamenes.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() +
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
                filagre["CodigoEXGC"] = ViewState["CodigoEXGC"].ToString();
                filagre["CodigoEXSE"] = DdlExamen.SelectedValue;
                filagre["Categoria"] = TxtCategoria.Text.Trim();
                filagre["Examen"] = DdlExamen.SelectedItem.ToString();
                filagre["Costo"] = TxtValorExamen.Text;
                filagre["Pvp"] = ViewState["Pvp"].ToString();
                filagre["Estado"] = "Activo";
                dtbexamenes.Rows.Add(filagre);
                dtbexamenes.DefaultView.Sort = "Examen";
                ViewState["Examenes"] = dtbexamenes;
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Costo"] = fila["Costo"].ToString();
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
                TxtValorExamen.Text = "0.00";
                ImgModExamen.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstadoGrupo_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoGrupo.Text = ChkEstadoGrupo.Checked ? "Activo" : "Inactivo";
        }

        protected void DdlCampos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlCampos.SelectedValue != "0")
            {
                FunSetearCampos(DdlCampos.SelectedItem.ToString());
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
                    ViewState["Pvp"] = dts.Tables[0].Rows[0]["Valor"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }

        }

        protected void DdlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargaMantenimiento();
        }

        protected void GrdvVariables_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkestado = (CheckBox)(e.Row.Cells[3].FindControl("ChkEstadoCampo"));
                    estado = GrdvVariables.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    if (estado == "Activo") chkestado.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (FunValidarCondiciones())
                {
                    dtbvariables = (DataTable)ViewState["Variables"];
                    result = dtbvariables.Select("Codigo='" + ViewState["CodigoVar"].ToString() + "'").FirstOrDefault();
                    result["Campo"] = DdlCampos.SelectedItem.ToString();
                    result["Field"] = DdlCampos.SelectedValue;
                    result["Operador"] = DdlOperador.SelectedValue;
                    result["Valor"] = TxtValor.Text.Trim();
                    dtbvariables.AcceptChanges();
                    ViewState["Variables"] = dtbvariables;
                    dtbvariablestmp = (DataTable)ViewState["VariablesTmp"];
                    dtbvariablestmp.Clear();
                    drtemp = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                    foreach (DataRow fila in drtemp)
                    {
                        filagretem = dtbvariablestmp.NewRow();
                        filagretem["Codigo"] = fila["Codigo"].ToString();
                        filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                        filagretem["Campo"] = fila["Campo"].ToString();
                        filagretem["Field"] = fila["Field"].ToString();
                        filagretem["Operador"] = fila["Operador"].ToString();
                        filagretem["Valor"] = fila["Valor"].ToString();
                        filagretem["Estado"] = fila["Estado"].ToString();
                        dtbvariablestmp.Rows.Add(filagretem);
                    }
                    dtbvariablestmp.DefaultView.Sort = "Campo";
                    dtbvariablestmp = dtbvariablestmp.DefaultView.ToTable();
                    ViewState["Variables"] = dtbvariables;
                    GrdvVariables.DataSource = dtbvariablestmp;
                    GrdvVariables.DataBind();
                    DdlCampos.SelectedValue = "0";
                    DdlOperador.SelectedValue = "0";
                    TxtValor.Text = "";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlCampos.SelectedValue != "0")
            {
                FunSetearCampos(DdlCampos.SelectedItem.ToString());
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
                if (string.IsNullOrEmpty(TxtValorExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Costo..!", this);
                    return;
                }
                if (TxtValorExamen.Text.Trim() == "0" || TxtValorExamen.Text.Trim() == "0.0" || TxtValorExamen.Text.Trim() == "0.00")
                {
                    new Funciones().funShowJSMessage("Ingrese Pvp..!", this);
                    return;
                }
                if (ViewState["Examenes"] != null)
                {
                    if (DdlExamen.SelectedValue != ViewState["CodiExeAnterior"].ToString())
                    {
                        dtbexamenes = (DataTable)ViewState["Examenes"];
                        result = dtbexamenes.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() +
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
                result["Categoria"] = TxtCategoria.Text.Trim();
                result["Costo"] = TxtValorExamen.Text;
                ViewState["Examenes"] = dtbexamenes;
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Costo"] = fila["Costo"].ToString();
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
                TxtValorExamen.Text = "0.00";
                ImgModExamen.Enabled = false;
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
                    fr.Cells[0].BackColor = Color.White;
                }
                GrdvExamenes.Rows[gvRow.RowIndex].Cells[0].BackColor = Color.Coral;
                ImgModExamen.Enabled = true;
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                codigoexeanterior = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + codigo + "'").FirstOrDefault();
                DdlExamen.SelectedValue = result["CodigoEXSE"].ToString();
                TxtCategoria.Text = result["Categoria"].ToString();
                TxtValorExamen.Text = result["Costo"].ToString();
                ViewState["CodiExeAnterior"] = codigo;
                ViewState["CodigoExamen"] = codigo;
                dtbgrupoexamen.AcceptChanges();
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

        protected void ImgDelExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                Array.Resize(ref objparam, 11);
                objparam[0] = 23;
                objparam[1] = "";
                objparam[2] = "";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = int.Parse(ViewState["CodigoEXGC"].ToString());
                objparam[7] = codigo;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                new Conexion(2, "").FunConsultaDatos1(objparam);
                //Buscar si no existe Efecto agregado
                dtbexamenes = (DataTable)ViewState["Examenes"];
                result = dtbexamenes.Select("Codigo='" + codigo + "'").FirstOrDefault();
                result.Delete();
                dtbexamenes.AcceptChanges();
                ViewState["Examenes"] = dtbexamenes;
                dtbexamenestmp = (DataTable)ViewState["ExamenesTmp"];
                dtbexamenestmp.Clear();
                drtemp = dtbexamenes.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "'");
                foreach (DataRow fila in drtemp)
                {
                    filagretem = dtbexamenestmp.NewRow();
                    filagretem["Codigo"] = fila["Codigo"].ToString();
                    filagretem["CodigoEXGC"] = fila["CodigoEXGC"].ToString();
                    filagretem["CodigoEXSE"] = fila["CodigoEXSE"].ToString();
                    filagretem["Categoria"] = fila["Categoria"].ToString();
                    filagretem["Examen"] = fila["Examen"].ToString();
                    filagretem["Costo"] = fila["Costo"].ToString();
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
                TxtValorExamen.Text = "0.00";
                ImgModExamen.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
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
                    objparam[6] = int.Parse(ViewState["CodigoEXGC"].ToString());
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
                dtbgrupoexamen = (DataTable)ViewState["GrupoExamen"];
                dtbvariables = (DataTable)ViewState["Variables"];
                dtbexamenes = (DataTable)ViewState["Examenes"];

                if (dtbgrupoexamen.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Ingrese Grupo de Examen..!", this);
                    return;
                }
                //if (dtbvariables.Rows.Count == 0)
                //{
                //    new Funciones().funShowJSMessage("Defina Variables de Configuración..!", this);
                //    return;
                //}
                if (dtbexamenes.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Ingrese al menos un Examen..!", this);
                    return;
                }
                Array.Resize(ref objparam, 28);
                objparam[6] = 0;
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = "";
                objparam[9] = 0;
                objparam[10] = "0";
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = "0";
                objparam[14] = "0";
                objparam[15] = "0";
                objparam[16] = "";
                objparam[17] = "";
                objparam[18] = "";
                objparam[19] = "";
                objparam[20] = "";
                objparam[21] = 0;
                objparam[22] = 0;
                objparam[23] = 0;
                objparam[24] = 0;
                objparam[25] = 0;
                objparam[26] = int.Parse(Session["usuCodigo"].ToString());
                objparam[27] = Session["MachineName"].ToString();
                foreach (DataRow drgrupo in dtbgrupoexamen.Rows)
                {
                    objparam[0] = 0;
                    objparam[1] = int.Parse(drgrupo["Codigo"].ToString());
                    objparam[2] = int.Parse(drgrupo["CodigoPROD"].ToString());
                    objparam[3] = drgrupo["GrupoExamen"].ToString();
                    objparam[4] = drgrupo["Observacion"].ToString();
                    objparam[5] = drgrupo["Estado"].ToString();
                    codigoexgc = new Conexion(2, "").FunNewGrupoExamenProducto(objparam);
                    drvariables = dtbvariables.Select("CodigoEXGC='" + drgrupo["Codigo"].ToString() + "'");
                    foreach (DataRow drvar in drvariables)
                    {
                        objparam[0] = 1;
                        objparam[2] = codigoexgc;
                        objparam[5] = drvar["Estado"].ToString();
                        objparam[6] = int.Parse(drvar["Codigo"].ToString());
                        objparam[7] = drvar["Campo"].ToString();
                        objparam[8] = drvar["Field"].ToString();
                        objparam[9] = drvar["Operador"].ToString();
                        objparam[10] = drvar["Valor"].ToString();
                        codvarexa = new Conexion(2, "").FunNewGrupoExamenProducto(objparam);
                    }
                    drexamenes = dtbexamenes.Select("CodigoEXGC='" + drgrupo["Codigo"].ToString() + "'");
                    foreach (var drexa in drexamenes)
                    {
                        objparam[0] = 2;
                        objparam[2] = codigoexgc;
                        objparam[5] = drexa["Estado"].ToString();
                        objparam[11] = int.Parse(drexa["Codigo"].ToString());
                        objparam[12] = int.Parse(drexa["CodigoEXSE"].ToString());
                        objparam[13] = drexa["Costo"].ToString();
                        objparam[14] = drexa["Pvp"].ToString();
                        codvarexa = new Conexion(2, "").FunNewGrupoExamenProducto(objparam);
                    }
                }
                response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(response, false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}