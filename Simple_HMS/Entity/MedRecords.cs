using Simple_HMS.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Simple_HMS.Entity
{
    public class MedRecords : IMedRecords
    {
        public int Id { get; set; }
        [Required, Display(Name = "Patients Regno")]
        public string patients_regno { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime entry_date { get; set; }
        [Required, Display(Name = "Amount")]
        public decimal amount { get; set; }
        [Required, Display(Name = "Treatment")]
        public string treatment { get; set; }
    }
}