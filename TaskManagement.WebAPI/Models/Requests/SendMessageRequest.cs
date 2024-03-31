using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class SendMessageRequest
    {
        [Required]
        public string from { get; set; }
        [Required]
        public string to { get; set; }
        [Required]
        public string message { get; set; }
    }
}
