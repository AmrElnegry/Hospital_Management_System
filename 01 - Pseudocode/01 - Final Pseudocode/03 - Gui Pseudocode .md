# Hospital Management System
## Academic Pseudocode Specification

### 1. Main Algorithm

```text
ALGORITHM HospitalManagementSystem
BEGIN
    CALL LoadSystemData()
    REGISTER automatic save handlers

    REPEAT
        DISPLAY Main Menu
            1. Book Appointment
            2. Patient Dashboard
            3. Staff Login
            4. Exit

        choice <- ReadMenuChoice(1, 4)

        CASE choice OF
            1: CALL BookingEntryFlow()
            2: CALL PatientLoginFlow()
            3: CALL StaffLoginFlow()
            4:
                CALL SaveSystemData()
                TERMINATE program
        END CASE
    UNTIL FALSE
END
```

---

### 2. Abstract Data Structure

```text
CLASS Person
    ATTRIBUTES
        name
        phone
    METHODS
        SetName(value)
        SetPhone(value)
        GetName()
        GetPhone()

CLASS Patient EXTENDS Person
    ATTRIBUTES
        PatientID
        Age
        Gender
        Password
    METHODS
        CreatePatient(id, name, age, gender, phone, password)

CLASS Doctor EXTENDS Person
    ATTRIBUTES
        DoctorID
        Username
        Specialty
        Password
        Age
        ExperienceYears
        CertificationsCount
    METHODS
        CreateDoctor(id, username, name, specialty, phone, password, age, experience, certifications)

CLASS Appointment
    ATTRIBUTES
        AppointmentID
        PatientID
        DoctorID
        AppointmentDate
        Status
        Diagnosis
    METHODS
        CreateAppointment(id, patientID, doctorID, appointmentDate, status, diagnosis)

ENUM AppointmentStatus
    Pending
    Completed
    Cancelled
```

---

### 3. Persistent Storage Module

```text
MODULE DB
    STORE
        Doctors[]
        Patients[]
        Appointments[]
        DoctorCount
        PatientCount
        AppointmentCount
        AdminPassword

    FUNCTION GenerateMixedID()
        REPEAT
            id <- generate random alphanumeric identifier
        UNTIL id does not already exist
        RETURN id

    PROCEDURE Load()
        CALL LoadSettings()
        CALL LoadDoctors()
        CALL LoadPatients()
        CALL LoadAppointments()

        IF no doctors exist THEN
            INSERT default doctors
            CALL Save()
        END IF

    PROCEDURE Save()
        CALL SaveDoctors()
        CALL SavePatients()
        CALL SaveAppointments()
        CALL SaveSettings()

    FUNCTION FindPatientByID(id)
    FUNCTION FindPatientByNameAndPassword(name, password)
    FUNCTION FindDoctorByLogin(username, password)
    FUNCTION FindPatientByNameAndPhone(name, phone)
    FUNCTION FindDoctorByID(id)
    FUNCTION FindAppointmentByDoctorAndTime(doctorID, time)
    FUNCTION CountAppointmentsForPatient(patientID)
    PROCEDURE GetDistinctSpecialties(result[], OUT count)
    PROCEDURE DeleteDoctorByIndex(index)
    PROCEDURE DeletePatientByIndex(index)
    PROCEDURE RemoveAppointmentAt(index)
END MODULE
```

---

### 4. Validation Module

```text
MODULE ValidationHelper
    FUNCTION IsValidFullName(input)
        ACCEPT letters, spaces, dots only
        REQUIRE at least two words

    FUNCTION IsValidTextOnly(input)
        ACCEPT letters, spaces, dots only

    FUNCTION IsValidEgyptianPhone(input)
        REQUIRE exactly 11 digits
        REQUIRE prefix in {010, 011, 012, 015}
END MODULE
```

---

### 5. Appointment Booking Subsystem

```text
PROCEDURE BookingEntryFlow()
BEGIN
    REPEAT
        DISPLAY Booking Entry Menu
            1. Login with ID
            2. New Registration
            3. Forgot ID
            4. Back

        choice <- ReadMenuChoice(1, 4)

        CASE choice OF
            1: patient <- LoginPatientByIDOnly()
            2: patient <- RegisterPatient()
            3: patient <- RecoverPatientByIdentity()
            4: RETURN
        END CASE

        IF patient is valid THEN
            CALL RunBookingForPatient(patient)
            RETURN
        END IF
    UNTIL FALSE
END
```

