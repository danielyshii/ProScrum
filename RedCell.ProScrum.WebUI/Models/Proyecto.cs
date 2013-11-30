using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            this.IntegranteProyectoes = new List<IntegranteProyecto>();
            this.Sprints = new List<Sprint>();
            this.UserStories = new List<UserStory>();
        }

        public int ProyectoId { get; set; }
        public int JefeProyectoId { get; set; }
        public int ContactoId { get; set; }
        public string Nombre { get; set; }
        public string Mnemonico { get; set; }
        public System.DateTime InicioEstimado { get; set; }
        public System.DateTime FinEstimado { get; set; }
        public int HorasEstimadas { get; set; }
        public int EstadoId { get; set; }
        public bool EsEliminado { get; set; }
        public virtual Contacto Contacto { get; set; }
        public virtual ICollection<IntegranteProyecto> IntegranteProyectoes { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Sprint> Sprints { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}