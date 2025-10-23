using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmCitaMedicaAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataSet _dts = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton btnselec = new ImageButton();
        string _token = "", _urltoken = "", _urldatos = "", _datositems = "", _fonocasa = "", _fonooficina = "", _celular = "",
            _fechaini = "", _fechafin = "";
        string[] _telefonos;
        bool _produccion = true;
        int _idproducto = 0, _titucodigo = 0, _fila = 0, _posicion = 0, _dias = 30;
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

                if(Session["codigocita"] != null) 
                {
                    if (int.Parse(Session["codigocita"].ToString()) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.updPrincipal, GetType(), "Resumen Cita", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosCitaMedica.aspx?CodigoCita=" + Session["codigocita"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=870px, height=430px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        Session["codigocita"] = 0;
                    }

                }

                lbltitulo.Text = "Administración Titulares (Agendamiento Médico)";
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = txtCriterio.Text.Trim().ToUpper();
                objparam[2] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCitaAdmin", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["grdvDatos"] = dt;

                //APLICAR LOG
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }

        }
        #endregion

        #region Botones y Eventos
        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            var strCodigo = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            var strCodProducto = grdvDatos.DataKeys[intIndex].Values["CodigoProducto"].ToString();
            var strFechaCobertura = grdvDatos.DataKeys[intIndex].Values["FechaCobertura"].ToString();

            string dateString = strFechaCobertura;
            string format = "dd/MM/yyyy";
            DateTime Cobertura = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            if (Session["Perfil"].ToString() == "NOVA")
            {
                Array.Resize(ref objparam, 11);
                objparam[0] = 4;
                objparam[1] = "DIAS";
                objparam[2] = "DIAS NOVA";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = 0;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;

                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    _dias = int.Parse(dt.Tables[0].Rows[0][0].ToString());
                }

                //programar para meses con 31 + 1
                //poner log

                int mesCobertura = Cobertura.Month;


                switch (mesCobertura)
                {
                    case 1:
                        _dias = _dias + 1;
                        break;
                    case 2:
                        _dias = 28;
                        break;
                    case 3:
                        _dias = _dias + 1;
                        break;
                    case 5:
                        _dias = _dias + 1;
                        break;
                    case 7:
                        _dias = _dias + 1;
                        break;
                    case 8:
                        _dias = _dias + 1;
                        break;
                    case 10:
                        _dias = _dias + 1;
                        break;
                    case 12:
                        _dias = _dias + 1;
                        break;
                }


                DateTime _fechaatual = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime _fechacobertura = DateTime.ParseExact(strFechaCobertura, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                TimeSpan difFechas = _fechaatual.Subtract(_fechacobertura);

                int _idasx = difFechas.Days;

                if(_idasx >= _dias)
                {
                    Response.Redirect("FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + strCodigo + "&CodigoProducto=" +
                        strCodProducto + "&Regresar=0");
                }
                else
                {
                    int _diffdias = _dias - _idasx;

                    _fechaatual = _fechaatual.AddDays(_diffdias);
                    string _fecha = DateTime.ParseExact(_fechaatual.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                    string _mensaje = "Usted aun no puede agendar, su fecha fecha de Cobertura inicia el: " + strFechaCobertura ;
                    _mensaje += "  Puede Agendar a partir del :  " + _fecha;

                    new Funciones().funShowJSMessage(_mensaje, this);
                   
                }

            }
            else
            {

                if(strCodProducto == "225" || strCodProducto == "226" || strCodProducto == "227")
                {
                    int mesCobertura = Cobertura.Month;
                    switch (mesCobertura)
                    {
                        case 1:
                            _dias = _dias + 1;
                            break;
                        case 2:
                            _dias = 28;
                            break;
                        case 3:
                            _dias = _dias + 1;
                            break;
                        case 5:
                            _dias = _dias + 1;
                            break;
                        case 7:
                            _dias = _dias + 1;
                            break;
                        case 8:
                            _dias = _dias + 1;
                            break;
                        case 10:
                            _dias = _dias + 1;
                            break;
                        case 12:
                            _dias = _dias + 1;
                            break;
                    }


                    DateTime _fechaatual = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime _fechacobertura = DateTime.ParseExact(strFechaCobertura, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    TimeSpan difFechas = _fechaatual.Subtract(_fechacobertura);

                    int _idasx = difFechas.Days;

                    if (_idasx >= _dias)
                    {
                        Response.Redirect("FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + strCodigo + "&CodigoProducto=" +
                            strCodProducto + "&Regresar=0");
                    }
                    else
                    {
                            int _diffdias = _dias - _idasx;

                        _fechaatual = _fechaatual.AddDays(_diffdias);
                        string _fecha = DateTime.ParseExact(_fechaatual.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        string _mensaje = "Usted aun no puede agendar, su fecha fecha de Cobertura inicia el: " + strFechaCobertura;
                        _mensaje += "  Puede Agendar a partir del :  " + _fecha;

                        new Funciones().funShowJSMessage(_mensaje, this);

                    }
                }
                else
                {
                    Response.Redirect("FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + strCodigo + "&CodigoProducto=" +
                        strCodProducto + "&Regresar=0");

                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCriterio.Text))
            {
                funCargaMantenimiento();
            }
        }
        protected void BtnBuscarWS_Click(object sender, EventArgs e)
        {
            try
            {
                _produccion = true;

                if (_produccion)
                {
                    _urltoken = "http://serviceweb.humana.med.ec/WSOAUTH2/token";
                    _urldatos = "http://serviceweb.humana.med.ec/WSCoris/api/ServicioExterno/Consulta_Datos_Titular";
                }
                else
                {
                    _urltoken = "http://almacen.humana.med.ec/WSOAUTH2/token";
                    _urldatos = "http://almacen.humana.med.ec/WSCobis/api/ServicioExterno/Consulta_Datos_Titular";
                }
                //IR A BUSCAR EN EL SERVICIO DE HUMANA 
                if (new Funciones().IsNumber(txtCriterio.Text.Trim()))
                {
                    //CONSULTAR EL CODIGO DEL PRODUCTO PARA HUMANA
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 4;
                    objparam[1] = "";
                    objparam[2] = "PRODUCTO HUMANA";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = 2;
                    objparam[7] = 0;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;

                    dt = new Conexion(2, "").FunConsultaDatos1(objparam);

                    if (dt.Tables[0].Rows.Count > 0) _idproducto = int.Parse(dt.Tables[0].Rows[0]["Codigo"].ToString());
                    else _idproducto = 190;

                    //PREGUNTAR SI YA EXISTE REGISTADA ESA CEDULA CON ESE PRODUCTO
                    Array.Resize(ref objparam, 3);
                    objparam[0] = _idproducto;
                    objparam[1] = txtCriterio.Text.Trim();
                    objparam[2] = 23;

                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                    if (dt != null)
                    {

                        try
                        {
                            if (dt.Tables[0].Rows.Count > 0) _titucodigo = int.Parse(dt.Tables[0].Rows[0]["TITU_CODIGO"].ToString());
                        }
                        catch (Exception)
                        {
                            _titucodigo = 0;
                        }
                    }

                    //_token = new Datos_Humana().GetToken(_urltoken, "usrprestasalud", "_.oMDBM263");

                    //_datositems = new Datos_Humana().GetItems(_urldatos, _token, txtCriterio.Text.Trim(), "1001722");

                    //dynamic strjson = JsonConvert.DeserializeObject(_datositems);


                    //if (strjson != null)
                    //{

                    //    DatosJson.Titular dttitular = new DatosJson.Titular();
                    //    DatosJson.Beneficiario dtbeneficiario = new DatosJson.Beneficiario();

                    //    var titular = strjson.rec_titular;

                    //    foreach (var titu in titular)
                    //    {

                    //        if (_titucodigo == 0)
                    //        {

                    //            dttitular.ti_tipoident = titu.ti_tipoident;
                    //            dttitular.ti_cedula = titu.ti_cedula;
                    //            dttitular.ti_nombre = titu.ti_nombre;
                    //            dttitular.ti_primernom = titu.ti_primernom;
                    //            dttitular.ti_segundonom = titu.ti_segundonom;
                    //            dttitular.ti_primerape = titu.ti_primerape;
                    //            dttitular.ti_segundoape = titu.ti_segundoape;
                    //            dttitular.ti_mail = titu.ti_mail == null ? "" : titu.ti_mail;
                    //            dttitular.ti_telefonos = titu.ti_telefono;
                    //            dttitular.ti_fechanaci = titu.ti_fecnaci == null ? "" : titu.ti_fecnaci;
                    //            dttitular.ti_estado = titu.ti_estado;
                    //            dttitular.ti_genero = titu.ti_codgenero;
                    //            dttitular.ti_contrato = titu.ti_contrato;
                    //            dttitular.ti_fechavigencia = titu.ti_fecinivigen;
                    //            dttitular.ti_codplan = titu.ti_codplan;

                    //            _telefonos = dttitular.ti_telefonos.Split(';');
                    //            _posicion = dttitular.ti_fechavigencia.IndexOf("--");

                    //            _fechaini = dttitular.ti_fechavigencia.Substring(0, _posicion).Trim();
                    //            _fechafin = dttitular.ti_fechavigencia.Substring(_posicion + 2).Trim();

                    //            switch (_telefonos.Length)
                    //            {
                    //                case 1:
                    //                    _fonocasa = _telefonos[0];
                    //                    break;
                    //                case 2:
                    //                    _fonocasa = _telefonos[0];
                    //                    _fonooficina = _telefonos[1];
                    //                    break;
                    //                case 3:
                    //                    _fonocasa = _telefonos[0];
                    //                    _fonooficina = _telefonos[1];
                    //                    _celular = _telefonos[2];
                    //                    break;
                    //            }

                    //            Array.Resize(ref objparam, 33);
                    //            objparam[0] = 0;
                    //            objparam[1] = "C";
                    //            objparam[2] = dttitular.ti_cedula;
                    //            objparam[3] = dttitular.ti_nombre;
                    //            objparam[4] = dttitular.ti_primernom;
                    //            objparam[5] = dttitular.ti_segundonom;
                    //            objparam[6] = dttitular.ti_primerape;
                    //            objparam[7] = dttitular.ti_segundoape;
                    //            objparam[8] = dttitular.ti_mail;
                    //            objparam[9] = _fonocasa;
                    //            objparam[10] = _fonooficina;
                    //            objparam[11] = _celular;
                    //            objparam[12] = dttitular.ti_fechanaci;
                    //            objparam[13] = dttitular.ti_estado;
                    //            objparam[14] = dttitular.ti_genero;
                    //            objparam[15] = dttitular.ti_contrato;
                    //            objparam[16] = _fechaini;
                    //            objparam[17] = _fechafin;
                    //            objparam[18] = dttitular.ti_codplan;
                    //            objparam[19] = _idproducto;
                    //            objparam[20] = _titucodigo;
                    //            objparam[21] = "";
                    //            objparam[22] = "";
                    //            objparam[23] = "";
                    //            objparam[24] = "";
                    //            objparam[25] = "";
                    //            objparam[26] = 0;
                    //            objparam[27] = 0;
                    //            objparam[28] = 0;
                    //            objparam[29] = 0;
                    //            objparam[30] = 0;
                    //            objparam[31] = int.Parse(Session["usuCodigo"].ToString());
                    //            objparam[32] = Session["MachineName"].ToString();

                    //            _dts = new Conexion(2, "").FunNewTitular(objparam);

                    //            _titucodigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                    //        }

                    //        if (_titucodigo > 0)
                    //        {

                    //            var beneficiario = titu.rec_dependiente;

                    //            if (beneficiario != null)
                    //            {
                    //                foreach (var vbene in beneficiario)
                    //                {
                    //                    _fila++;

                    //                    if (_fila > 1)
                    //                    {

                    //                        dtbeneficiario.de_tipoident = vbene.de_tipoident;
                    //                        dtbeneficiario.de_cedula = vbene.de_cedula;
                    //                        dtbeneficiario.de_nombre = vbene.de_nombre;
                    //                        dtbeneficiario.de_primernom = vbene.de_primernom;
                    //                        dtbeneficiario.de_segundonom = vbene.de_segundonom;
                    //                        dtbeneficiario.de_primerape = vbene.de_primerape;
                    //                        dtbeneficiario.de_segundoape = vbene.de_segundoape;
                    //                        dtbeneficiario.de_mail = vbene.de_mail == null ? "" : vbene.de_mail;
                    //                        dtbeneficiario.de_telefonos = vbene.de_telefono;
                    //                        dtbeneficiario.de_tipo = vbene.de_tipo;
                    //                        dtbeneficiario.de_autcodigo = vbene.de_autcodigo;
                    //                        dtbeneficiario.de_fechanaci = vbene.de_fechanac == null ? "" : vbene.de_fechanac;
                    //                        dtbeneficiario.de_genero = vbene.de_genero;

                    //                        _telefonos = dtbeneficiario.de_telefonos.Split(';');

                    //                        switch (_telefonos.Length)
                    //                        {
                    //                            case 1:
                    //                                _fonocasa = _telefonos[0];
                    //                                break;
                    //                            case 2:
                    //                                _fonocasa = _telefonos[0];
                    //                                _fonooficina = _telefonos[1];
                    //                                break;
                    //                            case 3:
                    //                                _fonocasa = _telefonos[0];
                    //                                _fonooficina = _telefonos[1];
                    //                                _celular = _telefonos[2];
                    //                                break;
                    //                        }

                    //                        Array.Resize(ref objparam, 33);
                    //                        objparam[0] = 0;
                    //                        objparam[1] = "C";
                    //                        objparam[2] = dtbeneficiario.de_cedula;
                    //                        objparam[3] = dtbeneficiario.de_nombre;
                    //                        objparam[4] = dtbeneficiario.de_primernom;
                    //                        objparam[5] = dtbeneficiario.de_segundonom;
                    //                        objparam[6] = dtbeneficiario.de_primerape;
                    //                        objparam[7] = dtbeneficiario.de_segundoape;
                    //                        objparam[8] = dtbeneficiario.de_mail;
                    //                        objparam[9] = _fonocasa;
                    //                        objparam[10] = _fonooficina;
                    //                        objparam[11] = _celular;
                    //                        objparam[12] = dtbeneficiario.de_fechanaci;
                    //                        objparam[13] = "";
                    //                        objparam[14] = dtbeneficiario.de_genero;
                    //                        objparam[15] = dtbeneficiario.de_autcodigo;
                    //                        objparam[16] = "";
                    //                        objparam[17] = "";
                    //                        objparam[18] = "";
                    //                        objparam[19] = 0;
                    //                        objparam[20] = _titucodigo;
                    //                        objparam[21] = dtbeneficiario.de_tipo;
                    //                        objparam[22] = "";
                    //                        objparam[23] = "";
                    //                        objparam[24] = "";
                    //                        objparam[25] = "";
                    //                        objparam[26] = 0;
                    //                        objparam[27] = 0;
                    //                        objparam[28] = 0;
                    //                        objparam[29] = 0;
                    //                        objparam[30] = 0;
                    //                        objparam[31] = int.Parse(Session["usuCodigo"].ToString());
                    //                        objparam[32] = Session["MachineName"].ToString();

                    //                        _dts = new Conexion(2, "").FunNewTitular(objparam);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }

                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = txtCriterio.Text.Trim().ToUpper();
                objparam[2] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCitaAdmin", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["grdvDatos"] = dt;
            }
            catch (Exception ex)
            {
                if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
                {
                    StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
                    _log.WriteLine("Error" + "|" + "Fecha_Registro");
                    _log.Close();
                }

                StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

                _writer.WriteLine(ex.Message + "|" + DateTime.Now);
                _writer.Close();
            }
        }
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    btnselec = (ImageButton)(e.Row.Cells[5].FindControl("btnselecc"));
                    string estado = e.Row.Cells[4].Text;
                    if (estado == "Inactivo")
                    {
                        btnselec.ImageUrl = "~/Botones/citamedicadesac.png";
                        btnselec.Height = 20;
                        btnselec.Enabled = false;
                    }
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