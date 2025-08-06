<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPrincipalPrestadora.aspx.cs" Inherits="Pry_PrestasaludWAP.Titulares.FrmPrincipalPrestadora" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset cols="20%, 80%" id="MenuPrincipal">            
            <frame noresize src="FrmMenuPrestadora.aspx" name="MenuPrestadora"  frameborder="0" id="MenuTreeview" />
            <frame noresize src="FrmDetallePrestadora.aspx" name="DetallePrestadora"  frameborder="0" id="DetalleTreeview" />
    </frameset>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
