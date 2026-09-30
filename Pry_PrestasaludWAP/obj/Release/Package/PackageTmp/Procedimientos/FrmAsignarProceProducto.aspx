<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAsignarProceProducto.aspx.cs" Inherits="Pry_PrestasaludWAP.Procedimientos.FrmAsignarProceProducto" %>

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
                <asp:UpdatePanel ID="updPrincipal" runat="server" UpdateMode="Conditional">
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
                                    <table class="table table-bordered table-condensed table-hover table-responsive">
                                        <tr>
                                            <td style="width:15%"></td>
                                            <td style="width:15%"></td>
                                            <td style="width:20%"></td>
                                            <td style="width:15%"></td>
                                            <td style="width:20%"></td>
                                            <td style="width:15%"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td><h5>Asistencia x Mes:</h5></td>
                                            <td>
                                                <asp:TextBox ID="txtAsisMensual" runat="server" CssClass="form-control alinearDerecha" MaxLength="4" TabIndex="4" Width="100%">1</asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtAsisMensual_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAsisMensual">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td><h5>Asistencia x Año:</h5></td>
                                            <td>
                                                <asp:TextBox ID="txtAsisAnual" runat="server" CssClass="form-control alinearDerecha" MaxLength="4" TabIndex="5" Width="100%">1</asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtAsisAnual_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAsisAnual">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="chkFechaCobertura" runat="server" Font-Size="Smaller" OnCheckedChanged="chkFechaCobertura_CheckedChanged" TabIndex="6" Text="Fec.Cobertura" AutoPostBack="True" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="chkFechaSistema" runat="server" Font-Size="Smaller" TabIndex="7" Text="Fec. Sistema" AutoPostBack="True" OnCheckedChanged="chkFechaSistema_CheckedChanged" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><h5>Procedimiento:</h5></td>
                                            <td colspan="5">
                                                <asp:DropDownList ID="ddlProcedimientos" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><h5>%Cobertura:</h5></td>
                                            <td>
                                                <asp:TextBox ID="txtCobertura" runat="server" CssClass="form-control upperCase" MaxLength="3" Width="100%" TabIndex="3">0</asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtCobertura_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCobertura">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td></td>
                                            <td>
                                                &nbsp;</td>
                                            <td></td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="8" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="9" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="10" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                    <table class="table table-bordered table-condensed table-hover table-responsive">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlDatos" runat="server" Height="350px" ScrollBars="Vertical">
                                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                        ShowHeaderWhenEmpty="True" DataKeyNames="Codigo,CodigoProducto,CodigoProcedimiento,Estado" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="11">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Procedimiento" HeaderText="Procedimiento" />
                                                            <asp:BoundField DataField="Cobertura" HeaderText="Cobertura(%)" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Estado">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelecc" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecc_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Etiqueta">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Editar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccionar.png" OnClick="btnselecc_Click" />
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
                                                <asp:Button ID="btnGrabar" runat="server" CssClass="button" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" TabIndex="12" />
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
