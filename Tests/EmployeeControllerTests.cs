using CompanyApi.Controllers;
using CompanyApi.Models;
using NUnit.Framework;

namespace CompanyApi.Tests;

[TestFixture]
public class EmployeeControllerTests
{
    [Test]
    public void Compare_Parsed_Objects_For_Employee()
    {
        var controller = new EmployeeController(null, null);
        var status = new EmployeeStatusDB("Former Employee");
        var title = new JobTitleDB("HR Partner");
        var employeeDB = new EmployeeDB(
            name: "test",
            birthdate: new DateTime(2020, 1, 1),
            status: status,
            jobTitle: title
        );
        var employee = controller.ConvertFromDatabase(employeeDB);
        Assert.That(employeeDB.Name, Is.EqualTo(employee.Name));
    }

}