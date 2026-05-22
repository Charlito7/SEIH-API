using Core.Domain.Procedures.SEIH;

namespace Core.Application.Model.Response;

public class UserSignInResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public bool? IsNewPasswordRequired { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Initial { get; set; }
    public IList<string>? UserRoles { get; set; }
}