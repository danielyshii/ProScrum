using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Contacto
    {
        public Contacto()
        {
            this.Proyectoes = new List<Proyecto>();
        }

        public int ContactoId { get; set; }
        public int EmpresaId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroTelefonico { get; set; }
        public bool EsEliminado { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<Proyecto> Proyectoes { get; set; }
    }
}
