using CompanyApi.Controllers;
using CompanyApi.Exceptions;
using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CompanyApi.Tests;

[TestFixture]
public class EmployeeControllerTests
{
    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        return new AppDbContext(options);
    }

    [Test]
    public void Compare_Parsed_Objects_For_Employee()
    {
        var controller = new EmployeeController(null, null);
        var status = new EmployeeStatusDB("Former Employee");
        var title = new JobTitleDB("HR Partner");
        var employeeDb = new EmployeeDB(
            name: "test",
            birthdate: new DateTime(2020, 1, 1),
            status: status,
            jobTitle: title
        );
        var employee = controller.ConvertFromDatabase(employeeDb);
        Assert.That(employeeDb.Name, Is.EqualTo(employee.Name));
    }



    [Test]
    public void Test_OK_Get_Employee()
    {
        var status = new EmployeeStatusDB("Undefined");
        var title = new JobTitleDB("Undefined");
        var employeeDb = new EmployeeDB(
            name: "testName",
            birthdate: new DateTime(2020, 1, 1),
            status: status,
            jobTitle: title
        );
        using (var dbContext = CreateInMemoryDbContext())
        {
            // Initialize data in the in-memory database
            dbContext.EmployeeStatus.Add(status);
            dbContext.JobTitles.Add(title);
            dbContext.Employees.Add(employeeDb);
            dbContext.SaveChanges();
        }

        using (var dbContext = CreateInMemoryDbContext())
        {
            var controller = new EmployeeController(null, dbContext);
            var employee = controller.GetEmployee("testName");
            Assert.That(employeeDb.Name, Is.EqualTo(employee.Name));
        }

    }

    [Test]
    public void Test_NotFound_Get_Employee()
    {
        using (var dbContext = CreateInMemoryDbContext())
        {
            var controller = new EmployeeController(null, dbContext);
            Assert.Throws<NotFoundException>(() => controller.GetEmployee("testName"));
        }
    }
}

