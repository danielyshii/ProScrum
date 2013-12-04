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
        public int ContactoId { get; set; }
        public string Mnemonico { get; set; }
        public string Nombre { get; set; }
        public List<IntegranteProyectoViewModel> Integrantes { get; set; }
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
}