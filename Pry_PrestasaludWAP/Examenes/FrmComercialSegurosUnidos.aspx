<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmComercialSegurosUnidos.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmComercialSegurosUnidos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Comercial · Seguros Unidos</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background: #f4f6f9;
            padding: 18px;
            color: #263746;
            font-family: Arial,sans-serif
        }

        .cabecera {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 12px;
            flex-wrap: wrap;
            margin-bottom: 16px
        }

        h1 {
            font-size: 23px;
            margin: 0 0 6px
        }

        .subtitulo {
            color: #627181;
            margin: 0
        }

        .contenedor {
            background: white;
            padding: 18px;
            border: 1px solid #dbe2e9;
            border-radius: 6px
        }

        .filtros {
            display: flex;
            gap: 12px;
            align-items: flex-end;
            flex-wrap: wrap;
            margin-bottom: 16px
        }

            .filtros label {
                display: block;
                font-size: 12px
            }

            .filtros input, .filtros select {
                max-width: 210px
            }

        .tabla {
            overflow-x: auto
        }

            .tabla th {
                white-space: nowrap;
                background: #edf3f8
            }

            .tabla td, .tabla th {
                padding: 9px !important;
                font-size: 12px;
                vertical-align: top !important
            }

        .resumen, .paginas {
            display: flex;
            justify-content: space-between;
            gap: 12px;
            align-items: center;
            flex-wrap: wrap;
            margin: 12px 0
        }

        .error {
            display: block;
            color: #a12622;
            margin-bottom: 12px
        }

        .pista {
            font-size: 12px;
            color: #627181
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="cabecera">
            <div>
                <h1>Seguros Unidos</h1>
                <p class="subtitulo">Consulta comercial de solicitudes y sus gestiones</p>
            </div>
            <asp:Button ID="BtnNueva" runat="server" Text="Nueva solicitud" CssClass="btn btn-primary" OnClick="BtnNueva_Click" />
        </div>
        <div class="contenedor">
            <asp:Label ID="LblError" runat="server" CssClass="error" role="alert" />
            <asp:Panel ID="PnlConsulta" runat="server" DefaultButton="BtnConsultar">
                <div class="filtros">
                    <div>
                        <label for="TxtDesde">Solicitud desde</label><asp:TextBox ID="TxtDesde" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div>
                        <label for="TxtHasta">Solicitud hasta</label><asp:TextBox ID="TxtHasta" runat="server" TextMode="Date" CssClass="form-control" /></div>
                    <div>
                        <label for="TxtDocumento">Documento del titular</label><asp:TextBox ID="TxtDocumento" runat="server" MaxLength="20" CssClass="form-control" /></div>
                    <div>
                        <label for="DdlEstado">Estado actual</label><asp:DropDownList ID="DdlEstado" runat="server" CssClass="form-control">
                            <asp:ListItem Value="">Todos</asp:ListItem>
                            <asp:ListItem Value="SRR">Solicitud realizada</asp:ListItem>
                            <asp:ListItem Value="SRV">Solicitud revisada</asp:ListItem>
                            <asp:ListItem Value="SGA">Agendada</asp:ListItem>
                            <asp:ListItem Value="EXR">Resultados enviados</asp:ListItem>
                            <asp:ListItem Value="AUA">Auditoría aceptada</asp:ListItem>
                            <asp:ListItem Value="AUR">Auditoría rechazada</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="BtnConsultar" runat="server" Text="Consultar" CssClass="btn btn-primary" OnClick="BtnConsultar_Click" />
                    <asp:Button ID="BtnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-default" OnClick="BtnLimpiar_Click" />
                </div>
                <p class="pista">Deja las fechas vacías para consultar todo el historial. Incluye solicitudes activas y canceladas, de todos los solicitantes de Seguros Unidos.</p>
                <div class="resumen">
                    <asp:Label ID="LblTotal" runat="server" /><asp:Label ID="LblConsulta" runat="server" /></div>
                <div class="tabla">
                    <asp:GridView ID="GrdvSolicitudes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" GridLines="None" EmptyDataText="No hay solicitudes para los filtros seleccionados.">
                        <Columns>
                            <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" HtmlEncode="true" />
                            <asp:BoundField DataField="Documento" HeaderText="Documento" HtmlEncode="true" />
                            <asp:BoundField DataField="Titular" HeaderText="Titular" HtmlEncode="true" />
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Creada" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado actual" HtmlEncode="true" />
                            <asp:BoundField DataField="Registro" HeaderText="Registro" HtmlEncode="true" />
                            <asp:BoundField DataField="Agenda" HeaderText="Agendamiento" HtmlEncode="true" />
                            <asp:BoundField DataField="FechaAgenda" HeaderText="Registro agenda" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Resultados" HeaderText="Resultados" HtmlEncode="true" />
                            <asp:BoundField DataField="FechaResultados" HeaderText="Envío resultados" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Auditoria" HeaderText="Auditoría" HtmlEncode="true" />
                            <asp:BoundField DataField="ProgresoAuditoria" HeaderText="Auditados / enviados" HtmlEncode="true" />
                            <asp:BoundField DataField="FechaAuditoria" HeaderText="Fin auditoría" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Cliente" HeaderText="Gestión cliente" HtmlEncode="true" />
                            <asp:BoundField DataField="ProgresoCliente" HeaderText="Informes / rechazados" HtmlEncode="true" />
                            <asp:BoundField DataField="FechaCliente" HeaderText="Fin cliente" HtmlEncode="true" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="paginas">
                    <asp:Button ID="BtnAnterior" runat="server" Text="Anterior" CssClass="btn btn-default" OnClick="BtnAnterior_Click" />
                    <asp:Label ID="LblPagina" runat="server" />
                    <asp:Button ID="BtnSiguiente" runat="server" Text="Siguiente" CssClass="btn btn-default" OnClick="BtnSiguiente_Click" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
