using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace GadevangTennisklub2025.Pages.Blog
{
    public class BlogPostModel : PageModel
    {
        IBlogPostServicesAsync blogPostServicesAsync;
        IMemberService memberService;
        [BindProperty]
        public BlogPost Post { get; set; }
        public string Member;
        public BlogPostModel(IBlogPostServicesAsync IBPSA, IMemberService IMS) 
        {
            blogPostServicesAsync = IBPSA;
            memberService = IMS;
        }
        public async Task OnGet(int BlogId)
        {
            Post = await blogPostServicesAsync.GetBlogPost(BlogId);
            Member= memberService.GetMemberById(Post.MemberId).Result.Name;
        }

        public async Task<IActionResult> OnPostDelete() 
        {
            blogPostServicesAsync.DeleteBlogPost(Post);
            return RedirectToPage("BlogSide");
        }
        public async Task<IActionResult> OnpostUpdate() 
        {
            blogPostServicesAsync.UpdateBlogPost(Post);
            return RedirectToPage("BlogSide");
        }
    }
}
