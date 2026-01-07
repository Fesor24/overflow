using System.ComponentModel.DataAnnotations;

namespace QuestionService.Entities;

public class Tag : BaseEntity
{
    public Tag(){}
    
    public Tag(string id) : base(id)
    {
        
    }
    
    [MaxLength(60)]
    public required string Name { get; set; }
    [MaxLength(60)]
    public required string Slug { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }
}