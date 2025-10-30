using AutoMapper;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Domain.Common;
using backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data.Repositories;
public class DefaultRepositoryImplementation<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : StronglyTypedId<TId>
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DefaultRepositoryImplementation(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
    }

    public virtual async Task<TId> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    private IQueryable<TEntity> WithFilter((int page, int pageSize)? pagination = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        if (filter != null)
        {
            query = filter(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (pagination is { page: int page, pageSize: int pageSize })
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return query;
    }

    public async Task<EntitiesResponse<TEntity>> QueryAsync((int page, int pageSize)? pagination = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
    {
        var entities = await WithFilter(pagination, filter, orderBy, include).ToListAsync(cancellationToken);
        var totalCount = await CountAsync(filter, orderBy, include, cancellationToken);
        return new(entities, totalCount, pagination?.page, pagination?.pageSize);
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        TId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        query = query.Where(e => e.Id.Equals(id));

        if (filter != null)
        {
            query = filter(query);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<EntitiesResponse<TProject>> QueryAsync<TProject>((int page, int pageSize)? pagination = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, Func<IQueryable<TEntity>, IQueryable<TProject>>? project = null, CancellationToken cancellationToken = default) where TProject : BaseResponse<TId>
    {
        var entittyQuery = WithFilter(pagination, filter, orderBy, include);
        var totalCount = await CountAsync(filter, orderBy, include, cancellationToken);

        if (project != null)
        {
            var entities = await project(entittyQuery).ToListAsync(cancellationToken);
            return new(entities, totalCount, pagination?.page, pagination?.pageSize);
        }
        else
        {
            var entities = await MappingExtensions.MapResourcesAsync<TProject, TEntity, TId>(_context, _mapper, entittyQuery);
            return new(entities, totalCount, pagination?.page, pagination?.pageSize);
        }
    }

    public virtual IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }

    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RemoveAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken:cancellationToken);
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
    {
        return await WithFilter(null, filter, orderBy, include).CountAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return _dbSet.AnyAsync(e => e.Id.Equals(id), cancellationToken);
    }
}