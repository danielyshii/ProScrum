using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Configure(int? id)
        {
            if (id.HasValue)
            {
                var proyecto = db.Proyectos.Find(id.Value);

                var model = new ConfigurarProyectoViewModel();
                model.ProyectoId = proyecto.ProyectoId;
                model.NombreProyecto = proyecto.Nombre;

                return View(model);
            }
            else
            {
                int estadoPorConfigurar = (int)EstadosProyecto[(int)EstadoProyectoEnum.PorConfigurar].EstadoId;

                var proyectoPorConfigurar = from proyecto in db.Proyectos
                                            join integranteProyecto in db.IntegrantesProyecto
                                            on proyecto.ProyectoId equals integranteProyecto.ProyectoId
                                            where (integranteProyecto.EsEncargado == true
                                            && integranteProyecto.IntegranteId == EncargadoId)
                                            && proyecto.EsEliminado == false
                                            && proyecto.EstadoId == estadoPorConfigurar
                                            select proyecto;

                if (!proyectoPorConfigurar.Any() || proyectoPorConfigurar.Count() > 1)
                {
                    return RedirectToAction("ToConfigure");
                }
                else
                {
                    return RedirectToAction("Configure", new { id = proyectoPorConfigurar.First().ProyectoId });
                }
            }


        }

        [HttpGet]
        public ActionResult ToConfigure()
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
                             && proyecto.JefeProyectoId == JefeProyectoId
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

            resultado = proyectos2.OrderByDescending(x => x.ProyectoId).ToList();
            
            resultado.ForEach(x => x.Estado = this.EstadosProyecto[x.EstadoProyectoId].Descripcion);

            return Json(resultado);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(RegistroProyectoViewModel proyecto)
        {
            if (ModelState.IsValid)
            {
                var oProyecto = new Proyecto();

                oProyecto.ContactoId = proyecto.ContactoId;
                oProyecto.JefeProyectoId = JefeProyectoId;
                oProyecto.Nombre = proyecto.Nombre;
                oProyecto.Mnemonico = proyecto.Mnemonico;
                oProyecto.InicioEstimado = proyecto.InicioEstimado;
                oProyecto.FinEstimado = proyecto.FinEstimado;
                oProyecto.HorasEstimadas = proyecto.HorasEstimadas;
                oProyecto.EstadoId = this.EstadosProyecto[(int)EstadoProyectoEnum.PorConfigurar].EstadoId;
                oProyecto.EsEliminado = false;
                oProyecto.FechaRegistro = DateTime.Now;

                foreach (var integrante in proyecto.Integrantes)
                {
                    var oIntegrante = new IntegranteProyecto();
                    oIntegrante.IntegranteId = integrante.IntegranteId;
                    oIntegrante.EsEncargado = integrante.EsEncargado;
                    oIntegrante.EsEliminado = false;
                    oProyecto.IntegranteProyectoes.Add(oIntegrante);
                }

                db.Proyectos.Add(oProyecto);


                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {

                    }
                }

            }

            return Json(true);
        }

        public ActionResult Edit(int id)
        {
            var proyectoViewModel = new EdicionProyectoViewModel();
            Proyecto proyecto = db.Proyectos.Find(id);
            proyectoViewModel.ProyectoId = proyecto.ProyectoId;
            proyectoViewModel.EsTotalmenteEditable = ((int)EstadoProyectoEnum.PorConfigurar == proyecto.EstadoId);

            return View(proyectoViewModel);
        }

        [HttpPost, ActionName("Edit")]
        public JsonResult EditConfirmed(EdicionProyectoViewModel model)
        {

            Proyecto proyecto = db.Proyectos.Find(model.ProyectoId);

            proyecto.ContactoId = model.ContactoId;
            proyecto.Nombre = model.Nombre;

            if (model.EsTotalmenteEditable)
            {
                proyecto.Mnemonico = model.Mnemonico;
                proyecto.InicioEstimado = model.InicioEstimado;
                proyecto.FinEstimado = model.FinEstimado;
                proyecto.HorasEstimadas = model.HorasEstimadas;
            }

            foreach (var elemento in model.Integrantes)
            {
                if (elemento.EsNuevo)
                {
                    proyecto.IntegranteProyectoes.Add(new IntegranteProyecto() { ProyectoId = proyecto.ProyectoId, IntegranteId = elemento.IntegranteId, EsEliminado = false });
                }
            }

            db.SaveChanges();

            return Json(true);
        }

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
            proyectoViewModel.Integrantes = new List<IntegranteEdicionProyectoViewModel>();

            foreach (var integrante in proyecto.IntegranteProyectoes)
            {
                var integranteViewModel = new IntegranteEdicionProyectoViewModel();
                integranteViewModel.IntegranteId = integrante.IntegranteId;
                integranteViewModel.Nombre = integrante.Usuario.Nombres + " " + integrante.Usuario.Apellidos;
                integranteViewModel.EsEncargado = integrante.EsEncargado;
                integranteViewModel.EsNuevo = false;
                proyectoViewModel.Integrantes.Add(integranteViewModel);
            }

            return Json(proyectoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            proyecto.EsEliminado = true;
            db.SaveChanges();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Configure(ConfigurarProyectoViewModel model)
        {
            var proyecto = db.Proyectos.Find(model.ProyectoId);
            var secuencialUserStory = 1;


            // Agregar Sprint y User Stories
            var sprint = new Sprint();
            sprint.ProyectoId = model.ProyectoId;
            sprint.Objetivo = model.ObjetivoSprint;
            sprint.Nombre = model.NombreSprintInicial;
            sprint.Inicio = model.FechaInicioSprintInicial;
            sprint.EstadoId = this.EstadosSprint[(int)EstadoSprintEnum.Iniciado].EstadoId;

            if (model.SprintUserStories != null)
            {
                foreach (var elementoUserStory in model.SprintUserStories)
                {
                    var userStory = new UserStory();
                    userStory.Codigo = proyecto.Mnemonico + "-" + secuencialUserStory.ToString();
                    userStory.Descripcion = elementoUserStory.DescripcionUserStory;
                    userStory.HorasEstimadas = elementoUserStory.HorasEstimadas;
                    userStory.ProyectoId = proyecto.ProyectoId;
                    userStory.EstadoId = this.EstadosUserStory[(int)EstadoUserStoryEnum.ToDo].EstadoId;

                    if (elementoUserStory.ActividadesUserStory != null)
                    {
                        foreach (var elementoActividad in elementoUserStory.ActividadesUserStory)
                        {
                            var actividad = new Actividad();
                            actividad.Descripcion = elementoActividad.Descripcion;
                            actividad.EstadoId = this.EstadosActividadStory[(int)EstadoActividadEnum.Definido].EstadoId;
                            actividad.TipoActividadId = (int)TipoActividadEnum.Desarrollo;
                            actividad.FechaRegistro = System.DateTime.Now;

                            userStory.Actividads.Add(actividad);
                        }
                    }

                    secuencialUserStory++;
                    sprint.UserStories.Add(userStory);
                }
            }

            if (model.BacklogUserStories != null)
            {
                // Agregar User Stories al Backlog
                foreach (var elementoUserStory in model.BacklogUserStories)
                {
                    var userStory = new UserStory();
                    userStory.Codigo = proyecto.Mnemonico + "-" + secuencialUserStory.ToString();
                    userStory.Descripcion = elementoUserStory.DescripcionUserStory;
                    userStory.HorasEstimadas = elementoUserStory.HorasEstimadas;
                    userStory.ProyectoId = proyecto.ProyectoId;
                    userStory.EstadoId = this.EstadosUserStory[(int)EstadoUserStoryEnum.ToDo].EstadoId;


                    if (elementoUserStory.ActividadesUserStory != null)
                    {
                        foreach (var elementoActividad in elementoUserStory.ActividadesUserStory)
                        {
                            var actividad = new Actividad();
                            actividad.Descripcion = elementoActividad.Descripcion;
                            actividad.EstadoId = this.EstadosActividadStory[(int)EstadoActividadEnum.Definido].EstadoId;
                            actividad.TipoActividadId = (int)TipoActividadEnum.Desarrollo;
                            actividad.FechaRegistro = System.DateTime.Now;

                            userStory.Actividads.Add(actividad);
                        }
                    }

                    secuencialUserStory++;
                    proyecto.UserStories.Add(userStory);
                }
            }


            //Agregamos el Sprint a la lsita de Sprints del Proyecto
            proyecto.Sprints.Add(sprint);
            proyecto.EstadoId = this.EstadosProyecto[(int)EstadoProyectoEnum.EnProgreso].EstadoId;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult ProyectosPorConfigurar()
        {
            int estadoPorConfigurar = (int)EstadosProyecto[(int)EstadoProyectoEnum.PorConfigurar].EstadoId;

            var resultado = new List<ListaProyectoPorConfigurar>();

            var proyectosPorConfigurar = from proyecto in db.Proyectos
                                         join integranteProyecto in db.IntegrantesProyecto
                                         on proyecto.ProyectoId equals integranteProyecto.ProyectoId
                                         join contacto in db.Contactos
                                         on proyecto.ContactoId equals contacto.ContactoId
                                         join empresa in db.Empresas
                                         on contacto.EmpresaId equals empresa.EmpresaId
                                         where (integranteProyecto.EsEncargado == true
                                         && integranteProyecto.IntegranteId == EncargadoId)
                                         && proyecto.EsEliminado == false
                                         && proyecto.EstadoId == estadoPorConfigurar
                                         select new
                                         {
                                             ProyectoId = proyecto.ProyectoId,
                                             Nombre = proyecto.Nombre,
                                             Cliente = empresa.Nombre,
                                             FechaRegistro = proyecto.FechaRegistro
                                         };

            foreach (var elemento in proyectosPorConfigurar)
            {
                resultado.Add(new ListaProyectoPorConfigurar() { ProyectoId = elemento.ProyectoId, Nombre = elemento.Nombre, Cliente = elemento.Cliente, FechaRegistro = elemento.FechaRegistro.ToString("dd/MM/yyyy") });
            }

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult ProyectosEnProgreso()
        {
            int estadoEnProgreso = (int)EstadosProyecto[(int)EstadoProyectoEnum.Concluido].EstadoId;

            var resultado = new List<ListaProyectoEnProgreso>();

            var proyectosEnProgreso = from proyecto in db.Proyectos
                                         join integranteProyecto in db.IntegrantesProyecto
                                         on proyecto.ProyectoId equals integranteProyecto.ProyectoId
                                         join contacto in db.Contactos
                                         on proyecto.ContactoId equals contacto.ContactoId
                                         join empresa in db.Empresas
                                         on contacto.EmpresaId equals empresa.EmpresaId
                                         where (integranteProyecto.EsEncargado == true
                                         && integranteProyecto.IntegranteId == EncargadoId)
                                         && proyecto.EsEliminado == false
                                         && proyecto.EstadoId == estadoEnProgreso
                                         select new
                                         {
                                             ProyectoId = proyecto.ProyectoId,
                                             Nombre = proyecto.Nombre,
                                             Cliente = empresa.Nombre,
                                             FechaRegistro = proyecto.FechaRegistro
                                         };

            foreach (var elemento in proyectosEnProgreso)
            {
                resultado.Add(new ListaProyectoEnProgreso() { ProyectoId = elemento.ProyectoId, Nombre = elemento.Nombre, Cliente = elemento.Cliente, FechaRegistro = elemento.FechaRegistro.ToString("dd/MM/yyyy") });
            }

            return Json(resultado);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}