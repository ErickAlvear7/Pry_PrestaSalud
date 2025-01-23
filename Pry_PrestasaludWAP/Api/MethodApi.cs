using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Pry_PrestasaludWAP.Api
{
    public class MethodApi
    {
        public string GetToken(string url, string _apikey)
        {
            try
            {
                HttpClient _client = new HttpClient();
                {
                    _client.BaseAddress = new Uri(url);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpContent _content = new StringContent(_apikey, Encoding.UTF8, "application/json");

                var _response = _client.PostAsync("login", _content).Result;

                if (_response.IsSuccessStatusCode)
                {
                    var responseContent = _response.Content.ReadAsStringAsync().Result;
                    dynamic token = JObject.Parse(responseContent);
                    return token.token;
                }
                else
                {
                    new Funciones().funCrearLogAuditoria(1, "ISSUE", "INSSUE", 1);
                    MessageBox.Show(_response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/GetToken", mensaje, 49);

            }

            return "";

        }

        public string GetIdContract(string url, string auth)
        {
            try
            {
                HttpClient _contract = new HttpClient();
                {
                    _contract.BaseAddress = new Uri(url);
                    _contract.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resConId = _contract.GetAsync("contratos").Result;

                if (resConId.IsSuccessStatusCode)
                {
                    var responseId = resConId.Content.ReadAsStringAsync().Result;
                    dynamic idcont = JArray.Parse(responseId);
                    string idespe = idcont[0].id;
                    return idespe;
                }
                else
                {
                    MessageBox.Show(resConId.StatusCode.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/GetIdContract", mensaje, 88);

            }

            return "";
        }

        public string GetServicios(string url, string idcont, string auth)
        {
            try
            {
                HttpClient _servicio = new HttpClient();
                {
                    _servicio.BaseAddress = new Uri(url);
                    _servicio.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resServId = _servicio.GetAsync("services/" + idcont).Result;

                if (resServId.IsSuccessStatusCode)
                {
                    var serviceId = resServId.Content.ReadAsStringAsync().Result;

                    dynamic dataSer = JArray.Parse(serviceId);
                    string ideser = dataSer[0].id;
                    return ideser;
                }
                else
                {
                    MessageBox.Show(resServId.StatusCode.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/GetServicios", mensaje, 129);
            }
          
            return "";
        }

        public string GetEspecialidad(string url, string auth, string idcont)
        {
            try
            {
                HttpClient _espec = new HttpClient();
                {
                    _espec.BaseAddress = new Uri(url);
                    _espec.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resEspeId = _espec.GetAsync("specialties/" + idcont).Result;

                if (resEspeId.IsSuccessStatusCode)
                {
                    var espeId = resEspeId.Content.ReadAsStringAsync().Result;

                    dynamic dataEsp = JArray.Parse(espeId);
                    string idespe = dataEsp[0].id;
                    return idespe;
                }
                else
                {
                    MessageBox.Show(resEspeId.StatusCode.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/GetEspecialidad", mensaje, 165);
            }
         
            return "";
        }

        public string PostCreatePatient(string url, string dataPatient, string auth)
        {
            try
            {
                HttpClient _patient = new HttpClient();
                {
                    _patient.BaseAddress = new Uri(url);
                    _patient.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpContent _content = new StringContent(dataPatient, Encoding.UTF8, "application/json");
                    var _resPatient = _patient.PostAsync("patient", _content).Result;

                    if (_resPatient.IsSuccessStatusCode)
                    {
                        var responseContent = _resPatient.Content.ReadAsStringAsync().Result;
                        dynamic idpat = JObject.Parse(responseContent);
                        return idpat.patient.id;
                    }
                    else
                    {
                        MessageBox.Show(_resPatient.StatusCode.ToString());
                        return "";

                    }

                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/PostCreatePatient", mensaje, 203);
            }

            return "";
        }

        public string Consultas(string url, string datacon, string auth)
        {
            try
            {
                HttpClient _consulta = new HttpClient();
                {
                    _consulta.BaseAddress = new Uri(url);
                    _consulta.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpContent _content = new StringContent(datacon, Encoding.UTF8, "application/json");
                    var _resConsulta = _consulta.PostAsync("consulta", _content).Result;

                    if (_resConsulta.IsSuccessStatusCode)
                    {
                        var responseContent = _resConsulta.Content.ReadAsStringAsync().Result;
                        return responseContent.ToString();
                    }
                    else
                    {
                        return "Horario";
                    }

                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/Consultas", mensaje, 241);
            }

            return "";
        }

    }
}