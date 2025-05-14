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
        public IMemberService memberService;
        public ICommentServiceAsync commentService;
        [BindProperty]
        public BlogPost Post { get; set; }
        public string Member;
        [BindProperty]
        public Comment MakeComment { get; set; }
        public List<Comment> PostComments { get; set; }
        public BlogPostModel(IBlogPostServicesAsync IBPSA, IMemberService IMS, ICommentServiceAsync ICSA) 
        {
            blogPostServicesAsync = IBPSA;
            memberService = IMS;
            commentService = ICSA;
           
        }
        public async Task OnGet(int BlogId)
        {
            Post = await blogPostServicesAsync.GetBlogPost(BlogId);
            Member= memberService.GetMemberById(Post.MemberId).Result.Name;
            PostComments = await commentService.GetCommentsForPost(BlogId);

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

        public async Task<IActionResult> OnPostComment() 
        {
            await commentService.CreateComment(new Comment(0,Post.Id,int.Parse(HttpContext.Session.GetString("Member_Id")),MakeComment.CommentContent));
            OnGet(Post.Id);
            return Page();
        }
    }
}
