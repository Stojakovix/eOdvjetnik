using CommunityToolkit.Mvvm.Messaging.Messages;

public class RefreshContacts : ValueChangedMessage<string>
{
    public RefreshContacts(string value) : base(value)
    {
    }
}