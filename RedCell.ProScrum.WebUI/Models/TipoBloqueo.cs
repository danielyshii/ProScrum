using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.Models
{
    public class TipoBloqueo
    {
        public TipoBloqueo()
        {
            this.Bloqueos = new List<Bloqueo>();
        }

        public int TipoBloqueoId { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Bloqueo> Bloqueos { get; set; }
    }
}