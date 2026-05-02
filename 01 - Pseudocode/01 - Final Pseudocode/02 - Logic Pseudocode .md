# Hospital Management System - Pseudocode

## 1. Program Start

```text
START PROGRAM
    Load data from text files
    Register auto-save on exit / crash

    LOOP forever
        Show Home Menu:
            1. Book Appointment
            2. Patient Dashboard
            3. Staff Login
            4. Exit

        Read user choice

        IF choice = 1
            Go to Booking Flow
        ELSE IF choice = 2
            Go to Patient Login
        ELSE IF choice = 3
            Go to Staff Login
        ELSE IF choice = 4
            Save data
            End program
        ELSE
            Show error message
    END LOOP
END PROGRAM
```

---

## 2. Data Model

```text
CLASS Person
    name
    phone
    methods:
        SetName()
        SetPhone()
        GetName()
        GetPhone()

CLASS Patient inherits Person
    PatientID
    Age
    Gender
    Password
    method:
        CreatePatient()

CLASS Doctor inherits Person
    DoctorID
    Username
    Specialty
    Password
    Age
    ExperienceYears
    CertificationsCount
    method:
        CreateDoctor()

CLASS Appointment
    AppointmentID
    PatientID
    DoctorID
    AppointmentDate
    Status
    Diagnosis
    method:
        CreateAppointment()

ENUM AppointmentStatus
    Pending
    Completed
    Cancelled
```

---

## 3. Database Logic

```text
STATIC CLASS DB
    Arrays:
        Doctors[100]
        Patients[200]
        Appointments[500]

    Counters:
        DoctorCount
        PatientCount
        AppointmentCount

    Store admin password

    FUNCTIONS:
        GenerateMixedID()
            Generate unique ID
            Return ID

        Save()
            Save doctors file
            Save patients file
            Save appointments file
            Save settings file

        Load()
            Load settings
            Load doctors
            Load patients
            Load appointments
            IF no doctors exist
                Add seed doctors
                Save all

        FindPatientByID()
        FindPatientByNameAndPassword()
        FindDoctorByLogin()
        FindPatientByNameAndPhone()
        FindDoctorByID()
        FindAppointmentByDoctorAndTime()
        CountAppointmentsForPatient()
        GetDistinctSpecialties()
        DeleteDoctorByIndex()
        DeletePatientByIndex()
        RemoveAppointmentAt()
```

---

## 4. Validation Logic

```text
CLASS ValidationHelper
    IsValidFullName()
        Accept only letters, spaces, dots
        Require at least first and last name

    IsValidTextOnly()
        Accept only letters, spaces, dots

    IsValidEgyptianPhone()
        Must be 11 digits
        Must start with 010 / 011 / 012 / 015
```

---

## 5. Booking Flow

```text
FUNCTION BookingEntryFlow()
    LOOP
        Show:
            1. Login with ID
            2. New Registration
            3. Forgot ID
            4. Back

        Read choice

        IF Login with ID
            Find patient by ID
        IF New Registration
            Register new patient
        IF Forgot ID
            Recover patient by name + password
        IF Back
            Return Home

        IF patient found successfully
            RunBookingForPatient(patient)
            EXIT function
    END LOOP
```

### Booking Details

```text
FUNCTION RunBookingForPatient(patient)
    Get all distinct specialties
    Show specialties
    User selects specialty

    Show doctors for selected specialty
    User selects doctor

    Show next 7 days
    User selects day

    Show predefined time slots
    For each slot:
        IF slot already booked
            Mark as Taken
        ELSE IF slot cancelled/blocked
            Mark as Unavailable
        ELSE
            Mark as Available

    User selects time

    IF slot is free
        Create appointment
        Save data
        Show booking confirmation
    ELSE
        Show error
```

---

## 6. Patient Registration

```text
FUNCTION RegisterPatient()
    Read full name
    Validate full name

    Read age
    Validate age > 0

    Read gender
    Validate Male or Female

    Read phone
    Validate phone

    Read password
    Validate not empty

    Create patient object
    Add to DB
    Save data
    Return patient
```

---

## 7. Patient Login and Dashboard

```text
FUNCTION PatientLoginFlow()
    Read patient ID
    Read password

    IF patient exists and password correct
        Open PatientDashboard(patient)
    ELSE
        Show error
```

### Patient Dashboard

