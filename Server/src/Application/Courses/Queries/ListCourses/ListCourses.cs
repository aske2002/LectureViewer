using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Application.Common.Models;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Queries.ListCourses;

public record ListCoursesWithPaginationQuery : IRequest<FilteredList<CourseDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListCoursesWithPaginationQueryHandler : IRequestHandler<ListCoursesWithPaginationQuery, FilteredList<CourseDto>>
{
    private readonly IRepository<Course, CourseId> _repository;

    public ListCoursesWithPaginationQueryHandler(IRepository<Course, CourseId> repository)
    {
        _repository = repository;
    }

    public async Task<FilteredList<CourseDto>> Handle(ListCoursesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var courses = await _repository.QueryAsync<CourseDto>(
            orderBy: q => q.OrderBy(c => c.Name),
            cancellationToken: cancellationToken,
            pagination: (request.PageNumber, request.PageSize));
        return FilteredList<CourseDto>.Create(courses, orderBy: null);
    }
}
