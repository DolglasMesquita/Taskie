using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taskie.Domain.Dto.Achievement;
using Taskie.Domain.Dto.Task;
using Taskie.Domain.Entities;
using Taskie.Domain.Interfaces.Repository;
using Taskie.Domain.Interfaces.Service;

namespace Taskie.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IAchievementRepository _repositoryAchievement;
        private readonly IAchievementUserRepository _repositoryAchievementUser;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repository, IAchievementRepository repositoryAchievement,
                            IAchievementUserRepository repositoryAchievementUser, IMapper mapper)
        {
            _repository = repository;
            _repositoryAchievement = repositoryAchievement;
            _repositoryAchievementUser = repositoryAchievementUser;
            _mapper = mapper;
        }
        public async Task<TaskDto> GetTaskById(int taskId, string idUser)
        {
            var task = await _repository.GetByIdAsync(taskId);

            if (task.UserId != idUser)
            {
                throw new UnauthorizedAccessException("Esta tarefa não pertence a este usuário");
            }

            return _mapper.Map<TaskDto>(task);
        }
        public async Task<IEnumerable<TaskDto>> GetAllTasks(string idUser)
        {
            var tasks = await _repository.GetAllByUserAsync(idUser);

            if (tasks == null) return null;

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
        public async Task<IEnumerable<TaskDto>> GetAllTasksFinished(string idUser)
        {
            var tasks = await _repository.GetByFinishAsync(idUser);

            if(tasks == null) return null;

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksPending(string idUser)
        {
            var tasks = await _repository.GetAllPendingByUserAsync(idUser);

            if (tasks == null) return null;

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksByPriority(string idUser, int priority)
        {
            var tasks = await _repository.GetByPriorityAsync(idUser, priority);

            if (tasks == null) return null;

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksFinishedInTime(string idUser)
        {
            var tasks = await _repository.GetByFinishedInTimeAsync(idUser);

            if (tasks == null) return null;

            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> CreateTask(TaskCreateDto taskCreate)
        {
            var difference = (taskCreate.Deadline - taskCreate.CreatedAt).Days;

            switch (taskCreate.Priority)
            {
                case Domain.Entities.Enums.PriorityEnum.HighPriority:
                    if (difference > 3) return null;
                    break;
                case Domain.Entities.Enums.PriorityEnum.MediumPriority:
                    if (difference > 5) return null;
                    break;
                default:
                    break;
            }

            var task = _mapper.Map<TaskEntity>(taskCreate);
            
            var resultCreate = await _repository.CreateAsync(task);

            return _mapper.Map<TaskDto>(resultCreate);
        }

        public async Task<bool> DeleteTask(int idTask, string idUser)
        {
            var task = await _repository.GetByIdAsync(idTask);

            if (task.UserId != idUser)
            {
                throw new UnauthorizedAccessException("Esta tarefa não pertence a este usuário");
            }

            var result = await _repository.DeleteAsync(idTask);

            return result;
        }

        public async Task<TaskDto> UpdateTask(TaskUpdateDto taskUpdate)
        {
            var task = await _repository.GetByIdAsync(taskUpdate.Id);

            if (task.UserId != taskUpdate.UserId)
            {
                throw new UnauthorizedAccessException("Esta tarefa não pertence a este usuário");
            }

            task.Title = taskUpdate.Title;
            task.Description = taskUpdate.Description;
            task.UpdatedAt = taskUpdate.UpdatedAt;

            var resultUpdate = await _repository.UpdateAsync(task);

            return _mapper.Map<TaskDto>(resultUpdate);
        }

        public async Task<IEnumerable<AchievementDto>> CompleteTask(int taskId, string idUser)
        {
            var task = await _repository.GetByIdAsync(taskId);

            List<AchievementEntity> achievementsReceived = new();

            if (task.Finished != null)
            {
                throw new InvalidOperationException("Esta tarefa já está finalizada");
            }


            if (task.UserId != idUser)
            {
                throw new UnauthorizedAccessException("Esta tarefa não pertence a este usuário");
            }

            var day = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            task.Finished = day;

            var difference = (day - task.Deadline).Days;

            if (difference > 0)
            {
                task.FinishedInTime = false;
                await _repository.UpdateAsync(task);
                return _mapper.Map<IEnumerable<AchievementDto>>(achievementsReceived);
            }
            else task.FinishedInTime = true;

            await _repository.UpdateAsync(task);

            int Priority1 = await _repository.GetFinishedInTimeByPriorityAsync(idUser, 1);
            int Priority2 = await _repository.GetFinishedInTimeByPriorityAsync(idUser, 2);
            int Priority3 = await _repository.GetFinishedInTimeByPriorityAsync(idUser, 3);

            var notObtained = await _repositoryAchievementUser.GetAllAchievementsNotObtainedByUserIdAsync(idUser);
            var achievements = await _repositoryAchievement.GetAllAsync();
            
            foreach (var achievement in achievements)
            {
                if (Priority1 >= achievement.Priority1 && Priority2 >= achievement.Priority2
                    && Priority3 >= achievement.Priority3)
                {
                    foreach (var item in notObtained)
                    {
                        if (item.Id == achievement.Id)
                        {
                            achievementsReceived.Add(achievement);
                        }
                    }
                }
            }

            AchievementUserEntity achievementUser = new();

            foreach (var achievement in achievementsReceived)
            {
                achievementUser.UserId = idUser;
                achievementUser.AchievementId = achievement.Id;

                await _repositoryAchievementUser.Create(achievementUser);
            }
            

            return _mapper.Map<IEnumerable<AchievementDto>>(achievementsReceived);
        }

    }
}
