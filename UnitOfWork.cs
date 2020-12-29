using ImageRepo.Entities;
using ImageRepo.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly RepositoryContext repositoryContext;
    private IUserRepository userRepository;
    public UnitOfWork(RepositoryContext repositoryContext)
    {
        this.repositoryContext = repositoryContext;
    }

    public IUserRepository Users
    {
        get
        {
            if (userRepository == null)
            {
                userRepository = new UserRepository(repositoryContext);
            }
            return userRepository;
        }
    }

    public void Save()
    {
        repositoryContext.SaveChanges();
    }
}