using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class DoctorForm : Form
    {
        readonly Doctor currentDoctor;
        DataGridView dgvSchedule;
        DataGridView dgvActions;
        DataGridView dgvSearch;
        TextBox txtDiagnosis;
        TextBox txtSearch;
        ComboBox cmbSearchMode;
        Label lblSelectedAppointment;

        public DoctorForm(Doctor doctor)
        {
            currentDoctor = doctor;

            Text = "Doctor Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(1140, 670),
                Location = new Point(30, 25),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Doctor Dashboard",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(350, 40),
                Location = new Point(394, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Welcome " + currentDoctor.GetName() + " | Specialization: " + currentDoctor.Specialty + " | ID: " + currentDoctor.DoctorID + " | Username: " + currentDoctor.Username,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.MediumSeaGreen,
                Size = new Size(980, 24),
                Location = new Point(80, 68),
                TextAlign = ContentAlignment.MiddleCenter
            });

            TabControl tabs = new TabControl
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(1080, 520),
                Location = new Point(30, 110)
            };

            tabs.TabPages.Add(CreateScheduleTab());
            tabs.TabPages.Add(CreateActionsTab());
            tabs.TabPages.Add(CreateSearchTab());

            Button btnLogout = new Button
            {
                Text = "Logout",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 42),
                Location = new Point(930, 20)
            };
            btnLogout.Click += delegate { Close(); };

            panel.Controls.Add(tabs);
            panel.Controls.Add(btnLogout);
            Controls.Add(panel);

            LoadScheduleGrid();
            LoadActionsGrid();
        }

        TabPage CreateScheduleTab()
        {
            TabPage tab = new TabPage("My Schedule");
            dgvSchedule = CreateAppointmentGrid(new Point(25, 25), new Size(1020, 420));
            tab.Controls.Add(dgvSchedule);
            return tab;
        }

        TabPage CreateActionsTab()
        {
            TabPage tab = new TabPage("Actions");
            tab.Controls.Add(new Label { Text = "Select Appointment", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(35, 20) });
            dgvActions = CreateAppointmentGrid(new Point(35, 50), new Size(1010, 210));
            dgvActions.SelectionChanged += delegate { UpdateSelectedAppointmentLabel(); };
            tab.Controls.Add(dgvActions);

            lblSelectedAppointment = new Label
            {
                Text = "Selected Appointment: None",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Size = new Size(800, 24),
                Location = new Point(35, 275)
            };
            tab.Controls.Add(lblSelectedAppointment);

            tab.Controls.Add(new Label { Text = "Diagnosis", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(35, 315) });
            txtDiagnosis = new TextBox { Font = new Font("Segoe UI", 10), Multiline = true, Size = new Size(620, 110), Location = new Point(35, 345) };
            tab.Controls.Add(txtDiagnosis);

            Button btnCompleteDiag = CreateButton("Add Diagnosis", Color.MediumSeaGreen, new Point(700, 350), new Size(170, 44));
            Button btnCompleteOnly = CreateButton("Complete", Color.SteelBlue, new Point(700, 405), new Size(170, 44));
            Button btnCancelAppointment = CreateButton("Cancel", Color.Crimson, new Point(885, 350), new Size(160, 44));
            Button btnCancel = CreateButton("Clear", Color.DimGray, new Point(885, 405), new Size(160, 44));

            btnCompleteDiag.Click += btnCompleteDiagnose_Click;
            btnCompleteOnly.Click += btnCompleteAppointment_Click;
            btnCancelAppointment.Click += btnCancelAppointment_Click;
            btnCancel.Click += delegate { txtDiagnosis.Clear(); };

            tab.Controls.AddRange(new Control[] { btnCompleteDiag, btnCompleteOnly, btnCancelAppointment, btnCancel });
            return tab;
        }

        TabPage CreateSearchTab()
        {
            TabPage tab = new TabPage("Search");
            tab.Controls.Add(new Label { Text = "Search By", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(35, 35) });
            cmbSearchMode = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), Size = new Size(150, 31), Location = new Point(35, 65) };
            cmbSearchMode.Items.AddRange(new object[] { "Patient ID", "Patient Name" });
            cmbSearchMode.SelectedIndex = 0;
            tab.Controls.Add(cmbSearchMode);

            tab.Controls.Add(new Label { Text = "Value", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(205, 35) });
            txtSearch = new TextBox { Font = new Font("Segoe UI", 10), Size = new Size(340, 30), Location = new Point(205, 65) };
            tab.Controls.Add(txtSearch);

            Button btnSearch = CreateButton("Search", Color.MediumSeaGreen, new Point(565, 58), new Size(140, 42));
            btnSearch.Click += btnSearch_Click;
            tab.Controls.Add(btnSearch);

            dgvSearch = CreateAppointmentGrid(new Point(35, 125), new Size(1010, 320));
            tab.Controls.Add(dgvSearch);
            return tab;
        }

        DataGridView CreateAppointmentGrid(Point location, Size size)
        {
            DataGridView grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Location = location,
                Size = size
            };
            grid.Columns.Add("colAppointmentID", "Appointment ID");
            grid.Columns.Add("colPatientID", "Patient ID");
            grid.Columns.Add("colPatientName", "Patient Name");
            grid.Columns.Add("colDate", "Date / Time");
            grid.Columns.Add("colStatus", "Status");
            grid.Columns.Add("colDiagnosis", "Diagnosis");
            return grid;
        }

        Button CreateButton(string text, Color color, Point location, Size size)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = location,
                Size = size
            };
        }

        void LoadScheduleGrid()
        {
            FillDoctorAppointmentsGrid(dgvSchedule);
        }

        void LoadActionsGrid()
        {
            FillDoctorAppointmentsGrid(dgvActions);
            UpdateSelectedAppointmentLabel();
        }

        void FillDoctorAppointmentsGrid(DataGridView grid)
        {
            if (grid == null) return;
            grid.Rows.Clear();
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                Appointment appointment = DB.Appointments[i];
                if (appointment == null || appointment.DoctorID != currentDoctor.DoctorID) continue;

                string patientName = "-";
                int patientIndex = DB.FindPatientByID(appointment.PatientID);
                if (patientIndex != -1) patientName = DB.Patients[patientIndex].GetName();

                grid.Rows.Add(
                    appointment.AppointmentID,
                    appointment.PatientID,
                    patientName,
                    appointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm"),
                    appointment.Status.ToString(),
                    string.IsNullOrWhiteSpace(appointment.Diagnosis) ? "-" : appointment.Diagnosis);
            }
        }

        int GetSelectedActionAppointmentIndex()
        {
            if (dgvActions.SelectedRows.Count == 0) return -1;
            string appointmentId = dgvActions.SelectedRows[0].Cells["colAppointmentID"].Value?.ToString() ?? "";
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                if (DB.Appointments[i] != null &&
                    DB.Appointments[i].DoctorID == currentDoctor.DoctorID &&
                    DB.Appointments[i].AppointmentID.Equals(appointmentId, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        void UpdateSelectedAppointmentLabel()
        {
            if (lblSelectedAppointment == null) return;
            if (dgvActions == null || dgvActions.SelectedRows.Count == 0)
            {
                lblSelectedAppointment.Text = "Selected Appointment: None";
                return;
            }

            string appointmentId = dgvActions.SelectedRows[0].Cells["colAppointmentID"].Value?.ToString() ?? "-";
            string patientId = dgvActions.SelectedRows[0].Cells["colPatientID"].Value?.ToString() ?? "-";
            lblSelectedAppointment.Text = "Selected Appointment: " + appointmentId + " | Patient: " + patientId;
        }

        void RefreshDoctorData()
        {
            LoadScheduleGrid();
            LoadActionsGrid();
            txtDiagnosis.Clear();
        }

        void btnCompleteDiagnose_Click(object sender, EventArgs e)
        {
            int appointmentIndex = GetSelectedActionAppointmentIndex();
            if (appointmentIndex == -1)
            {
                MessageBox.Show("Please select an appointment from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string diagnosis = txtDiagnosis.Text.Trim();
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                MessageBox.Show("Diagnosis is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DB.Appointments[appointmentIndex].Diagnosis = diagnosis;
            DB.Appointments[appointmentIndex].Status = AppointmentStatus.Completed;
            DB.Save();
            RefreshDoctorData();
        }

        void btnCompleteAppointment_Click(object sender, EventArgs e)
        {
            int appointmentIndex = GetSelectedActionAppointmentIndex();
            if (appointmentIndex == -1)
            {
                MessageBox.Show("Please select an appointment from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DB.Appointments[appointmentIndex].Status = AppointmentStatus.Completed;
            DB.Save();
            RefreshDoctorData();
        }

        void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            int appointmentIndex = GetSelectedActionAppointmentIndex();
            if (appointmentIndex == -1)
            {
                MessageBox.Show("Please select an appointment from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this appointment?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            DB.Appointments[appointmentIndex].Status = AppointmentStatus.Cancelled;
            DB.Save();
            RefreshDoctorData();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string value = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("Enter a search value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvSearch.Rows.Clear();
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                Appointment appointment = DB.Appointments[i];
                if (appointment == null || appointment.DoctorID != currentDoctor.DoctorID) continue;

                int patientIndex = DB.FindPatientByID(appointment.PatientID);
                string patientName = patientIndex != -1 ? DB.Patients[patientIndex].GetName() : "-";

                bool match = false;
                if (cmbSearchMode.Text == "Patient ID")
                    match = appointment.PatientID.Equals(value, StringComparison.OrdinalIgnoreCase);
                else if (patientIndex != -1)
                    match = DB.Patients[patientIndex].GetName().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

                if (!match) continue;

                dgvSearch.Rows.Add(
                    appointment.AppointmentID,
                    appointment.PatientID,
                    patientName,
                    appointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm"),
                    appointment.Status.ToString(),
                    string.IsNullOrWhiteSpace(appointment.Diagnosis) ? "-" : appointment.Diagnosis);
            }
        }
    }
}
