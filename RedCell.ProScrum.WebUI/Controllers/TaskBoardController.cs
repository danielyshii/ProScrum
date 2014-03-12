using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.ViewModels.TaskBoard;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class TaskBoardController : Controller
    {
        //
        // GET: /TaskBoard/

        public ActionResult Index()
        {
            if (true)
            {
                return RedirectToAction("Board");
            }

            return View();
        }

        public ActionResult Board()
        {
            return View();
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
