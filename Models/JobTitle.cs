namespace CompanyApi.Models;

public class JobTitle(string description, long jobTitleId)
{
    public long JobTitleId = jobTitleId;
    public string Description = description;
}