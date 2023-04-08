namespace hwSDI.Models
{
    public class ScreeningWithGuestDTO
    {
        public int ScreeningID { get; set; }
        public string? Location { get; set; }
        public int Room { get; set; }
        public int Seats { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public MovieDTO Movie { get; set; }

        public ICollection<Guest> Guests { get; set; }
    }
}
