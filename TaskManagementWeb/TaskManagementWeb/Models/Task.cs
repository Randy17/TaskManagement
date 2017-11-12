using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public Task Parent { get;set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Implementer { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public int Status { get; set; }
        public int PlannedExecutionTimeHours { get; set; }
        public int? ActualExecutionTimeHours { get; set; }
        public DateTime? CompleteTimeStamp { get; set; }
    }
}
