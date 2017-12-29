using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;
using RFI.LazarusJokes.Web.Connectors;
using RFI.LazarusJokes.Web.Models;

namespace RFI.LazarusJokes.Web.Controllers
{
    public class JokesController : Controller
    {
        private readonly ILazarusJokesServicesConnector _connector;

        public JokesController() : this(new LazarusJokesServicesConnector())  // TODO - use DI instead of this workaround
        {

        }

        public JokesController(ILazarusJokesServicesConnector connector)
        {
            _connector = connector;
        }

        public async Task<ActionResult> Jokes(JokesViewModel model)
        {
            ViewBag.Message = "All jokes";

            model.NewJoke = new JokeSimple
            {
                Author = User.Identity.Name,
                Date = DateTime.Now.Date
            };
            model.Jokes = await LoadJokesAsync();

            return View(model);
        }

        // POST: Jokes/AddJoke
        [HttpPost]
        public async Task<ActionResult> AddJoke(JokesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _connector.AddJokeAsync(model.NewJoke);
            }

            return RedirectToAction("Jokes");
        }

        public async Task<ActionResult> VoteForJoke(long jokeId, int userVote)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.Name;
                await _connector.VoteForJokeAcync(jokeId, new UserVote { UserName = user, Vote = userVote });
            }

            return RedirectToAction("Jokes");
        }

        private void SaveJokes(IEnumerable<Joke> jokes)
        {
            var serializer = new XmlSerializer(jokes.GetType());
            using (TextWriter streamWriter = new StreamWriter(GetFilePath()))
            {
                serializer.Serialize(streamWriter, jokes);
            }
        }

        private string GetFilePath()
        {
            return Server.MapPath("~/App_Data/jokes.xml");
        }

        private async Task<List<Joke>> LoadJokesAsync()
        {
            var user = User.Identity.Name;
            var jokes = await _connector.LoadJokesAsync().ConfigureAwait(false);
            jokes.ForEach(joke => joke.VotesOfCurrentUser = joke.UserVotes.Where(vote => vote.UserName == user).ToList());

            return jokes;
        }
    }
}
