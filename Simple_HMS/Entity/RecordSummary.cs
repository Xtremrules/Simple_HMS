using Simple_HMS.Interface;
using System;

namespace Simple_HMS.Entity
{
    public class RecordSummary : IRecordSummary
    {
        public string patients_regno { get; set; }
        public decimal total { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int count { get; set; }
    }
}