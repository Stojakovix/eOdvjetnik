using CommunityToolkit.Mvvm.Messaging.Messages;

public class AppointmentSpisId : ValueChangedMessage<string>
{
    public AppointmentSpisId(string value) : base(value)
    {
    }
}