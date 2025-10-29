using AutoMapper;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Infrastructure.Data.Repositories;

public class CourseRepository : DefaultRepositoryImplementation<Course, CourseId>
{
    public CourseRepository(IApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    
}