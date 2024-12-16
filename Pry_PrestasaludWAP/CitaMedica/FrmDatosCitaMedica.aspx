<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDatosCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmDatosCitaMedica" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
         <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
         <title>Datos Cita</title>
        <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />    
        <link href="../css/Estilos.css" rel="stylesheet" />
    </head>
   
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                        </asp:ToolkitScriptManager>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>                        
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>    
                            <tr>
                                <td></td>
                                 <td>
                                    <h5><strong>Código Cita:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblCodCita" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Ciudad Cita:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblCiudad" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                 <td>
                                    <h5><strong>Fecha Cita:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblFechaCita" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Hora Cita:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblHoraCita" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Prestadora:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblPrestadora" runat="server" Text=""></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Medico:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblMedico" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Especialidad:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblEspecialidad" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                             </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Observacion:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblDetalle" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                <td>
                                    <h5><strong>Cedula Titular:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblCedula" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                             </tr>
                              <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Tipo:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblTipo" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                 <td>
                                    <h5><strong>Paciente:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblPaciente" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                             </tr>
                              <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Fecha Nacimiento:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblFechaNaci" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                 <td>
                                    <h5><strong>Direccion Prestador:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblDirec" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                             </tr>
                              <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Teléfonos:</strong></h5>
                                </td>
                                <td>
                                    <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                                 <td>
                                    <h5><strong>Observacion:</strong></h5>
                                </td>
                                 <td>
                                    <asp:Label ID="lblObserva" runat="server" Text=""></asp:Label> 
                                </td>
                                <td></td>
                             </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center">
                                    <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" />
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
