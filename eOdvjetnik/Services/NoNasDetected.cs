using CommunityToolkit.Mvvm.Messaging.Messages;

public class NoNasDetected : ValueChangedMessage<string>
{
    public NoNasDetected(string value) : base(value)
    {
    }
}