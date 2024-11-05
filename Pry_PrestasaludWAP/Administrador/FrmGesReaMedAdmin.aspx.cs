namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmGesReaMedAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        Object[] _objparam = new Object[1];
        string _codigo = "", _prestador = "", _sql = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                FunCargaMantenimiento();
                Lbltitulo.Text = "Gestiones Realizadas << CONSULTA ADMINISTRADOR >>";
            }
        } 
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            try
            {
                _sql = "SELECT Identificacion = PE.pers_numerodocumento,Paciente = CASE CIT.cita_benecodigo WHEN 0 THEN PE.pers_primernombre+' '+PE.pers_segundonombre+' '+ PE.pers_primerapellido+' '+PE.pers_segundoapellido ELSE (SELECT pers_primernombre+' '+bene_segundonombre+' '+bene_primerapellido+' '+ bene_segundoapellido FROM Expert_BENEFICIARIO BE WHERE BE.BENE_CODIGO=CIT.cita_benecodigo) END,";
                _sql += "Parentesco = CASE CIT.cita_parentesco WHEN '0' THEN 'TITULAR' ELSE (SELECT pade_nombre FROM Expert_PARAMETRO_DETALLE WHERE pade_valor=CIT.cita_parentesco AND pade_estado=1 AND PARA_CODIGO IN (SELECT PARA_CODIGO FROM Expert_PARAMETRO_CABECERA WHERE para_estado=1 AND para_nombre='PARENTESCO')) END,";
                _sql += "FechaCita = CONVERT(VARCHAR(10),HIS.hici_fechacita,121),Estado = CASE CIT.cita_estatuscita WHEN 'A' THEN 'AGENDADA' WHEN 'C' THEN 'CANCELADA' WHEN 'T' THEN 'ATENTIDA' WHEN 'S' THEN 'AUSENTE' END,";
                _sql += "Medico = (SELECT MED.medi_nombre+' '+MED.medi_apellido FROM Expert_MEDICO MED (NOLOCK) WHERE MED.MEDI_COD=CIT.MEDI_COD), CitaCodigo = CIT.CITA_CODIGO ";
                _sql += " FROM Expert_CITAMEDICA CIT (NOLOCK) INNER JOIN Expert_HISTORIAL_CITAMEDICA HIS (NOLOCK) ON CIT.CITA_CODIGO=HIS.CITA_CODIGO ";
                _sql += "INNER JOIN Expert_TITULAR TI ON CIT.cita_titucodigo=TI.TITU_CODIGO ";
                _sql += "INNER JOIN Expert_PERSONA PE ON TI.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "WHERE CIT.cita_estatuscita='T' AND HIS.hici_estadocita='T' AND CIT.PRES_COD in(";

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
                    _prestador += ") ORDER BY CIT.cita_fechacreacion desc";
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
        protected void ImgAtencion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CitaCodigo"].ToString();
            Response.Redirect("../Medicos/FrmDetalleGestionesRealizadas.aspx?CodigoCita=" + _codigo + "&Regresar=1", true);
        }

        protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CitaCodigo"].ToString();
            Response.Redirect("../Medicos/RptRegistroAgendas.aspx?CodigoCita=" + _codigo + "&Regresar=1", true);
        } 
        #endregion
    }
}