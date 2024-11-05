<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevaCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmNuevaCitaMedica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
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
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>

    <script>
        $(function () {
            $("#acordionParametro").accordion();
        });
    </script>

    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
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
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CITA MEDICA</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%">
                                            <h5>Nombres:</h5>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            <h5>
                                            Apellidos:</td>
                                        <td style="width: 30%;">
                                            <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Genero:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGenero" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Estado Civil:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEstadoCivil" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><h5>Provincia:</h5></td>
                                        <td>
                                            <asp:DropDownList ID="ddlProvincia" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td><h5>Ciudad:</h5></td>
                                        <td>
                                            <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><h5>Prestadora/Clinica:</h5></td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlPrestadora" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlPrestadora_SelectedIndexChanged" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Medico:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <h5>Turno:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Especialidad:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Agendar a:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlAgendar" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlAgendar_SelectedIndexChanged">
                                                <asp:ListItem Value="N">--Seleccione Agendar a--</asp:ListItem>
                                                <asp:ListItem Value="T">TITULAR</asp:ListItem>
                                                <asp:ListItem Value="B">BENEFICIARIO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="lblBeneficiario" visible="false">Beneficiario:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlBeneficiario" runat="server" CssClass="form-control" Width="100%" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlEspacio" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Calendar ID="CalendarioCita" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="212px" Width="266px" OnSelectionChanged="CalendarioCita_SelectionChanged">
                                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                <NextPrevStyle VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#808080" />
                                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                <SelectorStyle BackColor="#CCCCCC" />
                                                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <WeekendDayStyle BackColor="#FFFFCC" />
                                            </asp:Calendar>
                                        </td>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlAgendamientos" runat="server" Height="250px" GroupingText="Agendar" ScrollBars="Vertical">
                                                <asp:GridView ID="grdvDatosCitas" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="HoraInicio,HoraFinal" OnRowDataBound="grdvDatosCitas_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="AGENDAR">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="imgSelecc_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Horario" HeaderText="HORA" />
                                                        <asp:TemplateField HeaderText="AGENDADO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgendado" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CLIENTE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCliente" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TIPO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTipo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PARENTESCO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblParentesco" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlEspacio1" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="pnlHorarios" runat="server" GroupingText="Horarios" Height="250px" ScrollBars="Vertical">
                                                <asp:GridView ID="grdvDatosHoras" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Dia" HeaderText="Dia" />
                                                        <asp:BoundField DataField="HoraIni" HeaderText="HoraInicio">
                                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HoraFin" HeaderText="HoraFinal">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <RowStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlResumen" runat="server" Height="280px" GroupingText="Resumen Cita">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 5%"></td>
                                                        <td style="width: 15%"><h5>Tipo Registro:</h5></td>
                                                        <td style="width: 65%" colspan="2">
                                                            <asp:DropDownList ID="ddlCiudad0" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" Width="100%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td><h5>Dia/Hora Cita:</h5></td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtDiaHora" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <h5>Médico Cita:</h5>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtMedico" runat="server" CssClass="form-control upperCase" Width="100%" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <h5>Detalle Cita:</h5>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtDetalleCita" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td colspan="2">
                                                            <asp:Panel ID="pnlEspacio2" runat="server" Height="20px"></asp:Panel>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td style="text-align: center; margin-left: 40px;">
                                                            <asp:ImageButton ID="imgAgendar" runat="server" Height="35px" ImageUrl="~/Botones/agendarmail.png" OnClick="imgAgendar_Click" />
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:ImageButton ID="imgCancelar" runat="server" Height="35px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" />
                                                        </td>                                                     
                                                    </tr>

                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4"></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">HISTORIAL CITAS MEDICAS</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
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
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlBeneficiarios" runat="server" Height="250px" ScrollBars="Vertical" GroupingText="Beneficarios">
                                                <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="CodigoBen" ShowHeaderWhenEmpty="True" Width="100%">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombres" HeaderText="NOMBRES" />
                                                        <asp:BoundField DataField="Apellidos" HeaderText="APELLIDOS">
                                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Parentesco" HeaderText="PARENTESCO">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" />
                                                    </Columns>
                                                    <RowStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
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
                                    <td style="text-align: right; width: 50%">
                                        <%--<asp:Button ID="btnGrabar" runat="server" Text="GRABAR" Width="120px" CssClass="button" />--%>
                                    </td>
                                    <td style="text-align: left; width: 50%">
                                        <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" />
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
