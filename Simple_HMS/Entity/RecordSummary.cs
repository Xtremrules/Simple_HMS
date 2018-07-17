using Simple_HMS.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Simple_HMS.Entity
{
    public class RecordSummary : IRecordSummary
    {
        [Display(Name = "Patients Regno")]
        public string patients_regno { get; set; }
        [Display(Name = "Total")]
        public decimal total { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? start_date { get; set; }
        [Display(Name = "End Date")]
        public DateTime? end_date { get; set; }
        public int count { get; set; }
        [Display(Name = "First Name")]
        public string first_name { get; set; }
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
    }
}