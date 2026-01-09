using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionService.Data;
using QuestionService.Dtos;
using QuestionService.Entities;

namespace QuestionService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController(QuestionDbContext dbContext) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Question>> Create(CreateQuestionRequest request)
    {
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
}
