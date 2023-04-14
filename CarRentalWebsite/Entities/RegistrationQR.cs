using System.ComponentModel.DataAnnotations;

namespace CarRentalWebsite.Entities
{
    public class RegistrationQR
    {
        [Key]
        public int Id { get; set; }
        public string? Owner { get; set; }

        [Display(Name = "Generated QR Code")]
        public byte[] Image { get; set; }

        [Display(Name = "Date")]
        public DateTime? Generation_Date { get; set; }

    }
}
