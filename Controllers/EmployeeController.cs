using CompanyApi.Exceptions;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace CompanyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(ILogger<EmployeeController> logger, AppDbContext contex) : ControllerBase
{
    private readonly AppDbContext _context = contex;
    private readonly ILogger<EmployeeController> _logger = logger;
    
    [HttpGet("Get")]
    public IActionResult Get(string name)
    {
        // Collect employee by name from the internal function
        Employee employee;
        try
        {
            employee = GetEmployee(name);
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        return Ok(employee);
    }

    private Employee GetEmployee(string name)
    {
        var status = new EmployeeStatusDB(1, "abc");
        var title = new JobTitleDB(1, "abc");
        var emp = new EmployeeDB(1, name, DateTime.Now, status, title); 
        if (emp.Name == "tiago")
        {
            return ConvertFromDatabase(emp);
        }

        var users = _context.Employees.ToArray();
        throw new NotFoundException("User not found!");
    }

    static Employee ConvertFromDatabase(EmployeeDB employeeDb)
    {
        return new Employee(
            name: employeeDb.Name,
            birthdate: employeeDb.Birthdate,
            status: employeeDb.StatusDb.Name,
            jobTitle: employeeDb.JobTitle.Description
        );
    }
    
    [HttpPost("Create")]
    public IActionResult Create(string name, int year, int month, int day)
    {
        // Collect employee by name from the internal function
        Employee employee;
        try
        {
            employee = CreateEmployee(name, year, month, day);
        }
        catch (InvalidParameterException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        return Ok(employee);
    }
    
    
    private Employee CreateEmployee(string name, int year, int month, int day)
    {
        var status = new EmployeeStatusDB(1, "None");
        var title = new JobTitleDB(1, "None");
        DateTime birthday;
        try
        {
            birthday = new DateTime(year, month, day);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidParameterException("Invalid parameter");
        }
        
        var emp = new EmployeeDB(1, name, birthday, status, title); 
        return ConvertFromDatabase(emp);
    }
    
    [HttpGet("GetAll")]
    public IEnumerable<Employee> GetAllEmployees()
    {
        var status = new EmployeeStatusDB(1, "abc");
        var title = new JobTitleDB(1, "abc");
        return Enumerable.Range(1, 5).Select(index => new Employee("tiago", DateTime.Now, status.Name, title.Description)).ToArray();
    }
    
    [HttpPatch("Update")]
    public Employee UpdateEmployee()
    {
        var status = new EmployeeStatusDB(1, "abc");
        var title = new JobTitleDB(1, "abc");
        var emp = new EmployeeDB(1, "tiago", DateTime.Now, status, title); 
        return ConvertFromDatabase(emp);
    }
    
    [HttpDelete("Delete")]
    public bool DeleteEmployee()
    {
        return true;
    }
}