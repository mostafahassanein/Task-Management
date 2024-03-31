using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class AddTaskRequest
    {
        [Required]
        public int taskId { get; set; }
        [Required]
        public string taskName { get; set; }
        [Required]
        public string taskDesc { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public string dueDate { get; set; }
        public string username { get; set; }
        [Required]
        public string priority { get; set; }
    }
}
