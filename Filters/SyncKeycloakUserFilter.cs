using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using KeycloakAuth.Data;
using KeycloakAuth.Entities;

namespace KeycloakAuth.Filters;

public class SyncKeycloakUserFilter(AppDbContext dbContext) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var principal = context.HttpContext.User;

        if (principal.Identity is { IsAuthenticated: true })
        {
            var keycloakIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(keycloakIdClaim, out Guid userId))
            {
                var name = principal.FindFirst("name")?.Value 
                           ?? principal.FindFirst("preferred_username")?.Value 
                           ?? "Usuário Keycloak";
                
                var email = principal.FindFirst(ClaimTypes.Email)?.Value 
                            ?? "sem-email@provedor.com";

                var userOnDb = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (userOnDb == null)
                {
                    var newUser = new User 
                    { 
                        Id = userId, 
                        Name = name, 
                        Email = email,
                        CreatedAt = DateTime.UtcNow 
                    };
                    dbContext.Users.Add(newUser);
                    await dbContext.SaveChangesAsync();
                }
                else if (userOnDb.Name != name || userOnDb.Email != email)
                {
                    userOnDb.Name = name;
                    userOnDb.Email = email;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        await next();
    }
}