
namespace backend.Domain.Interfaces;

public interface IRepository<TEntity, TId> where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>
{
    Task<int> CountAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default
    );
    Task<TEntity?> GetByIdAsync(
        TId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        CancellationToken cancellationToken = default);
    Task<EntitiesResponse<TEntity>> QueryAsync(
        (int page, int pageSize)? pagination = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default
        );
    Task<EntitiesResponse<TProject>> QueryAsync<TProject>(
        (int page, int pageSize)? pagination = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Func<IQueryable<TEntity>, IQueryable<TProject>>? project = null,
        CancellationToken cancellationToken = default
    ) where TProject : BaseResponse<TId>;
    Task<TId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TId entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query(); // Be careful with exposing IQueryable!
}