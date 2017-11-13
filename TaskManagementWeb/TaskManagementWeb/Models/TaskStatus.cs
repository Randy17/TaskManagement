using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models
{
    public enum TaskStatus
    {
        [Description("Назначена")]
        Assigned = 1,
        [Description("Выполняется")]
        Executing = 2,
        [Description("Приостановлена")]
        Suspended = 3,
        [Description("Завершена")]
        Completed = 4
    }
}
