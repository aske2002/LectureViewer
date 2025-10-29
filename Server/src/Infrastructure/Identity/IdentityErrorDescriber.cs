using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace backend.Infrastructure.Identity;

public class IdentityResources { }

public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
    private readonly IStringLocalizer _localizer;

    public LocalizedIdentityErrorDescriber(IStringLocalizer<IdentityResources> factory)
    {
        _localizer = factory;

    }
    public override IdentityError DuplicateUserName(string userName)
    {
        var name = nameof(DuplicateUserName);
        var value = _localizer[name];
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = string.Format(_localizer[nameof(DuplicateUserName)], userName)
        };
    }

    public override IdentityError DuplicateEmail(string email) =>
        new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = string.Format(_localizer[nameof(DuplicateEmail)], email)
        };

    public override IdentityError PasswordTooShort(int length) =>
        new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = string.Format(_localizer[nameof(PasswordTooShort)], length)
        };

    // ...continue overriding other IdentityError methods similarly
}
