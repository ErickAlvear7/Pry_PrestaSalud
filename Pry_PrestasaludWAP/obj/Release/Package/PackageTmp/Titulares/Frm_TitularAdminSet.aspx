<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_TitularAdminSet.aspx.cs" Inherits="Pry_PrestasaludWAP.Titulares.Frm_TitularAdminSet" %>

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
                        <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%" class="table table-bordered table-responsive">
                            <tr>
                                <td style="width: 20%">
                                    <div style="display: inline-block;">
                                        <asp:TreeView ID="TrvPrestadoras" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="TrvPrestadoras_SelectedNodeChanged" OnTreeNodePopulate="TrvPrestadoras_TreeNodePopulate" TabIndex="1">
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
                                    <table class="table table-bordered table-condensed table-hover table-responsive">
                                        <tr>
                                            <td>
                                                <button id="btnNuevo" runat="server" type="submit" class="btn btn-primary"
                                                    onserverclick="BtnNuevo_Click">
                                                    <span aria-hidden="true"
                                                        class="glyphicon glyphicon-plus"></span>
                                                </button>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table table-bordered table-condensed table-hover table-responsive">
                                        <tr>
                                            <td style="width: 35%">
                                                <h5>Criterio Cédula/Nombres-Apellidos:</h5>
                                            </td>
                                            <td style="width: 45%">
                                                <asp:TextBox ID="TxtCriterio" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="2"></asp:TextBox>
                                            </td>
                                            <td style="width: 20%; text-align: center;">
                                                <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" CssClass="button" OnClick="BtnBuscar_Click" TabIndex="1" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table table-bordered table-condensed table-hover table-responsive">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlDatos" runat="server" Height="350px" ScrollBars="Vertical">
                                                    <asp:GridView ID="GrdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                        ShowHeaderWhenEmpty="True" DataKeyNames="CodPersona,CodProducto,CodTitular" TabIndex="4">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Titular" HeaderText="Titular" />
                                                            <asp:BoundField DataField="NumeroDocumento" HeaderText="Numero Documento" />
                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                            <asp:TemplateField HeaderText="Editar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="Btnselecc_Click" />
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
                                </td>
                            </tr>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
