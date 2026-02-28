using Microsoft.EntityFrameworkCore;

namespace Mission08_Group2_9.Models;

public class EFTaskRepository : ITaskRepository
{
    private TaskDbContext _context;
    
    public EFTaskRepository(TaskDbContext temp)
    {
        _context = temp;
    }
    
    public List<TaskItem> Tasks => _context.Tasks.Include(t => t.Category).ToList();
    public List<Category> Categories => _context.Categories.ToList();
    
    public void AddTask(TaskItem task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }
    
    public void UpdateTask(TaskItem task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }
    
    public void DeleteTask(int taskId)
    {
        var task = _context.Tasks.Find(taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
    }
}