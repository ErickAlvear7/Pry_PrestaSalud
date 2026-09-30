<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfigInfAdicional.aspx.cs" Inherits="Pry_PrestasaludWAP.Configuracion.FrmConfigInfAdicional" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
            <div class="panel-heading">
                <asp:UpdatePanel ID="updTitular" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lbltitulo" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%" class="table table-bordered table-responsive">
                            <tr>
                                <td style="width: 20%">
                                    <div style="display: inline-block;">
                                        <asp:TreeView ID="trvClientes" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="trvClientes_SelectedNodeChanged" OnTreeNodePopulate="trvClientes_TreeNodePopulate" TabIndex="1">
                                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                            <Nodes>
                                                <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Clientes/Productos" Value="Clientes/Productos"></asp:TreeNode>
                                            </Nodes>
                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                            <ParentNodeStyle Font-Bold="False" />
                                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                        </asp:TreeView>
                                    </div>
                                </td>
                                <td style="width: 80%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 20%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 20%"></td>
                                            <td style="width: 15%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <h5>Cabecera:</h5>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtCabecera" runat="server" CssClass="form-control" MaxLength="50" TabIndex="1" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td style="text-align: center"></td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgModificar" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="2" />
                                            </td>
                                            <td style="text-align: center">&nbsp;</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                    <table style="width:100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlDatos" runat="server" Height="350px" ScrollBars="Vertical">
                                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                        ShowHeaderWhenEmpty="True" DataKeyNames="Codigo,Ver,Body,Estado" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="3">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Campo" HeaderText="Campo" />
                                                            <asp:BoundField DataField="Cabecera" HeaderText="Cabecera"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ver">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkVer" runat="server" AutoPostBack="True" OnCheckedChanged="chkVer_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Body Mail">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkBody" runat="server" AutoPostBack="True" OnCheckedChanged="chkBody_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Estado">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstado_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Selecc">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnselecc" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="btnselecc_Click" />
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
                                            <td style="text-align: center">
                                                <asp:Button ID="btnGrabar" runat="server" CssClass="button" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" TabIndex="4" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
<%--            <div class="panel panel-default">
                <asp:UpdatePanel ID="updOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    
                                </td>
                                <td style="width:5%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="22" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
        </div>
    </form>
</body>
</html>
