<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoMedicoOdon.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmNuevoMedicoOdon" %>

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

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/jquery.ui.accordion.css" rel="stylesheet" />

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
    <script>
        $(function () {
            $("#acordion").accordion();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
                <div id="acordion">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Datos del Médico</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%; height: 100%">
                                <tr>
                                    <td style="width: 10%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 30%"></td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Nombres:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Apellidos:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control upperCase" MaxLength="50" Width="100%" TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Género:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-control" Width="100%" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Dirección:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDireccion" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="250" TabIndex="4"></asp:TextBox>
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
                                        <h5>Celular 1:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular1" runat="server" Width="100%" MaxLength="10" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtCelular1_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular1">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                    <td>
                                        <h5>Celular 2:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular2" runat="server" CssClass="form-control" MaxLength="10" Width="100%" TabIndex="8"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtCelular2_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular2">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>E-mail 1:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail1" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar1" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar1_CheckedChanged" Text="Enviar" TabIndex="10" />
                                    </td>
                                    <td>
                                        <h5>E-mail 2:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail2" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="11"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar2" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar2_CheckedChanged" Text="Enviar" TabIndex="12" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5 runat="server" id="Label11" visible="false">Estado:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="chkEstado_CheckedChanged" Visible="False" CssClass="form-control" TabIndex="13" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Asignar Especialdad/Prestadora</h3>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td style="width: 15%">
                                            <h5>Prestadora/Clínica:</h5>
                                        </td>
                                        <td style="width: 40%">
                                            <asp:DropDownList ID="ddlPrestadora" runat="server" Width="100%" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPrestadora_SelectedIndexChanged" TabIndex="14">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 10%"></td>
                                        <td style="width: 10%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Especialidad:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" Width="100%" TabIndex="15">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="16" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="17" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="MediCod,PreeCodigo,Codigo" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="18">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="Prestadora" HeaderText="Prestadora" />
                                                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Estado">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEstadoMed" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstadoMed_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Etiqueta">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle Font-Size="X-Small" />
                                            </asp:GridView>

                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="19" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="20" />
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
