using System.Linq;
using ImageRepo.Entities;

namespace ImageRepo.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public bool Exists(string username)
        {
            return repositoryContext.Users.Any(e => e.Username.Equals(username));
        }
    }
}