using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models.ViewModels
{
    /// <summary>
    /// ViewModel объекта дерева задач
    /// </summary>
    public class TaskTreeItemViewModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public virtual List<TaskTreeItemViewModel> nodes { get; set; }
    }
}
