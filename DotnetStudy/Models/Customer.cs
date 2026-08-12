namespace DotnetStudy.Models;

public record Customer(
    int Id,
    string Name,
    string Email,
    string Department,
    DateTime CreatedAt
);
