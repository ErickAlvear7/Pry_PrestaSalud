<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAgendarCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmAgendarCitaMedica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<meta http-equiv="refresh" content="30"/>--%>
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

        .auto-style1 {
            width: 5%;
            height: 35px;
        }

        .auto-style2 {
            width: 15%;
            height: 35px;
        }

        .auto-style3 {
            width: 30%;
            height: 35px;
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
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCitaMedica">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Enviando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:UpdatePanel ID="updTimer" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="tmrdat" runat="server" OnTick="tmrdat_Tick">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="table-responsive">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS TITULAR</h3>
                <asp:UpdatePanel ID="updDatosPersonales" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="auto-style2"></td>
                                <td class="auto-style3"></td>
                                <td class="auto-style2"></td>
                                <td class="auto-style3">
                                    <h5 id="lblCelular" runat="server" visible="false"></h5>
                                </td>
                                <td class="auto-style1"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:LinkButton ID="lnkActualizar" runat="server" OnClick="lnkActualizar_Click">Actualizar Datos</asp:LinkButton>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="pnlDatosPersonales" runat="server" GroupingText="Datos Personales" Height="180px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatosPersonales" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" TabIndex="1" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo_Documento" />
                                                <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                                                <asp:BoundField DataField="Titular" HeaderText="Titular" />
                                                <asp:BoundField DataField="FonoCasa" HeaderText="Telf.Casa" />
                                                <asp:BoundField DataField="FonoOficina" HeaderText="Telf.Oficina" />
                                                <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
                                            <HeaderStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel ID="PnlDiv7" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="pnlInfAdicional" runat="server" GroupingText="Datos Adicionales" Height="180px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatosAdicionales" runat="server"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" TabIndex="2" Width="100%">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <RowStyle Font-Size="X-Small" />
                                            <HeaderStyle Font-Size="Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel-body">
                <div id="acordion">
                     <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CONTADOR DE CITAS</h3>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>
                             <div class="table-responsive">
                                 <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                     <tr>
                                         <td></td>
                                         <td>
                                             <h5>Medicina General</h5>
                                             <asp:Label ID="Label1" runat="server"></asp:Label>
                                         </td>
                                            
                                             <h5>Especialidades</h5>
                                             <asp:Label ID="Label2" runat="server"></asp:Label>
                                         </td>
                                          </td>
                                            
                                             <h5>Laboratorio</h5>
                                             <asp:Label ID="Label3" runat="server"></asp:Label>
                                         </td>
                                     </tr>
                                 </table>
                             </div>

                         </ContentTemplate>
                     </asp:UpdatePanel>
                </div>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CITA MEDICA</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <asp:Panel ID="pnlListaDatos" runat="server" GroupingText="Datos Titulares - Beneficiarios" TabIndex="3">
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

                                                <asp:GridView ID="grdvTitulares" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="TituCodigo,BeneCodigo,CodTipo,CodParentesco" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="4">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Edad" HeaderText="Edad"></asp:BoundField>
                                                        <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                                                        <asp:BoundField DataField="UltimaCita" HeaderText="Ult. Cita" />
                                                        <asp:TemplateField HeaderText="Seleccionar">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSeleccionar" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleccionar_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="Small" />
                                                </asp:GridView>

                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlOpcionesCita" runat="server" GroupingText="Opciones Cita" TabIndex="5">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 5%"></td>
                                            <td style="width: 15%"></td>
                                            <td style="width: 35%"></td>
                                            <td style="width: 10%"></td>
                                            <td style="width: 30%"></td>
                                            <td style="width: 5%"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Provincia:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlProvincia" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" TabIndex="6">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <h5 style="text-align: center">Ciudad:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" TabIndex="7">
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Prestadora/Clinica:</h5>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlPrestadora" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlPrestadora_SelectedIndexChanged" TabIndex="8">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgPrestadora" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="imgPrestadora_Click" ToolTip="Ver Horarios" TabIndex="9" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Especialidad:</h5>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged" TabIndex="10">
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Medico:</h5>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged" TabIndex="11">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="imgHorarios" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" ToolTip="Ver Horarios" OnClick="imgHorarios_Click" TabIndex="12" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Registro:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOpcion" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlOpcion_SelectedIndexChanged" TabIndex="13">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <h5 style="text-align: center">Motivo:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMotivoCita" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="14">
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Tipo Pago:</h5>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoPago" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="15">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <h5>Obervación:</h5>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtObservacion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="250" TabIndex="16" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                    </table>
                                </asp:Panel>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 5%"></td>
                                        <td style="width: 70%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Calendar ID="CalendarioCita" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="166px" Width="100%" OnSelectionChanged="CalendarioCita_SelectionChanged" Visible="False" TabIndex="17">
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
                                        <td>
                                            <asp:Panel ID="pnlAgendamientos" runat="server" Height="250px" GroupingText="Agendar" ScrollBars="Vertical" Visible="False" TabIndex="18">
                                                <asp:GridView ID="grdvDatosCitas" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="HodeCodigo,TumeCodigo" OnRowDataBound="grdvDatosCitas_RowDataBound" TabIndex="19">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Agendar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="imgSelecc_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                                        <asp:BoundField DataField="Accion" HeaderText="Accion" />
                                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel ID="pnlEspacio1" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Observación General:</h5>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="txtObservacionG" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="500" TabIndex="20" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="TrFileUpload" visible="false">
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="LblDocumento">Documento Adjunto:</h5>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="21" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlResumenCita" runat="server" GroupingText="Resumen Cita" Visible="False" TabIndex="22">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 5%"></td>
                                            <td style="width: 45%"></td>
                                            <td style="width: 45%"></td>
                                            <td style="width: 5%"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="2">
                                                <asp:GridView ID="grdvResumenCita" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="PreeCodigo,MediCodigo,HodeCodigo,CodigoPrestadora" TabIndex="23">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Paciente" DataField="Cliente" />
                                                        <asp:BoundField HeaderText="Ciudad" DataField="Ciudad" />
                                                        <asp:BoundField HeaderText="Prestadora" DataField="Prestadora" />
                                                        <asp:TemplateField HeaderText="Ver">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgVer" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="imgVer_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Medico" DataField="Medico" />
                                                        <asp:BoundField HeaderText="Especialidad" DataField="Especialidad" />
                                                        <asp:BoundField HeaderText="Fecha" DataField="FechaCita" />
                                                        <asp:BoundField HeaderText="Hora" DataField="Hora" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgEliminar" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="imgEliminar_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:UpdatePanel ID="updCitaMedica" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgAgendar" runat="server" Height="25px" ImageUrl="~/Botones/agendarmail.png" OnClick="imgAgendar_Click" TabIndex="24" />
                                                </td>
                                                <td style="text-align: center; margin-left: 40px;">
                                                    <asp:ImageButton ID="imgCancelar" runat="server" Height="25px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="25" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="imgAgendar" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">HISTORIAL CITAS MEDICAS</h3>
                  <%--  <div class="panel-info">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updCancelarCita">
                            <ProgressTemplate>
                                <div class="overlay" />
                                <div class="overlayContent">
                                    <h2>Enviando..</h2>
                                    <img src="../Images/load.gif" alt="Loading" border="1" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>--%>
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
                                        <td colspan="6">
                                            <asp:Panel ID="pnlHistorialCitas" runat="server" Height="250px" ScrollBars="Vertical" GroupingText="">
                                                <asp:GridView ID="grdvHistorialCitas" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CitaCodigo,HodeCodigo,PrestaCodigo,Estado,Prestadora,Tipo,CodigoGenerado" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvHistorialCitas_RowDataBound" TabIndex="26">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Estado">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgEstado" runat="server" Height="22px" ImageUrl="~/Botones/mailagenda.png" OnClick="imgEstado_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Cliente" HeaderText="Paciente" />
                                                        <asp:TemplateField HeaderText="Prestadora">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBuscaPres" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="imgBuscaPres_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Medico" HeaderText="Medico" />
                                                        <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                                                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                        <asp:BoundField DataField="FechaCita" HeaderText="Fecha_Cita" />
                                                        <asp:BoundField DataField="HoraCita" HeaderText="Hora_Cita" />
                                                        <asp:TemplateField HeaderText="Cancelar">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelecc" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="table-responsive">
                                <asp:Panel ID="Panel1" runat="server" GroupingText="CANCELAR CITAS">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%"></td>
                                            <td style="width: 25%">
                                                <h5>Motivo Cancelación:</h5>
                                            </td>
                                            <td style="width: 45%">
                                                <asp:DropDownList ID="ddlMotivoCancelar" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="27">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 15%"></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:UpdatePanel ID="updCancelarCita" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 15%"></td>
                                                <td style="width: 25%">
                                                    <asp:ImageButton ID="imgCancel" runat="server" Height="25px" ImageUrl="~/Botones/agendarmail.png" OnClick="imgCancel_Click" TabIndex="28" />
                                                </td>
                                                <td style="width: 45%"></td>
                                                <td style="width: 15%"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
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
                                        <td colspan="6">
                                            <asp:Panel ID="pnlHitorialDetalle" runat="server" Height="250px" ScrollBars="Vertical" GroupingText="Historial Agenda">
                                                <asp:GridView ID="grdvHistorialDetalle" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="29">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="TipoAgenda" HeaderText="Tipo Agenda" />
                                                        <asp:BoundField DataField="CodigoGenerado" HeaderText="Codigo" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                        <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha_Registro" />
                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
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
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="30" />
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
