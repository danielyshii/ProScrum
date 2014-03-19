using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.Models
{

    public class Inconformidad
    {
        public int InconformidadId { get; set; }
        public int UserStoryId { get; set; }
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UriAdjunto { get; set; }
        public bool EsEliminado { get; set; }

        public UserStory UserStory { get; set; }
        public Usuario Usuario { get; set; }
    }
}