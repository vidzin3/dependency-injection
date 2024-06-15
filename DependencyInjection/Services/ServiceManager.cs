using Contract;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repositorys;
using Service.Contract;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        public IAccountService accountService => _accountService.Value;
    }
}
