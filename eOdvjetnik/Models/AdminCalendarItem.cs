

namespace eOdvjetnik.Models
{
    public class AdminCalendarItem
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartTimeString { get; set; }
        public string StartDateString { get; set; }

        public string EndTimeString { get; set; }

        public string EventName { get; set; }
        public string DescriptionNotes { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }

        public Color Type { get; set; }
    }
}
