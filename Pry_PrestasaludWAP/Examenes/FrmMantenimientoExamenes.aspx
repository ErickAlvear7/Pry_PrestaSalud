<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMantenimientoExamenes.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmMantenimientoExamenes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mantenimiento de exámenes</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            padding: 20px;
            background: #f5f7fa;
            color: #243746;
            font-family: Arial
        }

        h1 {
            font-size: 24px
        }

        fieldset {
            background: white;
            border: 1px solid #d5dde5;
            border-radius: 5px;
            padding: 15px;
            margin: 18px 0
        }

        legend {
            font-size: 18px;
            width: auto;
            border: 0;
            padding: 0 8px;
            margin: 0
        }

        .fila {
            display: flex;
            gap: 12px;
            align-items: flex-end;
            flex-wrap: wrap;
            margin: 12px 0
        }

        .campo {
            flex: 1 1 180px;
            min-width: 150px
        }

        .campo label {
            display: block
        }

        .tabla {
            overflow: auto
        }

        .tabla th {
            white-space: nowrap
        }

        .tabla td {
            font-size: 12px;
            vertical-align: top !important
        }

        .tabla a {
            margin-right: 12px
        }

        .nota {
            color: #63717b;
            font-size: 12px
        }

        .mensaje {
            display: block;
            color: #9c2020;
            white-space: pre-wrap;
            margin: 8px 0
        }

        .buscador {
           position: relative;
        }

        .buscador .glyphicon-search {
            position: absolute;
            left: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #63717b;
            pointer-events: none;
        }

        .buscador .form-control {
            padding-left: 35px;
        }
        .check-alineado {
            display: inline-flex !important;
            align-items: center;
            gap: 6px;
            height: 34px;
            white-space: nowrap;
        }

        .check-alineado input[type="checkbox"],
        .check-alineado label {
            margin: 0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SmBusqueda" runat="server" />
        <asp:UpdatePanel ID="UpdContenido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="panel panel-primary">
                    <fieldset>
                        <asp:Label ID="LblMensaje" runat="server" CssClass="mensaje" role="status"></asp:Label><h1>Exámenes y tarifas por prestador</h1>
                        <p>Seleccione ciudad y prestador. El catálogo de nombres de exámenes es compartido; los precios corresponden a cada asociación.</p>
                        <div class="fila">
                            <div class="campo">
                                <label>Ciudad</label><asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="CiudadCambio"></asp:DropDownList>
                            </div>
                            <div class="campo">
                                <label>Prestador</label><asp:DropDownList ID="DdlPrestador" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="PrestadorCambio"></asp:DropDownList>
                            </div>
                            <asp:Button ID="BtnNuevoPrestador" runat="server" Text="Nuevo prestador" OnClick="NuevoPrestador" CssClass="btn btn-primary" CausesValidation="false"></asp:Button><asp:Button ID="BtnEditarPrestador" runat="server" Text="Editar prestador" OnClick="EditarPrestador" CssClass="btn btn-primary" CausesValidation="false"></asp:Button>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Datos del prestador</legend>
                        <div class="fila">
                            <div class="campo">
                                <label>Nombre</label><asp:TextBox ID="TxtPrestador" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="campo">
                                <label>Dirección</label><asp:TextBox ID="TxtDireccion" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="campo">
                                <label>Teléfono</label><asp:TextBox ID="TxtTelefono" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="campo">
                                <label>Correo de agendamiento</label><asp:TextBox ID="TxtEmail" runat="server" MaxLength="80" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:CheckBox ID="ChkPrestador" runat="server" Text="Activo" Checked="true" CssClass="check-alineado"></asp:CheckBox><asp:Button ID="BtnGuardarPrestador" runat="server" Text="Guardar prestador" OnClick="GuardarPrestador" CssClass="btn btn-primary" CausesValidation="false"></asp:Button>
                        </div>
                        <p class="nota">El correo se habilita para notificaciones del prestador. No se crea automáticamente su relación LABORATORIO ni sus horarios de agenda.</p>
                    </fieldset>
                </div>
                <div class="panel panel-primary">
                    <fieldset>
                        <legend>Tarifas del prestador seleccionado</legend>
                        <div class="fila">
                            <div class="campo">
                                <label>Examen</label><asp:DropDownList ID="DdlExamen" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ExamenTarifaCambio"></asp:DropDownList>
                            </div>
                            <asp:Panel ID="PnlPrecioPrincipal" runat="server" Style="display:contents;"><div class="campo">
                                <label>PVP</label><asp:TextBox ID="TxtPvp" runat="server" MaxLength="20" CssClass="form-control" inputmode="decimal" oninput="filtrarImporte(this);"></asp:TextBox>
                            </div>
                            <div class="campo">
                                <label>RED</label><asp:TextBox ID="TxtRed" runat="server" MaxLength="20" CssClass="form-control" inputmode="decimal" oninput="filtrarImporte(this);"></asp:TextBox>
                            </div></asp:Panel>
                            <asp:Label ID="LblPrecioGrupo" runat="server" Visible="false" CssClass="nota" Text="Este examen tiene subcategorías: configure sus precios por separado. El precio del principal no se suma." />
                            <div class="campo">
                                <label>Nota de tarifa</label><asp:TextBox ID="TxtNota" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:CheckBox ID="ChkTarifa" runat="server" Text="Activo" Checked="true" CssClass="check-alineado"></asp:CheckBox><asp:Button ID="BtnGuardarTarifa" runat="server" Text="Guardar tarifa" OnClick="GuardarTarifa" CssClass="btn btn-primary" CausesValidation="false"></asp:Button><asp:Button ID="BtnNuevaTarifa" runat="server" Text="Limpiar selección" OnClick="NuevaTarifa" CssClass="btn btn-primary" CausesValidation="false"></asp:Button>
                        </div>
                        <div class="fila">
                            <div class="campo">
                                <label for="TxtBuscarTarifa">Buscar valores por examen</label>
                                <div class="buscador">
                                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                                    <asp:TextBox ID="TxtBuscarTarifa" runat="server" MaxLength="500" CssClass="form-control" placeholder="Buscar examen prestador" />
                                </div>
                            </div>
                            <asp:Button ID="BtnBuscarTarifa" runat="server" Text="Buscar" OnClick="BuscarTarifa" CssClass="btn btn-primary" CausesValidation="false" style="display:none;"/>
                            <asp:Button ID="BtnLimpiarTarifa" runat="server" Text="Mostrar todos" OnClick="LimpiarBusquedaTarifa" CssClass="btn btn-primary" CausesValidation="false" />
                        </div>
                        <div class="tabla">
                            <asp:GridView ID="GrdTarifas" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" GridLines="None" AllowPaging="true" PageSize="15" OnPageIndexChanging="GrdTarifas_Pagina" OnRowCommand="TarifaComando" EmptyDataText="Sin registros.">
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HtmlEncode="true" Visible="false" />
                                    <asp:BoundField DataField="Examen" HeaderText="Examen" HtmlEncode="true" />
                                    <asp:TemplateField HeaderText="PVP"><ItemTemplate><%# Convert.ToBoolean(Eval("TieneSubcategorias")) ? "Por subcategoría" : System.Web.HttpUtility.HtmlEncode(Convert.ToString(Eval("PVP"))) %></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderText="RED"><ItemTemplate><%# Convert.ToBoolean(Eval("TieneSubcategorias")) ? "Por subcategoría" : System.Web.HttpUtility.HtmlEncode(Convert.ToString(Eval("RED"))) %></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Activo">
                                        <HeaderStyle CssClass="text-center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <span style='<%# Convert.ToBoolean(Eval("Activo"))
                                                ? "color:green;font-size:18px;": "color:red;font-size:18px;" %>'>
                                                <%# Convert.ToBoolean(Eval("Activo")) ? "✓" : "✗" %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ExamenActivo" HeaderText="Examen activo" HtmlEncode="true" Visible="false" />
                                    <asp:BoundField DataField="Nota" HeaderText="Nota" HtmlEncode="true" />
                                    <asp:TemplateField HeaderText="Acción">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Tarifas subcategorías" CommandName="Subtarifas" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" Visible='<%# Convert.ToBoolean(Eval("TieneSubcategorias")) %>' />
                                            <asp:LinkButton runat="server" Text="Historial" CommandName="Historial" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                            <asp:LinkButton runat="server" Text="Editar" CommandName="Elegir" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                            <asp:LinkButton runat="server" Text="Desactivar" CommandName="Baja" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" OnClientClick="return confirm('¿Desactivar este registro?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <asp:Panel ID="PnlHistorial" runat="server" Visible="false">
                    <fieldset>
                        <legend>Historial de precios</legend>
                        <asp:Label ID="LblHistorial" runat="server" />
                        <p class="nota">Cambios registrados desde la instalación. Guardar sin cambiar precios no genera historial. Se muestra el login del usuario que realizó el cambio.</p>
                        <div class="tabla">
                            <asp:GridView ID="GrdHistorial" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" AllowPaging="true" PageSize="15" OnPageIndexChanging="HistorialPagina" EmptyDataText="Esta tarifa todavía no tiene cambios de precios registrados.">
                                <Columns>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" HtmlEncode="true" />
                                    <asp:BoundField DataField="PvpAnterior" HeaderText="PVP anterior" DataFormatString="{0:N2}" NullDisplayText="Sin valor" />
                                    <asp:BoundField DataField="PvpNuevo" HeaderText="PVP nuevo" DataFormatString="{0:N2}" NullDisplayText="Sin valor" />
                                    <asp:BoundField DataField="RedAnterior" HeaderText="RED anterior" DataFormatString="{0:N2}" NullDisplayText="Sin valor" />
                                    <asp:BoundField DataField="RedNuevo" HeaderText="RED nuevo" DataFormatString="{0:N2}" NullDisplayText="Sin valor" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="BtnCerrarHistorial" runat="server" Text="Cerrar historial" OnClick="CerrarHistorial" CausesValidation="false" CssClass="btn btn-default" />
                    </fieldset>
                </asp:Panel>
                <div class="panel panel-primary">
                    <fieldset>
                        <legend>Catálogo general de exámenes</legend>
                        <p class="nota">Editar el nombre o desactivar aquí afecta a todos los prestadores que utilizan ese examen. Para retirarlo de un solo prestador, desactive su tarifa en la sección anterior.</p>
                        <div class="fila">
                            <div class="campo">
                                <label>Nombre del examen</label><asp:TextBox ID="TxtExamen" runat="server" MaxLength="500" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="campo">
                                <label>Detalle informativo general</label><asp:TextBox ID="TxtDetalle" runat="server" MaxLength="1000" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:CheckBox ID="ChkExamen" runat="server" Text="Activo" Checked="true" CssClass="check-alineado"></asp:CheckBox><asp:Button ID="BtnGuardarExamen" runat="server" Text="Guardar examen" OnClick="GuardarExamen" CssClass="btn btn-primary" CausesValidation="false"></asp:Button><asp:Button ID="BtnNuevoExamen" runat="server" Text="Nuevo examen" OnClick="NuevoExamen" CssClass="btn btn-primary" CausesValidation="false"></asp:Button>
                        </div>
                        <div class="fila">
                            <div class="campo">
                                <label for="TxtBuscarExamen">Buscar por nombre</label>
                                <div class="buscador">
                                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                                   <asp:TextBox ID="TxtBuscarExamen" runat="server" MaxLength="500" CssClass="form-control" placeholder="Buscar examen catalogo"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Button ID="BtnBuscarExamen" runat="server" Text="Buscar" OnClick="BuscarExamen" CssClass="btn btn-primary" CausesValidation="false" style="display:none;"></asp:Button>
                            <asp:Button ID="BtnLimpiarBusqueda" runat="server" Text="Mostrar todos" OnClick="LimpiarBusqueda" CssClass="btn btn-primary" CausesValidation="false"></asp:Button>
                        </div>
                        <div class="tabla">
                            <asp:GridView ID="GrdExamenes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" GridLines="None" AllowPaging="true" PageSize="15" OnPageIndexChanging="GrdExamenes_Pagina" OnRowCommand="ExamenComando" EmptyDataText="Sin registros.">
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HtmlEncode="true" Visible="false" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" HtmlEncode="true" />
                                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" HtmlEncode="true" />
                                    <asp:TemplateField HeaderText="Activo en catálogo">
                                        <ItemTemplate>
                                            <span style='<%# Convert.ToBoolean(Eval("Activo"))
                                                ? "color:green;font-size:18px;": "color:red;font-size:18px;" %>'>
                                                <%# Convert.ToBoolean(Eval("Activo")) ? "✓" : "✗" %>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acción">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Detalles / Subcategorías" CommandName="Detalles" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                            <asp:LinkButton runat="server" Text="Editar" CommandName="Elegir" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                            <asp:LinkButton runat="server" Text="Desactivar" CommandName="Baja" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" OnClientClick="return confirm('¿Desactivar este registro?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <asp:Panel ID="PnlDetallesSub" runat="server" Visible="false" CssClass="panel panel-primary">
                    <fieldset>
                        <legend>Detalles / Subcategorías del catálogo</legend>
                        <asp:Label ID="LblExamenSub" runat="server" Font-Bold="true" />
                        <p class="nota">Esta configuración es general para todos los prestadores. Marque «Seleccionable» para mostrar una casilla y asignar un precio independiente. Sin esa marca, se muestra solo como información. Si hay hijos seleccionables activos, el principal deja de ser seleccionable.</p>
                        <div class="fila">
                            <div class="campo"><label>Nombre</label><asp:TextBox ID="TxtSubNombre" runat="server" MaxLength="300" CssClass="form-control" /></div>
                            <div class="campo"><label>Detalle opcional</label><asp:TextBox ID="TxtSubInformacion" runat="server" MaxLength="1000" CssClass="form-control" /></div>
                            <div class="campo"><label>Orden</label><asp:TextBox ID="TxtSubOrden" runat="server" Text="1" MaxLength="9" inputmode="numeric" CssClass="form-control" /></div>
                            <asp:CheckBox ID="ChkSubSeleccionable" runat="server" Text="Seleccionable" Checked="true" CssClass="check-alineado" />
                            <asp:CheckBox ID="ChkSubActivo" runat="server" Text="Activo en catálogo" Checked="true" CssClass="check-alineado" />
                            <asp:Button ID="BtnGuardarSub" runat="server" Text="Guardar detalle" OnClick="GuardarDetalleSub" CausesValidation="false" CssClass="btn btn-primary" />
                            <asp:Button ID="BtnNuevoSub" runat="server" Text="Nuevo detalle" OnClick="NuevoDetalleSub" CausesValidation="false" CssClass="btn btn-default" />
                        </div>
                        <div class="tabla">
                            <asp:GridView ID="GrdDetallesSub" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" OnRowCommand="DetalleSubComando" EmptyDataText="Este examen no tiene detalles ni subcategorías.">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Informacion" HeaderText="Detalle" />
                                    <asp:BoundField DataField="Orden" HeaderText="Orden" />
                                    <asp:TemplateField HeaderText="Seleccionable"><ItemTemplate><%# Convert.ToBoolean(Eval("Seleccionable")) ? "Sí" : "Solo información" %></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Activo"><ItemTemplate><%# Convert.ToBoolean(Eval("Activo")) ? "✓" : "✗" %></ItemTemplate><ItemStyle HorizontalAlign="Center" /><HeaderStyle CssClass="text-center" /></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acción"><ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Editar" CommandName="ElegirSub" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                        <asp:LinkButton runat="server" Text="Desactivar" CommandName="BajaSub" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" OnClientClick="return confirm('¿Desactivar este detalle en todos los prestadores? Si no quedan subcategorías activas, el principal vuelve a funcionar como examen individual.');" />
                                    </ItemTemplate></asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="BtnCerrarDetallesSub" runat="server" Text="Cerrar detalles" OnClick="CerrarDetallesSub" CausesValidation="false" CssClass="btn btn-default" />
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="PnlTarifasSub" runat="server" Visible="false" CssClass="panel panel-primary">
                    <fieldset>
                        <legend>Precios de subcategorías por prestador</legend>
                        <asp:Label ID="LblTarifaSub" runat="server" Font-Bold="true" />
                        <p class="nota">Estos valores solo corresponden a la ciudad y prestador indicados. El modal suma cada subcategoría elegida, sin sumar el principal. Las subcategorías sin tarifa aparecen sin precio y deben configurarse antes de agendarlas.</p>
                        <div class="fila">
                            <div class="campo"><label>Subcategoría</label><asp:DropDownList ID="DdlTarifaSub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="TarifaSubCambio" CssClass="form-control" /></div>
                            <div class="campo"><label>PVP</label><asp:TextBox ID="TxtSubPvp" runat="server" MaxLength="20" inputmode="decimal" oninput="filtrarImporte(this);" CssClass="form-control" /></div>
                            <div class="campo"><label>RED</label><asp:TextBox ID="TxtSubRed" runat="server" MaxLength="20" inputmode="decimal" oninput="filtrarImporte(this);" CssClass="form-control" /></div>
                            <div class="campo"><label>Nota de tarifa</label><asp:TextBox ID="TxtSubNota" runat="server" MaxLength="250" CssClass="form-control" /></div>
                            <asp:CheckBox ID="ChkSubTarifa" runat="server" Text="Activa en prestador" Checked="true" CssClass="check-alineado" />
                            <asp:Button ID="BtnGuardarTarifaSub" runat="server" Text="Guardar tarifa subcategoría" OnClick="GuardarTarifaSub" CausesValidation="false" CssClass="btn btn-primary" />
                        </div>
                        <div class="tabla">
                            <asp:GridView ID="GrdTarifasSub" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" OnRowCommand="TarifaSubComando" EmptyDataText="Configure subcategorías seleccionables en el catálogo.">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Subcategoría" />
                                    <asp:BoundField DataField="PVP" HeaderText="PVP" DataFormatString="{0:N2}" NullDisplayText="Sin tarifa" />
                                    <asp:BoundField DataField="RED" HeaderText="RED" DataFormatString="{0:N2}" NullDisplayText="Sin tarifa" />
                                    <asp:TemplateField HeaderText="Tarifa activa"><ItemTemplate><%# Convert.ToInt32(Eval("Relacion")) == 0 ? "Sin configurar" : (Convert.ToBoolean(Eval("Activo")) ? "✓" : "✗") %></ItemTemplate><ItemStyle HorizontalAlign="Center" /><HeaderStyle CssClass="text-center" /></asp:TemplateField>
                                    <asp:TemplateField HeaderText="En catálogo"><ItemTemplate><%# Convert.ToBoolean(Eval("ExamenActivo")) ? "✓" : "✗" %></ItemTemplate><ItemStyle HorizontalAlign="Center" /><HeaderStyle CssClass="text-center" /></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acción"><ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Editar" CommandName="ElegirSub" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" />
                                        <asp:LinkButton runat="server" Text="Historial" CommandName="HistorialSub" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" Visible='<%# Convert.ToInt32(Eval("Relacion")) > 0 %>' />
                                        <asp:LinkButton runat="server" Text="Desactivar" CommandName="BajaSub" CommandArgument='<%# Eval("Codigo") %>' CausesValidation="false" Visible='<%# Convert.ToInt32(Eval("Relacion")) > 0 %>' OnClientClick="return confirm('¿Desactivar esta tarifa solo en este prestador?');" />
                                    </ItemTemplate></asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="BtnCerrarTarifasSub" runat="server" Text="Cerrar tarifas de subcategorías" OnClick="CerrarTarifasSub" CausesValidation="false" CssClass="btn btn-default" />
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        (function () {
            var gestor = Sys.WebForms.PageRequestManager.getInstance();
            var estados = [
                { campo: 'TxtBuscarExamen', boton: 'BtnBuscarExamen', limpiar: 'BtnLimpiarBusqueda', destino: '<%= BtnBuscarExamen.UniqueID %>' },
                { campo: 'TxtBuscarTarifa', boton: 'BtnBuscarTarifa', limpiar: 'BtnLimpiarTarifa', destino: '<%= BtnBuscarTarifa.UniqueID %>' }
            ];
            var enCurso = null;
            function cancelar(s) { clearTimeout(s.timer); s.pendiente = false; }
            function programar(s) { clearTimeout(s.timer); s.timer = setTimeout(function () { enviar(s); }, 450); }
            function enviar(s) {
                if (!s.pendiente) return;
                if (gestor.get_isInAsyncPostBack()) { programar(s); return; }
                var campo = document.getElementById(s.campo);
                if (!campo) return;
                s.enviado = s.texto; campo.value = s.texto; enCurso = s;
                __doPostBack(s.destino, '');
            }
            function preparar() {
                estados.forEach(function (s) {
                    var campo = document.getElementById(s.campo);
                    if (!campo) return;
                    campo.oninput = function () {
                        s.texto = campo.value; s.cursor = campo.selectionStart;
                        s.pendiente = true; programar(s);
                    };
                    campo.onkeydown = function (e) {
                        if (e.key === 'Enter') {
                            e.preventDefault(); s.texto = campo.value; s.cursor = campo.selectionStart;
                            s.pendiente = true; clearTimeout(s.timer); enviar(s);
                        }
                    };
                });
            }
            // Cancelar búsquedas pendientes cuando se elige otra acción de la pantalla.
            document.addEventListener('click', function (e) {
                var t = e.target;
                while (t && t !== document && !/^(A|BUTTON|INPUT|SELECT)$/.test(t.tagName)) t = t.parentNode;
                if (t && (t.tagName === 'A' || t.tagName === 'BUTTON' || t.tagName === 'SELECT' || /^(submit|button|checkbox|radio)$/i.test(t.type))) {
                    estados.forEach(cancelar);
                }
            }, true);
            gestor.add_endRequest(function (sender, e) {
                preparar();
                estados.forEach(function (s) {
                    if (!s.pendiente) return;
                    if (e.get_error()) { cancelar(s); return; }
                    var campo = document.getElementById(s.campo);
                    if (!campo) return;
                    campo.value = s.texto;
                    if (enCurso === s && (!document.activeElement || document.activeElement === document.body || document.activeElement.id === s.campo)) {
                        campo.focus();
                        if (campo.setSelectionRange && s.cursor != null) campo.setSelectionRange(s.cursor, s.cursor);
                    }
                    if (s.texto !== s.enviado) programar(s); else s.pendiente = false;
                });
                enCurso = null;
            });
            preparar();
        })();

        function filtrarImporte(campo) {
            var texto = campo.value.replace(/[^0-9.,]/g, '');
            var posicion = texto.search(/[.,]/);

            if (posicion >= 0) {
                texto =
                    texto.substring(0, posicion) +
                    texto.charAt(posicion) +
                    texto.substring(posicion + 1)
                        .replace(/[.,]/g, '')
                        .substring(0, 2);
            }

            if (campo.value !== texto) {
                campo.value = texto;
            }
        }

    </script>
</body>
</html>