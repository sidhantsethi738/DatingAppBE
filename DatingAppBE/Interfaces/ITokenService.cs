using DatingAppBE.Entities;

namespace DatingAppBE.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
