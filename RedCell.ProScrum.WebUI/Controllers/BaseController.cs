using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public Dictionary<int, Estado> EstadosProyecto { get; set; }

        public BaseController() {
            EstadosProyecto = new Dictionary<int, Estado>();
            
            EstadosProyecto.Add((int)EstadoProyectoEnum.PorConfigurar, new Estado { EstadoId = 1, Descripcion = "Por configurar" });
            EstadosProyecto.Add((int)EstadoProyectoEnum.Configurado, new Estado { EstadoId = 2, Descripcion = "Configurado" });
            EstadosProyecto.Add((int)EstadoProyectoEnum.EnProgreso, new Estado { EstadoId = 3, Descripcion = "En Progreso" });
        }
    }

    public class Estado
    {
        public int EstadoId { get; set; }
        public string Descripcion { get; set; }
    }

    public enum EstadoProyectoEnum
    {
        PorConfigurar = 1,
        Configurado = 2,
        EnProgreso = 3
    }
}
