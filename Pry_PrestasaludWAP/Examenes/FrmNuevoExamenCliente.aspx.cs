using System;
using System.Collections.Generic;
using System.Data;
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

                if (!IsPostBack)
                   
                    if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                        {
                            Response.Redirect("~/Reload.html");
                            return;
                        }

                    if (!IsPostBack)
                    {
                        Lbltitulo.Text = "Gestión de Solicitudes y Exámenes";
                        CargarSolicitudesCliente();
                    }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        //protected void FunCargaMantenimiento()
        //{
        //    try
        //    {
        //        dtbgrupoexamen.Columns.Add("Codigo");
        //        dtbgrupoexamen.Columns.Add("CodigoPROD");
        //        dtbgrupoexamen.Columns.Add("GrupoExamen");
        //        dtbgrupoexamen.Columns.Add("Observacion");
        //        dtbgrupoexamen.Columns.Add("Estado");
        //        dtbgrupoexamen.Columns.Add("ConVariables");
        //        dtbgrupoexamen.Columns.Add("ConExamen");
        //        ViewState["GrupoExamen"] = dtbgrupoexamen;

        //        dtbvariables.Columns.Add("Codigo");
        //        dtbvariables.Columns.Add("CodigoEXGC");
        //        dtbvariables.Columns.Add("Campo");
        //        dtbvariables.Columns.Add("Field");
        //        dtbvariables.Columns.Add("Operador");
        //        dtbvariables.Columns.Add("Valor");
        //        dtbvariables.Columns.Add("Estado");
        //        ViewState["Variables"] = dtbvariables;
        //        ViewState["VariablesTmp"] = dtbvariables.Copy();

        //        dtbexamenes.Columns.Add("Codigo");
        //        dtbexamenes.Columns.Add("CodigoEXGC");
        //        dtbexamenes.Columns.Add("CodigoEXSE");
        //        dtbexamenes.Columns.Add("Categoria");
        //        dtbexamenes.Columns.Add("Examen");
        //        dtbexamenes.Columns.Add("Costo");
        //        dtbexamenes.Columns.Add("Pvp");
        //        dtbexamenes.Columns.Add("Estado");
        //        ViewState["Examenes"] = dtbexamenes;
        //        ViewState["Examenestmp"] = dtbexamenes.Copy();

        //        dtbgrupoexamen.Clear();
        //        dtbvariables.Clear();
        //        dtbexamenes.Clear();

        //        //TrGrupoExamen.Visible = false;
        //        //TrVariables.Visible = false;
        //        //TrExamenes.Visible = false;

        //        Array.Resize(ref objparam, 3);
        //        objparam[0] = /*int.Parse(DdlProducto.SelectedValue);*/
        //        objparam[1] = "";
        //        //objparam[2] = 148;
        //        //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //        //TxtMonto.Text = dts.Tables[0].Rows[0]["Costo"].ToString().Replace(",",".");

        //        objparam[2] = 135;
        //        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //        //if (dts.Tables[0].Rows.Count > 0) TrGrupoExamen.Visible = true;
        //        ViewState["GrupoExamen"] = dts.Tables[0];
        //        GrdvGrupoExamen.DataSource = dts;
        //        GrdvGrupoExamen.DataBind();
        //        dtbexamenes = (DataTable)ViewState["Examenes"];
        //        Array.Resize(ref objparam, 3);
        //        objparam[1] = "";
        //        objparam[2] = 136;                
        //        foreach (DataRow drfila in dts.Tables[0].Rows)
        //        {
        //            objparam[0] = int.Parse(drfila["Codigo"].ToString());
        //            dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //            foreach (DataRow drvariable in dtx.Tables[0].Rows)
        //            {
        //                filagre = dtbvariables.NewRow();
        //                filagre["Codigo"] = drvariable["Codigo"].ToString();
        //                filagre["CodigoEXGC"] = drfila["Codigo"].ToString();
        //                filagre["Campo"] = drvariable["Campo"].ToString();
        //                filagre["Field"] = drvariable["Field"].ToString();
        //                filagre["Operador"] = drvariable["Operador"].ToString();
        //                filagre["Valor"] = drvariable["Valor"].ToString();
        //                filagre["Estado"] = drvariable["Estado"].ToString();
        //                dtbvariables.Rows.Add(filagre);
        //            }
        //            ViewState["Variables"] = dtbvariables;
        //            foreach (DataRow drexamen in dtx.Tables[1].Rows)
        //            {
        //                filagre = dtbexamenes.NewRow();
        //                filagre["Codigo"] = drexamen["Codigo"].ToString();
        //                filagre["CodigoEXGC"] = drfila["Codigo"].ToString();
        //                filagre["CodigoEXSE"] = drexamen["CodigoEXSE"].ToString();
        //                filagre["Categoria"] = drexamen["Categoria"].ToString();
        //                filagre["Examen"] = drexamen["Examen"].ToString();
        //                filagre["Costo"] = drexamen["Costo"].ToString();
        //                filagre["Pvp"] = drexamen["Pvp"].ToString();
        //                filagre["Estado"] = drexamen["Estado"].ToString();
        //                dtbexamenes.Rows.Add(filagre);
        //            }
        //            ViewState["Examenes"] = dtbexamenes;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        //private void FunCargarCombos(int opcion)
        //{
        //    switch (opcion)
        //    {
        //        case 0:
        //            Array.Resize(ref Objparam, 3);
        //            Objparam[0] = int.Parse(Session["usuCodigo"].ToString());
        //            Objparam[1] = "";
        //            Objparam[2] = 145;
        //            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
        //            codigoclus = int.Parse(dts.Tables[0].Rows[0]["CodigoCLUS"].ToString());
        //            codigocamp = int.Parse(dts.Tables[0].Rows[0]["CodigoCAMP"].ToString());

        //            Objparam[2] = 146;
        //            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
        //            //DdlCampos.DataSource = dts;
        //            //DdlCampos.DataTextField = "Descripcion";
        //            //DdlCampos.DataValueField = "Codigo";
        //            //DdlCampos.DataBind();

        //            Array.Resize(ref Objparam, 11);
        //            Objparam[0] = 14;
        //            Objparam[1] = "";
        //            Objparam[2] = "";
        //            Objparam[3] = "";
        //            Objparam[4] = "";
        //            Objparam[5] = "";
        //            Objparam[6] = codigocamp;
        //            Objparam[7] = int.Parse(Session["usuCodigo"].ToString());
        //            Objparam[8] = 0;
        //            Objparam[9] = 0;
        //            Objparam[10] = 0;
        //            dts = new Conexion(2, "").FunConsultaDatos1(Objparam);
        //            //DdlProducto.DataSource = dts;
        //            //DdlProducto.DataTextField = "Descripcion";
        //            //DdlProducto.DataValueField = "Codigo";
        //            //DdlProducto.DataBind();
        //            break;
        //        case 1:
        //            List<KeyValuePair<string, string>> listOper = new List<KeyValuePair<string, string>>();
        //            listOper.Add(new KeyValuePair<string, string>("0", "--<Seleccion Operación>--"));
        //            listOper.Add(new KeyValuePair<string, string>("=", "Igual (=)"));
        //            listOper.Add(new KeyValuePair<string, string>(">", "Mayor que (>)"));
        //            listOper.Add(new KeyValuePair<string, string>(">=", "Mayor Igual que (>=)"));
        //            listOper.Add(new KeyValuePair<string, string>("<", "Menor que (<)"));
        //            listOper.Add(new KeyValuePair<string, string>("<=", "Menor Igual que (<=)"));
        //            //listOper.Add(new KeyValuePair<string, string>("like", "Contiene Caracteres (Like)"));
        //            //listOper.Add(new KeyValuePair<string, string>("between", "Entre"));
        //            //DdlOperador.DataSource = listOper;
        //            //DdlOperador.DataTextField = "Value";
        //            //DdlOperador.DataValueField = "Key";
        //            //DdlOperador.DataBind();
        //            break;
        //        case 2:
        //            Array.Resize(ref objparam, 3);
        //            objparam[0] = 0;
        //            objparam[1] = "";
        //            objparam[2] = 137;
        //            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //            //DdlExamen.DataSource = dts;
        //            //DdlExamen.DataTextField = "Descripcion";
        //            //DdlExamen.DataValueField = "Codigo";
        //            //DdlExamen.DataBind();
        //            break;
        //    }
        //}

        //private void FunSetearCampos(string campo)
        //{
        //    try
        //    {
        //        TxtValor.Text = "";
        //        Array.Resize(ref Objparam, 3);
        //        Objparam[0] = 0;
        //        Objparam[1] = campo;
        //        Objparam[2] = 147;
        //        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
        //        ViewState["Tipo"] = dts.Tables[0].Rows[0]["Tipo"].ToString();
        //        TxtValor.Attributes.Clear();
        //        switch (ViewState["Tipo"].ToString())
        //        {
        //            case "date":
        //            case "datetime":
        //                CalendarExtender calExtender = new CalendarExtender();
        //                calExtender.Format = "MM/dd/yyyy";
        //                calExtender.TargetControlID = TxtValor.ID;
        //                PlaceTxt.Controls.Add(calExtender);
        //                break;
        //            case "int":
        //            case "smallint":
        //            case "tinyint":
        //                FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
        //                filter.FilterType = FilterTypes.Numbers;
        //                filter.TargetControlID = TxtValor.ID;
        //                PlaceTxt.Controls.Add(filter);
        //                break;
        //            case "decimal":
        //            case "numeric":
        //            case "float":
        //            case "money":
        //            case "real":
        //                TxtValor.Attributes.Add("onkeypress", "return NumeroDecimal(this.form.txtValor, event)");
        //                TxtValor.Attributes.Add("onchange", "ValidarDecimales();");
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        //private bool FunValidarCondiciones()
        //{
        //    if (DdlCampos.SelectedValue == "0")
        //    {
        //        new Funciones().funShowJSMessage("Debe seleccionar un Campo para la condición", this);
        //        return false;
        //    }
        //    if (DdlOperador.SelectedValue == "0")
        //    {
        //        new Funciones().funShowJSMessage("Debe seleccionar tipo de operación", this);
        //        return false;
        //    }
        //    if (TxtValor.Text == "")
        //    {
        //        new Funciones().funShowJSMessage("Debe ingresar un valor de comparación para la condición", this);
        //        return false;
        //    }
        //    if (ViewState["Variables"] != null)
        //    {
        //        dtbvariables = (DataTable)ViewState["Variables"];
        //        result = dtbvariables.Select("CodigoEXGC='" + ViewState["CodigoEXGC"].ToString() + "' and Campo='"+
        //            DdlCampos.SelectedItem.ToString() + "' and Operador='" + DdlOperador.SelectedItem.ToString() + "'").FirstOrDefault();
        //        lexiste = result != null ? true : false;
        //    }            
        //    lexiste = result != null ? true : false;
        //    if (lexiste)
        //    {
        //        new Funciones().funShowJSMessage("Ya existe un registro con esta condición", this);
        //        return false;
        //    }
            //FunSetearCampos(DdlCampos.SelectedValue);
        //    switch (ViewState["Tipo"].ToString())
        //    {
        //        case "numeric":
        //        case "double":
        //        case "decimal":
        //        case "int":
        //            if (DdlOperador.SelectedValue.ToString() == "like")
        //            {
        //                new Funciones().funShowJSMessage("No puede utilizar la operacion like para datos numéricos", this);
        //                return false;
        //            }
        //            break;
        //        case "date":
        //        case "smalldatetime":
        //        case "datetime":
        //            if (DdlOperador.SelectedValue.ToString() == "like")
        //            {
        //                new Funciones().funShowJSMessage("No puede utilizar la operacion like para datos tipo fecha", this);
        //                return false;
        //            }
        //            break;
        //    }
        //    return true;
        //}
        #endregion

        #region Botones y Eventos
    

        protected void GrdvGrupoExamen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
           
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                string estadoCodigo = GrdvGrupoExamen.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString().Trim().ToUpper();
                ImageButton btnGestion = e.Row.FindControl("ImgSeleccGrupo") as ImageButton;

                if (btnGestion == null)
                {
                    return;
                }

                if (estadoCodigo == "AUA" || estadoCodigo == "AUR")
                {
                    btnGestion.Enabled = true;
                    btnGestion.ImageUrl = "~/Botones/modificar.png";
                    btnGestion.ToolTip = "Gestionar auditoría";
                }
                else
                {
                    btnGestion.Enabled = false;
                    btnGestion.ImageUrl = "~/Botones/selecc.png";
                    btnGestion.ToolTip = "Disponible después de la auditoría";
                }
            
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void CargarSolicitudesCliente()
        {
            try
            {

                Lblerror.Text = "";

                int codigoUsuario = Convert.ToInt32(Session["usuCodigo"]);
                int codigoCampania = ObtenerCampaniaUsuario(codigoUsuario);

                Dictionary<int, string> productos =ObtenerProductosCliente(codigoCampania,codigoUsuario);

                DataTable listado = CrearTablaSolicitudesCliente();

                object[] parametros = new object[3];
                parametros[0] = 0;
                parametros[1] = "";
                parametros[2] = 144;

                DataSet dsSolicitudes = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos",parametros);

                if (dsSolicitudes != null && dsSolicitudes.Tables.Count > 0)
                {
                    foreach (DataRow fila in dsSolicitudes.Tables[0].Rows)
                    {
                        int codigoProducto = 0;

                        int.TryParse(fila["CodigoPROD"].ToString(),out codigoProducto);

                        if (!productos.ContainsKey(codigoProducto))
                        {
                            continue;
                        }

                        int codigoEXSO = 0;

                        int.TryParse(fila["CodigoEXSO"].ToString(),out codigoEXSO);


                        if (codigoEXSO <= 0)
                        {
                            continue;
                        }

                        DataRow nueva = listado.NewRow();

                        nueva["CodigoEXSO"] = codigoEXSO;
                        nueva["FechaSolicitud"] = fila["FecSolicitud"].ToString();
                        nueva["Cedula"] = fila["NumDocumento"].ToString();
                        nueva["Paciente"] = fila["Cliente"].ToString();
                        nueva["Producto"] = fila["Producto"].ToString();
                        string estadoCodigo = fila["EstadoCodigo"].ToString().Trim().ToUpper();
                        nueva["EstadoCodigo"] = estadoCodigo;
                        nueva["Estado"] = ObtenerDescripcionEstado(estadoCodigo);
                        nueva["FechaResultado"] = "";
                        listado.Rows.Add(nueva);
                    }
                }

                parametros = new object[3];
                parametros[0] = codigoCampania;
                parametros[1] = "";
                parametros[2] = 159;

                DataSet dsAuditoria = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos",parametros);

                if (dsAuditoria != null && dsAuditoria.Tables.Count > 0)
                {
                    foreach (DataRow fila in dsAuditoria.Tables[0].Rows)
                    {
                        int codigoEXSO = 0;
                        int.TryParse(fila["CodigoEXSO"].ToString(),out codigoEXSO);

                        if (codigoEXSO <= 0)
                        {
                            continue;
                        }

                        DataRow registro = null;
                        DataRow[] encontrados = listado.Select("CodigoEXSO = " + codigoEXSO.ToString());

                        if (encontrados.Length > 0)
                        {
                            registro = encontrados[0];
                        }
                        else
                        {
                            registro = listado.NewRow();
                            registro["CodigoEXSO"] = codigoEXSO;

                            DataRow datosSolicitud = ObtenerDatosSolicitud(codigoEXSO);

                            if (datosSolicitud != null)
                            {
                                registro["FechaSolicitud"] = datosSolicitud["FechaSolicita"].ToString();

                                int codigoProducto = 0;
                                int.TryParse(datosSolicitud["CodigoPROD"].ToString(),out codigoProducto);


                                if (productos.ContainsKey(codigoProducto))
                                {
                                    registro["Producto"] = productos[codigoProducto];
                                }
                                else
                                {
                                    registro["Producto"] = "";
                                }
                            }
                            else
                            {
                                registro["FechaSolicitud"] = "";
                                registro["Producto"] = "";
                            }

                            listado.Rows.Add(registro);
                        }

                        registro["Cedula"] = fila["Cedula"].ToString();
                        registro["Paciente"] = fila["Paciente"].ToString();
                        string estadoCodigo = fila["EstadoCodigo"].ToString().Trim().ToUpper();
                        registro["EstadoCodigo"] = estadoCodigo;
                        registro["Estado"] = ObtenerDescripcionEstado(estadoCodigo);
                        string fechaResultado = fila["FechaEnvioResultados"].ToString();

                        if (string.IsNullOrEmpty(fechaResultado))
                        {
                            fechaResultado = fila["FechaAprobado"].ToString();
                        }

                        registro["FechaResultado"] = fechaResultado;
                    }
                }

                DataView vista = listado.DefaultView;
                vista.Sort = "CodigoEXSO DESC";
                DataTable resultado = vista.ToTable();
                ViewState["SolicitudesCliente"] = resultado;
                GrdvGrupoExamen.DataSource = resultado;
                GrdvGrupoExamen.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private int ObtenerCampaniaUsuario(int codigoUsuario)
        {

            object[] parametros = new object[3];
            parametros[0] = codigoUsuario;
            parametros[1] = "";
            parametros[2] = 145;

            DataSet ds = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos",parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("El usuario no tiene un cliente/campaña seleccionado.");
            }

            int codigoCampania = 0;

            if (!int.TryParse(ds.Tables[0].Rows[0]["CodigoCAMP"].ToString(),out codigoCampania))
            {
                throw new Exception("No fue posible determinar la campaña del usuario.");
            }

            if (codigoCampania <= 0)
            {
                throw new Exception("El usuario no tiene una campaña válida.");
            }

            return codigoCampania;
        }

        private Dictionary<int, string> ObtenerProductosCliente(int codigoCampania,int codigoUsuario)
        {
            Dictionary<int, string> productos = new Dictionary<int, string>();

            object[] parametros = new object[3];
            parametros[0] = codigoCampania;
            parametros[1] = "";
            parametros[2] = 220;

            DataSet ds = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos",parametros);

            if (ds == null || ds.Tables.Count == 0)
            {
                return productos;
            }

            foreach (DataRow fila in ds.Tables[0].Rows)
            {

                int codigoProducto = 0;
                int.TryParse(fila["Codigo"].ToString(),out codigoProducto);

                if (codigoProducto <= 0)
                {
                    continue;
                }

                string descripcion = fila["Descripcion"].ToString();

                if (!productos.ContainsKey(codigoProducto))
                {
                    productos.Add(codigoProducto,descripcion);
                }
            }

            return productos;
        }

        private DataRow ObtenerDatosSolicitud(int codigoEXSO)
        {
            object[] parametros = new object[3];

            parametros[0] = codigoEXSO;
            parametros[1] = "";
            parametros[2] = 150;

            DataSet ds = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos",parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            return ds.Tables[0].Rows[0];
        }

        private DataTable CrearTablaSolicitudesCliente()
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add("CodigoEXSO",typeof(int));
            tabla.Columns.Add("FechaSolicitud",typeof(string));
            tabla.Columns.Add("Cedula",typeof(string));
            tabla.Columns.Add("Paciente",typeof(string));
            tabla.Columns.Add("Producto",typeof(string));
            tabla.Columns.Add("EstadoCodigo",typeof(string));
            tabla.Columns.Add("Estado",typeof(string));
            tabla.Columns.Add("FechaResultado",typeof(string));

            return tabla;
        }

        private string ObtenerDescripcionEstado(string estadoCodigo)
        {
            string estado = estadoCodigo == null ? "" : estadoCodigo.Trim().ToUpper();

            switch (estado)
            {
                case "SRR":

                    return "SOLICITADO";

                case "SRV":

                    return "SOLICITUD REVISADA";

                case "SGA":

                    return "AGENDADO";

                case "EXR":

                    return
                        "PENDIENTE DE AUDITORÍA";

                case "AUA":

                    return
                        "AUDITADO ACEPTADO";

                case "AUR":

                    return
                        "AUDITADO RECHAZADO";

                default:

                    return estado;
            }
        }

        protected void GrdvGrupoExamen_PageIndexChanging(object sender,GridViewPageEventArgs e)
        {
            try
            {

                GrdvGrupoExamen.PageIndex = e.NewPageIndex;

                DataTable tabla =ViewState["SolicitudesCliente"] as DataTable;

                if (tabla == null)
                {
                    CargarSolicitudesCliente();
                    return;
                }

                GrdvGrupoExamen.DataSource = tabla;
                GrdvGrupoExamen.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgGestion_Command(object sender,CommandEventArgs e)
        {
            try
            {
                int codigoEXSO = 0;

                if (!int.TryParse(e.CommandArgument.ToString(),out codigoEXSO))
                {
                    Lblerror.Text = "No se pudo identificar la solicitud.";
                    return;
                }

                if (codigoEXSO <= 0)
                {
                    Lblerror.Text = "El código de la solicitud no es válido.";
                    return;
                }

                Response.Redirect("FrmGestionExamenCliente.aspx?CodigoEXSO=" + codigoEXSO.ToString(),true);
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

        //protected void BtnSalir_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("../Examenes/FrmNuevoExamenCliente.aspx", true);
        //}
        #endregion
    }
}