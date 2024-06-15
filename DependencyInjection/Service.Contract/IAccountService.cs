using Entities.Models;

namespace Service.Contract
{
    public interface IAccountService
    {
        Task<object> Login(LoginRequestDto request);
    }
}
