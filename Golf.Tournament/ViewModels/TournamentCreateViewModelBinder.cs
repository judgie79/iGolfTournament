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
            var type = (TournamentType)Enum.Parse(typeof(TournamentType), request.Form.Get("TournamentType"));

            string dateKey = request.Form.Get("Tournament.Date") ?? request.Form.Get("Tournament.Date.Tournament.Date");
            DateTime date = DateTime.Parse(dateKey);

            Models.Tournament t;

            switch (type)
            {
                case TournamentType.Stableford_Single:
                    t = new Models.Tournament();
                    break;
                case TournamentType.Stableford_Team:
                    t = new Models.TeamTournament();
                    break;
                default:
                    break;
            }

            return new TournamentCreateViewModel<Models.Tournament>
            {
                ClubId = clubId,
                CourseId = courseId,
                TournamenType = type,
                Tournament = new Models.Tournament()
                {
                    Title = title,
                    Date = date.ToUniversalTime(),
                    Club = new Club()
                    {
                        Id = clubId
                    },
                    Course = new Course()
                    {
                        Id = courseId,
                        ClubId = clubId
                    }
                }
            };
        }
    }
}