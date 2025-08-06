<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_RepGerencialV2.aspx.cs" Inherits="Pry_PrestasaludWAP.Reportes.Frm_RepGerencialV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaDesde').datepicker(
                  {
                      //showOn: "both",
                      inline: true,
                      dateFormat: "mm/dd/yy",
                      monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                      monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                      dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                      numberOfMonths: 2,
                      showButtonPanel: true,
                      changeMonth: true,
                      changeYear: true,
                      yearRange: "-100:+5"


                  });

                $('#txtFechaHasta').datepicker(
                  {
                      inline: true,
                      dateFormat: "mm/dd/yy",
                      monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                      monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                      dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                      numberOfMonths: 2,
                      showButtonPanel: true,
                      changeMonth: true,
                      changeYear: true,
                      yearRange: "-100:+5"
                  });
            });
        }
    </script>
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
                <asp:Panel ID="pnlDivision0" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 15%">
                                <h5>Fecha Registro:</h5>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TabIndex="9" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 15%"></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TabIndex="9" Width="100%"></asp:TextBox>
                            </td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <h5>Tipo Agenda:</h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoAgenda" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                    <asp:ListItem Value="0">--Tipo Agenda--</asp:ListItem>
                                    <asp:ListItem Value="A">Agendadas</asp:ListItem>
                                    <asp:ListItem Value="C">Canceladas</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <h5>Tipo Cliente:</h5>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="form-control" Width="100%" TabIndex="4">
                                    <asp:ListItem Value="C">--Tipo Cliente--</asp:ListItem>
                                    <asp:ListItem Value="S">Todos</asp:ListItem>
                                    <asp:ListItem Value="T">Titulares</asp:ListItem>
                                    <asp:ListItem Value="B">Beneficiarios</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:UpdatePanel ID="updOpciones" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlOpciones" runat="server" GroupingText="Repote Gerencial Por" Height="100px" BorderStyle="Solid" BorderWidth="2px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 5%"></td>
                                                    <td style="width: 20%">
                                                        <asp:RadioButton ID="rdbHoras" runat="server" Font-Size="10pt" Text="Horas" AutoPostBack="True" OnCheckedChanged="rdbHoras_CheckedChanged" />
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:RadioButton ID="rdbCiudad" runat="server" Font-Size="10pt" Text="Ciudad" AutoPostBack="True" OnCheckedChanged="rdbCiudad_CheckedChanged" />
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:RadioButton ID="rdbPrestadora" runat="server" Font-Size="10pt" Text="Prestadora" AutoPostBack="True" OnCheckedChanged="rdbPrestadora_CheckedChanged" />
                                                    </td>
                                                    <td style="width: 20%">
                                                        <asp:RadioButton ID="rdbEspecialidad" runat="server" Font-Size="10pt" Text="Especialidad" AutoPostBack="True" OnCheckedChanged="rdbEspecialidad_CheckedChanged" />
                                                    </td>
                                                    <td style="width: 5%"></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbGenero" runat="server" Font-Size="10pt" Text="Genero" AutoPostBack="True" OnCheckedChanged="rdbGenero_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbProducto" runat="server" AutoPostBack="True" Font-Size="10pt" OnCheckedChanged="rdbProducto_CheckedChanged" Text="Producto" />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="4">
                                <asp:Panel ID="pnlDivision" runat="server" Height="20px">
                                </asp:Panel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: left"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 15%">

                            <asp:Button ID="btnProcesar" runat="server" CssClass="button" OnClick="btnProcesar_Click" TabIndex="10" Text="Procesar" Width="120px" />
                        </td>
                        <td style="width: 30%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 30%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 1%"></td>
                        <td style="width: 98%"></td>
                        <td style="width: 1%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="3">
                            <asp:Panel ID="pnlDivision1" runat="server" Height="20px">
                            </asp:Panel>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlFacturacion" runat="server" Height="510px" ScrollBars="Vertical" Visible="false">
                                <rsweb:ReportViewer ID="rptGerencial" runat="server" Width="912px" AsyncRendering="false" SizeToReportContent="true">
                                </rsweb:ReportViewer>
                            </asp:Panel>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="11" OnClick="btnSalir_Click" />
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

