using CommunityToolkit.Mvvm.Messaging.Messages;

public class NoSQLDetected : ValueChangedMessage<string>
{
    public NoSQLDetected(string value) : base(value)
    {
    }
}