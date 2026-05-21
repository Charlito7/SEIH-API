namespace Core.Application.Model.Features;

public class AddRolesToUserDto
{
    public string UserEmail { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
