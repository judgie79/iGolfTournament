using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    [JsonArray]
    public class TournamentParticipantCollection : List<TournamentParticipant>
    {
        public TournamentParticipantCollection()
            : base()
        {

        }

        public TournamentParticipantCollection(IEnumerable<TournamentParticipant> participants)
            : base(participants)
        {

        }
    }
}