```text
FUNCTION PatientDashboard(patient)
    LOOP
        Refresh patient from DB
        IF account deleted
            Show message
            Return

        Show patient info
        Show menu:
            1. View My Appointments
            2. Cancel Appointment
            3. Manage Account
            4. Logout

        IF View My Appointments
            List all appointments with:
                doctor
                specialization
                date/time
                status
                diagnosis

        IF Cancel Appointment
            Select appointment
            IF status is Pending
                Confirm cancellation
                Set status = Cancelled
                Save
            ELSE
                Show error

        IF Manage Account
            Open account management

        IF Logout
            Exit dashboard
    END LOOP
```

### Patient Account Management

```text
FUNCTION ManagePatientAccount(patient)
    LOOP
        Show:
            name
            age
            ID
            password

        Show menu:
            1. Confirm Edit
            2. Delete Account
            3. Cancel

        IF Confirm Edit
            Read new name
            Read new age
            Read new password
            Validate all
            Update patient
            Save

        IF Delete Account
            Confirm delete
            Delete patient and related appointments
            Save
            Return false

        IF Cancel
            Return true
    END LOOP
```

---

## 8. Staff Login

```text
FUNCTION StaffLoginFlow()
    LOOP
        Show:
            1. Admin
            2. Doctor
            3. Back

        IF Admin
            Go to AdminLogin
        IF Doctor
            Go to DoctorLogin
        IF Back
            Return Home
    END LOOP
```

### Admin Login

```text
FUNCTION AdminLoginFlow()
    Read admin password
    IF password correct
        Open AdminDashboard()
    ELSE
        Show error
```

### Doctor Login

```text
FUNCTION DoctorLoginFlow()
    Read username
    Read password

    IF doctor exists with matching username and password
        Open DoctorDashboard(doctor)
    ELSE
        Show error
```

---

## 9. Admin Dashboard

```text
FUNCTION AdminDashboard()
    LOOP
        Show menu:
            1. View Doctors
            2. Add Doctor
            3. Edit Doctor
            4. Delete Doctor
            5. View Patients
            6. Delete Patient
            7. Statistics
            8. Change Password
            9. Logout

        IF View Doctors
            Show all doctors

        IF Add Doctor
            Read doctor data:
                username
                full name
                specialization
                phone
                age
                experience
                certifications
                password
            Validate all
            Create doctor
            Save

        IF Edit Doctor
            Select doctor
            Read edited data
            Validate
            Update doctor
            Save

        IF Delete Doctor
            Select doctor
            Confirm delete
            Delete doctor + related appointments
            Save

        IF View Patients
            Show all patients

        IF Delete Patient
            Select patient
            Confirm delete
            Delete patient + related appointments
            Save

        IF Statistics
            Show:
                total doctors
                total patients
                total appointments
                pending/completed/cancelled counts

        IF Change Password
            Read old password
            Read new password
            Read confirm password
            Validate
            Save new admin password

        IF Logout
            Return to Home
    END LOOP
```

---

## 10. Doctor Dashboard

```text
FUNCTION DoctorDashboard(doctor)
    LOOP
        Show doctor info:
            name
            specialization
            ID
            username

        Show menu:
            1. My Schedule
            2. Add Diagnose
            3. Complete Appointment
            4. Cancel Appointment
            5. Search Appointments
            6. Logout

        IF My Schedule
            Show all doctor appointments

        IF Add Diagnose
            Select appointment
            Read diagnosis
            Set diagnosis
            Set status = Completed
            Save

        IF Complete Appointment
            Select appointment
            Set status = Completed
            Save

        IF Cancel Appointment
            Select appointment
            Confirm
            Set status = Cancelled
            Save

        IF Search Appointments
            Search by:
                patient ID
                OR patient name
            Show all matching appointments

        IF Logout
            Return Home
    END LOOP
```

---

## 11. Auto Save Strategy

```text
ON normal exit
    Save all data

ON application exit event
    Save all data

ON process exit event
    Save all data

ON unhandled exception
    Save all data

AFTER any critical modification:
    Save immediately
```

---

## 12. Overall System Flow Summary

```text
Home
 ├─ Book Appointment
 │   ├─ Login with ID
 │   ├─ New Registration
 │   └─ Forgot ID
 │       └─ Select Specialty → Doctor → Date/Time → Confirm Booking
 │
 ├─ Patient Dashboard
 │   ├─ Login
 │   ├─ View Appointments
 │   ├─ Cancel Appointment
 │   └─ Manage Account
 │
 ├─ Staff Login
 │   ├─ Admin Login
 │   │   └─ Admin Dashboard
 │   └─ Doctor Login
 │       └─ Doctor Dashboard
 │
 └─ Exit
```

