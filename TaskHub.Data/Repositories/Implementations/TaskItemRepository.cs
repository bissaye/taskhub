using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Custum;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;
using TaskHub.Data.Models.Errors;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data.Repositories.Implementations
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly DataContext _dataContext;

        public TaskItemRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<TaskItem> createTaskItem(Guid userId, TaskItem task)
        {
            try
            {
                TaskItem _task = task;
                _task.Created_at = DateTime.Now;
                _task.UserId = userId;
                _dataContext.TaskItems.Add(_task);
                await _dataContext.SaveChangesAsync();

                return _task;
            }
            catch (Exception ex)
            {
                throw new SavingErrorException($"error wile saving task item : {ex.Message}");
            }
        }

        public async Task<TaskItem> getTaskItemById(Guid Id)
        {
            try
            {
                TaskItem? taskItem = await _dataContext.TaskItems.FindAsync(Id);
                if (taskItem == null)
                {
                    throw new NotFoundException("task item not found");
                }
                return taskItem;
            }
            catch (Exception ex) when (!(ex is NotFoundException))
            {
                throw new ReadErrorException($"error while reading task item : {ex.Message}");
            }

        }

        public Paginate<TaskItem> getAllUserTaskItems(Guid userId, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1)
        {
            try
            {
                List<TaskItem> taskItems = _dataContext.TaskItems.
                    Where(taskItem => taskItem.UserId == userId).ToList();

                if (dueDate != null)
                {
                    taskItems = taskItems.FindAll(taskItem => taskItem.DueDate == dueDate);
                }

                if (status != null)
                {
                    taskItems = taskItems.FindAll(taskItems => taskItems.Status == status);
                }

                if (priority != null)
                {
                    taskItems = taskItems.FindAll(taskItems => taskItems.Priority == priority);
                }

                switch (sortBy.ToLower())
                {
                    case "duedate":
                        taskItems = taskItems.OrderByDescending(taskItem => taskItem.DueDate).ToList();
                        break;
                    case "priority":
                        taskItems = taskItems.OrderByDescending(taskItem => taskItem.Priority).ToList();
                        break;
                    case "status":
                        taskItems = taskItems.OrderBy(taskItem => taskItem.Status).ToList();
                        break;
                    case "Created_at":
                        taskItems = taskItems.OrderByDescending(taskItem => taskItem.Created_at).ToList();
                        break;
                    default:
                        taskItems = taskItems.OrderByDescending(taskItem => taskItem.Created_at).ToList();
                        break;
                }

                int total = taskItems.Count();
                int startIndex = (page - 1) * count;
                List<TaskItem> results = taskItems.Skip(startIndex).Take(count).ToList();
                return new Paginate<TaskItem>()
                {
                    datas = results,
                    count = results.Count(),
                    page = page,
                    total = total
                };
            }
            catch (Exception ex)
            {
                throw new ReadErrorException($"Error while reading all data {ex.Message}");
            }
        }
        public async Task<TaskItem> updateTaskItemStatus(Guid Id, TaskStatus status)
        {

            TaskItem _taskItem = await getTaskItemById(Id);
            try
            {
                _dataContext.TaskItems.Attach(_taskItem);

                _taskItem.Status = status;

                await _dataContext.SaveChangesAsync();

                return _taskItem;
            }
            catch (Exception ex)
            {
                throw new UpdateErrorException($"error while updating task item status :{ex.Message}");
            }
        }

        public async Task<TaskItem> updateTaskItemPriority(Guid Id, int priority)
        {

            TaskItem _taskItem = await getTaskItemById(Id);
            try
            {
                _dataContext.TaskItems.Attach(_taskItem);

                _taskItem.Priority = priority;

                await _dataContext.SaveChangesAsync();

                return _taskItem;
            }
            catch (Exception ex)
            {
                throw new UpdateErrorException($"error while updating task item priority :{ex.Message}");
            }
        }


        public async Task<TaskItem> updateTaskItem(Guid Id, TaskItem taskItem)
        {

            TaskItem _taskItem = await getTaskItemById(Id);
            try
            {
                _dataContext.TaskItems.Attach(_taskItem);

                _taskItem.Updated_at = DateTime.Now;
                _taskItem.Title = taskItem.Title;
                _taskItem.Description = taskItem.Description;
                _taskItem.DueDate = taskItem.DueDate;
                _taskItem.Priority = taskItem.Priority;
                _taskItem.Status = taskItem.Status;

                await _dataContext.SaveChangesAsync();

                return _taskItem;
            }
            catch (Exception ex)
            {
                throw new UpdateErrorException($"rror while updating task item :{ex.Message}");
            }
        }

        public async void DeleteTaskItem(Guid Id)
        {
            TaskItem? taskItem = await getTaskItemById(Id);
            if (taskItem == null)
            {
                throw new NotFoundException("these task item dosen't exists");
            }
            else
            {
                try
                {
                    _dataContext.TaskItems.Remove(taskItem);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DeleteErrorException($"error while deleting task item : {ex.Message}");
                }
            }
        }
    }
}
