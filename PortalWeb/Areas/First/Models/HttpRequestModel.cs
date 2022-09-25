using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace PortalWeb.Areas.First.Models
{
    [Bind("Url","Method","Content","MimeType")]
    public sealed class HttpRequestModel
    {
        [DataType(DataType.Url,ErrorMessage ="url_semantic")]
        [Required(ErrorMessage ="url_required")]
        [ScaffoldColumn(true)]
        [Display(Name ="url")]
        public string Url { get; set; }
        [ScaffoldColumn(true)]
        [Method]
        [Display(Name ="method")]
        [Required(ErrorMessage ="method_required")]
        public string Method { get; set; }
        [Display(Name ="file")]
        public IFormFile Content { get; set; }
        [Display(Name ="mime")]
        public string MimeType { get; set; }
    }
    public sealed class Method : ValidationAttribute {
        private readonly ICollection<string> _methodIDs;
        public Method()
        {
            _methodIDs = System.Enum.GetNames<MethodName>();
        }
        public override bool IsValid(object value)
        {
            return _methodIDs.Contains((System.Enum.Parse<MethodName>(value.ToString()).ToString()));
        }
    }

}
