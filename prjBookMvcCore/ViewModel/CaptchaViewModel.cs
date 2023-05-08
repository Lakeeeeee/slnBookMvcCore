using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace prjBookMvcCore.ViewModel
{
    public class CaptchaViewModel
    {
        [Required]
        public string? Captcha { get; set; }
    }
}
