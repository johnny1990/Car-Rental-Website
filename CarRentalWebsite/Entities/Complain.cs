using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CarRentalWebsite.Entities
{
    public class Complain
    {
        [Key]
        public int Id { get; set; }
        public string Owner { get; set; }

        [Display(Name = "Kit No")]
        public string Kit_Number { get; set; }
        public string Message { get; set; }

        [Display(Name = "Date")]
        public DateTime? Message_Date { get; set; }

    }
}
