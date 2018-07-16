using System;

namespace Simple_HMS.Interface
{
    public interface IMedRecords
    {
         int Id { get; set; }
         string patients_regno { get; set; }
         DateTime entry_date { get; set; }
         decimal amount { get; set; }
         string treatment { get; set; }
    }
}
