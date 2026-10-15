using System.Collections.Concurrent;
using TaskFlow.WebAPI.Domain;

namespace TaskFlow.WebAPI.Infrastructure
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

        public InMemoryTaskRepository()
        {
            var now = DateTimeOffset.UtcNow;
            var samples = new[]
            {
            new TaskItem { Title = "Review pull request", Description = "Review the feature branch and add comments.", Status = TaskFlow.WebAPI.Domain.TaskStatus.InProgress, CreatedAt = now, UpdatedAt = now },
            new TaskItem { Title = "Write unit tests", Description = "Cover service validation and CRUD operations.", Status = TaskFlow.WebAPI.Domain.TaskStatus.Todo, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)), CreatedAt = now, UpdatedAt = now },
            new TaskItem { Title = "Set up project", Description = "Create the initial application structure.", Status = TaskFlow.WebAPI.Domain.TaskStatus.Done, CreatedAt = now, UpdatedAt = now }
        };
            foreach (var task in samples) _tasks.TryAdd(task.Id, task);
        }

        public IReadOnlyList<TaskItem> GetAll() => _tasks.Values
            .OrderBy(t => t.Status)
            .ThenBy(t => t.DueDate)
            .ThenByDescending(t => t.CreatedAt)
            .ToArray();

        public TaskItem? GetById(Guid id) => _tasks.GetValueOrDefault(id);

        public TaskItem Add(TaskItem task)
        {
            if (!_tasks.TryAdd(task.Id, task)) throw new InvalidOperationException("A task with this ID already exists.");
            return task;
        }

        public bool Update(TaskItem task)
        {
            while (_tasks.TryGetValue(task.Id, out var current))
            {
                if (_tasks.TryUpdate(task.Id, task, current)) return true;
            }
            return false;
        }

        public bool Delete(Guid id) => _tasks.TryRemove(id, out _);
    }
}
