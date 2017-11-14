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
        private TasksRepository _repository;

        public HomeController(TasksRepository repository)
        {
            _repository = repository;            
        }

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetTreeData()
        {
            List<TaskTreeItemViewModel> taskTreeItems = _repository.GetTaskTreeItems();
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

            await _repository.AddTaskAsync(model);
            return new JsonResult(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveTask(int id)
        {
            int result = await _repository.RemoveTaskAsync(id);

            if (result != 0)
            {
                return new JsonResult(new { Success = true });
            }

            return new JsonResult(new { Success = false });           
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTask(Models.Task model)
        {
            if(!ModelState.IsValid)
            {
                return new JsonResult(new { Success = false });
            }

            if(model.Status == (int)Models.TaskStatus.Completed)
            {
                model.CompleteTimeStamp = DateTime.Now;
            }
            else
            {
                model.CompleteTimeStamp = null;
            }

            Models.Task updatedTask = await _repository.UpdateTaskAsync(model);
            if(updatedTask != null)
            {
                return new JsonResult(new { Success = true });
            }

            return new JsonResult(new { Success = false });            
        }
    }
}
