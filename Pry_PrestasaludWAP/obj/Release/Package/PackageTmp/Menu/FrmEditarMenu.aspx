<%@ Page Language="C#" AutoEventWireup="true" Inherits="Menu_FrmEditarMenu" Codebehind="FrmEditarMenu.aspx.cs" %>

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
                                    <h5>Nombre del Menú:</h5>
                                </td>
                                <td style="width: 60%">
                                    <asp:TextBox ID="txtNombreMenu" runat="server" Width="100%" MaxLength="80" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Estado" CssClass="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEstado" runat="server" Checked="True" Text="Activo" OnCheckedChanged="chkEstado_CheckedChanged" AutoPostBack="True" CssClass="form-control" TabIndex="2" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Menú Padre</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMenuPadre" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuPadre_SelectedIndexChanged" TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="lblMenuPadre" visible="false">Nombre Menú Padre</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombreMenuPadre" runat="server" Width="100%" MaxLength="50" CssClass="form-control upperCase" Visible="False" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <hr />
            <table style="height: 100%; width: 100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Height="350px" Width="100%">
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive" EmptyDataText="No existe ningún menú agregado" DataKeyNames="Checking,CodigoTarea" PageSize="100" TabIndex="5">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoTarea" HeaderText="Código Tarea" Visible="False"></asp:BoundField>
                                            <asp:BoundField DataField="Tarea" HeaderText="Tarea"></asp:BoundField>
                                            <asp:BoundField DataField="Ruta" HeaderText="Ruta/Página aspx"></asp:BoundField>
                                            <asp:BoundField DataField="EstadoTarea" HeaderText="Estado"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Subir Nivel">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgSubirNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_up.png" OnClick="imgSubirNivel_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bajar Nivel">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBajarNivel" runat="server" Height="20px" ImageUrl="~/Botones/activada_down.png" OnClick="imgBajarNivel_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
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
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="6" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="7" />
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
