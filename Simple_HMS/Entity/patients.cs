using Simple_HMS.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Simple_HMS.Entity
{
    public class Patients : IPatient
    {
        [Required, Display(Name = "Patients Regno")]
        public string patients_regno { get; set; }
        [Required, Display(Name = "First Name")]
        public string first_name { get; set; }
        [Required, Display(Name = "Last Name")]
        public string last_name { get; set; }
        [Required, Display(Name = "Date of Birth")]
        public DateTime dob { get; set; }
        [Display(Name = "Date Registed")]
        public DateTime reg_date { get; set; }
    }
}