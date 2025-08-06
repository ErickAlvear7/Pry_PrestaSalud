<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPrincipalCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmPrincipalCitaMedica" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset cols="20%, 80%" id="MenuPrincipal">            
            <frame noresize src="FrmMenuCitaMedica.aspx" name="MenuCitaMedica"  frameborder="0" id="MenuTreeview" />
            <frame noresize src="FrmDetalleCitaMedica.aspx" name="DetalleCitaMedica"  frameborder="0" id="DetalleTreeview" />
    </frameset>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
