using System;
using System.IO;

namespace Project_Hospital
{
    static class DB
    {
        public static Doctor[] Doctors = new Doctor[100];
        public static Patient[] Patients = new Patient[200];
        public static Appointment[] Appointments = new Appointment[500];

        public static int DoctorCount = 0;
        public static int PatientCount = 0;
        public static int AppointmentCount = 0;

        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string DoctorsFile = Path.Combine(BasePath, "doctors_db.txt");
        private static readonly string PatientsFile = Path.Combine(BasePath, "patients_db.txt");
        private static readonly string AppointmentsFile = Path.Combine(BasePath, "appointments_db.txt");
        private static readonly string SettingsFile = Path.Combine(BasePath, "system_settings.txt");

        private static readonly Random random = new Random();
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string AdminPassword = "123";

        public static string GenerateMixedID()
        {
            string id;
            do
            {
                id = "";
                for (int i = 0; i < 6; i++)
                    id += chars[random.Next(chars.Length)];
            }
            while (IDExists(id));

            return id;
        }

        static bool IDExists(string id)
        {
            for (int i = 0; i < DoctorCount; i++)
                if (Doctors[i] != null && Doctors[i].DoctorID == id) return true;

            for (int i = 0; i < PatientCount; i++)
                if (Patients[i] != null && Patients[i].PatientID == id) return true;

            for (int i = 0; i < AppointmentCount; i++)
                if (Appointments[i] != null && Appointments[i].AppointmentID == id) return true;

            return false;
        }

        public static void Save()
        {
            SaveDoctors();
            SavePatients();
            SaveAppointments();
            SaveSettings();
        }

        public static void Load()
        {
            LoadSettings();
            LoadDoctors();
            LoadPatients();
            LoadAppointments();

            if (DoctorCount == 0)
            {
                AddSeedDoctor("ahmed.mansour", "Dr. Ahmed Mansour", "Cardiology", "01011223344", "123", 45, 20, 5);
                AddSeedDoctor("sarah.kamal", "Dr. Sarah Kamal", "Surgery", "01122334455", "123", 38, 12, 3);
                Save();
            }
        }

        static void AddSeedDoctor(string username, string name, string specialty, string phone, string password, int age, int exp, int certs)
        {
            Doctor doctor = new Doctor();
            doctor.CreateDoctor(GenerateMixedID(), username, name, specialty, phone, password, age, exp, certs);
            Doctors[DoctorCount++] = doctor;
        }

        static void SaveDoctors()
        {
            try
            {
                string[] lines = new string[DoctorCount];
                for (int i = 0; i < DoctorCount; i++)
                {
                    lines[i] =
                        Doctors[i].DoctorID + "|" +
                        Doctors[i].Username + "|" +
                        Doctors[i].GetName() + "|" +
                        Doctors[i].Specialty + "|" +
                        Doctors[i].Password + "|" +
                        Doctors[i].GetPhone() + "|" +
                        Doctors[i].Age + "|" +
                        Doctors[i].ExperienceYears + "|" +
                        Doctors[i].CertificationsCount;
                }
                File.WriteAllLines(DoctorsFile, lines);
            }
            catch
            {
            }
        }

        static void SavePatients()
        {
            try
            {
                string[] lines = new string[PatientCount];
                for (int i = 0; i < PatientCount; i++)
                {
                    lines[i] =
                        Patients[i].PatientID + "|" +
                        Patients[i].GetName() + "|" +
                        Patients[i].Age + "|" +
                        Patients[i].Gender + "|" +
                        Patients[i].GetPhone() + "|" +
                        Patients[i].Password;
                }
                File.WriteAllLines(PatientsFile, lines);
            }
            catch
            {
            }
        }

        static void SaveAppointments()
        {
            try
            {
                string[] lines = new string[AppointmentCount];
                for (int i = 0; i < AppointmentCount; i++)
                {
                    lines[i] =
                        Appointments[i].AppointmentID + "|" +
                        Appointments[i].PatientID + "|" +
                        Appointments[i].DoctorID + "|" +
                        Appointments[i].AppointmentDate.ToString("yyyy-MM-dd HH:mm") + "|" +
                        Appointments[i].Status + "|" +
                        Appointments[i].Diagnosis;
                }
                File.WriteAllLines(AppointmentsFile, lines);
            }
            catch
            {
            }
        }

