namespace ServiceHub.Day10.Models;

public class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Scheduled";
}
