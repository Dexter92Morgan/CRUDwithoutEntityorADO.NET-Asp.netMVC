using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRUDwithoutEntity.Models
{
    public class PatientModelVeiw
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Patient Number is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Patient phone number")]
        public string PatientNumber { get; set; }

        [Required(ErrorMessage = "Patient Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Patient Email Address")]
        public string PatientEmail { get; set; }


        [Required(ErrorMessage = "Patient Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "BloodGroup is required.")]
        public string BloodGroup { get; set; }
    }
}