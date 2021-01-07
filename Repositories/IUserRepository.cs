using ImageRepo.Entities;

namespace ImageRepo.Repositories
{
    public interface IUserRepository : IRepositoryBase<User> {
        bool Exists(string username);
     }
}