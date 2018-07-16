using System;

namespace Simple_HMS.Interface
{
    public interface IPatient
    {
        string patients_regno { get; set; }
        string first_name { get; set; }
        string last_name { get; set; }
        DateTime dob { get; set; }
        DateTime reg_date { get; set; }
    }
}
