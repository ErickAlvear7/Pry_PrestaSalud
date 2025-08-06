<%@ Page Language="C#" AutoEventWireup="true" Inherits="Horarios_FrmNuevoHorario" Codebehind="FrmNuevoHorario.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Horario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 12px;
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
                                <td style="width: 10%"></td>
                                <td style="width: 15%">
                                    <h5>Nombre Horario:</h5>
                                </td>
                                <td style="width: 60%" colspan="2">
                                    <asp:TextBox ID="txtHorario" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Descripción:</span></h5>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDescripcion" runat="server" onkeydown = "return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="150" Height="50px" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Intervalo:</span></h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlIntervalo" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlIntervalo_SelectedIndexChanged" TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Hora Incio:</span></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlHoraIni" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                    </asp:DropDownList>

                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMinutoIni" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Hora Fin:</span></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlHoraFin" runat="server" CssClass="form-control" Width="100%" TabIndex="6">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMinutoFin" runat="server" CssClass="form-control" Width="100%" TabIndex="7">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgProcesar" runat="server" ImageUrl="~/Botones/procesar.png" OnClick="imgProcesar_Click" Height="25px" TabIndex="8" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="chkEstado_CheckedChanged" Text="Activo" Visible="False" TabIndex="9" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3">
                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Horarios Generados" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="10">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="HoraInicio" HeaderText="Hora_Inicio" >
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Hora_Fin" DataField="HoraFin" >
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
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
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="11" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="12" />
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
