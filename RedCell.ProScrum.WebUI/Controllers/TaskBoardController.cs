using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.ViewModels.Proyecto;
using RedCell.ProScrum.WebUI.ViewModels.TaskBoard;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class TaskBoardController : BaseController
    {
        private ProScrumContext db = new ProScrumContext();

        //
        // GET: /TaskBoard/

        public ActionResult Index()
        {
            int estadoEnProgreso = (int)EstadosProyecto[(int)EstadoProyectoEnum.EnProgreso].EstadoId;

            var proyectoEnProgreso = from proyecto in db.Proyectos
                                     join integranteProyecto in db.IntegrantesProyecto
                                     on proyecto.ProyectoId equals integranteProyecto.ProyectoId
                                     where (integranteProyecto.EsEncargado == true
                                         && integranteProyecto.IntegranteId == EncargadoId)
                                        && proyecto.EsEliminado == false
                                        && proyecto.EstadoId == estadoEnProgreso
                                     select proyecto;

            if (!proyectoEnProgreso.Any() || proyectoEnProgreso.Count() > 1)
            {
                return RedirectToAction("ListToBoard");
            }
            else
            {
                return RedirectToAction("Board", new { id = proyectoEnProgreso.First().ProyectoId });
            }

        }

        [HttpGet]
        public ActionResult Board(int id)
        {

            var model = new BoardViewModel();
            model.ProyectoId = id;

            return View(model);
        }

        [HttpPost]
        public JsonResult ListBoardUserStories(int id)
        {
            var userStories = from userStory in db.UserStories
                              join actividades in db.Actividades
                              on userStory.UserStoryId equals actividades.UserStoryId into actividadesLeft
                              from subActividades in actividadesLeft.DefaultIfEmpty()
                              where userStory.ProyectoId == id
                              && userStory.EsEliminado == false
                              select new UserStoryCompactViewModel
                              {
                                  UserStoryId = userStory.UserStoryId,
                                  Codigo = userStory.Codigo,
                                  Descripcion = userStory.Descripcion,
                                  EstaBloqueada = true,
                                  NumeroActividadTerminada = 4,
                                  NumeroActividadTotal = 5,
                                  EstadoUserStoryId = userStory.EstadoId,
                                  Color = 0
                              };

            var estadoUserStories = from estadoUserStory in db.EstadoUserStories
                                    select new BoardColumnViewModel
                                    {
                                        BoardColumnId = estadoUserStory.EstadoUserStoryId,
                                        Descripcion = estadoUserStory.Descripcion
                                    };

            return Json(new { BoardColumns = estadoUserStories, UserStories = userStories });
        }

        public ActionResult ListToBoard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserStoryDetail(int id)
        {
            var model = new UserStoryDetailViewModel();

            var userStoryEncontrado = (from userStory in db.UserStories
                                       join usuario in db.Usuarios
                                       on userStory.ResponsableId equals usuario.UsuarioId into usuariosLeft
                                       from subUsuario in usuariosLeft.DefaultIfEmpty()
                                       where userStory.UserStoryId == id
                                       && userStory.EsEliminado == false
                                       select new UserStoryDetailViewModel
                                       {
                                           UserStoryId = userStory.UserStoryId,
                                           Codigo = userStory.Codigo,
                                           Descripcion = userStory.Descripcion,
                                           Color = null,
                                           Usuario = subUsuario.Nombres + " " + subUsuario.Apellidos,
                                           UsuarioId = subUsuario.UsuarioId
                                       }).FirstOrDefault();

            var actividades = from actividad in db.Actividades
                              where actividad.UserStoryId == id
                              && actividad.EsEliminado == false
                              orderby actividad.ActividadId descending
                              select new 
                              {
                                  ActividadId = actividad.ActividadId,
                                  Descripcion = actividad.Descripcion,
                                  EstadoActividadId = actividad.EstadoId
                              };

            userStoryEncontrado.ListaActividades = new List<ActividadesViewModel>();

            if (actividades.Any())
            { 
                var estadoTerminado = this.EstadosActividadStory[(int)EstadoActividadEnum.Terminado].EstadoId;

                foreach (var elemento in actividades)
                {
                    userStoryEncontrado.ListaActividades.Add(new ActividadesViewModel
                    {
                        ActividadId = elemento.ActividadId,
                        Descripcion = elemento.Descripcion,
                        Terminado = (estadoTerminado == elemento.EstadoActividadId) ? true : false
                    });
                }
            }

            //model.ListaActividades = new List<ActividadesViewModel>();
            //model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 1, Descripcion = "Descripción de la primera Actividad 01", Terminado = true });
            //model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 2, Descripcion = "Descripción de la segunda Actividad 02", Terminado = true });
            //model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 3, Descripcion = "Descripción de la tercera Actividad 03", Terminado = false });

            return PartialView(userStoryEncontrado);
        }

    }
}
