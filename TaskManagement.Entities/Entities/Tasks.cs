using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int TaskStatusId { get; set; }
        public string Status { get; set; }
        //public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        //public virtual User User { get; set; }
        //public int PriorityId { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }

    }
}
