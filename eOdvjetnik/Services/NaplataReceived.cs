using CommunityToolkit.Mvvm.Messaging.Messages;

public class NaplataReceived : ValueChangedMessage<string>
{
    public NaplataReceived(string value) : base(value)
    {
    }
}