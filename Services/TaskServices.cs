using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeycloakAuth.Data;
using KeycloakAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeycloakAuth.Services;

public class TaskServices(AppDbContext context) : ITaskService
{
    public async Task<List<TaskItem>> GetTaskByUser(Guid UserId)
    {
        return await context.Tasks
            .Where(t => t.UserId == UserId)
            .ToListAsync();
    }

    public async Task CreateTask(string Title, Guid UserId)
    {
        var task = new TaskItem
        {
            Title = Title,
            UserId = UserId
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync();
    }

    public async Task<TaskItem?> CompleteTask(Guid TaskId, Guid UserId)
    {
        var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == TaskId && t.UserId == UserId);

        if (task == null) return null;

        task.IsCompleted = !task.IsCompleted; // Toggle
        await context.SaveChangesAsync();
        return task;
    } 
}