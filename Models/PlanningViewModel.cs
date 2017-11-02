using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d > DateTime.Now;

        }
    }
    public class PlanningViewModel : BaseEntity
    {
        [Required]
        [Display(Name = "Bride's Name")]
        public string bride { get; set; }

        [Required]
        [Display(Name = "Groom's Name")]
        public string groom { get; set; }

        [Required]
        [Display(Name = "Venue Address")]
        public string address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "This is not a scrap book. Only future weddings, please.")]
        [Display(Name = "Wedding Date")]
        public DateTime date { get; set; }
    }
}