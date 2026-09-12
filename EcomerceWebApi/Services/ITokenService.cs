using EcommerceWebApi.Model;

namespace EcommerceWebApi.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}