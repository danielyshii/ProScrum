using System;
using System.Collections.Generic;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            this.Contactoes = new List<Contacto>();
        }

        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public bool EsEliminado { get; set; }
        public virtual ICollection<Contacto> Contactoes { get; set; }
    }
}
