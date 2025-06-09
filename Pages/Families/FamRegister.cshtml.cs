using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuPairServices.Pages.Families
{
    public class FamRegisterModel : PageModel
    {
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
    }
}
