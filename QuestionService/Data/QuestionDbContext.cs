using Microsoft.EntityFrameworkCore;
using QuestionService.Entities;

namespace QuestionService.Data;

public class QuestionDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Tag> Tags => Set<Tag>();
}