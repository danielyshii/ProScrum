using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class IntegranteProyecto
    {
        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public bool Eliminado { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
