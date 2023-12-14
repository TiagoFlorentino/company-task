using CompanyApi.Models;

Employee GetEmployee(string name)
{
    var status = new EmployeeStatus(1, "abc");
    var title = new JobTitle(1, "abc");
    var emp = new Employee(1, "tiago", DateTime.Now, status, title); 
    if (emp.Name != "tiago")
    {
        return emp;
    }
    // var users = _context.Users.ToList();
    return emp;
}