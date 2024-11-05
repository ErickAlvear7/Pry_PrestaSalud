namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmPresuRealizadoAdministrador : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        string _codigo = "", _paciente = "", _titucodigo = "", _benecodigo = "", _medico = "", _codigoprestadora = "", _sql = "", _prestador = "", _codigomedico = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Presupuesto - Procedimientos Registrados << CITAS ODONTOLOGICAS >>";
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
                _sql = "SELECT DISTINCT Identificacion = (SELECT PE.pers_numerodocumento FROM Expert_PERSONA PE INNER JOIN Expert_TITULAR TI ON PE.PERS_CODIGO=TI.PERS_CODIGO WHERE TI.TITU_CODIGO=PCA.psto_titucodigo),Paciente = dbo.funGetDatosCita(0,PCA.psto_tipocliente,CASE PCA.psto_tipocliente WHEN 'T' THEN PCA.psto_titucodigo ELSE PCA.psto_benecodigo END),Parentesco = ISNULL((SELECT pade_nombre FROM Expert_PARAMETRO_DETALLE WHERE pade_valor=PCA.psto_parentesco AND pade_estado=1 AND PARA_CODIGO in(SELECT PARA_CODIGO FROM Expert_PARAMETRO_CABECERA WHERE para_estado=1 AND para_nombre='PARENTESCO')),'TITULAR'),FechaPresupuesto = CONVERT(VARCHAR(10),PCA.psto_fechapresupuesto,121),Estado = (SELECT pade_nombre FROM Expert_PARAMETRO_DETALLE WHERE pade_valor=PCA.psto_estado AND PARA_CODIGO in(SELECT PARA_CODIGO FROM Expert_PARAMETRO_CABECERA WHERE para_nombre='CABECERA PRESUPUESTO')),CodigoCabecera = PCA.PSTO_CODIGO,TituCodigo = PCA.psto_titucodigo,BeneCodigo = PCA.psto_benecodigo,Medico = (SELECT meod_nombre+' '+meod_apellido FROM Expert_MEDICO_ODONTO WHERE MEOD_CODIGO=CID.MEOD_CODIGO),CodigoPrestadora = CID.PPRO_CODIGO,CodigoMedico = CID.MEOD_CODIGO  ";
                _sql += "FROM Expert_PRESUPUESTO_CABECERA PCA ";
                _sql += "INNER JOIN Expert_CITAODONTO CID ON CID.ciod_titucodigo=PCA.psto_titucodigo AND CID.ciod_benecodigo=PCA.psto_benecodigo ";
                _sql += "WHERE CID.ciod_estatuscita='F' AND CID.PPRO_CODIGO IN(";

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
                    _prestador += ") ORDER BY FechaPresupuesto DESC";
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
        protected void ImgRealizado_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString();
            _titucodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["TituCodigo"].ToString();
            _benecodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["BeneCodigo"].ToString();
            _codigoprestadora = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPrestadora"].ToString();
            _codigomedico = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoMedico"].ToString();
            Response.Redirect("../MedicoOdonto/FrmPresupuestosRegistrados.aspx?CodigoCabecera=" + _codigo + "&CodigoMedico=" + _codigomedico + "&TituCodigo=" + _titucodigo + "&BeneCodigo=" + _benecodigo + "&CodigoPrestadora=" + _codigoprestadora + "&Regresar=1", true);
        }

        protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString();
            _paciente = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Paciente"].ToString();
            _medico = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Medico"].ToString();
            _codigoprestadora = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPrestadora"].ToString();
            Response.Redirect("../MedicoOdonto/RptPresupuestoRegistrado.aspx?CodigoCabecera=" + _codigo + "&CodigoPrestadora=" + _codigoprestadora + "&Regresar=1", true);
        } 
        #endregion
    }
}