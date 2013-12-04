﻿using System;
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

        public JsonResult ListarIntegrante(string patron)
        {
            var resultado = from integrante in db.Usuarios
                            where integrante.Nombres.ToLower().Contains(patron.ToLower()) ||
                            integrante.Apellidos.ToLower().Contains(patron.ToLower())
                            select new ListaIntegranteViewModel
                            {
                                IntegranteId = integrante.UsuarioId,
                                Nombre = integrante.Nombres + " " + integrante.Apellidos
                            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarContacto(int empresaId)
        {

            var resultado = from contacto in db.Contactos
                            where contacto.EmpresaId == empresaId
                            select new ListaContactoViewModel
                            {
                                ContactoId = contacto.ContactoId,
                                Nombre = contacto.Nombres + " " + contacto.Apellidos
                            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarClientes() {

            var resultado = from cliente in db.Empresas
                           select new ListaEmpresasViewModel
                           {
                               EmpresaId = cliente.EmpresaId,
                               Nombre = cliente.Nombre
                           };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarClienteConProyectos()
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
            return View();
        }

        //
        // POST: /Proyecto/Create

        //[ValidateAntiForgeryToken]
        [HttpPost]        
        public ActionResult Create(RegistroProyectoViewModel proyecto)
        {
            if (ModelState.IsValid)
            {
                var oProyecto = new Proyecto();

                oProyecto.ContactoId = proyecto.ContactoId;
                oProyecto.JefeProyectoId = 2;
                oProyecto.Nombre = proyecto.Nombre;
                oProyecto.Mnemonico = proyecto.Mnemonico;
                oProyecto.InicioEstimado = proyecto.InicioEstimado;
                oProyecto.FinEstimado = proyecto.FinEstimado;
                oProyecto.HorasEstimadas = proyecto.HorasEstimadas;
                oProyecto.EstadoId = 1;
                oProyecto.EsEliminado = false;

                foreach (var integrante in proyecto.Integrantes)
                {
                    var oIntegrante = new IntegranteProyecto();
                    oIntegrante.IntegranteId = integrante.IntegranteId;
                    oIntegrante.EsEncargado = integrante.EsEncargado;
                    oIntegrante.EsEliminado = false;
                    oProyecto.IntegranteProyectoes.Add(oIntegrante);
                }
                
                db.Proyectos.Add(oProyecto);
                db.SaveChanges();
                
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Proyecto/Edit/5
        [HttpPost]
        public JsonResult Edit(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);

            var proyectoViewModel = new EdicionProyectoViewModel();

    //public class IntegranteProyectoViewModel
    //{
    //    public int IntegranteId { get; set; }
    //    public string Nombre { get; set; }
    //    public bool EsEncargado { get; set; }
    //}

            proyectoViewModel.ContactoId = proyecto.ContactoId;
            proyectoViewModel.Mnemonico = proyecto.Mnemonico;
            proyectoViewModel.Nombre = proyecto.Nombre;
            proyectoViewModel.InicioEstimado = proyecto.InicioEstimado;
            proyectoViewModel.FinEstimado = proyecto.FinEstimado;
            proyectoViewModel.HorasEstimadas = proyecto.HorasEstimadas;

            foreach (var integrante in proyecto.IntegranteProyectoes)
            {
                var integranteViewModel = new IntegranteProyectoViewModel();
                integranteViewModel.IntegranteId = integrante.IntegranteId;
                integranteViewModel.Nombre = integrante.Usuario.Nombres + " " + integrante.Usuario.Apellidos;
                integranteViewModel.EsEncargado = integrante.EsEncargado;
                proyectoViewModel.Integrantes.Add(integranteViewModel);
            }

            return Json(proyectoViewModel);

        }

        //
        // POST: /Proyecto/Edit/5

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