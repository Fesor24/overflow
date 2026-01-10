using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionService.Data;
using QuestionService.Dtos;
using QuestionService.Entities;

namespace QuestionService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(QuestionDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Question>> Create(CreateQuestionRequest request)
    {
        var validTags = await dbContext.Tags
            .Where(x => request.Tags.Contains(x.Slug))
            .ToListAsync();

        var missing = request.Tags
            .Except(validTags.ConvertAll(x => x.Slug))
            .ToList();

        if (missing.Count != 0) 
            return BadRequest($"Invalid Tags: {string.Join(',', missing)}");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var name = User.FindFirstValue("name");

        if (userId is null || name is null)
            return BadRequest("Can not retrive user information");

        var question = new Question
        {
            Title = request.Title,
            Content = request.Content,
            AskerDisplayName = name,
            AskerId = userId,
        };

        await dbContext.Questions.AddAsync(question);

        await dbContext.SaveChangesAsync();

        return Created($"/questions/{question.Id}", question);

    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<Question>> GetQuestion(string id)
    {
        var question = await dbContext.Questions.FindAsync(id);

        if (question is null) return NotFound("Question not found");

        await dbContext.Questions
            .ExecuteUpdateAsync(setter =>
            setter.SetProperty(prop => prop.ViewCount, prop => prop.ViewCount + 1));

        return question;
    }

    [HttpDelete("/{id}")]
    public async Task<ActionResult> DeleteQuestion(string id)
    {
        var question = await dbContext.Questions.FindAsync(id);

        if (question is null) return NotFound("Question not found");

        dbContext.Questions.Remove(question);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
