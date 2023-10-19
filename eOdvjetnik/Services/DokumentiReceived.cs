using CommunityToolkit.Mvvm.Messaging.Messages;

public class DokumentiReceived : ValueChangedMessage<string>
{
    public DokumentiReceived(string value) : base(value)
    {
    }
}