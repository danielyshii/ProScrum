using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class IntegranteProyecto
    {
        public int ProyectoId { get; set; }
        public int IntegranteId { get; set; }
        public bool EsEncargado { get; set; }
        public bool EsEliminado { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
