using CommunityToolkit.Mvvm.Messaging.Messages;

public class MacWorkaround : ValueChangedMessage<string>
{
    public MacWorkaround(string value) : base(value)
    {
    }
}