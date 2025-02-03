using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Pry_PrestasaludWAP.Api
{
    public class MediLinkApi
    {

        public string PostAccesLogin(string url,string dataLogin)
        {
            try
            {
                HttpClient _login = new HttpClient();
                _login.BaseAddress = new Uri(url);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpContent _content = new StringContent(dataLogin, Encoding.UTF8, "application/json");
                var resLogin = _login.PostAsync("api/login", _content).Result;
                if (resLogin.IsSuccessStatusCode)
                {
                    var responseContent = resLogin.Content.ReadAsStringAsync().Result;
                    dynamic accessToken = JObject.Parse(responseContent);
                    string token = accessToken.datos.accesToken;
                    return token;
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostAccesLogin", mensaje, 36);
            }
            return "";
        }
    }
}