namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Globalization;

    public partial class FrmCitaOdontoAdministrador : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        Image _imgestado = new Image();
        ImageButton _imgselecc = new ImageButton();
        ImageButton _imgausent = new ImageButton();
        string _fechacita = "", _horasistema = "", _response = "", _fechaactual = "", _sql = "", _prestador = "", _codigocita = "", _titucodigo = "", _benecodigo = "", _codigomedico = "";
        TimeSpan _horaactual, _horaagenda;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Atención Citas - Odontológicas << ADMINISTRADOR >>";
                FunCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        } 
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            try
            {
                _sql = "SELECT Prestadora = (SELECT PRD.prod_nombre FROM Expert_PRESTADORA_ODONTO PRD (NOLOCK) WHERE PRD.PPRO_CODIGO=CIT.PPRO_CODIGO),";
                _sql += "Medico = (SELECT MED.meod_nombre+' '+MED.meod_apellido FROM Expert_MEDICO_ODONTO MED (NOLOCK) WHERE MED.MEOD_CODIGO=CIT.MEOD_CODIGO),CodigoCita = CIT.CIOD_CODIGO,TituCodigo = CIT.ciod_titucodigo,BeneCodigo = CIT.ciod_benecodigo,";
                _sql += "FechaCodigo = CONVERT(VARCHAR(10),HIS.hico_fechacita,101),Tipo = CASE CIT.ciod_tipocliente WHEN 'T' THEN 'TITULAR' ELSE 'BENEFICIARIO' END,Cedula = dbo.funGetDatosCita(3,CIT.ciod_tipocliente,CIT.ciod_titucodigo),Paciente = dbo.funGetDatosCita(0,CIT.ciod_tipocliente,CASE CIT.ciod_tipocliente WHEN 'T' THEN CIT.ciod_titucodigo ELSE CIT.ciod_benecodigo END),Producto = dbo.funGetDatosCita(4,CIT.ciod_tipocliente,CIT.ciod_titucodigo),FechaCita = CONVERT(VARCHAR(10),HIS.hico_fechacita,121),HoraCita = SUBSTRING(HIS.hico_horacita,1,5)+':00',Evento = (SELECT pade_nombre FROM Expert_PARAMETRO_DETALLE (NOLOCK) WHERE pade_valor=HIS.hico_descripcioncita and PARA_CODIGO in(SELECT PARA_CODIGO FROM Expert_PARAMETRO_CABECERA WHERE para_nombre='MOTIVOS ODONTO')),CodigoGenerado = ISNULL((SELECT GS.cogo_secuencial FROM Expert_GENERAR_SECUENCIALO GS WHERE GS.cogo_hicocodigo=HIS.HICO_CODIGO),HIS.CIOD_CODIGO),CodigoMedico = CIT.MEOD_CODIGO,Medico = (SELECT MDO.meod_nombre+' '+MDO.meod_apellido FROM Expert_MEDICO_ODONTO MDO (NOLOCK) WHERE MDO.MEOD_CODIGO=CIT.MEOD_CODIGO) ";
                _sql += "FROM Expert_CITAODONTO CIT INNER JOIN Expert_HISTORIAL_CITAODONTO HIS ON CIT.CIOD_CODIGO=HIS.CIOD_CODIGO WHERE CIT.ciod_estatuscita='A' and HIS.hico_estadocita='A' AND CIT.PPRO_CODIGO IN( ";

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[1] = "";
                _objparam[2] = 168;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        _prestador += _fila["CodigoPRES"].ToString() + ",";
                    }

                    _prestador = _prestador.Remove(_prestador.Length - 1);
                    _prestador += ") ORDER BY HIS.hico_fechacreacion DESC";
                }

                _sql = _sql + _prestador;

                Array.Resize(ref _objparam, 11);
                _objparam[0] = 30;
                _objparam[1] = _sql;
                _objparam[2] = "";
                _objparam[3] = "";
                _objparam[4] = "";
                _objparam[5] = "";
                _objparam[6] = 0;
                _objparam[7] = 0;
                _objparam[8] = 0;
                _objparam[9] = 0;
                _objparam[10] = 0;
                _dts = new Conexion(2, "").FunConsultaDatos1(_objparam);
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void Tmrdat_Tick(object sender, EventArgs e)
        {
            FunCargaMantenimiento();
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _horasistema = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "00";
                    _fechacita = GrdvDatos.DataKeys[e.Row.RowIndex].Values["FechaCodigo"].ToString();
                    _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
                    _horaactual = TimeSpan.Parse(_horasistema);
                    string[] hora = e.Row.Cells[7].Text.Split(new char[] { '-' });
                    _horaagenda = TimeSpan.Parse(hora[0].ToString());
                    _imgestado = (Image)(e.Row.Cells[8].FindControl("imgEstado"));
                    _imgselecc = (ImageButton)(e.Row.Cells[9].FindControl("ImgAtencion"));
                    _imgausent = (ImageButton)(e.Row.Cells[10].FindControl("imgAusente"));

                    if (DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(_fechacita, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        if (_horaactual > _horaagenda)
                        {
                            _imgestado.ImageUrl = "~/Botones/relojoff.png";
                            _imgestado.Height = 20;
                        }
                        else
                        {
                            _imgestado.ImageUrl = "~/Botones/relojon.png";
                            _imgestado.Height = 20;
                        }
                    }
                    else
                    {
                        _imgselecc.ImageUrl = "~/Botones/mueladisable.png";
                        _imgselecc.Enabled = false;
                        _imgausent.Enabled = false;
                        _imgselecc.Height = 20;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        protected void ImgAtencion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigomedico = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoMedico"].ToString();
            _codigocita = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCita"].ToString();
            _titucodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["TituCodigo"].ToString();
            _benecodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["BeneCodigo"].ToString();
            Response.Redirect("FrmAtencionOdontoAdministrador.aspx?CodigoMedico=" + _codigomedico + "&CodigoCita=" + _codigocita + "&CodigoTitu=" + _titucodigo + "&CodigoBene=" + _benecodigo, true);
        }

        protected void ImgAusente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocita = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCita"].ToString();

                //Array.Resize(ref _objparam, 3);
                //_objparam[0] = int.Parse(_codigocita);
                //_objparam[1] = "";
                //_objparam[2] = 78;
                //_dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                //if (_dts.Tables[0].Rows.Count > 0)
                //{
                //    _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "No se puede poner ausente, Tiene agreados procedimientos..!");
                //    Response.Redirect(_response, false);
                //    return;
                //}

                Array.Resize(ref _objparam, 14);
                _objparam[0] = 1;
                _objparam[1] = int.Parse(_codigocita);
                _objparam[2] = "S";
                _objparam[3] = 98;
                _objparam[4] = "S";
                _objparam[5] = "PACIENTE NO ASISTE";
                _objparam[6] = DateTime.Now.ToString("MM/dd/yyyy");
                _objparam[7] = DateTime.Now.ToString("HH:mm");
                _objparam[8] = "";
                _objparam[9] = "";
                _objparam[10] = 0;
                _objparam[11] = 0;
                _objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[13] = Session["MachineName"].ToString();
                _dts = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", _objparam);
                _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(_response, false);
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        } 
        #endregion
    }
}