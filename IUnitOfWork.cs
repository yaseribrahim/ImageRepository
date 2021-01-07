using ImageRepo.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IImageRepository Images { get; }
    void Save();
}
