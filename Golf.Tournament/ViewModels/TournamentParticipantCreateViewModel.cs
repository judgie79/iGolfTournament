using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class TournamentParticipantCreateViewModel
    {
        public string TournamentId { get; set; }

        [DisplayName("Player")]
        public string PlayerId { get; set; }

        public PlayerCollection Players { get; set; }

        [DisplayName("Teebox")]
        public string TeeboxId { get; set; }

        public TeeboxCollection Teeboxes { get; set; }
    }

    public class TournamentParticipantEditViewModel
    {
        public TournamentParticipant Participant { get; set; }

        public Models.Tournament Tournament { get; set; }

        public TeeboxCollection Teeboxes { get; set; }

        public Course Course { get; internal set; }
    }

    public class TournamentParticipantEditViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var form = request.Form;

            string id = form.Get("Participant.Id");
            string playerId = form.Get("Participant.Player.Id");

            string teeboxId = form.Get("Participant.TeebBoxId");
            DateTime TeaTime = DateTime.Parse(form.Get("Participant.TeaTime"));

            var participant = new TournamentParticipant()
            {
                Id = id,
                TeaTime = TeaTime,
                TeebBoxId = teeboxId,
                Player = new Player()
                {
                    Id = playerId,
                }
            };

            return new TournamentParticipantEditViewModel
            {
                Participant = participant
            };
        }
    }
}