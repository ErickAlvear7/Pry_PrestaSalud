using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmSolicitudExamen : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbexamenes = new DataTable();
        Object[] objparam = new Object[1];
        DataRow resultado, filagre;
        DataRow[] resultedad, resultgenero, resultmonto;
        bool lexiste = false;
        Byte[] bytes;
        ImageButton imgeliminar;
        string adicional = "", codigo = "", fechaactual = "", filePath = "", filename1 = "", ext = "", type = "",
            mensaje = "";
        int codigoclus = 0, codigocamp = 0;
        bool continuaredad = false, continuargenero = false, continuarmonto = false;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            //TxtPvp.Attributes.Add("onchange", "ValidarDecimales();");
            TxtMonto.Attributes.Add("onchange", "ValidarDecimales();");
            TxtFechaNacimiento.Attributes.Add("onchange", "Calcular_Edad();");
            TxtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
            if (!IsPostBack)
            {
                try
                {
                    TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaSolicitud.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaSolicitud.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoEXSO"] = Request["CodigoEXSO"];                    
                    FunCargarCombos(0);
                    if (ViewState["CodigoEXSO"].ToString() != "0")
                    {
                        DdlProducto.Enabled = false;
                        TxtMonto.Enabled = false;
                        TxtNumeroDocumento.Text = Request["NumDocumento"];
                        DdlTipoDocumento.Enabled = false;
                        TxtNumeroDocumento.Enabled = false;
                        TxtPrimerNombre.Enabled = false;
                        TxtSegundoNombre.Enabled = false;
                        TxtPrimerApellido.Enabled = false;
                        TxtSegundoApellido.Enabled = false;
                        DdlGenero.Enabled = false;
                        DdlEstadoCivil.Enabled = false;
                        TxtFechaNacimiento.Enabled = false;
                        DdlProvincia.Enabled = false;
                        DdlCiudad.Enabled = false;
                        TxtDireccion.Enabled = false;
                        TxtFonoCasa.Enabled = false;
                        TxtFonoOficina.Enabled = false;
                        TxtCelular.Enabled = false;
                        TxtEmail.Enabled = false;
                        TxtFechaSolicitud.Enabled = false;
                        DdlGrupoExamen.Enabled = false;
                        DdlExamen.Enabled = false;
                        //TxtPvp.Enabled = false;
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        TxtObservacion.Enabled = false;
                        ImgAddExamen.Visible = false;
                        LblArchivo.Visible = false;
                        FileUpload1.Visible = false;
                        //BtnGrabar.Enabled = false;
                        FunCargarCabecera();
                        FunCargaMantenimiento();
                        FunCargarExamenes();
                        Lbltitulo.Text = "Solicitud Examen Realizada";
                    }
                    else Lbltitulo.Text = "Nueva Solicitud Examen";
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
            else TxtEdad.Text = hidEdad.Value;
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunLimpiarCampos()
        {
            DdlTipoDocumento.SelectedIndex = 0;
            TxtPrimerNombre.Text = "";
            TxtSegundoNombre.Text = "";
            TxtPrimerApellido.Text = "";
            TxtSegundoApellido.Text = "";
            DdlGenero.SelectedIndex = 0;
            DdlEstadoCivil.SelectedIndex = 0;
            TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
            TxtEdad.Text = "0";
            DdlProvincia.SelectedIndex = 0;
            FunCargarCombos(1);
            TxtDireccion.Text = "";
            TxtFonoCasa.Text = "";
            TxtFonoOficina.Text = "";
            TxtCelular.Text = "";
            TxtEmail.Text = "";
        }

        private void FunCargarCabecera()
        {
            try
            {
                Array.Resize(ref objparam, 3);                
                objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[1] = "";
                objparam[2] = 150;
                dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                DdlProducto.SelectedValue = dtx.Tables[0].Rows[0]["CodigoPROD"].ToString();
                FunCargarCombos(2);
                DdlGrupoExamen.SelectedValue = dtx.Tables[0].Rows[0]["CodigoEXGC"].ToString();
                TxtFechaSolicitud.Text = dtx.Tables[0].Rows[0]["FechaSolicita"].ToString();
                TxtObservacion.Text = dtx.Tables[0].Rows[0]["Observacion"].ToString();
                ChkEstado.Checked = dtx.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                ChkEstado.Text = dtx.Tables[0].Rows[0]["Estado"].ToString();

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(DdlProducto.SelectedValue);
                objparam[1] = TxtNumeroDocumento.Text;
                objparam[2] = 23;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["CodigoTITU"] = dts.Tables[0].Rows[0]["TITU_CODIGO"].ToString();

                objparam[0] = int.Parse(ViewState["CodigoTITU"].ToString());
                objparam[1] = "";
                objparam[2] = 128;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                    TxtMonto.Text = dts.Tables[0].Rows[0]["Inf10"].ToString();
                else TxtMonto.Text = "0.00";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = TxtNumeroDocumento.Text.Trim();
                objparam[2] = 22;

                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    objparam[0] = int.Parse(dts.Tables[0].Rows[0]["CodigoPERS"].ToString());
                    objparam[1] = "";
                    objparam[2] = 152;
                    dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    if (dtx.Tables[0].Rows.Count > 0)
                    {
                        mensaje = "El Cliente Presenta proceso de Examen " +
                            dtx.Tables[0].Rows[0]["Proceso"].ToString() + " Realizado el " +
                            dtx.Tables[0].Rows[0]["FechaProceso"].ToString();
                        new Funciones().funShowJSMessage(mensaje, this);
                        TxtNumeroDocumento.Text = "";
                        return;
                    }

                    DdlTipoDocumento.SelectedValue = dts.Tables[0].Rows[0]["TipoDocumento"].ToString();
                    if (DdlTipoDocumento.SelectedValue == "C")
                    {
                        TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                        TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                        TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    }
                    else
                    {
                        TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                        TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                        TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                    }
                    ViewState["CodigoPERS"] = dts.Tables[0].Rows[0]["CodigoPERS"].ToString();
                    TxtNumeroDocumento.Text = dts.Tables[0].Rows[0]["Identificacion"].ToString();
                    TxtPrimerNombre.Text = dts.Tables[0].Rows[0]["PrimerNombre"].ToString();
                    TxtSegundoNombre.Text = dts.Tables[0].Rows[0]["SegundNombre"].ToString();
                    TxtPrimerApellido.Text = dts.Tables[0].Rows[0]["PrimerApellido"].ToString();
                    TxtSegundoApellido.Text = dts.Tables[0].Rows[0]["SegundoApellido"].ToString();
                    DdlGenero.SelectedValue = dts.Tables[0].Rows[0]["Genero"].ToString();
                    DdlEstadoCivil.SelectedValue = dts.Tables[0].Rows[0]["EstCivil"].ToString();
                    TxtFechaNacimiento.Text = dts.Tables[0].Rows[0]["fechanacimiento"].ToString();
                    TxtEdad.Text = dts.Tables[0].Rows[0]["edad"].ToString();
                    DdlProvincia.SelectedValue = dts.Tables[0].Rows[0]["codProv"].ToString();
                    FunCargarCombos(1);
                    DdlCiudad.SelectedValue = dts.Tables[0].Rows[0]["CodCiud"].ToString();
                    TxtDireccion.Text = dts.Tables[0].Rows[0]["Direccion"].ToString();
                    TxtFonoCasa.Text = dts.Tables[0].Rows[0]["FonoCasa"].ToString();
                    TxtFonoOficina.Text = dts.Tables[0].Rows[0]["FonoOfic"].ToString();
                    TxtCelular.Text = dts.Tables[0].Rows[0]["Celular"].ToString();
                    TxtEmail.Text = dts.Tables[0].Rows[0]["Email"].ToString();
                    hidEdad.Value = TxtEdad.Text;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarExamenes()
        {
            try
            {
                TrExamenes.Visible = true;
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[1] = "";
                objparam[2] = 143;
                dts= new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvExamenes.DataSource = dts;
                GrdvExamenes.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                        objparam[1] = "";
                        objparam[2] = 145;
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        if (dts.Tables[0].Rows.Count > 0)
                        {
                            codigoclus = int.Parse(dts.Tables[0].Rows[0]["CodigoCLUS"].ToString());
                            codigocamp = int.Parse(dts.Tables[0].Rows[0]["CodigoCAMP"].ToString());
                        }

                        Array.Resize(ref objparam, 11);
                        objparam[0] = 14;
                        objparam[1] = "";
                        objparam[2] = "";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = codigocamp;
                        objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                        objparam[8] = 0;
                        objparam[9] = 0;
                        objparam[10] = 0;
                        dts = new Conexion(2, "").FunConsultaDatos1(objparam);
                        if (dts.Tables[0].Rows.Count > 0)
                        {
                            DdlProducto.DataSource = dts;
                            DdlProducto.DataTextField = "Descripcion";
                            DdlProducto.DataValueField = "Codigo";
                            DdlProducto.DataBind();
                        }

                        Array.Resize(ref objparam, 1);
                        objparam[0] = 6;
                        DdlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        DdlProvincia.DataTextField = "Descripcion";
                        DdlProvincia.DataValueField = "Codigo";
                        DdlProvincia.DataBind();

                        Array.Resize(ref objparam, 3);
                        objparam[0] = DdlProvincia.SelectedValue;
                        objparam[1] = "";
                        objparam[2] = 4;
                        DdlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlCiudad.DataTextField = "Descripcion";
                        DdlCiudad.DataValueField = "Codigo";
                        DdlCiudad.DataBind();

                        Array.Resize(ref objparam, 1);
                        objparam[0] = "TIPO DOCUMENTOS";
                        DdlTipoDocumento.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                        DdlTipoDocumento.DataTextField = "Descripcion";
                        DdlTipoDocumento.DataValueField = "Valor";
                        DdlTipoDocumento.DataBind();

                        Array.Resize(ref objparam, 1);
                        objparam[0] = "GENERO";
                        DdlGenero.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                        DdlGenero.DataTextField = "Descripcion";
                        DdlGenero.DataValueField = "Valor";
                        DdlGenero.DataBind();

                        objparam[0] = "ESTADO CIVIL";
                        DdlEstadoCivil.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                        DdlEstadoCivil.DataTextField = "Descripcion";
                        DdlEstadoCivil.DataValueField = "Valor";
                        DdlEstadoCivil.DataBind();

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
                    case 1:
                        DdlCiudad.Items.Clear();
                        Array.Resize(ref objparam, 3);
                        objparam[0] = DdlProvincia.SelectedValue;
                        objparam[1] = "";
                        objparam[2] = 4;
                        DdlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlCiudad.DataTextField = "Descripcion";
                        DdlCiudad.DataValueField = "Codigo";
                        DdlCiudad.DataBind();
                        break;
                    case 2:
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(DdlProducto.SelectedValue);
                        objparam[1] = "";
                        //objparam[2] = 148;
                        //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        //ViewState["Costo"] = dts.Tables[0].Rows[0]["Costo"].ToString();

                        objparam[2] = 139;
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        if (dts.Tables[0].Rows.Count > 1)
                        {
                            DdlGrupoExamen.DataSource = dts;
                            DdlGrupoExamen.DataTextField = "Descripcion";
                            DdlGrupoExamen.DataValueField = "Codigo";
                            DdlGrupoExamen.DataBind();
                            LblTituloExa.Visible = true;
                            TrExamenes.Visible = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblTituloExa.Visible = false;
            TrExamenes.Visible = false;
            GrdvExamenes.DataSource = null;
            GrdvExamenes.DataBind();
            FunCargarCombos(2);
        }

        protected void DdlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtNumeroDocumento.Text = "";
            if (DdlTipoDocumento.SelectedValue == "C")
            {
                TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
            }
            else
            {
                TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
            }
        }

        protected void TxtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            FunLimpiarCampos();
            FunCargaMantenimiento();
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void DdlGrupoExamen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlProducto.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Producto..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Numero de Documento..!", this);
                    DdlGrupoExamen.SelectedValue = "0";
                    return;
                }
                if (string.IsNullOrEmpty(TxtPrimerNombre.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre del Cliente..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtPrimerApellido.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Apellido del Cliente..!", this);
                    return;
                }
                if (DdlGenero.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Genero del Cliente..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtEdad.Text.Trim()) || TxtEdad.Text.Trim() == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Fecha de Nacimiento del CLiente..!", this);
                    return;
                }

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(DdlGrupoExamen.SelectedValue);
                objparam[1] = "";
                objparam[2] = 149;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                //VALIDAR SI HAY EDAD
                continuaredad = true;
                continuargenero = true;
                continuarmonto = true;
                resultedad = dts.Tables[0].Select("Campo='" + "Edad" + "'");
                if (resultedad != null)
                {
                    if (resultedad.Count() == 1)
                    {
                        if (resultedad[0]["Operador"].ToString() == "=")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) == int.Parse(resultedad[0]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == ">")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) > int.Parse(resultedad[0]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == ">=")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) >= int.Parse(resultedad[0]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == "<")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) < int.Parse(resultedad[0]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == "<=")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) <= int.Parse(resultedad[0]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }
                    }
                    else if (resultedad.Count() == 2)
                    {
                        if (resultedad[0]["Operador"].ToString() == ">" && resultedad[1]["Operador"].ToString() == "<")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) > int.Parse(resultedad[0]["Valor"].ToString()) &&
                                int.Parse(TxtEdad.Text.Trim()) < int.Parse(resultedad[1]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == ">=" && resultedad[1]["Operador"].ToString() == "<")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) >= int.Parse(resultedad[0]["Valor"].ToString()) &&
                                int.Parse(TxtEdad.Text.Trim()) < int.Parse(resultedad[1]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == ">" && resultedad[1]["Operador"].ToString() == "<=")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) > int.Parse(resultedad[0]["Valor"].ToString()) &&
                                int.Parse(TxtEdad.Text.Trim()) <= int.Parse(resultedad[1]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }

                        if (resultedad[0]["Operador"].ToString() == ">=" && resultedad[1]["Operador"].ToString() == "<=")
                        {
                            if (int.Parse(TxtEdad.Text.Trim()) >= int.Parse(resultedad[0]["Valor"].ToString()) &&
                                int.Parse(TxtEdad.Text.Trim()) <= int.Parse(resultedad[1]["Valor"].ToString())) continuaredad = true;
                            else continuaredad = false;
                        }
                    }
                    else if (resultedad.Count() == 0) continuaredad = true;
                    else continuaredad = false;
                }
                else continuaredad = true;

                resultgenero = dts.Tables[0].Select("Campo='" + "Genero" + "'");
                if (resultgenero != null)
                {
                    if (resultgenero.Count() == 1)
                    {
                        if (resultgenero[0]["Operador"].ToString() == "=")
                        {
                            if (DdlGenero.SelectedValue == resultgenero[0]["Valor"].ToString()) continuargenero = true;
                            else continuargenero = false;
                        }
                    }
                    else if (resultgenero.Count() == 0) continuargenero = true;
                    else continuargenero = false;
                }
                else continuargenero = true;

                resultmonto = dts.Tables[0].Select("Campo='" + "Monto" + "'");
                if (resultmonto != null || resultmonto.Count() > 0)
                {
                    if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                    {
                        new Funciones().funShowJSMessage("Ingrese Monto Solicitado..!", this);
                        DdlGrupoExamen.SelectedValue = "0";
                        return;
                    }
                    if (resultmonto.Count() == 1)
                    {
                        if (resultmonto[0]["Operador"].ToString() == "=")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) == decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == ">")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == ">=")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == "<")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == "<=")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }
                    }
                    else if (resultmonto.Count() == 2)
                    {
                        if (resultmonto[0]["Operador"].ToString() == ">" && resultmonto[1]["Operador"].ToString() == "<")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                                decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == ">=" && resultmonto[1]["Operador"].ToString() == "<")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                                decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == ">" && resultmonto[1]["Operador"].ToString() == "<=")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                                decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }

                        if (resultmonto[0]["Operador"].ToString() == ">=" && resultmonto[1]["Operador"].ToString() == "<=")
                        {
                            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                                decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                            else continuarmonto = false;
                        }
                    }
                    else if (resultmonto.Count() == 0) continuarmonto = true;
                    else continuarmonto = false;
                }
                else continuarmonto = true;

                if (continuaredad && continuargenero && continuarmonto)
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(DdlGrupoExamen.SelectedValue);
                    objparam[1] = "";
                    objparam[2] = 141;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ViewState["Examenes"] = dts.Tables[0];
                    GrdvExamenes.DataSource = dts;
                    GrdvExamenes.DataBind();
                    if (dts.Tables[0].Rows.Count > 0) TrExamenes.Visible = true;
                }
                else
                {
                    new Funciones().funShowJSMessage("Configuración Exámenes NO Está Definida para los Parámetros del Cliente..!", this);
                    DdlExamen.SelectedValue = "0";
                    DdlGrupoExamen.SelectedValue = "0";
                    dtbexamenes.Clear();
                    ViewState["Examenes"] = dtbexamenes;
                    GrdvExamenes.DataSource = dtbexamenes;
                    GrdvExamenes.DataBind();                    
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
                if (ViewState["Examenes"] == null)
                {
                    new Funciones().funShowJSMessage("Primero Seleccione Grupo Exámenes..!", this);
                    return;
                }
                if (DdlExamen.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Examen..!", this);
                    return;
                }
                if (ViewState["Examenes"] != null)
                {
                    dtbexamenes = (DataTable)ViewState["Examenes"];
                    resultado = dtbexamenes.Select("CodigoEXSE='" + DdlExamen.SelectedValue + "'").FirstOrDefault();
                    if (resultado != null) lexiste = true;
                }
                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Examen ya se encuentra Agregado..!", this);
                    return;
                }
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(DdlExamen.SelectedValue);
                objparam[1] = "";
                objparam[2] = 138;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                dtbexamenes = (DataTable)ViewState["Examenes"];
                filagre = dtbexamenes.NewRow();
                filagre["CodigoEXSE"] = DdlExamen.SelectedValue;
                filagre["Categoria"] = dts.Tables[0].Rows[0]["Categoria"].ToString();
                filagre["Examen"] = DdlExamen.SelectedItem.ToString();
                filagre["Costo"] = dts.Tables[0].Rows[0]["Valor"].ToString().Replace(".",",");
                filagre["Pvp"] = "0";
                filagre["Adicional"] = "SI";
                dtbexamenes.Rows.Add(filagre);
                dtbexamenes.DefaultView.Sort = "Examen";
                ViewState["Examenes"] = dtbexamenes;
                GrdvExamenes.DataSource = dtbexamenes;
                GrdvExamenes.DataBind();
                DdlExamen.SelectedValue = "0";
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
                codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["CodigoEXSE"].ToString();
                //Buscar si no existe Efecto agregado
                dtbexamenes = (DataTable)ViewState["Examenes"];
                resultado = dtbexamenes.Select("CodigoEXSE='" + codigo + "'").FirstOrDefault();
                resultado.Delete();
                dtbexamenes.AcceptChanges();
                ViewState["Examenes"] = dtbexamenes;
                GrdvExamenes.DataSource = dtbexamenes;
                GrdvExamenes.DataBind();
                if (dtbexamenes.Rows.Count == 0) TrExamenes.Visible = false;
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
                    imgeliminar = (ImageButton)(e.Row.Cells[2].FindControl("ImgDelExamen"));
                    adicional = GrdvExamenes.DataKeys[e.Row.RowIndex].Values["Adicional"].ToString();
                    if (adicional == "SI" && ViewState["CodigoEXSO"].ToString() == "0")
                    {
                        imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                        imgeliminar.Height = 15;                        
                        imgeliminar.Enabled = true;
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
                if (ViewState["CodigoEXSO"].ToString() == "0")
                {
                    if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                    {
                        new Funciones().funShowJSMessage("Ingrese Monto Solicitado..!", this);
                        return;
                    }
                    if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Ingrese No. de Documento..!", this);
                        return;
                    }
                    if (string.IsNullOrEmpty(TxtPrimerNombre.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Ingrese Nombre..!", this);
                        return;
                    }
                    if (string.IsNullOrEmpty(TxtPrimerApellido.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Ingrese Apellido..!", this);
                        return;
                    }
                    if (string.IsNullOrEmpty(TxtFechaNacimiento.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Ingrese Fecha Nacimiento..!", this);
                        return;
                    }
                    if (!new Funciones().IsDate(TxtFechaNacimiento.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Fecha Nacimiento No Es Válida..!", this);
                        return;
                    }
                    if (int.Parse(TxtEdad.Text) < 0)
                    {
                        new Funciones().funShowJSMessage("Edad Incorrecta..!", this);
                        return;
                    }
                    if (DdlGenero.SelectedValue == "0")
                    {
                        new Funciones().funShowJSMessage("Seleccione Género..!", this);
                        return;
                    }
                    if (DdlEstadoCivil.SelectedValue == "0")
                    {
                        new Funciones().funShowJSMessage("Seleccione Estado Civil..!", this);
                        return;
                    }
                    if (DdlProvincia.SelectedValue == "0")
                    {
                        new Funciones().funShowJSMessage("Seleccione Provincia..!", this);
                        return;
                    }
                    if (DdlCiudad.SelectedValue == "0")
                    {
                        new Funciones().funShowJSMessage("Seleccione Ciudad..!", this);
                        return;
                    }
                    if (!new Funciones().IsDate(TxtFechaSolicitud.Text.Trim()))
                    {
                        new Funciones().funShowJSMessage("Fecha Solicitud No Es Válida..!", this);
                        return;
                    }
                    fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
                    DateTime dtmfechasolicita = DateTime.ParseExact(TxtFechaSolicitud.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtmfechaactual = DateTime.ParseExact(fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (dtmfechasolicita < dtmfechaactual)
                    {
                        new Funciones().funShowJSMessage("Fecha Solicitud Debe ser Igual o Mayor a la Fecha Actual..!", this);
                        return;
                    }
                    if (GrdvExamenes.Rows.Count < 1)
                    {
                        new Funciones().funShowJSMessage("No Existen Exámenes Seleccionados..!", this);
                        return;
                    }
                    if (FileUpload1.HasFile==false)
                    {
                        new Funciones().funShowJSMessage("Seleccione Archivo..!", this);
                        return;
                    }
                    else
                    {
                        filePath = FileUpload1.PostedFile.FileName;
                        filename1 = Path.GetFileName(filePath);
                        ext = Path.GetExtension(filename1);
                        type = string.Empty;

                        if (filename1.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo Máximo 100 Caractéres..!", this);
                            return;
                        }

                        switch (ext)
                        {
                            case ".doc":
                            case ".docx":
                                type = "application/word";
                                break;
                            case ".pdf":
                                type = "application/pdf";
                                break;
                            case ".png":
                                type = "application/png";
                                break;
                            case ".jpg":
                                type = "application/jpg";
                                break;
                        }
                        if (type != String.Empty)
                        {
                            Stream fs = FileUpload1.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }

                    Array.Resize(ref objparam, 43);
                    objparam[0] = 0;
                    objparam[1] = DdlProducto.SelectedValue;
                    objparam[2] = DdlTipoDocumento.SelectedValue;
                    objparam[3] = TxtNumeroDocumento.Text.Trim();
                    objparam[4] = TxtPrimerNombre.Text.Trim().ToUpper();
                    objparam[5] = TxtSegundoNombre.Text.Trim().ToUpper();
                    objparam[6] = TxtPrimerApellido.Text.Trim().ToUpper();
                    objparam[7] = TxtSegundoApellido.Text.Trim().ToUpper();
                    objparam[8] = DdlGenero.SelectedValue;
                    objparam[9] = DdlEstadoCivil.SelectedValue;
                    objparam[10] = TxtFechaNacimiento.Text.Trim();
                    objparam[11] = int.Parse(DdlCiudad.SelectedValue);
                    objparam[12] = TxtDireccion.Text.Trim().ToUpper();
                    objparam[13] = TxtFonoCasa.Text.Trim();
                    objparam[14] = TxtFonoOficina.Text.Trim();
                    objparam[15] = TxtCelular.Text.Trim();
                    objparam[16] = TxtEmail.Text.Trim();
                    objparam[17] = int.Parse(DdlGrupoExamen.SelectedValue);
                    objparam[18] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[19] = TxtFechaSolicitud.Text.Trim();
                    objparam[20] = TxtObservacion.Text.Trim().ToUpper();
                    objparam[21] = bytes;
                    objparam[22] = filename1;
                    objparam[23] = type;
                    objparam[24] = ext;
                    objparam[25] = 0;
                    objparam[26] = "0";
                    objparam[27] = "0";
                    objparam[28] = "";
                    objparam[29] = 0;
                    objparam[30] = ChkEstado.Checked ? "Activo" : "Inactivo";
                    objparam[31] = TxtMonto.Text.Trim();
                    objparam[32] = "";
                    objparam[33] = "";
                    objparam[34] = "";
                    objparam[35] = "";
                    objparam[36] = 0;
                    objparam[37] = 0;
                    objparam[38] = 0;
                    objparam[39] = 0;
                    objparam[40] = 0;
                    objparam[41] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[42] = Session["MachineName"].ToString();
                    dts = new Conexion(2, "").FunInsertSolictudExamen(objparam);
                    if (dts.Tables[0].Rows.Count > 0)
                    {
                        objparam[0] = 1;
                        objparam[29] = int.Parse(dts.Tables[0].Rows[0]["CodigoEXSO"].ToString());
                        dtbexamenes = (DataTable)ViewState["Examenes"];
                        foreach (DataRow drfila in dtbexamenes.Rows)
                        {
                            objparam[25] = int.Parse(drfila["CodigoEXSE"].ToString());
                            objparam[26] = drfila["Costo"].ToString().Replace(",", ".");
                            objparam[27] = drfila["Pvp"].ToString().Replace(",", ".");
                            objparam[28] = drfila["Adicional"].ToString();
                            dts = new Conexion(2, "").FunInsertSolictudExamen(objparam);
                        }
                    }
                }
                else
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                    objparam[1] = ChkEstado.Text;
                    objparam[2] = 151;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                }
                Response.Redirect("FrmSolicitudExamenAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSolicitudExamenAdmin.aspx");
        }
        #endregion
    }
}