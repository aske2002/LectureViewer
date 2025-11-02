using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public IList<CourseEnrollment> Enrollments { get; init; } = new List<CourseEnrollment>();
    public IList<CourseInstructor> Courses { get; init; } = new List<CourseInstructor>();
}