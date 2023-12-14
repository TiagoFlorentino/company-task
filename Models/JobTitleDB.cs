using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class JobTitleDB
{
    public JobTitleDB(long jobTitleId, string description)
    {
        JobTitleId = jobTitleId;
        Description = description;
    }
    
    public JobTitleDB()
    {
    }

    
    [Key]
    public long JobTitleId { get; set; } 
    public string Description { get; set; }
}