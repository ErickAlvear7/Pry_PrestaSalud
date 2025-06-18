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
            string tokens = "";
            try
            {
                HttpClient _client = new HttpClient();
                
                _client.BaseAddress = new Uri(url);
                _client.DefaultRequestHeaders.Add("Accept", "application/json");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpContent _content = new StringContent(_apikey, Encoding.UTF8, "application/json");

                var _response = _client.PostAsync("apikey/login", _content).Result;

                if (_response.IsSuccessStatusCode)
                {
                    var responseContent = _response.Content.ReadAsStringAsync().Result;
                    dynamic token = JObject.Parse(responseContent);
                    tokens = token.token;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetToken","token generado: " + tokens, 35);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetToken", _response.StatusCode.ToString(), 35);
                }
            }
            catch (Exception ex)
            {            
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetToken", ex.ToString(), 40);
            }

            return tokens;
        }

        public string GetIdContract(string url, string auth)
        {
            string responseId = "";
            try
            {
                HttpClient _contract = new HttpClient();
                
                _contract.BaseAddress = new Uri(url);
                _contract.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var resConId = _contract.GetAsync("contratos").Result;

                if (resConId.IsSuccessStatusCode)
                {
                    responseId = resConId.Content.ReadAsStringAsync().Result;
                    //dynamic idcont = JArray.Parse(responseId);
                    //idespe = idcont[0].id;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetIdContract","ContractID generados: " + responseId, 66);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetIdContract", resConId.StatusCode.ToString(), 70);
                }
            }
            catch (Exception ex)
            {
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetIdContract", ex.ToString(), 74);
            }

            return responseId;
        }

        public string GetServicios(string url, string idcont, string auth)
        {
            string ideser = "";
            try
            {
                HttpClient _servicio = new HttpClient();
                
                _servicio.BaseAddress = new Uri(url);
                _servicio.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resServId = _servicio.GetAsync("services/" + idcont).Result;

                if (resServId.IsSuccessStatusCode)
                {
                    var serviceId = resServId.Content.ReadAsStringAsync().Result;

                    dynamic dataSer = JArray.Parse(serviceId);
                    ideser = dataSer[0].id;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetServicios", "ServicioID generado: " + ideser, 100);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetServicios", resServId.StatusCode.ToString(), 104);
                }

            }
            catch (Exception ex)
            {
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetServicios", ex.ToString(), 110);
            }
          
            return ideser;
        }

        public string GetEspecialidad(string url, string auth, string idcont)
        {
            string idespe = "";
            try
            {
                HttpClient _espec = new HttpClient();
                
                _espec.BaseAddress = new Uri(url);
                _espec.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var resEspeId = _espec.GetAsync("specialties/" + idcont).Result;

                if (resEspeId.IsSuccessStatusCode)
                {
                    var espeId = resEspeId.Content.ReadAsStringAsync().Result;

                    dynamic dataEsp = JArray.Parse(espeId);
                    idespe = dataEsp[0].id;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetEspecialidad", "EspecialidadID generado: " + idespe, 135);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetEspecialidad", resEspeId.StatusCode.ToString(), 139);
                }
            }
            catch (Exception ex)
            {
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetEspecialidad", ex.ToString(), 144);
            }
         
            return idespe;
        }

        public string PostCreatePatient(string url, string dataPatient, string auth)
        {
            string idpatient = "";
            try
            {
                HttpClient _patient = new HttpClient();
                
                _patient.BaseAddress = new Uri(url);
                _patient.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpContent _content = new StringContent(dataPatient, Encoding.UTF8, "application/json");
                var _resPatient = _patient.PostAsync("patient", _content).Result;

                if (_resPatient.IsSuccessStatusCode)
                {
                    var responseContent = _resPatient.Content.ReadAsStringAsync().Result;
                    dynamic idpat = JObject.Parse(responseContent);
                    idpatient = idpat.patient.id;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/PostCreatePatient", "PatientID generado: " + idpatient, 170);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/PostCreatePatient", _resPatient.StatusCode.ToString(), 174);
                }             
            }
            catch (Exception ex)
            {
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/PostCreatePatient", ex.ToString(), 179);
            }

            return idpatient;
        }

        public string Consultas(string url, string datacon, string auth)
        {
            string responseContent = "";
            try
            {
                HttpClient _consulta = new HttpClient();
                
                _consulta.BaseAddress = new Uri(url);
                _consulta.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
              
                var queryString = new StringBuilder();
                queryString.Append("?notify=").Append(Uri.EscapeDataString("false"));
                queryString.Append("&notifyDoctor=").Append(Uri.EscapeDataString("true"));

                HttpContent _content = new StringContent(datacon, Encoding.UTF8, "application/json");
                    var _resConsulta = _consulta.PostAsync("consulta", _content).Result;

                if (_resConsulta.IsSuccessStatusCode)
                {
                    responseContent = _resConsulta.Content.ReadAsStringAsync().Result;
                    new Funciones().LogAuditoriaTele(5, "MethodApi.cs/Consultas", "Datos consulta: " + responseContent, 207);
                }
                else
                {
                    responseContent = "Horario";
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/Consultas", _resConsulta.StatusCode.ToString(), 212);
                }             
            }
            catch (Exception ex)
            {
                new Funciones().LogAuditoriaTele(1, "MethodApi.cs/Consultas", ex.ToString(), 218);
            }

            return responseContent;
        }

        public string GetMedicos(string url,string auth,string fecha,string idpaciente,string idservicio,string idespeci)
        {
            string responseHorarios = "";
            try
            {
                HttpClient _medicos = new HttpClient();
                
                _medicos.BaseAddress = new Uri(url);
                _medicos.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var queryString = new StringBuilder();
                queryString.Append("?fecha=").Append(Uri.EscapeDataString("2025-02-14"));
                queryString.Append("&id_paciente=").Append(Uri.EscapeDataString(idpaciente));
                queryString.Append("&id_servicio=").Append(Uri.EscapeDataString(idservicio));
                queryString.Append("&id_especialidad=").Append(Uri.EscapeDataString(idespeci));

                var resMedicos = _medicos.GetAsync("consultas/schedule/" + queryString).Result;

                if (resMedicos.IsSuccessStatusCode)
                {
                    responseHorarios = resMedicos.Content.ReadAsStringAsync().Result;
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetMedicos", "Horarios medicos: " + resMedicos, 245);
                }
                else
                {
                    new Funciones().LogAuditoriaTele(1, "MethodApi.cs/GetMedicos", resMedicos.StatusCode.ToString(), 249);
                }

            }
            catch (Exception ex)
            {
                var mensaje = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "MethodApi.cs/GetMedicos", mensaje, 256);
            }

            //responseHorarios = "{\r\n   \"medicos\":[\r\n      {\r\n         \"id_medico\":\"269efea0-e70d-4e89-8aac-84f89694509a\",\r\n         \"nombre\":\"Mónica\",\r\n         \"apellidos\":\"Prado Ramírez\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:33\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:45\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:20\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:42\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"39291aec-0beb-49a5-b441-defa040c35dc\",\r\n         \"nombre\":\"Salvador Alejandro\",\r\n         \"apellidos\":\"Gonzalez Chavez\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:36\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:48\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:36\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:48\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:36\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:48\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:36\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:48\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:36\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"4e4388eb-53f0-4ddd-82ef-af304512d3e5\",\r\n         \"nombre\":\"Alejandro\",\r\n         \"apellidos\":\"Domínguez Cuevas\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:42\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"22:54\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:06\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:18\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:30\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:42\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"5e7dc408-6aa6-4b46-af29-2492e3825de6\",\r\n         \"nombre\":\"Wendy Estefany\",\r\n         \"apellidos\":\"Luna Yunga\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"10:59\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:11\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:23\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:35\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:47\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:59\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:11\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:23\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:35\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:47\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:59\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:11\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:00\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:12\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:24\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:46\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"6e2ceca8-4c31-498e-a90f-1655f8fb0b5d\",\r\n         \"nombre\":\"Adriana Fernanda\",\r\n         \"apellidos\":\"Segura Zenón\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"23:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-28\",\r\n               \"hour\":\"00:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-28\",\r\n               \"hour\":\"00:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-28\",\r\n               \"hour\":\"00:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-28\",\r\n               \"hour\":\"00:46\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"83894592-3a59-45aa-8afa-3666d9289656\",\r\n         \"nombre\":\"Valeria\",\r\n         \"apellidos\":\"Mejía Martínez\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"10:59\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"11:11\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"12:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"13:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"14:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"15:46\"\r\n            }\r\n         ]\r\n      },\r\n      {\r\n         \"id_medico\":\"bec4d91c-92a6-4a71-b6c2-0c7822464da0\",\r\n         \"nombre\":\"Nathaly Pamela\",\r\n         \"apellidos\":\"Bolaños León\",\r\n         \"rangos\":[\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"16:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"17:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"18:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"19:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:46\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"20:58\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:10\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:22\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:34\"\r\n            },\r\n            {\r\n               \"date\":\"2025-02-27\",\r\n               \"hour\":\"21:46\"\r\n            }\r\n         ]\r\n      }\r\n   ]\r\n}";

            return responseHorarios;
        }

    }
}