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
    public class ProyectoController : BaseController
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

        public JsonResult ListarClientes()
        {

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
            var consulta = from cliente in db.Empresas
                            join contacto in db.Contactos
                            on cliente.EmpresaId equals contacto.EmpresaId
                            join proyecto in db.Proyectos
                            on contacto.ContactoId equals proyecto.ContactoId
                           select new
                            {
                                EmpresaId = cliente.EmpresaId,
                                Nombre = cliente.Nombre
                            };

            var resultado = consulta.Distinct().Select(d => new ListaEmpresasViewModel
            {
                EmpresaId = d.EmpresaId,
                Nombre = d.Nombre
            });

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProyectos(ParametroConsultaProyectoViewModel parametro)
        {

            int? empresaId = null;
            string descripcion = null; 
            
            if (parametro != null)
            {
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
                                 EstadoProyectoId = proyecto.EstadoId,
                                 Encargado = encargado.Nombres + " " + encargado.Apellidos 
                             };

            if (!String.IsNullOrWhiteSpace(descripcion))
            {
                proyectos2 = proyectos2.Where(p => p.Descripcion.ToLower().Contains(descripcion.ToLower()));
            }

            resultado = proyectos2.OrderByDescending(x=>x.ProyectoId).ToList();

            resultado.ForEach(x => x.Estado = this.EstadosProyecto[x.EstadoProyectoId].Descripcion);

            //var proyectos = new List<ListaProyectoViewModel>();
            //proyectos.Add(new ListaProyectoViewModel { ProyectoId = 1, Cliente = "Pepo", Descripcion = "Miro", Estado = "Cerrao", Encargado = "CDLL1" });
            //proyectos.Add(new ListaProyectoViewModel { ProyectoId = 2, Cliente = "Papa", Descripcion = "Nada", Estado = "Abierto", Encargado = "CDLL2" });

            return Json(resultado);
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
        public JsonResult Create(RegistroProyectoViewModel proyecto)
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
                oProyecto.EstadoId = this.EstadosProyecto[(int)EstadoProyectoEnum.PorConfigurar].EstadoId;
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

            return Json(true);
        }

        //
        // GET: /Proyecto/Edit/5

        public ActionResult Edit(int id)
        {
            var proyectoViewModel = new EdicionProyectoViewModel();
            Proyecto proyecto = db.Proyectos.Find(id);
            proyectoViewModel.ProyectoId = proyecto.ProyectoId;
            proyectoViewModel.EsTotalmenteEditable = ((int)EstadoProyectoEnum.PorConfigurar == proyecto.EstadoId);

            return View(proyectoViewModel); 
        }

        //
        // POST: /Proyecto/BuscarProyecto/5

        public JsonResult BuscarProyecto(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);

            var proyectoViewModel = new EdicionProyectoViewModel();

            proyectoViewModel.ContactoId = proyecto.ContactoId;
            proyectoViewModel.EmpresaId = proyecto.Contacto.EmpresaId;
            proyectoViewModel.Mnemonico = proyecto.Mnemonico;
            proyectoViewModel.Nombre = proyecto.Nombre;
            proyectoViewModel.InicioEstimado = proyecto.InicioEstimado;
            proyectoViewModel.FinEstimado = proyecto.FinEstimado;
            proyectoViewModel.HorasEstimadas = proyecto.HorasEstimadas;
            proyectoViewModel.Integrantes = new List<IntegranteProyectoViewModel>();

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
        // POST: /Proyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            proyecto.EsEliminado = true;
            db.SaveChanges();
            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}