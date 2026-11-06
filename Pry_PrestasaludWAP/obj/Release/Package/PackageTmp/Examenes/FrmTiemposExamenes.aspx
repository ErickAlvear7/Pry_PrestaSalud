<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTiemposExamenes.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmTiemposExamenes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Tiempos de gestión de exámenes</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            padding: 20px;
            background: #f5f7fa;
        }

        .filtros {
            display: flex;
            gap: 15px;
            flex-wrap: wrap;
            align-items: flex-end;
            margin-bottom: 18px;
        }

            .filtros label {
                display: block;
            }

        .tabla {
            overflow-x: auto;
        }

        th {
            white-space: nowrap;
        }

        .nota {
            color: #555;
            margin: 10px 0;
        }

        .etapa {
            min-width: 170px;
            max-width: 190px;
            padding: 8px 10px;
            border: 1px solid #d6dde5;
            border-top: 3px solid #8090a0;
            border-radius: 5px;
            background: #f7f9fb;
        }

            .etapa.finalizada {
                border-top-color: #278547;
                background: #f1faf4;
            }

            .etapa.curso {
                border-top-color: #2879bd;
                background: #f0f7fd;
            }

            .etapa h4 {
                margin: 0 0 3px;
                font-size: 13px;
                font-weight: bold;
            }

            .etapa .duracion {
                font-size: 17px;
                font-weight: bold;
                margin: 6px 0;
                white-space: nowrap;
            }

            .etapa .situacion {
                font-size: 12px;
                font-weight: bold;
                margin-bottom: 5px;
            }

            .etapa .fechas {
                font-size: 11px;
                color: #465463;
                line-height: 1.45;
            }

            .etapa .intervalo {
                color: #465463;
                font-size: 11px;
                min-height: 28px;
                line-height: 1.3;
            }

        .tabla td {
            vertical-align: top !important;
            padding: 6px !important;
            font-size: 12px;
        }

        .tabla th {
            padding: 7px 6px !important;
            font-size: 12px;
        }

        .paginacion td {
            padding: 8px !important;
        }

        .paginacion a, .paginacion span {
            display: inline-block;
            padding: 5px 10px;
            margin: 0 2px;
            border: 1px solid #d6dde5;
            border-radius: 4px;
        }

        .paginacion span {
            background: #2879bd;
            color: white;
        }

        .filtros input[type=checkbox] {
            margin-right: 6px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SmPrincipal" runat="server" />
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Tiempos de gestión de exámenes</h3>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdTiempos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="filtros">
                            <div>
                                <label for="TxtDesde">Solicitud desde</label><asp:TextBox ID="TxtDesde" runat="server" TextMode="Date" CssClass="form-control" /></div>
                            <div>
                                <label for="TxtHasta">Solicitud hasta</label><asp:TextBox ID="TxtHasta" runat="server" TextMode="Date" CssClass="form-control" /></div>
                            <div>
                                <label for="TxtDocumento">Documento del titular</label><asp:TextBox ID="TxtDocumento" runat="server" MaxLength="20" CssClass="form-control" /></div>
                            <div>
                                <label for="DdlEstado">Estado</label><asp:DropDownList ID="DdlEstado" runat="server" CssClass="form-control">
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
                            <asp:Button ID="BtnExportarExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-success" OnClick="BtnExportarExcel_Click" />
                            <asp:CheckBox ID="ChkEnLinea" runat="server" Text="Actualizar cada 15 segundos" Checked="true" AutoPostBack="true" OnCheckedChanged="ChkEnLinea_CheckedChanged" />
                        </div>
                        <asp:Label ID="LblError" runat="server" ForeColor="Red" EnableViewState="false" />
                        <div class="nota">
                            <asp:Label ID="LblActualizacion" runat="server" /></div>
                        <div class="tabla">
                            <asp:GridView ID="GrdvTiempos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
                                AllowPaging="true" PageSize="3" OnPageIndexChanging="GrdvTiempos_PageIndexChanging" EmptyDataText="No hay solicitudes para los filtros seleccionados.">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="Primera" LastPageText="Última" PageButtonCount="5" />
                                <PagerStyle CssClass="paginacion" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" />
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                    <asp:BoundField DataField="Paciente" HeaderText="Titular" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado actual" />
                                    <asp:BoundField DataField="FechaSolicitud" HeaderText="Solicitud creada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                    <asp:TemplateField HeaderText="1. Agendamiento">
                                        <ItemTemplate>
                                            <%# BloqueEtapa(Container.DataItem, "Agenda") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="2. Resultados">
                                        <ItemTemplate>
                                            <%# BloqueEtapa(Container.DataItem, "Resultados") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="3. Auditoría">
                                        <ItemTemplate>
                                            <%# BloqueEtapa(Container.DataItem, "Auditoria") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4. Cliente">
                                        <ItemTemplate>
                                            <%# BloqueEtapa(Container.DataItem, "Cliente") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <p class="nota">Tiempos en horas, minutos y segundos. Agendamiento cuenta las 24 horas de lunes a viernes. Resultados, auditoría y cliente incluyen fines de semana. «Sin registro» identifica una fecha que no fue registrada.</p>
                        <asp:Timer ID="TimerTiempos" runat="server" Interval="15000" OnTick="TimerTiempos_Tick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>

