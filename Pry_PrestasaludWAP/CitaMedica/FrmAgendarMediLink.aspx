﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAgendarMediLink.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmAgendarMediLink" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Agendar Cita Medilink</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />    
    <link href="../css/Estilos.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/DatePicker/jquery-ui.js"></script>
   <%-- <script type="text/javascript">
        function pageLoad(sender, arg)
        {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFecha').datepicker(
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

        }--%>
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 36px;
        }
    </style>
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
                                <td style="width: 10%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 60%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Documento:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblDocumento" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                <td>
                                     <h5><strong>Consultar:</strong></h5>
                                </td>
                                <td>
                                    <asp:Button ID="btnConsul" runat="server" Width="30%" CausesValidation="False" CssClass="button" TabIndex="30" OnClick="btnConsul_Click" Text="Consultar" />
                                </td>
                                <td></td>
                            </tr>
                      </table>
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
        <div class="panel-body">
            <asp:UpdatePanel ID="updPaciente" runat="server">
                <ContentTemplate>
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
                            <td></td>
                            <td>
                                 <h5><strong>Nombre 1:</strong></h5>
                            </td>
                             <td>
                                 <asp:TextBox ID="txtNombre1" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                             </td>
                            <td></td>
                            <td>
                                 <h5><strong>Nombre 2:</strong></h5>
                            </td>
                             <td>
                                 <asp:TextBox ID="txtNombre2" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                            </td>
                             <td></td>
                        </tr>
                         <tr>
                            <td></td>
                            <td>
                                 <h5><strong>Apellido 1:</strong></h5>
                            </td>
                             <td>
                                 <asp:TextBox ID="txtApellido1" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                             </td>
                            <td></td>
                            <td>
                                 <h5><strong>Apellido 2:</strong></h5>
                            </td>
                             <td>
                                 <asp:TextBox ID="txtApellido2" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
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
                                <asp:Button ID="Button1" runat="server"  CssClass="button" OnClick="Button1_Click" Text="Registrar" Width="148px" />
                            </td>
                            <td class="auto-style1"></td>
                            <td class="auto-style1"></td>
                            <td class="auto-style1"></td>
                            <td class="auto-style1"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel ID="updCombos" runat="server">
                <ContentTemplate>
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
			                        <asp:DropDownList ID="ddlSucursal" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%"></asp:DropDownList>
	                          </td>
	                          <td></td>
                          </tr>
                          <tr>
	                          <td></td>
	                          <td>
			                        <h5><strong>Especialidad:</strong></h5>
	                          </td>
	                          <td>
			                        <asp:DropDownList ID="ddlEspecialidad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlciudad_SelectedIndexChanged"></asp:DropDownList>
	                          </td>
	                          <td></td>
	                           <td>
			                        <h5><strong>Medicos:</strong></h5>
	                          </td>
	                          <td>
			                        <asp:DropDownList ID="ddlMedicos" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%"></asp:DropDownList>
	                          </td>
	                          <td></td>
                          </tr>

                      </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
