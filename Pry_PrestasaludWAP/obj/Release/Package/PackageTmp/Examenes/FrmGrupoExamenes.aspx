<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGrupoExamenes.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmGrupoExamenes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
    <link rel="stylesheet" href="../Style/chosen.css" />
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

        .contenedor-grid {
            width: 100%;
            overflow-x: auto;
        }

        .tabla-examenes {
            width: 100% !important;
            table-layout: fixed;
        }

            .tabla-examenes th,
            .tabla-examenes td {
                vertical-align: middle !important;
                white-space: normal !important;
                word-wrap: break-word;
                overflow-wrap: break-word;
                word-break: normal;
            }

            .tabla-examenes input[type="text"] {
                width: 100%;
                min-width: 0;
                box-sizing: border-box;
            }
            .tabla-responsive-examen {
                width: 100%;
                overflow-x: auto;
}

            /* Evita que una tabla invada la otra columna */
            .tabla-examen {
                width: 100% !important;
                table-layout: fixed;
            }


            .tabla-examen th,
            .tabla-examen td {
                vertical-align: middle !important;

                white-space: normal !important;

                word-wrap: break-word;
                overflow-wrap: break-word;
            }

            .estado-asignado {
                display: inline-block;

                padding: 4px 8px;

                border-radius: 4px;

                background-color: #dff0d8;

                color: #3c763d;

                font-size: 11px;

                font-weight: bold;

                white-space: nowrap;
            }

            .estado-no-asignado {
                display: inline-block;

                padding: 4px 8px;

                border-radius: 4px;

                background-color: #eeeeee;

                color: #555555;

                font-size: 11px;

                font-weight: bold;

                white-space: nowrap;
            }


            .paginador-examen {
                padding: 10px;
            }


            .paginador-examen table {
                margin: auto;
            }


            .paginador-examen td {
                padding: 2px;
            }


            .paginador-examen a,
            .paginador-examen span {
                display: inline-block;

                padding: 6px 10px;

                margin: 2px;

                border: 1px solid #dddddd;

                border-radius: 4px;

                text-decoration: none;
            }


            .paginador-examen span {
                font-weight: bold;

                background-color: #eeeeee;
            }
            .grupo-botones {
                display: flex;
                gap: 5px;
                align-items: center;
            }

            .grupo-botones .btn {
                white-space: nowrap;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary container-fluid">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server">Grupo Examenes</asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                            <asp:HiddenField ID="hidEdad" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- PRESTADOR -->
                <div class="row">
                    <div class="col-md-6">
                        <label>Prestador / Clínica</label>
                        <asp:DropDownList ID="ddlPrestador" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPrestador_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label>Grupo</label>
                        <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <!-- DATOS GRUPO -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Nombre del grupo</label>
                        <asp:TextBox ID="txtNombreGrupo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <div class="grupo-botones">
                            <asp:Button ID="btnNuevoGrupo" runat="server" Text="Nuevo Grupo" CssClass="btn btn-default"
                            OnClick="btnNuevoGrupo_Click" />
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Grupo" CssClass="btn btn-primary"
                            OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-6">
                        <h4>Exámenes disponibles</h4>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Buscar examen</label>
                                <asp:TextBox
                                    ID="txtBuscarExamen"
                                    runat="server"
                                    CssClass="form-control"
                                    placeholder="Buscar por nombre...">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label>Estado</label>
                                <asp:DropDownList
                                    ID="ddlFiltroAsignacion"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlFiltroAsignacion_SelectedIndexChanged">
                                    <asp:ListItem
                                        Text="Todos"
                                        Value="TODOS"
                                        Selected="True">
                                    </asp:ListItem>
                                    <asp:ListItem
                                        Text="Asignados"
                                        Value="1">
                                    </asp:ListItem>
                                    <asp:ListItem
                                        Text="No asignados"
                                        Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label>&nbsp;</label>
                                <div>
                                    <asp:Button
                                        ID="btnBuscarExamen"
                                        runat="server"
                                        Text="Buscar"
                                        CssClass="btn btn-primary"
                                        OnClick="btnBuscarExamen_Click" />
                                    <asp:Button
                                        ID="btnLimpiarBusqueda"
                                        runat="server"
                                        Text="Limpiar"
                                        CssClass="btn btn-default"
                                        OnClick="btnLimpiarBusqueda_Click" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="table-responsive contenedor-grid">
                            <asp:GridView ID="gvDisponibles" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover tabla-examenes"
                                DataKeyNames="EXPR_CODIGO,PVP,VALOR_RED" AllowPaging="true" PageSize="10"
                                OnPageIndexChanging="gvDisponibles_PageIndexChanging"
                                OnRowCommand="gvDisponibles_RowCommand">
                                <Columns>
                                    <asp:BoundField
                                        DataField="EXAMEN" HeaderText="Examen">
                                        <HeaderStyle Width="48%" />
                                        <ItemStyle Width="48%" />
                                    </asp:BoundField>
                                    <asp:BoundField
                                        DataField="PVP" HeaderText="PVP" DataFormatString="{0:0.00}">
                                        <HeaderStyle Width="13%" />
                                        <ItemStyle Width="13%" />
                                    </asp:BoundField>
                                    <asp:BoundField
                                        DataField="VALOR_RED" HeaderText="Red" DataFormatString="{0:0.00}">
                                        <HeaderStyle Width="12%" />
                                        <ItemStyle
                                            Width="12%"
                                            HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Estado">
                                        <HeaderStyle Width="20%" />
                                        <ItemStyle
                                            Width="20%"
                                            HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label
                                                ID="lblEstadoAsignacion"
                                                runat="server"
                                                Text='<%#
                                                    Convert.ToInt32(Eval("ASIGNADO")) == 1
                                                    ? "Asignado"
                                                    : "No asignado"
                                                %>'
                                                CssClass='<%#
                                                    Convert.ToInt32(Eval("ASIGNADO")) == 1
                                                    ? "estado-asignado"
                                                    : "estado-no-asignado"
                                                %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="8%" />
                                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-primary btn-sm" 
                                                Text="+" ToolTip="Agregar examen al grupo" CommandName="Agregar">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings
                                    Mode="NumericFirstLast"
                                    FirstPageText="«"
                                    LastPageText="»"
                                    PageButtonCount="5" />
                                <PagerStyle
                                    CssClass="paginador-examen"
                                    HorizontalAlign="Center" />
                                  <EmptyDataTemplate>
                                      <div class="alert alert-info">
                                            No se encontraron exámenes.
                                      </div>
                                  </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <!-- DETALLE -->
                    <div class="col-md-6">
                        <h4>Exámenes del grupo</h4>
                        <div class="table-responsive contenedor-grid">
                            <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover tabla-examenes"
                                DataKeyNames="EXPR_CODIGO"
                                OnRowCommand="gvDetalle_RowCommand">
                                <Columns>
                                    <asp:BoundField
                                        DataField="EXAMEN" HeaderText="Examen">
                                        <HeaderStyle Width="52%" />
                                        <ItemStyle Width="52%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="PVP">
                                        <HeaderStyle Width="18%" />
                                        <ItemStyle Width="18%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPvp" runat="server" CssClass="form-control"
                                                Text='<%# Eval("PVP", "{0:0.00}") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Red">
                                        <HeaderStyle Width="18%" />
                                        <ItemStyle Width="18%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRed" runat="server"
                                                CssClass="form-control"
                                                Text='<%# Eval("VALOR_RED", "{0:0.00}") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle Width="12%" />
                                        <ItemStyle Width="12%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger"
                                                Text="X"
                                                CommandName="Eliminar">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="text-right">
                            <h3>Total PVP:
                                <asp:Label ID="lblTotalPvp" runat="server" Text="$ 0.00"></asp:Label>
                            </h3>
                        </div>
                    </div>
                </div>
                <asp:Label ID="lblMensaje" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
