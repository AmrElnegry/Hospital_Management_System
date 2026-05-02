using System;

namespace Project_Hospital
{
    public enum AppointmentStatus { Pending, Completed, Cancelled }

    public abstract class Person
    {
        private string name = "";
        private string phone = "";

        public void SetName(string value) { name = value; }
        public void SetPhone(string value) { phone = value; }
        public string GetName() { return name; }
        public string GetPhone() { return phone; }
    }

    public class Patient : Person
    {
        public string PatientID = "";
        public int Age = 0;
        public string Gender = "";
        public string Password = "";

        public void CreatePatient(string id, string name, int age, string gender, string phone, string password)
        {
            PatientID = id;
            Age = age;
            Gender = gender;
            Password = password;
            SetName(name);
            SetPhone(phone);
        }
    }

    public class Doctor : Person
    {
        public string DoctorID = "";
        public string Username = "";
        public string Specialty = "";
        public string Password = "";
        public int Age = 0;
        public int ExperienceYears = 0;
        public int CertificationsCount = 0;

        public void CreateDoctor(string id, string username, string name, string specialty, string phone, string password, int age, int experience, int certifications)
        {
            DoctorID = id;
            Username = username;
            Specialty = specialty;
            Password = password;
            Age = age;
            ExperienceYears = experience;
            CertificationsCount = certifications;
            SetName(name);
            SetPhone(phone);
        }
    }

    public class Appointment
    {
        public string AppointmentID = "";
        public string PatientID = "";
        public string DoctorID = "";
        public DateTime AppointmentDate;
        public AppointmentStatus Status = AppointmentStatus.Pending;
        public string Diagnosis = "";

        public void CreateAppointment(string id, string patientID, string doctorID, DateTime appointmentDate, AppointmentStatus status, string diagnosis)
        {
            AppointmentID = id;
            PatientID = patientID;
            DoctorID = doctorID;
            AppointmentDate = appointmentDate;
            Status = status;
            Diagnosis = diagnosis;
        }
    }
}
