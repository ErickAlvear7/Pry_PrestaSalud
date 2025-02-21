using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pry_PrestasaludWAP.Modelo
{
    public class MediLinkModel
    {

        public class LoginApi
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class DatoObj
        {
            public List<Dato> datos { get; set; }

        }

        public class Dato
        {
            public string codCiudad { get; set; }
            public string distritoProvincia { get; set; }
            public string nombreCiudad { get; set; }
        }

        public class EspeObj
        {
            public List<Especialidad> datos { get; set; }
        }

        public class Especialidad
        {
            public string codEspecialidad { get; set; }
            public string codSucursal { get; set; }
            public string espNombre { get; set; }
        }

        public class Paciente
        {
            public string tipoIdentificacion { get; set; }
            public string numeroIdentificacion { get; set; }
            public string primerNombre { get; set; }
            public string segundoNombre { get; set; }
            public string primerApellido { get; set; }
            public string segundoApellido { get; set; }
            public string fechaNacimiento { get; set; }
            public string email { get; set; }
            public string sexo { get; set; }
            public string telefonioMovil { get; set; }

        }

        public class Admision
        {

            public string identificacion { get; set; }
            public string empresaAdmision { get; set; }

        }

        public class Sucursal
        {
            public string idSucursal { get; set; }
            public string nombreCentromedico { get; set; }
        }

        public class SucursalObj
        {
            public List<Sucursal> datos { get; set; }
        }

        public class Medicos
        {
            public string idMedico { get; set; }
            public string nombreCompleto { get; set; }
        }

        public class MedicoObj
        {
            public List<Medicos> datos { get; set; }
        }


        public class Disponibilidad 
        {
            //public List<DatosSucursal> datossucursal { get; set; }
            public string codigoSucursal { get; set; }
            public string nombreSucursal { get; set; }
            public string codigoMedico { get; set; }
            public string tipoIdentificacionMedico { get; set; }
            public string numeroIdentificacionMedico { get; set; }
            public string nombreMedico { get; set; }
            public List<Disponibles> disponibilidad { get; set; }
        }

        public class DisponibilidadObj 
        {
            public string estado { get; set; }
            public MensajesDispo mensajex { get; set; }
            public List<Disponibilidad> datos { get; set; }
        }
        public class MensajesDispo 
        {
            public string mensajes { get; set; }
        }
        public class Disponibles 
        {
            public string fechaDisponibilidad { get; set; }
            public List<Horarios> horario { get; set; }
        }

        public class DatosSucursal 
        {
            public string codigoSucursal { get; set; }
            public string nombreSucursal { get; set; }
            public string codigoMedico { get; set; }
            public string tipoIdentificacionMedico { get; set; }
            public string numeroIdentificacionMedico { get; set; }
            public string nombreMedico { get; set; }
        }

        public class Horarios 
        {
            public string idHorarioDisponible { get; set; }
            public string horaInicio { get; set; }
            public string horaFin { get; set; }
        }

        public class HorarioDisponible
        {
            public int idHorarioDisponible { get; set; }
        }

        public class CrearCita
        {
            public int idPaciente { get; set; }
            public int idCiudad { get; set; }
            public int idMedico { get; set; }
            public int idSucursal { get; set; }
            public int idEspecialidad { get; set; }
            public HorarioDisponible horarioDisponible { get; set; }
        }

    }
}