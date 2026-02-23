using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mission08_Group2_9.Models;

public class TaskItem
{
    [Key]
    public int TaskId { get; set; }
    
    [Required]
    public string Task { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    [Range(1,4)]
    public int Quadrant { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    public bool? Completed { get; set; }
}