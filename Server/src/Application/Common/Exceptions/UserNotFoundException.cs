
using backend.Application.Common.Models;

namespace backend.Application.Common.Exceptions;
public class UserNotFoundException : NotFoundExceptionBase<string, UserNotFoundException>
{
    public override string EntityName => nameof(ApplicationUser);
    public UserNotFoundException(string userId)
        : base(userId) { }
}