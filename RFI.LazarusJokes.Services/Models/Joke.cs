using System.Collections.Generic;

namespace RFI.LazarusJokes.Services.Models
{
    public class Joke : SimpleJoke
    {
        public long Id { get; set; }

        public List<UserVote> UserVotes { get; set; }

        public bool? VotingClosed { get; set; }

        public static Joke FromSimpleJoke(SimpleJoke simpleJoke)
        {
            return new Joke
            {
                Text = simpleJoke.Text,
                Date = simpleJoke.Date,
                Author = simpleJoke.Author
            };
        }
    }
}