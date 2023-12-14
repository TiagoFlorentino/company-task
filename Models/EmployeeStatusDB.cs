using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class EmployeeStatusDB
{
    public EmployeeStatusDB(string name)
    {
        Name = name;
    }
    
    public EmployeeStatusDB()
    {
    }


    [Key]
    public long StatusId { get; set; }
    public string Name { get; set; }
}