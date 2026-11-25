using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmNuevoExamenCliente : Page
    {
        private const int TamanoPagina = 5;
        private int PaginaActual
        {
            get { return ViewState["PaginaCliente"] == null ? 1 : Convert.ToInt32(ViewState["PaginaCliente"]); }
            set { ViewState["PaginaCliente"] = value; }
        }
        private int UsuarioActual()
        {
            int usuario;
            if (!int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) || usuario <= 0)
                throw new InvalidOperationException("La sesión expiró. Ingrese nuevamente.");
            return usuario;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UsuarioActual();
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Seguimiento de solicitudes y exámenes";
                    if (GuardarFiltros()) CargarSolicitudesCliente();
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Reload.html", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
        private bool GuardarFiltros()
        {
            Lblerror.Text = "";
            DateTime desde, hasta;
            if (!LeerFecha(TxtDesde.Text, new DateTime(1900, 1, 1), out desde)
                || !LeerFecha(TxtHasta.Text, new DateTime(9999, 12, 30), out hasta)
                || desde > hasta || desde.Year < 1753 || hasta > new DateTime(9999, 12, 30))
            { Lblerror.Text = "Revise el rango de fechas."; return false; }
            ViewState["DesdeCliente"] = desde; ViewState["HastaCliente"] = hasta;
            ViewState["DocumentoCliente"] = TxtDocumento.Text.Trim();
            ViewState["EstadoCliente"] = DdlEstado.SelectedValue;
            return true;
        }
        private bool LeerFecha(string texto, DateTime predeterminada, out DateTime valor)
        {
            valor = predeterminada;
            return string.IsNullOrWhiteSpace(texto) || DateTime.TryParseExact(texto.Trim(), "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out valor);
        }
        private DataSet Consultar(int pagina, int solicitud = 0)
        {
            object[] argumentos = solicitud > 0
                ? new object[] { UsuarioActual(), null, null, "", "", 1, 1, solicitud }
                : new object[] { UsuarioActual(), ViewState["DesdeCliente"], ViewState["HastaCliente"],
                    Convert.ToString(ViewState["DocumentoCliente"]), Convert.ToString(ViewState["EstadoCliente"]),
                    pagina, TamanoPagina, 0 };
            DataSet ds = new Conexion(2, "").FunSeguimientoExamenesCliente(argumentos);
            if (ds == null || ds.Tables.Count < 2 || ds.Tables[1].Rows.Count != 1)
                throw new InvalidOperationException("El seguimiento no devolvió una respuesta válida.");
            return ds;
        }
        private void CargarSolicitudesCliente()
        {
            Lblerror.Text = "";
            try
            {
                DataSet ds = Consultar(PaginaActual);
                DataRow resumen = ds.Tables[1].Rows[0];
                PaginaActual = Convert.ToInt32(resumen["Pagina"]);
                int paginas = Convert.ToInt32(resumen["Paginas"]);
                GrdvGrupoExamen.DataSource = ds.Tables[0]; GrdvGrupoExamen.DataBind();
                if (GrdvGrupoExamen.HeaderRow != null)
                {
                    GrdvGrupoExamen.UseAccessibleHeader = true;
                    GrdvGrupoExamen.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                LblTotal.Text = Convert.ToString(resumen["Total"]) + " solicitud(es)";
                LblPagina.Text = "Página " + PaginaActual + " de " + paginas;
                DateTime desdeAplicado = Convert.ToDateTime(ViewState["DesdeCliente"]);
                DateTime hastaAplicado = Convert.ToDateTime(ViewState["HastaCliente"]);
                string desdeTexto = desdeAplicado.Year == 1900 ? "Sin límite" : desdeAplicado.ToString("dd/MM/yyyy");
                string hastaTexto = hastaAplicado.Year == 9999 ? "Sin límite" : hastaAplicado.ToString("dd/MM/yyyy");
                ListItem estadoAplicado = DdlEstado.Items.FindByValue(Convert.ToString(ViewState["EstadoCliente"]));
                LblConsulta.Text = HttpUtility.HtmlEncode("Consulta: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    + " · Desde: " + desdeTexto + " · Hasta: " + hastaTexto
                    + " · Estado: " + (estadoAplicado == null ? "Todos" : estadoAplicado.Text));
                BtnAnterior.Enabled = PaginaActual > 1; BtnSiguiente.Enabled = PaginaActual < paginas;
            }
            catch (Exception ex)
            {
                GrdvGrupoExamen.DataSource = null; GrdvGrupoExamen.DataBind();
                LblTotal.Text = ""; LblPagina.Text = ""; LblConsulta.Text = "";
                BtnAnterior.Enabled = false; BtnSiguiente.Enabled = false;
                Lblerror.Text = HttpUtility.HtmlEncode(ex.Message);
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            if (GuardarFiltros())
            {
                PaginaActual = 1; CargarSolicitudesCliente();
            }
        }
        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtDesde.Text = ""; TxtHasta.Text = ""; TxtDocumento.Text = ""; DdlEstado.SelectedIndex = 0;
            if (GuardarFiltros())
            {
                PaginaActual = 1; CargarSolicitudesCliente();
            }
        }
        protected void BtnAnterior_Click(object sender, EventArgs e)
        {
            PaginaActual = Math.Max(1, PaginaActual - 1); CargarSolicitudesCliente();
        }
        protected void BtnSiguiente_Click(object sender, EventArgs e)
        {
            PaginaActual = Math.Min(1000000, PaginaActual + 1); CargarSolicitudesCliente();
        }
        protected void GrdvGrupoExamen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            DataRowView fila = e.Row.DataItem as DataRowView;
            if (fila == null) return;
            string estado = Convert.ToString(fila["EstadoCodigo"]).Trim().ToUpperInvariant();
            bool apelable = Convert.ToBoolean(fila["TieneRechazoApelable"]);
            Label etiqueta = e.Row.FindControl("LblEstadoFila") as Label;
            if (etiqueta != null)
            {
                TableCell celda = etiqueta.Parent as TableCell;
                if (celda != null)
                {
                    celda.Font.Bold = true;
                    if (estado == "AUR" || estado == "AUA")
                    {
                        celda.BackColor = apelable ? System.Drawing.Color.PeachPuff
                            : estado == "AUR" ? System.Drawing.Color.LightCoral : System.Drawing.Color.LightGreen;
                        celda.ForeColor = apelable ? System.Drawing.Color.DarkOrange
                            : estado == "AUR" ? System.Drawing.Color.DarkRed : System.Drawing.Color.DarkGreen;
                    }
                    else if (estado == "EXR")
                    { celda.BackColor = System.Drawing.Color.Aquamarine; celda.ForeColor = System.Drawing.Color.DarkSlateGray; }
                }
            }
            ImageButton boton = e.Row.FindControl("ImgGestion") as ImageButton;
            if (boton == null) return;
            bool permitido = Convert.ToBoolean(fila["PuedeGestionar"]);
            boton.Enabled = permitido;
            boton.ImageUrl = permitido ? "~/Botones/modificar.png" : "~/Botones/selecc.png";
            boton.ToolTip = permitido ? "Consultar / gestionar auditoría"
                : Convert.ToString(fila["Registro"]) == "Cancelada" ? "Solicitud cancelada"
                : "Disponible al completar la auditoría de todos los pacientes";
            boton.Style["opacity"] = permitido ? "1" : "0.35";
            boton.Style["cursor"] = permitido ? "pointer" : "not-allowed";
        }
        protected void ImgGestion_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int solicitud;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out solicitud) || solicitud <= 0)
                    throw new InvalidOperationException("No se pudo identificar la solicitud.");
                // Revalidar campaña, producto y auditoría: no confiar solo en el botón dibujado.
                DataSet ds = Consultar(1, solicitud);
                if (ds.Tables[0].Rows.Count != 1 || !Convert.ToBoolean(ds.Tables[0].Rows[0]["PuedeGestionar"]))
                    throw new InvalidOperationException("La solicitud no está disponible para gestión en su campaña o la auditoría no está completa.");
                Response.Redirect("FrmGestionExamenCliente.aspx?CodigoEXSO=" + solicitud, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }
    }
}
