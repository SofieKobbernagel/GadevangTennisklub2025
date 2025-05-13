using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Blog
{
    public class BlogSideModel : PageModel
    {
        public IBlogPostServicesAsync blogPostServicesAsync;
        public IMemberService memberService;    
        public List<BlogPost> BlogList { get; set; }

        public BlogSideModel(IBlogPostServicesAsync IBPSA, IMemberService IMS) 
        {
            memberService = IMS;
           blogPostServicesAsync = IBPSA; 
            BlogList = new List<BlogPost>();
        }
        public async Task OnGet()
        {
            BlogList = await blogPostServicesAsync.GetAllBlogPost();
            BlogList.Reverse();
        }

    }
}
