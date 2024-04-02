using System.ComponentModel.DataAnnotations;
using TaskManagement.WebAPI.Helper;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class GetChatHistoryRequest
    {
        [Required]
        [NumberMaxLength(20)]
        public string recipient { get; set; }
    }
}
