<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDatosOdontoHorarios.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaOdontologica.FrmDatosOdontoHorarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />    
    <link href="../css/Estilos.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            height: 35px;
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
                                <td>
                                    <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo" ShowHeaderWhenEmpty="True" Width="100%">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Dia" HeaderText="DIA" />
                                            <asp:BoundField DataField="Horario" HeaderText="HORARIO" />
                                            <asp:BoundField DataField="Intervalo" HeaderText="INTERVALO" />
                                            <asp:BoundField DataField="Estado" HeaderText="ESTADO" />
                                        </Columns>
                                        <RowStyle Font-Size="Small" />
                                    </asp:GridView>
                                </td>
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
                                <td style="text-align: center">
                                    <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" />
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
