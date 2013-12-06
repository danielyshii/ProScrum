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
        public Dictionary<int, Estado> EstadosSprint { get; set; }
        public Dictionary<int, Estado> EstadosUserStory { get; set; }
        public Dictionary<int, Estado> EstadosActividadStory { get; set; }

        public int JefeProyectoId = 4;

        public int EncargadoId = 5;

        public BaseController() {
            EstadosProyecto = new Dictionary<int, Estado>();
            EstadosProyecto.Add((int)EstadoProyectoEnum.PorConfigurar, new Estado { EstadoId = 1, Descripcion = "Por configurar" });
            EstadosProyecto.Add((int)EstadoProyectoEnum.Configurado, new Estado { EstadoId = 2, Descripcion = "Configurado" });
            EstadosProyecto.Add((int)EstadoProyectoEnum.EnProgreso, new Estado { EstadoId = 3, Descripcion = "En Progreso" });

            EstadosSprint = new Dictionary<int, Estado>();
            EstadosSprint.Add((int)EstadoProyectoEnum.PorConfigurar, new Estado { EstadoId = 1, Descripcion = "Definido" });

            EstadosUserStory = new Dictionary<int, Estado>();
            EstadosUserStory.Add((int)EstadoUserStoryEnum.Definido, new Estado { EstadoId = 1, Descripcion = "Definido" });

            EstadosActividadStory = new Dictionary<int, Estado>();
            EstadosActividadStory.Add((int)EstadoActividadEnum.Definido, new Estado { EstadoId = 1, Descripcion = "Definido" });
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

    public enum EstadoSprintEnum
    {
        Definido = 1
    }

    public enum EstadoUserStoryEnum
    {
        Definido = 1
    }

    public enum EstadoActividadEnum
    {
        Definido = 1
    }

    public enum TipoActividadEnum
    {
        Analisis = 1,
        Desarrollo = 2,
        Calidad = 3
    }
}
