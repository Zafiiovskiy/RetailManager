
using RMDesktopUI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string userName, string password);

        void LoggOffUser();
        Task GetLoggedInUserInfo(string token);
        HttpClient client { get; }
    }
}