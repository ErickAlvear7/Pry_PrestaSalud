namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmAtencionOdontoAdministrador : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbordenado = new DataTable();
        DataTable _dtbgrabarpresupuesto = new DataTable();
        DataTable _dtblagre;
        Object[] _objparam = new Object[1];
        DataTable _dtbpresupuesto = new DataTable();
        DataRow _filagre, _result, _nuevoresult;
        DataRow[] _rows;
        ImageButton _imgeliminar = new ImageButton();
        ImageButton _imgprioridad = new ImageButton();
        ImageButton _imgdetalle = new ImageButton();
        CheckBox _chkrealizar = new CheckBox();
        CheckBox _chkest = new CheckBox();
        string _codigo = "", _inicial = "", _realizado = "", _mensaje = "";
        int _codigocita = 0, _codigocabecera = 0, _codigoprocedimiento = 0, _cobertura = 0, _contar = 0, _maxcodigo = 0, _codigoprestadora = 0, _prioridad = 0, _codigoanterior = 0, _prioridadanterior = 0;
        bool _lexiste = false, _continuar;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                _dtbpresupuesto.Columns.Add("Codigo");
                _dtbpresupuesto.Columns.Add("CodigoCabecera");
                _dtbpresupuesto.Columns.Add("CodigoProcedimiento");
                _dtbpresupuesto.Columns.Add("Procedimiento");
                _dtbpresupuesto.Columns.Add("Pieza");
                _dtbpresupuesto.Columns.Add("Pvp");
                _dtbpresupuesto.Columns.Add("Costo");
                _dtbpresupuesto.Columns.Add("Cobertura");
                _dtbpresupuesto.Columns.Add("Total");
                _dtbpresupuesto.Columns.Add("Prioridad");
                _dtbpresupuesto.Columns.Add("Autorizado");
                _dtbpresupuesto.Columns.Add("Realizado");
                _dtbpresupuesto.Columns.Add("Inicial");
                _dtbpresupuesto.Columns.Add("Realizar");
                _dtbpresupuesto.Columns.Add("CodigoPrestadora");
                ViewState["tbPresupuesto"] = _dtbpresupuesto;

                ViewState["CodigoMedico"] = Request["CodigoMedico"];
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["CodigoTitu"] = Request["CodigoTitu"];
                ViewState["CodigoBene"] = Request["CodigoBene"];
                ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm:ss");
                ViewState["RegistraHistorial"] = "NO";
                FunCargarCombos(1);
                FunCargarCombos(3);
                FunCargarCombos(4);
                ListItem item = new ListItem();
                item.Text = "--Seleccione Pieza--";
                item.Value = "-1";
                DdlPieza.Items.Add(item);
                FunCargaMantenimiento();
            }
        } 
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                _objparam[1] = "";
                _objparam[2] = 65;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0) ViewState["Medico"] = _dts.Tables[0].Rows[0][0].ToString();
                else ViewState["Medico"] = "SIN MEDICO/COMUNICAR AL ADMINISTRADOR DEL SISTEMA";

                _objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[1] = "";
                _objparam[2] = 58;

                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0) ViewState["Paciente"] = _dts.Tables[0].Rows[0][0].ToString();
                else ViewState["Paciente"] = "SIN PACIENTE/COMUNICAR AL ADMINISTRADOR DEL SISTEMA";

                _objparam[2] = 114;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0) ViewState["CodigoPrestadora"] = _dts.Tables[0].Rows[0][0].ToString();
                else ViewState["CodigoPrestadora"] = "0";

                _objparam[2] = 79;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                Lbltitulo.Text = "Cita Odontológica - Paciente: " + ViewState["Paciente"].ToString() + " - Producto: " + _dts.Tables[0].Rows[0][0].ToString();

                //LLENAR LOS PRESUPUESTOS REGISTRADOS SI YA EXISTEN
                Array.Resize(ref _objparam, 7);
                _objparam[0] = 15;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                _objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                _objparam[4] = 0;
                _objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[6] = Session["MachineName"].ToString();
                new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);
                _objparam[0] = 0;

                _dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);

                if (_dts.Tables[0].Rows.Count == 0) FunLlenarPrimerRegistro();
                else
                {
                    _prioridad = 0;

                    foreach (DataRow drrow in _dts.Tables[0].Rows)
                    {
                        drrow["Prioridad"] = _prioridad;
                        _prioridad++;
                        _dts.Tables[0].AcceptChanges();
                    }

                    ViewState["tbPresupuesto"] = _dts.Tables[0];
                }

                _dtbpresupuesto = (DataTable)ViewState["tbPresupuesto"];
                GrdvPresupuesto.DataSource = _dtbpresupuesto;
                GrdvPresupuesto.DataBind();

                if (GrdvPresupuesto.Rows.Count > 0)
                {
                    _imgprioridad = (ImageButton)GrdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgprioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    Array.Resize(ref _objparam, 3);
                    _objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                    _objparam[1] = "";
                    _objparam[2] = 61;
                    DdlProcedimiento.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                    DdlProcedimiento.DataTextField = "Descripcion";
                    DdlProcedimiento.DataValueField = "Codigo";
                    DdlProcedimiento.DataBind();
                    break;
                case 2:
                    Array.Resize(ref _objparam, 3);
                    _objparam[0] = int.Parse(DdlProcedimiento.SelectedValue);
                    _objparam[1] = DdlOndotograma.SelectedValue;
                    _objparam[2] = 60;
                    DdlPieza.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                    DdlPieza.DataTextField = "Descripcion";
                    DdlPieza.DataValueField = "Codigo";
                    DdlPieza.DataBind();
                    break;
                case 3:
                    Array.Resize(ref _objparam, 1);
                    _objparam[0] = 28;
                    DdlRegistroOdonto.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", _objparam);
                    DdlRegistroOdonto.DataTextField = "Descripcion";
                    DdlRegistroOdonto.DataValueField = "Codigo";
                    DdlRegistroOdonto.DataBind();
                    break;
                case 4:
                    Array.Resize(ref _objparam, 1);
                    _objparam[0] = 32;
                    DdlOndotograma.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", _objparam);
                    DdlOndotograma.DataTextField = "Descripcion";
                    DdlOndotograma.DataValueField = "Codigo";
                    DdlOndotograma.DataBind();
                    break;
            }
        }

        private void FunLlenarPrimerRegistro()
        {
            try
            {
                Array.Resize(ref _objparam, 7);
                _objparam[0] = 1;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                _objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                _objparam[4] = 0;
                _objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[6] = Session["MachineName"].ToString();
                _dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dtblagre = new DataTable();
                    _dtblagre = (DataTable)ViewState["tbPresupuesto"];
                    _filagre = _dtblagre.NewRow();
                    _filagre["Codigo"] = _dts.Tables[0].Rows[0][0].ToString();
                    _filagre["CodigoCabecera"] = _dts.Tables[0].Rows[0][1].ToString();
                    _filagre["CodigoProcedimiento"] = _dts.Tables[0].Rows[0][2].ToString();
                    _filagre["Procedimiento"] = _dts.Tables[0].Rows[0][3].ToString();
                    _filagre["Pieza"] = _dts.Tables[0].Rows[0][4].ToString();
                    _filagre["Pvp"] = _dts.Tables[0].Rows[0][5].ToString();
                    _filagre["Costo"] = _dts.Tables[0].Rows[0][6].ToString();
                    _filagre["Cobertura"] = _dts.Tables[0].Rows[0][7].ToString();
                    _filagre["Total"] = _dts.Tables[0].Rows[0][8].ToString();
                    _filagre["Prioridad"] = _dts.Tables[0].Rows[0][9].ToString();
                    _filagre["Autorizado"] = _dts.Tables[0].Rows[0][10].ToString();
                    _filagre["Realizado"] = _dts.Tables[0].Rows[0][11].ToString();
                    _filagre["Inicial"] = _dts.Tables[0].Rows[0][12].ToString();
                    _filagre["Realizar"] = "SI";
                    _filagre["CodigoPrestadora"] = ViewState["CodigoPrestadora"].ToString();
                    _dtblagre.Rows.Add(_filagre);
                    ViewState["tbPresupuesto"] = _dtblagre;
                    GrdvPresupuesto.DataSource = _dtblagre;
                    GrdvPresupuesto.DataBind();
                    _imgprioridad = (ImageButton)GrdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgprioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void FunHistorialProcedimientos(int codCabecera, int codProcedimiento)
        {
            try
            {
                Array.Resize(ref _objparam, 7);
                _objparam[0] = 8;
                _objparam[1] = codCabecera;
                _objparam[2] = 0;
                _objparam[3] = 0;
                _objparam[4] = codProcedimiento;
                _objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[6] = Session["MachineName"].ToString();
                _dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgProductos_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updPresupuesto, GetType(), "Productos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('../MedicoOdonto/FrmProductoProcedimientos.aspx?CodigoCita=" + ViewState["CodigoCita"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=900px, height=450px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void DdlOndotograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlPieza.Items.Clear();
            ListItem _item = new ListItem();
            _item.Text = "--Seleccione Pieza--";
            _item.Value = "0";
            DdlPieza.Items.Add(_item);
            DdlProcedimiento.SelectedValue = "0";

            if (DdlOndotograma.SelectedValue != "0")
            {
                switch (DdlOndotograma.SelectedValue)
                {
                    case "A":
                        ImgOdontoGrama.ImageUrl = "~/Images/Odonto_Adultos.jpg";
                        break;
                    case "P":
                        ImgOdontoGrama.ImageUrl = "~/Images/Odonto_Pediatrico.jpg";
                        break;
                }
            }
        }

        protected void DdlProcedimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlProcedimiento.SelectedValue != "0" && DdlOndotograma.SelectedValue != "0") FunCargarCombos(2);
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();
                TxtEditor.Content = "";

                if (DdlOndotograma.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Odontograma..!", this);
                    return;
                }

                if (DdlProcedimiento.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Procedimiento..!", this);
                    return;
                }

                if (DdlPieza.SelectedValue == "-1")
                {
                    new Funciones().funShowJSMessage("Seleccione Pieza Dental..!", this);
                    return;
                }

                Array.Resize(ref _objparam, 7);
                _objparam[0] = 2;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                _objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                _objparam[4] = int.Parse(DdlProcedimiento.SelectedValue);
                _objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[6] = Session["MachineName"].ToString();

                _dts = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("No existe definido Procedimiento en la Prestadora..!", this);
                    return;
                }

                if (ViewState["tbPresupuesto"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tbPresupuesto"];

                    if (tblbuscar.Rows.Count > 0)
                    {
                        _maxcodigo = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                        _prioridad = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Prioridad"]));
                    }
                    else
                    {
                        _maxcodigo = 0;
                        _prioridad = 0;
                    }

                    _result = tblbuscar.Select("CodigoProcedimiento=" + DdlProcedimiento.SelectedValue + " and Pieza='" + DdlPieza.SelectedValue + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe Ingresado Procedimiento para la misma Pieza Dental..!", this);
                    return;
                }

                _dtblagre = new DataTable();
                _dtblagre = (DataTable)ViewState["tbPresupuesto"];
                _filagre = _dtblagre.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["CodigoCabecera"] = 0;
                _filagre["CodigoProcedimiento"] = DdlProcedimiento.SelectedValue;
                _filagre["Procedimiento"] = DdlProcedimiento.SelectedItem.ToString();
                _filagre["Pieza"] = DdlPieza.SelectedItem.ToString();
                _filagre["Pvp"] = _dts.Tables[0].Rows[0][0].ToString();
                _filagre["Costo"] = _dts.Tables[0].Rows[0][1].ToString();
                _filagre["Cobertura"] = _dts.Tables[0].Rows[0][2].ToString();
                _filagre["Total"] = _dts.Tables[0].Rows[0][3].ToString();
                _filagre["Prioridad"] = _prioridad + 1;
                _filagre["Autorizado"] = _dts.Tables[0].Rows[0][5].ToString();
                _filagre["Realizado"] = _dts.Tables[0].Rows[0][6].ToString();
                _filagre["Inicial"] = _dts.Tables[0].Rows[0][7].ToString();
                _filagre["Realizar"] = "NO";
                _filagre["CodigoPrestadora"] = ViewState["CodigoPrestadora"].ToString();
                _dtblagre.Rows.Add(_filagre);
                _dtblagre.DefaultView.Sort = "Realizado";
                ViewState["tbPresupuesto"] = _dtblagre;
                GrdvPresupuesto.DataSource = _dtblagre;
                GrdvPresupuesto.DataBind();

                if (GrdvPresupuesto.Rows.Count > 0)
                {
                    _imgprioridad = (ImageButton)GrdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgprioridad.Enabled = false;
                }

                DdlProcedimiento.SelectedValue = "0";
                DdlPieza.SelectedValue = "-1";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvPresupuesto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _inicial = GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Inicial"].ToString();
                    _realizado = GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Realizado"].ToString();
                    _codigocita = int.Parse(GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    _codigocabecera = int.Parse(GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoCabecera"].ToString());
                    _codigoprocedimiento = int.Parse(GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoProcedimiento"].ToString());
                    _codigoprestadora = int.Parse(GrdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoPrestadora"].ToString());
                    _imgeliminar = (ImageButton)(e.Row.Cells[0].FindControl("imgEliminar"));
                    _imgprioridad = (ImageButton)(e.Row.Cells[7].FindControl("imgPrioridad"));
                    _chkrealizar = (CheckBox)(e.Row.Cells[8].FindControl("chkRealizar"));
                    _imgdetalle = (ImageButton)(e.Row.Cells[9].FindControl("imgDetalle"));

                    if (_inicial == "DF")
                    {
                        _imgeliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        _imgeliminar.Height = 20;
                        _imgeliminar.Enabled = false;
                        _imgprioridad.Enabled = false;
                        _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                        _imgprioridad.Height = 20;
                        _chkrealizar.Checked = true;
                        _chkrealizar.Enabled = false;

                        if (_realizado == "SI")
                        {
                            _imgdetalle.ImageUrl = "~/Botones/Buscar.png";
                            _imgdetalle.Enabled = true;
                            _imgdetalle.Height = 20;
                        }
                    }
                    else if (_realizado == "SI")
                    {
                        _imgeliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        _imgeliminar.Height = 20;
                        _imgeliminar.Enabled = false;
                        _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                        _imgprioridad.Enabled = false;
                        _imgprioridad.Height = 20;
                        _imgdetalle.ImageUrl = "~/Botones/Buscar.png";
                        _imgdetalle.Enabled = true;
                        _imgdetalle.Height = 20;
                        _chkrealizar.Checked = true;
                        _chkrealizar.Enabled = false;
                    }

                    if (_codigoprestadora != int.Parse(ViewState["CodigoPrestadora"].ToString()))
                    {
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                    }
                    //ACTIVAR EL BOTON DETALLE SI EXISTE REGISTRO DEL PROCEDIMIENTO
                }
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
                _codigo = GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigocabecera = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString());
                _codigoprocedimiento = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoProcedimiento"].ToString());
                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();
                TxtEditor.Content = "";

                Array.Resize(ref _objparam, 7);
                _objparam[0] = 10;
                _objparam[1] = _codigocabecera;
                _objparam[2] = _codigo;
                _objparam[3] = 0;
                _objparam[4] = _codigoprocedimiento;
                _objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[6] = Session["MachineName"].ToString();
                new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", _objparam);
                _dtblagre = new DataTable();
                _dtblagre = (DataTable)ViewState["tbPresupuesto"];

                _rows = _dtblagre.Select("Codigo='" + _codigo + "'");

                foreach (DataRow _row in _rows)
                {
                    _dtblagre.Rows.Remove(_row);
                }

                _dtblagre.DefaultView.Sort = "Realizado";
                _prioridad = 0;

                foreach (DataRow _row in _dtblagre.Rows)
                {
                    _row["Prioridad"] = _prioridad;
                    _prioridad++;
                    _dtblagre.AcceptChanges();
                }

                ViewState["tbPresupuesto"] = _dtblagre;
                GrdvPresupuesto.DataSource = _dtblagre;
                GrdvPresupuesto.DataBind();

                if (GrdvPresupuesto.Rows.Count > 0)
                {
                    _imgprioridad = (ImageButton)GrdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgprioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgPrioridad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();
                TxtEditor.Content = "";

                _dtbpresupuesto = (DataTable)ViewState["tbPresupuesto"];
                _codigo = GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _prioridad = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Prioridad"].ToString());

                if (_prioridad - 1 > 0)
                {
                    _result = _dtbpresupuesto.Select("Codigo=" + _codigo).FirstOrDefault();
                    _result["Prioridad"] = _prioridad - 1;

                    _codigoanterior = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex - 1].Values["Codigo"].ToString());
                    _prioridadanterior = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex - 1].Values["Prioridad"].ToString());
                    _nuevoresult = _dtbpresupuesto.Select("Codigo=" + _codigoanterior).FirstOrDefault();
                    _nuevoresult["Prioridad"] = _prioridadanterior + 1;
                    _dtbpresupuesto.AcceptChanges();
                }

                _dtbpresupuesto.DefaultView.Sort = "Prioridad";
                ViewState["tbPresupuesto"] = _dtbpresupuesto;
                GrdvPresupuesto.DataSource = _dtbpresupuesto;
                GrdvPresupuesto.DataBind();

                if (GrdvPresupuesto.Rows.Count > 0)
                {
                    _imgprioridad = (ImageButton)GrdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    _imgprioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    _imgprioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ChkRealizar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TxtEditor.Content = "";
                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _chkest = (CheckBox)(gvRow.Cells[7].FindControl("chkRealizar"));

                ViewState["Checked"] = _chkest.Checked;
                _dtbpresupuesto = (DataTable)ViewState["tbPresupuesto"];
                _codigo = GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _cobertura = int.Parse(GrdvPresupuesto.Rows[gvRow.RowIndex].Cells[5].Text);
                _result = _dtbpresupuesto.Select("Codigo='" + _codigo + "'").FirstOrDefault();

                if (_chkest.Checked == true)
                {
                    if (_cobertura > 0)
                    {
                        _contar = _dtbpresupuesto.Select("Cobertura>0 and Realizar='SI' and Realizado='NO' and Inicial='0'").Length;

                        if (_contar > 0)
                        {
                            new Funciones().funShowJSMessage("Solo se puede realizar un procedimiento con cobertura!..", this);
                            _chkest.Checked = false;
                            ViewState["Checked"] = _chkest.Checked;
                        }
                    }
                }

                _dtbpresupuesto.AcceptChanges();
                _chkrealizar = (CheckBox)(gvRow.Cells[7].FindControl("chkRealizar"));
                _chkrealizar.Checked = (bool)ViewState["Checked"];

                if (_chkrealizar.Checked)
                {
                    GrdvPresupuesto.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Bisque;
                    _result["Realizar"] = "SI";
                    _dtbpresupuesto.AcceptChanges();
                }
                else
                {
                    GrdvPresupuesto.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.White;
                    _result["Realizar"] = "NO";
                    _dtbpresupuesto.AcceptChanges();
                }
                ViewState["tbPresupuesto"] = _dtbpresupuesto;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            TxtEditor.Content = "";

            foreach (GridViewRow _fr in GrdvPresupuesto.Rows)
            {
                _fr.Cells[2].BackColor = System.Drawing.Color.White;
            }

            GrdvPresupuesto.Rows[gvRow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Teal;
            _codigocabecera = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString());
            _codigo = GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
            _codigoprocedimiento = int.Parse(GrdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoProcedimiento"].ToString());
            FunHistorialProcedimientos(int.Parse(_codigo), _codigoprocedimiento);
        }

        protected void ImgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();

                foreach (GridViewRow _fr in GrdvDatos.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Teal;

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(_codigo);
                _objparam[1] = "";
                _objparam[2] = 120;

                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                TxtEditor.Enabled = false;
                TxtEditor.Content = _dts.Tables[0].Rows[0]["Detalle"].ToString();
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
                BtnGrabar.Enabled = false;
                _dtbpresupuesto = (DataTable)ViewState["tbPresupuesto"];
                string[] columnas = new[] { "Codigo", "CodigoCabecera","CodigoProcedimiento", "Procedimiento", "Pieza", "Pvp", "Costo",
                "Cobertura","Total","Prioridad","Autorizado","Realizado","Inicial","Realizar"};
                DataView view = new DataView(_dtbpresupuesto);
                _dtbpresupuesto = view.ToTable(true, columnas);
                _contar = _dtbpresupuesto.Select("Realizar='SI' and Realizado='NO'").Length;
                //System.Threading.Thread.Sleep(300);
                Array.Resize(ref _objparam, 3);

                if (_contar > 0)
                {
                    ViewState["RegistraHistorial"] = "SI";
                    _objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                    _objparam[1] = "F";
                    _objparam[2] = 75;
                    _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                }
                else ViewState["RegistraHistorial"] = "NO";

                Array.Resize(ref _objparam, 15);
                _objparam[0] = 0;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString());
                _objparam[3] = ViewState["RegistraHistorial"].ToString();
                _objparam[4] = "T";
                _objparam[5] = "";
                _objparam[6] = "";
                _objparam[7] = "";
                _objparam[8] = "";
                _objparam[9] = 0;
                _objparam[10] = 0;
                _objparam[11] = 0;
                _objparam[12] = 0;
                _objparam[13] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[14] = Session["MachineName"].ToString();
                _dts = new Conexion(2, "").FunInsertPresupuestoCab(_objparam);
                _codigocabecera = int.Parse(_dts.Tables[0].Rows[0]["CodigoPSTO"].ToString());
                _codigo = _dts.Tables[0].Rows[0]["CodigoHICO"].ToString();

                if (_codigocabecera > 0)
                {
                    Array.Resize(ref _objparam, 29);
                    _objparam[0] = 0;
                    _objparam[1] = _codigocabecera;
                    _objparam[2] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                    _objparam[3] = int.Parse(ViewState["CodigoCita"].ToString());
                    _objparam[4] = _codigo;
                    _objparam[5] = TxtEditor.Content;
                    _objparam[19] = "";
                    _objparam[20] = "";
                    _objparam[21] = "";
                    _objparam[22] = "";
                    _objparam[23] = 0;
                    _objparam[24] = 0;
                    _objparam[25] = 0;
                    _objparam[26] = 0;
                    _objparam[27] = int.Parse(Session["usuCodigo"].ToString());
                    _objparam[28] = Session["MachineName"].ToString();

                    foreach (DataRow _drpres in _dtbpresupuesto.Rows)
                    {
                        _objparam[6] = int.Parse(_drpres["Codigo"].ToString());
                        _objparam[7] = int.Parse(_drpres["CodigoProcedimiento"].ToString());
                        _objparam[8] = _drpres["Procedimiento"].ToString();
                        _objparam[9] = _drpres["Pieza"].ToString();
                        _objparam[10] = _drpres["Pvp"].ToString();
                        _objparam[11] = _drpres["Costo"].ToString();
                        _objparam[12] = int.Parse(_drpres["Cobertura"].ToString());
                        _objparam[13] = _drpres["Total"].ToString();
                        _objparam[14] = int.Parse(_drpres["Prioridad"].ToString());
                        _objparam[15] = _drpres["Autorizado"].ToString();
                        _objparam[16] = _drpres["Realizado"].ToString();
                        _objparam[17] = _drpres["Inicial"].ToString();
                        _objparam[18] = _drpres["Realizar"].ToString();
                        _mensaje = new Conexion(2, "").FunInsertPresupuestoDet(_objparam);
                    }

                    Response.Redirect("FrmPresuRealizadoAdministrador.aspx?MensajeRetornado=" + _mensaje, true);
                }
                else Lblerror.Text = "Problemas de Inserción en Base de Datos..!";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCitaOdontoAdministrador.aspx", true);
        }

        protected void BtnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                BtnFinalizar.Enabled = false;
                _dtbpresupuesto = (DataTable)ViewState["tbPresupuesto"];

                _result = _dtbpresupuesto.Select("Cobertura>0 and Realizar='SI'").FirstOrDefault();

                if (_result == null) _continuar = false;
                else _contar = _dtbpresupuesto.Select("Cobertura>0 and Realizar='SI'").Length;

                if (_contar < 2) _continuar = false;
                else _continuar = true;

                if (_continuar == false)
                {
                    new Funciones().funShowJSMessage("Para Finalizar al menos debe realizar un Procedimiento con Cobertura", this);
                    return;
                }

                string[] columnas = new[] { "Codigo", "CodigoCabecera","CodigoProcedimiento", "Procedimiento", "Pieza", "Pvp", "Costo",
                "Cobertura","Total","Prioridad","Autorizado","Realizado","Inicial","Realizar"};
                DataView _view = new DataView(_dtbpresupuesto);
                _dtbpresupuesto = _view.ToTable(true, columnas);
                //System.Threading.Thread.Sleep(3000);

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[1] = "F";
                _objparam[2] = 75;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                Array.Resize(ref _objparam, 8);
                _objparam[0] = 0;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString());
                _objparam[3] = ViewState["RegistraHistorial"].ToString();
                _objparam[4] = "F";
                _objparam[5] = TxtEditor.Content;
                _objparam[6] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[7] = Session["MachineName"].ToString();

                _dts = new Conexion(2, "").FunInsertarPresupuesto(_objparam, _dtbpresupuesto);

                if (_dts.Tables[0].Rows.Count > 0) Response.Redirect("FrmPresuRealizadoAdministrador.aspx?MensajeRetornado=Guardado con Éxito", true);
                else Lblerror.Text = _dts.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        } 
        #endregion
    }
}