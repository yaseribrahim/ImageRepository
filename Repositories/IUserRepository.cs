using ImageRepo.Entities;

namespace ImageRepo.Repository
{
    public interface IUserRepository : IRepositoryBase<User> {
        bool Exists(string username);
     }
}