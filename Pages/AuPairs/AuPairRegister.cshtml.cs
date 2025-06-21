using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuPairServices.Pages.AuPairs
{
    public class AuPairRegisterModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AuPairRegisterModel> _logger;
        private const long MaxFileSize = 20 * 1024 * 1024; // 20MB

        public string Message { get; set; }

        public AuPairRegisterModel(IWebHostEnvironment environment, ILogger<AuPairRegisterModel> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [BindProperty]
        public IFormFile VideoFile { get; set; }

        [Required(ErrorMessage = "Family name is required.")]
        public string? FamilyName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [BindProperty]
        public IFormFile? Photo1 { get; set; }
        [BindProperty]
        public IFormFile? Photo2 { get; set; }
        [BindProperty]
        public IFormFile? Photo3 { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (VideoFile == null || VideoFile.Length == 0)
            {
                ModelState.AddModelError("VideoFile", "Please select a video file");
                return Page();
            }

            if (!VideoFile.ContentType.StartsWith("video/"))
            {
                ModelState.AddModelError("VideoFile", "Only video files are allowed");
                return Page();
            }

            if (VideoFile.Length > MaxFileSize)
            {
                ModelState.AddModelError("VideoFile", "File size exceeds 20MB limit");
                return Page();
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + VideoFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await VideoFile.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"Video uploaded successfully: {uniqueFileName}");
                return RedirectToPage("./UploadSuccess");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video file");
                ModelState.AddModelError("", "An error occurred while uploading the file");
                return Page();
            }
        }
    }
}
