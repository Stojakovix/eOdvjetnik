using CommunityToolkit.Mvvm.Messaging.Messages;

public class ContactDeleted : ValueChangedMessage<string>
{
    public ContactDeleted(string value) : base(value)
    {
    }
}