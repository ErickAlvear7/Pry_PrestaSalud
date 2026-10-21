using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmComercialSegurosUnidos : Page
    {
        private const int TamanoPagina = 20;
        private int PaginaActual
        {
            get { return ViewState["Pagina"] == null ? 1 : Convert.ToInt32(ViewState["Pagina"]); }
            set { ViewState["Pagina"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int usuario;
            if (!ObtenerUsuario(out usuario))
            {
                PnlConsulta.Visible = false; BtnNueva.Visible = false;
                Response.Redirect("~/Reload.html", false);
                Context.ApplicationInstance.CompleteRequest(); return;
            }
            if (!IsPostBack) { if (GuardarFiltros()) Cargar(); }
        }

        private bool ObtenerUsuario(out int usuario)
        {
            return int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) && usuario > 0;
        }

        private bool GuardarFiltros()
        {
            LblError.Text = "";
            DateTime desde, hasta;
            string inicio = TxtDesde.Text.Trim(), fin = TxtHasta.Text.Trim();
            if (inicio == "") desde = new DateTime(1900, 1, 1);
            else if (!DateTime.TryParseExact(inicio, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out desde))
            { LblError.Text = "La fecha desde no es válida."; return false; }
            if (fin == "") hasta = new DateTime(9999, 12, 30);
            else if (!DateTime.TryParseExact(fin, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hasta))
            { LblError.Text = "La fecha hasta no es válida."; return false; }
            if (hasta < desde || desde.Year < 1753 || hasta > new DateTime(9999, 12, 30))
            { LblError.Text = "Revisa el rango de fechas."; return false; }
            ViewState["Desde"] = desde.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            ViewState["Hasta"] = hasta.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            ViewState["Documento"] = TxtDocumento.Text.Trim();
            ViewState["Estado"] = DdlEstado.SelectedValue;
            return true;
        }

        private DataSet Consultar(int pagina)
        {
            int usuario;
            if (!ObtenerUsuario(out usuario)) throw new InvalidOperationException("Sesión no válida.");
            object[] parametros = { usuario, Convert.ToString(ViewState["Desde"]), Convert.ToString(ViewState["Hasta"]),
                Convert.ToString(ViewState["Documento"]).Replace("'", "''"), Convert.ToString(ViewState["Estado"]), pagina, TamanoPagina };
            DataSet ds = new Conexion(2, "").funConsultarSqls("sp_Examen_ComercialSU_Listar", parametros);
            if (ds == null || ds.Tables.Count < 2 || ds.Tables[1].Rows.Count != 1)
                throw new InvalidOperationException("No se pudo consultar el reporte. Verifique el acceso comercial y la campaña seleccionada.");
            return ds;
        }

        private void Cargar()
        {
            LblError.Text = "";
            try
            {
                DataSet ds = Consultar(PaginaActual);
                DataRow resumen = ds.Tables[1].Rows[0];
                PaginaActual = Convert.ToInt32(resumen["Pagina"]);
                int paginas = Convert.ToInt32(resumen["Paginas"]);
                GrdvSolicitudes.DataSource = ds.Tables[0]; GrdvSolicitudes.DataBind();
                if (GrdvSolicitudes.HeaderRow != null)
                { GrdvSolicitudes.UseAccessibleHeader = true; GrdvSolicitudes.HeaderRow.TableSection = TableRowSection.TableHeader; }
                LblTotal.Text = Convert.ToString(resumen["Total"]) + " solicitud(es)";
                LblConsulta.Text = "Consulta: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                LblPagina.Text = "Página " + PaginaActual + " de " + paginas;
                BtnAnterior.Enabled = PaginaActual > 1; BtnSiguiente.Enabled = PaginaActual < paginas;
                BtnNueva.Visible = true;
            }
            catch (Exception)
            {
                GrdvSolicitudes.DataSource = null; GrdvSolicitudes.DataBind();
                LblTotal.Text = ""; LblPagina.Text = ""; LblConsulta.Text = "";
                BtnAnterior.Enabled = false; BtnSiguiente.Enabled = false; BtnNueva.Visible = false;
                LblError.Text = "No se pudo consultar. Revise el acceso comercial, la campaña 24 seleccionada y el registro de errores de Conexion.";
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        { if (GuardarFiltros()) { PaginaActual = 1; Cargar(); } }
        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtDesde.Text = ""; TxtHasta.Text = ""; TxtDocumento.Text = ""; DdlEstado.SelectedIndex = 0;
            if (GuardarFiltros()) { PaginaActual = 1; Cargar(); }
        }
        protected void BtnAnterior_Click(object sender, EventArgs e)
        { PaginaActual = Math.Max(1, PaginaActual - 1); Cargar(); }
        protected void BtnSiguiente_Click(object sender, EventArgs e)
        { PaginaActual = Math.Min(1000000, PaginaActual + 1); Cargar(); }
        protected void BtnNueva_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificación del permiso en BD también al pulsar el botón.
                Consultar(1);
                Response.Redirect("FrmSolicitudExamen.aspx?CodigoPERS=0&CodigoEXSO=0&NumDocumento=&OrigenComercial=SU", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception) { LblError.Text = "No se pudo abrir el registro. Verifique su acceso comercial y la campaña seleccionada."; }
        }
    }
}
