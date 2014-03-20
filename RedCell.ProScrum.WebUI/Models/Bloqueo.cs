using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.Models
{
    public class Bloqueo
    {
        public int BloqueoId { get; set; }
        public int TipoBloqueoId { get; set; }
        public int UserStoryId { get; set; }
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; }
        public DateTime InicioBloqueo { get; set; }
        public DateTime? FinBloqueo { get; set; }
        public bool EsEliminado { get; set; }

        public virtual TipoBloqueo TipoBloqueo { get; set; }
        public virtual UserStory UserStory { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}