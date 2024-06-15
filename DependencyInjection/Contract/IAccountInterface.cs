using Entities.Models;

namespace Contract
{
    public interface IAccountInterface
    {
        Task<object> Login(LoginRequestDto request);
        Task<object> Register(RegisterRequestDto request);
        Task<object> Logout();
        Task<object> RegisterAdmin(RegisterRequestDto request);
        Task<object> RefreshToken(TokenModel token);
    }
}
