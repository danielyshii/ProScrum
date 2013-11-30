using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedCell.ProScrum.WebUI.Models;

namespace RedCell.ProScrum.WebUI.Controllers
{
    public class ProyectoController : Controller
    {
        private DB_PROSCRUMContext db = new DB_PROSCRUMContext();

        //
        // GET: /Proyecto/

        public ActionResult Index()
        {
            var proyectoes = db.Proyectoes.Include(p => p.Contacto).Include(p => p.Usuario);
            return View(proyectoes.ToList());
        }

        //
        // GET: /Proyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            Proyecto proyecto = db.Proyectoes.Find(id);
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
            ViewBag.ContactoId = new SelectList(db.Contactoes, "ContactoId", "Nombres");
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
                db.Proyectoes.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContactoId = new SelectList(db.Contactoes, "ContactoId", "Nombres", proyecto.ContactoId);
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo", proyecto.JefeProyectoId);
            return View(proyecto);
        }

        //
        // GET: /Proyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Proyecto proyecto = db.Proyectoes.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactoId = new SelectList(db.Contactoes, "ContactoId", "Nombres", proyecto.ContactoId);
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
            ViewBag.ContactoId = new SelectList(db.Contactoes, "ContactoId", "Nombres", proyecto.ContactoId);
            ViewBag.JefeProyectoId = new SelectList(db.Usuarios, "UsuarioId", "Codigo", proyecto.JefeProyectoId);
            return View(proyecto);
        }

        //
        // GET: /Proyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Proyecto proyecto = db.Proyectoes.Find(id);
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
            Proyecto proyecto = db.Proyectoes.Find(id);
            db.Proyectoes.Remove(proyecto);
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