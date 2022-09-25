using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace PortalWeb.Areas.First.Models
{
    [Bind("Login","Password","Email","Icon")]
    public sealed class AuthorizationModel
    {
        [StringLength(32,MinimumLength =6,ErrorMessage ="login_length")]
        [ScaffoldColumn(true)]
        [Display(Name ="login")]
        public string Login { get; set; }
        [StringLength(10,MinimumLength =5,ErrorMessage ="pass_length")]
        [ScaffoldColumn(true)]
        [Display(Name ="pass")]
        public string Password { get; set; }
        [ScaffoldColumn(true)]
        [Display(Name ="icon")]
        public IFormFile Icon { get; set; }
        [Display(Name ="email")]
        [DataType(DataType.EmailAddress)]
        [ScaffoldColumn(true)]
        public string Email { get; set; }
    }
}
