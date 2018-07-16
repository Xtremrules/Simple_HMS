using Simple_HMS.Interface;
using System;

namespace Simple_HMS.Entity
{
    public class Patients : IPatient
    {
        public string patients_regno { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime dob { get; set; }
        public DateTime reg_date { get; set; }
    }
}