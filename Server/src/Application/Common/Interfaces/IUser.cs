using System.Security.Claims;

namespace backend.Application.Common.Interfaces;

public interface IUser
{
    string? Id { get; }
    ClaimsPrincipal? Principal { get; }
}
