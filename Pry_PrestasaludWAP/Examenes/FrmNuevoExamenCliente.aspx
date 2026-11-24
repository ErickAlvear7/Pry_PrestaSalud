<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoExamenCliente.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmNuevoExamenCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Gestión de Exámenes</title>

    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>

    <style type="text/css">
        .tabla-solicitudes {
            width: 100%;
            table-layout: auto;
        }

            .tabla-solicitudes th {
                background-color: #f5f5f5;
                font-size: 12px;
                vertical-align: middle !important;
                text-align: center;
            }

            .tabla-solicitudes td {
                font-size: 11px;
                vertical-align: middle !important;
            }

        .panel-listado {
            margin-top: 10px;
        }

        .estado-solicitado {
            font-weight: bold;
        }

        .estado-agendado {
            font-weight: bold;
        }

        .estado-pendiente {
            font-weight: bold;
        }

        .estado-aceptado {
            font-weight: bold;
        }

        .estado-rechazado {
            font-weight: bold;
        }

        .paginador td {
            padding: 4px;
        }

        .paginador a,
        .paginador span {
            display: inline-block;
            padding: 5px 9px;
            margin: 2px;
            border: 1px solid #ddd;
            border-radius: 4px;
            text-decoration: none;
        }

        .paginador span {
            font-weight: bold;
            background-color: #eeeeee;
        }

        .filtros-cliente {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: flex-end;
            margin-bottom: 12px;
        }

            .filtros-cliente > div {
                flex: 1 1 180px;
            }

            .filtros-cliente label {
                display: block;
            }

        .nota-cliente {
            color: #627181;
            font-size: 12px;
        }

        .resumen-cliente {
            display: flex;
            gap: 20px;
            margin: 12px 0;
            flex-wrap: wrap;
        }

        .paginas-cliente {
            display: flex;
            align-items: center;
            gap: 15px;
            margin-top: 15px;
        }

        .tabla-solicitudes {
            min-width: 1550px;
        }

            .tabla-solicitudes th {
                white-space: nowrap;
            }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 14px;">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="alert alert-info"
                    style="font-size: 12px; margin-bottom: 10px;">
                    <strong>Gestión de exámenes:</strong>
                </div>
                <asp:UpdatePanel ID="UpdCabecera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="panel panel-default panel-listado">
                            <div class="panel-heading">
                                <strong>SOLICITUDES / EXÁMENES
                                </strong>
                            </div>
                            <div class="panel-body" style="overflow-x: auto;">
                                <asp:Panel ID="PnlGrupoExamenes" runat="server">
                                    <asp:Panel ID="PnlFiltros" runat="server" DefaultButton="BtnConsultar">
                                        <div class="filtros-cliente">
                                            <div>
                                                <label>Solicitud desde</label><asp:TextBox ID="TxtDesde" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                            <div>
                                                <label>Solicitud hasta</label><asp:TextBox ID="TxtHasta" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                            <div>
                                                <label>Documento del titular</label><asp:TextBox ID="TxtDocumento" runat="server" MaxLength="20" CssClass="form-control" placeholder="Buscar documento" /></div>
                                            <div>
                                                <label>Estado actual</label><asp:DropDownList ID="DdlEstado" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="BtnConsultar_Click" CausesValidation="false">
                                                    <asp:ListItem Value="">Todos</asp:ListItem>
                                                    <asp:ListItem Value="SRR">Solicitud realizada</asp:ListItem>
                                                    <asp:ListItem Value="SRV">Solicitud revisada</asp:ListItem>
                                                    <asp:ListItem Value="SGA">Agendada</asp:ListItem>
                                                    <asp:ListItem Value="EXR">Exámenes realizados</asp:ListItem>
                                                    <asp:ListItem Value="AUA">Auditado aceptado</asp:ListItem>
                                                    <asp:ListItem Value="AUR">Auditado rechazado (todos)</asp:ListItem>
                                                    <asp:ListItem Value="APELABLE">Con rechazo apelable</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <asp:Button ID="BtnConsultar" runat="server" Text="Consultar" UseSubmitBehavior="false" OnClick="BtnConsultar_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                            <asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" OnClick="BtnLimpiar_Click" CssClass="btn btn-default" CausesValidation="false" />
                                        </div>
                                        <p class="nota-cliente">Deje las fechas vacías para consultar todo el historial de su campaña. Gestión se habilita al completar la auditoría del titular y los codeudores.</p>
                                        <div class="resumen-cliente">
                                            <asp:Label ID="LblTotal" runat="server" /><asp:Label ID="LblConsulta" runat="server" /></div>
                                        <div style="overflow-x: auto;">
                                            <asp:GridView ID="GrdvGrupoExamen" runat="server" AutoGenerateColumns="false"
                                                CssClass="table table-condensed table-bordered table-hover tabla-solicitudes" DataKeyNames="CodigoEXSO,EstadoCodigo"
                                                ShowHeaderWhenEmpty="true" OnRowDataBound="GrdvGrupoExamen_RowDataBound" Width="100%"
                                                EmptyDataText="No existen solicitudes para los filtros de su campaña.">
                                                <Columns>
                                                    <asp:BoundField DataField="FechaSolicitud" HeaderText="Solicitado" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField DataField="Cedula" HeaderText="Documento" HtmlEncode="true" />
                                                    <asp:BoundField DataField="Paciente" HeaderText="Titular" HtmlEncode="true" />
                                                    <asp:BoundField DataField="Producto" HeaderText="Producto" HtmlEncode="true" />
                                                    <asp:TemplateField HeaderText="Estado actual">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblEstadoFila" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Convert.ToString(Eval("Estado"))) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Agenda" HeaderText="Agendamiento" HtmlEncode="true" />
                                                    <asp:BoundField DataField="FechaAgenda" HeaderText="Registro agenda" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField DataField="Resultados" HeaderText="Resultados" HtmlEncode="true" />
                                                    <asp:BoundField DataField="FechaResultados" HeaderText="Envío resultados" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField DataField="Auditoria" HeaderText="Auditoría" HtmlEncode="true" />
                                                    <asp:BoundField DataField="ProgresoAuditoria" HeaderText="Auditados / pacientes" HtmlEncode="true" />
                                                    <asp:BoundField DataField="FechaAuditoria" HeaderText="Fin auditoría" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField DataField="GestionCliente" HeaderText="Gestión cliente" HtmlEncode="true" />
                                                    <asp:BoundField DataField="FechaSolicitudAgendamiento" HeaderText="Fecha solicitud agendamiento" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" NullDisplayText="—" />
                                                    <asp:TemplateField HeaderText="Gestión">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgGestion" runat="server" Height="22px" Enabled="false"
                                                                ImageUrl="~/Botones/selecc.png" CausesValidation="false"
                                                                CommandArgument='<%# Eval("CodigoEXSO") %>' OnCommand="ImgGestion_Command" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="paginas-cliente">
                                            <asp:Button ID="BtnAnterior" runat="server" Text="Anterior" OnClick="BtnAnterior_Click" CausesValidation="false" CssClass="btn btn-default" />
                                            <asp:Label ID="LblPagina" runat="server" />
                                            <asp:Button ID="BtnSiguiente" runat="server" Text="Siguiente" OnClick="BtnSiguiente_Click" CausesValidation="false" CssClass="btn btn-default" />
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnConsultar" />
                    </Triggers>
                </asp:UpdatePanel>
                <%--<div class="panel panel-default">
                    <div class="panel-body" style="text-align: center;">
                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" Text="Salir" Width="120px" CausesValidation="false" OnClick="BtnSalir_Click" />
                    </div>
                </div>--%>
            </div>
        </div>
    </form>
</body>

</html>
