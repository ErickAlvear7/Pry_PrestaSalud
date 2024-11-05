<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevaPrestadoraOdon.aspx.cs" Inherits="Pry_PrestasaludWAP.PrestadoraOdonto.FrmNuevaPrestadoraOdon" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
            <div class="panel-body">
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
                    <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                        <ProgressTemplate>
                            <div class="overlay" />
                            <div class="overlayContent">
                                <h2>Espere..</h2>
                                <img src="../Images/load.gif" alt="Loading" border="1" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%; height: 100%">
                                <tr>
                                    <td style="width: 10%">
                                        <h5>Provincia:</h5>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:DropDownList ID="ddlProvincia" runat="server" Width="100%" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 10%">
                                        <h5>Ciudad:</h5>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:DropDownList ID="ddlCiudad" runat="server" Width="100%" CssClass="form-control" TabIndex="2">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Prestadora:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPretadora" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="3"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Dirección:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDireccion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="150" TextMode="MultiLine" Width="100%" Height="50px" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Teléfono 1:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFono1" runat="server" Width="100%" CssClass="form-control" MaxLength="10" TabIndex="5"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtFono1_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFono1">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Teléfono 2:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFono2" runat="server" Width="100%" CssClass="form-control" MaxLength="10" TabIndex="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtFono2_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFono2">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Teléfono 3:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFono3" runat="server" Width="100%" CssClass="form-control" MaxLength="10" TabIndex="7"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtFono3_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFono3">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Celular:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" runat="server" Width="100%" CssClass="form-control" MaxLength="10" TabIndex="8"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtCelular_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Fax:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" Width="100%" MaxLength="10" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtFax_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFax">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>E-mail 1:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail1" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="10"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar1" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar1_CheckedChanged" Text="Enviar" TabIndex="11" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>E-mail 2:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail2" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="12"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar2" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar2_CheckedChanged" Text="Enviar" TabIndex="13" />
                                    </td>
                                    <td>
                                        <h5>E-mail 3:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail3" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="14"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar3" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar3_CheckedChanged" Text="Enviar" TabIndex="15" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Url:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtURL" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="16"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5 runat="server" id="Label11" visible="false">Estado:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="chkEstado_CheckedChanged" Visible="False" CssClass="form-control" TabIndex="17" />
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
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="18" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="19" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
