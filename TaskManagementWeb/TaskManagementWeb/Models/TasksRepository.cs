using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementWeb.Models.ViewModels;

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
            if (dbTask == null)
                return null;
            LoadAllSubTasks(dbTask);            

            return dbTask;
        }
        public List<TaskTreeItemViewModelTest> GetTaskTreeItems()
        {
            var tasksRecords = _context.Tasks
                .Select(task => new TaskTreeDbItem
                {
                    ID = task.ID,
                    Name = task.Name,
                    ParentID = task.Parent == null ? null : (int?)task.Parent.ID
                }).ToList();

            List<TaskTreeItemViewModelTest> taskTreeItems = tasksRecords.Where(t => t.ParentID == null)
                .Select(t => new TaskTreeItemViewModelTest
                {
                    id = t.ID,
                    text = t.Name,
                    nodes = GetTreeSubtasks(t.ID, tasksRecords)
                }).ToList();

            return taskTreeItems;
        }
        public async Task<int> AddTaskAsync(Task task)
        {
            if (task.ID == 0)
            {
                _context.Tasks.Add(task);
            }           
            return await _context.SaveChangesAsync();
        }

        public async Task<Models.Task> UpdateTaskAsync(Task task)
        {
            Task dbTask = GetTask(task.ID);
            if (dbTask == null)
                return null;

            if (task.Status == (int)TaskStatus.Completed && CanSetCompleted(dbTask))
                SetAllSubtasksCompleted(dbTask, (DateTime)task.CompleteTimeStamp);
            else if(task.Status != (int)TaskStatus.Completed)
            {
                dbTask.Status = task.Status;
            }

            dbTask.Name = task.Name;
            dbTask.Description = task.Description;
            dbTask.ActualExecutionTimeHours = task.ActualExecutionTimeHours;
            dbTask.PlannedExecutionTimeHours = task.PlannedExecutionTimeHours;                    
            dbTask.Implementer = task.Implementer;

            await _context.SaveChangesAsync();

            return dbTask;

        }
        public async Task<int> RemoveTaskAsync(int taskId)
        {
            Task dbTask = _context.Tasks.FirstOrDefault(t => t.ID == taskId);
            if (dbTask != null)
            {
                _context.Tasks.Remove(dbTask);                
                return await _context.SaveChangesAsync();
            }
            return 0;
        }


        private void LoadAllSubTasks(Task task)
        {
            _context.Entry(task).Collection(t => t.Subtasks).Load();
            foreach(var st in task.Subtasks)
            {
                LoadAllSubTasks(st);
            }
        }
        private List<TaskTreeItemViewModelTest> GetTreeSubtasks(int parentID, IEnumerable<TaskTreeDbItem> tasksRecords)
        {
            List<TaskTreeItemViewModelTest> subtasks = tasksRecords.Where(t => t.ParentID == parentID)
                .Select(t => new TaskTreeItemViewModelTest
                {
                    id = t.ID,
                    text = t.Name,
                    nodes = GetTreeSubtasks(t.ID, tasksRecords)
                }).ToList();

            if (subtasks.Count == 0)
                subtasks = null;

            return subtasks;
        }

        private bool CanSetCompleted(Task task)
        {
            if(task.Status == (int)TaskStatus.Assigned)
                return false;

            foreach (var st in task.Subtasks)
            {
                if(!CanSetCompleted(st))
                {
                    return false;
                }
            }
            return true;
        }

        private void SetAllSubtasksCompleted(Task task, DateTime completedTimeStamp)
        {
            if (task.Status != (int)TaskStatus.Completed)
            {
                task.Status = (int)TaskStatus.Completed;
                task.CompleteTimeStamp = completedTimeStamp;
            }

            foreach (var st in task.Subtasks)
            {
                SetAllSubtasksCompleted(st, completedTimeStamp);
            }
        }
    }
}
