using CommunityToolkit.Mvvm.Messaging.Messages;

public class CheckLicence : ValueChangedMessage<string>
{
    public CheckLicence(string value) : base(value)
    {
    }
}