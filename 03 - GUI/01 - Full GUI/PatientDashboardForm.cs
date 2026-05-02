using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class PatientDashboardForm : Form
    {
        Patient currentPatient;
        DataGridView dgvAppointments;
        Label lblHeader;

        public PatientDashboardForm(Patient patient)
        {
            currentPatient = patient;

            Text = "Patient Dashboard";
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
                Text = "Patient Dashboard",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(500, 40),
                Location = new Point(320, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            lblHeader = new Label
            {
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Size = new Size(900, 30),
                Location = new Point(120, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(lblHeader);

            dgvAppointments = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(1060, 420),
                Location = new Point(40, 125)
            };
            dgvAppointments.Columns.Add("colAppointmentId", "Appointment ID");
            dgvAppointments.Columns.Add("colDoctor", "Doctor");
            dgvAppointments.Columns.Add("colSpecialty", "Specialization");
            dgvAppointments.Columns.Add("colDate", "Date / Time");
            dgvAppointments.Columns.Add("colStatus", "Status");
            dgvAppointments.Columns.Add("colDiagnosis", "Diagnosis");
            panel.Controls.Add(dgvAppointments);

            Button btnCancelAppointment = CreateButton("Cancel that", Color.Crimson, new Point(365, 580));
            Button btnManageAccount = CreateButton("Manage Account", Color.SteelBlue, new Point(585, 580));

            Button btnLogout = new Button
            {
                Text = "Logout",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(920, 20)
            };

            btnCancelAppointment.Click += btnCancelAppointment_Click;
            btnManageAccount.Click += btnManageAccount_Click;
            btnLogout.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnCancelAppointment, btnManageAccount, btnLogout });
            Controls.Add(panel);

            RefreshDashboard();
        }

        Button CreateButton(string text, Color color, Point location)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(190, 48),
                Location = location
            };
        }

        void RefreshDashboard()
        {
            int index = DB.FindPatientByID(currentPatient.PatientID);
            if (index == -1)
            {
                MessageBox.Show("This account no longer exists.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            currentPatient = DB.Patients[index];
            lblHeader.Text = "Welcome " + currentPatient.GetName() + " | ID: " + currentPatient.PatientID + " | Age: " + currentPatient.Age;

            dgvAppointments.Rows.Clear();
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                Appointment appointment = DB.Appointments[i];
                if (appointment == null || appointment.PatientID != currentPatient.PatientID) continue;

                int doctorIndex = DB.FindDoctorByID(appointment.DoctorID);
                string doctorName = doctorIndex != -1 ? DB.Doctors[doctorIndex].GetName() : appointment.DoctorID;
                string specialty = doctorIndex != -1 ? DB.Doctors[doctorIndex].Specialty : "-";
                dgvAppointments.Rows.Add(
                    appointment.AppointmentID,
                    doctorName,
                    specialty,
                    appointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm"),
                    appointment.Status.ToString(),
                    string.IsNullOrWhiteSpace(appointment.Diagnosis) ? "-" : appointment.Diagnosis);
            }
        }

        void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string appointmentId = dgvAppointments.SelectedRows[0].Cells["colAppointmentId"].Value?.ToString() ?? "";
            int appointmentIndex = -1;
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                if (DB.Appointments[i] != null && DB.Appointments[i].AppointmentID == appointmentId)
                {
                    appointmentIndex = i;
                    break;
                }
            }

            if (appointmentIndex == -1)
            {
                MessageBox.Show("Appointment not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DB.Appointments[appointmentIndex].Status != AppointmentStatus.Pending)
            {
                MessageBox.Show("Only pending appointments can be cancelled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this appointment?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            DB.Appointments[appointmentIndex].Status = AppointmentStatus.Cancelled;
            DB.Save();
            RefreshDashboard();
        }

        void btnManageAccount_Click(object sender, EventArgs e)
        {
            using (PatientAccountForm form = new PatientAccountForm(currentPatient))
            {
                Hide();
                form.ShowDialog();
                if (form.AccountDeleted)
                {
                    Close();
                    return;
                }
                Show();
                RefreshDashboard();
            }
        }
    }
}
