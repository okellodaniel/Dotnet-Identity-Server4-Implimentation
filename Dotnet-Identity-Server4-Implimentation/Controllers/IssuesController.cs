using Dotnet_Identity_Server4_Implimentation.Data;
using Dotnet_Identity_Server4_Implimentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Identity_Server4_Implimentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    public async Task<IActionResult> GetById(Guid id)
    {
        var issue = await _context.Issues.FindAsync(id);
        return issue == null ? NotFound() : Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(Issue),StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CreateIssue(Issue issue)
    {
        await _context.Issues.AddAsync(issue);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new
        {
            id = issue.Id
        }, issue);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Issue), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(Issue), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, Issue issue)
    {
        await _context.Issues.FindAsync(id);

        if (id != issue.Id) return BadRequest();
        
        _context.Entry(issue).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();        
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Issue), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Issue), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
        {
            return NotFound();
        }

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("/identity")]
    public IActionResult GetIdentity()
    {
        return  new JsonResult(from c in User.Claims
            select new
            {
                c.Type,
                c.Value
            });
    }
}