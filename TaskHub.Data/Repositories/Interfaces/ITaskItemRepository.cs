using TaskHub.Data.Models.Custum;
using TaskHub.Data.Models.DAO;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Data.Repositories.Interfaces
{
    public interface ITaskItemRepository 
    {
        public Task<TaskItem> createTaskItem(Guid userId, TaskItem task);
        public Task<TaskItem> getTaskItemById(Guid Id);
        public Paginate<TaskItem> getAllUserTaskItems(Guid userId, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1);
        public Task<TaskItem> updateTaskItemStatus(Guid Id, TaskStatus status);
        public Task<TaskItem> updateTaskItemPriority(Guid Id, int priority);
        public Task<TaskItem> updateTaskItem(Guid Id, TaskItem taskItem);
        public void DeleteTaskItem(Guid Id);
    }
}
