namespace TaskFlow.WebAPI.Domain
{
    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }

    public record TaskItem
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Title { get; init; }
        public string? Description { get; init; }
        public TaskStatus Status { get; init; } = TaskStatus.Todo;
        public DateOnly? DueDate { get; init; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
    }
}
