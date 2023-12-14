using CompanyApi.Exceptions;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(ILogger<EmployeeController> logger, AppDbContext contex) : ControllerBase
{
    private readonly AppDbContext _context = contex;
    private readonly ILogger<EmployeeController> _logger = logger;

    internal Employee ConvertFromDatabase(EmployeeDB employeeDb)
    {
        return new Employee(
            name: employeeDb.Name,
            birthdate: employeeDb.Birthdate,
            status: employeeDb.StatusDb.Name,
            jobTitle: employeeDb.JobTitle.Description
        );
    }
    
    [HttpGet("Get")]
    public IActionResult Get(string name)
    {
        try
        {
            // Collect employee by name from the internal function
            return Ok(GetEmployee(name));
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (GenericException e)
        {
            Console.WriteLine(e);
            return ValidationProblem(e.Message);
        }
    }

    private Employee GetEmployee(string name)
    {
        try
        {
            var employee = _context.Employees.FirstOrDefault(
                it => it.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
            );
            if (employee != null)
            {
                return ConvertFromDatabase(employee);
            }
            else
            {
                throw new NotFoundException("No employee found!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new GenericException(e.Message);
        }
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
        var status = new EmployeeStatusDB("None");
        var title = new JobTitleDB("None");
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
        
        var emp = new EmployeeDB(name, birthday, status, title); 
        return ConvertFromDatabase(emp);
    }
    
    [HttpGet("GetAll")]
    public IEnumerable<Employee> GetAllEmployees()
    {
        var status = new EmployeeStatusDB("abc");
        var title = new JobTitleDB("abc");
        return Enumerable.Range(1, 5).Select(index => new Employee("tiago", DateTime.Now, status.Name, title.Description)).ToArray();
    }
    
    [HttpPatch("Update")]
    public Employee UpdateEmployee()
    {
        var status = new EmployeeStatusDB("abc");
        var title = new JobTitleDB("abc");
        var emp = new EmployeeDB("tiago", DateTime.Now, status, title); 
        return ConvertFromDatabase(emp);
    }
    
    [HttpDelete("Delete")]
    public bool DeleteEmployee()
    {
        return true;
    }
}