using System.Threading.Tasks;
using PortalWeb.Services.Options;
namespace PortalWeb.Services
{
    public interface IIdentityAuthenticator
    {
        Task<bool> SignInAsync(AuthenticationOptions options);
        Task<bool> SignUpAsync(AuthenticationOptions options);
        string DeclareRole(AuthenticationOptions options);
    }
}
