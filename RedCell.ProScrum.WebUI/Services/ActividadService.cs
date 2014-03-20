using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RedCell.ProScrum.WebUI.Controllers;
using RedCell.ProScrum.WebUI.Models;
using RedCell.ProScrum.WebUI.TransferObjects;

namespace RedCell.ProScrum.WebUI.Services
{
    public class ActividadService
    {
        public ProScrumContext db { get; set; }
        
        public ActividadService(ProScrumContext dbContext)
        {
            db = dbContext;
        }

        public UserStoryChangeStatus VerifyUserStoryChangeStatus(int aid)
        {
            var userStoryChangeStatus = new UserStoryChangeStatus();

            int totalTerminadas = 0;
            int totalActividades = 0;

            int estadoTerminadoActividad = (int)EstadoActividadEnum.Terminado;

            var actividadByIdQuery = (from actividad in db.Actividades
                                 where actividad.ActividadId == aid
                                 select actividad).FirstOrDefault();

            var userStoryByIdQuery = (from userStory in db.UserStories
                                      where userStory.UserStoryId == actividadByIdQuery.UserStoryId
                                      select userStory).FirstOrDefault();

            totalTerminadas = (from actividad in db.Actividades
                               where actividad.UserStoryId == actividadByIdQuery.UserStoryId
                               && actividad.EstadoId == estadoTerminadoActividad
                               select actividad).Count();

            totalActividades = (from actividad in db.Actividades
                                              where actividad.UserStoryId == actividadByIdQuery.UserStoryId
                                               select actividad).Count();

            if (totalTerminadas == totalActividades && totalTerminadas != 0 && totalActividades != 0)
            {
                userStoryChangeStatus.estadoAnteriorUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.estadoNuevoUserStory = userStoryByIdQuery.EstadoId + 1;
                userStoryChangeStatus.requiereCambio = true;
                userStoryChangeStatus.UserStoryId = userStoryByIdQuery.UserStoryId;                
            }
            else {
                userStoryChangeStatus.estadoAnteriorUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.estadoNuevoUserStory = userStoryByIdQuery.EstadoId;
                userStoryChangeStatus.requiereCambio = false;
                userStoryChangeStatus.UserStoryId = userStoryByIdQuery.UserStoryId;
            }

            return userStoryChangeStatus;
            
        }

        public UserStoryChangeStatus VerifyUserStoryToComplete() {

            return null;
        }


    }
}