using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetTreeData()
        {
            var tasksRecords = _context.Tasks.Where(task => task.Parent == null)
                .Select(task => new TaskTreeDbItem
                {
                    ID = task.ID,
                    Name = task.Name,
                    ParentID = task.Parent == null ? null : (int?)task.Parent.ID
                }).ToList();

            List<TaskTreeItemViewModel> taskTreeItems = tasksRecords.Where(t => t.ParentID == null)
                .Select(t => new TaskTreeItemViewModel
                {
                    id = t.ID,
                    text = t.Name,
                    children = GetSubtasks(t.ID, tasksRecords)
                }).ToList();

            return Json(taskTreeItems);
        }

        public ActionResult AddTask(int? parentID)
        {
            Models.Task newTask = new Models.Task();
            return View("AddTaskPartial", newTask);
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(Models.Task model)
        {
            if (!ModelState.IsValid)
                return View("AddTaskPartial", model);

            model.CreationTimeStamp = DateTime.Now;

            _context.Tasks.Add(model);
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
                await _context.SaveChangesAsync();
            }

            return new JsonResult(new { Success = true });
        }

        private List<TaskTreeItemViewModel> GetSubtasks(int parentID, IEnumerable<TaskTreeDbItem> tasksRecords)
        {
            List<TaskTreeItemViewModel> subtasks = tasksRecords.Where(t => t.ParentID == parentID)
                .Select(t => new TaskTreeItemViewModel
                {
                    id = t.ID,
                    text = t.Name,
                    children = GetSubtasks(t.ID, tasksRecords)
                }).ToList();

            return subtasks;
        }
    }
}
