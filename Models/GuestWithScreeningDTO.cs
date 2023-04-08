namespace hwSDI.Models
{
    public class GuestWithScreeningDTO
    {
        public int GuestID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }

        public ICollection<Screening> Screenings { get; set; }  
    }
}
