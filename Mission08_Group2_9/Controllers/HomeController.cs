using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        return View();
    }

    // Shows all not-completed tasks laid out in quadrants
    public IActionResult Quadrants()
    {
        var tasks = _repo.Tasks
            .Where(t => t.Completed != true)
            .OrderBy(t => t.DueDate)
            .ToList();

        return View(tasks);
    }

    // GET: Add or edit a task
    [HttpGet]
    public IActionResult TaskForm(int? id)
    {
        ViewBag.Categories = _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToList();

        if (id == null)
        {
            return View(new TaskItem());
        }

        var task = _context.Tasks
            .FirstOrDefault(t => t.TaskId == id);

        if (task == null)
        {
            return RedirectToAction("Quadrants");
        }

        return View(task);
    }

    // POST: Save a new or edited task
    [HttpPost]
    public IActionResult TaskForm(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToList();

            return View(task);
        }

        if (task.TaskId == 0)
        {
            _context.Tasks.Add(task);
        }
        else
        {
            _context.Tasks.Update(task);
        }

        _context.SaveChanges();

        return RedirectToAction("Quadrants");
    }

    // POST: Mark a task as completed
    [HttpPost]
    public IActionResult MarkComplete(int id)
    {
        var task = _context.Tasks
            .FirstOrDefault(t => t.TaskId == id);

        if (task != null)
        {
            task.Completed = true;
            _context.SaveChanges();
        }

        return RedirectToAction("Quadrants");
    }

    // POST: Delete a task
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks
            .FirstOrDefault(t => t.TaskId == id);

        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        return RedirectToAction("Quadrants");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}