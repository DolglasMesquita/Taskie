using System.Collections.Generic;
using System.Threading.Tasks;
using Taskie.Domain.Dto.Achievement;
using Taskie.Domain.Dto.Task;

namespace Taskie.Domain.Interfaces.Service
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskById(int taskId, string idUser);
        Task<IEnumerable<TaskDto>> GetAllTasks(string idUser);
        Task<IEnumerable<TaskDto>> GetAllTasksByPriority(string idUser, int priority);
        Task<IEnumerable<TaskDto>> GetAllTasksFinished(string idUser);
        Task<IEnumerable<TaskDto>> GetAllTasksPending(string idUser);
        Task<IEnumerable<TaskDto>> GetAllTasksFinishedInTime(string idUser);
        Task<TaskDto> CreateTask(TaskCreateDto taskCreate);
        Task<TaskDto> UpdateTask(TaskUpdateDto taskUpdate);
        Task<IEnumerable<AchievementDto>> CompleteTask(int taskId, string idUser);
        Task<bool> DeleteTask(int idTask, string idUser);
    }
}
