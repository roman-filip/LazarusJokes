using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RFI.LazarusJokes.Web.Models
{
    public class JokeSimple
    {
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
        public string Text { get; set; }
    }
}