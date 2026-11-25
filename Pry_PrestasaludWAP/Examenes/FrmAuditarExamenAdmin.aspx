<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAuditarExamenAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmAuditarExamenAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <style>
        .paginas-auditor {
            display: flex;
            align-items: center;
            gap: 15px;
            margin: 12px 0;
        }

        .filtros-auditor {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: flex-end;
            margin-bottom: 8px;
        }

            .filtros-auditor > div {
                flex: 1 1 180px;
                min-width: 150px;
            }

            .filtros-auditor label {
                display: block;
            }

        .nota-filtros-auditor, .resumen-auditor {
            font-size: 12px;
            color: #627181;
            margin-bottom: 12px;
        }

        .paginacion-auditor > td {
            padding: 10px !important;
            background: #f5f5f5;
        }

        .paginacion-auditor table {
            width: auto;
            margin: 0;
        }

            .paginacion-auditor table td {
                padding: 3px;
            }

        .paginacion-auditor a, .paginacion-auditor span {
            display: inline-block;
            padding: 5px 10px;
            border: 1px solid #ddd;
            border-radius: 3px;
        }

        .paginacion-auditor span {
            background: #337ab7;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updTimer" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Tmrdat" runat="server" Interval="300000">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PnlFiltrosAuditor" runat="server" DefaultButton="BtnConsultarAuditor">
                            <div class="filtros-auditor">
                                <div>
                                    <label>Envío resultados desde</label><asp:TextBox ID="TxtDesdeAuditor" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                <div>
                                    <label>Envío resultados hasta</label><asp:TextBox ID="TxtHastaAuditor" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                <div>
                                    <label>Cédula / documento</label><asp:TextBox ID="TxtDocumentoAuditor" runat="server" MaxLength="20" CssClass="form-control" placeholder="Buscar documento" /></div>
                                <div>
                                    <label>Proceso</label><asp:DropDownList ID="DdlProcesoAuditor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="BtnConsultarAuditor_Click" CausesValidation="false">
                                        <asp:ListItem Value="">Todos</asp:ListItem>
                                        <asp:ListItem Value="EXR">Pendiente de auditar</asp:ListItem>
                                        <asp:ListItem Value="AUA">Auditado aceptado</asp:ListItem>
                                        <asp:ListItem Value="AUR">Auditado rechazado (todos)</asp:ListItem>
                                        <asp:ListItem Value="APELABLE">Con rechazo apelable</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Button ID="BtnConsultarAuditor" runat="server" Text="Consultar" CssClass="btn btn-primary" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnConsultarAuditor_Click" />
                                <asp:Button ID="BtnLimpiarAuditor" runat="server" Text="Limpiar" CssClass="btn btn-default" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnLimpiarAuditor_Click" />
                            </div>
                        </asp:Panel>
                        <p class="nota-filtros-auditor">El rango corresponde a la fecha de envío de resultados. Seleccione las fechas y pulse Consultar.</p>
                        <div class="resumen-auditor">
                            <asp:Label ID="LblResumenAuditor" runat="server" /></div>
                        <div style="width: 100%; overflow-x: auto;">

                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                ShowHeaderWhenEmpty="True" DataKeyNames="CodigoEXSO,EstadoCodigo" EmptyDataText="No existen datos para mostrar"
                                OnRowDataBound="GrdvDatos_RowDataBound" AllowPaging="true" PageSize="5" OnPageIndexChanging="GrdvDatos_PageIndexChanging">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                    <asp:BoundField DataField="FechaEnvioResultados" HeaderText="Fecha Envío Resultados" />
                                    <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                                    <asp:TemplateField HeaderText="Auditar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="BtnAuditar" runat="server"
                                                Height="20px"
                                                ImageUrl="~/Botones/btnauditarexamen.png"
                                                OnClick="BtnAuditar_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Visible="false" Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="« Primera" LastPageText="Última »" />
                                <PagerStyle CssClass="paginacion-auditor" HorizontalAlign="Left" />
                                <RowStyle Font-Size="X-Small" />
                            </asp:GridView>
                            <div class="paginas-auditor">
                                <asp:Button ID="BtnAnteriorAuditor" runat="server" Text="Anterior" OnClick="BtnAnteriorAuditor_Click" CausesValidation="false" CssClass="btn btn-default" Enabled="false" />
                                <asp:Label ID="LblPaginaAuditor" runat="server" Text="Página 1 de 1" />
                                <asp:Button ID="BtnSiguienteAuditor" runat="server" Text="Siguiente" OnClick="BtnSiguienteAuditor_Click" CausesValidation="false" CssClass="btn btn-default" Enabled="false" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnAnteriorAuditor" />
                        <asp:PostBackTrigger ControlID="BtnSiguienteAuditor" />
                        <asp:PostBackTrigger ControlID="BtnConsultarAuditor" />
                        <asp:PostBackTrigger ControlID="BtnLimpiarAuditor" />
                        <asp:PostBackTrigger ControlID="DdlProcesoAuditor" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
