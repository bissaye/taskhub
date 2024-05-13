using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Data.Models.DAO
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        [Range(1, 10, ErrorMessage = "The priority must be between 1 and 10")]
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } 

    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
