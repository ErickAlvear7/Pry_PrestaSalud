<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGestionExamenCliente.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmGestionExamenCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Gestión Cliente - Exámenes</title>

    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
  
    <style type="text/css">

        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }

        .titulo-seccion {
            font-size: 13px;
            display: block;
            text-align: left;
            margin-top: 5px;
            margin-bottom: 10px;
            padding: 7px 10px;
        }

        .info-label {
            font-weight: bold;
            font-size: 12px;
        }

        .info-valor {
            font-size: 12px;
        }

        .info-fila {
            margin-bottom: 7px;
        }

        .panel-auditoria {
            border-left: 4px solid #337ab7;
        }

        .panel-cliente {
            border-left: 4px solid #5cb85c;
        }

        .panel-historial {
            border-left: 4px solid #999;
        }

        .estado-aceptado {
            color: green;
            font-weight: bold;
        }

        .estado-rechazado {
            color: red;
            font-weight: bold;
        }

        .estado-pendiente {
            color: #777;
            font-weight: bold;
        }

        .informe-lectura {
            border: 1px solid #ddd;
            background-color: #fafafa;
            padding: 10px;
            min-height: 50px;
            border-radius: 3px;
            overflow-x: auto;
        }

        .modal-pdf-fondo {
            display: none;
            position: fixed;
            z-index: 9999;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.65);
            overflow: auto;
            padding: 12px;
        }


        .modal-pdf-contenedor {
            width: 96%;
            max-width: 1400px;
            margin: 0 auto;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 4px 18px rgba(0, 0, 0, 0.35);
            overflow: hidden;
        }


        .modal-pdf-cabecera {
            position: relative;
            min-height: 42px;
            padding: 11px 48px 10px 15px;
            background-color: #337ab7;
            color: #fff;
            font-size: 13px;
            font-weight: bold;
            line-height: 20px;
        }


        .modal-pdf-cerrar {
            position: absolute;
            top: 4px;
            right: 8px;
            width: 36px;
            height: 34px;
            padding: 0;
            border: 0;
            background: transparent;
            color: #fff;
            font-size: 27px;
            line-height: 30px;
            cursor: pointer;
        }


        .modal-pdf-cuerpo {
            width: 100%;
            padding: 0;
            background-color: #d9d9d9;
            overflow: hidden;

            height: 75vh;
            min-height: 400px;
        }


        #visorResultadoCliente {
            width: 100%;
            height: 100%;
        }


        .modal-pdf-pie {
            min-height: 48px;
            padding: 7px 12px;
            text-align: right;
            border-top: 1px solid #ddd;
            background-color: #fff;
        }

        @media (max-width: 991px) {

            .modal-pdf-fondo {
                padding: 8px;
            }

            .modal-pdf-contenedor {
                width: 100%;
                max-width: none;
            }
        }

        @media (max-width: 767px) {

            .modal-pdf-fondo {
                padding: 4px;
            }

            .modal-pdf-contenedor {
                width: 98%;
                margin: 5px auto;
                border-radius: 2px;
            }

            .modal-pdf-cabecera {
                min-height: 44px;
                padding: 11px 48px 9px 10px;
                font-size: 12px;
            }

            .modal-pdf-cerrar {
                width: 42px;
                height: 40px;
                top: 1px;
                right: 3px;
            }

            .modal-pdf-cuerpo {
                height: 70vh;
                min-height: 300px;
            }

            .modal-pdf-pie {
                min-height: 52px;
                padding: 7px;
                text-align: center;
            }

            .modal-pdf-pie .btn {
                width: 100%;
                min-height: 38px;
            }
        }

        @media (max-width: 480px) {

            .modal-pdf-fondo {
                padding: 2px;
            }

            .modal-pdf-cabecera {
                font-size: 11px;
            }

        }

        .overlay {
            position: fixed;
            z-index: 98;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #aaa;
            opacity: .8;
        }

        .overlayContent {
            z-index: 99;
            position: fixed;
            top: 40%;
            left: 47%;
            text-align: center;
        }

        .overlayContent img {
            width: 80px;
            height: 80px;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdError" runat="server">
            <ContentTemplate>
                <div style="background-color: beige; text-align: left; width: 100%; font-size: 14px;">
                    <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <div class="overlay"></div>
                <div class="overlayContent">
                    <h4>Procesando...</h4>
                    <img src="../Images/load.gif" alt="Procesando" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="panel-body">
            <div class="alert alert-info" style="font-size: 12px;">
                <strong>Solicitud:</strong>
                <asp:Label ID="LblCodigoSolicitud" runat="server"></asp:Label>
            </div>
            <asp:Repeater ID="RptPacientes" runat="server" OnItemDataBound="RptPacientes_ItemDataBound">
                <ItemTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <strong>
                                <%# Eval("TIPO_PERSONA").ToString() == "CODEPENDIENTE"
                                    ? "CODEUDOR"
                                    : Eval("TIPO_PERSONA").ToString() %>

                                -

                                <%# Eval("PACIENTE") %>

                            </strong>
                        </div>
                        <div class="panel-body">
                            <span class="label label-primary titulo-seccion">
                                INFORMACIÓN DEL PACIENTE
                            </span>
                            <div class="row info-fila">
                                <div class="col-sm-2 info-label">
                                    Nombres:
                                </div>
                                <div class="col-sm-4 info-valor">
                                    <%# Eval("NOMBRES") %>
                                </div>
                                <div class="col-sm-2 info-label">
                                    Apellidos:
                                </div>
                                <div class="col-sm-4 info-valor">
                                    <%# Eval("APELLIDOS") %>
                                </div>
                            </div>
                            <div class="row info-fila">
                                <div class="col-sm-2 info-label">
                                    Edad:
                                </div>
                                <div class="col-sm-4 info-valor">
                                    <%# Eval("EDAD") %> años
                                </div>
                                <div class="col-sm-2 info-label">
                                    Fecha nacimiento:
                                </div>
                                <div class="col-sm-4 info-valor"><%#FormatearFecha(Eval("FECHA_NACIMIENTO")) %></div>
                            </div>
                            <div class="row info-fila">
                                <div class="col-sm-2 info-label">
                                    Ciudad:
                                </div>
                                <div class="col-sm-4 info-valor">
                                    <%# Eval("CIUDAD") %>
                                </div>
                                <div class="col-sm-2 info-label">
                                    Monto:
                                </div>
                                <div class="col-sm-4 info-valor"><%#FormatearMonto(Eval("MONTO_TOTAL")) %></div>
                            </div>
                            <div class="row info-fila">
                                <div class="col-sm-2 info-label">
                                    Dirección:
                                </div>
                                <div class="col-sm-10 info-valor">
                                    <%# Eval("DIRECCION") %>
                                </div>
                            </div>
                            <asp:HiddenField ID="HfCodigoPERS" runat="server" Value='<%# Eval("PERS_CODIGO") %>' />
                            <hr />
                            <span class="label label-primary titulo-seccion">
                                RESULTADOS DEL EXAMEN
                            </span>
                            <asp:Repeater ID="RptResultados" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-condensed table-bordered" style="font-size: 11px;">
                                        <thead>
                                            <tr>
                                                <th style="width: 8%; text-align: center;">#</th>
                                                <th>Archivo</th>
                                                <th style="width: 15%; text-align: center;">Tipo</th>
                                                <th style="width: 15%; text-align: center;">Ver</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%# Eval("EXRD_ORDEN")%>
                                        </td>
                                        <td>
                                            <%# Eval("EXRD_NOMBRE") %>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# MostrarTipoArchivo(Eval("EXRD_EXTENSION"))%>
                                        </td>
                                        <td style="text-align: center;">
                                           <a href="javascript:void(0);"
                                               class="btn btn-primary btn-xs"
                                               style='<%# EsPdf(Eval("EXRD_EXTENSION")) ? "" : "display:none;" %>'
                                               data-url='<%# ObtenerUrlResultado(Eval("EXRD_CODIGO")) %>'
                                               onclick="
                                                   var visor = document.getElementById('visorResultadoCliente');
                                                   var modal = document.getElementById('modalResultadoPdf');

                                                   if (visor != null && modal != null) {
                                                       visor.setAttribute(
                                                           'src',
                                                           this.getAttribute('data-url')
                                                       );

                                                       modal.style.display = 'block';
                                                       document.body.style.overflow = 'hidden';
                                                   }

                                                   return false;
                                               ">

                                                Ver

                                            </a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>

                                        </tbody>

                                    </table>

                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Label ID="LblSinResultados" runat="server" Visible="false" Text="No existen resultados adjuntos." ForeColor="Gray"></asp:Label>
                            <hr />
                            <div class="panel panel-default panel-auditoria">
                                <div class="panel-heading">
                                    <strong>REVISIÓN DEL AUDITOR</strong>
                                    <asp:Label ID="LblEstadoAuditoria" runat="server" CssClass="pull-right"></asp:Label>
                                </div>
                                <div class="panel-body">
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class="col-sm-2">
                                            <strong>Estado:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:Label ID="LblEstadoAuditoriaDetalle" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class="col-sm-2">
                                            <strong>Informe:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <div class="informe-lectura">
                                                <asp:Literal ID="LitInformeAuditor" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <strong>Adjunto:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:Label ID="LblAdjuntoAuditor" runat="server" ForeColor="Gray"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default panel-cliente">
                                <div class="panel-heading">
                                    <strong>GESTIÓN CLIENTE</strong>
                                    <asp:Label ID="LblEstadoCliente" runat="server" CssClass="pull-right" Text="PENDIENTE"></asp:Label>
                                </div>
                                <div class="panel-body">
                                    <div class="row" style="margin-bottom: 12px;">
                                        <div class="col-sm-2">
                                            <strong>Decisión:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:RadioButton ID="RdbClienteAceptado" runat="server" GroupName="EstadoCliente" Text=" ACEPTADO" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="RdbClienteRechazado" runat="server" GroupName="EstadoCliente" Text=" RECHAZADO" />
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 12px;">
                                        <div class="col-sm-2">
                                            <strong>Informe:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:HiddenField ID="HfInformeCliente" runat="server" />
                                            <asp:Button ID="BtnEditarInformeCliente" runat="server" Text="Editar informe" CssClass="btn btn-primary btn-xs" CausesValidation="false"
                                                CommandArgument='<%#Eval("PERS_CODIGO")%>'
                                                OnCommand="BtnEditarInformeCliente_Command" />
                                            &nbsp;
                                            <asp:Label ID="LblResumenInformeCliente" runat="server" Text="Sin informe" ForeColor="Gray"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 12px;">
                                        <div class="col-sm-2">
                                            <strong>Adjunto:</strong>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:FileUpload ID="FupAdjuntoCliente" runat="server" />
                                            <div style="margin-top: 5px;">
                                                <asp:Label ID="LblAdjuntoCliente" runat="server" Text="Sin archivo adjunto." ForeColor="Gray"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12" style="text-align: right;">
                                            <asp:Button ID="BtnGuardarCliente" runat="server" Text="Guardar Gestión" CssClass="btn btn-success btn-sm" CausesValidation="false"
                                                CommandArgument='<%#Eval("PERS_CODIGO")%>'
                                                OnCommand=" BtnGuardarCliente_Command" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default panel-historial">
                                <div class="panel-heading">
                                    <strong>HISTORIAL GESTIÓN CLIENTE</strong>
                                </div>
                                <div class="panel-body">
                                    <asp:GridView ID="GrdvHistorialCliente" runat="server" AutoGenerateColumns="False" Width="100%"
                                        ShowHeaderWhenEmpty="True"
                                        EmptyDataText="No existen gestiones realizadas."
                                        CssClass="table table-condensed table-bordered">
                                        <Columns>
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:BoundField DataField="Informe" HeaderText="Informe" />
                                            <asp:BoundField DataField="Adjunto" HeaderText="Adjunto" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="PnlSinPacientes" runat="server" Visible="false">
                <div class="alert alert-warning">
                    No existe información de pacientes
                    para esta solicitud.
                </div>
            </asp:Panel>
            <asp:Panel ID="PnlEditorCliente" runat="server" Visible="false" CssClass="panel panel-default">
                <div class="panel-heading">
                    <strong>INFORME DEL CLIENTE</strong>
                    &nbsp;|&nbsp;
                    <asp:Label ID="LblPacienteEditorCliente" runat="server"></asp:Label>
                </div>
                <div class="panel-body">
                    <cc1:Editor ID="EditorInformeCliente" runat="server" Height="300px" Width="100%" AutoFocus="False" />
                    <div style="text-align: right; margin-top: 10px;">
                        <asp:Button ID="BtnAceptarInformeCliente" runat="server" Text="Aceptar informe"
                            CssClass="btn btn-success btn-sm"
                            CausesValidation="false"
                            OnClick="BtnAceptarInformeCliente_Click" />
                        &nbsp;
                        <asp:Button ID="BtnCerrarEditorCliente" runat="server" Text="Cerrar" CssClass="btn btn-default btn-sm"
                            CausesValidation="false"
                            OnClick="BtnCerrarEditorCliente_Click" />
                    </div>
                </div>
            </asp:Panel>
            <div style="text-align: center; margin-top: 15px;">
                <asp:Button ID="BtnSalir" runat="server" Text="Regresar" Width="120px"
                    CssClass="button"
                    CausesValidation="false"
                    OnClick="BtnSalir_Click" />
            </div>
        </div>
    </div>
<%--    <div id="modalResultadoPdf" class="modal-pdf-fondo">
        <div class="modal-pdf-contenedor">
            <div class="modal-pdf-cabecera">
                RESULTADO DEL EXAMEN
                <button type="button" class="modal-pdf-cerrar" onclick="cerrarResultadoCliente();">×</button>
            </div>
            <div class="modal-pdf-cuerpo">
                <embed id="visorResultadoCliente" type="application/pdf" style="width: 100%;
                           height: 100%;
                           border: 0;" />
            </div>
            <div id="contenedorPdfCliente" class="modal-pdf-cuerpo" style="overflow: auto; background-color: #525659; text-align: center; padding: 15px;">
                <div id="paginasPdfCliente"></div>
            </div>
            <div class="modal-pdf-pie">
                <button type="button" class="btn btn-default btn-sm" onclick="cerrarResultadoCliente();">Cerrar</button>
            </div>
        </div>
    </div>--%>
    <div id="modalResultadoPdf" class="modal-pdf-fondo">

       <div class="modal-pdf-contenedor">
            <div class="modal-pdf-cabecera">
            <span>
                VISUALIZACIÓN DE RESULTADO
            </span>

            <button type="button"
                    class="modal-pdf-cerrar"
                    onclick="
                        var visor =
                            document.getElementById(
                                'visorResultadoCliente'
                            );

                        var modal =
                            document.getElementById(
                                'modalResultadoPdf'
                            );

                        if (visor != null) {
                            visor.removeAttribute('src');
                        }

                        if (modal != null) {
                            modal.style.display = 'none';
                        }

                        document.body.style.overflow = '';

                        return false;
                    ">
                ×
            </button>
        </div>
        <div class="modal-pdf-cuerpo">

            <embed
                id="visorResultadoCliente"
                type="application/pdf"
                style="width: 100%;
                       height: 100%;
                       border: 0;" />

        </div>
        <div class="modal-pdf-pie">
            <button type="button"
                    class="btn btn-default btn-sm"
                    onclick="
                        var visor =
                            document.getElementById(
                                'visorResultadoCliente'
                            );

                        var modal =
                            document.getElementById(
                                'modalResultadoPdf'
                            );

                        if (visor != null) {
                            visor.removeAttribute('src');
                        }

                        if (modal != null) {
                            modal.style.display = 'none';
                        }

                        document.body.style.overflow = '';

                        return false;
                    ">

                Cerrar
            </button>
        </div>
    </div>

</div>
    <script type="text/javascript">

        function cerrarResultadoCliente() {

            var visor = document.getElementById('visorResultadoCliente');

            var modal = document.getElementById('modalResultadoPdf');

            if (visor != null) {
                visor.removeAttribute('src');
            }

            if (modal != null) {
                modal.style.display = 'none';
            }

            document.body.style.overflow = '';

            return false;
        }

    </script>

</form>

</body>

</html>