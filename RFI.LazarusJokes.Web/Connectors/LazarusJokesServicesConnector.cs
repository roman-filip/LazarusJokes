using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RFI.LazarusJokes.Web.Models;

namespace RFI.LazarusJokes.Web.Connectors
{
    public class LazarusJokesServicesConnector : ILazarusJokesServicesConnector
    {
        public async Task<List<Joke>> LoadJokesAsync()
        {
            var jokes = await RestUtils.CallGetMethodAsync<List<Joke>>(LazarusJokesServicesUri.LoadJokes);
            return jokes;
        }

        public Task<Joke> AddJokeAsync(JokeSimple joke)
        {
            return RestUtils.CallPostMethodAsync<Joke, JokeSimple>(LazarusJokesServicesUri.AddJoke, joke);
        }
    }


    // TODO extract class
    public static class RestUtils
    {
        public static Task<TResult> CallGetMethodAsync<TResult>(string methodUri)
        {
            return CallRestMethodAsync<TResult>(methodUri, (client) => client.GetAsync(methodUri));
        }

        public static Task<TResult> CallPostMethodAsync<TResult, TData>(string methodUri, TData data)
        {
            return CallRestMethodAsync<TResult>(methodUri, (client) => client.PostAsJsonAsync(methodUri, data));
        }

        public static async Task<TResult> CallRestMethodAsync<TResult>(string methodUri, Func<HttpClient, Task<HttpResponseMessage>> func)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60887/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await func.Invoke(client).ConfigureAwait(false);
            }
            response.EnsureSuccessStatusCode();   // TODO add functionality what gets error message from response

            var result = await response.Content.ReadAsAsync<TResult>().ConfigureAwait(false);
            return result;
        }
    }
}