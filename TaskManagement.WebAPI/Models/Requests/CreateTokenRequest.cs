using System.ComponentModel.DataAnnotations;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class CreateTokenRequest
    {
        [Required]
        public string username { get; set; }
    }
}
