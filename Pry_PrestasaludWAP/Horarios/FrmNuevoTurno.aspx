<%@ Page Language="C#" AutoEventWireup="true" Inherits="Horarios_FrmNuevoTurno" Codebehind="FrmNuevoTurno.aspx.cs" %>

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
                                <td style="width: 20%"></td>
                                <td style="width: 15%">
                                    <h5>Nombre Turno:</h5>
                                </td>
                                <td style="width: 45%" colspan="3">
                                    <asp:TextBox ID="txtTurno" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 20%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Descripción:</span></h5>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDescripcion" runat="server" onkeydown = "return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="150" Height="100px" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control"
                                        OnCheckedChanged="chkEstado_CheckedChanged" Text="Activo" Visible="False" TabIndex="3" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="3"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Día:</h5>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDia" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Horario:</span></h5>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="Panel1" runat="server" Height="30px">
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="6" />
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="7" />
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="8" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="pnlTurnos" runat="server" GroupingText="Turnos Asociados" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoDia,CodigoHora" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="9">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Dia" HeaderText="Día" />
                                                <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HoraFin" HeaderText="Hora Fin">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Intervalo" HeaderText="Intervalo(min)" >
                                                <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
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
                                                <asp:TemplateField HeaderText="Editar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccionar.png" OnClick="imgSelecc_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
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
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="10" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="11" />
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
