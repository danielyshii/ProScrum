using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.TransferObjects
{
    public class SprintChangeStatus
    {
        public SprintChangeStatus()
        {

        }

        public int estadoAnteriorSprint { get; set; }
        public int estadoNuevoSprint { get; set; }
        public bool requiereCambio { get; set; }
        public int SprintId { get; set; }
        public int totalTerminados { get; set; }
        public int totalUserStories { get; set; }
    }
}