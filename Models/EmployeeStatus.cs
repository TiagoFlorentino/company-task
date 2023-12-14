namespace CompanyApi.Models;

public class EmployeeStatus(long statusId, string name)
{
    public long StatusId { get; set; } = statusId;
    public string Name { get; set; } = name;
}