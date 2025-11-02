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
    private readonly IRepository<Course, CourseId> _repository;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListCoursesWithPaginationQueryHandler(IRepository<Course, CourseId> repository, IApplicationDbContext context, IMapper mapper)
    {
        _repository = repository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CourseListDto>> Handle(ListCoursesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        // First check if there are any course instructors in the database
        var instructorCount = await _context.CourseInstructors.CountAsync(cancellationToken);
        var courseCount = await _context.Courses.CountAsync(cancellationToken);
        
        var courses = await _context.Courses
                .Include(c => c.Semester)
                .Include(c => c.Instructors)
                    .ThenInclude(ci => ci.Instructor)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

        // Debug: Check raw data
        var debugInfo = courses.Select(c => new { 
            CourseName = c.Name, 
            InstructorCount = c.Instructors?.Count ?? 0,
            InstructorNames = c.Instructors?.Select(ci => $"{ci.Instructor?.FirstName} {ci.Instructor?.LastName}").ToList() ?? new List<string>()
        }).ToList();

        var result = _mapper.Map<List<CourseListDto>>(courses);
        
        // Debug: Check mapped data
        var mappedDebugInfo = result.Select(c => new { 
            CourseName = c.Name, 
            InstructorCount = c.Instructors?.Count ?? 0 
        }).ToList();

        return result;
    }
}
