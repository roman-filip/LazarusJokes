using System.Collections.Generic;
using System.Threading.Tasks;
using RFI.LazarusJokes.Web.Models;

namespace RFI.LazarusJokes.Web.Connectors
{
    public interface ILazarusJokesServicesConnector
    {
        Task<List<Joke>> LoadJokesAsync();
    }
}
