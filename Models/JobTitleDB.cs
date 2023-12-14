using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class JobTitleDB
{
    public JobTitleDB(string description)
    {
        Description = description;
    }
    
    public JobTitleDB()
    {
    }

    
    [Key]
    public long JobTitleId { get; set; } 
    public string Description { get; set; }
}