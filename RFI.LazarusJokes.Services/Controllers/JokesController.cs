using RFI.LazarusJokes.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Serialization;

namespace RFI.LazarusJokes.Services.Controllers
{
    //[Authorize]
    public class JokesController : ApiController
    {
        // GET: LazarusJokes/api/jokes
        public IEnumerable<Joke> Get()
        {
            var jokes = LoadJokes();
            return jokes;
        }

        // GET: LazarusJokes/api/jokes/get/1
        public Joke Get(int id)
        {
            var jokes = LoadJokes();
            var joke = jokes.SingleOrDefault(j => j.Id == id);
            return joke;
        }

        // POST: api/Jokes
        [HttpPost]
        public void AddJoke([FromBody]Joke joke)
        {
            var jokes = LoadJokes();

            joke.Id = jokes.Any() ? jokes.Max(j => j.Id) + 1 : 1;
            jokes.Add(joke);

            SaveJokes(jokes);
        }

        // GET: LazarusJokes/api/jokes/closevoting?closingdate=2015-12-24
        [HttpGet]
        public void CloseVoting(DateTime closingDate)
        {
            var jokes = LoadJokes();
            jokes.Where(joke => joke.Date <= closingDate).ToList().ForEach(joke => joke.VotingClosed = true);
            SaveJokes(jokes);
        }







        // PUT: api/Jokes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Jokes/5
        public void Delete(int id)
        {
        }




        private List<Joke> LoadJokes()
        {
            if (!File.Exists(GetFilePath()))
            {
                return new List<Joke>();
            }

            var serializer = new XmlSerializer(typeof(List<Joke>));
            using (Stream reader = new FileStream(GetFilePath(), FileMode.Open))
            {
                var jokes = (List<Joke>)serializer.Deserialize(reader);

                var user = User.Identity.Name;
                jokes.ForEach(joke => joke.VotesOfCurrentUser = joke.UserVotes.Where(vote => vote.UserName == user).ToList());
                return jokes.OrderByDescending(joke => joke.Date).ToList();
            }
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
            return System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/jokes.xml");
        }
    }
}
