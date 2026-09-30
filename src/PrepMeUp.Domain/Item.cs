namespace PrepMeUp.Domain;

public class Item
{
    public Guid Id { get; init; }   // Guid (Globally Unique Identifier) is a built-in .NET type.
                                    // standard way to generate a unique ID for an entity
    public required string Name { get; init; }
}