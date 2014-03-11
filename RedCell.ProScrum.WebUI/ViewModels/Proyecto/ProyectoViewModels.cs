using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.ViewModels.Proyecto
{
    public class ListaProyectoViewModel
    {
        public int ProyectoId { get; set; }
        public int EmpresaId { get; set; }
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public int EstadoProyectoId { get; set; }
        public string Estado { get; set; }
        public string Encargado { get; set; }
    }

    public class ListaEmpresasViewModel
    {
        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
    }

    public class ListaContactoViewModel
    {
        public int ContactoId { get; set; }
        public string Nombre { get; set; }
    }

    public class ListaIntegranteViewModel
    {
        public int IntegranteId { get; set; }
        public string Nombre { get; set; }
    }

    public class ParametroConsultaProyectoViewModel
    {
        public int? empresaId { get; set; }
        public string descripcion { get; set; }
    }

    public class RegistroProyectoViewModel
    {
        public int ContactoId { get; set; }
        public string Mnemonico { get; set; }
        public string Nombre { get; set; }
        public List<IntegranteProyectoViewModel> Integrantes { get; set; }
        public DateTime InicioEstimado { get; set; }
        public DateTime FinEstimado { get; set; }
        public int HorasEstimadas { get; set; }
    }

    public class EdicionProyectoViewModel
    {
        public int ProyectoId { get; set; }
        public bool EsTotalmenteEditable { get; set; }
        public int ContactoId { get; set; }
        public int EmpresaId { get; set; }
        public string Mnemonico { get; set; }
        public string Nombre { get; set; }
        public List<IntegranteEdicionProyectoViewModel> Integrantes { get; set; }
        public DateTime InicioEstimado { get; set; }
        public DateTime FinEstimado { get; set; }
        public int HorasEstimadas { get; set; }
    }

    public class IntegranteProyectoViewModel
    {
        public int IntegranteId { get; set; }
        public string Nombre { get; set; }
        public bool EsEncargado { get; set; }
    }

    public class IntegranteEdicionProyectoViewModel
    {
        public int IntegranteId { get; set; }
        public string Nombre { get; set; }
        public bool EsEncargado { get; set; }
        public bool EsNuevo { get; set; }
    }

    public class ListaProyectoPorConfigurar
    {
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public string Cliente { get; set; }
        public string FechaRegistro { get; set; }
    }

    public class ListaProyectoEnProgreso
    {
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public string Cliente { get; set; }
        public string FechaRegistro { get; set; }
    }

    public class ConfigurarProyectoViewModel
    {

        public int ProyectoId { get; set; }
        public string NombreProyecto { get; set; }

        public int SemanasEstimadas { get; set; }
        public bool UsarSecuencial { get; set; }
        public string NombreSprintInicial { get; set; }
        public DateTime FechaInicioSprintInicial { get; set; }
        public string ObjetivoSprint { get; set; }
        public List<UserStoryViewModel> SprintUserStories { get; set; }
        public List<UserStoryViewModel> BacklogUserStories { get; set; }

    }

    public class UserStoryViewModel
    {
        public string DescripcionUserStory { get; set; }
        public int HorasEstimadas { get; set; }
        public bool PerteneceSprintInicial { get; set; }
        public List<ActividadViewModel> ActividadesUserStory { get; set; }
    }

    public class ActividadViewModel
    {
        public string Descripcion { get; set; }
    }
}