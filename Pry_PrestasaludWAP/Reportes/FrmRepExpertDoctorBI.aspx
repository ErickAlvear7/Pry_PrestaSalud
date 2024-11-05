<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRepExpertDoctorBI.aspx.cs" Inherits="Pry_PrestasaludWAP.Reportes.FrmRepExpertDoctorBI" %>

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
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaIni').datepicker(
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

            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaFin').datepicker(
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
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCabecera">
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
                <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fecha Carga:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 35%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <h5>Cliente:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCliente" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Producto:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-control" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Width="120px" CssClass="button" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="imgExportar" runat="server" ImageUrl="~/Images/excel.png" Width="40px" Height="30px" Visible="false" OnClick="imgExportar_Click" />
                                        <asp:Label ID="lblExportar" runat="server" Text="Exportar" Visible="false"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info">
                            <div style="overflow: scroll; width: 1024px; height: 300px" runat="server" id="divBeneficiario">
                                <asp:GridView ID="grdvDatos" runat="server" Width="100%"
                                    AutoGenerateColumns="False"
                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" PageSize="50" OnPageIndexChanging="grdvDatos_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Carga" />
                                        <asp:BoundField DataField="FechaCita" HeaderText="Cliente" />
                                        <asp:BoundField DataField="HoraCita" HeaderText="Producto" />
                                        <asp:BoundField DataField="CodigoCita" HeaderText="Titulares_Insert" />
                                        <asp:BoundField DataField="Cliente" HeaderText="Titulares_Update">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Producto" HeaderText="Beneficiarios_Insert">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Beneficiarios_Update" />
                                    </Columns>
                                    <RowStyle Font-Size="XX-Small" />
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
