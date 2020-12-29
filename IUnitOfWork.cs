using ImageRepo.Repository;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    void Save();
}
