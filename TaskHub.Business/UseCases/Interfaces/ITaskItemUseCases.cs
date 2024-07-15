using System.Security.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Models.Errors;
using TaskHub.Data.Models.Custum;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.UseCases.Interfaces
{
    public interface ITaskItemUseCases
    {
        public Task<CustumHttpResponse> createTaskItem(ClaimsPrincipal User, TaskReq taskReq);
        public Task<CustumHttpResponse> getTaskItemData(ClaimsPrincipal User, Guid taskItemId);
        public Task<CustumHttpResponse> getAllTaskItemData(ClaimsPrincipal User, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1);
        public Task<CustumHttpResponse> updateTaskItem(ClaimsPrincipal User, Guid taskItemId, TaskReq taskReq);
        public Task<CustumHttpResponse> updateTaskItemStatus(ClaimsPrincipal User, Guid taskItemId, TaskStatus status);
        public Task<CustumHttpResponse> updateTaskItemPriority(ClaimsPrincipal User, Guid taskItemId, int priority);
        public Task<CustumHttpResponse> deleteTaskItem(ClaimsPrincipal User, Guid taskItemId);

    }
}
