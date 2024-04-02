using System.ComponentModel.DataAnnotations;
using TaskManagement.WebAPI.Helper;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class AddTaskRequest
    {
        [Required]
        [ValidateInetger]
        public int taskId { get; set; }
        
        [Required]
        [NumberMaxLength(100)]
        public string taskName { get; set; }
        
        [Required]
        public string taskDesc { get; set; }
        
        [Required]
        [NumberMaxLength(20)]
        public string status { get; set; }
        [Required]
        public string dueDate { get; set; }
        public string username { get; set; }
        
        [Required]
        [NumberMaxLength(20)]
        public string priority { get; set; }
    }
}
