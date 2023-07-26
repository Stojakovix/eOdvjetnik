using CommunityToolkit.Mvvm.Messaging.Messages;

public class ContactEdited : ValueChangedMessage<string>
{
    public ContactEdited(string value) : base(value)
    {
    }
}