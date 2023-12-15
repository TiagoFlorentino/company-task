namespace CompanyApi.Models;

public class Employee
{
    public Employee(long id, string name, DateTime birthdate, string status, string jobTitle)
    {
        EmployeeId = id;
        Name = name;
        Birthdate = birthdate;
        Status = status;
        JobTitle = jobTitle;
    }
    
    public Employee()
    {
    }

    public long? EmployeeId { get; set; }
    public string? Name { get; set; } 
    public DateTime? Birthdate { get; set; } 
    public string? Status { get; set; }
    public string? JobTitle { get; set; }
}