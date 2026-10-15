using TaskFlow.WebAPI.Infrastructure;
using TaskFlow.WebAPI.Models;
using TaskFlow.WebAPI.Service;

namespace TaskFlow.Tests
{
    public sealed class TaskServiceTests
    {
        private static TaskService CreateService() => new(new InMemoryTaskRepository());

        [Fact]
        public void Create_TrimsTitleAndReturnsTask()
        {
            var service = CreateService();
            var result = service.Create(new CreateTaskRequest { Title = "  Prepare demo  " });
            Assert.Equal("Prepare demo", result.Title);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public void Update_ChangesExistingTask()
        {
            var service = CreateService();
            var created = service.Create(new CreateTaskRequest { Title = "Old title" });
            var updated = service.Update(created.Id, new UpdateTaskRequest { Title = "New title", Status = TaskFlow.WebAPI.Domain.TaskStatus.Done });
            Assert.NotNull(updated);
            Assert.Equal("New title", updated.Title);
            Assert.Equal(TaskFlow.WebAPI.Domain.TaskStatus.Done, updated.Status);
        }

        [Fact]
        public void Update_ReturnsNullForUnknownId()
        {
            var service = CreateService();
            var result = service.Update(Guid.NewGuid(), new UpdateTaskRequest { Title = "Missing" });
            Assert.Null(result);
        }

        [Fact]
        public void Delete_RemovesTask()
        {
            var service = CreateService();
            var created = service.Create(new CreateTaskRequest { Title = "Delete me" });
            Assert.True(service.Delete(created.Id));
            Assert.Null(service.GetById(created.Id));
        }
    }
}