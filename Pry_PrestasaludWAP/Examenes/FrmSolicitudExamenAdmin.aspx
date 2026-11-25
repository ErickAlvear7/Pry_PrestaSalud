<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSolicitudExamenAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmSolicitudExamenAdmin" %>

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
        .filtros-solicita {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: flex-end;
            margin-bottom: 8px;
        }

            .filtros-solicita > div {
                flex: 1 1 180px;
                min-width: 150px;
            }

            .filtros-solicita label {
                display: block;
            }

        .nota-filtros-solicita, .resumen-solicita {
            font-size: 12px;
            color: #627181;
            margin-bottom: 12px;
        }

        .paginacion-solicita > td {
            padding: 10px !important;
            background: #f5f5f5;
        }

        .paginacion-solicita table {
            width: auto;
            margin: 0;
        }

            .paginacion-solicita table td {
                padding: 3px;
            }

        .paginacion-solicita a, .paginacion-solicita span {
            display: inline-block;
            padding: 5px 10px;
            border: 1px solid #ddd;
            border-radius: 3px;
        }

        .paginacion-solicita span {
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
            <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdPrincipal" runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <button id="BtnNuevo" runat="server" type="submit" class="btn btn-primary"
                                        onserverclick="BtnNuevo_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="PnlFiltrosSolicita" runat="server" DefaultButton="BtnConsultarSolicita">
                            <div class="filtros-solicita">
                                <div>
                                    <label>Solicitud desde</label><asp:TextBox ID="TxtDesdeSolicita" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                <div>
                                    <label>Solicitud hasta</label><asp:TextBox ID="TxtHastaSolicita" runat="server" TextMode="Date" CssClass="form-control" /></div>
                                <div>
                                    <label>Cédula / documento</label><asp:TextBox ID="TxtDocumentoSolicita" runat="server" MaxLength="20" CssClass="form-control" placeholder="Buscar documento" /></div>
                                <div>
                                    <label>Proceso</label><asp:DropDownList ID="DdlProcesoSolicita" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="BtnConsultarSolicita_Click" CausesValidation="false">
                                        <asp:ListItem Value="">Todos</asp:ListItem>
                                        <asp:ListItem Value="SRR">Solicitud realizada</asp:ListItem>
                                        <asp:ListItem Value="SRV">Solicitud revisada</asp:ListItem>
                                        <asp:ListItem Value="SGA">Solicitud solicitada</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Button ID="BtnConsultarSolicita" runat="server" Text="Consultar" CssClass="btn btn-primary" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnConsultarSolicita_Click" />
                                <asp:Button ID="BtnLimpiarSolicita" runat="server" Text="Limpiar" CssClass="btn btn-default" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnLimpiarSolicita_Click" />
                            </div>
                        </asp:Panel>
                        <p class="nota-filtros-solicita">Fechas del registro de la solicitud. Seleccione las fechas y pulse Consultar.</p>
                        <div class="resumen-solicita">
                            <asp:Label ID="LblResumenSolicita" runat="server" /></div>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False" DataKeyNames="CodigoEXSO,CodigoPERS,NumDocumento,EstadoCodigo"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" OnRowDataBound="GrdvDatos_RowDataBound" AllowPaging="true" PageSize="5" OnPageIndexChanging="GrdvDatos_PageIndexChanging">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="FecSolicitud" HeaderText="Fecha_Solicitud" />
                                            <asp:BoundField DataField="NumDocumento" HeaderText="No.Documento" />
                                            <asp:BoundField DataField="Cliente" HeaderText="Paciente" />
                                            <asp:BoundField DataField="Observacion" HeaderText="Fecha Agendamiento" />
                                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                                            <asp:BoundField DataField="EstadoSolicitud" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Detalle">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDetalle" runat="server" Height="20px" ImageUrl="~/Botones/busqueda.png" OnClick="ImgDetalle_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="« Primera" LastPageText="Última »" />
                                        <PagerStyle CssClass="paginacion-solicita" HorizontalAlign="Left" />
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnConsultarSolicita" />
                        <asp:PostBackTrigger ControlID="BtnLimpiarSolicita" />
                        <asp:PostBackTrigger ControlID="DdlProcesoSolicita" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
