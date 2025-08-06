<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmProcedimientosAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Procedimientos.FrmProcedimientosAdmin" %>

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
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <button id="btnNuevo" runat="server" type="submit" class="btn btn-primary"
                                        onserverclick="btnNuevo_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <%--<asp:Panel ID="pnlDatos" runat="server" Height="300px" ScrollBars="Vertical">--%>
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="Codigo">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Procedimiento" HeaderText="Procedimiento" />
                                            <asp:BoundField DataField="Costo" HeaderText="Pvp" >
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Aplica" HeaderText="Se Aplica A" />
                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                            <asp:BoundField DataField="Defecto" HeaderText="Default" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Editar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="btnselecc_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                    <%--</asp:Panel>--%>
                                    <script>
                                        $(document).ready(function () {
                                            $('#grdvDatos').dataTable();
                                        });
                                    </script>
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
