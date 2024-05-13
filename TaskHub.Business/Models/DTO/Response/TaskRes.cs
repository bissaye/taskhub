using TaskStatus = TaskHub.Data.Models.DAO.TaskStatus;

namespace TaskHub.Business.Models.DTO.Response
{
    public class TaskDataRes
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public Guid UserId { get; set; }
    }
}
