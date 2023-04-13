using System.ComponentModel.DataAnnotations;

namespace CarRentalWebsite.Models
{
    public class GenerateQRCodeModel
    {
        [Display(Name = "Select car name to generate QR Code!")]
        public string QRCode
        {
            get;
            set;
        }
    }
}
