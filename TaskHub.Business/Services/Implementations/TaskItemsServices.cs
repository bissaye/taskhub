using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Data;
using TaskHub.Data.Models.Custum;
using TaskHub.Data.Models.DAO;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.Services.Implementations
{
    public class TaskItemsServices: ITaskItemsServices
    {
        private readonly IGateway _gateway;

        public TaskItemsServices(IGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TaskDataRes> getTaskById(Guid taskItemId)
        {
            TaskItem taskItem = await _gateway.TaskItemRepository().getTaskItemById(taskItemId);
            return taskToTaskDataRes(taskItem);
        }

        public void deleteTaskItem(Guid taskItemId)
        {
            _gateway.TaskItemRepository().DeleteTaskItem(taskItemId);
        }

        public async Task<TaskDataRes> createTaskItem(Guid userId, TaskReq taskCreateReq)
        {
            TaskItem taskItem = taskReqToTask(taskCreateReq);
            taskItem = await _gateway.TaskItemRepository().createTaskItem(userId, taskItem);
            return taskToTaskDataRes(taskItem);
        }

        public Paginate<TaskDataRes> getAllUserTaskItem(Guid userId, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1)
        {
            Paginate<TaskItem> taskItems = _gateway.TaskItemRepository().getAllUserTaskItems(userId, sortBy, dueDate, status, priority, count, page);

            Paginate<TaskDataRes> taskDatRes = new Paginate<TaskDataRes>()
            {
                datas = new List<TaskDataRes> { },
                count = taskItems.count,
                page = taskItems.page,
                total = taskItems.total
            };

            foreach (TaskItem taskItem in taskItems.datas)
            {
                taskDatRes.datas.Add(taskToTaskDataRes(taskItem));
            }

            return taskDatRes;

        }

        public async Task<TaskDataRes> updateTaskIem(Guid taskItemId, TaskReq taskReq)
        {
            TaskItem taskItem = taskReqToTask(taskReq);
            taskItem = await _gateway.TaskItemRepository().updateTaskItem(taskItemId, taskItem);
            return taskToTaskDataRes(taskItem);
        }

        public async Task<TaskDataRes> updateTaskIemStatus(Guid taskItemId, TaskStatus status)
        {
            TaskItem taskItem = await _gateway.TaskItemRepository().updateTaskItemStatus(taskItemId, status);
            return taskToTaskDataRes(taskItem);
        }

        public async Task<TaskDataRes> updateTaskIemPriority(Guid taskItemId, int priority)
        {
            TaskItem taskItem = await _gateway.TaskItemRepository().updateTaskItemPriority(taskItemId, priority);
            return taskToTaskDataRes(taskItem);
        }

        public TaskDataRes taskToTaskDataRes(TaskItem taskItem)
        {
            return new TaskDataRes()
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Priority = taskItem.Priority,
                Status = taskItem.Status,
                Created_at = taskItem.Created_at,
                Updated_at = taskItem.Updated_at,
                UserId = taskItem.UserId
            };
        }

        public TaskItem taskReqToTask(TaskReq taskCreateReq)
        {
            return new TaskItem()
            {
                Title = taskCreateReq.Title,
                Description = taskCreateReq.Description,
                DueDate = taskCreateReq.DueDate,
                Priority = taskCreateReq.Priority,
                Status = taskCreateReq.Status,
            };
        }
    }
}
