## **Hospital Management System**

## **Full Professional Pseudocode**





|**🔷 ENUMS :**|
|-|
|<br />Enum AppointmentStatus<br />    Pending<br />    Completed<br />    Cancelled<br />End Enum<br />|



|**🔷 BASE CLASS** **:**|
|-|
|<br />Class Person<br /><br />    Private name<br />    Private phone<br /><br />    Method SetName(name)<br />        this.name = name<br />    End<br /><br />    Method GetName()<br />        Return this.name<br />    End<br /><br />    Method SetPhone(phone)<br />        this.phone = phone<br />    End<br /><br />    Method GetPhone()<br />        Return this.phone<br />    End<br /><br />End Class<br />|



|**🔷 PATIENT CLASS** **:**|
|-|
|<br />Class Patient Inherits Person<br /><br />    Private patientID<br />    Private age<br />    Private gender<br />    Private complaint<br /><br />    Method CreatePatient(name, age, gender, phone, complaint)<br /><br />        this.patientID = IDGenerator.GeneratePatientID()<br />        SetName(name)<br />        SetPhone(phone)<br />        this.age = age<br />        this.gender = gender<br />        this.complaint = complaint<br /><br />        Return this<br /><br />    End<br /><br />    Method GetPatientData()<br />        Return (patientID, GetName(), age, gender, complaint, GetPhone())<br />    End<br /><br />End Class<br />|



|**🔷 DOCTOR CLASS** **:**|
|-|
|<br />Class Doctor Inherits Person<br /><br />    Private doctorID<br />    Private specialty<br />    Private username<br />    Private password<br />    Private schedule\[]<br /><br />    Method CreateDoctor(name, username, password, specialty, phone)<br /><br />        this.doctorID = IDGenerator.GenerateDoctorID()<br />        this.username = ToLower(username)<br />        this.password = password<br /><br />        SetName(name)<br />        SetPhone(phone)<br />        this.specialty = specialty<br /><br />        Initialize schedule as empty list<br /><br />        Return this<br /><br />    End<br /><br />    Method GetDoctorData()<br />        Return (doctorID, GetName(), specialty, username)<br />    End<br /><br />    Method GetDoctorSchedule()<br />        Return schedule<br />    End<br /><br />    Method ValidateDoctorCredentials(username, password)<br /><br />        If ToLower(username) == this.username AND password == this.password Then<br />            Return True<br />        End If<br /><br />        Return False<br /><br />    End<br /><br />    Method IsAvailable(timeSlot)<br /><br />        For each appointment in schedule<br />            If appointment.timeSlot == timeSlot Then<br />                Return False<br />            End If<br />        End For<br /><br />        Return True<br /><br />    End<br /><br />End Class<br />|



|**🔷 APPOINTMENT CLASS** **:**|
|-|
|<br />Class Appointment<br /><br />    Private appointmentID<br />    Private patient<br />    Private doctor<br />    Private date<br />    Private timeSlot<br />    Private status<br />    Private diagnosis<br /><br />    Method CreateAppointment(patient, doctor, date, timeSlot)<br /><br />        this.appointmentID = IDGenerator.GenerateAppointmentID()<br />        this.patient = patient<br />        this.doctor = doctor<br />        this.date = date<br />        this.timeSlot = timeSlot<br />        this.status = AppointmentStatus.Pending<br />        this.diagnosis = null<br /><br />        Return this<br /><br />    End<br /><br />    Method LinkPatientToDoctor()<br />        Add this to doctor.schedule<br />    End<br /><br />    Method SetAppointmentStatus(newStatus)<br />        this.status = newStatus<br />    End<br /><br />    Method AddDiagnosis(text)<br />        this.diagnosis = text<br />        SetAppointmentStatus(AppointmentStatus.Completed)<br />    End<br /><br />    Method GetAppointmentData()<br />        Return (appointmentID, patient.GetName(), doctor.GetName(), date, timeSlot, status, diagnosis)<br />    End<br /><br />End Class<br />|



|**🔷 ID GENERATOR** **:**|
|-|
|<br />Class IDGenerator<br /><br />    Static patientCounter = 1<br />    Static doctorCounter = 1<br />    Static appointmentCounter = 1<br /><br />    Method GeneratePatientID()<br />        id = "P" + patientCounter<br />        patientCounter++<br />        Return id<br />    End<br /><br />    Method GenerateDoctorID()<br />        id = "D" + doctorCounter<br />        doctorCounter++<br />        Return id<br />    End<br /><br />    Method GenerateAppointmentID()<br />        id = "A" + appointmentCounter<br />        appointmentCounter++<br />        Return id<br />    End<br /><br />End Class<br />|



