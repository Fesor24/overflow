namespace QuestionService.Dtos;

public record CreateQuestionRequest(
    string Title,
    string Content,
    List<string> Tags
    );
