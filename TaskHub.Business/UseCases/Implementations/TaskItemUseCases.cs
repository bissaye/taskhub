﻿using Bissaye.JwtAuth.Services;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskHub.Business.Models.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Cache.Services.Interfaces;
using TaskHub.Data.Models.Custum;
using TaskHub.Data.Models.Errors;
using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.UseCases.Implementations
{
    public class TaskItemUseCases : ITaskItemUseCases
    {
        private readonly ITaskItemsServices _taskItemsServices;
        private readonly ITokenServices _tokenService;
        private readonly ILogger _logger;
        private readonly ICacheServices _cacheServices;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public TaskItemUseCases(ITaskItemsServices taskItemsServices, ITokenServices tokenService, ILogger logger)
        {
            _taskItemsServices = taskItemsServices;
            _tokenService = tokenService;
            _logger = logger;
        }


        public TaskItemUseCases(ITaskItemsServices taskItemsServices, ITokenServices tokenService, ILogger logger, ICacheServices cacheServices)
        {
            _taskItemsServices = taskItemsServices;
            _tokenService = tokenService;
            _logger = logger;
            _cacheServices = cacheServices;
        }

        public async Task<CustumHttpResponse<TaskDataRes>> createTaskItem(ClaimsPrincipal User, TaskReq taskReq)
        {
            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Creating task item for {userId}", userId);

            GenericResponse<TaskDataRes> response = CustomHttpErrorNumber<TaskDataRes>.success;

            response.detail = await _taskItemsServices.createTaskItem(userId, taskReq);

            return new CustumHttpResponse<TaskDataRes>(
                content: response,
                statusCode: 201
            );
        }

        public async Task<CustumHttpResponse<TaskDataRes>> getTaskItemData(ClaimsPrincipal User, Guid taskItemId)
        {

            _logger.LogInformation("Fetching task item details for {taskItemId}", taskItemId);

            TaskDataRes taskDataRes;

            string cacheKey = $"Task_{taskItemId}";
            var cacheData = await _cacheServices.GetCachedDateAsync<TaskDataRes>(cacheKey);

            if(cacheData == null)
            {
                taskDataRes = await _taskItemsServices.getTaskById(taskItemId);
            }
            else
            {
                taskDataRes = cacheData;
            }
 
            if (taskDataRes.UserId == _tokenService.GetClaimsValue<AuthClaims>(User).UserId)
            {
                if (cacheData != null) _cacheServices.SetCachedDateAsync<TaskDataRes>(cacheKey, taskDataRes, _cacheDuration);

                GenericResponse<TaskDataRes> response = CustomHttpErrorNumber<TaskDataRes>.success;
                response.detail = taskDataRes;

                return new CustumHttpResponse<TaskDataRes>(
                    content: response,
                    statusCode: 200
                );
            }
            else
            {
                throw new UnauthorizedAccessException($"you do not have access to this task {taskItemId}");
            }

        }

        public async Task<CustumHttpResponse<List<TaskDataRes>>> getAllTaskItemData(ClaimsPrincipal User, string sortBy = "Created_at", DateTime? dueDate = null, TaskStatus? status = null, int? priority = null, int count = 10, int page = 1)
        {

            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            string cacheKey = $"Tasks_{userId}_{sortBy}_{dueDate}_{status}_{priority}_{count}_{page}";
            var cacheData = await _cacheServices.GetCachedDateAsync<Paginate<TaskDataRes>>(cacheKey);

            _logger.LogInformation("Fetching all task items for {userId}", userId);

            Paginate<TaskDataRes> taskDataRes;

            if(cacheData != null)
            {
                taskDataRes = cacheData;
            }
            else
            {
                taskDataRes = _taskItemsServices.getAllUserTaskItem(userId, sortBy, dueDate, status, priority, count, page);
                _cacheServices.SetCachedDateAsync<Paginate<TaskDataRes>>(cacheKey, taskDataRes, _cacheDuration);
            }

            GenericResponse<List<TaskDataRes>> response = CustomHttpErrorNumber<List<TaskDataRes>>.success;

            response.detail = taskDataRes.datas;
            response.page = taskDataRes.page;
            response.count = taskDataRes.count;
            response.total = taskDataRes.total;

            return new CustumHttpResponse<List<TaskDataRes>>(
                content: response,
                statusCode: 200
            );
        }

        public async Task<CustumHttpResponse<TaskDataRes>> updateTaskItem(ClaimsPrincipal User, Guid taskItemId, TaskReq taskReq)
        {
            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Updating task item with ID: {taskItemId} for user ID: {userId}", taskItemId, userId);

            TaskDataRes taskDataRes = await _taskItemsServices.getTaskById(taskItemId);

            if (taskDataRes.UserId == userId)
            {
                GenericResponse<TaskDataRes> response = CustomHttpErrorNumber<TaskDataRes>.success;

                response.detail = await _taskItemsServices.updateTaskIem(taskItemId, taskReq);

                string cacheKey = $"Task_{taskItemId}";
                string AllTasksPattern = $"Tasks_{userId}_*";
                _cacheServices.SetCachedDateAsync<TaskDataRes>(cacheKey, response.detail, _cacheDuration);
                _cacheServices.InvalidateDatasAsync(AllTasksPattern);

                return new CustumHttpResponse<TaskDataRes>(
                    content: response,
                    statusCode: 200
                );
            }
            else
            {
                throw new UnauthorizedAccessException("you do not have access to this task");
            }
        }

        public async Task<CustumHttpResponse<TaskDataRes>> updateTaskItemStatus(ClaimsPrincipal User, Guid taskItemId, TaskStatus status)
        {
            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Updating task item status with ID: {taskItemId} for user ID: {userId}", taskItemId, userId);

            TaskDataRes taskDataRes = await _taskItemsServices.getTaskById(taskItemId);

            if (taskDataRes.UserId == userId)
            {
                GenericResponse<TaskDataRes> response = CustomHttpErrorNumber<TaskDataRes>.success;

                response.detail = await _taskItemsServices.updateTaskIemStatus(taskItemId, status);

                string cacheKey = $"Task_{taskItemId}";
                string AllTasksPattern = $"Tasks_{userId}_*";
                _cacheServices.SetCachedDateAsync<TaskDataRes>(cacheKey, response.detail, _cacheDuration);
                _cacheServices.InvalidateDatasAsync(AllTasksPattern);

                return new CustumHttpResponse<TaskDataRes>(
                    content: response,
                    statusCode: 200
                );
            }
            else
            {
                throw new UnauthorizedAccessException("you do not have access to this task");
            }
        }

        public async Task<CustumHttpResponse<TaskDataRes>> updateTaskItemPriority(ClaimsPrincipal User, Guid taskItemId, int priority)
        {
            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Updating task item priority with ID: {taskItemId} for user ID: {userId}", taskItemId, userId);

            TaskDataRes taskDataRes = await _taskItemsServices.getTaskById(taskItemId);

            if (taskDataRes.UserId == userId)
            {
                GenericResponse<TaskDataRes> response = CustomHttpErrorNumber<TaskDataRes>.success;

                response.detail = await _taskItemsServices.updateTaskIemPriority(taskItemId, priority);

                string cacheKey = $"Task_{taskItemId}";
                string AllTasksPattern = $"Tasks_{userId}_*";
                _cacheServices.SetCachedDateAsync<TaskDataRes>(cacheKey, response.detail, _cacheDuration);
                _cacheServices.InvalidateDatasAsync(AllTasksPattern);

                return new CustumHttpResponse<TaskDataRes>(
                    content: response,
                    statusCode: 200
                );
            }
            else
            {
                throw new UnauthorizedAccessException("you do not have access to this task");
            }
        }

        public async Task<CustumHttpResponse<string>> deleteTaskItem(ClaimsPrincipal User, Guid taskItemId)
        {

            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Deleting task item with ID: {taskItemId} for user ID: {userId}", taskItemId, userId);

            TaskDataRes taskDataRes = await _taskItemsServices.getTaskById(taskItemId);

            if (taskDataRes.UserId == userId)
            {
                GenericResponse<string> response = CustomHttpErrorNumber<string>.success;

                _taskItemsServices.deleteTaskItem(taskItemId);

                string cacheKey = $"Task_{taskItemId}";
                _cacheServices.RemovedCachedDataAsync(cacheKey);

                response.detail = "task Item deleted successfully";

                return new CustumHttpResponse<string>(
                    content: response,
                    statusCode: 200
                );
            }
            else
            {
                throw new UnauthorizedAccessException("you do not have access to this task");
            }
        }
    }
}
