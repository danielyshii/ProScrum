using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RedCell.ProScrum.WebUI.Controllers;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.TransferObjects;
using WebMatrix.WebData;

namespace RedCell.ProScrum.WebUI.Services
{
    public class ActividadService
    {
        public ProScrumContext db { get; set; }
        
        public ActividadService(ProScrumContext dbContext)
        {
            db = dbContext;
        }

        public UserStoryChangeStatus VerifyUserStoryChangeStatus(int usid)
        {
            var userStoryChangeStatus = new UserStoryChangeStatus();

            int totalTerminadas = 0;
            int totalActividades = 0;

            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var userStoryByIdQuery = (from userStory in db.UserStories
                                      where userStory.UserStoryId == usid
                                      select userStory).FirstOrDefault();

            totalTerminadas = (from actividad in db.Actividades
                               where actividad.UserStoryId == userStoryByIdQuery.UserStoryId
                               && actividad.EstadoId == estadoTerminadoActividad
                               && actividad.EsEliminado == false
                               select actividad).Count();

            totalActividades = (from actividad in db.Actividades
                                where actividad.UserStoryId == userStoryByIdQuery.UserStoryId
                                && actividad.EsEliminado == false
                                select actividad).Count();

            if (totalTerminadas == totalActividades && totalTerminadas != 0 && totalActividades != 0)
            {
                userStoryChangeStatus.estadoAnteriorUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.estadoNuevoUserStory = userStoryByIdQuery.EstadoId + 1;
                userStoryChangeStatus.requiereCambio = true;                
            }
            else {
                userStoryChangeStatus.estadoAnteriorUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.estadoNuevoUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.requiereCambio = false;
            }

            userStoryChangeStatus.UserStoryId = userStoryByIdQuery.UserStoryId;
            userStoryChangeStatus.totalTerminadas = totalTerminadas;
            userStoryChangeStatus.totalActividades = totalActividades;

            return userStoryChangeStatus;
            
        }

        public UserStoryChangeStatus SaveUserStoryStatus(int usid, int currentUserId) {

            int estadoInProcessUserStory = (int)EstadoUserStoryEnum.InProcess;            

            var userStoryChangeStatus = VerifyUserStoryChangeStatus(usid);

            if (userStoryChangeStatus.requiereCambio)
            {
                var element = (from userStory in db.UserStories
                               where userStory.UserStoryId == usid
                               select userStory).FirstOrDefault();

                

                element.EstadoId = userStoryChangeStatus.estadoNuevoUserStory;                

                if (userStoryChangeStatus.estadoAnteriorUserStory == estadoInProcessUserStory)
                {
                    var actividad = new Actividad();
                    actividad.Descripcion = "Pruebas de aceptación";
                    actividad.EsEliminado = false;
                    actividad.EstadoId = (int)EstadoActividadEnum.Definido;
                    actividad.TipoActividadId = (int)TipoActividadEnum.Calidad;
                    actividad.UserStoryId = usid;
                    actividad.FechaRegistro = System.DateTime.Now;
                    actividad.UsuarioId = currentUserId;

                    db.Actividades.Add(actividad);

                    userStoryChangeStatus.totalActividades += 1;
                }

                db.SaveChanges();

                var sprintChangeStatus = VerifySprintChangeStatus((int)element.SprintId);

                if (sprintChangeStatus.requiereCambio)
                {
                    var sprintPorTerminar = (from sprint in db.Sprints
                                             where sprint.SprintId == usid
                                             select sprint).FirstOrDefault();

                    sprintPorTerminar.EstadoId = sprintChangeStatus.estadoNuevoSprint;

                    db.SaveChanges();
                }

            }

            return userStoryChangeStatus;
        }

        public SprintChangeStatus VerifySprintChangeStatus(int sid)
        {
            var sprintChangeStatus = new SprintChangeStatus();

            int totalTerminados = 0;
            int totalUserStories = 0;

            int estadoDoneUserStory = (int)EstadoUserStoryEnum.Done;

            var sprintByIdQuery = (from sprint in db.Sprints
                                   where sprint.SprintId == sid
                                   select sprint).FirstOrDefault();

            totalTerminados = (from userStory in db.UserStories
                               where userStory.SprintId == sid
                               && userStory.EstadoId == estadoDoneUserStory
                               && userStory.EsEliminado == false
                               select userStory).Count();

            totalUserStories = (from userStory in db.UserStories
                                where userStory.SprintId == sid
                                && userStory.EsEliminado == false
                                select userStory).Count();

            if (totalTerminados == totalUserStories && totalTerminados != 0 && totalUserStories != 0)
            {
                sprintChangeStatus.estadoAnteriorSprint = sprintByIdQuery.EstadoId;
                sprintChangeStatus.estadoNuevoSprint = sprintByIdQuery.EstadoId + 1;
                sprintChangeStatus.requiereCambio = true;                
            }
            else
            {
                sprintChangeStatus.estadoAnteriorSprint = sprintByIdQuery.EstadoId;
                sprintChangeStatus.estadoNuevoSprint = sprintByIdQuery.EstadoId;
                sprintChangeStatus.requiereCambio = false;
            }

            sprintChangeStatus.SprintId = sprintByIdQuery.SprintId;
            sprintChangeStatus.totalTerminados = totalTerminados;
            sprintChangeStatus.totalUserStories = totalUserStories;

            return sprintChangeStatus;

        }


    }
}