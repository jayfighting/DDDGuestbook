using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.API.ViewModels
{
    // Note: doesn't expose events or behavior
    public class ClaimViewModel
    {
        [Required]
        [Display(Name = "Claim Type")]
        public string Type { get; set; }
 
        [Required]
        [Display(Name = "Claim Value")]
        public string Value { get; set; }
    }
}
