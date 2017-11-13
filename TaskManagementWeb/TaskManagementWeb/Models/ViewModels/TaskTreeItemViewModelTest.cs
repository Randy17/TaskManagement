using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models.ViewModels
{
    public class TaskTreeItemViewModelTest
    {
        public int id { get; set; }
        public string text { get; set; }
        public virtual List<TaskTreeItemViewModelTest> nodes { get; set; }
        public State state { get; set; }
    }

    public class State
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}
