using Dotnet_Identity_Server4_Implimentation.Data;
using Dotnet_Identity_Server4_Implimentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Identity_Server4_Implimentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssuesController : ControllerBase
{
   private readonly IssueDbContext _context;

   public IssuesController(IssueDbContext context)
   
   =>   _context = context;

    [HttpGet]
    [ProducesResponseType(typeof(Issue),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Issue),StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<Issue>> Get()
       => await _context.Issues.ToListAsync();

    [HttpGet("id")]
    [ProducesResponseType(typeof(Issue),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Issue),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var issue = await _context.Issues.FindAsync(id);
        return issue == null ? NotFound() : Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateIssue(Issue issue)
    {
        await _context.Issues.AddAsync(issue);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByIdAsync), new
        {
            id = issue.Id
        }, issue);
    }

}