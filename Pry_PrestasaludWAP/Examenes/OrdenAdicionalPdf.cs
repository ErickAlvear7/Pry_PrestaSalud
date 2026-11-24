using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Pry_PrestasaludWAP.Examenes
{
    public static class OrdenAdicionalPdf
    {
        public static byte[] Crear(DataRow datos)
        {
            if (datos == null) throw new ArgumentNullException("datos");
            using (var documento = new PdfDocument())
            {
                documento.Info.Title = "Exámenes adicionales - Solicitud " + datos["CodigoEXSO"];
                documento.Info.Author = "Prestasalud";
                var opciones = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                var normal = new XFont("Arial", 10.5, XFontStyle.Regular, opciones);
                var negrita = new XFont("Arial", 10.5, XFontStyle.Bold, opciones);
                var titulo = new XFont("Arial", 15, XFontStyle.Bold, opciones);
                var pie = new XFont("Arial", 8, XFontStyle.Regular, opciones);
                string referencia = "Solicitud " + datos["CodigoEXSO"] + " · Orden adicional " + datos["OrdenCodigo"];
                PdfPage pagina = null; XGraphics grafico = null; double y = 0;
                Action nuevaPagina = () =>
                {
                    if (grafico != null) grafico.Dispose();
                    pagina = documento.AddPage(); pagina.Size = PdfSharp.PageSize.A4;
                    grafico = XGraphics.FromPdfPage(pagina); y = 45;
                    y += 20;
                    grafico.DrawLine(new XPen(XColor.FromArgb(43, 93, 140), 1), 45, y, pagina.Width.Point - 45, y); y += 24;
                };
                Action<string, XFont, double> escribir = (texto, fuente, interlineado) =>
                {
                    foreach (string lineaOriginal in Regex.Split(texto ?? "", "\r\n|\r|\n"))
                    {
                        string limpia = Regex.Replace(lineaOriginal.Replace("\t", "    "), @"[\x00-\x08\x0B\x0C\x0E-\x1F]", "");
                        foreach (string linea in new List<string>(Ajustar(limpia, fuente, grafico, pagina.Width.Point - 90)))
                        {
                            if (y > pagina.Height.Point - 60) nuevaPagina();
                            grafico.DrawString(linea, fuente, XBrushes.Black, new XPoint(45, y)); y += interlineado;
                        }
                    }
                };
                try
                {
                    nuevaPagina();
                    escribir("EXÁMENES ADICIONALES SOLICITADOS", titulo, 22); y += 8;
                    escribir("Titular: " + Convert.ToString(datos["Titular"]), normal, 16);
                    escribir("Documento del titular: " + Convert.ToString(datos["DocumentoTitular"]), normal, 16);
                    escribir("Producto: " + Convert.ToString(datos["Producto"]), normal, 16);
                    escribir("Paciente: " + Convert.ToString(datos["Paciente"]), normal, 16);
                    escribir("Documento del paciente: " + Convert.ToString(datos["DocumentoPaciente"]), normal, 16);
                    escribir("Tipo de paciente: " + Convert.ToString(datos["TipoPaciente"]), normal, 16);
                    escribir("Solicitud original: " + Convert.ToDateTime(datos["FechaSolicitudOriginal"]).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), normal, 16);
                    escribir("Solicitado por: " + Convert.ToString(datos["UsuarioCliente"]), normal, 16);
                    escribir("Motivo: auditoría rechazada apelable", normal, 16); y += 16;
                    escribir("EXÁMENES REQUERIDOS", negrita, 20);
                    escribir(Convert.ToString(datos["ExamenesSolicitados"]), normal, 16);
                    grafico.Dispose(); grafico = null;
                    for (int i = 0; i < documento.PageCount; i++)
                    {
                        using (var g = XGraphics.FromPdfPage(documento.Pages[i], XGraphicsPdfPageOptions.Append))
                        {
                            g.DrawString(referencia, negrita, XBrushes.DimGray, new XPoint(45, 45));
                            g.DrawString("Página " + (i + 1) + " de " + documento.PageCount, pie, XBrushes.DimGray,
                                new XRect(45, documento.Pages[i].Height.Point - 36, documento.Pages[i].Width.Point - 90, 16), XStringFormats.TopRight);
                        }
                    }
                    using (var memoria = new MemoryStream()) { documento.Save(memoria, false); return memoria.ToArray(); }
                }
                finally { if (grafico != null) grafico.Dispose(); }
            }
        }
        private static IEnumerable<string> Ajustar(string texto, XFont fuente, XGraphics g, double ancho)
        {
            if (texto.Length == 0) { yield return ""; yield break; }
            string pendiente = texto;
            while (pendiente.Length > 0)
            {
                if (g.MeasureString(pendiente, fuente).Width <= ancho) { yield return pendiente; yield break; }
                int izquierda = 1, derecha = pendiente.Length;
                while (izquierda < derecha)
                {
                    int mitad = (izquierda + derecha + 1) / 2;
                    if (g.MeasureString(pendiente.Substring(0, mitad), fuente).Width <= ancho) izquierda = mitad; else derecha = mitad - 1;
                }
                int corte = izquierda;
                int espacio = pendiente.LastIndexOf(' ', Math.Max(0, corte - 1), corte);
                if (espacio > 0) corte = espacio;
                if (corte > 0 && corte < pendiente.Length && char.IsHighSurrogate(pendiente[corte - 1]) && char.IsLowSurrogate(pendiente[corte])) corte--;
                if (corte <= 0) corte = Math.Min(2, pendiente.Length);
                yield return pendiente.Substring(0, corte).TrimEnd(); pendiente = pendiente.Substring(corte).TrimStart();
            }
        }
    }
}
