using AjaxControlToolkit.HTMLEditor.Popups;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using static Pry_PrestasaludWAP.Modelo.MediLinkModel;

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
                else
                {
                    MessageBox.Show(resLogin.StatusCode.ToString());
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostAccesLogin", mensaje, 36);
            }
            return "";
        }


        public string GetVerificarPaciente(string url,string token,string documento,string tipo)
        {
            string responsePaciente = "";
            try
            {
                HttpClient _paciente = new HttpClient();
                _paciente.BaseAddress = new Uri(url);
                _paciente.DefaultRequestHeaders.Add("rucEmpresa", "1792206979001");
                _paciente.DefaultRequestHeaders.Add("Accept", "*/*");
                _paciente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resPaciente = _paciente.GetAsync("api/VerificarPaciente/" + documento + "/" + tipo).Result;

                if (resPaciente.IsSuccessStatusCode)
                {
                    responsePaciente = "SI";
                }
                else
                {
                    responsePaciente = "NO";
                }


                
            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetVerificarPaciente", mensaje, 57);
            }

            return responsePaciente;
        }

        public string GetCiudad(string url, string token)
        {
            string responseContent = "";
            try
            {
                
                HttpClient _ciudad = new HttpClient();
                _ciudad.BaseAddress = new Uri(url);
                _ciudad.DefaultRequestHeaders.Add("Accept", "*/*");
                _ciudad.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resCiudad = _ciudad.GetAsync("api/ObtenerCodCiudad").Result;

                if (resCiudad.IsSuccessStatusCode)
                {
                    responseContent = resCiudad.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    responseContent = resCiudad.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetCiudad", mensaje, 122);
            }

            return responseContent;
        }


        public string GetEspecialidad(string url,string token, int sucursal)
        {
            string responseEspe = "";

            try
            {
                HttpClient _espec = new HttpClient();
                _espec.BaseAddress = new Uri(url);
                _espec.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resEspe = _espec.GetAsync("api/ObtenerEspecialidad/" + sucursal).Result;

                if (resEspe.IsSuccessStatusCode)
                {
                    responseEspe = resEspe.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    responseEspe = resEspe.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetEspecialidad", mensaje, 157);
            }

            return responseEspe;

        }
    }
}