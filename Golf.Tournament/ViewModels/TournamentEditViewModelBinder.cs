using Golf.Tournament.Models;
using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentEditViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var form = request.Form;

            string id = form.Get("Tournament.Id");
            string courseId = form.Get("Tournament.Course.Id");
            string clubId = form.Get("Tournament.Club.Id");

            string title = form.Get("Tournament.Title");

            TournamentType type = (TournamentType)Enum.Parse(typeof(TournamentType), request.Form.Get("Tournament.TournamentType"));
            ScoreType scoreType = (ScoreType)Enum.Parse(typeof(ScoreType), request.Form.Get("Tournament.ScoreType"));

            DateTime date = DateTime.Parse(form.Get("Tournament.Date.Tournament.Date"));

            Models.Tournament t;

            switch (type)
            {
                case TournamentType.Single:
                    t = new Models.Tournament();

                    break;
                case TournamentType.Team:
                    t = new Models.TeamTournament();
                    break;
                default:
                    t = new Models.Tournament();
                    break;
            }

            t.ScoreType = scoreType;
            t.Title = title;
            t.Date = date.ToUniversalTime();
            t.Id = id;
            t.Club = new Club()
            {
                Id = clubId
            };
            t.Course = new Course()
            {
                Id = courseId,
                ClubId = clubId
            };

            return new TournamentEditViewModel<Models.Tournament>
            {
                Tournament = t
            };
        }
    }
}