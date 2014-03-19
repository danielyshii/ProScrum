using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class UserStory
    {
        public UserStory()
        {
            this.Actividads = new List<Actividad>();
            this.Bloqueos = new List<Bloqueo>();
        }

        public int UserStoryId { get; set; }
        public int ProyectoId { get; set; }
        public int? SprintId { get; set; }
        public int? ResponsableId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int HorasEstimadas { get; set; }
        public int? Prioridad { get; set; }
        public int? Color { get; set; }
        public int EstadoId { get; set; }
        public bool EsEliminado { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Actividad> Actividads { get; set; }
        public virtual ICollection<Bloqueo> Bloqueos { get; set; }
        public virtual ICollection<Inconformidad> Inconformidades { get; set; }
    }
}
