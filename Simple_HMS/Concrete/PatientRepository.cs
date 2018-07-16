using Simple_HMS.Entity;
using Simple_HMS.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Simple_HMS.Concrete
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connString;

        public PatientRepository(string connString)
        {
            _connString = connString;
        }

        public void AddMedRecord(IMedRecords medRecord)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("AddMed_records", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patients_regno", medRecord.patients_regno);
                cmd.Parameters.AddWithValue("@amount", medRecord.amount);
                cmd.Parameters.AddWithValue("@treatment", medRecord.treatment);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void AddPatient(IPatient patient)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("AddPatient", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patients_regno", patient.patients_regno);
                cmd.Parameters.AddWithValue("@first_name", patient.first_name);
                cmd.Parameters.AddWithValue("@last_name", patient.last_name);
                cmd.Parameters.AddWithValue("@dob", patient.dob);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<IMedRecords> GetAllPatientsMedRecords(string regno)
        {
            List<IMedRecords> records = new List<IMedRecords>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("getPatientsAllMedRecord", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patients_regno", regno);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IMedRecords record = new MedRecords();

                    record.Id = Convert.ToInt32(reader["Id"]);
                    record.entry_date = Convert.ToDateTime(reader["entry_date"]);
                    record.amount = Convert.ToDecimal(reader["amount"]);
                    record.patients_regno = reader["patients_regno"].ToString();
                    record.treatment = reader["treatment"].ToString();

                    records.Add(record);
                }
                conn.Close();
            }
            return records;
        }

        public IEnumerable<IMedRecords> GetMedRecordRange(string regno, string start_date, string end_date)
        {
            List<IMedRecords> records = new List<IMedRecords>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("getPatientsMedRecordsByRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patient_regno", regno);
                cmd.Parameters.AddWithValue("@start_date", start_date ?? "");
                cmd.Parameters.AddWithValue("@end_date", end_date ?? "");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IMedRecords record = new MedRecords();

                    record.Id = Convert.ToInt32(reader["Id"]);
                    record.entry_date = Convert.ToDateTime(reader["entry_date"]);
                    record.amount = Convert.ToDecimal(reader["amount"]);
                    record.patients_regno = reader["patients_regno"].ToString();
                    record.treatment = reader["treatment"].ToString();

                    records.Add(record);
                }
                conn.Close();
            }
            return records;
        }

        public IRecordSummary GetMedRecordSummaryRange(string regno, string start_date, string end_date)
        {
            IRecordSummary recordSum = new RecordSummary();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("getPatientsRecordSummaryByRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patient_regno", regno);
                cmd.Parameters.AddWithValue("@start_date", start_date ?? "");
                cmd.Parameters.AddWithValue("@end_date", end_date ?? "");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recordSum.total = Convert.ToDecimal(reader["Total"]);
                    //recordSum.start_date = reader["start_date"] != null ? Convert.IsDBNull // != null ? Convert.ToDateTime(reader["start_date"]) : null;
                    recordSum.start_date = Convert.IsDBNull(reader["start_date"]) ? default(DateTime) : Convert.ToDateTime(reader["start_date"]);
                    recordSum.end_date = Convert.ToDateTime(reader["end_date"]);
                    recordSum.count = Convert.ToInt32(reader["count"]);
                    recordSum.patients_regno = reader["patients_regno"].ToString();
                }
                conn.Close();
            }
            return recordSum;
        }

        public IEnumerable<IPatient> GetPatient(string regno)
        {
            List<IPatient> patients = new List<IPatient>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("getPatientsByRegno", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@patient_regno", regno);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IPatient patient = new Patients();

                    patient.first_name = reader["first_name"].ToString();
                    patient.last_name = reader["last_name"].ToString();
                    patient.dob = Convert.ToDateTime(reader["dob"]);
                    patient.patients_regno = reader["patients_regno"].ToString();
                    patient.reg_date = Convert.ToDateTime(reader["reg_date"]);

                    patients.Add(patient);
                }
                conn.Close();
            }
            return patients;
        }

        public int? GetPatientCount()
        {
            using (var conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "SELECT COUNT(*) AS Count from patients";

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;
                    return Convert.ToInt32(reader["Count"]);
                }
            }
        }

        public IEnumerable<IPatient> SearchPatient(string term_1, string term_2)
        {
            List<IPatient> patients = new List<IPatient>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("searchPatient", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@term_1", term_1 ?? "");
                cmd.Parameters.AddWithValue("@term_2", term_2 ?? "");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IPatient patient = new Patients();

                    patient.first_name = reader["first_name"].ToString();
                    patient.last_name = reader["last_name"].ToString();
                    patient.dob = Convert.ToDateTime(reader["dob"]);
                    patient.patients_regno = reader["patients_regno"].ToString();
                    patient.reg_date = Convert.ToDateTime(reader["reg_date"]);

                    patients.Add(patient);
                }
                conn.Close();
            }
            return patients;
        }
    }
}