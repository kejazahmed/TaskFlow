namespace TaskFlow.WebAPI.Models
{
    public record TaskResponse(
        Guid Id,
        string Title,
        string? Description,
        TaskFlow.WebAPI.Domain.TaskStatus Status,
        DateOnly? DueDate,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);
}