        static void LoadDoctors()
        {
            try
            {
                if (!File.Exists(DoctorsFile)) return;
                string[] lines = File.ReadAllLines(DoctorsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] p = lines[i].Split('|');
                    if (p.Length >= 8 && DoctorCount < Doctors.Length)
                    {
                        string username = "";
                        string name = "";
                        string specialty = "";
                        string password = "";
                        string phone = "";
                        int age = 0;
                        int exp = 0;
                        int certs = 0;

                        if (p.Length == 8)
                        {
                            username = BuildDoctorUsername(p[1]);
                            name = p[1];
                            specialty = p[2];
                            password = p[3];
                            phone = p[4];
                            int.TryParse(p[5], out age);
                            int.TryParse(p[6], out exp);
                            int.TryParse(p[7], out certs);
                        }
                        else
                        {
                            username = p[1];
                            name = p[2];
                            specialty = p[3];
                            password = p[4];
                            phone = p[5];
                            int.TryParse(p[6], out age);
                            int.TryParse(p[7], out exp);
                            int.TryParse(p[8], out certs);
                        }

                        Doctor doctor = new Doctor();
                        doctor.CreateDoctor(p[0], username, name, specialty, phone, password, age, exp, certs);
                        Doctors[DoctorCount++] = doctor;
                    }
                }
            }
            catch
            {
                Doctors = new Doctor[100];
                DoctorCount = 0;
            }
        }

