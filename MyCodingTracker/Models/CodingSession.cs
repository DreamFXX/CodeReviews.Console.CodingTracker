namespace MyCodingTracker.Models
{
    public class CodingSession
    {
        public int Id { get; set; }

        public string Date { get; set; } = String.Empty;

        public string StartTime { get; set; } = String.Empty;

        public string EndTime { get; set; } = String.Empty;

        public string Duration { get; set; } = String.Empty;
    }
}
