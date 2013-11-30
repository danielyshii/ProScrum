using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class TipoActividad
    {
        public TipoActividad()
        {
            this.Actividads = new List<Actividad>();
            this.Competencias = new List<Competencia>();
        }

        public int TipoActividadId { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Actividad> Actividads { get; set; }
        public virtual ICollection<Competencia> Competencias { get; set; }
    }
}
