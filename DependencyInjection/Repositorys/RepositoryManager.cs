using Contract;

namespace Repositorys
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext repositoryContext;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }
        public Task SaveAsync() => repositoryContext.SaveChangesAsync();
    }
}
