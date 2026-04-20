using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KeycloakAuth.Filters;
using KeycloakAuth.Services; 
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ServiceFilter(typeof(SyncKeycloakUserFilter))]
public class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var tasks = await taskService.GetTaskByUser(userId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string title)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await taskService.CreateTask(title, userId);
        return Ok();
    }
    [HttpPatch("{id:guid}/toggle-complete")]
    public async Task<IActionResult> ToggleComplete(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var success = await taskService.CompleteTask(id, userId);

        if (!success) 
        {
            return NotFound("Tarefa não encontrada ou você não tem permissão para editá-la.");
        }

        return NoContent();
    }
}