<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoUserAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Configuracion.FrmNuevoUserAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../css/chosen.css" />
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
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
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Usuario:</span></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlUsuario" runat="server" class="chzn-select" Width="100%" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5 runat="server" id="LblEstado" visible="false">Estado:</span></h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="ChkEstado_CheckedChanged" TabIndex="2" Visible="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Prestadora:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoPresta" runat="server" AutoPostBack="True" class="form-control" TabIndex="3" Width="100%" OnSelectedIndexChanged="DdlTipoPresta_SelectedIndexChanged">
                                        <asp:ListItem Value="M">Medico</asp:ListItem>
                                        <asp:ListItem Value="O">Odontologico</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Prestadora:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlPrestadora" runat="server" class="chzn-select" TabIndex="4" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel2" runat="server" Height="20px">
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:ImageButton ID="ImgAgregar" runat="server" Height="25px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAgregar_Click" TabIndex="5" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="Panel1" runat="server" Height="30px">
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="PnlDatos" runat="server" GroupingText="Prestadoras Asignadas" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoPRES,CodigoPEDE,Estado,Nuevo,CodigoAUXI" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="6" OnRowDataBound="GrdvDatos_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TipoPrestadora" HeaderText="Tipo_Prestadora" />
                                                <asp:BoundField DataField="Prestadora" HeaderText="Prestadora">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkEstadoPres" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstadoPres_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgEliminar" runat="server" Height="20px" ImageUrl="~/Botones/eliminaroff.jpg" OnClick="ImgEliminar_Click" Enabled="False" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
                <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="7" OnClick="BtnGrabar_Click" />
                                </td>
                                <td style="width: 5%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="8" OnClick="BtnSalir_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {
                $(".chzn-select").chosen({ width: "100%" });
                $(".chzn-container").css({ "width": "100%" });
                $(".chzn-drop").css({ "width": "95%" });
            }
        </script>
    </form>
</body>
</html>
