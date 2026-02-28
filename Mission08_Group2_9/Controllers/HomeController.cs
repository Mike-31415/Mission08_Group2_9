using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mission08_Group2_9.Models;

namespace Mission08_Group2_9.Controllers;

public class HomeController : Controller
{
    private readonly ITaskRepository _repo;
    private readonly TaskDbContext _context;

    public HomeController(ITaskRepository temp, TaskDbContext context)
    {
        _repo = temp;
        _context = context;
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

    // Quadrants View - displays tasks organized by quadrant
    public IActionResult Quadrants()
    {
        // Get only incomplete tasks
        var tasks = _repo.Tasks.Where(t => t.Completed == false || t.Completed == null).ToList();
        return View(tasks);
    }

    // GET: Edit task
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var task = _repo.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }
        ViewBag.Categories = _repo.Categories;
        return View(task);
    }

    // POST: Edit task
    [HttpPost]
    public IActionResult Edit(int TaskId, string Task, DateTime? DueDate, int Quadrant, int? CategoryId, bool? Completed)
    {
        var task = new TaskItem
        {
            TaskId = TaskId,
            Task = Task,
            DueDate = DueDate,
            Quadrant = Quadrant,
            CategoryId = CategoryId,
            Completed = Completed
        };

        if (ModelState.IsValid)
        {
            _repo.UpdateTask(task);
            return RedirectToAction("Quadrants");
        }

        // Repopulate ViewBag.Categories for the dropdown if validation fails
        ViewBag.Categories = _repo.Categories;
        return View(task);
    }

    // GET: Delete confirmation
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var task = _repo.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    // POST: Delete task
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _repo.DeleteTask(id);
        return RedirectToAction("Quadrants");
    }

    // POST: Mark task as completed
    [HttpPost]
    public IActionResult MarkCompleted(int id)
    {
        var task = _repo.GetTaskById(id);
        if (task != null)
        {
            task.Completed = true;
            _repo.UpdateTask(task);
        }
        return RedirectToAction("Quadrants");
    }

    public IActionResult Privacy()
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