        static void LoadPatients()
        {
            try
            {
                if (!File.Exists(PatientsFile)) return;
                string[] lines = File.ReadAllLines(PatientsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] p = lines[i].Split('|');
                    if (p.Length == 6 && PatientCount < Patients.Length)
                    {
                        int.TryParse(p[2], out int age);
                        Patient patient = new Patient();
                        patient.CreatePatient(p[0], p[1], age, p[3], p[4], p[5]);
                        Patients[PatientCount++] = patient;
                    }
                }
            }
            catch
            {
                Patients = new Patient[200];
                PatientCount = 0;
            }
        }

        static void LoadAppointments()
        {
            try
            {
                if (!File.Exists(AppointmentsFile)) return;
                string[] lines = File.ReadAllLines(AppointmentsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] p = lines[i].Split('|');
                    if (p.Length >= 6 && AppointmentCount < Appointments.Length)
                    {
                        DateTime.TryParse(p[3], out DateTime date);
                        AppointmentStatus status = AppointmentStatus.Pending;
                        try { status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), p[4]); }
                        catch { }

                        string diagnosis = p.Length > 5 ? p[5] : "";

                        Appointment appointment = new Appointment();
                        appointment.CreateAppointment(p[0], p[1], p[2], date, status, diagnosis);
                        Appointments[AppointmentCount++] = appointment;
                    }
                }
            }
            catch
            {
                Appointments = new Appointment[500];
                AppointmentCount = 0;
            }
        }

        public static int FindPatientByID(string id)
        {
            for (int i = 0; i < PatientCount; i++)
                if (Patients[i] != null && Patients[i].PatientID.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return i;
            return -1;
        }

        public static int FindPatientByNameAndPassword(string name, string password)
        {
            for (int i = 0; i < PatientCount; i++)
            {
                if (Patients[i] != null &&
                    Patients[i].GetName().Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    Patients[i].Password == password)
                    return i;
            }
            return -1;
        }

        public static int FindDoctorByLogin(string name, string password)
        {
            for (int i = 0; i < DoctorCount; i++)
            {
                if (Doctors[i] != null &&
                    Doctors[i].Username.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    Doctors[i].Password == password)
                    return i;
            }
            return -1;
        }

        public static int FindPatientByNameAndPhone(string name, string phone)
        {
            for (int i = 0; i < PatientCount; i++)
            {
                if (Patients[i] != null &&
                    Patients[i].GetName().Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    Patients[i].GetPhone() == phone)
                    return i;
            }
            return -1;
        }

        public static int FindDoctorByID(string id)
        {
            for (int i = 0; i < DoctorCount; i++)
                if (Doctors[i] != null && Doctors[i].DoctorID.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return i;
            return -1;
        }

        public static int FindAppointmentByDoctorAndTime(string doctorID, DateTime time)
        {
            for (int i = 0; i < AppointmentCount; i++)
            {
                if (Appointments[i] != null &&
                    Appointments[i].DoctorID == doctorID &&
                    Appointments[i].AppointmentDate == time)
                    return i;
            }
            return -1;
        }

        public static int FindAppointmentByIDAndDoctor(string appointmentID, string doctorID)
        {
            for (int i = 0; i < AppointmentCount; i++)
            {
                if (Appointments[i] != null &&
                    Appointments[i].AppointmentID.Equals(appointmentID, StringComparison.OrdinalIgnoreCase) &&
                    Appointments[i].DoctorID == doctorID)
                    return i;
            }
            return -1;
        }

        public static void GetDistinctSpecialties(string[] result, out int count)
        {
            count = 0;
            for (int i = 0; i < DoctorCount; i++)
            {
                if (Doctors[i] == null) continue;
                bool found = false;
                for (int j = 0; j < count; j++)
                {
                    if (result[j].Equals(Doctors[i].Specialty, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    result[count++] = Doctors[i].Specialty;
            }
        }

        public static int CountAppointmentsForPatient(string patientId)
        {
            int count = 0;
            for (int i = 0; i < AppointmentCount; i++)
            {
                if (Appointments[i] != null && Appointments[i].PatientID == patientId)
                    count++;
            }
            return count;
        }

        public static void DeleteDoctorByIndex(int doctorIndex)
        {
            if (doctorIndex < 0 || doctorIndex >= DoctorCount || Doctors[doctorIndex] == null) return;
            string doctorId = Doctors[doctorIndex].DoctorID;

            for (int i = 0; i < AppointmentCount; i++)
            {
                if (Appointments[i] != null && Appointments[i].DoctorID == doctorId)
                {
                    RemoveAppointmentAt(i);
                    i--;
                }
            }

            for (int i = doctorIndex; i < DoctorCount - 1; i++)
                Doctors[i] = Doctors[i + 1];
            Doctors[DoctorCount - 1] = null;
            DoctorCount--;
        }

        public static void DeletePatientByIndex(int patientIndex)
        {
            if (patientIndex < 0 || patientIndex >= PatientCount || Patients[patientIndex] == null) return;
            string patientId = Patients[patientIndex].PatientID;

            for (int i = 0; i < AppointmentCount; i++)
            {
                if (Appointments[i] != null && Appointments[i].PatientID == patientId)
                {
                    RemoveAppointmentAt(i);
                    i--;
                }
            }

            for (int i = patientIndex; i < PatientCount - 1; i++)
                Patients[i] = Patients[i + 1];
            Patients[PatientCount - 1] = null;
            PatientCount--;
        }

        public static void RemoveAppointmentAt(int index)
        {
            if (index < 0 || index >= AppointmentCount) return;
            for (int i = index; i < AppointmentCount - 1; i++)
                Appointments[i] = Appointments[i + 1];
            Appointments[AppointmentCount - 1] = null;
            AppointmentCount--;
        }

        static void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsFile)) return;
                string[] lines = File.ReadAllLines(SettingsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] p = lines[i].Split('=');
                    if (p.Length != 2) continue;
                    if (p[0] == "AdminPassword")
                        AdminPassword = p[1];
                }
            }
            catch
            {
                AdminPassword = "123";
            }
        }

        static void SaveSettings()
        {
            try
            {
                File.WriteAllLines(SettingsFile, new string[] { "AdminPassword=" + AdminPassword });
            }
            catch
            {
            }
        }

        static string BuildDoctorUsername(string name)
        {
            string cleaned = name.Replace("Dr. ", "", StringComparison.OrdinalIgnoreCase).Trim().ToLower();
            return cleaned.Replace(" ", ".");
        }
    }
}
