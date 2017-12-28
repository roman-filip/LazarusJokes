using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace RFI.LazarusJokes.Services.Models
{
    public class JokeSimple
    {
        [Required]
        public string Author { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [XmlElement(ElementName = "JokeText")] // TODO - remove it
        public string Text { get; set; }
    }
}