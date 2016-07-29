using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentStartViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var form = request.Form;

            string id = form.Get("Tournament.Id");
            string courseId = form.Get("Tournament.Course.Id");
            string clubId = form.Get("Tournament.Club.Id");
            

            var tournament = new Models.Tournament()
            {
                Club = new Club()
                {
                    Id = clubId
                },
                Course = new Course()
                {
                    Id = courseId,
                    ClubId = clubId
                },
                Id = id,
                Participants = new TournamentParticipantCollection()
            };

            return new TournamentEditViewModel
            {
                Tournament = tournament
            };
        }
    }
}