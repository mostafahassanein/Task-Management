using System.ComponentModel.DataAnnotations;
using TaskManagement.WebAPI.Helper;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class SendMessageRequest
    {
        [Required]
        [NumberMaxLength(20)]
        public string from { get; set; }
        
        [Required]
        [NumberMaxLength(20)]
        public string to { get; set; }
        
        [Required]
        public string message { get; set; }
    }
}
