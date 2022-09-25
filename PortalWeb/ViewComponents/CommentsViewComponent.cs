using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using PortalWeb;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
namespace PortalWeb.ViewComponents
{
    public sealed class CommentsViewComponent:ViewComponent
    {
        private readonly Repository _repos;
        public CommentsViewComponent(Repository repos)
        {
            _repos = repos;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Main",model: await _repos.Comments.ToListAsync());
        }
    }
}
