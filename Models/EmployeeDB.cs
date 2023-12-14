using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class EmployeeDB
{
    public EmployeeDB(long employeeId, string name, DateTime birthdate, EmployeeStatusDB statusDb, JobTitleDB jobTitle)
    {
        EmployeeId = employeeId;
        Name = name;
        Birthdate = birthdate;
        StatusDb = statusDb;
        JobTitle = jobTitle;
    }
    
    public EmployeeDB()
    {
    }

    [Key]
    public long EmployeeId { get; set; }
    public string Name { get; set; } 
    public DateTime Birthdate { get; set; } 
    public EmployeeStatusDB StatusDb { get; set; }
    public JobTitleDB JobTitle { get; set; }
}