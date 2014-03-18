using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Competencia
    {
        public int UsuarioId { get; set; }
        public int TipoActividadId { get; set; }
        public bool EsEliminado { get; set; }

        public virtual TipoActividad TipoActividad { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
