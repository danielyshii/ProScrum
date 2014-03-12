﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.ViewModels.TaskBoard
{
    public class UserStoryDetailViewModel
    {
        public int UserStoryId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Color { get; set; }
        public int? UsuarioId { get; set; }
        public string Usuario { get; set; }
        public List<ActividadesViewModel> ListaActividades { get; set; }
    }

    public class ActividadesViewModel
    {
        public int? ActividadId { get; set; }
        public string Descripcion { get; set; }
        public bool Terminado { get; set; }
    }
}