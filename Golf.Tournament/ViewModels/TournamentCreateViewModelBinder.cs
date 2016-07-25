using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
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
            DateTime date = DateTime.Parse(request.Form.Get("Tournament.Date"));

            return new TournamentCreateViewModel
            {
                ClubId = clubId,
                CourseId = courseId,
                Tournament = new Tournament()
                {
                    Title = title,
                    Date = date,
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