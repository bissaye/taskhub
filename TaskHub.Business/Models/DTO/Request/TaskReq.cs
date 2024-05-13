using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.Models.DTO.Request
{
    public class TaskReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
    }

    public class TaskUpdateStatusReq
    {
        public TaskStatus Status { get; set; }
    }

    public class TaskUpdatePriorityReq
    {
        public int priority { get; set; }
    }
}
