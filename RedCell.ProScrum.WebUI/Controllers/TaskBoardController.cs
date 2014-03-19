using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.ViewModels.Proyecto;
using RedCell.ProScrum.WebUI.ViewModels.TaskBoard;
using WebMatrix.WebData;
using RedCell.ProScrum.WebUI.Filters;

namespace RedCell.ProScrum.WebUI.Controllers
{
    [InitializeSimpleMembership]
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
                                     where (integranteProyecto.IntegranteId == WebSecurity.CurrentUserId)
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
            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var userStories = from userStory in db.UserStories
                              where userStory.ProyectoId == id
                              && userStory.EsEliminado == false
                              select new UserStoryCompactViewModel
                              {
                                  UserStoryId = userStory.UserStoryId,
                                  Codigo = userStory.Codigo,
                                  Descripcion = userStory.Descripcion,
                                  EstaBloqueada = (from bloqueo in db.Bloqueos
                                                   where bloqueo.UserStoryId == userStory.UserStoryId
                                                   && bloqueo.EsEliminado == false
                                                   select bloqueo.BloqueoId).Count() > 0,
                                  NumeroActividadTerminada = (from actividadTerminada in db.Actividades
                                                              where actividadTerminada.UserStoryId == userStory.UserStoryId
                                                              && actividadTerminada.EstadoId == estadoTerminadoActividad
                                                              && actividadTerminada.EsEliminado == false
                                                              select actividadTerminada).Count(),
                                  NumeroActividadTotal = (from actividadTotal in db.Actividades
                                                          where actividadTotal.UserStoryId == userStory.UserStoryId
                                                          && actividadTotal.EsEliminado == false
                                                          select actividadTotal).Count(),
                                  EstadoUserStoryId = userStory.EstadoId,
                                  Color = userStory.Color
                              };

            var estadoUserStories = from estadoUserStory in db.EstadoUserStories
                                    select new BoardColumnViewModel
                                    {
                                        BoardColumnId = estadoUserStory.EstadoUserStoryId,
                                        Descripcion = estadoUserStory.Descripcion
                                    };

            return Json(new { BoardColumns = estadoUserStories, UserStories = userStories });
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
                                           Color = userStory.Color,
                                           Usuario = WebSecurity.CurrentUserName,
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

        [HttpPost]
        public JsonResult ChangeUserStoryColor(int usid, int? color)
        {

            var element = (from userStory in db.UserStories
                            where userStory.UserStoryId == usid
                            select userStory).FirstOrDefault();

            element.Color = color;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }


            return Json(new { UserStoryId = usid, NewColor = color });
        }

        [HttpPost]
        public JsonResult AddActivity(string descripcion, int uid)
        {
            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var element = new Actividad();
            element.Descripcion = descripcion;
            element.EsEliminado = false;
            element.EstadoId = this.EstadosActividadStory[(int)EstadoActividadEnum.Definido].EstadoId;
            element.TipoActividadId = (int)TipoActividadEnum.Desarrollo;
            element.UserStoryId = uid;
            element.FechaRegistro = System.DateTime.Now;
            element.UsuarioId = WebSecurity.CurrentUserId;

            db.Actividades.Add(element);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }

            var cantidadActividadTotal = (from actividadTerminada in db.Actividades
                                          where actividadTerminada.UserStoryId == uid
                                          && actividadTerminada.EsEliminado == false
                                          select actividadTerminada).Count();

            var cantidadActividadTerminada = (from actividadTerminada in db.Actividades
                                              where actividadTerminada.UserStoryId == uid
                                              && actividadTerminada.EstadoId == estadoTerminadoActividad
                                              && actividadTerminada.EsEliminado == false
                                              select actividadTerminada).Count();

            return Json(new { Descripcion = descripcion, ActividadId = element.ActividadId, UserStoryId = uid, NumeroActividadTerminada = cantidadActividadTerminada, NumeroActividadTotal = cantidadActividadTotal });
        }

        [HttpPost]
        public JsonResult DeleteActivity(int aid)
        {
            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var element = (from activity in db.Actividades
                           where activity.ActividadId == aid
                           select activity).FirstOrDefault();

            element.EsEliminado = true;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }

            var cantidadActividadTotal = (from actividadTerminada in db.Actividades
                                          where actividadTerminada.UserStoryId == element.UserStoryId
                                          && actividadTerminada.EsEliminado == false
                                          select actividadTerminada).Count();

            var cantidadActividadTerminada = (from actividadTerminada in db.Actividades
                                              where actividadTerminada.UserStoryId == element.UserStoryId
                                              && actividadTerminada.EstadoId == estadoTerminadoActividad
                                              && actividadTerminada.EsEliminado == false
                                              select actividadTerminada).Count();

            return Json(new { Descripcion = element.Descripcion, ActividadId = element.ActividadId, UserStoryId = element.UserStoryId, NumeroActividadTerminada = cantidadActividadTerminada, NumeroActividadTotal = cantidadActividadTotal });
        }

        [HttpPost]
        public JsonResult EndActivity(int aid)
        {
            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var element = (from activity in db.Actividades
                           where activity.ActividadId == aid
                           select activity).FirstOrDefault();

            element.EstadoId = this.EstadosActividadStory[(int)EstadoActividadEnum.Terminado].EstadoId;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }

            var cantidadActividadTotal = (from actividadTerminada in db.Actividades
                                          where actividadTerminada.UserStoryId == element.UserStoryId
                                          && actividadTerminada.EsEliminado == false
                                          select actividadTerminada).Count();

            var cantidadActividadTerminada = (from actividadTerminada in db.Actividades
                                              where actividadTerminada.UserStoryId == element.UserStoryId
                                              && actividadTerminada.EstadoId == estadoTerminadoActividad
                                              && actividadTerminada.EsEliminado == false
                                              select actividadTerminada).Count();

            return Json(new { Descripcion = element.Descripcion, ActividadId = element.ActividadId, UserStoryId = element.UserStoryId, NumeroActividadTerminada = cantidadActividadTerminada, NumeroActividadTotal = cantidadActividadTotal });
        
        }

        [HttpPost]
        public JsonResult AssignUserStory(int usid)
        {
            int nuevoEstadoUserStory = this.EstadosUserStory[(int)EstadoUserStoryEnum.InProcess].EstadoId;

            var element = (from userStory in db.UserStories
                           where userStory.UserStoryId == usid
                           select userStory).FirstOrDefault();

            element.ResponsableId = WebSecurity.CurrentUserId;
            element.EstadoId = nuevoEstadoUserStory;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }

            return Json(new { NombreUsuario = WebSecurity.CurrentUserName, NuevoEstadoUserStory = nuevoEstadoUserStory, UserStoryId = usid });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
