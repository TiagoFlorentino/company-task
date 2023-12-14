using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<JobTitleDB> JobTitles { get; set; }
    public DbSet<EmployeeStatusDB> EmployeeStatus { get; set; }
    public DbSet<EmployeeDB> Employees { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
