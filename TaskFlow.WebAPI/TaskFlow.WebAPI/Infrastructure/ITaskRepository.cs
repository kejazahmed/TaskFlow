using TaskFlow.WebAPI.Domain;

namespace TaskFlow.WebAPI.Infrastructure
{
    public interface ITaskRepository
    {
        IReadOnlyList<TaskItem> GetAll();
        TaskItem? GetById(Guid id);
        TaskItem Add(TaskItem task);
        bool Update(TaskItem task);
        bool Delete(Guid id);
    }
}
