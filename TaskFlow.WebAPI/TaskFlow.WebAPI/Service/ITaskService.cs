using TaskFlow.WebAPI.Models;

namespace TaskFlow.WebAPI.Service
{
    public interface ITaskService
    {
        IReadOnlyList<TaskResponse> GetAll();
        TaskResponse? GetById(Guid id);
        TaskResponse Create(CreateTaskRequest request);
        TaskResponse? Update(Guid id, UpdateTaskRequest request);
        bool Delete(Guid id);
    }
}
