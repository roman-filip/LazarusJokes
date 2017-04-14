using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace RFI.LazarusJokes.Services.Models
{
    // TODO - inherit from SimpleJoke???
    public class Joke
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Autor vtipu")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Vtip")]
        [DataType(DataType.MultilineText)]
        public string JokeText { get; set; }

        public List<UserVote> UserVotes { get; set; }

        [Display(Name = "Celkový počet bodů")]
        public int? TotalVote
        {
            get
            {
                return UserVotes.Sum(vote => vote.Vote);
            }
        }

        [XmlIgnore]
        public List<UserVote> VotesOfCurrentUser { get; set; }

        public int? UserVote
        {
            get
            {
                return VotesOfCurrentUser.Any() ? VotesOfCurrentUser[0].Vote : default(int?);
            }
        }

        public bool? VotingClosed { get; set; }

        public static Joke FromSimpleJoke(SimpleJoke simpleJoke)
        {
            return new Joke
            {
                JokeText = simpleJoke.Text,
                Date = simpleJoke.Date,
                Author = simpleJoke.Author
            };
        }
    }
}