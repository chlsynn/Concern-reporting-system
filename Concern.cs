namespace ConcernReportingSystem;

public class Concern
{
    public int Id { get; set; }
    public string ReporterName { get; set; } = "";
    public string Category { get; set; } = "";
    public string Location { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Pending";
    public string AdminRemarks { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}