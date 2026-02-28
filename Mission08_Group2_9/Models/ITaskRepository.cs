namespace Mission08_Group2_9.Models;

public interface ITaskRepository
{
    List<TaskItem> Tasks { get; }
    List<Category> Categories { get; }
    void AddTask(TaskItem task);
    void UpdateTask(TaskItem task);
    void DeleteTask(int taskId);
    TaskItem GetTaskById(int taskId);
}