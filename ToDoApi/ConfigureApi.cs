using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi;

public static class ConfigureApi
{
    public static void ConfigureMappings(this WebApplication app)
    {
        app.MapGet("api/todo", async (ToDoDbContext context) =>
        {
            var items = await context.ToDos.ToListAsync();

            return Results.Ok(items);
        });

        app.MapGet("api/todo/{id}", async (ToDoDbContext context, int id) =>
        {
            var item = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);

            return Results.Ok(item);
        });

        app.MapPost("api/todo", async (ToDoDbContext context, ToDo todo) =>
        {
            await context.ToDos.AddAsync(todo);

            await context.SaveChangesAsync();

            return Results.Created($"api/todo/{todo.Id}", todo);
        });

        app.MapPut("api/todo/{id}", async (ToDoDbContext context, int id, ToDo todo) =>
        {
            var item = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);

            if (item is null)
                return Results.NotFound();

            item.ToDoName = todo.ToDoName;

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("api/todo/{id}", async (ToDoDbContext context, int id) =>
        {
            var item = context.ToDos.FirstOrDefault(t => t.Id == id);

            if (item is null)
                return Results.NotFound();

            context.ToDos.Remove(item);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
