<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoTurnoOdon.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmNuevoTurnoOdon" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <style>
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
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 20%">
                                        <div style="display:inline-block;">
                                            <asp:TreeView ID="trvPrestadoras" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="trvPrestadoras_SelectedNodeChanged" OnTreeNodePopulate="trvPrestadoras_TreeNodePopulate" TabIndex="1">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                <Nodes>
                                                    <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Prestardoras/Clínicas" Value="Prestardoras/Clínicas"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>
                                        </div>
                                    </td>
                                    <td style="width: 80%">
                                     <%--<asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <table style="width: 100%" class="table table-bordered table-responsive">
                                            <tr>
                                                <td style="width: 20%">
                                                    <h5>Médico:</h5>
                                                </td>
                                                <td style="width: 80%" colspan="3">
                                                    <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" ReadOnly="True" TabIndex="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Horario</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlHorario_SelectedIndexChanged" TabIndex="3">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Intervalo:</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlIntervalo" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Día:</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddlDia" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="6" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="7" />
                                                </td>
                                                <td style="text-align: center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlTurnos" runat="server" GroupingText="Turnos Asociados" Height="250px" ScrollBars="Vertical">
                                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="8">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Dia" HeaderText="Día" />
                                                                <asp:BoundField DataField="Horario" HeaderText="Horario" />
                                                                <asp:BoundField DataField="Intervalo" HeaderText="Intervalo" />
                                                                <asp:TemplateField HeaderText="Estado">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkEstadoDet" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstadoDet_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Etiqueta">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgEliminar" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="imgEliminar_Click" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle Font-Size="X-Small" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="3">
                                                    <asp:Button ID="btnGrabar" runat="server" CssClass="button" OnClick="btnGrabar_Click" Text="GRABAR" Width="120px" TabIndex="9" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 100%"></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
    <script>
        function Close() {
            window.top.location.reload();
        }
    </script>
</body>
</html>
