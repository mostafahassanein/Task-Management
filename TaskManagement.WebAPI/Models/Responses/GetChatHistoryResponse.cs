namespace TaskManagement.WebAPI.Models.Responses
{
    public class GetChatHistoryResponse
    {
        public string user { get; set; }
        public string message { get; set; }
        public string recipient { get; set; }
    }
}
