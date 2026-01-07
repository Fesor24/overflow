using System.ComponentModel.DataAnnotations;

namespace QuestionService.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString(); 
    }

    public BaseEntity(string id)
    {
        Id = id;
    }

    [MaxLength(36)]
    public string Id { get; private set; }
}