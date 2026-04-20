using KeycloakAuth.Entities;

namespace KeycloakAuth.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);
}