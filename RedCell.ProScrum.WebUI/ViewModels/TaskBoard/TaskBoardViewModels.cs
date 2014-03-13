using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.ViewModels.TaskBoard
{

    public class BoardViewModel
    {
        public int ProyectoId { get; set; }
    }


    public class BoardColumnViewModel
    { 
        public int BoardColumnId { get; set; }
        public string Descripcion { get; set; }
    }

    public class UserStoryCompactViewModel
    {
        public int UserStoryId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool EstaBloqueada { get; set; }
        public int? NumeroActividadTotal { get; set; }
        public int? NumeroActividadTerminada { get; set; }
        public int EstadoUserStoryId { get; set; }
        public int? Color { get; set; }
    }

    public class UserStoryDetailViewModel
    {
        public int UserStoryId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int? Color { get; set; }
        public int? UsuarioId { get; set; }
        public string Usuario { get; set; }
        public List<ActividadesViewModel> ListaActividades { get; set; }
    }

    public class ActividadesViewModel
    {
        public int? ActividadId { get; set; }
        public string Descripcion { get; set; }
        public bool Terminado { get; set; }
    }
}