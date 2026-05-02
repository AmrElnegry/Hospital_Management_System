using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class AppointmentDateTimeForm : Form
    {
        readonly Patient currentPatient;
        readonly Doctor selectedDoctor;
        readonly string[] Slots =
        {
            "09:00 AM", "10:00 AM", "11:00 AM", "12:00 PM",
            "01:00 PM", "02:00 PM", "03:00 PM", "04:00 PM"
        };

        ListBox lstDays;
        DataGridView dgvSlots;
        Label lblSelectedDay;

        public AppointmentDateTimeForm(Patient patient, Doctor doctor)
        {
            currentPatient = patient;
            selectedDoctor = doctor;

            Text = "Appointment Date and Time";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(1140, 650),
                Location = new Point(30, 30),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Appointment Date and Time",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(1000, 36),
                Location = new Point(70, 15),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Select a day and an available time slot to complete the appointment booking",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(1000, 23),
                Location = new Point(70, 60),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Patient: " + currentPatient.GetName() + " | ID: " + currentPatient.PatientID,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Size = new Size(1000, 23),
                Location = new Point(70, 90),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Doctor: " + selectedDoctor.GetName() + " | Specialty: " + selectedDoctor.Specialty,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.MediumSeaGreen,
                Size = new Size(1000, 23),
                Location = new Point(70, 118),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label { Text = "Upcoming Days", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(70, 165) });
            panel.Controls.Add(new Label { Text = "Time Slot", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(330, 165) });

            lstDays = new ListBox
            {
                Font = new Font("Segoe UI", 10),
                ItemHeight = 23,
                Size = new Size(220, 360),
                Location = new Point(70, 195)
            };
            lstDays.SelectedIndexChanged += delegate { OnSelectedDayChanged(); };
            panel.Controls.Add(lstDays);

            lblSelectedDay = new Label
            {
                Text = "Selected Day:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.SeaGreen,
                Size = new Size(740, 24),
                Location = new Point(330, 165),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(lblSelectedDay);

            dgvSlots = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(740, 350),
                Location = new Point(330, 194)
            };
            dgvSlots.Columns.Add("colNo", "#");
            dgvSlots.Columns.Add("colTime", "Time");
            dgvSlots.Columns.Add("colStatus", "Status");
            dgvSlots.Columns.Add("colDateTime", "Date Time");
            dgvSlots.Columns["colDateTime"].Visible = false;
            panel.Controls.Add(dgvSlots);

            Button btnBook = new Button { Text = "Confirm Booking", BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(330, 575) };
            Button btnBack = new Button { Text = "Back", BackColor = Color.DimGray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(590, 575) };

            btnBook.Click += btnBook_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnBook, btnBack });
            Controls.Add(panel);

            LoadDays();
        }

        void LoadDays()
        {
            lstDays.Items.Clear();
            for (int i = 0; i < 7; i++)
                lstDays.Items.Add(new DayItem(DateTime.Today.AddDays(i)));

            if (lstDays.Items.Count > 0)
                lstDays.SelectedIndex = 0;
        }

        void OnSelectedDayChanged()
        {
            if (lstDays.SelectedItem is DayItem selectedDay)
            {
                lblSelectedDay.Text = "Selected Day: " + selectedDay.DateValue.ToString("dd/MM/yyyy - dddd");
                LoadSlots(selectedDay.DateValue);
            }
        }

        void RefreshSlots()
        {
            if (lstDays.SelectedItem is DayItem selectedDay)
                LoadSlots(selectedDay.DateValue);
        }

        void LoadSlots(DateTime selectedDate)
        {
            dgvSlots.Rows.Clear();
            for (int i = 0; i < Slots.Length; i++)
            {
                DateTime slotTime = DateTime.Parse(selectedDate.ToShortDateString() + " " + Slots[i]);
                string statusText = "Available";
                int apptIndex = DB.FindAppointmentByDoctorAndTime(selectedDoctor.DoctorID, slotTime);

                if (apptIndex != -1)
                {
                    if (DB.Appointments[apptIndex].Status == AppointmentStatus.Pending)
                        statusText = "Taken";
                    else if (DB.Appointments[apptIndex].Status == AppointmentStatus.Cancelled)
                        statusText = "Unavailable";
                    else if (DB.Appointments[apptIndex].Status == AppointmentStatus.Completed)
                        statusText = "Completed";
                }

                dgvSlots.Rows.Add(i + 1, Slots[i], statusText, slotTime.ToString("yyyy-MM-dd HH:mm"));
            }
        }

        void btnBook_Click(object sender, EventArgs e)
        {
            if (lstDays.SelectedItem is not DayItem selectedDay)
            {
                MessageBox.Show("Please select a day first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSlots.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a time slot.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string timeText = dgvSlots.SelectedRows[0].Cells["colTime"].Value?.ToString() ?? "";
            string status = dgvSlots.SelectedRows[0].Cells["colStatus"].Value?.ToString() ?? "";

            if (status == "Taken")
            {
                MessageBox.Show("This slot is already taken by another patient.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (status == "Unavailable")
            {
                MessageBox.Show("This slot is marked as unavailable by the doctor.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (status == "Completed")
            {
                MessageBox.Show("This slot cannot be booked.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime finalTime = DateTime.Parse(selectedDay.DateValue.ToShortDateString() + " " + timeText);
            int existingIndex = DB.FindAppointmentByDoctorAndTime(selectedDoctor.DoctorID, finalTime);

            if (existingIndex == -1)
            {
                if (DB.AppointmentCount >= DB.Appointments.Length)
                {
                    MessageBox.Show("Appointment storage is full.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Appointment appointment = new Appointment();
                appointment.CreateAppointment(DB.GenerateMixedID(), currentPatient.PatientID, selectedDoctor.DoctorID, finalTime, AppointmentStatus.Pending, "");
                DB.Appointments[DB.AppointmentCount++] = appointment;
                DB.Save();

                using (BookingConfirmationForm form = new BookingConfirmationForm(appointment, selectedDoctor))
                {
                    Hide();
                    form.ShowDialog();
                }
                Close();
            }
            else
            {
                Appointment existingAppointment = DB.Appointments[existingIndex];
                if (existingAppointment.Status == AppointmentStatus.Pending)
                    MessageBox.Show("This slot is already taken by another patient.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (existingAppointment.Status == AppointmentStatus.Cancelled)
                    MessageBox.Show("This slot is marked as unavailable by the doctor.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("This slot cannot be booked.", "Unavailable Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        class DayItem
        {
            public DateTime DateValue { get; }
            public DayItem(DateTime date) { DateValue = date; }
            public override string ToString() { return DateValue.ToString("dd/MM/yyyy - dddd"); }
        }
    }
}
