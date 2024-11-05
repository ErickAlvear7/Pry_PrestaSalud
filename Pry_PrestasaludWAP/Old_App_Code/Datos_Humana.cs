using Newtonsoft.Json.Linq;
//using RestSharp;
using System;
using System.IO;

namespace Pry_PrestasaludWAP.Old_App_Code
{
    public class Datos_Humana
    {
        #region Load
        //public string GetToken(string url, string username, string password)
        //{
        //    try
        //    {
        //        var client = new RestClient(url);
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //        request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&username=" + username + "&password=" + password, ParameterType.RequestBody);
        //        request.Timeout = 6000;
        //        IRestResponse response = client.Execute(request);
        //        dynamic resp = JObject.Parse(response.Content);
        //        String token = resp.access_token;

        //        if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
        //        {
        //            StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
        //            _log.WriteLine("Error" + "|" + "Fecha_Registro");
        //            _log.Close();
        //        }

        //        StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

        //        _writer.WriteLine(token + "|" + DateTime.Now);
        //        _writer.Close();

        //        return token;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
        //        {
        //            StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
        //            _log.WriteLine("Error" + "|" + "Fecha_Registro");
        //            _log.Close();
        //        }

        //        StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

        //        _writer.WriteLine(ex.Message + "|" + DateTime.Now);
        //        _writer.Close();

        //        return null;
        //    }
        //}

        //public string GetItems(string url, string token, string cedula, string codpress)
        //{
        //    try
        //    {
        //        var client = new RestClient(url);
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("Authorization", "Bearer " + token);
        //        request.AddParameter("cedula", cedula);
        //        request.AddParameter("codpres", codpress);
        //        request.Timeout = 6000;
        //        IRestResponse response = client.Execute(request);
        //        //dynamic resp = JObject.Parse(response.Content).ToString();

        //        if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
        //        {
        //            StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
        //            _log.WriteLine("Error" + "|" + "Fecha_Registro");
        //            _log.Close();
        //        }

        //        StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

        //        _writer.WriteLine(response + "|" + DateTime.Now);
        //        _writer.Close();

        //        return JObject.Parse(response.Content).ToString(); ;
        //    }
        //    catch (Exception ex)
        //    {

        //        if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
        //        {
        //            StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
        //            _log.WriteLine("Error" + "|" + "Fecha_Registro");
        //            _log.Close();
        //        }

        //        StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

        //        _writer.WriteLine(ex.Message + "|" + DateTime.Now);
        //        _writer.Close();

        //        return null;
        //    }

        //} 
        #endregion
    }
}