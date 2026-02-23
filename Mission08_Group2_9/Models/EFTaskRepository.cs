using SQLitePCL;

namespace Mission08_Group2_9.Models;

public class EFTaskRepository : ITaskRepository
{
    private TaskDbContext _context;
    public EFTaskRepository(TaskDbContext temp)
    {
        _context = temp;
    }
    
    public List<TaskItem> Tasks => _context.Tasks.ToList();
}