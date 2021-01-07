using ImageRepo.Entities;
using ImageRepo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RepositoryContext repositoryContext;
    private IUserRepository userRepository;
    private IImageRepository imageRepository;
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

    public IImageRepository Images
    {
        get
        {
            if (imageRepository == null)
            {
                imageRepository = new ImageRepository(repositoryContext);
            }
            return imageRepository;
        }
    }

    public void Save()
    {
        repositoryContext.SaveChanges();
    }
}