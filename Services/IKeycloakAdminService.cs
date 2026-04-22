using System.Security.Claims;

namespace KeycloakAuth.Services;

using KeycloakAuth.DTOs;

public interface IKeycloakAdminService
{
    Task AddRoleToUserAsync(string userId, string roleName);
    Task RemoveRoleFromUserAsync(string userId, string roleName);
    Task SetUserAttributeAsync(string userId, string attributeName, string attributeValue);
    Task RemoveAttributeFromUserAsync(string userId, string attributeName);
    Task<CreateUserResponseDto> CreateUserAsync(CreateUserDto newUser);
}