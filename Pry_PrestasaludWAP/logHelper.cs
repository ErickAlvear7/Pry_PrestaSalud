using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Pry_PrestasaludWAP
{
    public class logHelper
    {
       
        static string basePath = @"C:\LogsProyecto";

        public static void RegistrarAccion(string usuario, string modulo, string accion, string detalle)
        {
            try
            {
                
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                // Carpeta específica para el usuario
                string userFolder = Path.Combine(basePath, usuario);
                Directory.CreateDirectory(userFolder);

                // Archivo con la fecha actual (uno por día)
                string filePath = Path.Combine(userFolder, $"{DateTime.Now:yyyyMMdd}.log");

                // Texto que se va a escribir en el log
                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {modulo} | {accion} | {detalle}";

                // Escribe el log en el archivo (crea el archivo si no existe)
                File.AppendAllText(filePath, log + Environment.NewLine);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar log: " + ex.Message);
                
            }
        }

        public static void LimpiarLogsAntiguos(int diasRetencion = 30)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    return;

                foreach (var userDir in Directory.GetDirectories(basePath))
                {
                    foreach (var file in Directory.GetFiles(userDir, "*.log"))
                    {
                        FileInfo fi = new FileInfo(file);

                        // Si el archivo es más antiguo que el número de días especificado
                        if (fi.CreationTime < DateTime.Now.AddDays(-diasRetencion))
                        {
                            fi.Delete();
                            Console.WriteLine($"Log eliminado: {fi.FullName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al limpiar logs antiguos: " + ex.Message);
                //throw;
            }
        }
    }
}