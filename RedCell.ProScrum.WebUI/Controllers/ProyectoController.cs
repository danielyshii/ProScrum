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
            return View();
        }


        public JsonResult ListarCliente()
        {
            var resultado = from cliente in db.Empresas
                            join contacto in db.Contactos
                            on cliente.EmpresaId equals contacto.EmpresaId
                            join proyecto in db.Proyectos
                            on contacto.ContactoId equals proyecto.ContactoId
                            select new ListaEmpresasViewModel
                            {
                                EmpresaId = cliente.EmpresaId,
                                Nombre = cliente.Nombre
                            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProyectos(ParametroConsultaProyectoViewModel parametro)
        {

            int? empresaId = null;
            string descripcion = null; 
            
            if(parametro != null){
                empresaId = parametro.empresaId;
                descripcion = parametro.descripcion;
            }

            var resultado = new List<ListaProyectoViewModel>();

            var proyectos2 = from proyecto in db.Proyectos
                             join contacto in db.Contactos
                             on proyecto.ContactoId equals contacto.ContactoId
                             join empresa in db.Empresas.Where(x => x.EmpresaId == (empresaId.HasValue ? empresaId.Value : x.EmpresaId))
                             on contacto.EmpresaId equals empresa.EmpresaId
                             join integrante in db.IntegrantesProyecto
                             on proyecto.ProyectoId equals integrante.ProyectoId
                             join encargado in db.Usuarios
                             on integrante.IntegranteId equals encargado.UsuarioId
                             where proyecto.EsEliminado == false
                             && integrante.EsEncargado == true
                             select new ListaProyectoViewModel
                             {
                                 ProyectoId = proyecto.ProyectoId,
                                 Cliente = empresa.Nombre,
                                 Descripcion = proyecto.Nombre,
                                 Estado = "Cerrao!",
                                 Encargado = encargado.Nombres + " " + encargado.Apellidos 
                             };

            if (!String.IsNullOrWhiteSpace(descripcion))
            {
                proyectos2 = proyectos2.Where(p => p.Descripcion.ToLower().Contains(descripcion.ToLower()));
            }

            resultado = proyectos2.ToList();

            //var proyectos = new List<ListaProyectoViewModel>();
            //proyectos.Add(new ListaProyectoViewModel { ProyectoId = 1, Cliente = "Pepo", Descripcion = "Miro", Estado = "Cerrao", Encargado = "CDLL1" });
            //proyectos.Add(new ListaProyectoViewModel { ProyectoId = 2, Cliente = "Papa", Descripcion = "Nada", Estado = "Abierto", Encargado = "CDLL2" });

            return Json(resultado);
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