using System;

namespace Simple_HMS.Interface
{
    public interface IRecordSummary
    {
        string patients_regno { get; set; }
        decimal total { get; set; }
        string first_name { get; set; }
        string last_name { get; set; }
        DateTime? start_date { get; set; }
        DateTime? end_date { get; set; }
        int count { get; set; }
    }
}