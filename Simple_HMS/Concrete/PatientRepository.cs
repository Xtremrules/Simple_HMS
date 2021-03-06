﻿using Simple_HMS.Entity;
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
                SqlCommand cmd = new SqlCommand("AddMed_records", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
                SqlCommand cmd = new SqlCommand("AddPatient", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
                SqlCommand cmd = new SqlCommand("getPatientsAllMedRecord", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@patients_regno", regno);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IMedRecords record = new MedRecords
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        entry_date = Convert.ToDateTime(reader["entry_date"]),
                        amount = Convert.ToDecimal(reader["amount"]),
                        patients_regno = reader["patients_regno"].ToString(),
                        treatment = reader["treatment"].ToString()
                    };

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
                SqlCommand cmd = new SqlCommand("getPatientsMedRecordsByRange", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@patient_regno", regno);
                cmd.Parameters.AddWithValue("@start_date", start_date ?? "");
                cmd.Parameters.AddWithValue("@end_date", end_date ?? "");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IMedRecords record = new MedRecords
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        entry_date = Convert.ToDateTime(reader["entry_date"]),
                        amount = Convert.ToDecimal(reader["amount"]),
                        patients_regno = reader["patients_regno"].ToString(),
                        treatment = reader["treatment"].ToString()
                    };

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
                SqlCommand cmd = new SqlCommand("getPatientsRecordSummaryByRange", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@patient_regno", regno);
                cmd.Parameters.AddWithValue("@start_date", start_date);
                cmd.Parameters.AddWithValue("@end_date", end_date);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    recordSum.total = Convert.ToDecimal(reader["Total"]);
                    recordSum.end_date = reader["end_date"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(reader["end_date"].ToString());
                    recordSum.start_date = reader["start_date"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(reader["start_date"].ToString());
                    recordSum.count = Convert.ToInt32(reader["count"]);
                    recordSum.patients_regno = reader["patients_regno"].ToString();
                    recordSum.last_name = reader["last_name"].ToString();
                    recordSum.first_name = reader["first_name"].ToString();
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
                SqlCommand cmd = new SqlCommand("getPatientsByRegno", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@patient_regno", regno);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IPatient patient = new Patients
                    {
                        first_name = reader["first_name"].ToString(),
                        last_name = reader["last_name"].ToString(),
                        dob = Convert.ToDateTime(reader["dob"]),
                        patients_regno = reader["patients_regno"].ToString(),
                        reg_date = Convert.ToDateTime(reader["reg_date"])
                    };

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
                SqlCommand cmd = new SqlCommand("searchPatient", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@term_1", term_1);
                if (term_2 == null || string.IsNullOrEmpty(term_2))
                    cmd.Parameters.AddWithValue("@term_2", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@term_2", term_2);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IPatient patient = new Patients
                    {
                        first_name = reader["first_name"].ToString(),
                        last_name = reader["last_name"].ToString(),
                        dob = Convert.ToDateTime(reader["dob"]),
                        patients_regno = reader["patients_regno"].ToString(),
                        reg_date = Convert.ToDateTime(reader["reg_date"])
                    };

                    patients.Add(patient);
                }
                conn.Close();
            }
            return patients;
        }
    }
}