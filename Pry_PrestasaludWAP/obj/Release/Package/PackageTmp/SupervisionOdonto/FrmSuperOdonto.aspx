<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSuperOdonto.aspx.cs" Inherits="Pry_PrestasaludWAP.SupervisionOdonto.FrmSuperOdonto" %>

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
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
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
                                    <h5>Supervisor:</span></h5>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlSupervisor" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlSupervisor_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5>Provincia:</span></h5></td>
                                <td>
                                    <asp:DropDownList ID="ddlProvincia" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                                <td><h5 style="text-align: center">Ciudad:</span></h5></td>
                                <td>
                                    <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" TabIndex="3">
                                    </asp:DropDownList>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5>Descripción:</span</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDescripcion" runat="server" Width="100%" MaxLength="250" CssClass="form-control upperCase" TabIndex="4" Height="50px"></asp:TextBox>                                
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5>Prestadora:</span></h5></td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlPrestadora" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlPrestadora_SelectedIndexChanged" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5>Médicos:</span></h5></td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlMedicos" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlMedicos_SelectedIndexChanged" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5 runat="server" id="lblEstado" visible="false">Estado:</span></h5></td>
                                <td>
                                    <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="chkEstado_CheckedChanged" Visible="False" TabIndex="6" />
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
                                    <asp:Panel ID="pnlMedicos" runat="server" GroupingText="Lista de Médicos" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoPerm,CodigoDeta,Estado" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="7">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" >
                                                <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Prestadora" HeaderText="Prestadora">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Medico" HeaderText="Medico">
                                                <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Logueo" HeaderText="Login">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelecc" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecc_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Etiqueta">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgEstado" runat="server" Height="20px" ImageUrl="~/Botones/sin_usuario.png" />
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
                                            <RowStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 50%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="GRABAR" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="8" />
                                </td>
                                <td style="text-align: left; width: 50%">
                                    <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" TabIndex="9" OnClick="btnSalir_Click" />
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
