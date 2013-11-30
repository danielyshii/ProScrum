using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Sprint
    {
        public Sprint()
        {
            this.UserStories = new List<UserStory>();
        }

        public int SprintId { get; set; }
        public int ProyectoId { get; set; }
        public string Objetivo { get; set; }
        public string Nombre { get; set; }
        public System.DateTime Inicio { get; set; }
        public System.DateTime Fin { get; set; }
        public int EstadoId { get; set; }
        public bool EsEliminado { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}
