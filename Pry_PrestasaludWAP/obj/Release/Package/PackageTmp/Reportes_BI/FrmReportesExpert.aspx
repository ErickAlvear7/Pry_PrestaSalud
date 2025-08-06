<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reportes_BI_FrmReportesExpert" CodeBehind="FrmReportesExpert.aspx.cs" %>

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
    <%--<script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>--%>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>

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

        var windowWith = (parseInt($(window).width()) / 2) - 200;
        $('divClass').css(
            {
                'width': windowWith, 'heigth': "350px"
            });

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
            <hr />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%"></td>
                        <td style="width: 50%">
                            <asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <div id="divProgress" style="text-align: center">
                                        <h2>Generando...Espere un momento!...</h2>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/loadingbar.gif" Height="70px" Width="153px" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>

                        </td>
                        <td style="width: 25%"></td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="panel-body">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 8%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Seleccione Producto:</h5>
                                    <%--<asp:Label ID="Label1" runat="server" CssClass="Label" Text="Seleccione Producto:"></asp:Label>--%>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlCampain" runat="server" Width="100%" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <button id="btnAgregar" runat="server" type="submit" class="btn btn-primary"
                                        onserverclick="btnAgregar_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Producto(s) Seleccionado(s)</h5>
                                </td>
                                <td colspan="4">
                                    <asp:ListBox ID="lstProdSelec" runat="server" Width="100%" Height="150px"></asp:ListBox>
                                </td>
                                <td>
                                    <button id="btnQuitar" runat="server" type="submit" visible="false" class="btn btn-primary"
                                        onserverclick="btnQuitar_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-minus"></span>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fecha Registro:</h5>
                                    <%--<asp:Label ID="Label2" runat="server" CssClass="Label" Text="Fecha Registro:"></asp:Label>--%>
                                </td>
                                <td>
                                    <h5>Desde:</h5>
                                    <%--<asp:Label ID="Label3" runat="server" CssClass="Label" Text="Desde:"></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td style="text-align: center">
                                    <h5>Hasta:</h5>
                                    <%--<asp:Label ID="Label4" runat="server" CssClass="Label" Text="Hasta:"></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaFin" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Cliente:</h5>
                                    <%--<asp:Label ID="Label7" runat="server" CssClass="Label" Text="Tipo Cliente:"></asp:Label>--%>
                                </td>
                                <td colspan="4">

                                    <asp:DropDownList ID="ddlTipoCliente" runat="server" Width="100%" CssClass="form-control">
                                        <asp:ListItem>TITULAR</asp:ListItem>
                                        <asp:ListItem>BENEFICIARIO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Movimiento:</h5>
                                    <%--<asp:Label ID="Label5" runat="server" CssClass="Label" Text="Tipo Movimiento:"></asp:Label>--%>
                                </td>
                                <td colspan="4">

                                    <asp:DropDownList ID="ddlMovimiento" runat="server" Width="100%" CssClass="form-control">
                                        <asp:ListItem>AGENDADAS</asp:ListItem>
                                        <asp:ListItem>CANCELADAS</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1">
                                    <h5>Tipo Reporte:</h5>
                                    <%--<asp:Label ID="Label8" runat="server" CssClass="Label" Text="Tipo Reporte"></asp:Label>--%>
                                </td>
                                <td class="auto-style1">
                                    <asp:RadioButton ID="rdbReporFTP" runat="server" Checked="True" CssClass="form-control" GroupName="Opcion" Text="FTP" Width="100%" />
                                </td>
                                <td class="auto-style1">
                                    <asp:RadioButton ID="rdbReporABF" runat="server" GroupName="Opcion" Text="ABF" CssClass="form-control" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Panel ID="pnlSep1" runat="server" Height="30px">
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button ID="btnProcesar" runat="server" Text="PROCESAR" Width="130px" OnClick="btnProcesar_Click" CssClass="button" />
                                </td>

                                <td colspan="2" style="text-align: left">
                                    <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="130px" OnClick="btnSalir_Click" CssClass="button" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:ImageButton ID="imgExportar" runat="server" ImageUrl="~/Images/excel.png" OnClick="imgExportar_Click" ToolTip="Exportar a Excel" Visible="False" Width="40px" />
                                    <asp:Label ID="Label6" runat="server" CssClass="Label" Text="Exportar Excel" Visible="False"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div class="panel-info">
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="width: 70%">
                                    <div style="overflow: scroll; width: 800px; height: 300px" runat="server" id="divBeneficiario" visible="false">
                                        <asp:GridView ID="grdvDatos" runat="server" Width="100%"
                                            AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" OnPageIndexChanging="grdvDatos_PageIndexChanging">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Tipo_Proceso" HeaderText="Tipo_Proceso" />
                                                <asp:BoundField DataField="Clave_Cliente" HeaderText="Clave_Cliente" />
                                                <asp:BoundField DataField="Clave_Contrato" HeaderText="Clave_Contrato" />
                                                <asp:BoundField DataField="Num_Nomina" HeaderText="Num_Nomina" />
                                                <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" />
                                                <asp:BoundField DataField="FecIni_Cobertura" HeaderText="FecIni_Cobertura" />
                                                <asp:BoundField DataField="FecFin_Cobertura" HeaderText="FecFin_Cobertura" />
                                                <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                                <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                                <asp:BoundField DataField="Nombre1" HeaderText="Nombre1" />
                                                <asp:BoundField DataField="Nombre2" HeaderText="Nombre2" />
                                                <asp:BoundField DataField="Ape1" HeaderText="Ape1" />
                                                <asp:BoundField DataField="Ape2" HeaderText="Ape2" />
                                                <asp:BoundField DataField="Fecha_Nacimiento" HeaderText="Fecha_Nacimiento" />
                                                <asp:BoundField DataField="Codigo_Genero" HeaderText="Codigo_Genero" />
                                                <asp:BoundField DataField="Cod_Postal" HeaderText="Cod_Postal" />
                                                <asp:BoundField DataField="Tipo_Negocio" HeaderText="Tipo_Negocio" />
                                                <asp:BoundField DataField="IdUnico" HeaderText="IdUnico" />
                                                <asp:BoundField DataField="Tipo_Proceso" HeaderText="Tipo_Proceso" />
                                                <asp:BoundField DataField="Clave_Cliente" HeaderText="Clave_Cliente" />
                                                <asp:BoundField DataField="Clave_Contrato" HeaderText="Clave_Contrato" />
                                                <asp:BoundField DataField="Num_Nomina" HeaderText="Num_Nomina" />
                                                <asp:BoundField DataField="FecIni_Cobertura" HeaderText="FecIni_Cobertura" />
                                                <asp:BoundField DataField="FecFin_Cobertura" HeaderText="FecFin_Cobertura" />
                                                <asp:BoundField DataField="Codigo_Plan" HeaderText="Codigo_Plan" />
                                                <asp:BoundField DataField="Desc_Plan" HeaderText="Desc_Plan" />
                                                <asp:BoundField DataField="Cedula1" HeaderText="Cedula" />
                                                <asp:BoundField DataField="Nombre3" HeaderText="Nombre1" />
                                                <asp:BoundField DataField="Nombre4" HeaderText="Nombre2" />
                                                <asp:BoundField DataField="Ape3" HeaderText="Ape1" />
                                                <asp:BoundField DataField="Ape4" HeaderText="Ape2" />
                                                <asp:BoundField DataField="Fecha_Nacimiento1" HeaderText="Fecha_Nacimiento" />
                                                <asp:BoundField DataField="Codigo_Genero1" HeaderText="Codigo_Genero" />
                                                <asp:BoundField DataField="Cod_Postal" HeaderText="Cod_Postal" />
                                                <asp:BoundField DataField="Tipo_Negocio" HeaderText="Tipo_Negocio" />
                                                <asp:BoundField DataField="Contratante" HeaderText="Contratante" />
                                                <asp:BoundField DataField="IdUnico" HeaderText="IdUnico" />
                                                <asp:BoundField DataField="Tipo_Identificacion" HeaderText="Tipo_Identificacion" />
                                            </Columns>
                                            <RowStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div style="overflow: scroll; width: 800px; height: 300px" runat="server" id="divTitular" visible="false">
                                        <asp:GridView ID="grdvDatos1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" EmptyDataText="No existen datos para mostrar" OnPageIndexChanging="grdvDatos1_PageIndexChanging">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Tipo_Proceso" HeaderText="Tipo_Proceso" />
                                                <asp:BoundField DataField="Clave_Cliente" HeaderText="Clave_Cliente" />
                                                <asp:BoundField DataField="Clave_Contrato" HeaderText="Clave_Contrato" />
                                                <asp:BoundField DataField="Num_Nomina" HeaderText="Num_Nomina" />
                                                <asp:BoundField DataField="FecIni_Cobertura" HeaderText="FecIni_Cobertura" />
                                                <asp:BoundField DataField="FecFin_Cobertura" HeaderText="FecFin_Cobertura" />
                                                <asp:BoundField DataField="Codigo_Plan" HeaderText="Codigo_Plan" />
                                                <asp:BoundField DataField="Desc_Plan" HeaderText="Desc_Plan" />
                                                <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                                                <asp:BoundField DataField="Nombre1" HeaderText="Nombre1" />
                                                <asp:BoundField DataField="Nombre2" HeaderText="Nombre2" />
                                                <asp:BoundField DataField="Ape1" HeaderText="Ape1" />
                                                <asp:BoundField DataField="Ape2" HeaderText="Ape2" />
                                                <asp:BoundField DataField="Fecha_Nacimiento" HeaderText="Fecha_Nacimiento" />
                                                <asp:BoundField DataField="Codigo_Genero" HeaderText="Codigo_Genero" />
                                                <asp:BoundField DataField="Cod_Postal" HeaderText="Cod_Postal" />
                                                <asp:BoundField DataField="Tipo_Negocio" HeaderText="Tipo_Negocio" />
                                                <asp:BoundField DataField="Contratante" HeaderText="Contratante" />
                                                <asp:BoundField DataField="IdUnico" HeaderText="IdUnico" />
                                                <asp:BoundField DataField="Tipo_Identificacion" HeaderText="Tipo_Identificacion" />
                                            </Columns>
                                            <RowStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
