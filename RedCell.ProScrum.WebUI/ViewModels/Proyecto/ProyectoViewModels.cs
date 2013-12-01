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

    public class ParametroConsultaProyectoViewModel
    {
        public int? empresaId { get; set; }
        public string descripcion { get; set; }
    }
}