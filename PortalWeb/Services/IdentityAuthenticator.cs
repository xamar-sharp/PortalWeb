using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PortalWeb.Services.Options;
namespace PortalWeb.Services
{
    public sealed class IdentityAuthenticator:IIdentityAuthenticator
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signManager;
        public IdentityAuthenticator(UserManager<CustomUser> userManager,SignInManager<CustomUser> signManager)
        {
            _signManager = signManager;
            _userManager = userManager;
        }
        public async Task<bool> SignInAsync(AuthenticationOptions options)
        {
            SignInResult res = await _signManager.PasswordSignInAsync(options.Login, options.Password, true, false);
            if (res.Succeeded)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> SignUpAsync(AuthenticationOptions options)
        {
            CustomUser user = new CustomUser() { UserName = options.Login, Email = options.Email, Icon = options.Icon };
            IdentityResult res = await _userManager.CreateAsync(user, options.Password);
            IdentityResult res2 = await _userManager.AddToRoleAsync(user, DeclareRole(options));
            if (res.Succeeded && res2.Succeeded)
            {
                await _signManager.SignInAsync(user, true);
                return true;
            }
            return false;
        }
        public string DeclareRole(AuthenticationOptions options) => options.Email == "xamacoredevelopment@gmail.com" ? "ADMIN" : "USER";
    }
}
