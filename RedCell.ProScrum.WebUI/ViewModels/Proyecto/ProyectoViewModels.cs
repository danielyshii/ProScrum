using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.ViewModels.Proyecto
{
    public class ListaProyectoViewModel
    {
        public int ProyectoId { get; set; }
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Encargado { get; set; }
    }
}