<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptReporteFacturacion.aspx.cs" Inherits="Pry_PrestasaludWAP.Medicos.RptReporteFacturacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reporte Facturación</title>
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
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 20%">
                            <h5>Tipo Reporte:</h5>
                        </td>
                        <td style="width: 35%">
                            <asp:DropDownList ID="ddlTipoReporte" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="4" OnSelectedIndexChanged="ddlTipoReporte_SelectedIndexChanged">
                                <asp:ListItem Value="0">--Seleccione Reporte--</asp:ListItem>
                                <asp:ListItem Value="1">Facturación Proveedor</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 35%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <h5>Fecha Reporte</h5>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TabIndex="9" Width="100%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TabIndex="9" Width="100%"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td style="text-align: center">
                            <h5>Desde</h5>
                        </td>
                        <td style="text-align: center">
                            <h5>Hasta</h5>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td style="text-align: center">                            
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UpdOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnProcesar" runat="server" CssClass="button" OnClick="btnProcesar_Click" TabIndex="10" Text="Procesar" Width="120px" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
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
                                <td>
                                    <asp:Panel ID="pnlFacturacion" runat="server" Height="310px" ScrollBars="Vertical">
                                        <rsweb:ReportViewer ID="rptFacturacion" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="912px" AsyncRendering="false" SizeToReportContent="true">
                                            <LocalReport ReportPath="Reports\RptFacturaPrestaMedico.rdlc">
                                            </LocalReport>
                                        </rsweb:ReportViewer>
                                    </asp:Panel>
                                </td>
                                <td></td>
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
                                <td style="text-align: center">
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
