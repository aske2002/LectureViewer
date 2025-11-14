using AutoMapper;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Application.Common.Models;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Queries.ListCourses;

public record ListCoursesWithPaginationQuery : IRequest<List<CourseListDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListCoursesWithPaginationQueryHandler : IRequestHandler<ListCoursesWithPaginationQuery, List<CourseListDto>>
{
    private readonly ICourseService _courseService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListCoursesWithPaginationQueryHandler(ICourseService courseService, IApplicationDbContext context, IMapper mapper)
    {
        _courseService = courseService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CourseListDto>> Handle(ListCoursesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseService.ListCoursesAsync();
        return _mapper.Map<List<CourseListDto>>(courses);
    }
}
