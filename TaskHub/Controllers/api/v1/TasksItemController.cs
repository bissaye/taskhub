using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.UseCases.Interfaces;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Controllers.api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksItemController : ControllerBase
    {
        private readonly ITaskItemUseCases _taskItemUseCases;

        public TasksItemController(ITaskItemUseCases taskItemUseCases)
        {
            _taskItemUseCases = taskItemUseCases;

        }

        /// <summary>
        ///      add new task item
        /// </summary>
        /// 
        /// <param name="taskReq">task item's data</param>
        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TaskDataRes), 200)]
        public async Task<CustumHttpResponse> addTaskItem([FromBody] TaskReq taskReq)
        {
            return await _taskItemUseCases.createTaskItem(User, taskReq);
        }

        /// <summary>
        ///      get user specific task item data
        /// </summary>
        /// 
        /// <param name="taskItemId">task item Id</param>
        [HttpGet("get/{taskItemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TaskDataRes), 200)]
        public async Task<CustumHttpResponse> getTaskItemData(Guid taskItemId)
        {
            return await _taskItemUseCases.getTaskItemData(User, taskItemId);
        }

        /// <summary>
        ///      get all user task item data
        /// </summary>
        /// 
        /// <param name="sortBy">sort parameter which can be either: duedate, priority, status or null</param>
        /// <param name="dueDate">dueDate filter parameter</param>
        /// <param name="status">Task status filter parameter</param>
        /// <param name="priority">priority filter parameter</param>
        /// <param name="count">number of task item per page</param>
        /// <param name="page">the page number that you want</param>
        [HttpGet("get/all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<TaskDataRes>), 200)]
        public Task<CustumHttpResponse> getAllTaskItemData(string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1)
        {
            return _taskItemUseCases.getAllTaskItemData(User, sortBy, dueDate, status, priority, count, page);
        }

        /// <summary>
        ///     update task item informations
        /// </summary>
        /// 
        /// <param name="taskItemId">task item Id</param>
        /// <param name="taskReq">task item data</param>
        [HttpPut("update/{taskItemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TaskDataRes), 200)]
        public async Task<CustumHttpResponse> updateTaskItemData(Guid taskItemId, [FromBody] TaskReq taskReq)
        {
            return await _taskItemUseCases.updateTaskItem(User, taskItemId, taskReq);
        }

        /// <summary>
        ///     update task item status
        /// </summary>
        /// 
        /// <param name="taskItemId">task item Id</param>
        /// <param name="taskUpdateStatusReq">new status value</param>
        [HttpPut("update/status/{taskItemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TaskDataRes), 200)]
        public async Task<CustumHttpResponse> updateTaskItemStatus(Guid taskItemId, [FromBody] TaskUpdateStatusReq taskUpdateStatusReq)
        {
            return await _taskItemUseCases.updateTaskItemStatus(User, taskItemId, taskUpdateStatusReq.Status);
        }

        /// <summary>
        ///     update task item priority
        /// </summary>
        /// 
        /// <param name="taskItemId">task item Id</param>
        /// <param name="taskUpdatePriorityReq">new priority value</param>
        [HttpPut("update/priority/{taskItemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TaskDataRes), 200)]
        public async Task<CustumHttpResponse> updateTaskItemStatus(Guid taskItemId, [FromBody] TaskUpdatePriorityReq taskUpdatePriorityReq)
        {
            return await _taskItemUseCases.updateTaskItemPriority(User, taskItemId, taskUpdatePriorityReq.priority);
        }

        /// <summary>
        ///     delete task item 
        /// </summary>
        /// 
        /// <param name="taskItemId">task item Id</param>
        [HttpDelete("delete/{taskItemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<CustumHttpResponse> deleteTaskItem(Guid taskItemId)
        {
            return await _taskItemUseCases.deleteTaskItem(User, taskItemId);
        }
    }
}
