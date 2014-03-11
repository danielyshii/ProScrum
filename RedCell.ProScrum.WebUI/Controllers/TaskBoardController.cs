using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.ViewModels.Proyecto;

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

        public ActionResult Board(int? id)
        {
            return View();
        }

        public ActionResult ListToBoard()
        {
            return View();
        }

    }
}
