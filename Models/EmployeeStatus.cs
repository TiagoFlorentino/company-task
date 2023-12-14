using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class EmployeeStatus
{
    public EmployeeStatus(long statusId, string name)
    {
        StatusId = statusId;
        Name = name;
    }
    
    public EmployeeStatus()
    {
    }

    [Key]
    public long StatusId { get; set; }
    public string Name { get; set; }
}