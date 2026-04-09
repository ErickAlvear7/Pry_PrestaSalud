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
            <asp:ToolkitScriptManager 
                ID="ToolkitScriptManager2"
                runat="server"
                AsyncPostBackTimeout="600" />
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; right:0; bottom:0; background:rgba(0,0,0,.35); z-index:9999;">
                <div style="position:absolute; top:50%; left:50%; transform:translate(-50%,-50%); background:#fff; padding:20px; border-radius:8px; text-align:center;">
                    <h3>Generando...</h3>
                    <img src="../Images/load.gif" alt="Loading" />
                </div>
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
                                <td><h5>Cliente:</h5></td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlClienteNova" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="1" />
                                </td>
                                <td></td>
                            </tr>
                        <%--    <tr>
                                <td></td>
                                <td><h5>Fecha Registro:</h5></td>
                                <td>
                                    <asp:TextBox ID="txtFechaInicio" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="2" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaFinal" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="3" />
                                </td>
                                <td></td>
                            </tr>--%>
                            <tr>
                                <td></td>
                                <td><h5>Filtrar por:</h5></td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlTipoFecha" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="2">
                                        <asp:ListItem Text="Fecha Registro" Value="R" Selected="True" />
                                        <asp:ListItem Text="Fecha Cita" Value="C" />
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td><h5>Rango de fechas:</h5></td>
                                <td>
                                    <asp:TextBox ID="txtFechaInicio" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="3" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaFinal" runat="server"
                                        CssClass="form-control" Width="100%" TabIndex="4" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td style="text-align:center"><h5>Desde</h5></td>
                                <td style="text-align:center"><h5>Hasta</h5></td>
                                <td></td>
                            </tr>
                        </table>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnProcesar" runat="server"
                                            Text="Procesar"
                                            Width="120px"
                                            CssClass="button"
                                            OnClick="btnProcesar_Click"
                                            OnClientClick="document.getElementById('loadingOverlay').style.display='block';"
                                            UseSubmitBehavior="false"
                                            TabIndex="4" />
                                    </td>

                                    <td style="width:10%"></td>

                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server"
                                            Text="Salir"
                                            Width="120px"
                                            CssClass="button"
                                            CausesValidation="False"
                                            OnClick="btnSalir_Click"
                                            TabIndex="5" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel panel-default">
                            <table style="width: 100%">
                                <tr>
                                    <td><h5 id="totalreg" runat="server"></h5></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="imgExportar" runat="server"
                                            ImageUrl="~/Images/excel.png"
                                            Width="40px"
                                            Height="30px"
                                            Visible="false"
                                            OnClick="imgExportar_Click"
                                            TabIndex="6" />
                                        <asp:Label ID="lblExportar" runat="server"
                                            Text="Exportar"
                                            Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="panel-info">
                            <div style="overflow: scroll; width: 1024px; height: 300px">
                               <asp:GridView ID="grdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" AllowPaging="True" PageSize="50" OnPageIndexChanging="grdvDatos_PageIndexChanging" TabIndex="7">
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
                                           <asp:BoundField DataField="FechaCancelacion" HeaderText="Fecha_Cancelación" />
                                           <%-- <asp:BoundField DataField="Pvp" HeaderText="Pvp" />
                                            <asp:BoundField DataField="Costo" HeaderText="Costo" />
                                            <asp:BoundField DataField="IdBeneficiario" HeaderText="IdBeneficiario" />--%>
                                            <%--<asp:BoundField DataField="ObservaG" HeaderText="ObservaG">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>--%>
                                            <%--<asp:BoundField DataField="ProvinciaNaci" HeaderText="ProvinciaNacimiento" />--%>
                                            <asp:BoundField DataField="FechaNacimiento" HeaderText="FechaNacimiento" />
                                            <asp:BoundField DataField="TipoAgendamiento" HeaderText="Agendamiento" />
                                            <asp:BoundField DataField="Procedimiento" HeaderText="Procedimiento" />
                                            <asp:BoundField DataField="Pieza" HeaderText="Pieza" />
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        </Columns>
                                        <RowStyle Font-Size="XX-Small" />
                                    </asp:GridView>
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnProcesar" />
                        <asp:PostBackTrigger ControlID="imgExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
