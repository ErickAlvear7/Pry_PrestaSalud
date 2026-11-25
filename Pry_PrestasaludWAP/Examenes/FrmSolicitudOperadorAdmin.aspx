<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSolicitudOperadorAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmSolicitudOperadorAdmin" %>

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
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        html, body, #form1 {
            width: 100%;
            max-width: 100%;
            margin: 0;
        }

            #form1, #form1 * {
                box-sizing: border-box;
            }

        .solicitudes-listado {
            width: 100%;
            max-width: 100%;
            min-width: 0;
        }

            .solicitudes-listado .panel-body {
                min-width: 0;
                padding: 12px;
            }

        .solicitudes-tabla {
            width: 100%;
            max-width: 100%;
            overflow-x: auto;
        }

        #GrdvDatos {
            width: 100%;
            min-width: 1040px;
            table-layout: fixed;
            margin-bottom: 8px;
        }

            #GrdvDatos th, #GrdvDatos td {
                white-space: normal;
                overflow-wrap: break-word;
                vertical-align: middle;
            }

            #GrdvDatos th {
                font-size: 12px;
            }

                #GrdvDatos th:nth-child(4), #GrdvDatos td:nth-child(4) {
                    width: 100px;
                    white-space: nowrap;
                }

                #GrdvDatos th:nth-child(5), #GrdvDatos td:nth-child(5) {
                    width: 110px;
                }

                #GrdvDatos th:nth-child(6), #GrdvDatos td:nth-child(6) {
                    width: 100px;
                }

                #GrdvDatos th:nth-child(7), #GrdvDatos td:nth-child(7) {
                    width: 62px;
                }

            #GrdvDatos .paginacion-agenda > td {
                padding: 10px;
                background: #f5f5f5;
            }

            #GrdvDatos .paginacion-agenda table {
                width: auto;
                min-width: 0;
                margin: 0;
            }

                #GrdvDatos .paginacion-agenda table td {
                    width: auto !important;
                    padding: 3px;
                    white-space: nowrap;
                }

            #GrdvDatos .paginacion-agenda a, #GrdvDatos .paginacion-agenda span {
                display: inline-block;
                padding: 5px 10px;
                border: 1px solid #ddd;
                border-radius: 3px;
            }

            #GrdvDatos .paginacion-agenda span {
                background: #337ab7;
                color: white;
            }

        .filtros-agenda {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: flex-end;
            margin-bottom: 8px;
        }

            .filtros-agenda > div {
                flex: 1 1 180px;
                min-width: 150px;
            }

            .filtros-agenda label {
                display: block;
            }

        .nota-filtros-agenda, .resumen-agenda {
            font-size: 12px;
            color: #627181;
            margin-bottom: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary solicitudes-listado">
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
                <asp:Panel ID="PnlFiltrosAgenda" runat="server" DefaultButton="BtnConsultarAgenda">
                    <div class="filtros-agenda">
                        <div>
                            <label>Solicitud desde</label><asp:TextBox ID="TxtDesdeAgenda" runat="server" TextMode="Date" CssClass="form-control" />
                        </div>
                        <div>
                            <label>Solicitud hasta</label><asp:TextBox ID="TxtHastaAgenda" runat="server" TextMode="Date" CssClass="form-control" />
                        </div>
                        <div>
                            <label>Cédula / documento</label><asp:TextBox ID="TxtDocumentoAgenda" runat="server" MaxLength="20" CssClass="form-control" placeholder="Buscar documento" />
                        </div>
                        <div>
                            <label>Proceso</label><asp:DropDownList ID="DdlProcesoAgenda" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="BtnConsultarAgenda_Click" CausesValidation="false">
                                <asp:ListItem Value="">Todos</asp:ListItem>
                                <asp:ListItem Value="SRR">Solicitud realizada</asp:ListItem>
                                <asp:ListItem Value="SRV">Solicitud revisada</asp:ListItem>
                                <asp:ListItem Value="SGA">Solicitud agendada</asp:ListItem>
                                <asp:ListItem Value="EXR">Exámenes realizados</asp:ListItem>
                                <asp:ListItem Value="AUA">Auditado aceptado</asp:ListItem>
                                <asp:ListItem Value="AUR">Auditado rechazado (todos)</asp:ListItem>
                                <asp:ListItem Value="OAE">Orden adicional pendiente de agendar</asp:ListItem>
                                <asp:ListItem Value="OAG">Orden adicional agendada</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Button ID="BtnConsultarAgenda" runat="server" Text="Consultar" CssClass="btn btn-primary" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnConsultarAgenda_Click" />
                        <asp:Button ID="BtnLimpiarAgenda" runat="server" Text="Limpiar" CssClass="btn btn-default" CausesValidation="false" UseSubmitBehavior="false" OnClick="BtnLimpiarAgenda_Click" />
                    </div>
                </asp:Panel>
                <p class="nota-filtros-agenda">Fechas de la solicitud inicial o del envío de la orden adicional. Seleccione las fechas y pulse Consultar.</p>
                <div class="resumen-agenda">
                    <asp:Label ID="LblResumenAgenda" runat="server" />
                </div>
                <div class="solicitudes-tabla">
                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                        AutoGenerateColumns="False" DataKeyNames="CodigoEXSO,CodigoPERS,EstadoCodigo,CodigoTITU,Ext,CodigoPROD,OrdenCodigo,EsOrdenAdicional,CodigoPersonaOrden,EstadoOrden"
                        CssClass="table table-condensed table-bordered table-hover"
                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" OnRowDataBound="GrdvDatos_RowDataBound" AllowPaging="true" PageSize="5" OnPageIndexChanging="GrdvDatos_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="FecSolicitud" HeaderText="Fecha_Solicitud" />
                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario_Solicita" />
                            <asp:BoundField DataField="NumDocumento" HeaderText="Documento" />
                            <asp:BoundField DataField="Cliente" HeaderText="Titular" />
                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                            <asp:BoundField DataField="TipoOrden" HeaderText="Orden" />
                            <asp:TemplateField HeaderText="Adjunto">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDescargar" runat="server" Height="20px" ImageUrl="~/Botones/downloadcolor.png" OnClick="ImgDescargar_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Adjunto Daq.">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDescargarDaquilema"
                                        runat="server"
                                        Height="20px"
                                        ImageUrl="~/Botones/downloadcolor.png"
                                        ToolTip="Descargar documento adicional DAQUILEMA"
                                        Visible="false"
                                        OnClick="ImgDescargarDaquilema_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Examen">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDescargarPdf" runat="server" Height="20px"
                                        ImageUrl="~/Botones/downloadcolor.png"
                                        ToolTip="Descargar solicitud en PDF"
                                        OnClick="ImgDescargarPdf_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agendar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgAgendar" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="ImgAgendar_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cita Adicional">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgCitaAdicional" runat="server" Height="20px"
                                        ImageUrl="~/Botones/citamedica.png"
                                        ToolTip="Registrar cita adicional"
                                        OnClick="ImgCitaAdicional_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Resultados">
                                <ItemTemplate>
                                    <asp:LinkButton ID="BtnResultados" runat="server"
                                        CssClass="btn btn-primary btn-xs"
                                        ToolTip="Cargar resultados de exámenes"
                                        OnClick="BtnResultados_Click"
                                        CausesValidation="false">
                                                <span class="glyphicon glyphicon-upload"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="« Primera" LastPageText="Última »" Position="Bottom" />
                        <PagerStyle CssClass="paginacion-agenda" HorizontalAlign="Left" />
                        <RowStyle Font-Size="X-Small" />
                    </asp:GridView>
                    <script>
                                //$(document).ready(function () {
                                //    $('#GrdvDatos').dataTable();
                                //});
                    </script>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
