using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public int? ParentId { get; set; }
        public Task Parent { get;set; }
        [Required(ErrorMessage ="Введите название задачи")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Укажите список исполнителей")]
        public string Implementer { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public int Status { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Должно быть больше 0")]
        public int PlannedExecutionTimeHours { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Должно быть больше 0")]
        public int? ActualExecutionTimeHours { get; set; }
        public DateTime? CompleteTimeStamp { get; set; }
        [InverseProperty("Parent")]
        public List<Task> Subtasks { get; set; }
        [NotMapped]
        public bool HasSubtasks
        {
            get
            {
                return Subtasks != null && Subtasks.Count > 0;
            }
        }
        [NotMapped]
        public int CalculatedPlannedExecutionTimeHours
        {
            get
            {
                if(Subtasks == null)
                {
                    return PlannedExecutionTimeHours;
                }
                return Subtasks.Sum(st => st.CalculatedPlannedExecutionTimeHours) + PlannedExecutionTimeHours;
            }
        }
        [NotMapped]
        public int CalculatedActualExecutionTimeHours
        {
            get
            {
                if (Subtasks == null)
                {
                    return ActualExecutionTimeHours ?? 0;
                }
                return Subtasks.Sum(st => st.CalculatedActualExecutionTimeHours) + ActualExecutionTimeHours ?? 0;
            }
        }
    }
}
