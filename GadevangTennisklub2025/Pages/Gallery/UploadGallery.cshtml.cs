using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GadevangTennisklub2025.Pages.Gallery
{
    public class UploadGalleryModel : PageModel
    {

        private IGalleryService _galleryService;
        private readonly IWebHostEnvironment _env;

        public UploadGalleryModel(IGalleryService galleryService, IWebHostEnvironment env)
        {
            _galleryService = galleryService;
            _env = env;
        }

        [BindProperty]
        public IFormFile PhotoFile { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || PhotoFile == null)
            {
                Message = "Please select a valid photo.";
                return Page();
            }

            bool result = await _galleryService.UploadPhotoAsync(PhotoFile, Description, _env);

            Message = result ? "Photo uploaded successfully." : "Upload failed.";
            return Page();
        }
    }
}

