using Color = Microsoft.Maui.Graphics.Color;

namespace eOdvjetnik.Models
{
    public class Appointment
    {
        
        public int ID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool AllDay { get; set; }
        public string EventName { get; set; }
        public string DescriptionNotes { get; set; }
        public string CategoryColor { get; set; }
        public Brush Background { get; set; }
    }
}
