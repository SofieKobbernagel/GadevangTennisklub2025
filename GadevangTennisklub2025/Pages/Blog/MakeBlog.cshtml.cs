using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Blog
{
    public class MakeBlogModel : PageModel
    {
        public IBlogPostServicesAsync blogPostServicesAsync;
        [BindProperty]
        public BlogPost Post { get; set; }

        public MakeBlogModel(IBlogPostServicesAsync IBPSA)
        {
            Post = new BlogPost();
            blogPostServicesAsync = IBPSA;
        }
        public async Task OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Post.MemberId = int.Parse(HttpContext.Session.GetString("Member_Id"));
            blogPostServicesAsync.CreateBlogPost(Post);

            return RedirectToPage("BlogSide");
        }
    }
}
