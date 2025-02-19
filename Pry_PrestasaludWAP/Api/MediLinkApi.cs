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

        public string GetSucursal(string url,string token, int cuidad)
        {

            string responseSucursal = "";
            try
            {
                HttpClient _sucursal = new HttpClient();
                _sucursal.BaseAddress = new Uri(url);
                _sucursal.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resSucursal = _sucursal.GetAsync("api/obtener_sucursales/" + cuidad).Result;

                if (resSucursal.IsSuccessStatusCode)
                {
                    responseSucursal = resSucursal.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    responseSucursal = resSucursal.StatusCode.ToString();
                }


            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetSucursal", mensaje, 144);
            }
               
                return responseSucursal;
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

        public string GetMedicos(string url,string token,int codEspeci, int codSucursal)
        {
            string responseMed = "";
            try
            {

                HttpClient _medico = new HttpClient();
                _medico.BaseAddress = new Uri(url);
                _medico.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resMedico = _medico.GetAsync("api/ObtenerMedicos/" + codEspeci + "/" + codSucursal).Result;

                if (resMedico.IsSuccessStatusCode)
                {
                    responseMed = resMedico.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    responseMed = resMedico.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetMedicos", mensaje, 212);
            }


            return responseMed;
        }

        public string PostAdmision(string url,string dataAdmision,string token)
        {
            try
            {
                HttpClient _admision = new HttpClient();
                {
                    _admision.BaseAddress = new Uri(url);
                    _admision.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpContent _content = new StringContent(dataAdmision, Encoding.UTF8, "application/json");
                    var _resAdmision = _admision.PostAsync("api/admisionPaciente", _content).Result;

                    if (_resAdmision.IsSuccessStatusCode)
                    {
                        var responseAdmision = _resAdmision.Content.ReadAsStringAsync().Result;
                        return responseAdmision;
                    }

                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostAdmision", mensaje, 161);
            }
          
            return "";
        }

        public string PostCrearPaciente(string url, string data,string token)
        {
            try
            {

                HttpClient _patient = new HttpClient();
                {

                    _patient.BaseAddress = new Uri(url);
                    _patient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpContent _content = new StringContent(data, Encoding.UTF8, "application/json");
                    var _resPatient = _patient.PostAsync("api/RegistrarPaciente", _content).Result;

                    if (_resPatient.IsSuccessStatusCode)
                    {
                        var responseContent = _resPatient.Content.ReadAsStringAsync().Result;
                    }
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostCrearPaciente", mensaje, 207);
            }


            return "";
        }

        public string GetDisponibilidad(string url,string token,int codCiudad,int codEspe,int codSucur, string fechadispon)
        {
            string responDispo = "";
            try
            {
                HttpClient _dispo = new HttpClient();
                _dispo.BaseAddress = new Uri(url);
                _dispo.DefaultRequestHeaders.Add("Accept", "*/*");
                _dispo.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var queryString = new StringBuilder();
                queryString.Append("?fechaFin=").Append(Uri.EscapeDataString(fechadispon));
                queryString.Append("&fechaInit=").Append(Uri.EscapeDataString(fechadispon));

                var resDispo = _dispo.GetAsync("api/ObtenerDisponibilidades/" + codCiudad + "/" + codEspe + "/" + codSucur + queryString).Result;
                

                if (resDispo.IsSuccessStatusCode)
                {
                    responDispo = resDispo.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    MessageBox.Show(resDispo.StatusCode.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/GetDisponibilidad", mensaje, 312);
            }

            return responDispo;
        }
    }
}