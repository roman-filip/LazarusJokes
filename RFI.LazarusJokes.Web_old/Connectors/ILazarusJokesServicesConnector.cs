using System.Collections.Generic;
using System.Threading.Tasks;
using RFI.LazarusJokes.Web.Models;

namespace RFI.LazarusJokes.Web.Connectors
{
    // TODO - rename interface and class
    public interface ILazarusJokesServicesConnector
    {
        Task<List<Joke>> LoadJokesAsync();

        Task<Joke> AddJokeAsync(JokeSimple joke);

        Task VoteForJokeAcync(long jokeId, UserVote userVote);
    }
}
