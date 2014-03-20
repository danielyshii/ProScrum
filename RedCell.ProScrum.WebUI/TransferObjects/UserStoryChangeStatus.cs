using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedCell.ProScrum.WebUI.TransferObjects
{
    public class UserStoryChangeStatus
    {
        public UserStoryChangeStatus() {

        }

        public int estadoAnteriorUserStory { get; set; }
        public int estadoNuevoUserStory { get; set; }
        public bool requiereCambio { get; set; }
        public int UserStoryId { get; set; }

    }
}