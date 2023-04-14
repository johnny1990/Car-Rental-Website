using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalWebsite.Entities
{
    public class Vehicle
    {
   
        [Key]
        public int Id { get; set; }
        public string ? Owner { get; set; }

        [Display(Name = "Kit No")]
        public string ? Kit_Nr { get; set; }

        [Display(Name = "License plate")]
        public string ?  Licence_Plate { get; set; }

        [Display(Name = "Vehicle model")]
        public string ? Vehicle_Model { get; set; }

        [Display(Name = "Vehicle type")]
        public string ? Vehicle_Type { get; set; }

        public enum EnumVehicleType { Regular, SUV, Truck  }

        [Display(Name = "Purchase year")]
        public string ? Purchase_Year { get; set; }


        [Display(Name = "Activation date")]
        public DateTime? Activation_Date { get; set; }


        [Display(Name = "Validity")]
        public bool? Validity { get; set; }
    }
}
