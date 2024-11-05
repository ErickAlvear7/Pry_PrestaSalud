namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmCitaAdministrador : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        Image _imgestado = new Image();
        ImageButton _imgselecc = new ImageButton();
        ImageButton _imgausent = new ImageButton();
        string _codigocita = "", _titucodigo = "", _benecodigo = "", _sql = "", _prestador = "", _medicodigo = "", _fechacita = "", _horasistema = "", _fechaactual = "", _response = "";
        TimeSpan _horaactual, _horaagenda;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Atención Citas - Médicas << ADMINISTRADOR >>";
                FunCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        } 
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                _sql = "SELECT CodigoCita = CIT.CITA_CODIGO,TituCodigo = CIT.cita_titucodigo,BeneCodigo = CIT.cita_benecodigo,";
                _sql += "FechaCodigo = convert(varchar(10),HIS.hici_fechacita,101),Prestadora = PRE.pres_nombre,";
                _sql += "Paciente = dbo.funGetDatosCita(0,CIT.cita_tipocliente,case CIT.cita_tipocliente when 'T' then CIT.cita_titucodigo ";
                _sql += "else CIT.cita_benecodigo end),Tipo = case CIT.cita_tipocliente when 'T' then 'TITULAR' else 'BENEFICIARIO' end,";
                _sql += "FechaCita = convert(varchar(10),HIS.hici_fechacita,121),HoraCita = HIS.hici_horacita,CodigoMedico = CIT.MEDI_COD,";
                _sql += "Medico = (select MED.medi_nombre+' '+MED.medi_apellido from Expert_MEDICO MED (NOLOCK) WHERE MED.MEDI_COD=CIT.MEDI_COD),";
                _sql += "Cedula =(select PER.pers_numerodocumento from Expert_PERSONA PER (NOLOCK) WHERE PER.PERS_CODIGO=";
                _sql += "(SELECT TIT.PERS_CODIGO FROM Expert_TITULAR TIT (NOLOCK) WHERE TIT.TITU_CODIGO=CIT.cita_titucodigo))";
                _sql += " FROM Expert_CITAMEDICA CIT (NOLOCK) INNER JOIN Expert_HISTORIAL_CITAMEDICA HIS (NOLOCK) ON CIT.CITA_CODIGO=HIS.CITA_CODIGO ";
                _sql += "INNER JOIN Expert_PRESTADORA PRE (NOLOCK) ON CIT.PRES_COD=PRE.PRES_COD ";
                _sql += "INNER JOIN Expert_ESPECIPRESTADORA EPE (NOLOCK) ON CIT.EXPR_CODIGO=EPE.EXPR_CODIGO ";
                _sql += "WHERE CIT.cita_estatuscita='A' and CIT.EXPR_CODIGO NOT IN(212) AND HIS.hici_estadocita='A' AND CIT.PRES_COD in(";

                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                _objparam[1] = "";
                _objparam[2] = 167;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _fila in _dts.Tables[0].Rows)
                    {
                        _prestador += _fila["CodigoPRES"].ToString() + ",";
                    }

                    _prestador = _prestador.Remove(_prestador.Length - 1);
                    _prestador += ") ORDER BY HIS.hici_fechacreacion DESC";
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
                    string[] _hora = e.Row.Cells[6].Text.Split(new char[] { '-' });
                    _horaagenda = TimeSpan.Parse(_hora[0].ToString());
                    _imgestado = (Image)(e.Row.Cells[7].FindControl("ImgEstado"));
                    _imgselecc = (ImageButton)(e.Row.Cells[8].FindControl("IngAtencion"));
                    _imgausent = (ImageButton)(e.Row.Cells[9].FindControl("ImgAusente"));

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
                        _imgselecc.ImageUrl = "~/Botones/atencionmedicaoff.png";
                        _imgselecc.Enabled = false;
                        _imgselecc.Height = 20;
                        _imgselecc.Enabled = false;
                        _imgselecc.Height = 20;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        protected void IngAtencion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigocita = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCita"].ToString();
            _titucodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["TituCodigo"].ToString();
            _benecodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["BeneCodigo"].ToString();
            _medicodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoMedico"].ToString();
            Response.Redirect("FrmAtencionAdminMed.aspx?CodigoCita=" + _codigocita + "&CodigoTitu=" + _titucodigo + "&CodigoBene=" + _benecodigo + "&CodigoMedi=" + _medicodigo, true);
        }

        protected void ImgAusente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigocita = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCita"].ToString();
                _medicodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoMedico"].ToString();
                Array.Resize(ref _objparam, 14);
                _objparam[0] = 0;
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
                _objparam[12] = int.Parse(_medicodigo);
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

        protected void Tmrdat_Tick(object sender, EventArgs e)
        {
            FunCargaMantenimiento();
        } 
        #endregion
    }
}