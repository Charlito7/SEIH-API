using Core.Domain.Entities;
using Core.Domain.Entity.SEIH;
using Core.Domain.Procedures.SEIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interface.Token;

public interface ITokenServices
{
    string BuildToken(string key, string issuer, string audience, UserEntity user, IList<string> userRoles);
    public string BuildToken(string key, string issuer, string audience, UserEntity user, Claim[] claims);
    bool ValidateTokenWithExpiryTime(string key, string issuer, string audience, string token);
    bool ValidateTokenWithoutExpiryTime(string key, string issuer, string audience, string token);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string key);
    ClaimsPrincipal GetPrincipalFromToken(string token);
    string BuildToken2(string key, string issuer, string audience, UsersEntity user, IEnumerable<GetUserRolesResponse> userRoles);
}
