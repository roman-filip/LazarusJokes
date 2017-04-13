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

        public JokesController()
        {
            _connector = new LazarusJokesServicesConnector();
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
        public ActionResult AddJoke(JokesViewModel model)
        {
            if (ModelState.IsValid)
            {
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