```text
PROCEDURE RunBookingForPatient(patient)
BEGIN
    specialties <- GetDistinctSpecialties()
    DISPLAY specialties
    selectedSpecialty <- user selection

    doctors <- doctors matching selectedSpecialty
    DISPLAY doctors
    selectedDoctor <- user selection

    DISPLAY next seven days
    selectedDate <- user selection

    FOR each slot in daily schedule DO
        IF appointment exists and status = Pending THEN
            mark slot as Taken
        ELSE IF appointment exists and status = Cancelled THEN
            mark slot as Unavailable
        ELSE
            mark slot as Available
        END IF
    END FOR

    selectedTime <- user selection

    IF selected slot is available THEN
        CREATE new appointment
        STORE appointment
        SAVE data
        DISPLAY booking confirmation
    ELSE
        DISPLAY error message
    END IF
END
```

---

### 6. Patient Registration Subsystem

```text
FUNCTION RegisterPatient() RETURNS Patient
BEGIN
    READ full name
    VALIDATE full name

    READ age
    VALIDATE age > 0

    READ gender
    VALIDATE gender = Male OR Female

    READ phone
    VALIDATE phone format

    READ password
    VALIDATE non-empty password

    CREATE patient object
    ADD patient to storage
    SAVE data

    RETURN patient
END
```

---

### 7. Patient Authentication and Dashboard

```text
PROCEDURE PatientLoginFlow()
BEGIN
    READ patient ID
    READ password

    IF credentials are valid THEN
        CALL PatientDashboard(patient)
    ELSE
        DISPLAY access denied message
    END IF
END
```

```text
PROCEDURE PatientDashboard(patient)
BEGIN
    REPEAT
        REFRESH patient from storage
        IF patient no longer exists THEN
            DISPLAY message
            RETURN
        END IF

        DISPLAY patient information
        DISPLAY menu
            1. View My Appointments
            2. Cancel Appointment
            3. Manage Account
            4. Logout

        choice <- ReadMenuChoice(1, 4)

        CASE choice OF
            1: CALL ShowPatientAppointments(patient)
            2: CALL CancelPatientAppointment(patient)
            3: accountExists <- ManagePatientAccount(patient)
               IF accountExists = FALSE THEN RETURN
            4: RETURN
        END CASE
    UNTIL FALSE
END
```

```text
PROCEDURE ShowPatientAppointments(patient)
BEGIN
    FOR each appointment belonging to patient DO
        DISPLAY appointment ID
        DISPLAY doctor name and specialization
        DISPLAY date and time
        DISPLAY status
        DISPLAY diagnosis if present
    END FOR
END
```

```text
PROCEDURE CancelPatientAppointment(patient)
BEGIN
    DISPLAY patient appointments
    selectedAppointment <- user selection

    IF selectedAppointment status = Pending THEN
        ASK for confirmation
        IF confirmed THEN
            SET appointment status <- Cancelled
            SAVE data
        END IF
    ELSE
        DISPLAY validation error
    END IF
END
```

```text
FUNCTION ManagePatientAccount(patient) RETURNS BOOLEAN
BEGIN
    DISPLAY patient account data
    DISPLAY menu
        1. Confirm Edit
        2. Delete Account
        3. Cancel

    choice <- ReadMenuChoice(1, 3)

    CASE choice OF
        1:
            READ updated fields
            VALIDATE updated fields
            UPDATE patient record
            SAVE data
            RETURN TRUE

        2:
            ASK for confirmation
            IF confirmed THEN
                DELETE patient and related appointments
                SAVE data
                RETURN FALSE
            END IF
            RETURN TRUE

        3:
            RETURN TRUE
    END CASE
END
```

---

### 8. Staff Authentication Subsystem

```text
PROCEDURE StaffLoginFlow()
BEGIN
    REPEAT
        DISPLAY Staff Menu
            1. Admin
            2. Doctor
            3. Back

        choice <- ReadMenuChoice(1, 3)

        CASE choice OF
            1: CALL AdminLoginFlow()
            2: CALL DoctorLoginFlow()
            3: RETURN
        END CASE
    UNTIL FALSE
END
```

```text
PROCEDURE AdminLoginFlow()
BEGIN
    READ admin password
    IF password matches stored admin password THEN
        CALL AdminDashboard()
    ELSE
        DISPLAY authentication error
    END IF
END
```

```text
PROCEDURE DoctorLoginFlow()
BEGIN
    READ username
    READ password

    IF username/password pair is valid THEN
        CALL DoctorDashboard(doctor)
    ELSE
        DISPLAY authentication error
    END IF
END
```

---

### 9. Admin Management Subsystem

```text
PROCEDURE AdminDashboard()
BEGIN
    REPEAT
        DISPLAY Admin Menu
            1. View Doctors
            2. Add Doctor
            3. Edit Doctor
            4. Delete Doctor
            5. View Patients
            6. Delete Patient
            7. Statistics
            8. Change Password
            9. Logout

        choice <- ReadMenuChoice(1, 9)

        CASE choice OF
            1: CALL ShowDoctors()
            2: CALL AddDoctor()
            3: CALL EditDoctor()
            4: CALL DeleteDoctor()
            5: CALL ShowPatients()
            6: CALL DeletePatient()
            7: CALL ShowStatistics()
            8: CALL ChangeAdminPassword()
            9: RETURN
        END CASE
    UNTIL FALSE
END
```

