using CommunityToolkit.Mvvm.Messaging.Messages;

public class RestartNaplata : ValueChangedMessage<string>
{
    public RestartNaplata(string value) : base(value)
    {
    }
}