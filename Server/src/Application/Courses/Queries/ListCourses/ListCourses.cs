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

public record ListCoursesWithPaginationQuery : IRequest<FilteredList<CourseEntityDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListCoursesWithPaginationQueryHandler : IRequestHandler<ListCoursesWithPaginationQuery, FilteredList<CourseEntityDto>>
{
    private readonly IRepository<Course, CourseId> _repository;

    public ListCoursesWithPaginationQueryHandler(IRepository<Course, CourseId> repository)
    {
        _repository = repository;
    }

    public async Task<FilteredList<CourseEntityDto>> Handle(ListCoursesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var courses = await _repository.QueryAsync(
            orderBy: q => q.OrderBy(c => c.Name),
            cancellationToken: cancellationToken,
include: q => q
                .Include(c => c.Semester)
                .Include(c => c.Instructors),
            project: q => q.Select(c => new CourseEntityDto()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Semester = new SemesterDto
                {
                    Id = c.Semester.Id,
                    Season = c.Semester.Season,
                    Year = c.Semester.Year
                },
                Colour = c.Colour,
                InternalIdentifier = c.InternalIdentifier,
                Instructors = c.Instructors.Select(i => new PublicUser
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    UserName = i.UserName
                }).ToList()
            }),
            pagination: (request.PageNumber, request.PageSize));

        return FilteredList<CourseEntityDto>.Create(courses, orderBy: null);
    }
}
