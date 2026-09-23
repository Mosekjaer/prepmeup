namespace PrepMeUp.Application;

// Validator to check, if an ite key even has any content. Returns false if empty.
public static class ItemKeyValidator
{
    public static bool IsValid(string? key) => !string.IsNullOrWhiteSpace(key);
}