using System.Collections.Generic;

namespace RFI.LazarusJokes.Services.Models
{
    public class Joke : JokeSimple
    {
        public long Id { get; set; }

        public List<UserVote> UserVotes { get; set; }

        public bool? VotingClosed { get; set; }

        public static Joke FromJokeSimple(JokeSimple jokeSimple)
        {
            return new Joke
            {
                Text = jokeSimple.Text,
                Date = jokeSimple.Date,
                Author = jokeSimple.Author
            };
        }
    }
}