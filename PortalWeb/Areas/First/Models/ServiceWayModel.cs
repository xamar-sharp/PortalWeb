using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc;
namespace PortalWeb.Areas.First.Models
{
    [Bind("Intent")]
    public sealed class ServiceWayModel
    {
        [Display(Name ="intent")]
        [ScaffoldColumn(true)]
        [StringLength(32,MinimumLength =1,ErrorMessage ="intent_error")]
        public string Intent { get; set; }
    }
}
