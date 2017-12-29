using System.Collections.Generic;

namespace RFI.LazarusJokes.Web.Models
{
    public class JokesViewModel
    {
        public JokeSimple NewJoke { get; set; }

        public IEnumerable<Joke> Jokes { get; set; }
    }
}