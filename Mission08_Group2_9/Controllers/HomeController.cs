using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        return View();
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
    public IActionResult Edit(TaskItem task)
    {
        if (ModelState.IsValid)
        {
            _repo.UpdateTask(task);
            return RedirectToAction("Quadrants");
        }
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
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}