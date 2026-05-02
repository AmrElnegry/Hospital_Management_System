using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class DoctorSelectionForm : Form
    {
        readonly Patient currentPatient;
        readonly string selectedSpecialty;
        DataGridView dgvDoctors;
        public Doctor SelectedDoctor { get; private set; }

        public DoctorSelectionForm(Patient patient, string specialty)
        {
            currentPatient = patient;
            selectedSpecialty = specialty;

            Text = "Doctor Selection";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(1120, 620),
                Location = new Point(40, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Doctor Selection",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(980, 40),
                Location = new Point(70, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Choose one doctor from the selected specialty",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(980, 24),
                Location = new Point(70, 65),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Patient: " + currentPatient.GetName() + " | ID: " + currentPatient.PatientID,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Size = new Size(980, 23),
                Location = new Point(70, 95),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Specialty: " + selectedSpecialty,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.MediumSeaGreen,
                Size = new Size(980, 23),
                Location = new Point(70, 120),
                TextAlign = ContentAlignment.MiddleCenter
            });

            dgvDoctors = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(980, 350),
                Location = new Point(70, 165)
            };
            dgvDoctors.Columns.Add("colDoctorID", "Doctor ID");
            dgvDoctors.Columns.Add("colDoctorName", "Doctor Name");
            dgvDoctors.Columns.Add("colSpecialty", "Specialty");
            dgvDoctors.Columns.Add("colExperience", "Experience Years");
            dgvDoctors.Columns.Add("colAge", "Age");
            dgvDoctors.Columns.Add("colCertifications", "Certifications");
            panel.Controls.Add(dgvDoctors);

            Button btnSelect = new Button { Text = "Select Doctor", BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(390, 545) };
            Button btnBack = new Button { Text = "Back", BackColor = Color.DimGray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(640, 545) };

            btnSelect.Click += btnSelect_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnSelect, btnBack });
            Controls.Add(panel);

            LoadDoctors();
        }

        void LoadDoctors()
        {
            dgvDoctors.Rows.Clear();
            for (int i = 0; i < DB.DoctorCount; i++)
            {
                Doctor doctor = DB.Doctors[i];
                if (doctor != null && doctor.Specialty.Equals(selectedSpecialty, StringComparison.OrdinalIgnoreCase))
                {
                    dgvDoctors.Rows.Add(doctor.DoctorID, doctor.GetName(), doctor.Specialty, doctor.ExperienceYears, doctor.Age, doctor.CertificationsCount);
                }
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string doctorId = dgvDoctors.SelectedRows[0].Cells["colDoctorID"].Value?.ToString() ?? "";
            int doctorIndex = DB.FindDoctorByID(doctorId);

            if (doctorIndex == -1)
            {
                MessageBox.Show("Selected doctor was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SelectedDoctor = DB.Doctors[doctorIndex];
            using (AppointmentDateTimeForm form = new AppointmentDateTimeForm(currentPatient, SelectedDoctor))
            {
                Hide();
                form.ShowDialog();
                Close();
            }
        }
    }
}
