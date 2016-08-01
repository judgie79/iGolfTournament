using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentCreateViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            string courseId = request.Form.Get("CourseId");
            string clubId = request.Form.Get("ClubId");

            string title = request.Form.Get("Tournament.Title");
            var type = (TournamentType)Convert.ToInt16(request.Form.Get("TournamentType"));
            var scoreType = (ScoreType)Convert.ToInt16(request.Form.Get("StrokeType"));

            string dateKey = request.Form.Get("Tournament.Date") ?? request.Form.Get("Tournament.Date.Tournament.Date");
            DateTime date = DateTime.Parse(dateKey);

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
            t.Club = new Club()
            {
                Id = clubId
            };
            t.Course = new Course()
            {
                Id = courseId,
                ClubId = clubId
            };

            return new TournamentCreateViewModel<Models.Tournament>
            {
                ClubId = clubId,
                CourseId = courseId,
                TournamentType = type,
                Tournament = t,
                ScoreType = scoreType
            };
        }
    }
}