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
        public static string stringtest { get; set; }
        [BindProperty]
        public BlogPost Post { get; set; }
        public string Member;
        [BindProperty]
        public Comment MakeComment { get; set; }
        [BindProperty]
        public List<Comment> PostComments { get; set; }
        [BindProperty]
        public string test { get; set; }
        [BindProperty]
        public Comment EditComment { get; set; }
        public BlogPostModel(IBlogPostServicesAsync IBPSA, IMemberService IMS, ICommentServiceAsync ICSA) 
        {
            blogPostServicesAsync = IBPSA;
            memberService = IMS;
            commentService = ICSA;
           
        }
        public async Task OnGet(int BlogId)
        {
           
            Post =  blogPostServicesAsync.GetBlogPost(BlogId).Result;
            Models.Member t=  memberService.GetMemberById(Post.MemberId).Result;
            Member = t.Name;
            PostComments =  commentService.GetCommentsForPost(BlogId).Result;

        }

        public async Task<IActionResult> OnPostDelete() 
        {
            await  blogPostServicesAsync.DeleteBlogPost(Post);
            return RedirectToPage("BlogSide");
        }
        public async Task<IActionResult> OnpostUpdate() 
        {
            await blogPostServicesAsync.UpdateBlogPost(Post);
            return RedirectToPage("BlogSide");
        }

        public async Task<IActionResult> OnPostComment() 
        {
            test = "";
            if (!ModelState.IsValid) 
            {
                OnGet(Post.Id);
                return Page();
            }
            await commentService.CreateComment(new Comment(0,Post.Id,int.Parse(HttpContext.Session.GetString("Member_Id")),MakeComment.CommentContent));
            //OnGet(Post.Id);
            return RedirectToPage("BlogSide");
        }
        public async Task<IActionResult> OnPostDeleteComment(int id)
        {
            await commentService.DeleteComment(await commentService.GetComment(id));
            return RedirectToPage("BlogSide");
        }
        public async Task<IActionResult> OnpostUpdateComment(int id, string text)
        {
            Comment comment = await commentService.GetComment(id);
            comment.CommentContent = EditComment.CommentContent;
            await commentService.UpdateComment(comment);
            return RedirectToPage("BlogSide");
        }
    }

}
