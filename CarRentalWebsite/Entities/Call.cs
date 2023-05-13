using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace CarRentalWebsite.Entities
{
    public class Call
    {
        //(1. Owner Name 2. Kit no 3. Contact Person mobile no 4. date with time 5. Call duration)
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
