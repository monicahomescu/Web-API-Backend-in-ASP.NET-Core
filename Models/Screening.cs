namespace hwSDI.Models
{
    public class Screening
    {
        public int ScreeningID { get; set; }
        public string? Location { get; set; }
        public int Room { get; set; }
        public int Seats { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public int MovieID { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
