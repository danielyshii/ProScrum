using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            this.Actividads = new List<Actividad>();
            this.Competencias = new List<Competencia>();
            this.IntegranteProyectoes = new List<IntegranteProyecto>();
            this.Proyectoes = new List<Proyecto>();
            this.UserStories = new List<UserStory>();
        }

        public int UsuarioId { get; set; }
        public string Codigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public bool EsEliminado { get; set; }
        public virtual ICollection<Actividad> Actividads { get; set; }
        public virtual ICollection<Competencia> Competencias { get; set; }
        public virtual ICollection<IntegranteProyecto> IntegranteProyectoes { get; set; }
        public virtual ICollection<Proyecto> Proyectoes { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}
