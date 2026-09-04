using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmSolicitudExamen : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbexamenes = new DataTable();
        Object[] objparam = new Object[1];
        Object[] objparamp = new Object[1]; //cambio aki
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
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);

            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(BtnGrabar);
            }

            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            string guardarFechaCodep = "document.getElementById('" + HdnFechaNacimientoCodep.ClientID + "').value = document.getElementById('" + TxtFechaNacimientoCodep.ClientID + "').value;";
            TxtFechaNacimientoCodep.Attributes["onchange"] = guardarFechaCodep;
            DdlProvinciaCodep.Attributes["onchange"] = guardarFechaCodep;

            //TxtPvp.Attributes.Add("onchange", "ValidarDecimales();");
            //TxtMonto.Attributes.Add("onchange", "ValidarDecimales();"); codigo comentado
            TxtFechaNacimiento.Attributes.Add("onchange", "Calcular_Edad();");
            TxtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
            if (!IsPostBack)
            {

                BtnAgregarCodependiente.Enabled = false;
                PnlCodependienteSeleccionado.Visible = false;
                LblSinCodependiente.Visible = true;

                ViewState["CodigoTITUCodependiente"] = 0;
                ViewState["CodigoPERSCodependiente"] = 0;

                CargarCombosCodependiente();

                ViewState["PersonaExiste"] = "NO";
                BtnConsultarRequisitos.Enabled = false;

                RequisitosActuales = CrearTablaRequisitos();
                BindRequisitos();

                TrDocumentoAdjunto.Visible = false;
                ViewState["TitularCargado"] = false;
                ViewState["RequisitosConsultados"] = false;
                ViewState["RequiereArchivo"] = false;
                BtnGrabar.Enabled = false;

                try
                {
                    TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaSolicitud.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm tt");
                    TxtFechaSolicitud.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm tt");
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoEXSO"] = Request["CodigoEXSO"];
                    FunCargarCombos(0);
                    if (ViewState["CodigoEXSO"].ToString() != "0")
                    {
                        DdlCampaign.Enabled = false;
                        DdlProducto.Enabled = false;
                        TxtMonto.Enabled = true;
                        TxtMontoAc.Enabled = true;
                        TxtNumeroDocumento.Text = Request["NumDocumento"];
                        DdlTipoDocumento.Enabled = false;
                        TxtNumeroDocumento.Enabled = false;
                        //TxtPrimerNombre.Enabled = false;
                        //TxtSegundoNombre.Enabled = false;
                        //TxtPrimerApellido.Enabled = false;
                        //TxtSegundoApellido.Enabled = false;
                        DdlGenero.Enabled = false;
                        DdlEstadoCivil.Enabled = false;
                        TxtFechaNacimiento.Enabled = false;
                        DdlProvincia.Enabled = false;
                        DdlCiudad.Enabled = false;
                        //TxtDireccion.Enabled = false;
                        //TxtFonoCasa.Enabled = false;
                        //TxtFonoOficina.Enabled = false;
                        //TxtCelular.Enabled = false;
                        //TxtEmail.Enabled = false;
                        TxtFechaSolicitud.Enabled = false;
                        //DdlGrupoExamen.Enabled = false;
                        //DdlExamen.Enabled = false;
                        //TxtPvp.Enabled = false;
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        //TxtObservacion.Enabled = false;
                        //ImgAddExamen.Visible = false; comentado
                        LblArchivo.Visible = false;
                        FileUpload1.Visible = false;
                        UpdRequisitos.Visible = true;
                        //panel.Visible = false;
                        //BtnGrabar.Enabled = false;
                        FunCargarCabecera();
                        FunCargaMantenimiento();
                        //FunCargarExamenes(); //codigo comentado
                        ActualizarArchivoDaquilema();
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
            //DdlProducto.SelectedIndex = 0; //codigo aumentado
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
                //DdlCampaign.SelectedValue = dtx.Tables[0].Rows[0]["CodigoCampaign"].ToString(); //cambio aki
                //DdlProducto.SelectedValue = dtx.Tables[0].Rows[0]["CodigoPROD"].ToString();
                //FunCargarCombos(2);

                //CODIGO AUMENTADO
                string codigoCampaign = dtx.Tables[0].Rows[0]["CodigoCampaign"].ToString();
                string codigoProducto = dtx.Tables[0].Rows[0]["CodigoPROD"].ToString();

                //FunCargarCampaign(); codigo comentado
                if (DdlCampaign.Items.FindByValue(codigoCampaign) != null)
                {
                    DdlCampaign.ClearSelection();
                    DdlCampaign.SelectedValue = codigoCampaign;
                }

                int campaniaId;

                if (int.TryParse(codigoCampaign, out campaniaId))
                {
                    FunCargarProductos(campaniaId);
                }
                if (DdlProducto.Items.FindByValue(codigoProducto) != null)
                {
                    DdlProducto.ClearSelection();
                    DdlProducto.SelectedValue = codigoProducto;
                }


                //DdlGrupoExamen.SelectedValue = dtx.Tables[0].Rows[0]["CodigoEXGC"].ToString();
                TxtFechaSolicitud.Text = dtx.Tables[0].Rows[0]["FechaSolicita"].ToString();
                //TxtObservacion.Text = dtx.Tables[0].Rows[0]["Observacion"].ToString();
                ChkEstado.Checked = dtx.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                ChkEstado.Text = dtx.Tables[0].Rows[0]["Estado"].ToString();
                TxtMonto.Text = dtx.Tables[0].Rows[0]["Monto"].ToString().Trim();
                TxtMontoAc.Text = dtx.Tables[0].Rows[0]["Total"].ToString().Trim();

                //Codigo titular en vase al id del producto
                //Array.Resize(ref objparamp, 3);
                //objparamp[0] = 24;//int.Parse(DdlProducto.SelectedValue);
                //objparamp[1] = TxtNumeroDocumento.Text;
                //objparamp[2] = 23;
                //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparamp);
                //ViewState["CodigoTITU"] = dts.Tables[0].Rows[0]["TITU_CODIGO"].ToString();

                //Comentado
                //objparam[0] = int.Parse(ViewState["CodigoTITU"].ToString());
                //objparam[1] = "";
                //objparam[2] = 128;
                //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                //if (dts.Tables[0].Rows.Count > 0)
                //    TxtMonto.Text = dts.Tables[0].Rows[0]["Inf10"].ToString();
                //else TxtMonto.Text = "0.00";
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
                    //objparam[0] = int.Parse(dts.Tables[0].Rows[0]["CodigoPERS"].ToString());// codigo comentado
                    //objparam[1] = "";
                    //objparam[2] = 152;
                    //dtx = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    //if (dtx.Tables[0].Rows.Count > 0)
                    //{
                    //    mensaje = "El Cliente Presenta proceso de Examen " +
                    //        dtx.Tables[0].Rows[0]["Proceso"].ToString() + " Realizado el " +
                    //        dtx.Tables[0].Rows[0]["FechaProceso"].ToString();
                    //    new Funciones().funShowJSMessage(mensaje, this);
                    //    TxtNumeroDocumento.Text = "";
                    //    return;
                    //}

                    //ViewState["PersonaExiste"] = "SI";

                    BtnConsultarRequisitos.Enabled = true;
                    //BtnGrabar.Text = "Actualizar";
                    ViewState["CodigoPERS"] = dts.Tables[0].Rows[0]["CodigoPERS"].ToString();

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
                else
                {
                    //ViewState["PersonaExiste"] = "NO";
                    //ViewState["CodigoPERS"] = "0";
                    //BtnConsultarRequisitos.Enabled = false;
                    //BtnGrabar.Text = "Crear Titular";
                    //LimpiarRequisitosAsegurabilidad();
                    //new Funciones().funShowJSMessage("Titular no registrado. Complete los datos y presione Crear Titular.",this);
                    ViewState["CodigoPERS"] = "0";
                    BtnConsultarRequisitos.Enabled = false;
                    BtnGrabar.Enabled = true;
                    BtnGrabar.Text = "Crear Titular";
                    LblEstadoRequisitos.Text = "Titular no registrado. Complete los datos y presione Crear Titular.";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private bool ExistePersonaPorDocumento()
        {
            object[] parametros = new object[3];

            parametros[0] = 0;
            parametros[1] = TxtNumeroDocumento.Text.Trim();
            parametros[2] = 22;

            DataSet dsPersona = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);

            return
                dsPersona != null &&
                dsPersona.Tables.Count > 0 &&
                dsPersona.Tables[0].Rows.Count > 0;
        }



        //private void FunCargarExamenes()
        //{
        //    try
        //    {
        //        //TrExamenes.Visible = true;
        //        Array.Resize(ref objparam, 3);
        //        objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
        //        objparam[1] = "";
        //        objparam[2] = 143;
        //        dts= new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //        //GrdvExamenes.DataSource = dts;
        //        //GrdvExamenes.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion) //consulta a la tabla de Expert_CLIENTE_USUARIOS
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

                        //Array.Resize(ref objparam, 11);
                        //objparam[0] = 14;
                        //objparam[1] = "";
                        //objparam[2] = "";
                        //objparam[3] = "";
                        //objparam[4] = "";
                        //objparam[5] = "";
                        //objparam[6] = codigocamp;
                        //objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                        //objparam[8] = 0;
                        //objparam[9] = 0;
                        //objparam[10] = 0;
                        //dts = new Conexion(2, "").FunConsultaDatos1(objparam);
                        //if (dts.Tables[0].Rows.Count > 0)
                        //{
                        //    DdlProducto.DataSource = dts;
                        //    DdlProducto.DataTextField = "Descripcion";
                        //    DdlProducto.DataValueField = "Codigo";
                        //    DdlProducto.DataBind();
                        //}

                        Array.Resize(ref objparam, 1);
                        objparam[0] = 6;
                        DdlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        DdlProvincia.DataTextField = "Descripcion";
                        DdlProvincia.DataValueField = "Codigo";
                        DdlProvincia.DataBind();

                        //codigo aumentado para llenar combo campaign
                        //Array.Resize(ref objparam, 1);
                        //objparam[0] = 64;
                        //DdlCampaign.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        //DdlCampaign.DataTextField = "Descripcion";
                        //DdlCampaign.DataValueField = "Codigo";
                        //DdlCampaign.DataBind();

                        //cambiar aki
                        Array.Resize(ref objparam, 3);
                        objparam[0] = codigocamp;
                        objparam[1] = "";
                        objparam[2] = 224;
                        DdlCampaign.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlCampaign.DataTextField = "Descripcion";
                        DdlCampaign.DataValueField = "Codigo";
                        DdlCampaign.DataBind();


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
                        //DdlExamen.DataSource = dts;
                        //DdlExamen.DataTextField = "Descripcion";
                        //DdlExamen.DataValueField = "Codigo";
                        //DdlExamen.DataBind();
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
                    //Array.Resize(ref objparam, 3);
                    //objparam[0] = int.Parse(DdlProducto.SelectedValue); //codigo comentado
                    //objparam[1] = "";
                    //objparam[2] = 148;

                    //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    //ViewState["Costo"] = dts.Tables[0].Rows[0]["Costo"].ToString();

                    //objparam[2] = 139; // codigo comentado
                    //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    //if (dts.Tables[0].Rows.Count > 1)
                    //{
                    //    DdlGrupoExamen.DataSource = dts;
                    //    DdlGrupoExamen.DataTextField = "Descripcion";
                    //    DdlGrupoExamen.DataValueField = "Codigo";
                    //    DdlGrupoExamen.DataBind();
                    //    LblTituloExa.Visible = true;
                    //    TrExamenes.Visible = true;
                    //}
                    //break;
                    case 3:
                        DdlProducto.Items.Clear(); // codigo aumentado
                        Array.Resize(ref objparam, 3);
                        objparam[0] = DdlCampaign.SelectedValue;
                        objparam[1] = "";
                        objparam[2] = 220;
                        DdlProducto.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlProducto.DataTextField = "Descripcion";
                        DdlProducto.DataValueField = "Codigo";
                        DdlProducto.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        private void LimpiarFormularioNuevoCodependiente()
        {
            // Documento
            TxtNumeroDocumentoCodep.Text = "";


            // Nombres
            TxtPrimerNombreCodep.Text = "";
            TxtSegundoNombreCodep.Text = "";

            TxtPrimerApellidoCodep.Text = "";
            TxtSegundoApellidoCodep.Text = "";


            // Fecha
            TxtFechaNacimientoCodep.Text = "";


            // Contacto
            TxtEmailCodep.Text = "";

            TxtDireccionCodep.Text = "";

            TxtFonoCasaCodep.Text = "";
            TxtFonoOficinaCodep.Text = "";
            TxtCelularCodep.Text = "";


            // Tipo documento
            if (DdlTipoDocumentoCodep.Items.Count > 0)
            {
                DdlTipoDocumentoCodep.SelectedIndex = 0;
            }


            // Género
            if (DdlGeneroCodep.Items.Count > 0)
            {
                DdlGeneroCodep.SelectedIndex = 0;
            }


            // Estado civil
            if (DdlEstadoCivilCodep.Items.Count > 0)
            {
                DdlEstadoCivilCodep.SelectedIndex = 0;
            }


            // Provincia
            if (DdlProvinciaCodep.Items.Count > 0)
            {
                DdlProvinciaCodep.SelectedIndex = 0;

                CargarCiudadesCodependiente();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblTituloExa.Visible = false;
            ActualizarArchivoDaquilema();
            //TrExamenes.Visible = false;
            //GrdvExamenes.DataSource = null;
            //GrdvExamenes.DataBind();
            //FunCargarCombos(2);
            LimpiarRequisitosAsegurabilidad();
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

        protected void DdlCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(3);
            FunLimpiarCampos();
            LimpiarRequisitosAsegurabilidad();

            ViewState["TitularCargado"] = false;

        }

        protected void TxtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroDocumento = TxtNumeroDocumento.Text.Trim();
                string tipoDocumento = DdlTipoDocumento.SelectedValue;

                FunLimpiarCampos();
                LimpiarRequisitosAsegurabilidad();

                TxtNumeroDocumento.Text = numeroDocumento;

                if (DdlTipoDocumento.Items.FindByValue(tipoDocumento) != null)
                {
                    DdlTipoDocumento.SelectedValue = tipoDocumento;
                }

                FunCargaMantenimiento();
                ActualizarEstadoCodependiente();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }

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
                    //DdlGrupoExamen.SelectedValue = "0";
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

                //CODIGO COMENTADO
                //Array.Resize(ref objparam, 3);
                //objparam[0] = int.Parse(DdlGrupoExamen.SelectedValue);
                //objparam[1] = "";
                //objparam[2] = 149;
                //dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
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

                //codigo comentado columna de Monto
                //if (resultmonto != null || resultmonto.Count() > 0)
                //{
                //    if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                //    {
                //        new Funciones().funShowJSMessage("Ingrese Monto Solicitado..!", this);
                //        DdlGrupoExamen.SelectedValue = "0";
                //        return;
                //    }
                //    if (resultmonto.Count() == 1)
                //    {
                //        if (resultmonto[0]["Operador"].ToString() == "=")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) == decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == ">")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == ">=")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == "<")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == "<=")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[0]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }
                //    }
                //    else if (resultmonto.Count() == 2)
                //    {
                //        if (resultmonto[0]["Operador"].ToString() == ">" && resultmonto[1]["Operador"].ToString() == "<")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                //                decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == ">=" && resultmonto[1]["Operador"].ToString() == "<")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                //                decimal.Parse(TxtMonto.Text.Trim()) < decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == ">" && resultmonto[1]["Operador"].ToString() == "<=")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) > decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                //                decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }

                //        if (resultmonto[0]["Operador"].ToString() == ">=" && resultmonto[1]["Operador"].ToString() == "<=")
                //        {
                //            if (decimal.Parse(TxtMonto.Text.Trim()) >= decimal.Parse(resultmonto[0]["Valor"].ToString()) &&
                //                decimal.Parse(TxtMonto.Text.Trim()) <= decimal.Parse(resultmonto[1]["Valor"].ToString())) continuarmonto = true;
                //            else continuarmonto = false;
                //        }
                //    }
                //    else if (resultmonto.Count() == 0) continuarmonto = true;
                //    else continuarmonto = false;
                //}
                //else continuarmonto = true;

                //CODIGO COMENTADO
                //if (continuaredad && continuargenero && continuarmonto)
                //{
                //    Array.Resize(ref objparam, 3);
                //    objparam[0] = int.Parse(DdlGrupoExamen.SelectedValue);
                //    objparam[1] = "";
                //    objparam[2] = 141;
                //    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                //    ViewState["Examenes"] = dts.Tables[0];
                //    GrdvExamenes.DataSource = dts;
                //    GrdvExamenes.DataBind();
                //    if (dts.Tables[0].Rows.Count > 0) TrExamenes.Visible = true;
                //}
                //else
                //{
                //    new Funciones().funShowJSMessage("Configuración Exámenes NO Está Definida para los Parámetros del Cliente..!", this);
                //    DdlExamen.SelectedValue = "0";
                //    DdlGrupoExamen.SelectedValue = "0";
                //    dtbexamenes.Clear();
                //    ViewState["Examenes"] = dtbexamenes;
                //    GrdvExamenes.DataSource = dtbexamenes;
                //    GrdvExamenes.DataBind();                    
                //}
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //protected void ImgAddExamen_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (ViewState["Examenes"] == null)
        //        {
        //            new Funciones().funShowJSMessage("Primero Seleccione Grupo Exámenes..!", this);
        //            return;
        //        }
        //        if (DdlExamen.SelectedValue == "0")
        //        {
        //            new Funciones().funShowJSMessage("Seleccione Examen..!", this);
        //            return;
        //        }
        //        if (ViewState["Examenes"] != null)
        //        {
        //            dtbexamenes = (DataTable)ViewState["Examenes"];
        //            resultado = dtbexamenes.Select("CodigoEXSE='" + DdlExamen.SelectedValue + "'").FirstOrDefault();
        //            if (resultado != null) lexiste = true;
        //        }
        //        if (lexiste)
        //        {
        //            new Funciones().funShowJSMessage("Examen ya se encuentra Agregado..!", this);
        //            return;
        //        }
        //        Array.Resize(ref objparam, 3);
        //        objparam[0] = int.Parse(DdlExamen.SelectedValue);
        //        objparam[1] = "";
        //        objparam[2] = 138;
        //        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
        //        dtbexamenes = (DataTable)ViewState["Examenes"];
        //        filagre = dtbexamenes.NewRow();
        //        filagre["CodigoEXSE"] = DdlExamen.SelectedValue;
        //        filagre["Categoria"] = dts.Tables[0].Rows[0]["Categoria"].ToString();
        //        filagre["Examen"] = DdlExamen.SelectedItem.ToString();
        //        filagre["Costo"] = dts.Tables[0].Rows[0]["Valor"].ToString().Replace(".",",");
        //        filagre["Pvp"] = "0";
        //        filagre["Adicional"] = "SI";
        //        dtbexamenes.Rows.Add(filagre);
        //        dtbexamenes.DefaultView.Sort = "Examen";
        //        ViewState["Examenes"] = dtbexamenes;
        //        GrdvExamenes.DataSource = dtbexamenes;
        //        GrdvExamenes.DataBind();
        //        DdlExamen.SelectedValue = "0";
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        //protected void ImgDelExamen_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //        codigo = GrdvExamenes.DataKeys[gvRow.RowIndex].Values["CodigoEXSE"].ToString();
        //        //Buscar si no existe Efecto agregado
        //        dtbexamenes = (DataTable)ViewState["Examenes"];
        //        resultado = dtbexamenes.Select("CodigoEXSE='" + codigo + "'").FirstOrDefault();
        //        resultado.Delete();
        //        dtbexamenes.AcceptChanges();
        //        ViewState["Examenes"] = dtbexamenes;
        //        GrdvExamenes.DataSource = dtbexamenes;
        //        GrdvExamenes.DataBind();
        //        if (dtbexamenes.Rows.Count == 0) TrExamenes.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.Message;
        //    }
        //}

        //protected void GrdvExamenes_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowIndex >= 0)
        //        {
        //            imgeliminar = (ImageButton)(e.Row.Cells[2].FindControl("ImgDelExamen"));
        //            adicional = GrdvExamenes.DataKeys[e.Row.RowIndex].Values["Adicional"].ToString();
        //            if (adicional == "SI" && ViewState["CodigoEXSO"].ToString() == "0")
        //            {
        //                imgeliminar.ImageUrl = "~/Botones/eliminar.png";
        //                imgeliminar.Height = 15;                        
        //                imgeliminar.Enabled = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["CodigoEXSO"].ToString() == "0")
                {
                    bool personaExiste = ExistePersonaPorDocumento();

                    if (!personaExiste)
                    {
                        if (!ValidarTitularNuevo())
                        {
                            return;
                        }

                        /* CREAR PERSONA + TITULAR */

                        DataSet dsTitular = GuardarTitularNuevo();

                        if (dsTitular == null || dsTitular.Tables.Count == 0 || dsTitular.Tables[0].Rows.Count == 0)
                        {
                            Lblerror.Text = "No fue posible crear el titular.";
                            return;
                        }

                        string resultadoTitular = dsTitular.Tables[0].Rows[0]["Resultado"].ToString().Trim();

                        if (resultadoTitular != "OK-TITULAR")
                        {
                            Lblerror.Text = "No fue posible crear el titular.";
                            return;
                        }

                        Response.Redirect("FrmSolicitudExamenAdmin.aspx" + "?MensajeRetornado='Titular creado correctamente'", true);
                        return;
                    }

                    DataTable requisitosGuardar = null;

                    if (TxtMonto.Text.Trim() == "0" || TxtMonto.Text.Trim() == "0.0" || TxtMonto.Text.Trim() == "0.00")
                    {
                        new Funciones().funShowJSMessage("Ingrese Monto Anterior..!", this);
                        return;
                    }

                    if (TxtMontoAc.Text.Trim() == "0" || TxtMontoAc.Text.Trim() == "0.0" || TxtMontoAc.Text.Trim() == "0.00")
                    {
                        new Funciones().funShowJSMessage("Ingrese Monto Actual..!", this);
                        return;
                    }

                    decimal montoAnterior = 0m;
                    decimal montoTotal = 0m;
                    string valorMontoAnterior = TxtMonto.Text.Trim().Replace(",", ".");
                    string valorMontoTotal = TxtMontoAc.Text.Trim().Replace(",", ".");

                    if (valorMontoAnterior == "")
                    {
                        valorMontoAnterior = "0";
                    }

                    if (valorMontoTotal == "")
                    {
                        valorMontoTotal = "0";
                    }

                    if (!decimal.TryParse(valorMontoAnterior, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out montoAnterior))
                    {
                        new Funciones().funShowJSMessage("El Monto Anterior no tiene un formato válido.", this);
                        return;
                    }

                    if (!decimal.TryParse(valorMontoTotal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out montoTotal))
                    {
                        new Funciones().funShowJSMessage("El Monto Total no tiene un formato válido.", this);
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

                    fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
                    //DateTime dtmfechasolicita = DateTime.ParseExact(TxtFechaSolicitud.Text.Trim(), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    DateTime dtmfechasolicita = DateTime.Now;
                    DateTime dtmfechaactual = DateTime.ParseExact(fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    bytes = new byte[0];
                    filename1 = "";
                    ext = "";
                    type = "";

                    if (FileUpload1.HasFile)
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

                        switch (ext.ToLower())
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
                            case ".jpeg":

                                type = "application/jpg";
                                break;
                        }

                        if (type == String.Empty)
                        {
                            new Funciones().funShowJSMessage("Seleccione archivos de tipo (.doc,.docx,.pdf,.png,.jpg,.jpeg)", this);
                            return;
                        }

                        Stream fs = FileUpload1.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }

                    DataSet dsRequisitos = ObtenerRequisitosAsegurabilidad();

                    if (dsRequisitos == null || dsRequisitos.Tables.Count == 0 || dsRequisitos.Tables[0].Rows.Count == 0)
                    {
                        Lblerror.Text = "No fue posible validar los requisitos.";
                        return;
                    }

                    string resultadoRegla = dsRequisitos.Tables[0].Rows[0]["RESULTADO"].ToString().Trim();

                    if (resultadoRegla != "REQUISITOS")
                    {
                        new Funciones().funShowJSMessage("No corresponde generar solicitud de exámenes", this);
                        return;
                    }

                    if (dsRequisitos.Tables.Count < 2 || dsRequisitos.Tables[1].Rows.Count == 0)
                    {
                        Lblerror.Text = "No existen requisitos configurados.";
                        return;
                    }

                    requisitosGuardar = dsRequisitos.Tables[1].Copy();

                    bool requiereArchivo = Convert.ToBoolean(dsRequisitos.Tables[0].Rows[0]["REQUIERE_ARCHIVO"]);

                    if (requiereArchivo && FileUpload1.HasFile == false)
                    {
                        new Funciones().funShowJSMessage("Debe adjuntar el Formulario de Declaración de Salud.", this);
                        return;
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
                    objparam[17] = /*int.Parse(DdlGrupoExamen.SelectedValue)*/0; //cambio aki
                    objparam[18] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[19] = TxtFechaSolicitud.Text.Trim();
                    objparam[20] = "";//TxtObservacion.Text.Trim().ToUpper();
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
                    objparam[31] = FormatearDecimal(TxtMonto.Text);
                    objparam[32] = FormatearDecimal(TxtMontoAc.Text);
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

                    if (dts != null && dts.Tables.Count > 0 && dts.Tables[0].Rows.Count > 0)
                    {
                        int codigoEXSO = Convert.ToInt32(dts.Tables[0].Rows[0]["CodigoEXSO"]);
                        ViewState["CodigoEXSO"] = codigoEXSO;
                        GuardarCodependienteSolicitud(codigoEXSO);
                        GuardarExamenAdicional(codigoEXSO);
                        GuardarDocumentoDaquilema(codigoEXSO);
                        GuardarRequisitosSolicitud(codigoEXSO, requisitosGuardar);
                        byte[] pdf = GenerarPdfSolicitud(codigoEXSO);
                        GuardarPdfSolicitud(codigoEXSO, pdf);

                    }
                    else
                    {
                        Lblerror.Text = "No se pudo obtener el código de la solicitud.";
                        return;
                    }

                    //enviar email
                    string body = "NUEVA SOLICITUD DE EXAMEN " + "<br/> ";
                    body += "------------------------------------------------------------------" + "<br/>";
                    body += "CEDULA:" + " " + TxtNumeroDocumento.Text.Trim() + "<br/>";
                    body += "TITULAR:" + " " + TxtPrimerNombre.Text.Trim().ToUpper() + " " + TxtPrimerApellido.Text.Trim().ToUpper() + "<br/>";

                    string correo = new Funciones().SendHtmlEmailExamen("vroldan@prestasalud.com", "SOLICITUD EXAMEN", body, "mail.prestasalud.com", 587,
                        true, "info@prestasalud.com", "Info.Presta.2025$", "", "", "ealvear@prestasalud.com,vgavilanez@prestasalud.com");


                    //codigo comentado
                    //if (dts.Tables[0].Rows.Count > 0)
                    //{
                    //    objparam[0] = 1;
                    //    objparam[29] = int.Parse(dts.Tables[0].Rows[0]["CodigoEXSO"].ToString());
                    //    dtbexamenes = (DataTable)ViewState["Examenes"];
                    //    foreach (DataRow drfila in dtbexamenes.Rows)
                    //    {
                    //        objparam[25] = int.Parse(drfila["CodigoEXSE"].ToString());
                    //        objparam[26] = drfila["Costo"].ToString().Replace(",", ".");
                    //        objparam[27] = drfila["Pvp"].ToString().Replace(",", ".");
                    //        objparam[28] = drfila["Adicional"].ToString();
                    //        dts = new Conexion(2, "").FunInsertSolictudExamen(objparam);
                    //    }
                    //}
                }
                else
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                    objparam[1] = ChkEstado.Text;
                    objparam[2] = 151;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                    //codigo aumentado
                    int codper = 0;

                    if (dts != null && dts.Tables.Count > 0 && dts.Tables[0].Rows.Count > 0)
                    {
                        codper = Convert.ToInt32(dts.Tables[0].Rows[0]["codper"]);
                    }

                    Array.Resize(ref objparam, 11);
                    objparam[0] = 43;
                    objparam[1] = TxtPrimerNombre.Text.Trim().ToUpper();
                    objparam[2] = TxtPrimerApellido.Text.Trim().ToUpper();
                    objparam[3] = TxtDireccion.Text.Trim().ToUpper();
                    objparam[4] = TxtFonoCasa.Text.Trim();
                    objparam[5] = TxtCelular.Text.Trim();
                    objparam[6] = codper;
                    objparam[7] = 0;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);

                    string resultado = dts.Tables[0].Rows[0]["Resultado"].ToString();

                    if (resultado.Trim() == "OK UPDATE")
                    {
                        Response.Redirect("FrmSolicitudExamenAdmin.aspx?MensajeRetornado='Actualizado con Éxito'", true);
                    }

                }
                Response.Redirect("FrmSolicitudExamenAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
                Context.ApplicationInstance.CompleteRequest();
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

        //CODIGO NUEVO
        private void FunCargarCampaign()
        {
            try
            {
                object[] parametros = new object[3];

                parametros[0] = 0;
                parametros[1] = "";
                parametros[2] = 221;

                DataSet dsCampaign = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);

                DdlCampaign.Items.Clear();

                if (dsCampaign != null &&
                    dsCampaign.Tables.Count > 0 &&
                    dsCampaign.Tables[0].Rows.Count > 0)
                {
                    DdlCampaign.DataSource = dsCampaign.Tables[0];

                    DdlCampaign.DataValueField = "CAMP_CODIGO";
                    DdlCampaign.DataTextField = "camp_nombre";

                    DdlCampaign.DataBind();
                }

                DdlCampaign.Items.Insert(0, new ListItem("-- Seleccione campaña --", "0"));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        private void FunCargarProductos(int codigoCampaign)
        {
            try
            {
                object[] parametros = new object[3];

                parametros[0] = codigoCampaign;
                parametros[1] = "";
                parametros[2] = 222;

                DataSet dsProductos = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);

                DdlProducto.Items.Clear();

                if (dsProductos != null &&
                    dsProductos.Tables.Count > 0 &&
                    dsProductos.Tables[0].Rows.Count > 0)
                {
                    DdlProducto.DataSource = dsProductos.Tables[0];
                    DdlProducto.DataValueField = "PROD_CODIGO";
                    DdlProducto.DataTextField = "prod_nombre";
                    DdlProducto.DataBind();
                }

                DdlProducto.Items.Insert(0, new ListItem("-- Seleccione producto --", "0"));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private string FormatearDecimal(string valor)
        {
            decimal numero = 0m;

            if (string.IsNullOrWhiteSpace(valor))
            {
                return "0.00";
            }

            valor = valor.Trim().Replace(",", ".");

            if (!decimal.TryParse(valor, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out numero))
            {
                throw new Exception("El valor '" + valor + "' no es un número decimal válido.");
            }

            return numero.ToString("0.00", CultureInfo.InvariantCulture);
        }

        //CODIGO AGREGADO PARA LISTAR EXAMENES
        private DataTable CrearTablaExamenesSeleccionados()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EXPR_CODIGO", typeof(int));
            dt.Columns.Add("EXAMEN", typeof(string));
            dt.Columns.Add("COSTO", typeof(decimal));
            dt.Columns.Add("PVP", typeof(decimal));
            return dt;
        }

        private DataTable ExamenesSeleccionados
        {
            get
            {
                if (ViewState["ExamenesSeleccionados"] == null)
                {
                    ViewState["ExamenesSeleccionados"] = CrearTablaExamenesSeleccionados();
                }

                return (DataTable)ViewState["ExamenesSeleccionados"];
            }

            set
            {
                ViewState["ExamenesSeleccionados"] = value;
            }
        }

        //private void BindExamenesSeleccionados()
        //{
        //    GrdvExamenesSeleccionados.DataSource = ExamenesSeleccionados;

        //    GrdvExamenesSeleccionados.DataBind();
        //    LblCantidadExamenes.Text = ExamenesSeleccionados.Rows.Count.ToString() + " examen(es)";
        //}

        //protected void GrdvExamenesSeleccionados_RowCommand(object sender,GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName != "Quitar")
        //    {
        //        return;
        //    }

        //    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

        //    int examenId =Convert.ToInt32(GrdvExamenesSeleccionados.DataKeys[row.RowIndex].Value);

        //    DataTable dt =ExamenesSeleccionados;
        //    DataRow[] filas = dt.Select("EXPR_CODIGO = " + examenId.ToString());

        //    if (filas.Length > 0)
        //    {
        //        dt.Rows.Remove(filas[0]);
        //    }

        //    ExamenesSeleccionados = dt;

        //    // Refrescar seleccionados
        //    BindExamenesSeleccionados();
        //    CargarExamenesDisponibles();
        //}

        private DataTable ObtenerTodosLosExamenes()
        {
            DataSet ds = new DataSet();
            object[] parametros = new object[0];
            ds = new Conexion(2, "").funConsultarSqls("sp_SE_Examenes_Listar", parametros);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }
        private void CargarExamenesDisponibles()
        {
            DataTable dt =
                ObtenerTodosLosExamenes();

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                int examenId = Convert.ToInt32(dt.Rows[i]["EXPR_CODIGO"]);
                DataRow[] seleccionado = ExamenesSeleccionados.Select("EXPR_CODIGO = " + examenId.ToString());

                if (seleccionado.Length > 0)
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            /* =========================================
               BUSCADOR
               ========================================= */

            //string buscar = TxtBuscarExamen.Text.Trim();

            //if (!string.IsNullOrWhiteSpace(buscar))
            //{
            //    for(int i = dt.Rows.Count - 1;i >= 0;i--)
            //    {
            //        string examen = dt.Rows[i]["EXAMEN"].ToString();
            //        if (examen.IndexOf(buscar,StringComparison.OrdinalIgnoreCase)< 0)
            //        {
            //            dt.Rows.RemoveAt(i);
            //        }
            //    }
            //}


            /* =========================================
               CONTROL PAGINACIÓN
               ========================================= */

            int registros = dt.Rows.Count;
            int paginas = 0;

            //if (GrdvExamenesDisponibles.PageSize > 0)
            //{
            //    paginas = (int)Math.Ceiling((double)registros/GrdvExamenesDisponibles.PageSize);
            //}

            //if (paginas == 0)
            //{
            //    GrdvExamenesDisponibles.PageIndex = 0;
            //}
            //else if (GrdvExamenesDisponibles.PageIndex >= paginas)
            //{
            //    GrdvExamenesDisponibles.PageIndex = paginas - 1;
            //}

            //GrdvExamenesDisponibles.DataSource = dt;
            //GrdvExamenesDisponibles.DataBind();
        }
        protected void BtnBuscarExamen_Click(object sender, EventArgs e)
        {
            //GrdvExamenesDisponibles.PageIndex = 0;
            //CargarExamenesDisponibles();
        }
        protected void BtnLimpiarExamen_Click(object sender, EventArgs e)
        {
            //TxtBuscarExamen.Text = "";
            //GrdvExamenesDisponibles.PageIndex = 0;
            //CargarExamenesDisponibles();
        }
        protected void GrdvExamenesDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GrdvExamenesDisponibles.PageIndex = e.NewPageIndex;
            //CargarExamenesDisponibles();
        }

        //protected void GrdvExamenesDisponibles_RowCommand(object sender,GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName != "Agregar")
        //    {
        //        return;
        //    }


        //    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        //    int examenId = Convert.ToInt32(GrdvExamenesDisponibles.DataKeys[row.RowIndex].Value);
        //    string examen = Server.HtmlDecode(row.Cells[0].Text).Trim();

        //    DataTable dt = ExamenesSeleccionados;

        //    DataRow[] existe = dt.Select("EXPR_CODIGO = " + examenId.ToString());
        //    if (existe.Length > 0)
        //    {
        //        return;
        //    }

        //    DataRow nueva = dt.NewRow();
        //    nueva["EXPR_CODIGO"] = examenId;
        //    nueva["EXAMEN"] = examen;
        //    nueva["COSTO"] = 0m;
        //    nueva["PVP"] = 0m;
        //    dt.Rows.Add(nueva);

        //    ExamenesSeleccionados = dt;


        //    BindExamenesSeleccionados();


        //    CargarExamenesDisponibles();
        //}


        //private void GuardarExamenesSeleccionados(int codigoEXSO)
        //{
        //    foreach (DataRow fila in ExamenesSeleccionados.Rows)
        //    {
        //        GuardarDetalleExamen(codigoEXSO, fila);
        //    }
        //}



        private void GuardarDetalleExamen(int codigoEXSO, DataRow fila)
        {
            object[] parametros = new object[43];

            parametros[0] = 1;
            parametros[1] = 0;
            parametros[2] = "";
            parametros[3] = "";
            parametros[4] = "";
            parametros[5] = "";
            parametros[6] = "";
            parametros[7] = "";
            parametros[8] = "";
            parametros[9] = "";
            parametros[10] = "";
            parametros[11] = 0;
            parametros[12] = "";
            parametros[13] = "";
            parametros[14] = "";
            parametros[15] = "";
            parametros[16] = "";
            parametros[17] = 0;
            parametros[18] = int.Parse(Session["usuCodigo"].ToString());
            parametros[19] = DateTime.Now;
            parametros[20] = "";
            parametros[21] = new byte[0];
            parametros[22] = "";
            parametros[23] = "";
            parametros[24] = "";
            parametros[25] = Convert.ToInt32(fila["EXPR_CODIGO"]);
            parametros[26] = Convert.ToDecimal(fila["COSTO"]).ToString("0.00", CultureInfo.InvariantCulture);
            parametros[27] = Convert.ToDecimal(fila["PVP"]).ToString("0.00", CultureInfo.InvariantCulture);
            parametros[28] = "0";
            parametros[29] = codigoEXSO;
            parametros[30] = "Activo";
            parametros[31] = "";
            parametros[32] = "";
            parametros[33] = "";
            parametros[34] = "";
            parametros[35] = "";
            parametros[36] = 0;
            parametros[37] = 0;
            parametros[38] = 0;
            parametros[39] = 0;
            parametros[40] = 0;
            parametros[41] = int.Parse(Session["usuCodigo"].ToString());
            parametros[42] = Session["MachineName"].ToString();

            new Conexion(2, "").FunInsertSolictudExamen(parametros);
        }

        //PARAMETROS EXAMENES
        private object[] CrearParametrosOperacionSolicitud(int tipo, int codigoEXSO)
        {
            object[] parametros = new object[43];

            parametros[0] = tipo;
            parametros[1] = 0;
            parametros[2] = "";
            parametros[3] = "";
            parametros[4] = "";
            parametros[5] = "";
            parametros[6] = "";
            parametros[7] = "";
            parametros[8] = "";
            parametros[9] = "";
            parametros[10] = "";
            parametros[11] = 0;
            parametros[12] = "";
            parametros[13] = "";
            parametros[14] = "";
            parametros[15] = "";
            parametros[16] = "";
            parametros[17] = 0;
            parametros[18] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[19] = DateTime.Now;
            parametros[20] = "";
            parametros[21] = new byte[0];
            parametros[22] = "";
            parametros[23] = "";
            parametros[24] = "";
            parametros[25] = 0;
            parametros[26] = "0.00";
            parametros[27] = "0.00";
            parametros[28] = "0";
            parametros[29] = codigoEXSO;
            parametros[30] = "Activo";
            parametros[31] = "";
            parametros[32] = "";
            parametros[33] = "";
            parametros[34] = "";
            parametros[35] = "";
            parametros[36] = 0;
            parametros[37] = 0;
            parametros[38] = 0;
            parametros[39] = 0;
            parametros[40] = 0;
            parametros[41] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";
            return parametros;
        }

        //Obtener datos para generar el PDF
        private DataSet ObtenerDatosSolicitudPdf(int codigoEXSO)
        {
            object[] parametros = CrearParametrosOperacionSolicitud(5, codigoEXSO);
            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);
            return ds;
        }

        //funcion auxiliar para obtener valores
        private string ObtenerValor(DataRow fila, string columna)
        {
            if (fila == null || !fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
            {
                return "";
            }

            return fila[columna].ToString().Trim();
        }

        //crear el PDF
        private byte[] GenerarPdfSolicitud(int codigoEXSO)
        {
            DataSet ds = ObtenerDatosSolicitudPdf(codigoEXSO);

            if (ds == null || ds.Tables.Count < 2)
            {
                throw new Exception("No se encontraron los datos necesarios para generar el PDF.");
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No se encontró la solicitud.");
            }

            DataRow titular = ds.Tables[0].Rows[0];
            DataTable examenes = ds.Tables[1];
            DataRow codependiente = null;

            if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0)
            {
                codependiente = ds.Tables[2].Rows[0];
            }
            string examenAdicional = ObtenerValor(titular, "EXAMEN_ADICIONAL");
            PdfDocument documento = new PdfDocument();
            documento.Info.Title = "Solicitud de Exámenes " + codigoEXSO.ToString();
            PdfPage pagina = documento.AddPage();
            pagina.Size = PdfSharp.PageSize.A4;
            XGraphics gfx = XGraphics.FromPdfPage(pagina);
            XFont fuenteTitulo = new XFont("Arial", 16, XFontStyle.Bold);
            XFont fuenteSubtitulo = new XFont("Arial", 11, XFontStyle.Bold);
            XFont fuenteNormal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont fuenteNegrita = new XFont("Arial", 9, XFontStyle.Bold);
            double margen = 40;
            double y = 40;

            gfx.DrawString("SOLICITUD DE EXÁMENES",fuenteTitulo,XBrushes.Black,new XRect(margen,y,pagina.Width - 80,30),XStringFormats.TopCenter);
            y += 40;

            gfx.DrawString("Nro. Solicitud:",fuenteNegrita,XBrushes.Black,margen,y);
            gfx.DrawString(codigoEXSO.ToString(),fuenteNormal,XBrushes.Black,margen + 90,y);

            string fechaSolicitud = "";

            if (titular["exso_fechasolicita"] != DBNull.Value)
            {
                DateTime fecha = Convert.ToDateTime(titular["exso_fechasolicita"]);

                fechaSolicitud = fecha.ToString("dd/MM/yyyy HH:mm");
            }

            gfx.DrawString("Fecha:",fuenteNegrita,XBrushes.Black,350,y);

            gfx.DrawString(fechaSolicitud,fuenteNormal,XBrushes.Black,395,y);

            y += 30;

            gfx.DrawString("DATOS DEL TITULAR",fuenteSubtitulo,XBrushes.Black,margen,y);
            y += 8;

            gfx.DrawLine(XPens.Gray,margen,y,pagina.Width - margen,y);
            y += 20;

            string nombres = ObtenerValor(titular, "pers_primernombre") + " " + ObtenerValor(titular, "pers_segundonombre");
            string apellidos = ObtenerValor(titular, "pers_primerapellido") + " " + ObtenerValor(titular, "pers_segundoapellido");

            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Documento:",ObtenerValor(titular,"pers_numerodocumento"));
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Género:",ObtenerValor(titular,"pers_genero"));

            y += 24;

            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Nombres:",nombres.Trim());
            y += 24;
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Apellidos:",apellidos.Trim());
            y += 24;

            string fechaNacimiento = "";

            if (titular["pers_fechanacimiento"] != DBNull.Value)
            {
                fechaNacimiento = Convert.ToDateTime(titular["pers_fechanacimiento"]).ToString("dd/MM/yyyy");
            }

            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Fecha nacimiento:",fechaNacimiento);
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Estado civil:",ObtenerValor(titular,"pers_estadocivil"));
            y += 24;
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Teléfono:",ObtenerValor(titular,"pers_telefonocasa"));
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Celular:",ObtenerValor(titular,"pers_celular"));
            y += 24;
            DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Email:",ObtenerValor(titular,"pers_email"));
            y += 30;
            gfx.DrawString("Dirección:",fuenteNegrita,XBrushes.Black,margen,y);
            XTextFormatter direccionFormatter = new XTextFormatter(gfx);
            direccionFormatter.DrawString(ObtenerValor(titular,"pers_direccion"),fuenteNormal,XBrushes.Black,new XRect(margen + 70,y - 10,pagina.Width - margen - 110,35),XStringFormats.TopLeft);
            y += 50;
            if (codependiente != null)
            {
                if (y > 610)
                {
                    gfx.Dispose();

                    pagina = documento.AddPage();
                    pagina.Size = PdfSharp.PageSize.A4;
                    gfx = XGraphics.FromPdfPage(pagina);
                    y = 40;
                    gfx.DrawString("SOLICITUD DE EXÁMENES - " + codigoEXSO.ToString(),fuenteSubtitulo,XBrushes.Black,margen,y);
                    y += 30;
                }

                gfx.DrawString("DATOS DEL CODEPENDIENTE",fuenteSubtitulo,XBrushes.Black,margen,y);
                y += 8;

                gfx.DrawLine(XPens.Gray,margen,y,pagina.Width - margen,y);
                y += 20;

                string nombresCodependiente = ObtenerValor(codependiente,"pers_primernombre") + " " + ObtenerValor(codependiente,"pers_segundonombre");
                string apellidosCodependiente = ObtenerValor(codependiente,"pers_primerapellido") + " " + ObtenerValor(codependiente,"pers_segundoapellido");

                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Documento:",ObtenerValor(codependiente,"pers_numerodocumento"));
                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Género:",ObtenerValor(codependiente,"pers_genero"));
                y += 24;

                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Nombres:",nombresCodependiente.Trim());
                y += 24;

                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Apellidos:",apellidosCodependiente.Trim());
                y += 24;

                string fechaNacimientoCodependiente = "";

                if (codependiente["pers_fechanacimiento"] != DBNull.Value)
                {
                    fechaNacimientoCodependiente = Convert.ToDateTime(codependiente["pers_fechanacimiento"]).ToString("dd/MM/yyyy");
                }


                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Fecha nacimiento:",fechaNacimientoCodependiente);
                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Estado civil:",ObtenerValor(codependiente,"pers_estadocivil"));
                y += 24;

                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Teléfono:",ObtenerValor(codependiente,"pers_telefonocasa"));
                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,320,y,"Celular:",ObtenerValor(codependiente,"pers_celular"));
                y += 24;

                DibujarCampoPdf(gfx,fuenteNegrita,fuenteNormal,margen,y,"Email:",ObtenerValor(codependiente,"pers_email"));
                y += 30;

                gfx.DrawString("Dirección:",fuenteNegrita,XBrushes.Black,margen,y);

                XTextFormatter direccionCodepFormatter = new XTextFormatter(gfx);
                direccionCodepFormatter.DrawString(ObtenerValor(codependiente,"pers_direccion"),fuenteNormal,
                    XBrushes.Black,new XRect(margen + 70,y - 10,pagina.Width - margen - 110,35),XStringFormats.TopLeft);
                y += 50;
            }

            if (y > 720)
            {
                gfx.Dispose();

                pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;

                gfx = XGraphics.FromPdfPage(pagina);
                y = 40;

                gfx.DrawString("SOLICITUD DE EXÁMENES - " + codigoEXSO.ToString(),fuenteSubtitulo,XBrushes.Black,margen,y);
                y += 30;
            }

            gfx.DrawString("EXÁMENES SOLICITADOS",fuenteSubtitulo,XBrushes.Black,margen,y);
            y += 8;

            gfx.DrawLine(XPens.Gray,margen,y,pagina.Width - margen,y);
            y += 22;

            int numero = 1;

            foreach (DataRow examen in examenes.Rows)
            {
                string nombreExamen = ObtenerValor(examen,"EXAMEN");
                double altoFila = 32;

                if (nombreExamen.Length > 150)
                {
                    altoFila = 68;
                }
                else if (nombreExamen.Length > 75)
                {
                    altoFila = 50;
                }

                if (y + altoFila > 790)
                {
                    gfx.Dispose();

                    pagina = documento.AddPage();
                    pagina.Size = PdfSharp.PageSize.A4;

                    gfx = XGraphics.FromPdfPage(pagina);
                    y = 40;

                    gfx.DrawString("SOLICITUD DE EXÁMENES - " + codigoEXSO.ToString(),fuenteSubtitulo,XBrushes.Black,margen,y);
                    y += 25;

                    gfx.DrawString("EXÁMENES SOLICITADOS - CONTINUACIÓN",fuenteSubtitulo,XBrushes.Black,margen,y);
                    y += 8;

                    gfx.DrawLine(XPens.Gray,margen,y,pagina.Width - margen,y);
                    y += 22;
                }

                gfx.DrawRectangle(XPens.LightGray,margen,y,pagina.Width - 80,altoFila);
                gfx.DrawString(numero.ToString() + ".",fuenteNegrita,XBrushes.Black,margen + 8,y + 17);

                XTextFormatter formatter =
                    new XTextFormatter(gfx);

                formatter.DrawString(nombreExamen,fuenteNormal,XBrushes.Black,new XRect(margen + 30,y + 5,pagina.Width - 125,altoFila - 8),XStringFormats.TopLeft);
                y += altoFila;

                numero++;
            }

            if (!string.IsNullOrWhiteSpace(examenAdicional))
            {
                double altoFilaAdicional = 32;


                if (examenAdicional.Length > 150)
                {
                    altoFilaAdicional = 68;
                }
                else if (examenAdicional.Length > 75)
                {
                    altoFilaAdicional = 50;
                }

                if (y + altoFilaAdicional > 790)
                {
                    gfx.Dispose();

                    pagina = documento.AddPage();
                    pagina.Size = PdfSharp.PageSize.A4;

                    gfx = XGraphics.FromPdfPage(pagina);
                    y = 40;

                    gfx.DrawString("SOLICITUD DE EXÁMENES - " + codigoEXSO.ToString(),fuenteSubtitulo,XBrushes.Black,margen,y);

                    y += 25;

                    gfx.DrawString("EXÁMENES SOLICITADOS - CONTINUACIÓN",fuenteSubtitulo,XBrushes.Black,margen,y);
                    y += 8;

                    gfx.DrawLine(XPens.Gray,margen,y,pagina.Width - margen,y);
                    y += 22;
                }

                gfx.DrawRectangle(XPens.LightGray,margen,y,pagina.Width - 80,altoFilaAdicional);
                gfx.DrawString(numero.ToString() + ".",fuenteNegrita,XBrushes.Black,margen + 8,y + 17);

                XTextFormatter formatterAdicional = new XTextFormatter(gfx);

                formatterAdicional.DrawString(examenAdicional,fuenteNormal,XBrushes.Black,
                    new XRect(margen + 30,y + 5,pagina.Width - 125,altoFilaAdicional - 8),XStringFormats.TopLeft);

                y += altoFilaAdicional;

                numero++;
            }

            int totalExamenes = examenes.Rows.Count;

            if (!string.IsNullOrWhiteSpace(examenAdicional))
            {
                totalExamenes++;
            }

            if (y + 40 > 810)
            {
                gfx.Dispose();

                pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;

                gfx = XGraphics.FromPdfPage(pagina);
                y = 40;

                gfx.DrawString("SOLICITUD DE EXÁMENES - " + codigoEXSO.ToString(),fuenteSubtitulo,XBrushes.Black,margen,y);
                y += 30;
            }

            y += 20;

            gfx.DrawString("Total de exámenes: " + totalExamenes.ToString(),fuenteNegrita,XBrushes.Black,margen,y);

            gfx.Dispose();

            using (MemoryStream memoria = new MemoryStream())
            {
                documento.Save(memoria, false);
                documento.Close();
                return memoria.ToArray();
            }
        }

        private void DibujarCampoPdf(XGraphics gfx, XFont fuenteTitulo, XFont fuenteValor, double x, double y, string titulo, string valor)
        {
            gfx.DrawString(titulo, fuenteTitulo, XBrushes.Black, x, y);
            gfx.DrawString(valor == null ? "" : valor, fuenteValor, XBrushes.Black, x + 95, y);
        }

        //Guardar PDF
        private void GuardarPdfSolicitud(int codigoEXSO, byte[] pdf)
        {
            if (pdf == null || pdf.Length == 0)
            {
                throw new Exception("No fue posible generar el archivo PDF.");
            }

            object[] parametros = CrearParametrosOperacionSolicitud(4, codigoEXSO);
            string nombreTitular = ObtenerNombreTitularPdf(codigoEXSO);

            parametros[21] = pdf;
            parametros[22] = "Examenes_" + nombreTitular + "_" + codigoEXSO.ToString() + ".pdf";
            parametros[23] = "application/pdf";
            parametros[24] = ".pdf";
            parametros[29] = codigoEXSO;

            new Conexion(2, "").FunInsertSolictudExamen(parametros);
        }
        private void GuardarExamenAdicional(int codigoEXSO)
        {
            string examenAdicional = TxtExamenAdicional.Text.Trim();

            if (string.IsNullOrWhiteSpace(examenAdicional))
            {
                return;
            }

            if (examenAdicional.Length > 250)
            {
                throw new Exception("El examen adicional no puede superar los 250 caracteres.");
            }

            object[] parametros = CrearParametrosOperacionSolicitud(11, codigoEXSO);
            parametros[0] = 11;
            parametros[20] = examenAdicional.ToUpper();
            parametros[29] = codigoEXSO;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No fue posible guardar el examen adicional.");
            }

            string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();

            if (resultado != "OK-EXAMEN-ADICIONAL")
            {
                throw new Exception("No fue posible guardar el examen adicional. " + "Respuesta: " + resultado);
            }
        }

        private string ObtenerNombreTitularPdf(int codigoEXSO)
        {
            DataSet ds = ObtenerDatosSolicitudPdf(codigoEXSO);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "TITULAR";
            }

            DataRow fila = ds.Tables[0].Rows[0];

            string primerNombre = ObtenerValor(fila, "pers_primernombre");
            string primerApellido = ObtenerValor(fila, "pers_primerapellido");
            string nombreTitular = primerNombre + " " + primerApellido;
            nombreTitular = nombreTitular.Trim();

            return LimpiarNombreArchivo(nombreTitular);
        }

        private string LimpiarNombreArchivo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return "TITULAR";
            }

            texto = texto.Trim().ToUpper().Replace(" ", "_");
            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();

            foreach (char caracter in caracteresInvalidos)
            {
                texto = texto.Replace(caracter.ToString(), "");
            }

            if (texto.Length > 90)
            {
                texto = texto.Substring(0, 90);
            }

            return texto;
        }

        //crear tabla
        private DataTable CrearTablaRequisitos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ASRQ_CODIGO", typeof(int));
            dt.Columns.Add("GRUPO", typeof(string));
            dt.Columns.Add("REQUISITO", typeof(string));
            dt.Columns.Add("TIPO", typeof(string));
            dt.Columns.Add("ORDEN", typeof(int));
            return dt;
        }
        private DataTable RequisitosActuales
        {
            get
            {
                if (ViewState["RequisitosActuales"] == null)
                {
                    ViewState["RequisitosActuales"] = CrearTablaRequisitos();
                }

                return
                    (DataTable)ViewState["RequisitosActuales"];
            }

            set
            {
                ViewState["RequisitosActuales"] = value;
            }
        }
        private void BindRequisitos()
        {
            GrdvRequisitos.DataSource = RequisitosActuales;
            GrdvRequisitos.DataBind();
            LblCantidadRequisitos.Text = RequisitosActuales.Rows.Count.ToString() + " requisito(s)";
        }
        private DataSet ObtenerRequisitosAsegurabilidad()
        {

            if (DdlProducto.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlProducto.SelectedValue))
            {
                throw new Exception("Seleccione el Canal.");
            }

            DateTime fechaNacimiento;

            if (!DateTime.TryParseExact(TxtFechaNacimiento.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fechaNacimiento))
            {
                throw new Exception("La fecha de nacimiento no es válida.");
            }

            string montoTexto = FormatearDecimal(TxtMontoAc.Text);
            decimal montoTotal = decimal.Parse(montoTexto, CultureInfo.InvariantCulture);

            if (montoTotal <= 0)
            {
                throw new Exception("Ingrese un Monto Total mayor a cero.");
            }

            object[] parametros = CrearParametrosOperacionSolicitud(6, 0);
            parametros[1] = Convert.ToInt32(DdlProducto.SelectedValue);
            parametros[10] = fechaNacimiento.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            parametros[32] = montoTexto;

            return
                new Conexion(2, "").FunInsertSolictudExamen(parametros);
        }
        private bool ProcesarRequisitos(DataSet ds, bool mostrarMensaje)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No se obtuvo respuesta de la configuración de asegurabilidad.");
            }

            DataRow informacion = ds.Tables[0].Rows[0];
            string resultado = informacion["RESULTADO"].ToString().Trim();
            string mensaje = informacion["MENSAJE"].ToString().Trim();

            if (resultado == "SIN_REQUISITOS" || resultado == "NO_REALIZA")
            {
                RequisitosActuales = CrearTablaRequisitos();
                BindRequisitos();

                TrDocumentoAdjunto.Visible = false;
                BtnGrabar.Enabled = false;
                LblEstadoRequisitos.Text = "No corresponde generar solicitud de exámenes";

                if (mostrarMensaje)
                {
                    new Funciones().funShowJSMessage("No corresponde generar solicitud de exámenes", this);
                }

                return false;
            }

            if (resultado != "REQUISITOS")
            {
                RequisitosActuales = CrearTablaRequisitos();
                BindRequisitos();
                TrDocumentoAdjunto.Visible = false;
                BtnGrabar.Enabled = false;
                LblEstadoRequisitos.Text = mensaje;

                if (mostrarMensaje)
                {
                    new Funciones().funShowJSMessage(mensaje, this);
                }
                return false;
            }

            if (ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0)
            {
                throw new Exception("La regla existe pero no tiene requisitos configurados.");
            }

            RequisitosActuales = ds.Tables[1].Copy();
            BindRequisitos();

            bool requiereArchivo = Convert.ToBoolean(informacion["REQUIERE_ARCHIVO"]);

            ViewState["RequiereArchivo"] = requiereArchivo;
            TrDocumentoAdjunto.Visible = requiereArchivo;
            BtnGrabar.Enabled = true;
            LblEstadoRequisitos.Text = "Requisitos encontrados correctamente.";
            return true;
        }
        protected void BtnConsultarRequisitos_Click(object sender, EventArgs e)
        {
            try
            {
                bool personaExiste = ExistePersonaPorDocumento();
                if (!personaExiste)
                {
                    new Funciones().funShowJSMessage("Primero debe crear el titular.", this);
                    return;
                }

                Lblerror.Text = "";
                if (DdlCampaign.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Campaign.", this);
                    return;
                }

                if (DdlProducto.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Canal.", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtNumeroDocumento.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese una cédula válida.", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtFechaNacimiento.Text))
                {
                    new Funciones().funShowJSMessage("No existe fecha de nacimiento para el titular.", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtNumeroDocumento.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese una cédula válida.", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPrimerNombre.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese el primer nombre del titular.", this);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPrimerApellido.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese el primer apellido del titular.", this);
                    return;
                }

                DateTime fechaNacimiento;

                if (!DateTime.TryParseExact(TxtFechaNacimiento.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaNacimiento))
                {
                    new Funciones().funShowJSMessage("Ingrese una fecha de nacimiento válida.", this);
                    return;
                }

                decimal montoTotal;


                if (!decimal.TryParse(FormatearDecimal(TxtMontoAc.Text), NumberStyles.Any, CultureInfo.InvariantCulture, out montoTotal) || montoTotal <= 0)
                {
                    new Funciones().funShowJSMessage("Ingrese un Monto Total válido.", this);
                    return;
                }

                DataSet ds = ObtenerRequisitosAsegurabilidad();
                ProcesarRequisitos(ds, true);

                bool correcto = ProcesarRequisitos(ds, true);
                ViewState["RequisitosConsultados"] = correcto;
            }
            catch (Exception ex)
            {
                LimpiarRequisitosAsegurabilidad();
                Lblerror.Text = ex.Message;
            }
        }
        private void LimpiarRequisitosAsegurabilidad()
        {
            RequisitosActuales = CrearTablaRequisitos();
            BindRequisitos();

            LblEstadoRequisitos.Text = "";
            LblCantidadRequisitos.Text = "0 requisito(s)";
            TrDocumentoAdjunto.Visible = false;
            ViewState["RequiereArchivo"] = false;
            ViewState["RequisitosConsultados"] = false;
            BtnGrabar.Enabled = false;
        }

        protected void TxtMontoAc_TextChanged(object sender, EventArgs e)
        {
            //LimpiarRequisitosAsegurabilidad();
            //if (ViewState["TitularCargado"] != null && Convert.ToBoolean(ViewState["TitularCargado"]))
            //{
            //    LblEstadoRequisitos.Text = "Monto Total modificado. Consulte nuevamente los requisitos.";
            //}
            if (!string.IsNullOrWhiteSpace(TxtNumeroDocumento.Text))
            {
                LblEstadoRequisitos.Text = "Monto Total modificado. Consulte nuevamente los requisitos.";
            }
        }

        //private void GuardarRequisitosSolicitud(int codigoEXSO,DataTable requisitos)
        //{
        //    if (requisitos == null || requisitos.Rows.Count == 0)
        //    {
        //        throw new Exception("No existen requisitos para guardar.");
        //    }


        //    foreach (DataRow fila in requisitos.Rows)
        //    {
        //        object[] parametros = CrearParametrosOperacionSolicitud(7,codigoEXSO);

        //        parametros[0] = 7;
        //        parametros[25] = Convert.ToInt32(fila["ASRQ_CODIGO"]);
        //        parametros[29] = codigoEXSO;


        //        parametros[41] = Convert.ToInt32(Session["usuCodigo"]);

        //        parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";

        //        DataSet dsResultado = new Conexion(2, "").FunInsertSolictudExamen(parametros);

        //        if (dsResultado == null || dsResultado.Tables.Count == 0 || dsResultado.Tables[0].Rows.Count == 0)
        //        {
        //            throw new Exception("No se pudo guardar uno de los requisitos de la solicitud.");
        //        }
        //    }
        //}
        private void GuardarRequisitosSolicitud(int codigoEXSO, DataTable requisitos)
        {
            if (requisitos == null || requisitos.Rows.Count == 0)
            {
                throw new Exception("No existen requisitos para guardar.");
            }

            foreach (DataRow fila in requisitos.Rows)
            {
                object[] parametros = CrearParametrosOperacionSolicitud(7, codigoEXSO);

                parametros[25] = Convert.ToInt32(fila["ASRQ_CODIGO"]);
                parametros[29] = codigoEXSO;
                parametros[41] = Convert.ToInt32(Session["usuCodigo"]);
                parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";

                DataSet dsResultado = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (dsResultado == null || dsResultado.Tables.Count == 0 || dsResultado.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("No se obtuvo respuesta al guardar el requisito " + fila["ASRQ_CODIGO"].ToString() + ".");
                }

                string resultado = dsResultado.Tables[0].Rows[0]["Resultado"].ToString();

                if (resultado != "OK-REQUISITO" && resultado != "DUPLICADO")
                {
                    throw new Exception("No se pudo guardar el requisito " + fila["ASRQ_CODIGO"].ToString() + ". Resultado: " + resultado);
                }
            }
        }
        private bool PersonaExisteEnBase()
        {
            return ViewState["PersonaExiste"] != null && ViewState["PersonaExiste"].ToString() == "SI";
        }

        private DataSet GuardarTitularNuevo()
        {
            object[] parametros = CrearParametrosOperacionSolicitud(8, 0);

            parametros[0] = 8;
            parametros[1] = Convert.ToInt32(DdlProducto.SelectedValue);
            parametros[2] = DdlTipoDocumento.SelectedValue;
            parametros[3] = TxtNumeroDocumento.Text.Trim();
            parametros[4] = TxtPrimerNombre.Text.Trim().ToUpper();
            parametros[5] = TxtSegundoNombre.Text.Trim().ToUpper();
            parametros[6] = TxtPrimerApellido.Text.Trim().ToUpper();
            parametros[7] = TxtSegundoApellido.Text.Trim().ToUpper();
            parametros[8] = DdlGenero.SelectedValue;
            parametros[9] = DdlEstadoCivil.SelectedValue;
            parametros[10] = TxtFechaNacimiento.Text.Trim();
            parametros[11] = Convert.ToInt32(DdlCiudad.SelectedValue);
            parametros[12] = TxtDireccion.Text.Trim().ToUpper();
            parametros[13] = TxtFonoCasa.Text.Trim();
            parametros[14] = TxtFonoOficina.Text.Trim();
            parametros[15] = TxtCelular.Text.Trim();
            parametros[16] = TxtEmail.Text.Trim();
            parametros[17] = 0;
            parametros[18] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[20] = "";
            parametros[21] = new byte[0];
            parametros[22] = "";
            parametros[23] = "";
            parametros[24] = "";
            parametros[25] = 0;
            parametros[26] = "0";
            parametros[27] = "0";
            parametros[28] = "";
            parametros[29] = 0;
            parametros[30] = "Activo";
            parametros[31] = "0.00";
            parametros[32] = "0.00";
            parametros[41] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";

            return new Conexion(2, "").FunInsertSolictudExamen(parametros);
        }

        private bool ValidarTitularNuevo()
        {
            if (DdlProducto.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Canal.", this);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtNumeroDocumento.Text))
            {
                new Funciones().funShowJSMessage("Ingrese No. de Documento.", this);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPrimerNombre.Text))
            {
                new Funciones().funShowJSMessage("Ingrese Primer Nombre.", this);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPrimerApellido.Text))
            {
                new Funciones().funShowJSMessage("Ingrese Primer Apellido.", this);
                return false;
            }

            if (DdlGenero.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Género.", this);
                return false;
            }

            if (DdlEstadoCivil.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Estado Civil.", this);
                return false;
            }

            DateTime fechaNacimiento;

            if (!DateTime.TryParseExact(TxtFechaNacimiento.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaNacimiento))
            {
                new Funciones().funShowJSMessage("Fecha de nacimiento no válida.", this);
                return false;
            }

            if (DdlProvincia.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Provincia.", this);
                return false;
            }

            if (DdlCiudad.SelectedValue == "0")
            {
                new Funciones().funShowJSMessage("Seleccione Ciudad.", this);
                return false;
            }

            return true;
        }

    
        protected void DdlProvinciaCodep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarCiudadesCodependiente();
            //MostrarModalCodependiente();
            try
            {
            
                string fechaNacimiento = HdnFechaNacimientoCodep.Value;
                CargarCiudadesCodependiente();

                if (!string.IsNullOrWhiteSpace(fechaNacimiento))
                {
                    TxtFechaNacimientoCodep.Text = fechaNacimiento;
                }

                PnlNuevoCodependiente.Visible = true;
                PnlCodependienteEncontrado.Visible = false;
                BtnCrearCodependiente.Visible = true;
                BtnSeleccionarCodependiente.Visible = false;
                MpeCodependiente.Show();
            }
            catch (Exception ex)
            {
                LblMensajeCodependiente.Text = ex.Message;
                MpeCodependiente.Show();
            }
        }

        private void ActualizarEstadoCodependiente()
        {
            bool titularExiste = false;

            if (!string.IsNullOrWhiteSpace(TxtNumeroDocumento.Text))
            {
                titularExiste = ExistePersonaPorDocumento();
            }

            BtnAgregarCodependiente.Enabled = titularExiste;
        }

        private void CargarCombosCodependiente()
        {
            try
            {
                object[] parametros;

                parametros = new object[1];
                parametros[0] = "TIPO DOCUMENTOS";

                DdlTipoDocumentoCodep.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", parametros);
                DdlTipoDocumentoCodep.DataTextField = "Descripcion";
                DdlTipoDocumentoCodep.DataValueField = "Valor";
                DdlTipoDocumentoCodep.DataBind();

                parametros = new object[1];
                parametros[0] = "GENERO";

                DdlGeneroCodep.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", parametros);
                DdlGeneroCodep.DataTextField = "Descripcion";
                DdlGeneroCodep.DataValueField = "Valor";
                DdlGeneroCodep.DataBind();

                parametros = new object[1];
                parametros[0] = "ESTADO CIVIL";

                DdlEstadoCivilCodep.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", parametros);
                DdlEstadoCivilCodep.DataTextField = "Descripcion";
                DdlEstadoCivilCodep.DataValueField = "Valor";
                DdlEstadoCivilCodep.DataBind();

                parametros = new object[1];
                parametros[0] = 6;

                DdlProvinciaCodep.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", parametros);
                DdlProvinciaCodep.DataTextField = "Descripcion";
                DdlProvinciaCodep.DataValueField = "Codigo";
                DdlProvinciaCodep.DataBind();

                CargarCiudadesCodependiente();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void CargarCiudadesCodependiente()
        {
            try
            {
                DdlCiudadCodep.Items.Clear();

                if (DdlProvinciaCodep.Items.Count == 0)
                {
                    return;
                }

                object[] parametros = new object[3];
                parametros[0] = DdlProvinciaCodep.SelectedValue;
                parametros[1] = "";
                parametros[2] = 4;

                DdlCiudadCodep.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);
                DdlCiudadCodep.DataTextField = "Descripcion";
                DdlCiudadCodep.DataValueField = "Codigo";
                DdlCiudadCodep.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #region Funciones
        private DataSet GuardarTitularCodependiente(string tipoDocumento, string documento, string primerNombre, string segundoNombre,
                string primerApellido, string segundoApellido, string genero, string estadoCivil, string fechaNacimiento,
                int codigoCiudad, string direccion, string telefonoCasa, string telefonoOficina, string celular, string email)
        {
            object[] parametros = CrearParametrosOperacionSolicitud(8, 0);
            parametros[1] = Convert.ToInt32(DdlProducto.SelectedValue);
            parametros[2] = tipoDocumento;
            parametros[3] = documento.Trim();
            parametros[4] = primerNombre.Trim().ToUpper();
            parametros[5] = segundoNombre.Trim().ToUpper();
            parametros[6] = primerApellido.Trim().ToUpper();
            parametros[7] = segundoApellido.Trim().ToUpper();
            parametros[8] = genero;
            parametros[9] = estadoCivil;
            parametros[10] = fechaNacimiento.Trim();
            parametros[11] = codigoCiudad;
            parametros[12] = direccion.Trim().ToUpper();
            parametros[13] = telefonoCasa.Trim();
            parametros[14] = telefonoOficina.Trim();
            parametros[15] = celular.Trim();
            parametros[16] = email.Trim().ToLower();
            parametros[17] = 0;
            parametros[18] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[31] = FormatearDecimal(TxtMonto.Text);
            parametros[41] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";
            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);
            return ds;
        }

        private void MostrarCodependienteSeleccionado(int codigoPERS, int codigoTITU, string documento, string nombre)
        {
            ViewState["CodigoPERSCodependiente"] = codigoPERS;
            ViewState["CodigoTITUCodependiente"] = codigoTITU;
            LblDocumentoCodependiente.Text = documento.Trim();
            LblNombreCodependiente.Text = nombre.Trim().ToUpper();
            PnlCodependienteSeleccionado.Visible = true;
            LblSinCodependiente.Visible = false;
            BtnAgregarCodependiente.Text = "Cambiar Codependiente";
        }
        private void GuardarCodependienteSolicitud(int codigoEXSO)
        {

            if (ViewState["CodigoTITUCodependiente"] == null)
            {
                return;
            }

            int codigoTITUCodependiente = 0;

            if (!int.TryParse(ViewState["CodigoTITUCodependiente"].ToString(), out codigoTITUCodependiente))
            {
                return;
            }

            if (codigoTITUCodependiente <= 0)
            {
                return;
            }

            object[] parametros = CrearParametrosOperacionSolicitud(9, codigoEXSO);
            parametros[0] = 9;
            parametros[29] = codigoEXSO;
            parametros[36] = codigoTITUCodependiente;
            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No se obtuvo respuesta al guardar el codependiente.");
            }

            string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();

            if (resultado != "OK-CODEPENDIENTE")
            {
                throw new Exception("No fue posible relacionar el codependiente con la solicitud. Respuesta: " + resultado);
            }
        }
        private string ObtenerFechaNacimientoCodependiente()
        {
            string texto = TxtFechaNacimientoCodep.Text.Trim();


            // Respaldo
            if (string.IsNullOrWhiteSpace(texto))
            {
                texto = HdnFechaNacimientoCodep.Value.Trim();
            }

            if (string.IsNullOrWhiteSpace(texto))
            {
                throw new Exception("Seleccione la fecha de nacimiento del codependiente.");
            }


            DateTime fecha;

            if (!DateTime.TryParseExact(texto, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                throw new Exception("La fecha de nacimiento del codependiente no es válida.");
            }

            return fecha.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
        private bool EsProductoDaquilema()
        {
            if (DdlProducto.SelectedItem == null)
            {
                return false;
            }

            string producto = DdlProducto.SelectedItem.Text.Trim();

            return producto.IndexOf("DAQUILEMA", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        private void ActualizarArchivoDaquilema()
        {
            PnlArchivoDaquilema.Visible = EsProductoDaquilema();
        }
        private void GuardarDocumentoDaquilema(int codigoEXSO)
        {

            if (!EsProductoDaquilema())
            {
                return;
            }

            if (!FileUploadDaquilema.HasFile)
            {
                return;
            }

            string nombreArchivo = Path.GetFileName(FileUploadDaquilema.PostedFile.FileName);

            if (nombreArchivo.Length > 100)
            {
                throw new Exception(
                    "El nombre del documento adicional DAQUILEMA " +
                    "no puede superar los 100 caracteres."
                );
            }

            string extension = Path.GetExtension(nombreArchivo).ToLower();
            string mime = "";

            switch (extension)
            {
                case ".pdf":
                    mime = "application/pdf";
                    break;

                case ".png":
                    mime = "image/png";
                    break;

                case ".jpg":
                case ".jpeg":
                    mime = "image/jpeg";
                    break;

                case ".doc":
                    mime = "application/msword";
                    break;

                case ".docx":
                    mime =
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;

                default:
                    throw new Exception(
                        "El documento adicional DAQUILEMA debe ser " +
                        "PDF, DOC, DOCX, PNG, JPG o JPEG."
                    );
            }

            byte[] archivo;
            using (BinaryReader br = new BinaryReader(FileUploadDaquilema.PostedFile.InputStream))
            {
                archivo = br.ReadBytes(FileUploadDaquilema.PostedFile.ContentLength);
            }

            object[] parametros = CrearParametrosOperacionSolicitud(10, codigoEXSO);

            parametros[0] = 10;
            parametros[21] = archivo;
            parametros[22] = nombreArchivo;
            parametros[23] = mime;
            parametros[24] = extension;
            parametros[29] = codigoEXSO;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No fue posible guardar el documento adicional DAQUILEMA.");
            }


            string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();


            if (resultado != "OK-DOCUMENTO-DAQUILEMA")
            {
                throw new Exception("No fue posible guardar el documento adicional DAQUILEMA. " + resultado);
            }
        }

        #endregion

        #region botones

        protected void BtnBuscarCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarResultadoBusquedaCodependiente();

                if (!ExistePersonaPorDocumento())
                {
                    LblMensajeCodependiente.Text = "Primero debe crear o seleccionar un titular existente.";
                    MostrarModalCodependiente();
                    return;
                }

                if (DdlProducto.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlProducto.SelectedValue))
                {
                    LblMensajeCodependiente.Text = "Seleccione el producto.";
                    MostrarModalCodependiente();
                    return;
                }

                string documento = TxtNumeroDocumentoCodep.Text.Trim();

                if (documento == "")
                {
                    LblMensajeCodependiente.Text = "Ingrese el número de documento del codependiente.";
                    MostrarModalCodependiente();
                    return;
                }

                if (documento == TxtNumeroDocumento.Text.Trim())
                {
                    LblMensajeCodependiente.Text = "El titular principal no puede ser su propio codependiente.";
                    MostrarModalCodependiente();
                    return;
                }

                object[] parametros = new object[3];
                parametros[0] = 0;
                parametros[1] = documento;
                parametros[2] = 22;

                DataSet dsPersona = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);


                if (dsPersona != null && dsPersona.Tables.Count > 0 && dsPersona.Tables[0].Rows.Count > 0)
                {
                    DataRow fila = dsPersona.Tables[0].Rows[0];
                    int codigoPERS = Convert.ToInt32(fila["CodigoPERS"]);
                    ViewState["CodigoPERSCodependienteBusqueda"] = codigoPERS;

                    string primerNombre = fila["PrimerNombre"].ToString().Trim();
                    string segundoNombre = fila["SegundNombre"].ToString().Trim();
                    string primerApellido = fila["PrimerApellido"].ToString().Trim();
                    string segundoApellido = fila["SegundoApellido"].ToString().Trim();
                    string nombreCompleto = (
                            primerNombre + " " +
                            segundoNombre + " " +
                            primerApellido + " " +
                            segundoApellido
                        ).Trim();

                    LblDocumentoCodepEncontrado.Text = fila["Identificacion"].ToString().Trim();
                    LblNombreCodepEncontrado.Text = nombreCompleto;
                    LblFechaNacimientoCodepEncontrado.Text = fila["fechanacimiento"].ToString().Trim();
                    LblEmailCodepEncontrado.Text = fila["Email"].ToString().Trim();
                    PnlCodependienteEncontrado.Visible = true;
                    PnlNuevoCodependiente.Visible = false;
                    BtnSeleccionarCodependiente.Visible = true;
                    BtnCrearCodependiente.Visible = false;
                    LblMensajeCodependiente.Text = "";
                    MostrarModalCodependiente();
                    return;
                }

                ViewState["CodigoPERSCodependienteBusqueda"] = 0;
                PnlCodependienteEncontrado.Visible = false;
                PnlNuevoCodependiente.Visible = true;
                BtnSeleccionarCodependiente.Visible = false;
                BtnCrearCodependiente.Visible = true;
                LblMensajeCodependiente.Text = "La persona no existe. Complete los datos para crear el codependiente.";
                MostrarModalCodependiente();
            }
            catch (Exception ex)
            {
                LblMensajeCodependiente.Text = ex.Message;
                MostrarModalCodependiente();
            }
        }

        protected void BtnSeleccionarCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                string documento = TxtNumeroDocumentoCodep.Text.Trim();

                if (documento == "")
                {
                    LblMensajeCodependiente.Text = "Ingrese el número de documento.";
                    MostrarModalCodependiente();
                    return;
                }

                object[] parametros = new object[3];
                parametros[0] = 0;
                parametros[1] = documento;
                parametros[2] = 22;

                DataSet dsPersona = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", parametros);

                if (dsPersona == null || dsPersona.Tables.Count == 0 || dsPersona.Tables[0].Rows.Count == 0)
                {
                    LblMensajeCodependiente.Text = "La persona ya no se encuentra disponible.";
                    MostrarModalCodependiente();
                    return;
                }

                DataRow fila = dsPersona.Tables[0].Rows[0];

                string tipoDocumento = fila["TipoDocumento"].ToString().Trim();
                string identificacion = fila["Identificacion"].ToString().Trim();
                string primerNombre = fila["PrimerNombre"].ToString().Trim();
                string segundoNombre = fila["SegundNombre"].ToString().Trim();
                string primerApellido = fila["PrimerApellido"].ToString().Trim();
                string segundoApellido = fila["SegundoApellido"].ToString().Trim();
                string genero = fila["Genero"].ToString().Trim();
                string estadoCivil = fila["EstCivil"].ToString().Trim();
                string fechaNacimiento = fila["fechanacimiento"].ToString().Trim();
                int codigoCiudad = 0;
                if (fila["CodCiud"] != DBNull.Value && fila["CodCiud"].ToString().Trim() != "")
                {
                    codigoCiudad = Convert.ToInt32(fila["CodCiud"]);
                }

                string direccion = fila["Direccion"].ToString().Trim();
                string telefonoCasa = fila["FonoCasa"].ToString().Trim();
                string telefonoOficina = fila["FonoOfic"].ToString().Trim();
                string celular = fila["Celular"].ToString().Trim();
                string email = fila["Email"].ToString().Trim();

                DataSet dsTitular = GuardarTitularCodependiente(tipoDocumento, identificacion, primerNombre, segundoNombre,
                        primerApellido, segundoApellido, genero, estadoCivil, fechaNacimiento, codigoCiudad, direccion,
                        telefonoCasa, telefonoOficina, celular, email);


                if (dsTitular == null || dsTitular.Tables.Count == 0 || dsTitular.Tables[0].Rows.Count == 0)
                {
                    LblMensajeCodependiente.Text = "No fue posible agregar el codependiente.";
                    MostrarModalCodependiente();
                    return;
                }

                DataRow respuesta = dsTitular.Tables[0].Rows[0];
                string resultado = respuesta["Resultado"].ToString().Trim();

                if (resultado != "OK-TITULAR")
                {
                    LblMensajeCodependiente.Text = "No fue posible crear o actualizar el titular codependiente.";
                    MostrarModalCodependiente();
                    return;
                }

                int codigoPERS = Convert.ToInt32(respuesta["CodigoPERS"]);
                int codigoTITU = Convert.ToInt32(respuesta["CodigoTITU"]);

                string nombreCompleto = (primerNombre + " " + segundoNombre + " " + primerApellido + " " + segundoApellido).Trim();

                MostrarCodependienteSeleccionado(codigoPERS, codigoTITU, identificacion, nombreCompleto);
                MpeCodependiente.Hide();
                TxtNumeroDocumentoCodep.Text = "";
                LimpiarResultadoBusquedaCodependiente();
            }
            catch (Exception ex)
            {
                LblMensajeCodependiente.Text = ex.Message;
                MostrarModalCodependiente();
            }
        }

        protected void BtnCrearCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                LblMensajeCodependiente.Text = "";

                if (DdlProducto.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlProducto.SelectedValue))
                {
                    LblMensajeCodependiente.Text = "Seleccione primero el producto.";
                    MpeCodependiente.Show();
                    return;
                }

                string documento = TxtNumeroDocumentoCodep.Text.Trim();

                if (string.IsNullOrEmpty(documento))
                {
                    LblMensajeCodependiente.Text = "Ingrese el número de documento.";
                    MpeCodependiente.Show();
                    return;
                }

                if (documento == TxtNumeroDocumento.Text.Trim())
                {
                    LblMensajeCodependiente.Text = "El codependiente no puede ser el mismo titular principal.";
                    MpeCodependiente.Show();
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPrimerNombreCodep.Text))
                {
                    LblMensajeCodependiente.Text = "Ingrese el primer nombre.";
                    MpeCodependiente.Show();
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPrimerApellidoCodep.Text))
                {
                    LblMensajeCodependiente.Text = "Ingrese el primer apellido.";
                    MpeCodependiente.Show();
                    return;
                }

                if (DdlGeneroCodep.Items.Count == 0 || string.IsNullOrWhiteSpace(DdlGeneroCodep.SelectedValue))
                {
                    LblMensajeCodependiente.Text = "Seleccione el género.";
                    MpeCodependiente.Show();
                    return;
                }


                if (DdlEstadoCivilCodep.Items.Count == 0 || string.IsNullOrWhiteSpace(DdlEstadoCivilCodep.SelectedValue))
                {
                    LblMensajeCodependiente.Text = "Seleccione el estado civil.";
                    MpeCodependiente.Show();
                    return;
                }

                if (DdlCiudadCodep.Items.Count == 0 || string.IsNullOrWhiteSpace(DdlCiudadCodep.SelectedValue))
                {
                    LblMensajeCodependiente.Text = "Seleccione la ciudad.";
                    MpeCodependiente.Show();
                    return;
                }


                int codigoCiudad;

                if (!int.TryParse(DdlCiudadCodep.SelectedValue, out codigoCiudad))
                {
                    LblMensajeCodependiente.Text = "La ciudad seleccionada no es válida.";
                    MpeCodependiente.Show();
                    return;
                }

                string fechaNacimiento = ObtenerFechaNacimientoCodependiente();
                string tipoDocumento = DdlTipoDocumentoCodep.SelectedValue;
                string primerNombre = TxtPrimerNombreCodep.Text.Trim().ToUpper();
                string segundoNombre = TxtSegundoNombreCodep.Text.Trim().ToUpper();
                string primerApellido = TxtPrimerApellidoCodep.Text.Trim().ToUpper();
                string segundoApellido = TxtSegundoApellidoCodep.Text.Trim().ToUpper();
                string genero = DdlGeneroCodep.SelectedValue;
                string estadoCivil = DdlEstadoCivilCodep.SelectedValue;
                string direccion = TxtDireccionCodep.Text.Trim().ToUpper();
                string telefonoCasa = TxtFonoCasaCodep.Text.Trim();
                string telefonoOficina = TxtFonoOficinaCodep.Text.Trim();
                string celular = TxtCelularCodep.Text.Trim();
                string email = TxtEmailCodep.Text.Trim().ToLower();

                DataSet ds = GuardarTitularCodependiente(tipoDocumento, documento, primerNombre, segundoNombre,
                        primerApellido, segundoApellido, genero, estadoCivil, fechaNacimiento, codigoCiudad,
                        direccion, telefonoCasa, telefonoOficina, celular, email);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    LblMensajeCodependiente.Text = "No fue posible crear el codependiente.";
                    MpeCodependiente.Show();
                    return;
                }

                DataRow fila = ds.Tables[0].Rows[0];
                string resultado = fila["Resultado"].ToString().Trim();

                if (resultado != "OK-TITULAR")
                {
                    LblMensajeCodependiente.Text = "No fue posible crear el codependiente. " + resultado;
                    MpeCodependiente.Show();
                    return;
                }

                int codigoPERS = Convert.ToInt32(fila["CodigoPERS"]);
                int codigoTITU = Convert.ToInt32(fila["CodigoTITU"]);

                string nombreCompleto = (primerNombre + " " + segundoNombre + " " + primerApellido + " " + segundoApellido).Replace("  ", " ").Trim();
                MostrarCodependienteSeleccionado(codigoPERS, codigoTITU, documento, nombreCompleto);

                TxtNumeroDocumentoCodep.Text = "";
                TxtPrimerNombreCodep.Text = "";
                TxtSegundoNombreCodep.Text = "";
                TxtPrimerApellidoCodep.Text = "";
                TxtSegundoApellidoCodep.Text = "";
                TxtFechaNacimientoCodep.Text = "";
                TxtEmailCodep.Text = "";
                TxtDireccionCodep.Text = "";
                TxtFonoCasaCodep.Text = "";
                TxtFonoOficinaCodep.Text = "";
                TxtCelularCodep.Text = "";

                LimpiarResultadoBusquedaCodependiente();

                MpeCodependiente.Hide();
            }
            catch (Exception ex)
            {
                LblMensajeCodependiente.Text = ex.Message;
                MpeCodependiente.Show();
            }
        }

        protected void BtnQuitarCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["CodigoPERSCodependiente"] = 0;
                ViewState["CodigoTITUCodependiente"] = 0;
                ViewState["CodigoPERSCodependienteBusqueda"] = 0;

                LblNombreCodependiente.Text = "";
                LblDocumentoCodependiente.Text = "";
                PnlCodependienteSeleccionado.Visible = false;
                LblSinCodependiente.Visible = true;

                BtnAgregarCodependiente.Text = "Agregar Codependiente";
                BtnAgregarCodependiente.Enabled = true;

                TxtNumeroDocumentoCodep.Text = "";
                LimpiarResultadoBusquedaCodependiente();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //MODAL
        private void MostrarModalCodependiente()
        {
            MpeCodependiente.Show();
        }

        private void LimpiarResultadoBusquedaCodependiente()
        {
            ViewState["CodigoPERSCodependienteBusqueda"] = 0;

            PnlCodependienteEncontrado.Visible = false;
            PnlNuevoCodependiente.Visible = false;

            BtnSeleccionarCodependiente.Visible = false;
            BtnCrearCodependiente.Visible = false;

            LblMensajeCodependiente.Text = "";

            LblDocumentoCodepEncontrado.Text = "";
            LblNombreCodepEncontrado.Text = "";
            LblFechaNacimientoCodepEncontrado.Text = "";
            LblEmailCodepEncontrado.Text = "";
        }

        protected void BtnAgregarCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                TxtNumeroDocumentoCodep.Text = "";
                LimpiarFormularioNuevoCodependiente();
                LimpiarResultadoBusquedaCodependiente();
                PnlCodependienteEncontrado.Visible = false;
                PnlNuevoCodependiente.Visible = false;
                BtnSeleccionarCodependiente.Visible = false;
                BtnCrearCodependiente.Visible = false;
                LblMensajeCodependiente.Text = "";
                MpeCodependiente.Show();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnCancelarCodependiente_Click(object sender, EventArgs e)
        {
            try
            {
                TxtNumeroDocumentoCodep.Text = "";
                LimpiarFormularioNuevoCodependiente();
                LimpiarResultadoBusquedaCodependiente();
                MpeCodependiente.Hide();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion

        #endregion
    }
}