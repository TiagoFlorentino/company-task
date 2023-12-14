using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(ILogger<EmployeeController> logger, AppDbContext contex) : ControllerBase
{
    private readonly AppDbContext _context = contex;
    private readonly ILogger<EmployeeController> _logger = logger;
    
    [HttpGet(Name = "GetEmployee")]
    public Employee Get()
    {
        var status = new EmployeeStatus(1, "abc");
        var title = new JobTitle(1, "abc");
        var emp = new Employee(1, "tiago", DateTime.Now, status, title); 
        var users = _context.Users.ToList();
        return emp;
    }
}