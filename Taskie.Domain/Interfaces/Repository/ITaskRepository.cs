using System.Collections.Generic;
using System.Threading.Tasks;
using Taskie.Domain.Entities;

namespace Taskie.Domain.Interfaces.Repository
{
    public interface ITaskRepository
    {
        Task<TaskEntity> CreateAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task<bool> DeleteAsync(int id);
        Task<TaskEntity> GetByIdAsync(int id);
        Task<IEnumerable<TaskEntity>> GetAllByUserAsync(string idUser);
        Task<IEnumerable<TaskEntity>> GetAllPendingByUserAsync(string idUser);
        Task<IEnumerable<TaskEntity>> GetByPriorityAsync(string idUser, int priority);
        Task<IEnumerable<TaskEntity>> GetByFinishAsync(string idUser);
        Task<IEnumerable<TaskEntity>> GetByFinishedInTimeAsync(string idUser);
        Task<int> GetFinishedInTimeByPriorityAsync(string idUser, int priority);
    }
}
