using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repositorys;
using Service.Contract;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository accountRepository;

        public AccountService(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<object> Login(LoginRequestDto request)
        {
            var login = await accountRepository.Login(request);
            return login;
        }
    }
}
