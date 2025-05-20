using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Gallery
{
    public class ShowGalleryModel : PageModel
    {
        private IGalleryService _galleryService;

        public bool isAdmin { get; set; } = false;

        public ShowGalleryModel(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        public List<Models.Gallery> Photos { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("IsAdmin") != null && bool.Parse(HttpContext.Session.GetString("IsAdmin")) == true)
            {
                isAdmin = true;
            }
            Photos = await _galleryService.GetAllPhotos();
        }
    }
}
