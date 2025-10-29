namespace backend.Infrastructure.Data.Seeders;
public interface IDataSeeder
{
    Task<bool> ShouldSeedAsync();
    Task SeedAsync(CancellationToken cancellationToken = default);
}