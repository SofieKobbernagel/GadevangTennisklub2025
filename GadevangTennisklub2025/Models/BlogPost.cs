using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GadevangTennisklub2025.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        
        [Display(Name = "BlogTitel")]
        [Required(ErrorMessage = "Du skal angive navnet på dit indslag")]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ\s]{3,}$", ErrorMessage = "Navnet på dit indslag må kun indeholde bogstaver og skal være på mindst 3 tegn.")]
        public string Title { get; set; }
        [Display(Name = "indslag indhold")]
        [Required(ErrorMessage = "Du skal angive inhold til dit indslag")]
        public string Content { get; set; }
        public int MemberId { get; set; }
        public BlogPost() { }
        public BlogPost(int id, string title, string content, int mid) 
        {
            Id = id;
            Title = title;
            Content = content;
            MemberId = mid;
        }
    }
}
