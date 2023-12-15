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
        var employee = new Employee(
            name: employeeDb.Name,
            birthdate: employeeDb.Birthdate,
            status: employeeDb.StatusDb.Name,
            jobTitle: employeeDb.JobTitle.Description
        );
        return employee;
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
            // Collect the first employee or null
            var employee = _context.Employees.FirstOrDefault(
                it => it.Name.Equals(name)
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
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        return Ok(employee);
    }
    
    
    private Employee CreateEmployee(string name, int year, int month, int day)
    {
        EmployeeStatusDB? initialStatus;
        JobTitleDB? initialTitle;
        try
        {
            initialTitle = _context.JobTitles.FirstOrDefault(
                it => it.Description.Equals("Undefined")
            );
            initialStatus = _context.EmployeeStatus.FirstOrDefault(
                it => it.Name.Equals("Undefined")
            );
            
        }
        catch (Exception e)
        {
            // This will most likely never happen unless there's an issue with the connection to the DB
            // or the DB was not populated
            throw new NotFoundException("Error collecting initial tiles or status! Verify this issue with the owner of the platform!");
        }

        if (initialStatus == null || initialTitle == null)
        {
            throw new NotFoundException("Failed to collect initial tiles or status! Verify this issue with the owner of the platform!");
        }
        
        DateTime birthday;
        try
        {
            birthday = new DateTime(year, month, day);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidParameterException("Invalid parameter - Date is not valid");
        }
        
        var newEmployee = new EmployeeDB(name, birthday, initialStatus, initialTitle); 
        _context.Employees.Add(newEmployee);
        _context.SaveChanges();
        return ConvertFromDatabase(newEmployee);
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAllEmployees()
    {
        try
        {
            // Using eager loading to ensure that the related entities are loaded along with the main entities.
            var employeeDbs = _context.Employees
                .Include(e => e.JobTitle)
                .Include(a => a.StatusDb)
                .ToArray();
            List<Employee> employeeList = new List<Employee>();
            foreach (EmployeeDB employee in employeeDbs)
            {
                employeeList.Add(ConvertFromDatabase(employee));
            }
            return Ok(employeeList);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Failed to collect all employees");
        }
    }
    
    [HttpDelete("Delete")]
    public IActionResult DeleteEmployee(string name)
    {
        var employee = _context.Employees.FirstOrDefault(
            it => it.Name.Equals(name)
        );
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok("Employee deleted");
        }
        else
        {
            return NotFound("Employee not found");
        }
    }
    
    [HttpPatch("Update")]
    public Employee UpdateEmployee()
    {
        var status = new EmployeeStatusDB("abc");
        var title = new JobTitleDB("abc");
        var emp = new EmployeeDB("tiago", DateTime.Now, status, title); 
        return ConvertFromDatabase(emp);
    }
    
    
    [HttpGet("GenerateData")]
    public void GenerateData()
    {
        string[] titles = {"Developer", "PM", "HR Partner", "CEO", "Ex-Employee", "No relation", "Undefined"};
        foreach (string title in titles) 
        {
            _context.JobTitles.Add(new JobTitleDB(title));
        }
        _context.SaveChanges();
        
        string[] statusList = {"Working", "Vacation", "Ex-Employee", "No relation", "Undefined"};
        foreach (string status in statusList) 
        {
            _context.EmployeeStatus.Add(new EmployeeStatusDB(status));
        }
        _context.SaveChanges();
    }
}