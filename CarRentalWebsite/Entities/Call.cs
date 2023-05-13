using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace CarRentalWebsite.Entities
{
    public class Call
    {
        
        [Key]
        public int Id { get; set; }
        public string OwnerName { get; set; }

        [Display(Name = "Kit No")]
        public string Kit_Number { get; set; }
        public string Phone_Number { get;set; }

        [Display(Name = "Date with time")]
        public DateTime? Date_And_Time { get; set; }
            
            
        [Display(Name = "Call duration")]
        public string Call_Duration { get; set; }

    }
}
