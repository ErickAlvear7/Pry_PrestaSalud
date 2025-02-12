<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAgendarMediLink.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmAgendarMediLink" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Agendar Cita Medilink</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />    
    <link href="../css/Estilos.css" rel="stylesheet" />
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
                                <td style="width: 30%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
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
                                    <asp:Button ID="btnConsul" runat="server" Width="100%" CausesValidation="False" CssClass="button" TabIndex="30" OnClick="btnConsul_Click" Text="Consultar" />
                                </td>
                                <td></td>
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
                                    <h5><strong>Especialidad:</strong></h5>
                              </td>
                              <td>
                                    <asp:DropDownList ID="ddlEspecialidad" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%"></asp:DropDownList>
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
