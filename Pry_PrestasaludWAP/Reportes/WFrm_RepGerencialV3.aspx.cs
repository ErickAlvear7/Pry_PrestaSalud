using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Reportes
{
    public partial class WFrm_RepGerencialV3 : System.Web.UI.Page
    {
        #region Variables
        ListItem ItemC = new ListItem();
        ListItem ItemP = new ListItem();
        Object[] Objparam = new Object[1];
        DataSet Dts = new DataSet();
        DataSet DtsCabecera = new DataSet();
        DataSet DtsCiudad = new DataSet();
        DataSet DtsPrestador = new DataSet();
        DataSet DtsEspeci = new DataSet();
        DataSet DtsGenero = new DataSet();
        bool Validar = false, Continuar = true;
        string PathLogoCabecera = "", PathLogoPie = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    Lbltitulo.Text = "REPORTE GERENCIAL CITAS MEDICAS";
                    TxtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    ViewState["Boton"] = "1";
                    ViewState["Iniciar"] = "S";
                    FunCargarCombos(0);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref Objparam, 11);
                    Objparam[0] = 13;
                    Objparam[1] = "";
                    Objparam[2] = "";
                    Objparam[3] = "";
                    Objparam[4] = "";
                    Objparam[5] = "";
                    Objparam[6] = 0;
                    Objparam[7] = int.Parse(Session["usuCodigo"].ToString()); 
                    Objparam[8] = 0;
                    Objparam[9] = 0;
                    Objparam[10] = 0;
                    Dts = new Conexion(2, "").FunConsultaDatos1(Objparam);
                    DdlCliente.DataSource = Dts;
                    DdlCliente.DataTextField = "Descripcion";
                    DdlCliente.DataValueField = "Codigo";
                    DdlCliente.DataBind();
                    DdlCliente.SelectedIndex = 1;

                    Objparam[0] = 14;
                    Objparam[6] = int.Parse(DdlCliente.SelectedValue);
                    Dts= new Conexion(2, "").FunConsultaDatos1(Objparam);
                    DdlProducto.DataSource = Dts;
                    DdlProducto.DataTextField = "Descripcion";
                    DdlProducto.DataValueField = "Codigo";
                    DdlProducto.DataBind();
                    DdlProducto.SelectedIndex = 1;
                    break;
                case 1:
                    Array.Resize(ref Objparam, 11);
                    Objparam[0] = 14;
                    Objparam[1] = "";
                    Objparam[2] = "";
                    Objparam[3] = "";
                    Objparam[4] = "";
                    Objparam[5] = "";
                    Objparam[6] = int.Parse(DdlCliente.SelectedValue);
                    Objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                    Objparam[8] = 0;
                    Objparam[9] = 0;
                    Objparam[10] = 0;
                    Objparam[0] = 14;
                    Objparam[6] = int.Parse(DdlCliente.SelectedValue);
                    Dts = new Conexion(2, "").FunConsultaDatos1(Objparam);
                    DdlProducto.DataSource = Dts;
                    DdlProducto.DataTextField = "Descripcion";
                    DdlProducto.DataValueField = "Codigo";
                    DdlProducto.DataBind();
                    DdlProducto.SelectedIndex = 1;
                    break;
            }
        }

        private bool FunValidarCampos()
        {
            try
            {
                Validar = true;
                if (DdlCliente.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Cliente..!", this);
                    Validar = false;
                }
                if (DdlProducto.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Producto..!", this);
                    Validar = false;
                }
                if (!new Funciones().IsDate(TxtFechaDesde.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    Validar = false;
                }
                if (!new Funciones().IsDate(TxtFechaHasta.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    Validar = false;
                }
                if (DateTime.ParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new Funciones().funShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    Validar = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return Validar;
        }

        private void FunGenerarReporte(DataTable DtbCabecera, DataTable DtbCiudad, DataTable DtbPrestadora, DataTable DtbEspecialidad,
            DataTable DtbGenero)
        {
            try
            {
                RptGerencial.Reset();
                RptGerencial.SizeToReportContent = true;
                RptGerencial.LocalReport.ReportPath = Server.MapPath("~/Reports/RtpGerenDoctor.rdlc");
                RptGerencial.LocalReport.EnableExternalImages = true;
                RptGerencial.LocalReport.DataSources.Clear();
                ReportDataSource DtsCabecera = new ReportDataSource("DsCabecera", DtbCabecera);
                ReportDataSource DtsCiudad = new ReportDataSource("DsCiudad", DtbCiudad);
                ReportDataSource DtsPrestadora = new ReportDataSource("DsPrestadora", DtbPrestadora);
                ReportDataSource DtsEspecialidad = new ReportDataSource("DsEspecialidad", DtbEspecialidad);
                ReportDataSource DtsGenero = new ReportDataSource("DsGenero", DtbGenero);

                RptGerencial.LocalReport.DataSources.Add(DtsCabecera);
                RptGerencial.LocalReport.DataSources.Add(DtsCiudad);
                RptGerencial.LocalReport.DataSources.Add(DtsPrestadora);
                RptGerencial.LocalReport.DataSources.Add(DtsEspecialidad);
                RptGerencial.LocalReport.DataSources.Add(DtsGenero);
                RptGerencial.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                Continuar = FunValidarCampos();
                if (Continuar)
                {
                    Array.Resize(ref Objparam, 3);
                    Objparam[0] = int.Parse(DdlCliente.SelectedValue);
                    Objparam[1] = "";
                    Objparam[2] = 112;
                    Dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", Objparam);
                    ViewState["LogoC"] = Dts.Tables[0].Rows[0][0].ToString();
                    ViewState["LogoP"] = Dts.Tables[0].Rows[0][1].ToString();
                    PathLogoCabecera = new Uri(Server.MapPath(ViewState["LogoC"].ToString())).AbsoluteUri;
                    PathLogoPie = new Uri(Server.MapPath(ViewState["LogoP"].ToString())).AbsoluteUri;

                    //OBETENER GESTIONES REALIZADAS PRIMERA CABECERA REPORTE
                    Array.Resize(ref Objparam, 18);
                    Objparam[0] = 0;
                    Objparam[1] = int.Parse(DdlCliente.SelectedValue);
                    Objparam[2] = int.Parse(DdlProducto.SelectedValue);
                    Objparam[3] = DdlCliente.SelectedItem.ToString();
                    Objparam[4] = DdlProducto.SelectedItem.ToString();
                    Objparam[5] = "";
                    Objparam[6] = "";
                    Objparam[7] = "";
                    Objparam[8] = TxtFechaDesde.Text.Trim();
                    Objparam[9] = TxtFechaHasta.Text.Trim();
                    Objparam[10] = PathLogoCabecera;
                    Objparam[11] = PathLogoPie;
                    Objparam[12] = "";
                    Objparam[13] = "";
                    Objparam[14] = "";
                    Objparam[15] = 0;
                    Objparam[16] = 0;
                    Objparam[17] = 0;
                    DtsCabecera = new Conexion(2, "").FunRepGerenCabecera(Objparam);
                    if (int.Parse(DtsCabecera.Tables[0].Rows[0]["TotalRegistros"].ToString()) == 0)
                    {
                        new Funciones().funShowJSMessage("No Existe Registros Para Mostrar..!", this);
                        return;
                    }

                    if (ViewState["Boton"].ToString() == "1")
                    {
                        ViewState["Boton"] = "0";
                        PnlReporteGerencial.Visible = true;
                        PnlDivision0.Visible = false;
                        BtnProcesar.Text = "Consultar";
                    }
                    else
                    {
                        ViewState["Boton"] = "1";
                        PnlReporteGerencial.Visible = false;
                        PnlDivision0.Visible = true;
                        BtnProcesar.Text = "Procesar";
                    }
                    Array.Resize(ref Objparam, 12);
                    Objparam[0] = 0;
                    Objparam[1] = int.Parse(DdlCliente.SelectedValue);
                    Objparam[2] = int.Parse(DdlProducto.SelectedValue);
                    Objparam[3] = TxtFechaDesde.Text.Trim();
                    Objparam[4] = TxtFechaHasta.Text.Trim();
                    Objparam[5] = "";
                    Objparam[6] = "";
                    Objparam[7] = "";
                    Objparam[8] = "";
                    Objparam[9] = 0;
                    Objparam[10] = 0;
                    Objparam[11] = 0;
                    //OBTENER DATOS POR CIUDAD                  
                    DtsCiudad = new Conexion(2, "").FunRepGerenMedicos("sp_RepGerenMedCiud", Objparam);
                    //OBTENER DATOS POR PRESTADORA
                    DtsPrestador = new Conexion(2, "").FunRepGerenMedicos("sp_RepGerenMedPresta", Objparam);
                    //OBTENER DATOS POR ESPECIALIDAD
                    DtsEspeci = new Conexion(2, "").FunRepGerenMedicos("sp_RepGerenMedEspe", Objparam);
                    //OBTENER DATOS POR GENERO
                    DtsGenero = new Conexion(2, "").FunRepGerenMedicos("sp_RepGerenMedGene", Objparam);
                    //OBTENER ENCABEZADO

                    FunGenerarReporte(DtsCabecera.Tables[0], DtsCiudad.Tables[0], DtsPrestador.Tables[0],
                        DtsEspeci.Tables[0], DtsGenero.Tables[0]);
                }
                else new Funciones().funShowJSMessage("No Existen Datos para Generar el Reporte..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}