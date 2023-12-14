using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class JobTitle
{
    public JobTitle(long jobTitleId, string description)
    {
        JobTitleId = jobTitleId;
        Description = description;
    }
    
    public JobTitle()
    {
    }

    
    [Key]
    public long JobTitleId { get; set; } 
    public string Description { get; set; }
}