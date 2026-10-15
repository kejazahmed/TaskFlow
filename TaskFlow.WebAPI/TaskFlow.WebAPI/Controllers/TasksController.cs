using Microsoft.AspNetCore.Mvc;
using TaskFlow.WebAPI.Models;
using TaskFlow.WebAPI.Service;

namespace TaskFlow.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ITaskService service, ILogger<TasksController> logger) : ControllerBase
    { 
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TaskResponse>), StatusCodes.Status200OK)]
        public ActionResult<IReadOnlyList<TaskResponse>> GetAll() => Ok(service.GetAll());

        [HttpGet("{id:guid}")]
        public ActionResult<TaskResponse> GetById(Guid id)
        {
            logger.LogInformation(
                "Retrieving task with ID {TaskId}", id);

            var task = service.GetById(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskResponse> Create([FromBody] CreateTaskRequest request)
        {
            var created = service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<TaskResponse> Update(Guid id, [FromBody] UpdateTaskRequest request)
        {
            var updated = service.Update(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id) => service.Delete(id) ? NoContent() : NotFound();
    }
}
