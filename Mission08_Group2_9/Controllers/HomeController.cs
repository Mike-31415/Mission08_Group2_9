using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mission08_Group2_9.Models;

namespace Mission08_Group2_9.Controllers;

public class HomeController : Controller
{
    private ITaskRepository _repo;

    public HomeController(ITaskRepository temp)
    {
        _repo = temp;
    }

    public IActionResult Index()
    {
        var tasks = _repo.Tasks.Where(t => t.Completed != true).ToList();
        return View(tasks);
    }

    [HttpGet]
    public IActionResult AddEditTask(int? id)
    {
        ViewBag.Categories = new SelectList(_repo.Categories, "CategoryId", "CategoryName");
        
        if (id == null)
            return View(new TaskItem());
        
        var task = _repo.Tasks.FirstOrDefault(t => t.TaskId == id);
        return View(task);
    }

    [HttpPost]
    public IActionResult AddEditTask(TaskItem taskItem)
    {
        if (ModelState.IsValid)
        {
            if (taskItem.TaskId == 0)
                _repo.AddTask(taskItem);
            else
                _repo.UpdateTask(taskItem);
        
            return RedirectToAction("Index");
        }
    
        ViewBag.Categories = new SelectList(_repo.Categories, "CategoryId", "CategoryName");
        return View(taskItem);
    }

    [HttpPost]
    public IActionResult DeleteTask(int taskId)
    {
        _repo.DeleteTask(taskId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MarkComplete(int taskId)
    {
        var task = _repo.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        if (task != null)
        {
            task.Completed = true;
            _repo.UpdateTask(task);
        }
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}