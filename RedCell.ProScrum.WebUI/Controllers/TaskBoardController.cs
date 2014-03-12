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
                return View(proyectoEnProgreso);
            }
            else
            {
                return RedirectToAction("Board", new { id = proyectoEnProgreso.First().ProyectoId });
            }

            }

        public ActionResult Board(int? id)
        {
            var userStories = from userStory in db.UserStories
                              join actividades in db.Actividades
                              on userStory.UserStoryId equals actividades.UserStoryId into actividadesLeft
                              from subActividades in actividadesLeft
                              select new UserStoryCompactViewModel { 
                                  UserStoryId = userStory.UserStoryId,
                                  EstaBloqueada = true,
                                  NumeroActividadTerminada = 4,
                                  NumeroActividadTotal = 5,
                                  Color = 0
                              };

            var model = userStories.ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult UserStoryDetail()
        {
            var model = new UserStoryDetailViewModel();
            model.UserStoryId = 1;
            model.Codigo = "PSKG-0001";
            model.Descripcion = "User Story de Prueba 01 Lorem ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum";
            model.Color = 1;
            model.UsuarioId = null;
            model.Usuario = "Daniel";

            model.ListaActividades = new List<ActividadesViewModel>();
            model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 1, Descripcion = "Descripción de la primera Actividad 01", Terminado = true });
            model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 2, Descripcion = "Descripción de la segunda Actividad 02", Terminado = true });
            model.ListaActividades.Add(new ActividadesViewModel { ActividadId = 3, Descripcion = "Descripción de la tercera Actividad 03", Terminado = false });

            return PartialView(model);
        }

    }
}
