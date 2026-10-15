using TaskFlow.WebAPI.Domain;
using TaskFlow.WebAPI.Infrastructure;
using TaskFlow.WebAPI.Models;

namespace TaskFlow.WebAPI.Service
{
    public class TaskService(ITaskRepository repository) : ITaskService
    {
        public IReadOnlyList<TaskResponse> GetAll() => repository.GetAll().Select(ToResponse).ToArray();

        public TaskResponse? GetById(Guid id)
        {
            var task = repository.GetById(id);
            return task is null ? null : ToResponse(task);
        }

        public TaskResponse Create(CreateTaskRequest request)
        {
            var now = DateTimeOffset.UtcNow;
            var task = new TaskItem
            {
                Title = request.Title.Trim(),
                Description = CleanDescription(request.Description),
                Status = request.Status,
                DueDate = request.DueDate,
                CreatedAt = now,
                UpdatedAt = now
            };
            return ToResponse(repository.Add(task));
        }

        public TaskResponse? Update(Guid id, UpdateTaskRequest request)
        {
            var existing = repository.GetById(id);
            if (existing is null) return null;

            var updated = existing with
            {
                Title = request.Title.Trim(),
                Description = CleanDescription(request.Description),
                Status = request.Status,
                DueDate = request.DueDate,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            return repository.Update(updated) ? ToResponse(updated) : null;
        }

        public bool Delete(Guid id) => repository.Delete(id);

        private static string? CleanDescription(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static TaskResponse ToResponse(TaskItem task) => new(
            task.Id, task.Title, task.Description, task.Status, task.DueDate, task.CreatedAt, task.UpdatedAt);
    }
}
