using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.ViewModels.Proyecto;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class ProyectoController : Controller
    {
        private ProScrumContext db = new ProScrumContext();

        //
        // GET: /Proyecto/

        public ActionResult Index()
        {
            var proyectoes = db.Proyectos.Include(p => p.Contacto).Include(p => p.Usuario);
            return View(proyectoes.ToList());
        }


        public JsonResult ListProyects()
        {
            var proyectos = new List<ListaProyectoViewModel>();
            proyectos.Add(new ListaProyectoViewModel { ProyectoId = 1, Cliente = "Pepo", Descripcion = "Miro", Estado= "Cerrao", Encargado="CDLL1" });
            proyectos.Add(new ListaProyectoViewModel { ProyectoId = 2, Cliente = "Papa", Descripcion = "Nada", Estado = "Abierto", Encargado = "CDLL2" });

            return Json(proyectos, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Proyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        //
        // GET: /Proyecto/Create

        public ActionResult Create()
        {
            ViewBag.ContactoId = new SelectList(db.Contactos, "ContactoId", "Nombres");
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo");
            return View();
        }

        //
        // POST: /Proyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Proyectos.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContactoId = new SelectList(db.Contactos, "ContactoId", "Nombres", proyecto.ContactoId);
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo", proyecto.JefeProyectoId);
            return View(proyecto);
        }

        //
        // GET: /Proyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactoId = new SelectList(db.Contactos, "ContactoId", "Nombres", proyecto.ContactoId);
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo", proyecto.JefeProyectoId);
            return View(proyecto);
        }

        //
        // POST: /Proyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactoId = new SelectList(db.Contactos, "ContactoId", "Nombres", proyecto.ContactoId);
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo", proyecto.JefeProyectoId);
            return View(proyecto);
        }

        //
        // GET: /Proyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        //
        // POST: /Proyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            db.Proyectos.Remove(proyecto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}