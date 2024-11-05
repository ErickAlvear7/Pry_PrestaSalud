<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAtencionCitaOdonto.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmAtencionCitaOdonto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Atención Odontológica</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />

    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }

        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

            .overlayContent h2 {
                font-size: 18px;
                font-weight: bold;
                color: #000;
            }

            .overlayContent img {
                width: 80px;
                height: 80px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updDetalle">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">PROCEDIMIENTOS</h3>
                <asp:UpdatePanel ID="updPresupuesto" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 50%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <h5>Coberturas:</h5>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgProductos" runat="server" Height="30px" ImageUrl="~/Botones/productos.png" OnClick="imgProductos_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>OdontoGrama:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOndotograma" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlOndotograma_SelectedIndexChanged" Width="100%" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%">
                                                <h5>Procedimiento</h5>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:DropDownList ID="ddlProcedimiento" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlProcedimiento_SelectedIndexChanged" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Pieza</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPieza" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgAgregar" runat="server" Height="30px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="4" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td style="width: 50%; text-align: center;">

                                    <asp:Image ID="imgOdontoGrama" runat="server" Height="300px" Width="350px" TabIndex="9" />

                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">REGISTRO PRESUPUESTO - PROCEDIMIENTOS</h3>
                <asp:UpdatePanel ID="updDetalle" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlPresupuesto" runat="server" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvPresupuesto" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="Codigo,CodigoCabecera,CodigoProcedimiento,Inicial,Realizado,Prioridad,Realizar,Cobertura,CodigoPrestadora" OnRowDataBound="grdvPresupuesto_RowDataBound" ShowFooter="True" TabIndex="5">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEliminar" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="imgEliminar_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Procedimiento" HeaderText="Procedimiento" />
                                                <asp:BoundField HeaderText="Pieza" DataField="Pieza">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Pvp" HeaderText="Pvp($)">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Costo" HeaderText="Costo_Red($)">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cobertura" HeaderText="Cobertura(%)">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total($)">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Prioridad" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgPrioridad" runat="server" Height="20px" ImageUrl="~/Botones/activada_up.png" OnClick="imgPrioridad_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Realizar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRealizar" runat="server" AutoPostBack="True" OnCheckedChanged="chkRealizar_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deta.">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDetalle" runat="server" Height="20px" ImageUrl="~/Botones/buscaroff.png" OnClick="imgDetalle_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 70%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td></td>
                                <td>
                                    <h5>Registro Atención</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRegistroOdonto" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="6">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Observación</h5>
                                </td>
                                <td>
                                    <cc1:Editor ID="txtEditor" runat="server" Height="300px" />
                                </td>
                                <td></td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">HISTORIAL CITAS MEDICAS</h3>
                <asp:UpdatePanel ID="updHistorial" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 90%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="10" DataKeyNames="Codigo">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="FechaCita" HeaderText="Fecha_Cita" />
                                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Registro" />
                                            <asp:BoundField DataField="Prestadora" HeaderText="Prestadora" />
                                            <asp:BoundField DataField="Medico" HeaderText="Medico" />
                                            <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                                            <asp:TemplateField HeaderText="Observación">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgVer" runat="server" Height="20px" ImageUrl="~/Botones/busqueda.png" OnClick="imgVer_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <%--                <asp:UpdatePanel ID="updBotones" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right; width: 33%">
                            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="8" />
                        </td>
                        <td style="text-align: center; width: 33%">
                            <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" />
                        </td>
                        <td style="text-align: left; width: 33%">
                            <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnFinalizar_Click" OnClientClick="if ( !confirm('Esta Seguro de Finalizar Presupuesto?')) return false;" />
                        </td>
                    </tr>
                </table>
                <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </form>
</body>
</html>
