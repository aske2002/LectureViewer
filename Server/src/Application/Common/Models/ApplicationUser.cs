namespace backend.Application.Common.Models;


public class RegisterRequest
{
    /// <summary>
    /// The user's email address which acts as a user name.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }
    /// <summary>
    /// The user's first name.
    /// </summary>
    public required string FirstName { get; init; }
    /// <summary>
    /// The user's last name.
    /// </summary>
    public required string LastName { get; init; }
}

