namespace CompanyApi.Models;

public class Employee(long employeeId, string name,  DateTime birthdate, EmployeeStatus status, JobTitle jobTitle)
{
    public long EmployeeId { get; set; } = employeeId;
    public string Name { get; set; } = name;
    public DateTime Birthdate { get; set; } = birthdate;
    public EmployeeStatus Status { get; set; } = status;
    public JobTitle JobTitle { get; set; } = jobTitle;
}