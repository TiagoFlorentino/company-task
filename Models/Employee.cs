namespace CompanyApi.Models;

public class Employee
{
    public Employee(long employeeId, string name, DateTime birthdate, EmployeeStatus status, JobTitle jobTitle)
    {
        EmployeeId = employeeId;
        Name = name;
        Birthdate = birthdate;
    }
    
    public Employee(long employeeId, string name, DateTime birthdate)
    {
        EmployeeId = employeeId;
        Name = name;
        Birthdate = birthdate;
    }

    public long EmployeeId { get; set; }
    public string Name { get; set; } 
    public DateTime Birthdate { get; set; } 
    public EmployeeStatus Status { get; set; }
    public JobTitle JobTitle { get; set; }
}