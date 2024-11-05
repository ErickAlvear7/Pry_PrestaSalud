<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPrincipalPrestaEspe.aspx.cs" Inherits="Pry_PrestasaludWAP.Prestadora.FrmPrincipalPrestaEspe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset cols="20%, 80%" id="MenuPrincipal">            
            <frame noresize src="FrmMenuPrestaEspe.aspx" name="MenuPrestaEspe"  frameborder="0" id="MenuTreeview" />
            <frame noresize src="FrmDetallePrestaEspe.aspx" name="DetallePrestaEspe"  frameborder="0" id="DetalleTreeview" />
    </frameset>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
