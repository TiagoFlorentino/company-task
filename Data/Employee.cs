namespace CompanyApi.Models;

public class Employee
{
    public Employee(string name, DateTime birthdate, string status, string jobTitle)
    {
        Name = name;
        Birthdate = birthdate;
        Status = status;
        JobTitle = jobTitle;
    }

    public string Name { get; set; } 
    public DateTime Birthdate { get; set; } 
    public string Status { get; set; }
    public string JobTitle { get; set; }
}