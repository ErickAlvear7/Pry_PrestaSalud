using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmMantenimientoExamenes : Page
    {
        private int Id(string clave)
        {
            return ViewState[clave] == null ? 0 : Convert.ToInt32(ViewState[clave]);
        }
        private int Elegido(DropDownList c)
        {
            int n;
            return int.TryParse(c.SelectedValue, out n) ? n : 0;
        }
        private void Mostrar(Exception e)
        {
            LblMensaje.Text = System.Web.HttpUtility.HtmlEncode(e.Message);
        }
        private DataTable Ejecutar(string accion, string nombre = "", string detalle = "", object pvp = null, object red = null, bool activo = true, int prestadorEdicion = -1)
        {
            int usuario;
            if (!int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) || usuario <= 0)
                throw new InvalidOperationException("La sesión expiró. Ingrese nuevamente.");
            object[] args ={accion,Elegido(DdlCiudad),prestadorEdicion<0?Elegido(DdlPrestador):prestadorEdicion,Id("Examen"),Id("Relacion"),
            nombre,detalle,pvp,red,activo,usuario,Convert.ToString(Session["MachineName"]),
            TxtDireccion.Text.Trim(),TxtTelefono.Text.Trim(),TxtEmail.Text.Trim()};
            DataSet ds = new Conexion(2, "").FunMantenimientoExamenes(args);
            if (ds == null || ds.Tables.Count == 0) throw new InvalidOperationException("El mantenimiento no devolvió respuesta.");
            return ds.Tables[0];
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int usuario;
            if (!int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) || usuario <= 0)
            {
                Response.Redirect("~/Reload.html", false);
                Context.ApplicationInstance.CompleteRequest(); return;
            }
            if (!IsPostBack) try
                {
                    Lista(DdlCiudad, Ejecutar("CIUDADES"), "Seleccione ciudad");
                    Lista(DdlPrestador, new DataTable(), "Seleccione prestador"); CargarExamenes(); Tarifas();
                }
                catch (Exception ex)
                {
                    Mostrar(ex);
                }
        }
        private void Lista(DropDownList control, DataTable tabla, string texto, int seleccionar = 0)
        {
            control.Items.Clear();
            foreach (DataRow r in tabla.Rows)
            {
                string n = Convert.ToString(r["Nombre"]);
                if (tabla.Columns.Contains("Activo") && !Convert.ToBoolean(r["Activo"])) n += " [INACTIVO]";
                control.Items.Add(new ListItem(n, Convert.ToString(r["Codigo"])));
            }
            control.Items.Insert(0, new ListItem(texto, "0"));
            ListItem item = control.Items.FindByValue(seleccionar.ToString());
            if (item != null) control.SelectedValue = item.Value;
        }
        private void CargarExamenes()
        {
            DataTable tabla = Ejecutar("EXAMENES"); ViewState["Catalogo"] = tabla;
            int elegido = Elegido(DdlExamen);
            Lista(DdlExamen, tabla, "Seleccione examen", elegido);
            DataTable filtrada = tabla.Clone();
            string buscar = TxtBuscarExamen.Text.Trim();
            foreach (DataRow fila in tabla.Rows)
                if (buscar == "" || Convert.ToString(fila["Nombre"]).IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   Convert.ToString(fila["Codigo"]).IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0)
                    filtrada.ImportRow(fila);
            int ultima = Math.Max(0, (filtrada.Rows.Count - 1) / GrdExamenes.PageSize);
            if (GrdExamenes.PageIndex > ultima) GrdExamenes.PageIndex = ultima;
            GrdExamenes.DataSource = filtrada; GrdExamenes.DataBind();
        }
        protected void BuscarExamen(object sender, EventArgs e)
        {
            try
            {
                LblMensaje.Text = "";
                GrdExamenes.PageIndex = 0;
                CargarExamenes();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void LimpiarBusqueda(object sender, EventArgs e)
        {
            TxtBuscarExamen.Text = "";
            BuscarExamen(sender, e);
        }
        private void Prestadores(int seleccionar = 0)
        {
            DataTable t = Ejecutar("PRESTADORES");
            ViewState["Prestadores"] = t;
            Lista(DdlPrestador, t, "Seleccione prestador", seleccionar);
        }
        private void CargarHistorial()
        {
            int anterior = Id("Relacion");
            try
            {
                ViewState["Relacion"] = Id("HistorialRelacion");
                GrdHistorial.DataSource = Convert.ToString(ViewState["HistorialTipo"]) == "SUB"
                    ? EjecutarSub("HISTORIAL", relacion: Id("HistorialRelacion")) : Ejecutar("HISTORIAL"); GrdHistorial.DataBind();
            }
            finally
            {
                ViewState["Relacion"] = anterior;
            }
        }
        protected void HistorialPagina(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdHistorial.PageIndex = e.NewPageIndex;
                CargarHistorial();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void CerrarHistorial(object sender, EventArgs e)
        {
            PnlHistorial.Visible = false;
            ViewState["HistorialRelacion"] = null;
        }
        private void Tarifas()
        {
            CerrarHistorial(null, null);
            DataTable t = Ejecutar("TARIFAS"); ViewState["Tarifas"] = t;
            DataTable filtrada = t.Clone(); string buscar = TxtBuscarTarifa.Text.Trim();
            foreach (DataRow fila in t.Rows)
                if (buscar == "" || Convert.ToString(fila["Examen"]).IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    Convert.ToString(fila["Codigo"]).IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    Convert.ToString(fila["ExamenCodigo"]).IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0)
                    filtrada.ImportRow(fila);
            int ultima = Math.Max(0, (filtrada.Rows.Count - 1) / GrdTarifas.PageSize);
            if (GrdTarifas.PageIndex > ultima) GrdTarifas.PageIndex = ultima;
            GrdTarifas.DataSource = filtrada; GrdTarifas.DataBind();
        }
        protected void BuscarTarifa(object sender, EventArgs e)
        {
            try
            {
                LblMensaje.Text = "";
                GrdTarifas.PageIndex = 0;
                Tarifas();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void LimpiarBusquedaTarifa(object sender, EventArgs e)
        {
            TxtBuscarTarifa.Text = "";
            BuscarTarifa(sender, e);
        }
        private void CargarEditorTarifa(DataRow r)
        {
            ViewState["Relacion"] = Convert.ToInt32(r["Codigo"]);
            ViewState["Examen"] = Convert.ToInt32(r["ExamenCodigo"]);
            DdlExamen.SelectedValue = Convert.ToString(r["ExamenCodigo"]); DdlExamen.Enabled = true;
            TxtPvp.Text = r["PVP"] == DBNull.Value ? "" : Convert.ToDecimal(r["PVP"]).ToString("0.00", CultureInfo.InvariantCulture);
            TxtRed.Text = r["RED"] == DBNull.Value ? "" : Convert.ToDecimal(r["RED"]).ToString("0.00", CultureInfo.InvariantCulture);
            TxtNota.Text = Convert.ToString(r["Nota"]); ChkTarifa.Checked = Convert.ToBoolean(r["Activo"]);
            MostrarModoPrecio(Convert.ToInt32(r["ExamenCodigo"]));
            if (Convert.ToBoolean(r["TieneSubcategorias"]))
                AbrirTarifasSub(Convert.ToInt32(r["ExamenCodigo"]), Convert.ToInt32(r["Codigo"]));
        }
        protected void ExamenTarifaCambio(object sender, EventArgs e)
        {
            try
            {
                LblMensaje.Text = "";
                CerrarHistorial(null, null);
                int examen = Elegido(DdlExamen);
                NuevaTarifa(null, null);
                if (examen <= 0) return;
                DdlExamen.SelectedValue = examen.ToString();
                ViewState["Examen"] = examen;
                MostrarModoPrecio(examen);
                if (Elegido(DdlCiudad) <= 0 || Elegido(DdlPrestador) <= 0)
                    throw new InvalidOperationException("Seleccione ciudad y prestador.");
                DataTable tarifas = Ejecutar("TARIFAS"); ViewState["Tarifas"] = tarifas;
                DataRow encontrada = null;
                foreach (DataRow r in tarifas.Rows)
                    if (Convert.ToInt32(r["ExamenCodigo"]) == examen)
                    {
                        if (encontrada != null) throw new InvalidOperationException("Hay tarifas duplicadas para este examen. Revise las asociaciones.");
                        encontrada = r;
                    }
                if (encontrada != null)
                {
                    CargarEditorTarifa(encontrada);
                    LblMensaje.Text = "Tarifa existente cargada para editar.";
                }
                else LblMensaje.Text = PnlPrecioPrincipal.Visible
                    ? "Examen sin tarifa en este prestador. Ingrese PVP y RED para asociarlo."
                    : "Guarde la asociación del examen y luego configure las tarifas de sus subcategorías.";
            }
            catch (Exception ex)
            {
                NuevaTarifa(null, null);
                Mostrar(ex);
            }
        }
        protected void CiudadCambio(object sender, EventArgs e)
        {
            try
            {
                LblMensaje.Text = "";
                //NuevoPrestador(null, null); 
                LimpiarPrestador();
                NuevaTarifa(null, null);
                TxtBuscarTarifa.Text = "";
                Prestadores();
                GrdTarifas.PageIndex = 0;
                Tarifas();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void PrestadorCambio(object sender, EventArgs e)
        {
            try
            {
                LblMensaje.Text = "";
                LimpiarPrestador();
                NuevaTarifa(null, null);
                TxtBuscarTarifa.Text = "";
                GrdTarifas.PageIndex = 0;
                Tarifas();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        private void LimpiarPrestador()
        {
            ViewState["PrestadorEdicion"] = 0;
            TxtPrestador.Text = "";
            TxtDireccion.Text = "";
            TxtTelefono.Text = "";
            TxtEmail.Text = "";
            ChkPrestador.Checked = true;
        }
        protected void NuevoPrestador(object sender, EventArgs e)
        {

            if (Elegido(DdlCiudad) <= 0)
            {
                LblMensaje.Text = "Seleccione una ciudad antes de crear un prestador.";
                return;
            }

            LblMensaje.Text = "";
            LimpiarPrestador();

            if (DdlPrestador.Items.Count > 0)
                DdlPrestador.SelectedIndex = 0;

            NuevaTarifa(null, null);
            TxtBuscarTarifa.Text = "";
            GrdTarifas.PageIndex = 0;
            Tarifas();
        }
        private DataRow Buscar(string tabla, int codigo)
        {
            DataTable t = ViewState[tabla] as DataTable; if (t != null) foreach (DataRow r in t.Rows)
                    if (Convert.ToInt32(r["Codigo"]) == codigo) return r; throw new InvalidOperationException("Registro no disponible. Actualice el listado.");
        }
        protected void EditarPrestador(object sender, EventArgs e)
        {
            try
            {
                DataRow r = Buscar("Prestadores", Elegido(DdlPrestador));
                ViewState["PrestadorEdicion"] = Elegido(DdlPrestador);
                TxtPrestador.Text = Convert.ToString(r["Nombre"]);
                TxtDireccion.Text = Convert.ToString(r["Direccion"]);
                TxtTelefono.Text = Convert.ToString(r["Telefono"]);
                TxtEmail.Text = Convert.ToString(r["Email"]);
                ChkPrestador.Checked = Convert.ToBoolean(r["Activo"]);
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void GuardarPrestador(object sender, EventArgs e)
        {
            try
            {
                if (Elegido(DdlCiudad) <= 0) throw new InvalidOperationException("Seleccione ciudad.");
                if (TxtPrestador.Text.Trim() == "") throw new InvalidOperationException("Ingrese el nombre del prestador.");
                if (TxtEmail.Text.Trim() != "")
                {
                    var correo = new System.Net.Mail.MailAddress(TxtEmail.Text.Trim());
                    if (correo.Address != TxtEmail.Text.Trim()) throw new InvalidOperationException("Ingrese una sola dirección de correo.");
                }
                int anterior = Elegido(DdlPrestador);
                int id = Id("PrestadorEdicion");
                if (id > 0 && id != anterior) throw new InvalidOperationException("Vuelva a seleccionar el prestador que está editando.");
                DataTable r = Ejecutar("GUARDAR_PRESTADOR", TxtPrestador.Text.Trim(), activo: ChkPrestador.Checked, prestadorEdicion: id);
                int nuevo = Convert.ToInt32(r.Rows[0]["Prestador"]);
                Prestadores(nuevo);
                LimpiarPrestador();
                NuevaTarifa(null, null);
                Tarifas();
                LblMensaje.Text = "Prestador guardado.";
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void NuevoExamen(object sender, EventArgs e)
        {
            ViewState["ExamenEdicion"] = 0;
            TxtExamen.Text = "";
            TxtDetalle.Text = "";
            ChkExamen.Checked = true;
        }
        protected void GuardarExamen(object sender, EventArgs e)
        {
            try
            {
                int previo = Id("Examen"); ViewState["Examen"] = Id("ExamenEdicion");
                try
                {
                    Ejecutar("GUARDAR_EXAMEN", TxtExamen.Text.Trim(), TxtDetalle.Text.Trim(), activo: ChkExamen.Checked);
                }
                finally
                {
                    ViewState["Examen"] = previo;
                }
                NuevoExamen(null, null); CargarExamenes(); Tarifas(); LblMensaje.Text = "Examen guardado en el catálogo general.";
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void NuevaTarifa(object sender, EventArgs e)
        {
            PnlTarifasSub.Visible = false; ViewState["TarifaSubPadre"] = 0;
            PnlPrecioPrincipal.Visible = true; LblPrecioGrupo.Visible = false;
            ViewState["Relacion"] = 0;
            ViewState["Examen"] = 0;
            DdlExamen.Enabled = true;
            if (DdlExamen.Items.Count > 0) DdlExamen.SelectedIndex = 0;
            TxtPvp.Text = "";
            TxtRed.Text = "";
            TxtNota.Text = "";
            ChkTarifa.Checked = true;
        }
        private decimal Importe(string texto, string nombre)
        {
            decimal valor;
            texto = texto.Trim();
            if (texto.Contains(",") && texto.Contains(".")) throw new InvalidOperationException("No use separadores de miles en " + nombre);
            texto = texto.Replace(',', '.');
            if (!decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor) || valor < 0 || valor > 9999999999999999.99m || decimal.Round(valor, 2) != valor)
                throw new InvalidOperationException("Ingrese " + nombre + " no negativo con hasta dos decimales."); return valor;
        }
        protected void GuardarTarifa(object sender, EventArgs e)
        {
            try
            {
                if (Elegido(DdlCiudad) <= 0 || Elegido(DdlPrestador) <= 0) throw new InvalidOperationException("Seleccione ciudad y prestador.");
                ViewState["Examen"] = Elegido(DdlExamen);
                bool grupo = EsGrupo(Elegido(DdlExamen));
                DataTable respuesta = Ejecutar("GUARDAR_TARIFA", detalle: TxtNota.Text.Trim(),
                    pvp: grupo ? null : (object)Importe(TxtPvp.Text, "PVP"),
                    red: grupo ? null : (object)Importe(TxtRed.Text, "RED"), activo: ChkTarifa.Checked);
                int examenGuardado = Elegido(DdlExamen);
                int relacionGuardada = Convert.ToInt32(respuesta.Rows[0]["Relacion"]);
                NuevaTarifa(null, null); Tarifas();
                if (grupo) AbrirTarifasSub(examenGuardado, relacionGuardada);
                LblMensaje.Text = grupo ? "Asociación guardada. Configure los precios de cada subcategoría." : "Tarifa guardada.";
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void TarifaComando(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Subtarifas")
            {
                try
                {
                    DataRow r = Buscar("Tarifas", Convert.ToInt32(e.CommandArgument));
                    AbrirTarifasSub(Convert.ToInt32(r["ExamenCodigo"]), Convert.ToInt32(r["Codigo"]));
                }
                catch (Exception ex) { Mostrar(ex); }
                return;
            }
            if (e.CommandName == "Historial")
            {
                try
                {
                    int codigo = Convert.ToInt32(e.CommandArgument);
                    DataRow tarifa = Buscar("Tarifas", codigo);
                    ViewState["HistorialRelacion"] = codigo; ViewState["HistorialTipo"] = "PRINCIPAL";
                    LblHistorial.Text = System.Web.HttpUtility.HtmlEncode(DdlCiudad.SelectedItem.Text + " / " + DdlPrestador.SelectedItem.Text + " / " + Convert.ToString(tarifa["Examen"]));
                    GrdHistorial.PageIndex = 0; CargarHistorial(); PnlHistorial.Visible = true;
                }
                catch (Exception ex)
                {
                    Mostrar(ex);
                }
                return;
            }
            if (e.CommandName != "Elegir" && e.CommandName != "Baja") return;
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataRow r = Buscar("Tarifas", id);
                ViewState["Relacion"] = id;
                ViewState["Examen"] = Convert.ToInt32(r["ExamenCodigo"]);
                if (e.CommandName == "Baja")
                {
                    Ejecutar("DESACTIVAR_TARIFA");
                    NuevaTarifa(null, null);
                    Tarifas();
                    LblMensaje.Text = "Asociación desactivada.";
                    return;
                }
                CargarEditorTarifa(r);
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void ExamenComando(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detalles")
            {
                try { AbrirDetalles(Convert.ToInt32(e.CommandArgument)); } catch (Exception ex) { Mostrar(ex); }
                return;
            }
            if (e.CommandName != "Elegir" && e.CommandName != "Baja") return;
            try
            {
                int id = Convert.ToInt32(e.CommandArgument); DataRow r = Buscar("Catalogo", id);
                if (e.CommandName == "Baja")
                {
                    int previo = Id("Examen");
                    ViewState["Examen"] = id;
                    try
                    {
                        Ejecutar("DESACTIVAR_EXAMEN");
                    }
                    finally
                    {
                        ViewState["Examen"] = previo;
                    }
                    CargarExamenes();
                    Tarifas();
                    LblMensaje.Text = "Examen desactivado en todos los prestadores.";
                    return;
                }
                ViewState["ExamenEdicion"] = id;
                TxtExamen.Text = Convert.ToString(r["Nombre"]);
                TxtDetalle.Text = Convert.ToString(r["Detalle"]);
                ChkExamen.Checked = Convert.ToBoolean(r["Activo"]);
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void GrdTarifas_Pagina(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdTarifas.PageIndex = e.NewPageIndex;
                Tarifas();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        protected void GrdExamenes_Pagina(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdExamenes.PageIndex = e.NewPageIndex;
                CargarExamenes();
            }
            catch (Exception ex)
            {
                Mostrar(ex);
            }
        }
        private DataTable EjecutarSub(string accion, int examen = 0, int subexamen = 0, int relacion = 0,
            string nombre = "", string informacion = "", int orden = 1, bool seleccionable = true,
            bool activo = true, object pvp = null, object red = null, string nota = "")
        {
            int usuario;
            if (!int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) || usuario <= 0)
                throw new InvalidOperationException("La sesión expiró. Ingrese nuevamente.");
            object[] args = { accion, examen, subexamen, relacion, nombre, informacion, orden, seleccionable,
                activo, pvp, red, usuario, Convert.ToString(Session["MachineName"]), Elegido(DdlCiudad), Elegido(DdlPrestador), nota };
            DataSet ds = new Conexion(2, "").FunMantenimientoSubexamenes(args);
            if (ds == null || ds.Tables.Count == 0) throw new InvalidOperationException("El mantenimiento de subcategorías no devolvió respuesta.");
            return ds.Tables[0];
        }
        private bool EsGrupo(int examen)
        {
            if (examen <= 0) return false;
            DataTable t = Ejecutar("EXAMENES");
            foreach (DataRow r in t.Rows)
                if (Convert.ToInt32(r["Codigo"]) == examen) return Convert.ToBoolean(r["TieneSubcategorias"]);
            throw new InvalidOperationException("El examen ya no está disponible.");
        }
        private void MostrarModoPrecio(int examen)
        { bool grupo = EsGrupo(examen); PnlPrecioPrincipal.Visible = !grupo; LblPrecioGrupo.Visible = grupo; }
        private void AbrirDetalles(int examen)
        {
            DataRow r = Buscar("Catalogo", examen); ViewState["DetalleExamenPadre"] = examen;
            LblExamenSub.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(r["Nombre"]));
            NuevoDetalleSub(null, null); CargarDetallesSub(); PnlDetallesSub.Visible = true; MostrarSeccion(PnlDetallesSub);
        }
        private void CargarDetallesSub()
        {
            DataTable t = EjecutarSub("DETALLES", examen: Id("DetalleExamenPadre"));
            ViewState["DetallesSub"] = t; GrdDetallesSub.DataSource = t; GrdDetallesSub.DataBind();
        }
        protected void NuevoDetalleSub(object sender, EventArgs e)
        { ViewState["DetalleEdicion"] = 0; TxtSubNombre.Text = ""; TxtSubInformacion.Text = ""; TxtSubOrden.Text = "1"; ChkSubSeleccionable.Checked = true; ChkSubActivo.Checked = true; }
        protected void GuardarDetalleSub(object sender, EventArgs e)
        {
            try
            {
                int orden; if (!int.TryParse(TxtSubOrden.Text, out orden) || orden < 0)
                    throw new InvalidOperationException("Ingrese un orden no negativo.");
                EjecutarSub("GUARDAR_DETALLE", examen: Id("DetalleExamenPadre"), subexamen: Id("DetalleEdicion"),
                    nombre: TxtSubNombre.Text.Trim(), informacion: TxtSubInformacion.Text.Trim(), orden: orden,
                    seleccionable: ChkSubSeleccionable.Checked, activo: ChkSubActivo.Checked);
                NuevoDetalleSub(null, null); CargarDetallesSub(); CargarExamenes();
                NuevaTarifa(null, null); Tarifas();
                LblMensaje.Text = "Detalle guardado. Si es seleccionable, configure su tarifa en cada prestador.";
            }
            catch (Exception ex) { Mostrar(ex); }
        }
        protected void DetalleSubComando(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ElegirSub" && e.CommandName != "BajaSub") return;
            try
            {
                int codigo = Convert.ToInt32(e.CommandArgument); DataRow r = Buscar("DetallesSub", codigo);
                if (e.CommandName == "BajaSub")
                {
                    EjecutarSub("DESACTIVAR_DETALLE", examen: Id("DetalleExamenPadre"), subexamen: codigo);
                    NuevoDetalleSub(null, null); CargarDetallesSub(); CargarExamenes(); NuevaTarifa(null, null); Tarifas();
                    LblMensaje.Text = "Detalle desactivado en el catálogo general."; return;
                }
                ViewState["DetalleEdicion"] = codigo; TxtSubNombre.Text = Convert.ToString(r["Nombre"]);
                TxtSubInformacion.Text = Convert.ToString(r["Informacion"]); TxtSubOrden.Text = Convert.ToString(r["Orden"]);
                ChkSubSeleccionable.Checked = Convert.ToBoolean(r["Seleccionable"]); ChkSubActivo.Checked = Convert.ToBoolean(r["Activo"]);
            }
            catch (Exception ex) { Mostrar(ex); }
        }
        protected void CerrarDetallesSub(object sender, EventArgs e) { PnlDetallesSub.Visible = false; }
        private void AbrirTarifasSub(int examen, int relacion)
        {
            CerrarHistorial(null, null);
            ViewState["TarifaSubExamen"] = examen; ViewState["TarifaSubPadre"] = relacion;
            DataRow principal = Buscar("Tarifas", relacion);
            LblTarifaSub.Text = System.Web.HttpUtility.HtmlEncode(DdlCiudad.SelectedItem.Text + " / " + DdlPrestador.SelectedItem.Text + " / " + Convert.ToString(principal["Examen"]));
            CargarTarifasSub(); LimpiarEditorSub(); PnlTarifasSub.Visible = true; MostrarSeccion(PnlTarifasSub);
        }
        private void CargarTarifasSub()
        {
            DataTable t = EjecutarSub("TARIFAS", relacion: Id("TarifaSubPadre"));
            ViewState["TarifasSub"] = t; GrdTarifasSub.DataSource = t; GrdTarifasSub.DataBind();
            DdlTarifaSub.Items.Clear(); DdlTarifaSub.Items.Add(new ListItem("Seleccione subcategoría", "0"));
            foreach (DataRow r in t.Rows) DdlTarifaSub.Items.Add(new ListItem(Convert.ToString(r["Nombre"])
                + (Convert.ToBoolean(r["ExamenActivo"]) ? "" : " [INACTIVO EN CATÁLOGO]"), Convert.ToString(r["Codigo"])));
        }
        private void LimpiarEditorSub()
        { DdlTarifaSub.SelectedIndex = 0; TxtSubPvp.Text = ""; TxtSubRed.Text = ""; TxtSubNota.Text = ""; ChkSubTarifa.Checked = true; }
        private void ElegirTarifaSub(int codigo)
        {
            DataRow r = Buscar("TarifasSub", codigo); DdlTarifaSub.SelectedValue = codigo.ToString();
            TxtSubPvp.Text = r["PVP"] == DBNull.Value ? "" : Convert.ToDecimal(r["PVP"]).ToString("0.00", CultureInfo.InvariantCulture);
            TxtSubRed.Text = r["RED"] == DBNull.Value ? "" : Convert.ToDecimal(r["RED"]).ToString("0.00", CultureInfo.InvariantCulture);
            TxtSubNota.Text = Convert.ToString(r["Nota"]);
            ChkSubTarifa.Checked = Convert.ToInt32(r["Relacion"]) == 0 || Convert.ToBoolean(r["Activo"]);
        }
        protected void TarifaSubCambio(object sender, EventArgs e)
        {
            try { int codigo = Elegido(DdlTarifaSub); if (codigo == 0) LimpiarEditorSub(); else ElegirTarifaSub(codigo); }
            catch (Exception ex) { Mostrar(ex); }
        }
        protected void GuardarTarifaSub(object sender, EventArgs e)
        {
            try
            {
                if (Elegido(DdlTarifaSub) <= 0) throw new InvalidOperationException("Seleccione subcategoría.");
                EjecutarSub("GUARDAR_TARIFA", examen: Id("TarifaSubExamen"), subexamen: Elegido(DdlTarifaSub),
                    relacion: Id("TarifaSubPadre"), pvp: Importe(TxtSubPvp.Text, "PVP"), red: Importe(TxtSubRed.Text, "RED"),
                    activo: ChkSubTarifa.Checked, nota: TxtSubNota.Text.Trim());
                CargarTarifasSub(); LimpiarEditorSub(); CerrarHistorial(null, null);
                LblMensaje.Text = "Tarifa de subcategoría guardada.";
            }
            catch (Exception ex) { Mostrar(ex); }
        }
        protected void TarifaSubComando(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ElegirSub" && e.CommandName != "BajaSub" && e.CommandName != "HistorialSub") return;
            try
            {
                int codigo = Convert.ToInt32(e.CommandArgument); DataRow r = Buscar("TarifasSub", codigo);
                if (e.CommandName == "ElegirSub") { ElegirTarifaSub(codigo); return; }
                if (e.CommandName == "BajaSub")
                {
                    EjecutarSub("DESACTIVAR_TARIFA", examen: Id("TarifaSubExamen"), subexamen: codigo, relacion: Id("TarifaSubPadre"));
                    CargarTarifasSub(); LimpiarEditorSub(); CerrarHistorial(null, null);
                    LblMensaje.Text = "Tarifa de subcategoría desactivada en este prestador."; return;
                }
                ViewState["HistorialTipo"] = "SUB"; ViewState["HistorialRelacion"] = Convert.ToInt32(r["Relacion"]);
                LblHistorial.Text = LblTarifaSub.Text + " / " + System.Web.HttpUtility.HtmlEncode(Convert.ToString(r["Nombre"]));
                GrdHistorial.PageIndex = 0; CargarHistorial(); PnlHistorial.Visible = true; MostrarSeccion(PnlHistorial);
            }
            catch (Exception ex) { Mostrar(ex); }
        }
        protected void CerrarTarifasSub(object sender, EventArgs e) { PnlTarifasSub.Visible = false; }
        private void MostrarSeccion(Control control)
        {
            string id = System.Web.HttpUtility.JavaScriptStringEncode(control.ClientID);
            ScriptManager.RegisterStartupScript(this, GetType(), "SeccionSubcategorias",
                "var seccion=document.getElementById('" + id + "');if(seccion){seccion.scrollIntoView(true);}", true);
        }

    }
}
