using System.Threading.Tasks;
using Application.User;

namespace Application.Interfaces
{
    public interface ISocialAccessor
    {
         Task<FacebookUserInfo> FacebookLogin(string accessToken);
    }
}