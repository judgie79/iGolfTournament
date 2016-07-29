using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
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
            DateTime date = DateTime.Parse(form.Get("Tournament.Date.Tournament.Date"));

            var tournament = new Tournament()
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
                },
                Id = id,
                Participants = new TournamentParticipantCollection()
            };

            //int participantCounter = 0; //"Course.TeeBoxes[0].Holes[0].HoleId"

            //while (true)
            //{
            //    if (!string.IsNullOrEmpty(form["Participants[" + participantCounter + "].Id"]))
            //    {
            //        string Id = form["Participants[" + participantCounter + "].Name"];
            //        DateTime TeaTime = DateTime.Parse(form["Participants[" + participantCounter + "].TeaTime"].ToString());
            //        string TeebBoxId = form["Participants[" + participantCounter + "].TeebBoxId"];
            //        string PlayerId = form["Participants[" + participantCounter + "].PlayerId"];

            //        var participant = new TournamentParticipant()
            //        {
            //            Id = Id,
            //            TeaTime = TeaTime,
            //            TeebBoxId = TeebBoxId,
            //            Player = new Player()
            //            {
            //                Id = PlayerId
            //            }
            //        };

            //        tournament.Participants.Add(participant);
            //        participantCounter++;
            //    }
            //    else
            //    {
            //        participantCounter = 0;
            //        break;
            //    }


            //}

            return new TournamentEditViewModel
            {
                Tournament = tournament
            };
        }
    }
}