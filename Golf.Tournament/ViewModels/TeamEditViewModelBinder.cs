using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TeamEditViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var form = request.Form;

            string id = form.Get("Team.Id");
            string name = form.Get("Team.Name");

            string teeboxId = form.Get("Team.TeeboxId");
            DateTime Teetime = DateTime.Parse(form.Get("Team.Teetime.Team.Teetime"));

            var team = new Team()
            {
                Id = id,
                Name = name,
                Teetime = Teetime,
                TeeboxId = teeboxId
            };

            return new TeamEditViewModel
            {
                Team = team
            };
        }
    }
}