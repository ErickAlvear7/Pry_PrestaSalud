<%@ Page Language="C#" AutoEventWireup="true" Inherits="Perfil_FrmEditarPerfil" Codebehind="FrmEditarPerfil.aspx.cs" %>

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
            <asp:ScriptManager ID="smmantenimiento" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
            <hr />
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 20%">
                                    <h5>Perfil:</h5>
                                </td>
                                <td style="width: 60%">
                                    <asp:TextBox ID="txtPerfil" runat="server" Width="100%" MaxLength="80" CssClass="form-control upperCase" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Descripción:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescripcion" runat="server" onkeydown = "return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="80" TextMode="MultiLine" Height="50px" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkestado" runat="server" AutoPostBack="True" OnCheckedChanged="chkestado_CheckedChanged" Text="Activo" Checked="True" CssClass="form-control" TabIndex="3" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Crear Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkCrear" runat="server" AutoPostBack="True" OnCheckedChanged="chkCrear_CheckedChanged" Text="No" CssClass="form-control" TabIndex="4" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Modificar Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkModificar" runat="server" Text="No" AutoPostBack="True" OnCheckedChanged="chkModificar_CheckedChanged" CssClass="form-control" TabIndex="5" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Eliminar Parámetros:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEliminar" runat="server" Text="No" AutoPostBack="True" OnCheckedChanged="chkEliminar_CheckedChanged" CssClass="form-control" TabIndex="6" />
                                </td>
                                <td></td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <table style="height: 100%; width: 100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Height="350px" Width="100%">
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        EmptyDataText="No existe ningún menú agregado" DataKeyNames="Codigo" TabIndex="7">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Codigo" HeaderText="Código" Visible="False" />
                                            <asp:BoundField DataField="Menu" HeaderText="Menú" />
                                            <asp:BoundField DataField="SubMenu" HeaderText="SubMenu" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Agregar">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAgregar" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </asp:Panel>
<%--                                <script>
                                    $(document).ready(function () {
                                        $('#grdvDatos').dataTable();
                                    });
                                </script>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="8" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" OnClick="btnSalir_Click" CssClass="button" TabIndex="9" />
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
