
namespace backend.Domain.Common;
public record EntitiesResponse<TEntity>
{
    public List<TEntity> Entities { get; set; } = new();
    public int TotalCount { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }


    public EntitiesResponse(List<TEntity> entities, int totalCount, int? pageNumber, int? pageSize)
    {
        Entities = entities;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}