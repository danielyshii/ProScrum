using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class BaseController : Controller
    {
        private ProScrumContext db = new ProScrumContext();

        public Dictionary<int, Estado> EstadosProyecto { get; set; }
        public Dictionary<int, Estado> EstadosSprint { get; set; }
        public Dictionary<int, Estado> EstadosUserStory { get; set; }
        public Dictionary<int, Estado> EstadosActividadStory { get; set; }

        public int JefeProyectoId = 4;

        public int EncargadoId = 5;

        public BaseController() {
            CargarEstadoActividad();
            CargarEstadoProyecto();
            CargarEstadoSprint();
            CargarEstadoUserStory();
        }

        private void CargarEstadoActividad()
        {
            EstadosActividadStory = new Dictionary<int, Estado>();

            var EstadosActividadesQuery = from estado in db.EstadoActividades
                                       orderby estado.EstadoActividadId ascending
                                       select estado;

            foreach (var estado in EstadosActividadesQuery)
            {
                EstadosActividadStory.Add(estado.EstadoActividadId, new Estado { EstadoId = estado.EstadoActividadId, Descripcion = estado.Descripcion });
            }
        }

        private void CargarEstadoProyecto(){
            EstadosProyecto = new Dictionary<int, Estado>();

            var EstadosProyectoQuery = from estado in db.EstadoProyectos
                                       orderby estado.EstadoProyectoId ascending
                                       select estado;

            foreach (var estado in EstadosProyectoQuery)
            {
                EstadosProyecto.Add(estado.EstadoProyectoId, new Estado { EstadoId = estado.EstadoProyectoId, Descripcion = estado.Descripcion });
            }
        }

        private void CargarEstadoSprint()
        {
            EstadosSprint = new Dictionary<int, Estado>();

            var EstadosSprintQuery = from estado in db.EstadoSprints
                                       orderby estado.EstadoSprintId ascending
                                       select estado;

            foreach (var estado in EstadosSprintQuery)
            {
                EstadosSprint.Add(estado.EstadoSprintId, new Estado { EstadoId = estado.EstadoSprintId, Descripcion = estado.Descripcion });
            }
        }

        private void CargarEstadoUserStory()
        {
            EstadosUserStory = new Dictionary<int, Estado>();

            var EstadosUserStoryQuery = from estado in db.EstadoUserStories
                                     orderby estado.EstadoUserStoryId ascending
                                     select estado;

            foreach (var estado in EstadosUserStoryQuery)
            {
                EstadosUserStory.Add(estado.EstadoUserStoryId, new Estado { EstadoId = estado.EstadoUserStoryId, Descripcion = estado.Descripcion });
            }
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
        ToDo = 1,
        InProcess = 2,
        ToVerify = 3,
        Done = 4
    }

    public enum EstadoActividadEnum
    {
        Definido = 1,
        Terminado = 2
    }

    public enum TipoActividadEnum
    {
        Analisis = 1,
        Desarrollo = 2,
        Calidad = 3
    }
}
