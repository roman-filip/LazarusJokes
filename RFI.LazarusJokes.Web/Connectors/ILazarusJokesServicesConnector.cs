using System.Collections.Generic;
using System.Threading.Tasks;
using RFI.LazarusJokes.Web.Models;

namespace RFI.LazarusJokes.Web.Connectors
{
    interface ILazarusJokesServicesConnector
    {
        Task<List<Joke>> LoadJokesAsync();
    }
}
