<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAtencionCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.Medicos.FrmAtencionCitaMedica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Atención Médica</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <style type="text/css">
        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

            .overlayContent h2 {
                font-size: 18px;
                font-weight: bold;
                color: #000;
            }

            .overlayContent img {
                width: 80px;
                height: 80px;
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updDetalle">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Generando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS TITULAR</h3>
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="grdvDatosTitular" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" TabIndex="1" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                                            <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                        <HeaderStyle Font-Size="Small" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS PACIENTE - ATENCION MEDICA</h3>
                <asp:UpdatePanel ID="updBody" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 40%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="grdvDatosCita" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" TabIndex="2" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoCita" HeaderText="Codigo" />
                                            <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                                            <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                            <asp:BoundField DataField="FechaNacimiento" HeaderText="Fec.Nacim" />
                                            <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                            <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                        <HeaderStyle Font-Size="Small" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">REGISTRO ATENCION MEDICA</h3>
                <asp:UpdatePanel ID="updDetalle" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 70%"></td>
                                <td style="width: 5%; text-align: center;"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>VADEMECUM ---></h5>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImgVademecum" runat="server" Height="25px" ImageUrl="~/Images/Vademecum.png" OnClick="ImgVademecum_Click" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Buscar CIE10:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control upperCase" MaxLength="250" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgBuscar" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="imgBuscar_Click" TabIndex="4" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Diagnóstico:</h5>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstCIE10" runat="server" AutoPostBack="True" Height="180px" OnSelectedIndexChanged="lstCIE10_SelectedIndexChanged" TabIndex="5" Width="100%" Font-Size="9pt"></asp:ListBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Detalle:</h5>
                                </td>
                                <td>
                                    <cc1:Editor ID="txtEditor" runat="server" Height="300px" AutoFocus="False" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">HISTORIAL CITAS MEDICAS</h3>
                <asp:UpdatePanel ID="updHistorialCitas" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 90%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="350px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="7" DataKeyNames="Codigo">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoAgenda" HeaderText="Tipo" />
                                                <asp:BoundField DataField="FechaRegistro" HeaderText="Registro" />
                                                <asp:BoundField DataField="FechaCita" HeaderText="Fecha_Cita" />
                                                <asp:BoundField DataField="Horacita" HeaderText="Hora_Cita" />
                                                <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                                <asp:BoundField DataField="RegistroCIE10" HeaderText="CIE10">
                                                    <ItemStyle Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Medico" HeaderText="Medico">
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Detalle">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgVer" runat="server" Height="20px" ImageUrl="~/Botones/busqueda.png" OnClick="imgVer_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
                                            <HeaderStyle Font-Size="Small" />
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
                <%--                <asp:UpdatePanel ID="updBotones" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right; width: 45%">
                            <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="8" />
                        </td>
                        <td style="width: 10%"></td>
                        <td style="text-align: left; width: 45%">
                            <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="9" />
                        </td>
                    </tr>
                </table>
                <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </form>
</body>
</html>
