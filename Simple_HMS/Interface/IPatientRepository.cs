using System.Collections.Generic;

namespace Simple_HMS.Interface
{
    public interface IPatientRepository
    {
        IEnumerable<IPatient> GetPatient(string regno);
        IEnumerable<IMedRecords> GetAllPatientsMedRecords(string regno);
        IEnumerable<IMedRecords> GetMedRecordRange(string regno, string start_date, string end_date);
        IRecordSummary GetMedRecordSummaryRange(string regno, string start_date, string end_date);
        IEnumerable<IPatient> SearchPatient(string term_1, string term_2);
        int? GetPatientCount();
        void AddPatient(IPatient patient);
        void AddMedRecord(IMedRecords medRecord);
    }
}
