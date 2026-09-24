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
                                    <asp:GridView ID="GrdvGrupoExamen" runat="server"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover tabla-solicitudes"
                                        DataKeyNames="CodigoEXSO,EstadoCodigo"
                                        AllowPaging="True"
                                        PageSize="10"
                                        ShowHeaderWhenEmpty="True"
                                        OnRowDataBound="GrdvGrupoExamen_RowDataBound"
                                        OnPageIndexChanging="GrdvGrupoExamen_PageIndexChanging"
                                        Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="CodigoEXSO" HeaderText="Solicitud">
                                                <ItemStyle HorizontalAlign="Center" Width="8%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud">
                                                <ItemStyle HorizontalAlign="Center" Width="12%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cedula" HeaderText="Cédula">
                                                <ItemStyle HorizontalAlign="Center" Width="12%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Paciente" HeaderText="Paciente">
                                                <ItemStyle Width="24%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Producto" HeaderText="Producto">
                                                <ItemStyle Width="18%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Estado" HeaderText="Estado">
                                                <ItemStyle HorizontalAlign="Center" Width="16%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FechaResultado" HeaderText="Fecha Resultado">
                                                <ItemStyle HorizontalAlign="Center" Width="12%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Gestión">
                                                <%--<ItemTemplate>
                                                    <asp:ImageButton ID="ImgSeleccGrupo" runat="server" Height="18px" ImageUrl="~/Botones/selecc.png" CausesValidation="false" OnClick="ImgSeleccGrupo_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="8%" />--%>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgGestion" runat="server" Height="22px"
                                                        ImageUrl="~/Botones/selecc.png"
                                                        ToolTip="Gestionar examen"
                                                        CausesValidation="false"
                                                        CommandArgument='<%# Eval("CodigoEXSO") %>'
                                                        OnCommand="ImgGestion_Command" />
                                                </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-warning" style="font-size: 12px; margin-bottom: 0;">
                                                No existen solicitudes de exámenes
                                                para el cliente seleccionado.
                                            </div>
                                        </EmptyDataTemplate>
                                        <PagerStyle HorizontalAlign="Center" CssClass="paginador" />
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
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
