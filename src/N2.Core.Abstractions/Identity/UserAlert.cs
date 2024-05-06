
namespace N2.Core.Identity;

public class UserAlert
{
    public UserAlert()
    {
        Time = DateTime.UtcNow;
    }

    public UserAlert(string message, Priority priority)
    {
        Time = DateTime.UtcNow;
        Message = message;
        Priority = priority;
    }

    public DateTime Time { get; set; }
    public string Message { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Low;
}