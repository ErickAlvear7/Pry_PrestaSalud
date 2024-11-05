<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDetalleGestionesRealizadas.aspx.cs" Inherits="Pry_PrestasaludWAP.Medicos.FrmDetalleGestionesRealizadas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />

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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS TITULAR</h3>
                <asp:UpdatePanel ID="updDetalle" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdvTitular" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" ShowFooter="True" TabIndex="1">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                            <asp:BoundField HeaderText="Genero" DataField="Genero"></asp:BoundField>
                                            <asp:BoundField DataField="EstCivil" HeaderText="Est.Civil"></asp:BoundField>
                                            <asp:BoundField DataField="Celular" HeaderText="Celular"></asp:BoundField>
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS PACIENTE</h3>
                <asp:UpdatePanel ID="updPaciente" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdvPaciente" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="2">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                                            <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                            <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha_Nacimiento" />
                                            <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">HISTORIAL CITAS</h3>
                <asp:UpdatePanel ID="updHistorial" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvHistorial" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="2" DataKeyNames="Codigo">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoCita" HeaderText="Tipo_Cita" />
                                                <asp:BoundField DataField="FechaCita" HeaderText="Fecha_Cita" />
                                                <asp:BoundField DataField="HoraCita" HeaderText="Hora_Cita" />
                                                <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                                <asp:TemplateField HeaderText="Detalle">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgVer" runat="server" Height="20px" ImageUrl="~/Botones/busqueda.png" OnClick="imgVer_Click" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 50%"></td>
                                <td style="text-align: left; width: 50%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="3" />
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
