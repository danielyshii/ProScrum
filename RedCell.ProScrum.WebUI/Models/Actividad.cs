using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Actividad
    {
        public int ActividadId { get; set; }
        public int UserStoryId { get; set; }
        public int? UsuarioId { get; set; }
        public int TipoActividadId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int EstadoId { get; set; }
        public bool EsEliminado { get; set; }

        public virtual TipoActividad TipoActividad { get; set; }
        public virtual UserStory UserStory { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
