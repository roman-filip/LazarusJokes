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

            model.NewJoke = new Joke
            {
                Author = User.Identity.Name,
                Date = DateTime.Now.Date
            };
            model.Jokes = await _connector.LoadJokesAsync();

            return View(model);
        }

        // POST: Jokes/AddJoke
        [HttpPost]
        public async Task<ActionResult> AddJoke(JokesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _connector.AddJokeAsync(model.NewJoke);



                // TODO - remove following lines
                var jokes = _connector.LoadJokesAsync().Result;
                model.NewJoke.Id = jokes.Any() ? jokes.Max(joke => joke.Id) + 1 : 1;
                jokes.Add(model.NewJoke);
                SaveJokes(jokes);
            }

            return RedirectToAction("Jokes");
        }

        public ActionResult VoteForJoke(long jokeId, int userVote)
        {
            var user = User.Identity.Name;

            var jokes = _connector.LoadJokesAsync().Result;

            var actualJoke = jokes.Single(joke => joke.Id == jokeId);
            var givenUserVote = actualJoke.UserVotes.SingleOrDefault(vote => vote.UserName == user);
            if (givenUserVote == null)
            {
                givenUserVote = new UserVote { UserName = user };
                actualJoke.UserVotes.Add(givenUserVote);
            }
            givenUserVote.Vote = userVote;

            SaveJokes(jokes);

            //xxxx

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
    }
}
