namespace PrepMeUp.Application;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
