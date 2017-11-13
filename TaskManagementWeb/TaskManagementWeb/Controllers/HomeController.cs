using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementWeb.Models;
using TaskManagementWeb.Models.ViewModels;

namespace TaskManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private TasksRepository _repository;

        public HomeController(ApplicationDbContext context, TasksRepository repository)
        {
            _context = context;
            _repository = repository;

            
        }

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetTreeData()
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
                    nodes = GetSubtasks(t.ID, tasksRecords)
                }).ToList();

            return Json(taskTreeItems);
        }

        public ActionResult TaskDetails(int taskID)
        {

            Models.Task task = _repository.GetTask(taskID);
            if(task != null)
            {
                return PartialView("TaskDetailsPartial", task);
            }

            return new EmptyResult();
        }

        public ActionResult AddTask(int? parentID)
        {
            Models.Task newTask = new Models.Task();
            if(parentID != null)
            {
                newTask.ParentId = parentID;
            }
            return View("AddTaskPartial", newTask);
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(Models.Task model)
        {
            if (!ModelState.IsValid)
                return View("AddTaskPartial", model);

            model.CreationTimeStamp = DateTime.Now;
            model.Status = (int)Models.TaskStatus.Assigned;

            _context.Tasks.Add(model);
            UpdateParents(model.ParentId, model.PlannedExecutionTimeHours, model.ActualExecutionTimeHours);
            await _context.SaveChangesAsync();


            return new JsonResult(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveTask(int id)
        {
            Models.Task dbTask = _context.Tasks.FirstOrDefault(p => p.ID == id);
            if (dbTask != null)
            {
                _context.Tasks.Remove(dbTask);
                UpdateParents(dbTask.ParentId, -dbTask.PlannedExecutionTimeHours, -dbTask.ActualExecutionTimeHours);
                await _context.SaveChangesAsync();
                return new JsonResult(new { Success = true });
            }
            else
            {
                return new JsonResult(new { Success = false });
            }            
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTask(Models.Task model)
        {
            if(model.Status == (int)Models.TaskStatus.Completed)
            {
                model.CompleteTimeStamp = DateTime.Now;
            }
            else
            {
                model.CompleteTimeStamp = null;
            }


            Models.Task dbTask = _context.Tasks.FirstOrDefault(t => t.ID == model.ID);
            if(dbTask != null)
            {
                int? actualExecutionTimeDiff = 0;
                int plannedExecutionTimeDiff = 0;

                dbTask.Name = model.Name;
                dbTask.Description = model.Description;

                if(dbTask.ActualExecutionTimeHours != null && model.ActualExecutionTimeHours != null)
                {
                    actualExecutionTimeDiff = model.ActualExecutionTimeHours - dbTask.ActualExecutionTimeHours;
                }
                dbTask.ActualExecutionTimeHours = model.ActualExecutionTimeHours;

                plannedExecutionTimeDiff = model.PlannedExecutionTimeHours - dbTask.PlannedExecutionTimeHours;
                dbTask.PlannedExecutionTimeHours = model.PlannedExecutionTimeHours;

                dbTask.CompleteTimeStamp = model.CompleteTimeStamp;
                dbTask.Implementer = model.Implementer;
                dbTask.Status = model.Status;

                UpdateParents(dbTask.ParentId, plannedExecutionTimeDiff, actualExecutionTimeDiff);
                await _context.SaveChangesAsync();
                return new JsonResult(new { Success = true });
            }

            return new JsonResult(new { Success = false });
        }

        private List<TaskTreeItemViewModelTest> GetSubtasks(int parentID, IEnumerable<TaskTreeDbItem> tasksRecords)
        {
            List<TaskTreeItemViewModelTest> subtasks = tasksRecords.Where(t => t.ParentID == parentID)
                .Select(t => new TaskTreeItemViewModelTest
                {
                    id = t.ID,
                    text = t.Name,
                    nodes = GetSubtasks(t.ID, tasksRecords)
                }).ToList();

            if (subtasks.Count == 0)
                subtasks = null;

            return subtasks;
        }

        private void UpdateParents(int? parentID, int plannedExecutionTimeDiff, int? actualExecutionTimeDiff)
        {
            //if (parentID == null)
            //    return;
            //Models.Task dbTask = _context.Tasks.FirstOrDefault(t => t.ID == parentID);
            //if(dbTask != null)
            //{
            //    dbTask.PlannedExecutionTimeHours += plannedExecutionTimeDiff;
            //    if(actualExecutionTimeDiff != null)
            //        dbTask.ActualExecutionTimeHours += actualExecutionTimeDiff;
            //    UpdateParents(dbTask.ParentId, plannedExecutionTimeDiff, actualExecutionTimeDiff);
            //}
        }
    }
}
