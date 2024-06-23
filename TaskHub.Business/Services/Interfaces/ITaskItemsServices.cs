using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Data.Models.Custum;
using TaskHub.Data.Models.DAO;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.Services.Interfaces
{
    public interface ITaskItemsServices
    {
        public Task<TaskDataRes> getTaskById(Guid taskItemId);
        public void deleteTaskItem(Guid taskItemId);
        public Task<TaskDataRes> createTaskItem(Guid userId, TaskReq taskCreateReq);
        public Paginate<TaskDataRes> getAllUserTaskItem(Guid userId, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1);
        public Task<TaskDataRes> updateTaskIem(Guid taskItemId, TaskReq taskReq);
        public Task<TaskDataRes> updateTaskIemStatus(Guid taskItemId, TaskStatus status);
        public Task<TaskDataRes> updateTaskIemPriority(Guid taskItemId, int priority);
        public TaskDataRes taskToTaskDataRes(TaskItem taskItem);
        public TaskItem taskReqToTask(TaskReq taskCreateReq);
    }
}
