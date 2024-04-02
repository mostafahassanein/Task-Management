using System.ComponentModel.DataAnnotations;
using TaskManagement.WebAPI.Helper;

namespace TaskManagement.WebAPI.Models.Requests
{
    public class CreateTokenRequest
    {
        [Required]
        [NumberMaxLength(20)]
        public string username { get; set; }
    }
}
