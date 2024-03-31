namespace TaskManagement.WebAPI.Models.Responses
{
    public class GetTaskResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string username { get; set; }
        public string priority { get; set; }
        public DateTime dueDate { get; set; }
    }
}
