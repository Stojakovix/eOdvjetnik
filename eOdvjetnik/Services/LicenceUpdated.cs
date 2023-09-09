using CommunityToolkit.Mvvm.Messaging.Messages;

public class LicenceUpdated : ValueChangedMessage<string>
{
    public LicenceUpdated(string value) : base(value)
    {
    }
}