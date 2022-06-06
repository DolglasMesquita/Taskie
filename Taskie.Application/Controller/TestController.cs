using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Taskie.Domain.Dto.Task;
using Taskie.Domain.Interfaces.Service;

namespace Taskie.Application.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITaskService _taskService;


        public TestController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("teste")]
        public async Task<IActionResult> CreateTask(TaskCreateDto task)
        {
            try
            {
                var resultCreate = await _taskService.CreateTask(task);

                if (resultCreate == null) return BadRequest("O prazo definido não é válido");

                return Created("Criado com sucesso", resultCreate);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateTask(TaskUpdateDto task)
        {
            try
            {
                var resultUpdate = await _taskService.UpdateTask(task);

                return Ok(resultUpdate);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompelteTask(TaskCompleteDto task)
        {
            try
            {
                var resultComplete = await _taskService.CompleteTask(task.TaskId, task.UserId);

                if (resultComplete.Any()) return Ok(resultComplete);

                return Ok("Tarefa concluída");

            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