|**🔷 DATABASE MODULE (CORE LOGIC)** **:**|
|-|
|<br />Class DB<br /><br />    Static doctors\[]<br />    Static patients\[]<br />    Static appointments\[]<br /><br />    Static Times = \["9 AM","10 AM","11 AM","12 PM","1 PM","2 PM","3 PM","4 PM"]<br /><br />    Method Login(username, password)<br /><br />        For each doc in doctors<br />            If doc.ValidateDoctorCredentials(username, password) Then<br />                Return doc<br />            End If<br />        End For<br /><br />        Return null<br /><br />    End<br /><br />    Method AdminLogin(username, password)<br />        Return (username == "admin" AND password == "123")<br />    End<br /><br />    Method AddDoctor(name, username, password, specialty, phone)<br /><br />        For each doc in doctors<br />            If doc.username == ToLower(username) Then<br />                Return False<br />            End If<br />        End For<br /><br />        doctor = new Doctor.CreateDoctor(name, username, password, specialty, phone)<br /><br />        Add doctor to doctors<br /><br />        Return True<br /><br />    End<br /><br />    Method Book(doctorID, patient, timeSlot)<br /><br />        doctor = Find doctor by ID<br /><br />        If doctor == null Then Return False<br /><br />        If doctor.IsAvailable(timeSlot) == False Then<br />            Return False<br />        End If<br /><br />        appointment = new Appointment.CreateAppointment(patient, doctor, Today, timeSlot)<br /><br />        appointment.LinkPatientToDoctor()<br /><br />        Add appointment to appointments<br /><br />        Return True<br /><br />    End<br /><br />    Method GetSpecs()<br /><br />        Create list<br /><br />        For each doc in doctors<br />            Add doc.specialty to list<br />        End For<br /><br />        Remove duplicates (ignore case)<br />        Sort list<br /><br />        Return list<br /><br />    End<br /><br />End Class<br />|



|**🔷 FILE HANDLING** **:**|
|-|
|<br />Function SaveData()<br />    SavePatients()<br />    SaveDoctors()<br />    SaveAppointments()<br />End<br /><br />Function LoadData()<br />    LoadPatients()<br />    LoadDoctors()<br />    LoadAppointments()<br />End<br />|



|**🔷 VALIDATION** **:**|
|-|
|<br />Function ValidatePatientData(name, age)<br /><br />    If name is empty Then Return False<br />    If age < 1 OR age > 120 Then Return False<br /><br />    Return True<br /><br />End<br />|



|**🔷 STATISTICS** **:**|
|-|
|<br />Function CountDoctors()<br />    Return DB.doctors.Count<br />End<br /><br />Function CountAppointments()<br />    Return DB.appointments.Count<br />End<br /><br />Function CountSpecialties()<br />    Return DB.GetSpecs().Count<br />End<br />|



|**🔷 BOOKING FLOW (STEP-BASED)** **:**|
|-|
|<br />Function StartBookingFlow()<br /><br />    // STEP 1<br />    Input complaint OR specialty<br /><br />    // STEP 2<br />    doctors = DB.doctors filtered by specialty<br /><br />    Display doctors<br />    Select doctor<br /><br />    // STEP 3<br />    For each time in DB.Times<br />        If doctor.IsAvailable(time)<br />            Display Available<br />        Else<br />            Display Disabled<br />        End If<br />    End For<br /><br />    Select timeSlot<br /><br />    // STEP 4<br />    Input name, age, gender, phone<br /><br />    If ValidatePatientData(name, age) == False Then<br />        Display Error<br />        Return<br />    End If<br /><br />    patient = new Patient.CreatePatient(name, age, gender, phone, complaint)<br /><br />    success = DB.Book(doctor.ID, patient, timeSlot)<br /><br />    If success Then<br />        Display "Booking Success"<br />    Else<br />        Display "Time already booked"<br />    End If<br /><br />End<br />|



|**🔷 DOCTOR DASHBOARD** **:**|
|-|
|<br />Function DoctorDashboard(doctor)<br /><br />    While True<br /><br />        Input searchQuery<br /><br />        list = doctor.GetDoctorSchedule()<br /><br />        If searchQuery not empty Then<br />            Filter list by patient name<br />        End If<br /><br />        Display list<br /><br />        Input choice<br /><br />        If choice == AddDiagnosis Then<br />            appointment.AddDiagnosis(text)<br />        End If<br /><br />        If choice == Complete Then<br />            appointment.SetAppointmentStatus(Completed)<br />        End If<br /><br />        If choice == Cancel Then<br />            appointment.SetAppointmentStatus(Cancelled)<br />        End If<br /><br />        If choice == Logout Then Break<br /><br />    End While<br /><br />End<br />|



|**🔷 ADMIN DASHBOARD** **:**|
|-|
|<br />Function AdminDashboard()<br /><br />    While True<br /><br />        Display CountDoctors(), CountAppointments(), CountSpecialties()<br /><br />        For each spec in DB.GetSpecs()<br />            Display spec count<br />        End For<br /><br />        Display doctors list<br /><br />        Input choice<br /><br />        If choice == AddDoctor Then<br />            success = DB.AddDoctor(...)<br />            If success == False<br />                Display "Duplicate Username"<br />            End If<br />        End If<br /><br />        If choice == DeleteDoctor Then<br />            Confirm then delete<br />        End If<br /><br />        If choice == ViewAppointments Then<br /><br />            For each doctor in DB.doctors<br />                Display doctor<br />                For each appointment in doctor.schedule<br />                    Display appointment<br />                End For<br />            End For<br /><br />        End If<br /><br />        If choice == Logout Then Break<br /><br />    End While<br /><br />End<br />|



|**🔷 MAIN SYSTEM** **:**|
|-|
|<br />Function Main()<br /><br />    LoadData()<br />    Initialize sample doctors (if empty)<br /><br />    While True<br /><br />        Display "1. Book Appointment"<br />        Display "2. Login"<br /><br />        Input choice<br /><br />        If choice == 1 Then<br />            StartBookingFlow()<br />        End If<br /><br />        If choice == 2 Then<br /><br />            Input username, password<br /><br />            If DB.AdminLogin(username, password) Then<br />                AdminDashboard()<br />            Else<br />                doctor = DB.Login(username, password)<br /><br />                If doctor != null Then<br />                    DoctorDashboard(doctor)<br />                Else<br />                    Display "Invalid Login"<br />                End If<br />            End If<br /><br />        End If<br /><br />    End While<br /><br />End<br />|





