<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAgendarMediLink.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmAgendarMediLink" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Agendar Cita Medilink</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaIni').datepicker(
                    {
                        inline: true,
                        dateFormat: "yy-mm-dd",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
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
                <asp:Label ID="lbltitulo" runat="server" CssClass="text-center">MEDILINK</asp:Label>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel ID="updCabecera" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 10%"></td>
                            <td style="width: 5%"></td>
                            <td style="width: 5%"></td>
                            <td style="width: 30%"></td>
                            <td style="width: 5%"></td>
                            <td style="width: 5%"></td>
                        </tr>
                        <tr>
                            <td>
                                <h5 class="label label-primary">Documento:</h5>
                            </td>
                            <td>
                                <asp:Label ID="lblDocumento" runat="server" Text=""></asp:Label>
                            </td>
                            <td></td>
                            <td>
                                <h5 class="label label-primary">Nombres:</h5>
                            </td>
                            <td>
                                <asp:Label ID="lblNombresCompletos" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <h5 class="label label-primary">Registro:</h5>
                            </td>
                            <td>
                                <asp:Label ID="lblRegistro" CssClass="bg-success" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--        <div class="panel-body">
            <asp:UpdatePanel ID="updPaciente" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlPaciente" runat="server" GroupingText="Datos Personales" Height="280px" ScrollBars="Vertical" Visible="False">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%" aria-hidden="False"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td class="auto-style2"></td>
                                <td class="auto-style2">
                                    <h5><strong>Nombre 1:</strong></h5>
                                </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="txtNombre1" runat="server" CssClass="form-control" MaxLength="50" TabIndex="2" Width="100%"></asp:TextBox>
                                </td>
                                <td class="auto-style2"></td>
                                <td class="auto-style2">
                                    <h5><strong>Nombre 2:</strong></h5>
                                </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="txtNombre2" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td class="auto-style2"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Apellido 1:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApellido1" runat="server" CssClass="form-control" MaxLength="50" TabIndex="4" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Apellido 2:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApellido2" runat="server" CssClass="form-control" MaxLength="50" TabIndex="5" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Email:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Direccion:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Celular:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Fecha.Nacimiento:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1">
                                    <asp:Button ID="btnRegistrar" runat="server" CssClass="button" OnClick="btnRegistrar_Click" Text="Registrar" Width="148px" />
                                </td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                                <td class="auto-style1"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>
        <div class="panel-body">
            <asp:UpdatePanel ID="updCombos" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlOpciones" runat="server" GroupingText="" Height="180px" ScrollBars="Vertical" Visible="False">
                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CITA MEDICA</h3>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Ciudad:</strong></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlciudad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlciudad_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Sucursal:</strong></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSucursal" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Fecha Cita:</strong></h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" CssClass="form-control" Width="100%" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Especialidad:</strong></h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEspecialidad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlespeci_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>&nbsp;</td>
                                <td></td>
                                <td></td>
                                <td>
                                    <%--<h5><strong>Medicos:</strong></h5>--%>
                                </td>
                                <td>
                                    <%-- <asp:DropDownList ID="ddlMedicos" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlmedico_SelectedIndexChanged"></asp:DropDownList>--%>
                                </td>
                                <td></td>
                            </tr>

                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="panel-body">
            <asp:UpdatePanel ID="updMedico" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlDatosMedicos" runat="server" GroupingText="" Height="180px" ScrollBars="Vertical" >
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 45%">
                                </td>
                                <td style="width: 10%"></td>
                                <td style="width: 45%">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstBoxMedicos" runat="server" AutoPostBack="true" Height="106px" Width="374px" OnSelectedIndexChanged="lstBoxMedicos_SelectedIndexChanged"></asp:ListBox>
                                </td>
                                <td></td>
                                <td>
                                    <asp:ListBox ID="LstBoxHorario" runat="server" AutoPostBack="true" Height="106px" Width="374px" OnSelectedIndexChanged="lstBoxHorasMedicos_SelectedIndexChanged"></asp:ListBox>
                                </td>

                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
