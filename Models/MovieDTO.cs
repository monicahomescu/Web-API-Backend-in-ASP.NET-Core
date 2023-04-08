namespace hwSDI.Models
{
    public class MovieDTO
    {
        public int MovieID { get; set; }
        public string? Title { get; set; }
        public int ReleaseYear { get; set; }
        public string? Genre { get; set; }
        public string? Producer { get; set; }
        public int LengthMinutes { get; set; }
    }
}
