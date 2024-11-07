<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_RepExpertDoctorNovaV1.aspx.cs" Inherits="Pry_PrestasaludWAP.Reportes.Frm_RepExpertDoctorNovaV1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet"/>
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>

    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaInicio').datepicker(
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
                $('#txtFechaFinal').datepicker(
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
            <div class="panel-heading" id="Aja">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
            </asp:ToolkitScriptManager>
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
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Cliente:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlClienteNova" runat="server" CssClass="form-control" Width="100%" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Fecha Registro:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" Width="100%" TabIndex="2"></asp:TextBox>
                                </td>
                                <td>

                                    <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="form-control" TabIndex="3" Width="100%"></asp:TextBox>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <h5 style="text-align: center">Desde</h5>
                                </td>
                                <td style="text-align: center">
                                    <h5>Hasta</h5>
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
                       <%--     <tr>
                                <td>
                                    <h5>Tipo Agenda:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoAgenda" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>Tipo Cliente:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="form-control" Width="100%" TabIndex="5">
                                        <asp:ListItem Value="0">--Todos--</asp:ListItem>
                                        <asp:ListItem Value="T">TITULAR</asp:ListItem>
                                        <asp:ListItem Value="B">BENEFICIARIO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>--%>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Width="120px" CssClass="button" OnClick="btnProcesar_Click" TabIndex="6" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="7" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:ImageButton ID="imgExportar" runat="server" ImageUrl="~/Images/excel.png" Width="40px" Height="30px" Visible="false" OnClick="imgExportar_Click" TabIndex="8" />
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
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" PageSize="50" OnPageIndexChanging="grdvDatos_PageIndexChanging" TabIndex="12">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Registro" />
                                        <asp:BoundField DataField="FechaCita" HeaderText="Fecha_Cita" />
                                        <asp:BoundField DataField="HoraCita" HeaderText="Hora_Cita" />
                                        <asp:BoundField DataField="CodigoCita" HeaderText="Codigo" />
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Producto" HeaderText="Producto">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                        <asp:BoundField DataField="Cedula" HeaderText="Identificación"></asp:BoundField>
                                        <asp:BoundField DataField="Telefono_Casa" HeaderText="Fono_Casa"></asp:BoundField>
                                        <asp:BoundField DataField="Telefono_Oficina" HeaderText="Fono_Oficina" />
                                        <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                        <asp:BoundField DataField="Paciente" HeaderText="Paciente">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Titular" HeaderText="Titular">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                        <asp:BoundField DataField="Genero" HeaderText="Genero" />
                                        <asp:BoundField DataField="TipoAgenda" HeaderText="Agenda">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MotivoAgenda" HeaderText="Motivo">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistroCIE10" HeaderText="RegistroCIE10" >
                                        <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Observacion" HeaderText="Observacion">
                                        <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Provincia" HeaderText="Provincia">
                                        <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Prestadora" HeaderText="Prestadora">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Especialidad" HeaderText="Especialidad">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Medico" HeaderText="Medico">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UsuarioAgenda" HeaderText="Usuario_Agenda" />
                                        <asp:BoundField DataField="UsuarioCancela" HeaderText="Usuario_Cancela" />
                                       <%-- <asp:BoundField DataField="FechaCancelacion" HeaderText="Fecha_Cancelación" />
                                        <asp:BoundField DataField="Pvp" HeaderText="Pvp" />
                                        <asp:BoundField DataField="Costo" HeaderText="Costo" />
                                        <asp:BoundField DataField="IdBeneficiario" HeaderText="IdBeneficiario" />--%>
                                        <asp:BoundField DataField="ObservaG" HeaderText="ObservaG">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="ProvinciaNaci" HeaderText="ProvinciaNacimiento" />--%>
                                        <asp:BoundField DataField="FechaNacimiento" HeaderText="FechaNacimiento" />

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
