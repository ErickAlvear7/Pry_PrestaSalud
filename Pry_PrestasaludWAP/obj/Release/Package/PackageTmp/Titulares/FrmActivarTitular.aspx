<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmActivarTitular.aspx.cs" Inherits="Pry_PrestasaludWAP.Titulares.FrmActivarTitular" %>

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
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table style="width:100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 25%">
                                    <h5>Criterio Cédula/Nombres-Apellidos:</h5>
                                </td>
                                <td style="width: 45%">
                                    <asp:TextBox ID="txtCriterio" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 20%; text-align: center;">
                                    <asp:Button ID="btnBuscar" runat="server" CssClass="button" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="2" />
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                        </table>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlDatos" runat="server" Height="450px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" DataKeyNames="Codigo,CodigoProducto,Estado" TabIndex="3" OnRowDataBound="grdvDatos_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="NumeroDocumento" HeaderText="Identificación" />
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                <asp:BoundField DataField="Titular" HeaderText="Titular" />
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                <asp:TemplateField HeaderText="Selecionar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelecc" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecc_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                    <%--                                    <script>
                                        $(document).ready(function () {
                                            $('#grdvDatos').dataTable();
                                        });
                                    </script>--%>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
