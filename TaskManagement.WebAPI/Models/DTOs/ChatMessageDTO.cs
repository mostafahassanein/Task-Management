namespace TaskManagement.WebAPI.Models.DTOs
{
    public class ChatMessageDTO
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string Recipient { get; set; }
    }
}
