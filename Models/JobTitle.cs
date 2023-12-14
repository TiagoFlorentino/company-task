namespace CompanyApi.Models;

public class JobTitle(long jobTitleId, string description)
{
    public long JobTitleId { get; set; } = jobTitleId;
    public string Description { get; set; } = description;
}