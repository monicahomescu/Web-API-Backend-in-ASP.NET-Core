namespace hwSDI.Models
{
    public class Ticket
    {
        public int ScreeningID { get; set; }
        public int GuestID { get; set; }
        public int Price { get; set; }
        public int NumberOf { get; set; }

        public virtual Screening Screening { get; set; }
        public virtual Guest Guest { get; set; }
    }
}