```text
PROCEDURE AddDoctor()
BEGIN
    READ username
    READ name
    READ specialization
    READ phone
    READ age
    READ experience
    READ certifications
    READ password

    VALIDATE all inputs
    REQUIRE doctor age between 25 and 75

    CREATE doctor object
    ADD doctor to storage
    SAVE data
END
```

```text
PROCEDURE EditDoctor()
BEGIN
    SELECT target doctor
    READ updated doctor data
    VALIDATE all inputs
    UPDATE doctor object
    SAVE data
END
```

```text
PROCEDURE DeleteDoctor()
BEGIN
    SELECT target doctor
    ASK for confirmation
    IF confirmed THEN
        DELETE doctor
        DELETE all related appointments
        SAVE data
    END IF
END
```

```text
PROCEDURE ShowPatients()
BEGIN
    FOR each patient DO
        DISPLAY patient identity and statistics
    END FOR
END
```

```text
PROCEDURE DeletePatient()
BEGIN
    SELECT target patient
    ASK for confirmation
    IF confirmed THEN
        DELETE patient
        DELETE related appointments
        SAVE data
    END IF
END
```

```text
PROCEDURE ShowStatistics()
BEGIN
    COUNT total doctors
    COUNT total patients
    COUNT total appointments
    COUNT pending appointments
    COUNT completed appointments
    COUNT cancelled appointments
    DISPLAY all statistics
END
```

```text
PROCEDURE ChangeAdminPassword()
BEGIN
    READ old password
    VERIFY old password
    READ new password
    READ confirmation password

    IF new password matches confirmation THEN
        UPDATE stored admin password
        SAVE data
    ELSE
        DISPLAY validation error
    END IF
END
```

---

### 10. Doctor Operational Subsystem

```text
PROCEDURE DoctorDashboard(doctor)
BEGIN
    REPEAT
        DISPLAY doctor profile
        DISPLAY menu
            1. My Schedule
            2. Add Diagnose
            3. Complete Appointment
            4. Cancel Appointment
            5. Search Appointments
            6. Logout

        choice <- ReadMenuChoice(1, 6)

        CASE choice OF
            1: CALL ShowDoctorAppointments(doctor)
            2: CALL DoctorAddDiagnose(doctor)
            3: CALL DoctorCompleteAppointment(doctor)
            4: CALL DoctorCancelAppointment(doctor)
            5: CALL DoctorSearchAppointments(doctor)
            6: RETURN
        END CASE
    UNTIL FALSE
END
```

```text
PROCEDURE ShowDoctorAppointments(doctor)
BEGIN
    DISPLAY all appointments assigned to the doctor
END
```

```text
PROCEDURE DoctorAddDiagnose(doctor)
BEGIN
    SELECT doctor appointment
    READ diagnosis text

    IF diagnosis is valid THEN
        SET appointment diagnosis
        SET appointment status <- Completed
        SAVE data
    ELSE
        DISPLAY validation error
    END IF
END
```

```text
PROCEDURE DoctorCompleteAppointment(doctor)
BEGIN
    SELECT doctor appointment
    SET appointment status <- Completed
    SAVE data
END
```

```text
PROCEDURE DoctorCancelAppointment(doctor)
BEGIN
    SELECT doctor appointment
    ASK for confirmation

    IF confirmed THEN
        SET appointment status <- Cancelled
        SAVE data
    END IF
END
```

```text
PROCEDURE DoctorSearchAppointments(doctor)
BEGIN
    DISPLAY search modes
        1. Patient ID
        2. Patient Name

    mode <- user selection
    value <- search text

    FOR each doctor appointment DO
        IF appointment matches search condition THEN
            DISPLAY appointment details
        END IF
    END FOR
END
```

---

### 11. Reliability and Auto-Save Policy

```text
ON normal exit:
    Save all data

ON application exit event:
    Save all data

ON process exit event:
    Save all data

ON unhandled exception:
    Save all data

AFTER every critical update:
    Save immediately
```

---

### 12. High-Level System Flow

```text
HOME
    ├── Booking Subsystem
    │   ├── Login with ID
    │   ├── New Registration
    │   └── Recover ID
    │       └── Specialty → Doctor → Date/Time → Confirmation
    │
    ├── Patient Subsystem
    │   ├── Authentication
    │   ├── Appointments Review
    │   ├── Appointment Cancellation
    │   └── Account Management
    │
    ├── Staff Subsystem
    │   ├── Admin Authentication
    │   │   └── Administrative Management
    │   └── Doctor Authentication
    │       └── Doctor Operational Dashboard
    │
    └── Exit
```

