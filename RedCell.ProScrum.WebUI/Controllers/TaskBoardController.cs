using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
