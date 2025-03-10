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
        //function pageLoad(sender, arg) {
        //    $(document).ready(function () {
        //        $.datepicker.setDefaults($.datepicker.regional['es']);
        //        $('#txtFechaIni').datepicker(
        //            {
        //                inline: true,
        //                dateFormat: "yy-mm-dd",
        //                monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        //                monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        //                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        //                numberOfMonths: 1,
        //                showButtonPanel: true,
        //                changeMonth: true,
        //                changeYear: true,
        //                yearRange: "-100:+5"
        //            });
        //    });

        //}

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server" CssClass="text-center">MEDILINK</asp:Label>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
        <div class="panel-body">
            <asp:UpdatePanel ID="updCombos" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlOpciones" runat="server" GroupingText="" Height="140px" ScrollBars="Vertical" Visible="False">
                        <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CITA MEDICA</h3>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                            </tr>
                            <tr>
                                <td>
                                    <h5>Ciudad:</h5>
                               </td>
                                <td>
                                     <asp:DropDownList ID="ddlciudad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlciudad_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                   <h5>Sucursal:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSucursal" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <h5>Especialidad:</h5> 
                                </td>
                                <td>
	                                 <asp:DropDownList ID="ddlEspecialidad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlespeci_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel ID="upcCalendario" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel3" runat="server" GroupingText="" Height="170px" ScrollBars="Vertical" >
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Calendar ID="Calendar" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="139px" Width="91%" Visible="False" OnSelectionChanged="Calendar_SelectionChanged">
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
                                <td></td>
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
                    <asp:Panel ID="pnlDatosMedicos" runat="server" GroupingText="" Height="170px" ScrollBars="Vertical" >
                        <h3 id="lblHorarios" runat="server" class="label label-primary" style="font-size: 14px; display: block; text-align: left" visible="false">MEDICOS - HORARIOS DISPONIBLES</h3>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:ListBox ID="lstBoxMedicos" runat="server" AutoPostBack="true" Height="106px" Width="374px" OnSelectedIndexChanged="lstBoxMedicos_SelectedIndexChanged" Visible="False"></asp:ListBox>
                                </td>
                                <td></td>
                                <td></td>
                                <td>
                                   <asp:ListBox ID="LstBoxHorario" runat="server" AutoPostBack="true" Height="106px" Width="374px" OnSelectedIndexChanged="lstBoxHorasMedicos_SelectedIndexChanged" Visible="False"></asp:ListBox>
                                </td>
                                 <td></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
         <div class="panel-body">
             <asp:UpdatePanel ID="updAgendar" runat="server" Visible="true">
                  <ContentTemplate>
                      <asp:Panel ID="Panel1" runat="server" GroupingText="" Height="60px" ScrollBars="Vertical">
                           <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 95%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCrearCita" runat="server" Text="Agendar" CssClass="button" CausesValidation="false" Visible="False" OnClick="btnAgendar_Click" />
                                    </td>
                                    <td></td>
                                </tr>
                           </table>
                      </asp:Panel>
                  </ContentTemplate>
             </asp:UpdatePanel>
         </div>
         <div class="panel-body">
             <asp:UpdatePanel ID="updDetalleCita" runat="server" Visible="true">
                 <ContentTemplate>
                      <asp:Panel ID="Panel2" runat="server" GroupingText="" Height="280px" ScrollBars="Vertical">
                           <h3 id="txtTitCita" runat="server" class="label label-primary" style="font-size: 14px; display: block; text-align: left" visible="false">DETALLE CITA</h3>
                           <table style="width: 100%">
                               <tr>
                                    <td style="width: 10%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 40%"></td>
                                </tr>
                                 <tr>
                                    <td>
                                         <h5 id="txtCodCita" runat="server" class="label label-primary" Visible="false">Codigo Cita:</h5>
                                    </td>
                                     <td>
                                         <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                                     </td>
                                     <td></td>
                                     <td></td>
                                     <td></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <h5 id="txtCiudad" runat="server" class="label label-primary" Visible="false">Ciudad Cita:</h5>
                                    </td>
                                      <td>
                                         <asp:Label ID="lblCiudad" runat="server"></asp:Label>
                                     </td>
                                     <td></td>
                                     <td>
                                          <h5 id="txtFecha" runat="server" class="label label-primary" Visible="false">Fecha Cita:</h5>
                                     </td>
                                     <td>
                                          <asp:Label ID="lblFecha" runat="server"></asp:Label>
                                     </td>
                                 </tr>
                               <tr>
                                   <td>
                                       <h5 id="txtHora" runat="server" class="label label-primary" Visible="false">Hora Cita:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblHora" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtPrestador" runat="server" class="label label-primary" Visible="false">Prestador:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblPrestador" runat="server"></asp:Label>
                                   </td>
                               </tr>
                               <tr>
                                    <td>
                                       <h5 id="txtMedico" runat="server" class="label label-primary" Visible="false">Medico:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblMedico" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtEspe" runat="server" class="label label-primary" Visible="false">Especialidad:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblEspe" runat="server"></asp:Label>
                                   </td>
                               </tr>
                                <tr>
                                    <td>
                                       <h5 id="txtObservacion" runat="server" class="label label-primary" Visible="false">Observacion:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblObser" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtCedula" runat="server" class="label label-primary" Visible="false">Cedula:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblCedula" runat="server"></asp:Label>
                                   </td>
                               </tr>
                                <tr>
                                    <td>
                                       <h5 id="txtTipo" runat="server" class="label label-primary" Visible="false">Tipo:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblTipo" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtPaciente" runat="server" class="label label-primary" Visible="false">Paciente:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblPaciente" runat="server"></asp:Label>
                                   </td>
                               </tr>
                                    <tr>
                                    <td>
                                       <h5 id="txtFechaNaci" runat="server" class="label label-primary" Visible="false">Fecha Nacimiento:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblFechaNaci" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtDireccion" runat="server" class="label label-primary" Visible="false">Direc.Prestador:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblDireccion" runat="server"></asp:Label>
                                   </td>
                               </tr>
                                <tr>
                                    <td>
                                       <h5 id="txtTelefono" runat="server" class="label label-primary" Visible="false">Telefono:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblTelefono" runat="server"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td>
                                       <h5 id="txtOb" runat="server" class="label label-primary" Visible="false">Observacion:</h5>
                                   </td>
                                   <td>
                                         <asp:Label ID="lblOb" runat="server"></asp:Label>
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
