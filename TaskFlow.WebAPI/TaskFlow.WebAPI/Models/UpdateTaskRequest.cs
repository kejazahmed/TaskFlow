using System.ComponentModel.DataAnnotations;

namespace TaskFlow.WebAPI.Models
{
    public class UpdateTaskRequest
    {
        [Required, StringLength(120, MinimumLength = 1)]
        public string Title { get; init; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; init; }

        [EnumDataType(typeof(TaskFlow.WebAPI.Domain.TaskStatus))]
        public TaskFlow.WebAPI.Domain.TaskStatus Status { get; init; } = TaskFlow.WebAPI.Domain.TaskStatus.Todo;

        public DateOnly? DueDate { get; init; }
    }
}
