using Library.Interfaces;
using Library.Models;

namespace Library.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync() => await _repository.GetAllTasksAsync();

        public async Task<TaskItem> GetTaskByIdAsync(int id) => await _repository.GetTaskByIdAsync(id);

        public async Task AddTaskAsync(TaskItem task) => await _repository.AddTaskAsync(task);

        public async Task UpdateTaskAsync(TaskItem task) => await _repository.UpdateTaskAsync(task);

        public async Task DeleteTaskAsync(int id) => await _repository.DeleteTaskAsync(id);
    }

}
