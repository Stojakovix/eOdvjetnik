using Color = Microsoft.Maui.Graphics.Color;

namespace eOdvjetnik.Models
{
    public class Appointment
    {
        
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string Subject { get; set; }
        public string DescriptionNotes { get; set; }
        public string CategoryColor { get; set; }
        public Brush Background { get; set; }
    }
}