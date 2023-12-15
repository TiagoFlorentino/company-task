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
        // Convert Employee DB model to Employee API model
        // Allows us to filters which information can be displayed to the end user
        var employee = new Employee(
            id: employeeDb.EmployeeId,
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
            return StatusCode(200, GetEmployee(name));
        }
        catch (NotFoundException e)
        {
            // Not found
            return StatusCode(404, e.Message);
        }
        catch (GenericException e)
        {
            // Internal Server Error
            return StatusCode(500, e.Message);
        }
    }

    private Employee GetEmployee(string name)
    {
        try
        {
            // Collect the first employee or null
            var employee = _context.Employees
                .Include(e => e.JobTitle)
                .Include(a => a.StatusDb)
                .FirstOrDefault(
                it => it.Name.Equals(name)
            );
            if (employee == null) throw new NotFoundException("No employee found!");
            return ConvertFromDatabase(employee);

        }
        catch (Exception e)
        {
            // Catch any other exception
            throw new GenericException(e.Message);
        }
    }
    
    [HttpPost("Create")]
    public IActionResult Create(string name, int year, int month, int day)
    {
        // Collect employee by name from the internal function
        try
        {
            return Ok( CreateEmployee(name, year, month, day));
        }
        catch (InvalidParameterException e)
        {
            // Bad Request
            return StatusCode(400, e.Message);
        }
        catch (NotFoundException e)
        {
            // Not found
            return StatusCode(404, e.Message);
        }
        catch (GenericException e)
        {
            // Internal Server Error
            return StatusCode(500, e.Message);
        }
    }

    private static DateTime GenerateValidDateTime(int year, int month, int day)
    {
        try
        {
            return new DateTime(year, month, day);
        }
        catch (Exception e)
        {
            // Non valid date
            throw new InvalidParameterException("Invalid parameter - Date is not valid");
        }
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
        if (initialStatus == null || initialTitle == null) throw new NotFoundException("Failed to collect initial tiles or status! Verify this issue with the owner of the platform!");

        try
        {
            var newEmployee = new EmployeeDB(name, GenerateValidDateTime(year, month, day), initialStatus, initialTitle); 
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            var loadEmployee = GetEmployee(name); 
            return loadEmployee;
        }
        catch (Exception e)
        {
            throw new GenericException(e.Message);
        }
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
           return Ok(GetAllEmployees());
        }
        catch (GenericException e)
        {
            // Internal Server Error
            return StatusCode(500, e.Message);
        }
    }

    private List<Employee> GetAllEmployees()
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
            return employeeList;
        }
        catch (Exception e)
        {
            throw new GenericException(e.Message);
        }
    }
    
    [HttpDelete("Delete")]
    public IActionResult Delete(string name)
    {
        try
        {
            DeleteEmployee(name);
            return Ok("Employee Deleted");
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    private void DeleteEmployee(string name)
    {
        var employee = _context.Employees.FirstOrDefault(
            it => it.Name.Equals(name)
        );
        if (employee == null) throw new NotFoundException("Employee not found");
        _context.Employees.Remove(employee);
        _context.SaveChanges();
    }
    
    [HttpPatch("Update")]
    public IActionResult Update([FromBody] Employee employeeEntity)
    {
        try
        {
            UpdateEmployee(employeeEntity);
            return Ok("Employee was updated!");
        }
        catch (InvalidParameterException e)
        {
            // Bad Request
            return StatusCode(400, e.Message);
        }
        catch (NotFoundException e)
        {
            // Not found
            return StatusCode(404, e.Message);
        }
        catch (Exception e)
        {
            // Internal Server Error
            return StatusCode(500, e.Message);
        }
    }
    
    private void UpdateEmployee([FromBody] Employee employeeEntity)
    {
        if (employeeEntity.EmployeeId == null) throw new InvalidParameterException("Missing employee ID");

        var employeeToUpdate = _context.Employees.Find(employeeEntity.EmployeeId);
        if (employeeToUpdate == null) throw new NotFoundException("No employee with that ID available!");

        string[] specialParameters = { "Status", "JobTitle" };
        foreach (var property in employeeEntity.GetType().GetProperties())
        {
            var propertyValue = property.GetValue(employeeEntity);
            // ignore nulls - nothing to update
            if (propertyValue == null || property.Name == "EmployeeId") continue;
            if (specialParameters.Contains(property.Name))
            {
                if (property.Name == "Status")
                {
                    var newStatus = _context.EmployeeStatus.FirstOrDefault(
                        it => it.Name.Equals(employeeEntity.Status)
                    );
                    // Skip on null
                    if (newStatus == null) continue;
                    employeeToUpdate.StatusDb = newStatus;
                }
                else
                {
                    var newTitle = _context.JobTitles.FirstOrDefault(
                        it => it.Description.Equals(employeeEntity.JobTitle)
                    );
                    // Skip on null
                    if (newTitle == null) continue;
                    employeeToUpdate.JobTitle = newTitle;
                }
            }
            else
            {
                // Get the correct property from the DB model given we are parsing the DB models and API models
                var dbProperty = employeeToUpdate.GetType().GetProperties().FirstOrDefault(entity => entity.Name == property.Name);
                // Set its value dynamically
                dbProperty?.SetValue(employeeToUpdate, propertyValue);
            }
        }
        _context.SaveChanges();
    }
    
    
    [HttpGet("GenerateData")]
    public void GenerateData()
    {
        // This endpoint is meant to be use for demo purposes only
        // Tests will not be added given this would generally not be part of the application
        // A separate solution to having this function would be to initialize the parameters on the DB on application startup (if not yet present)
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