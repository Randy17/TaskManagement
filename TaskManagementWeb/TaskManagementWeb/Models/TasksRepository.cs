using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementWeb.Models
{
    public class TasksRepository
    {
        private ApplicationDbContext _context;

        public TasksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task GetTask(int taskID)
        {
            Task dbTask = _context.Tasks.FirstOrDefault(t => t.ID == taskID);
            LoadAllSubTasks(dbTask);            

            return dbTask;
        }
        public void LoadAllSubTasks(Task task)
        {
            _context.Entry(task).Collection(t => t.Subtasks).Load();
            foreach(var st in task.Subtasks)
            {
                LoadAllSubTasks(st);
            }
        }

    }
}
