<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMedicoUsuarioAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Medicos.FrmMedicoUsuarioAdmin" %>

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
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="Codigo,Asignado,CodigoUsu" OnRowDataBound="grdvDatos_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Medico" HeaderText="Médico" />
                                            <asp:BoundField DataField="Genero" HeaderText="Género" />
                                            <asp:BoundField DataField="Username" HeaderText="Login" />
                                            <asp:TemplateField HeaderText="Asignar Usuario">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAsignarUsu" runat="server" Height="20px" ImageUrl="~/Botones/agregar_usuario.jpg" OnClick="imgAsignarUsu_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desasignar Usuario">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDesasignaUsu" runat="server" Height="20px" ImageUrl="~/Botones/quitar_usuario.jpg" OnClick="imgDesasignaUsu_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
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
