namespace Library.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; }
}
