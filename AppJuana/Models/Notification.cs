namespace AppJuana.Models
{
    public class Notification
    {
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public Color IconBackgroundColor { get; set; } = Colors.Transparent;
    }}
