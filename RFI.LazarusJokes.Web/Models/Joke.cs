using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace RFI.LazarusJokes.Web.Models
{
    public class Joke : JokeSimple
    {
        public long Id { get; set; }

        public List<UserVote> UserVotes { get; set; }

        [Display(Name = "Celkový počet bodů")]
        [JsonIgnore]
        public int? TotalVote
        {
            get
            {
                return UserVotes.Sum(vote => vote.Vote);
            }
        }

        public List<UserVote> VotesOfCurrentUser { get; set; }  // TODO - why is it list???

        [JsonIgnore]   // TODO - remove this, JokeSimple should be used instead of Joke
        public int? UserVote
        {
            get
            {
                return VotesOfCurrentUser.Any() ? VotesOfCurrentUser[0].Vote : default(int?);
            }
        }

        public bool? VotingClosed { get; set; }
    }
}