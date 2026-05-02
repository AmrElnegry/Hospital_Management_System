using System;
using System.IO;

namespace Project_Hospital
{
    enum AppointmentStatus { Pending, Completed, Cancelled }

    abstract class Person
    {
        private string name = "";
        private string phone = "";

        public void SetName(string value) { name = value; }
        public void SetPhone(string value) { phone = value; }
        public string GetName() { return name; }
        public string GetPhone() { return phone; }
    }

    class Patient : Person
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

    class Doctor : Person
    {
        public string DoctorID = "";
        public string Specialty = "";
        public string Password = "";
        public int Age = 0;
        public int ExperienceYears = 0;
        public int CertificationsCount = 0;

        public void CreateDoctor(string id, string name, string specialty, string phone, string password, int age, int experience, int certifications)
        {
            DoctorID = id;
            Specialty = specialty;
            Password = password;
            Age = age;
            ExperienceYears = experience;
            CertificationsCount = certifications;
            SetName(name);
            SetPhone(phone);
        }

        public bool IsAvailable(DateTime time)
        {
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                Appointment a = DB.Appointments[i];
                if (a != null &&
                    a.DoctorID == DoctorID &&
                    a.AppointmentDate == time &&
                    (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Cancelled))
                    return false;
            }
            return true;
        }
    }

    class Appointment
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

    static class DB
    {
        public static Doctor[] Doctors = new Doctor[100];
        public static Patient[] Patients = new Patient[200];
        public static Appointment[] Appointments = new Appointment[500];

        public static int DoctorCount = 0;
        public static int PatientCount = 0;
        public static int AppointmentCount = 0;

        private static string DoctorsFile = "doctors_db.txt";
        private static string PatientsFile = "patients_db.txt";
        private static string AppointmentsFile = "appointments_db.txt";

        private static Random random = new Random();
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateMixedID()
        {
            string id = "";
            do
            {
                id = "";
                for (int i = 0; i < 4; i++) id += chars[random.Next(chars.Length)];
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
        }

        public static void Load()
        {
            LoadDoctors();
            LoadPatients();
            LoadAppointments();

            if (DoctorCount == 0)
            {
                AddSeedDoctor("Dr. Ahmed Mansour", "Cardiology", "01011223344", "123", 45, 20, 5);
                AddSeedDoctor("Dr. Sarah Kamal", "Surgery", "01122334455", "123", 38, 12, 3);
                Save();
            }
        }

        static void AddSeedDoctor(string name, string specialty, string phone, string password, int age, int exp, int certs)
        {
            Doctor doctor = new Doctor();
            doctor.CreateDoctor(GenerateMixedID(), name, specialty, phone, password, age, exp, certs);
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
            catch (Exception ex)
            {
                Console.WriteLine("Save Error: " + ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine("Save Error: " + ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine("Save Error: " + ex.Message);
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
                    if (p.Length == 8 && DoctorCount < Doctors.Length)
                    {
                        int age = 0, exp = 0, certs = 0;
                        int.TryParse(p[5], out age);
                        int.TryParse(p[6], out exp);
                        int.TryParse(p[7], out certs);

                        Doctor doctor = new Doctor();
                        doctor.CreateDoctor(p[0], p[1], p[2], p[4], p[3], age, exp, certs);
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
                        int age = 0;
                        int.TryParse(p[2], out age);
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
                        DateTime date = DateTime.Now;
                        DateTime.TryParse(p[3], out date);
                        AppointmentStatus status = AppointmentStatus.Pending;
                        try { status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), p[4]); }
                        catch { status = AppointmentStatus.Pending; }
                        string diagnosis = "";
                        if (p.Length > 5) diagnosis = p[5];

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
                    (Doctors[i].GetName().Equals(name, StringComparison.OrdinalIgnoreCase) ||
                     Doctors[i].GetName().Equals("Dr. " + name, StringComparison.OrdinalIgnoreCase)) &&
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
                bool found = false;
                for (int j = 0; j < count; j++)
                {
                    if (result[j].Equals(Doctors[i].Specialty, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) result[count++] = Doctors[i].Specialty;
            }
        }
    }

    class Program
    {
        static string[] Slots =
        {
            "09:00 AM", "10:00 AM", "11:00 AM", "12:00 PM",
            "01:00 PM", "02:00 PM", "03:00 PM", "04:00 PM"
        };

        static void Main()
        {
            DB.Load();

            while (true)
            {
                Console.WriteLine("\n======== Hospital Management System ======== ");
                Console.WriteLine("Type 'B' at any prompt to go back");
                Console.WriteLine("1. Book Appointment");
                Console.WriteLine("2. Staff Login (Admin/Doctor)");
                Console.WriteLine("3. Patient Registration");
                Console.WriteLine("4. Exit & Save");

                string choice = GetInput("Choice: ");
                if (choice == "4") { DB.Save(); break; }

                switch (choice)
                {
                    case "1": StartBooking(); break;
                    case "2": StaffLogin(); break;
                    case "3": RegisterPatient(); break;
                    default:
                        if (choice.ToLower() != "b") Console.WriteLine("Error: This option does not exist! Please try again.");
                        break;
                }
            }
        }

        static string GetInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();
                if (input != "") return input;
                Console.WriteLine("Error: Entry cannot be empty!");
            }
        }

        static bool IsBack(string input)
        {
            return input.ToLower() == "b";
        }

        static string GetValidText(string label, bool isName)
        {
            while (true)
            {
                string input = GetInput(label);
                if (IsBack(input)) return "BACK_SIGNAL";

                bool valid = true;
                int names = 0;
                bool inWord = false;

                for (int i = 0; i < input.Length; i++)
                {
                    char ch = input[i];
                    if (char.IsLetter(ch) || ch == ' ' || ch == '.')
                    {
                        if (ch != ' ' && !inWord)
                        {
                            names++;
                            inWord = true;
                        }
                        else if (ch == ' ')
                        {
                            inWord = false;
                        }
                    }
                    else
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    if (isName && names < 2)
                    {
                        Console.WriteLine("Error: Please provide a valid full name (First and Last name).");
                        continue;
                    }
                    return input;
                }

                Console.WriteLine("Error: Only letters are allowed (No numbers or special characters).");
            }
        }

        static string GetValidPhone(string label)
        {
            while (true)
            {
                string input = GetInput(label);
                if (IsBack(input)) return "BACK_SIGNAL";

                bool digitsOnly = true;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!char.IsDigit(input[i]))
                    {
                        digitsOnly = false;
                        break;
                    }
                }

                if (!digitsOnly)
                {
                    Console.WriteLine("Error: Phone must contain ONLY numbers. Letters, spaces, or symbols are not allowed.");
                    continue;
                }

                if (input.Length != 11)
                {
                    Console.WriteLine("Error: Phone must be exactly 11 digits. You entered " + input.Length + " digits.");
                    continue;
                }

                if (!(input.StartsWith("010") || input.StartsWith("011") || input.StartsWith("012") || input.StartsWith("015")))
                {
                    Console.WriteLine("Error: Phone must start with a valid prefix (010, 011, 012, or 015).");
                    continue;
                }

                return input;
            }
        }

        static int GetValidNumber(string label, int min, int max)
        {
            while (true)
            {
                string input = GetInput(label);
                if (IsBack(input)) return -1;

                int number;
                if (int.TryParse(input, out number) && number >= min && number <= max)
                    return number;

                Console.WriteLine("Error: Input must be a number between " + min + " and " + max + ".");
            }
        }

        static string GetValidGender(string label)
        {
            while (true)
            {
                string input = GetInput(label);
                if (IsBack(input)) return "BACK_SIGNAL";

                string standardized = "";
                if (input.Length > 0)
                    standardized = char.ToUpper(input[0]) + input.Substring(1).ToLower();

                if (standardized == "Male" || standardized == "Female")
                    return standardized;

                Console.WriteLine("Error: Invalid entry! Gender must be exactly 'Male' or 'Female'.");
            }
        }

        static Patient RegisterPatient(bool returnObject = false)
        {
            Console.WriteLine("\n--- Registration ---");
            string n = GetValidText("Full Name: ", true); if (n == "BACK_SIGNAL") return null;
            int a = GetValidNumber("Age: ", 5, 100); if (a == -1) return null;
            string g = GetValidGender("Gender (Male/Female): "); if (g == "BACK_SIGNAL") return null;
            string ph = GetValidPhone("Phone Number: "); if (ph == "BACK_SIGNAL") return null;
            string p = GetInput("Password: "); if (IsBack(p)) return null;

            Patient pat = new Patient();
            pat.CreatePatient(DB.GenerateMixedID(), n, a, g, ph, p);
            DB.Patients[DB.PatientCount++] = pat;
            DB.Save();
            Console.WriteLine("Registration Successful! Your Unique ID is: " + pat.PatientID);

            if (returnObject) return pat;
            return null;
        }

        static void StartBooking(Patient existingPatient = null)
        {
            Patient current = existingPatient;

            while (current == null)
            {
                Console.WriteLine("\n1. Login with ID | 2. New Registration | 3. Forget ID?");
                string c = GetInput("Choice: "); if (IsBack(c)) return;

                if (c == "2")
                {
                    current = RegisterPatient(true);
                    if (current == null) return;
                }
                else if (c == "3")
                {
                    string n = GetValidText("Enter Full Name: ", true); if (n == "BACK_SIGNAL") return;
                    string p = GetInput("Enter Password: "); if (IsBack(p)) return;

                    int idx = DB.FindPatientByNameAndPassword(n, p);
                    if (idx != -1)
                    {
                        current = DB.Patients[idx];
                        Console.WriteLine("ID Successfully Recovered: " + current.PatientID);
                    }
                    else Console.WriteLine("Identity not found! Please check your credentials.");
                }
                else if (c == "1")
                {
                    string id = GetInput("Enter ID: ").ToUpper(); if (IsBack(id)) return;
                    int idx = DB.FindPatientByID(id);
                    if (idx != -1) current = DB.Patients[idx];
                    else Console.WriteLine("ID not recognized in our system.");
                }
            }

            while (true)
            {
                string[] specs = new string[100];
                int specsCount = 0;
                DB.GetDistinctSpecialties(specs, out specsCount);

                Console.WriteLine("\nSpecialties Available:");
                for (int i = 0; i < specsCount; i++) Console.WriteLine((i + 1) + ". " + specs[i]);

                string sIn = GetInput("Choice: "); if (IsBack(sIn)) return;
                string chosen = "";

                int specIndex;
                if (int.TryParse(sIn, out specIndex) && specIndex > 0 && specIndex <= specsCount)
                {
                    chosen = specs[specIndex - 1];
                }
                else
                {
                    for (int i = 0; i < specsCount; i++)
                    {
                        if (specs[i].Equals(sIn, StringComparison.OrdinalIgnoreCase))
                        {
                            chosen = specs[i];
                            break;
                        }
                    }
                }

                if (chosen == "")
                {
                    Console.WriteLine("Error: Specialty not found!");
                    continue;
                }

                Console.WriteLine("\nDoctors Available:");
                for (int i = 0; i < DB.DoctorCount; i++)
                {
                    if (DB.Doctors[i].Specialty.Equals(chosen, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("ID: " + DB.Doctors[i].DoctorID + " | " + DB.Doctors[i].GetName() + " | Experience: " + DB.Doctors[i].ExperienceYears + " years");
                    }
                }

                string dIn = GetInput("Select Doctor ID: ").ToUpper(); if (IsBack(dIn)) continue;
                int doctorIndex = DB.FindDoctorByID(dIn);
                if (doctorIndex == -1 || !DB.Doctors[doctorIndex].Specialty.Equals(chosen, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: Doctor ID not found!");
                    continue;
                }

                while (true)
                {
                    Console.WriteLine("\nSelect Date (Upcoming 7 Days):");
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime day = DateTime.Today.AddDays(i);
                        Console.WriteLine((i + 1) + ". " + day.ToString("dd/MM/yyyy") + " - " + day.DayOfWeek);
                    }

                    string dayIn = GetInput("Choice: "); if (IsBack(dayIn)) break;
                    int dIdx;
                    if (!int.TryParse(dayIn, out dIdx) || dIdx <= 0 || dIdx > 7)
                    {
                        Console.WriteLine("Error: Invalid day selection!");
                        continue;
                    }

                    DateTime selected = DateTime.Today.AddDays(dIdx - 1);

                    while (true)
                    {
                        Console.WriteLine("\nHours for " + selected.ToString("dd/MM/yyyy") + " (" + selected.DayOfWeek + "):");
                        for (int i = 0; i < Slots.Length; i++)
                        {
                            DateTime time = DateTime.Parse(selected.ToShortDateString() + " " + Slots[i]);
                            string statusText = "Available";
                            int apptIndex = DB.FindAppointmentByDoctorAndTime(DB.Doctors[doctorIndex].DoctorID, time);

                            if (apptIndex != -1)
                            {
                                if (DB.Appointments[apptIndex].Status == AppointmentStatus.Pending) statusText = "Taken";
                                else if (DB.Appointments[apptIndex].Status == AppointmentStatus.Cancelled) statusText = "Unavailable";
                            }

                            Console.WriteLine((i + 1) + ". " + Slots[i] + " - " + statusText);
                        }

                        string hIn = GetInput("Choice (Number or Time): "); if (IsBack(hIn)) break;

                        string match = "";
                        int hIdx;
                        if (int.TryParse(hIn, out hIdx) && hIdx > 0 && hIdx <= Slots.Length)
                        {
                            match = Slots[hIdx - 1];
                        }
                        else
                        {
                            for (int i = 0; i < Slots.Length; i++)
                            {
                                if (Slots[i].Equals(hIn, StringComparison.OrdinalIgnoreCase))
                                {
                                    match = Slots[i];
                                    break;
                                }
                            }
                        }

                        if (match == "")
                        {
                            Console.WriteLine("Error: Invalid time slot selection!");
                            continue;
                        }

                        DateTime final = DateTime.Parse(selected.ToShortDateString() + " " + match);
                        int existingIndex = DB.FindAppointmentByDoctorAndTime(DB.Doctors[doctorIndex].DoctorID, final);

                        if (existingIndex == -1)
                        {
                            Appointment appointment = new Appointment();
                            appointment.CreateAppointment(DB.GenerateMixedID(), current.PatientID, DB.Doctors[doctorIndex].DoctorID, final, AppointmentStatus.Pending, "");
                            DB.Appointments[DB.AppointmentCount++] = appointment;
                            DB.Save();
                            Console.WriteLine("Booking Confirmed! System has reserved your slot.");
                            return;
                        }
                        else if (DB.Appointments[existingIndex].Status == AppointmentStatus.Pending)
                        {
                            Console.WriteLine("Error: This slot is already taken by another patient!");
                        }
                        else if (DB.Appointments[existingIndex].Status == AppointmentStatus.Cancelled)
                        {
                            Console.WriteLine("Error: This slot is marked as Unavailable by the doctor!");
                        }
                    }
                }
            }
        }

        static void StaffLogin()
        {
            while (true)
            {
                Console.WriteLine("\n1. Admin Panel | 2. Doctor Portal");
                string c = GetInput("Choice: "); if (IsBack(c)) return;

                if (c == "1")
                {
                    if (GetInput("Admin Password: ") == "123") AdminPanel();
                    else Console.WriteLine("Access Denied: Incorrect Password!");
                }
                else if (c == "2")
                {
                    string n = GetValidText("Full Name: ", true); if (n == "BACK_SIGNAL") return;
                    string p = GetInput("Password: "); if (IsBack(p)) return;

                    int idx = DB.FindDoctorByLogin(n, p);
                    if (idx != -1) DoctorPanel(DB.Doctors[idx]);
                    else Console.WriteLine("Access Denied: Staff credentials not recognized.");
                }
            }
        }

        static void AdminPanel()
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Management Dashboard ---");
                Console.WriteLine("1. Add Doctor | 2. Edit Doctor | 3. Delete Doctor | 4. Edit Patient | 5. View All Doctors | 6. View All Patients | 7. Back");
                string c = GetInput("Choice: "); if (IsBack(c) || c == "7") return;

                switch (c)
                {
                    case "1":
                        string n = GetValidText("Full Name: ", true); if (n == "BACK_SIGNAL") break;
                        if (!n.StartsWith("Dr. ")) n = "Dr. " + n;
                        string s = GetValidText("Specialty: ", false); if (s == "BACK_SIGNAL") break;
                        string ph = GetValidPhone("Phone: "); if (ph == "BACK_SIGNAL") break;
                        int age = GetValidNumber("Age: ", 25, 75); if (age == -1) break;
                        int exp = GetValidNumber("Experience Years: ", 0, 50); if (exp == -1) break;
                        int cert = GetValidNumber("Certifications Count: ", 0, 20); if (cert == -1) break;
                        string pass = GetInput("Password: "); if (IsBack(pass)) break;

                        if (DB.DoctorCount < DB.Doctors.Length)
                        {
                            Doctor doctor = new Doctor();
                            doctor.CreateDoctor(DB.GenerateMixedID(), n, s, ph, pass, age, exp, cert);
                            DB.Doctors[DB.DoctorCount++] = doctor;
                            Console.WriteLine("Doctor successfully enrolled.");
                        }
                        break;

                    case "4":
                        string searchN = GetValidText("Search Patient Name: ", true); if (searchN == "BACK_SIGNAL") break;
                        string searchPh = GetValidPhone("Search Patient Phone: "); if (searchPh == "BACK_SIGNAL") break;

                        int patientIndex = DB.FindPatientByNameAndPhone(searchN, searchPh);
                        if (patientIndex != -1)
                        {
                            Patient pat = DB.Patients[patientIndex];
                            Console.WriteLine("Found: " + pat.GetName() + " (ID: " + pat.PatientID + ")");

                            string newN = GetValidText("New Name: ", true); if (newN != "BACK_SIGNAL") pat.SetName(newN);
                            string newPh = GetValidPhone("New Phone: "); if (newPh != "BACK_SIGNAL") pat.SetPhone(newPh);
                            int newA = GetValidNumber("New Age (5-100): ", 5, 100); if (newA != -1) pat.Age = newA;
                            string newPass = GetInput("New Password: "); if (!IsBack(newPass)) pat.Password = newPass;

                            string oldID = pat.PatientID;
                            pat.PatientID = DB.GenerateMixedID();

                            Console.WriteLine("Would you like to cancel all pending appointments for this patient? (Yes/No)");
                            if (GetInput("Choice: ").ToLower() == "yes")
                            {
                                for (int i = 0; i < DB.AppointmentCount; i++)
                                {
                                    if (DB.Appointments[i].PatientID == oldID && DB.Appointments[i].Status == AppointmentStatus.Pending)
                                        DB.Appointments[i].Status = AppointmentStatus.Cancelled;
                                }
                                Console.WriteLine("Pending appointments cancelled.");
                            }
                            else
                            {
                                for (int i = 0; i < DB.AppointmentCount; i++)
                                {
                                    if (DB.Appointments[i].PatientID == oldID)
                                        DB.Appointments[i].PatientID = pat.PatientID;
                                }
                                Console.WriteLine("Appointments updated with the new ID.");
                            }
                            Console.WriteLine("Profile updated successfully! New ID: " + pat.PatientID);
                        }
                        else Console.WriteLine("Error: Patient record not found!");
                        break;

                    case "5":
                        Console.WriteLine("\n--- DOCTORS OVERVIEW ---");
                        for (int i = 0; i < DB.DoctorCount; i++)
                            Console.WriteLine("ID: " + DB.Doctors[i].DoctorID + " | " + DB.Doctors[i].GetName() + " | " + DB.Doctors[i].Specialty);
                        break;

                    case "6":
                        Console.WriteLine("\n--- PATIENTS OVERVIEW ---");
                        for (int i = 0; i < DB.PatientCount; i++)
                            Console.WriteLine("ID: " + DB.Patients[i].PatientID + " | Name: " + DB.Patients[i].GetName() + " | Phone: " + DB.Patients[i].GetPhone());
                        break;

                    default:
                        Console.WriteLine("Error: Invalid choice!");
                        break;
                }
                DB.Save();
            }
        }

        static void DoctorPanel(Doctor currentDoctor)
        {
            while (true)
            {
                Console.WriteLine("\n--- Welcome " + currentDoctor.GetName() + " ---");
                Console.WriteLine("1. My Schedule | 2. Finalize Clinical Visit | 3. Cancel Appointment | 4. Back");
                string c = GetInput("Choice: "); if (IsBack(c) || c == "4") return;

                switch (c)
                {
                    case "1":
                        bool found = false;
                        for (int i = 0; i < DB.AppointmentCount; i++)
                        {
                            if (DB.Appointments[i].DoctorID == currentDoctor.DoctorID)
                            {
                                found = true;
                                Console.WriteLine(
                                    "Visit ID: " + DB.Appointments[i].AppointmentID +
                                    " | Date: " + DB.Appointments[i].AppointmentDate.ToString("dd/MM/yyyy HH:mm") +
                                    " | Status: " + DB.Appointments[i].Status);
                            }
                        }
                        if (!found) Console.WriteLine("No visits in queue.");
                        break;

                    case "3":
                        Console.WriteLine("\nSelect Date to Cancel (Upcoming 7 Days):");
                        for (int i = 0; i < 7; i++)
                        {
                            DateTime day = DateTime.Today.AddDays(i);
                            Console.WriteLine((i + 1) + ". " + day.ToString("dd/MM/yyyy") + " - " + day.DayOfWeek);
                        }

                        string dayIn = GetInput("Choice: "); if (IsBack(dayIn)) break;
                        int dIdx;
                        if (int.TryParse(dayIn, out dIdx) && dIdx > 0 && dIdx <= 7)
                        {
                            DateTime selected = DateTime.Today.AddDays(dIdx - 1);
                            Console.WriteLine("\nHours for " + selected.ToString("dd/MM/yyyy") + ":");
                            for (int i = 0; i < Slots.Length; i++) Console.WriteLine((i + 1) + ". " + Slots[i]);

                            string hIn = GetInput("Choice (Number): "); if (IsBack(hIn)) break;
                            int hIdx;
                            if (int.TryParse(hIn, out hIdx) && hIdx > 0 && hIdx <= 8)
                            {
                                DateTime finalTime = DateTime.Parse(selected.ToShortDateString() + " " + Slots[hIdx - 1]);
                                int targetIndex = DB.FindAppointmentByDoctorAndTime(currentDoctor.DoctorID, finalTime);

                                if (targetIndex != -1)
                                {
                                    Console.WriteLine("Are you sure you want to cancel the appointment for Patient " + DB.Appointments[targetIndex].PatientID + "? (Yes/No)");
                                    if (GetInput("Choice: ").ToLower() == "yes")
                                    {
                                        DB.Appointments[targetIndex].Status = AppointmentStatus.Cancelled;
                                        Console.WriteLine("Appointment successfully cancelled and marked as Unavailable.");
                                    }
                                }
                                else
                                {
                                    Appointment block = new Appointment();
                                    block.CreateAppointment(DB.GenerateMixedID(), "DOCTOR_BLOCK", currentDoctor.DoctorID, finalTime, AppointmentStatus.Cancelled, "");
                                    DB.Appointments[DB.AppointmentCount++] = block;
                                    Console.WriteLine("Time slot has been blocked as Unavailable by doctor.");
                                }
                            }
                        }
                        break;

                    case "2":
                        string aid = GetInput("Enter Visit ID to Finalize: ").ToUpper(); if (IsBack(aid)) break;
                        int appointmentIndex = DB.FindAppointmentByIDAndDoctor(aid, currentDoctor.DoctorID);

                        if (appointmentIndex != -1)
                        {
                            while (true)
                            {
                                string diag = GetInput("Medical Diagnosis (Min 4 words): "); if (IsBack(diag)) break;
                                string[] words = diag.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (words.Length >= 4)
                                {
                                    DB.Appointments[appointmentIndex].Diagnosis = diag;
                                    DB.Appointments[appointmentIndex].Status = AppointmentStatus.Completed;
                                    break;
                                }
                                Console.WriteLine("Error: Diagnosis notes too short.");
                            }
                        }
                        break;
                }
                DB.Save();
            }
        }
    }
}
