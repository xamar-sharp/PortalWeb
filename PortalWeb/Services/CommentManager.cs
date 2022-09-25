using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System;
namespace PortalWeb.Services
{
    public sealed class CommentManager
    {
        private readonly Repository _repos;
        public CommentManager(Repository repos)
        {
            _repos = repos;
        }
        public async Task<bool> AddAsync(string userName,Comment comment)
        {
            if((await _repos.Users.FirstOrDefaultAsync(ent=>ent.UserName==userName)) is CustomUser user)
            {
                comment.User = user;
                await _repos.Comments.AddAsync(comment);
                try
                {
                    await _repos.SaveChangesAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
