using System.ComponentModel.DataAnnotations;

namespace GadevangTennisklub2025.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Display(Name = "Event navn")]
        [Required(ErrorMessage = "Du skal angive et navn")]
        public string Title { get; set; }
        [Display(Name = "Dato")]
        public DateTime Date { get; set; }
        [Display(Name = "Beskrivelse")]
        [Required(ErrorMessage = "Du skal beskrive dit event")]
        public string Description { get; set; }
        [Display(Name = "Antal pladser")]
        [Required(ErrorMessage = "du skal angive hvormange der kommer ")]
        public int Maximum { get; set; }

        public Event() { }
        public Event(int id, string title, DateTime date, string desc, int max) 
        {
            Id = id;
            Title = title;
            Date = date;
            Description = desc;
           Maximum = max;
        }
    }
}
