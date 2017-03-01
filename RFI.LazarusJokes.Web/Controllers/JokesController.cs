using RFI.LazarusJokes.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace RFI.LazarusJokes.Web.Controllers
{
    public class JokesController : Controller
    {
        public async Task<ActionResult> Jokes(JokesViewModel model)
        {
            ViewBag.Message = "All jokes";

            model.NewJoke = new Joke
            {
                Author = User.Identity.Name,
                Date = DateTime.Now.Date
            };
            model.Jokes = await LoadJokes();

            return View(model);
        }

        // POST: Jokes/AddJoke
        [HttpPost]
        public ActionResult AddJoke(JokesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jokes = LoadJokes().Result;
                model.NewJoke.Id = jokes.Any() ? jokes.Max(joke => joke.Id) + 1 : 1;
                jokes.Add(model.NewJoke);
                SaveJokes(jokes);
            }

            return RedirectToAction("Jokes");
        }

        public ActionResult VoteForJoke(long jokeId, int userVote)
        {
            var user = User.Identity.Name;

            var jokes = LoadJokes().Result;

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

        private async Task<List<Joke>> LoadJokes()
        {
            //var jokes = await CallRestMethod<List<Joke>>(HttpMethod.Get, "LazarusJokes/api/jokes");

            var jokes = await RestUtils.CallGetMethodAsync<List<Joke>>("LazarusJokes/api/jokes");


            return jokes;
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

        private static async Task<TResult> CallRestMethod<TResult>(HttpMethod httpMethod, string methodUri, object parameter = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60887/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (httpMethod == HttpMethod.Get)
                {
                    var response = await client.GetAsync(methodUri);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsAsync<TResult>();

                    return result;
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    var response = await client.PostAsJsonAsync(methodUri, parameter);
                    response.EnsureSuccessStatusCode();

                    return default(TResult);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Unsupported HTTP method");
                }
            }
        }
    }


    public static class RestUtils
    {
        public static Task<TResult> CallGetMethodAsync<TResult>(string methodUri)
        {
            return CallRestMethodAsync<TResult>(methodUri, (client) => client.GetAsync(methodUri));
        }

        public static async Task<TResult> CallRestMethodAsync<TResult>(string methodUri, Func<HttpClient, Task<HttpResponseMessage>> func)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60887/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await func.Invoke(client);
            }
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<TResult>();
            return result;
        }
    }
}
