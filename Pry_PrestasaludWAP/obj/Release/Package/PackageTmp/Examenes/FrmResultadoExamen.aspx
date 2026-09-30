<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmResultadoExamen.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmResultadoExamen" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Carga de Resultados de Exámenes
            </div>
            <div class="panel-body">
                <asp:Label ID="Lblerror" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <div class="panel panel-info">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-3">
                            <strong>Solicitud:</strong>
                            <asp:Label ID="LblSolicitud" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Fecha Solicitud:</strong>
                            <asp:Label ID="LblFechaSolicitud" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Producto:</strong>
                            <asp:Label ID="LblProducto" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <strong>Estado:</strong>
                            <asp:Label ID="LblEstado" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">Pacientes</div>
                <div class="panel-body">
                    <asp:GridView ID="GrdvPacientes" runat="server" Width="100%" AutoGenerateColumns="False"
                        DataKeyNames="PERS_CODIGO,TITU_CODIGO,TIPO_PERSONA"
                        CssClass="table table-condensed table-bordered table-hover"
                        EmptyDataText="No existen pacientes para esta solicitud">
                        <Columns>
                            <asp:BoundField DataField="TIPO_PERSONA" HeaderText="Tipo" />
                            <asp:BoundField DataField="NUM_DOCUMENTO" HeaderText="No. Documento" />
                            <asp:BoundField DataField="PACIENTE" HeaderText="Paciente" />
                            <asp:BoundField DataField="ESTADO_RESULTADO" HeaderText="Estado Resultados" />
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="BtnCargar" runat="server" Text="Cargar Resultados"
                                        CssClass="btn btn-primary btn-xs" CausesValidation="false"
                                        OnClick="BtnCargar_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:Panel ID="PnlCarga" runat="server" Visible="false" CssClass="panel panel-info">
                <div class="panel-heading">
                    Cargar Resultados
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4">
                            <strong>Paciente:</strong>
                            <asp:Label ID="LblPacienteSeleccionado" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <strong>Documento:</strong>
                            <asp:Label ID="LblDocumentoSeleccionado" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <strong>Tipo:</strong>
                            <asp:Label ID="LblTipoPersonaSeleccionada" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <label>Resultado 1</label>
                            <asp:FileUpload ID="FileResultado1" runat="server" CssClass="form-control" />
                            <br />
                            <asp:Label ID="LblArchivo1" runat="server" Text="Sin archivo" ForeColor="Gray"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="BtnEliminarArchivo1" runat="server" Text="Eliminar"
                                CssClass="btn btn-danger btn-xs"
                                CausesValidation="false"
                                Visible="false"
                                CommandArgument="1"
                                OnClick="BtnEliminarArchivo_Click">
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                            <label>Resultado 2</label>
                            <asp:FileUpload ID="FileResultado2" runat="server" CssClass="form-control" />
                            <br />
                            <asp:Label ID="LblArchivo2" runat="server" Text="Sin archivo" ForeColor="Gray"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="BtnEliminarArchivo2" runat="server" Text="Eliminar"
                                CssClass="btn btn-danger btn-xs"
                                CausesValidation="false"
                                Visible="false"
                                CommandArgument="2"
                                OnClick="BtnEliminarArchivo_Click">
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                            <label>Resultado 3</label>
                            <asp:FileUpload ID="FileResultado3" runat="server" CssClass="form-control" />
                            <br />
                            <asp:Label ID="LblArchivo3" runat="server" Text="Sin archivo" ForeColor="Gray"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="BtnEliminarArchivo3" runat="server" Text="Eliminar"
                                CssClass="btn btn-danger btn-xs"
                                CausesValidation="false"
                                Visible="false"
                                CommandArgument="3"
                                OnClick="BtnEliminarArchivo_Click">
                             </asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <label>Observación</label>
                            <asp:TextBox ID="TxtObservacion" runat="server" TextMode="MultiLine" Rows="3" MaxLength="250" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div style="text-align: center;">
                        <asp:Button ID="BtnGuardarResultados" runat="server" Text="Guardar Resultados" CssClass="btn btn-success" CausesValidation="false" OnClick="BtnGuardarResultados_Click" />
                        &nbsp;
                        <asp:Button ID="BtnCancelarCarga" runat="server" Text="Cancelar" CssClass="btn btn-default" CausesValidation="false" OnClick="BtnCancelarCarga_Click" />
                    </div>
                </div>
            </asp:Panel>
            <div style="text-align: center; margin-top: 20px;">
                <asp:Button ID="BtnEnviarResultados" runat="server" Text="Faltan Resultados" CssClass="btn btn-default" Enabled="false"
                    CausesValidation="false"
                    OnClick="BtnEnviarResultados_Click" />
                &nbsp;
                <asp:Button ID="BtnSalir" runat="server" Text="Salir" CssClass="btn btn-default" CausesValidation="false"
                    OnClick="BtnSalir_Click" />
            </div>
        </div>
    </form>
</body>
</html>
