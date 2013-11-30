using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class UserStory
    {
        public UserStory()
        {
            this.Actividads = new List<Actividad>();
        }

        public int UserStoryId { get; set; }
        public int ProyectoId { get; set; }
        public Nullable<int> SprintId { get; set; }
        public Nullable<int> ResponsableId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int HorasEstimadas { get; set; }
        public Nullable<int> Prioridad { get; set; }
        public int EstadoId { get; set; }
        public bool EsEliminado { get; set; }
        public virtual ICollection<Actividad> Actividads { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
