using Dotnet_Identity_Server4_Implimentation.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Identity_Server4_Implimentation.Data;

public class IssueDbContext:DbContext
{
    public IssueDbContext(DbContextOptions<IssueDbContext> options) : base(options)
    {
        
    }

    public DbSet<Issue> Issues { get; set; }
}