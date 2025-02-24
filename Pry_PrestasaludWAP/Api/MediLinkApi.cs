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
                    var responseContent = resPaciente.Content.ReadAsStringAsync().Result;
                    dynamic idpaciente = JObject.Parse(responseContent);
                    string id = idpaciente.datos.idPersona;
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
            string responseAdmi = "";
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
                        //responseAdmi = _resAdmision.Content.ReadAsStringAsync().Result;

                        responseAdmi = "OK";
                    }
                    else
                    {
                        responseAdmi = _resAdmision.StatusCode.ToString();
                    }

                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostAdmision", mensaje, 161);
            }
          
            return responseAdmi;
        }

        public string PostCrearPaciente(string url, string data,string token)
        {
            string responsePaciente = "";
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
                        responsePaciente = _resPatient.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        responsePaciente = _resPatient.StatusCode.ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostCrearPaciente", mensaje, 207);
            }


            return responsePaciente;
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

                //responDispo = "{\r\n  \"estado\": \"OK\",\r\n  \"datos\": [\r\n    {\r\n      \"codigoSucursal\": 1,\r\n      \"nombreSucursal\": \"MediLink Sur\",\r\n      \"codigoMedico\": 1919,\r\n      \"tipoIdentificacionMedico\": \"C\",\r\n      \"numeroIdentificacionMedico\": \"0956252779\",\r\n      \"nombreMedico\": \"BERMEO PAREDES JOSUE DAVID\",\r\n      \"disponibilidad\": [\r\n        {\r\n          \"fechaDisponibilidad\": \"2025-02-19\",\r\n          \"horario\": [\r\n            {\r\n              \"idHorarioDisponible\": 16022219,\r\n              \"horaInicio\": \"10:20\",\r\n              \"horaFin\": \"10:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16022220,\r\n              \"horaInicio\": \"10:40\",\r\n              \"horaFin\": \"11:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16022221,\r\n              \"horaInicio\": \"11:00\",\r\n              \"horaFin\": \"11:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16022222,\r\n              \"horaInicio\": \"11:20\",\r\n              \"horaFin\": \"11:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16022223,\r\n              \"horaInicio\": \"11:40\",\r\n              \"horaFin\": \"12:00\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"codigoSucursal\": 1,\r\n      \"nombreSucursal\": \"MediLink Sur\",\r\n      \"codigoMedico\": 463,\r\n      \"tipoIdentificacionMedico\": \"C\",\r\n      \"numeroIdentificacionMedico\": \"0927018101\",\r\n      \"nombreMedico\": \"CASTILLO RODRIGUEZ JOSELYN NATHALI\",\r\n      \"disponibilidad\": [\r\n        {\r\n          \"fechaDisponibilidad\": \"2025-02-19\",\r\n          \"horario\": [\r\n            {\r\n              \"idHorarioDisponible\": 16016774,\r\n              \"horaInicio\": \"10:20\",\r\n              \"horaFin\": \"10:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016775,\r\n              \"horaInicio\": \"10:40\",\r\n              \"horaFin\": \"11:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016776,\r\n              \"horaInicio\": \"11:00\",\r\n              \"horaFin\": \"11:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016777,\r\n              \"horaInicio\": \"11:20\",\r\n              \"horaFin\": \"11:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016778,\r\n              \"horaInicio\": \"11:40\",\r\n              \"horaFin\": \"12:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016779,\r\n              \"horaInicio\": \"12:00\",\r\n              \"horaFin\": \"12:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016780,\r\n              \"horaInicio\": \"12:20\",\r\n              \"horaFin\": \"12:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016781,\r\n              \"horaInicio\": \"12:40\",\r\n              \"horaFin\": \"13:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016785,\r\n              \"horaInicio\": \"14:00\",\r\n              \"horaFin\": \"14:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016786,\r\n              \"horaInicio\": \"14:20\",\r\n              \"horaFin\": \"14:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016787,\r\n              \"horaInicio\": \"14:40\",\r\n              \"horaFin\": \"15:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016788,\r\n              \"horaInicio\": \"15:00\",\r\n              \"horaFin\": \"15:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016789,\r\n              \"horaInicio\": \"15:20\",\r\n              \"horaFin\": \"15:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016790,\r\n              \"horaInicio\": \"15:40\",\r\n              \"horaFin\": \"16:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016791,\r\n              \"horaInicio\": \"16:00\",\r\n              \"horaFin\": \"16:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016792,\r\n              \"horaInicio\": \"16:20\",\r\n              \"horaFin\": \"16:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016793,\r\n              \"horaInicio\": \"16:40\",\r\n              \"horaFin\": \"17:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016794,\r\n              \"horaInicio\": \"17:00\",\r\n              \"horaFin\": \"17:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016795,\r\n              \"horaInicio\": \"17:20\",\r\n              \"horaFin\": \"17:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016796,\r\n              \"horaInicio\": \"17:40\",\r\n              \"horaFin\": \"18:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016797,\r\n              \"horaInicio\": \"18:00\",\r\n              \"horaFin\": \"18:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016798,\r\n              \"horaInicio\": \"18:20\",\r\n              \"horaFin\": \"18:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016799,\r\n              \"horaInicio\": \"18:40\",\r\n              \"horaFin\": \"19:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016800,\r\n              \"horaInicio\": \"19:00\",\r\n              \"horaFin\": \"19:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016801,\r\n              \"horaInicio\": \"19:20\",\r\n              \"horaFin\": \"19:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16016802,\r\n              \"horaInicio\": \"19:40\",\r\n              \"horaFin\": \"20:00\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"codigoSucursal\": 1,\r\n      \"nombreSucursal\": \"MediLink Sur\",\r\n      \"codigoMedico\": 244,\r\n      \"tipoIdentificacionMedico\": \"C\",\r\n      \"numeroIdentificacionMedico\": \"1756862775\",\r\n      \"nombreMedico\": \"SANCHEZ BARREDA JANYS\",\r\n      \"disponibilidad\": [\r\n        {\r\n          \"fechaDisponibilidad\": \"2025-02-19\",\r\n          \"horario\": [\r\n            {\r\n              \"idHorarioDisponible\": 16005884,\r\n              \"horaInicio\": \"10:20\",\r\n              \"horaFin\": \"10:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005885,\r\n              \"horaInicio\": \"10:40\",\r\n              \"horaFin\": \"11:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005886,\r\n              \"horaInicio\": \"11:00\",\r\n              \"horaFin\": \"11:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005887,\r\n              \"horaInicio\": \"11:20\",\r\n              \"horaFin\": \"11:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005888,\r\n              \"horaInicio\": \"11:40\",\r\n              \"horaFin\": \"12:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005892,\r\n              \"horaInicio\": \"13:00\",\r\n              \"horaFin\": \"13:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005893,\r\n              \"horaInicio\": \"13:20\",\r\n              \"horaFin\": \"13:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005894,\r\n              \"horaInicio\": \"13:40\",\r\n              \"horaFin\": \"14:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005895,\r\n              \"horaInicio\": \"14:00\",\r\n              \"horaFin\": \"14:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005896,\r\n              \"horaInicio\": \"14:20\",\r\n              \"horaFin\": \"14:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005897,\r\n              \"horaInicio\": \"14:40\",\r\n              \"horaFin\": \"15:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005898,\r\n              \"horaInicio\": \"15:00\",\r\n              \"horaFin\": \"15:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005899,\r\n              \"horaInicio\": \"15:20\",\r\n              \"horaFin\": \"15:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005900,\r\n              \"horaInicio\": \"15:40\",\r\n              \"horaFin\": \"16:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005901,\r\n              \"horaInicio\": \"16:00\",\r\n              \"horaFin\": \"16:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005902,\r\n              \"horaInicio\": \"16:20\",\r\n              \"horaFin\": \"16:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16005903,\r\n              \"horaInicio\": \"16:40\",\r\n              \"horaFin\": \"17:00\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"codigoSucursal\": 1,\r\n      \"nombreSucursal\": \"MediLink Sur\",\r\n      \"codigoMedico\": 269,\r\n      \"tipoIdentificacionMedico\": \"C\",\r\n      \"numeroIdentificacionMedico\": \"0959780883\",\r\n      \"nombreMedico\": \"VIDAL PEREZ ZORAIDA\",\r\n      \"disponibilidad\": [\r\n        {\r\n          \"fechaDisponibilidad\": \"2025-02-19\",\r\n          \"horario\": [\r\n            {\r\n              \"idHorarioDisponible\": 16011329,\r\n              \"horaInicio\": \"10:20\",\r\n              \"horaFin\": \"10:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011330,\r\n              \"horaInicio\": \"10:40\",\r\n              \"horaFin\": \"11:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011331,\r\n              \"horaInicio\": \"11:00\",\r\n              \"horaFin\": \"11:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011332,\r\n              \"horaInicio\": \"11:20\",\r\n              \"horaFin\": \"11:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011333,\r\n              \"horaInicio\": \"11:40\",\r\n              \"horaFin\": \"12:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011337,\r\n              \"horaInicio\": \"13:00\",\r\n              \"horaFin\": \"13:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011338,\r\n              \"horaInicio\": \"13:20\",\r\n              \"horaFin\": \"13:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011339,\r\n              \"horaInicio\": \"13:40\",\r\n              \"horaFin\": \"14:00\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011340,\r\n              \"horaInicio\": \"14:00\",\r\n              \"horaFin\": \"14:20\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011341,\r\n              \"horaInicio\": \"14:20\",\r\n              \"horaFin\": \"14:40\"\r\n            },\r\n            {\r\n              \"idHorarioDisponible\": 16011342,\r\n              \"horaInicio\": \"14:40\",\r\n              \"horaFin\": \"15:00\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"mensajes\": [\r\n    \"Medicos Disponibles: 4\"\r\n  ]\r\n}";

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

        public string PostCrearCita(string url,string token,string cita)
        {
            string responseCita = "";

            try
            {
                HttpClient _agendar = new HttpClient();
                _agendar.BaseAddress = new Uri(url);
                HttpContent _content = new StringContent(cita, Encoding.UTF8, "application/json");
                _agendar.DefaultRequestHeaders.Add("Accept", "*/*");
                _agendar.DefaultRequestHeaders.Add("rucEmpresa", "1792206979001");
                _agendar.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resCita = _agendar.PostAsync("api/CrearCita", _content).Result;

                if (resCita.IsSuccessStatusCode)
                {
                    responseCita = resCita.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    MessageBox.Show(resCita.StatusCode.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {

                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MediLinkApi.cs/PostCrearCita", mensaje, 361);
            }


            return responseCita;
        }
    }
}