using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models
{
    /// <summary>
    /// Объект дерева для извлесения из БД
    /// </summary>
    public class TaskTreeDbItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
    }
}
