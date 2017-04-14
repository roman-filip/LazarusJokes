using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RFI.LazarusJokes.Services.Models
{
    public class SimpleJoke
    {
        [Required]
        public string Author { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [JsonProperty(PropertyName = "JokeText")]  // TODO - remove it
        public string Text { get; set; }
    }
}