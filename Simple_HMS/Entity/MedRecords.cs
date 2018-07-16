using Simple_HMS.Interface;
using System;

namespace Simple_HMS.Entity
{
    public class MedRecords : IMedRecords
    {
        public int Id { get; set; }
        public string patients_regno { get; set; }
        public DateTime entry_date { get; set; }
        public decimal amount { get; set; }
        public string treatment { get; set; }
    }
}