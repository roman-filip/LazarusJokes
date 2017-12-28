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

        // POST: LazarusJokes/api/jokes
        [HttpPost]
        public HttpResponseMessage AddJoke([FromBody]JokeSimple joke)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var newJoke = Joke.FromJokeSimple(joke);
            var jokes = LoadJokes();
            newJoke.Id = jokes.Any() ? jokes.Max(j => j.Id) + 1 : 1;
            jokes.Add(newJoke);

            SaveJokes(jokes);

            return Request.CreateResponse(HttpStatusCode.OK);  // TODO - the Post method should return newly created object
        }

        // PUT: LazarusJokes/api/jokes/1
        [HttpPut]
        [Route("LazarusJokes/api/jokes/{jokeId:long}")]
        public HttpResponseMessage VoteForJoke([FromUri]long jokeId, [FromBody]UserVote userVote)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var jokes = LoadJokes();

            var joke = jokes.Single(j => j.Id == jokeId);
            var givenUserVote = joke.UserVotes.SingleOrDefault(vote => vote.UserName == userVote.UserName);
            if (givenUserVote == null)
            {
                givenUserVote = new UserVote { UserName = userVote.UserName };
                joke.UserVotes.Add(givenUserVote);
            }
            givenUserVote.Vote = userVote.Vote;

            SaveJokes(jokes);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET: LazarusJokes/api/jokes/closevoting?closingdate=2015-12-24
        [HttpGet]
        public void CloseVoting(DateTime closingDate)
        {
            var jokes = LoadJokes();
            jokes.Where(joke => joke.Date <= closingDate).ToList().ForEach(joke => joke.VotingClosed = true);
            SaveJokes(jokes);
        }




        // TODO - extract following methods to Repository
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
