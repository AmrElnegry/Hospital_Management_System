using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class AdminForm : Form
    {
        DataGridView dgvDoctors;
        DataGridView dgvPatients;
        Label lblStats;

        TextBox txtDoctorUsername;
        TextBox txtDoctorName;
        TextBox txtDoctorSpecialty;
        TextBox txtDoctorPhone;
        TextBox txtDoctorAge;
        TextBox txtDoctorExperience;
        TextBox txtDoctorCertifications;
        TextBox txtDoctorPassword;

        TextBox txtOldPassword;
        TextBox txtNewPassword;
        TextBox txtConfirmPassword;

        public AdminForm()
        {
            Text = "Admin Dashboard";
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
                Text = "Admin Dashboard",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(350, 40),
                Location = new Point(394, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Welcome Admin | Manage doctors, patients, statistics, and security settings",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(600, 24),
                Location = new Point(269, 65),
                TextAlign = ContentAlignment.MiddleCenter
            });

            TabControl tabs = new TabControl
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(1080, 515),
                Location = new Point(30, 110)
            };

            tabs.TabPages.Add(CreateDoctorsTab());
            tabs.TabPages.Add(CreatePatientsTab());
            tabs.TabPages.Add(CreateStatisticsTab());
            tabs.TabPages.Add(CreatePasswordTab());

            Button btnLogout = new Button
            {
                Text = "Logout",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(930, 20)
            };
            btnLogout.Click += delegate { Close(); };

            panel.Controls.Add(tabs);
            panel.Controls.Add(btnLogout);
            Controls.Add(panel);

            RefreshAll();
        }

        TabPage CreateDoctorsTab()
        {
            TabPage tab = new TabPage("All Doctors");

            dgvDoctors = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(1020, 230),
                Location = new Point(25, 20)
            };
            dgvDoctors.Columns.Add("colDoctorID", "Doctor ID");
            dgvDoctors.Columns.Add("colDoctorUsername", "Username");
            dgvDoctors.Columns.Add("colDoctorName", "Name");
            dgvDoctors.Columns.Add("colDoctorSpecialty", "Specialization");
            dgvDoctors.Columns.Add("colDoctorPhone", "Phone");
            dgvDoctors.Columns.Add("colDoctorAge", "Age");
            dgvDoctors.Columns.Add("colDoctorExp", "Experience");
            dgvDoctors.Columns.Add("colDoctorCerts", "Certifications");
            tab.Controls.Add(dgvDoctors);

            int x = 130;
            int y = 270;
            tab.Controls.Add(CreateLabel("Username", x, y));
            txtDoctorUsername = CreateTextBox(x, y + 28, 200);
            tab.Controls.Add(txtDoctorUsername);

            x += 215;
            tab.Controls.Add(CreateLabel("Full Name", x, y));
            txtDoctorName = CreateTextBox(x, y + 28, 220);
            tab.Controls.Add(txtDoctorName);

            x += 235;
            tab.Controls.Add(CreateLabel("Specialization", x, y));
            txtDoctorSpecialty = CreateTextBox(x, y + 28, 180);
            tab.Controls.Add(txtDoctorSpecialty);

            x += 195;
            tab.Controls.Add(CreateLabel("Phone", x, y));
            txtDoctorPhone = CreateTextBox(x, y + 28, 170);
            tab.Controls.Add(txtDoctorPhone);

            x = 230;
            y = 345;
            tab.Controls.Add(CreateLabel("Age", x, y));
            txtDoctorAge = CreateTextBox(x, y + 28, 110);
            tab.Controls.Add(txtDoctorAge);

            x += 125;
            tab.Controls.Add(CreateLabel("Experience", x, y));
            txtDoctorExperience = CreateTextBox(x, y + 28, 140);
            tab.Controls.Add(txtDoctorExperience);

            x += 155;
            tab.Controls.Add(CreateLabel("Certifications", x, y));
            txtDoctorCertifications = CreateTextBox(x, y + 28, 140);
            tab.Controls.Add(txtDoctorCertifications);

            x += 155;
            tab.Controls.Add(CreateLabel("Password", x, y));
            txtDoctorPassword = CreateTextBox(x, y + 28, 180);
            txtDoctorPassword.PasswordChar = '*';
            tab.Controls.Add(txtDoctorPassword);

            Button btnAdd = CreateButton("Add Doctor", Color.MediumSeaGreen, new Point(300, 430), new Size(150, 42));
            Button btnEdit = CreateButton("Edit Doctor", Color.SteelBlue, new Point(460, 430), new Size(150, 42));
            Button btnDelete = CreateButton("Delete Doctor", Color.Crimson, new Point(620, 430), new Size(150, 42));
            btnAdd.Click += btnAddDoctor_Click;
            btnEdit.Click += btnEditDoctor_Click;
            btnDelete.Click += btnDeleteDoctor_Click;

            tab.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });
            return tab;
        }

        TabPage CreatePatientsTab()
        {
            TabPage tab = new TabPage("All Patients");
            dgvPatients = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(1020, 350),
                Location = new Point(25, 20)
            };
            dgvPatients.Columns.Add("colPatientID", "Patient ID");
            dgvPatients.Columns.Add("colPatientName", "Name");
            dgvPatients.Columns.Add("colPatientAge", "Age");
            dgvPatients.Columns.Add("colPatientGender", "Gender");
            dgvPatients.Columns.Add("colPatientPhone", "Phone");
            dgvPatients.Columns.Add("colPatientAppointments", "Appointments");
            tab.Controls.Add(dgvPatients);

            Button btnDelete = CreateButton("Delete Patient", Color.Crimson, new Point(465, 395), new Size(150, 42));
            btnDelete.Click += btnDeletePatient_Click;
            tab.Controls.Add(btnDelete);
            return tab;
        }

        TabPage CreateStatisticsTab()
        {
            TabPage tab = new TabPage("Statistics");
            lblStats = new Label
            {
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Size = new Size(900, 320),
                Location = new Point(80, 70)
            };
            tab.Controls.Add(lblStats);
            return tab;
        }

        TabPage CreatePasswordTab()
        {
            TabPage tab = new TabPage("Change Password");
            tab.Controls.Add(CreateLabel("Old Password", 290, 55));
            txtOldPassword = CreateTextBox(290, 85, 500);
            txtOldPassword.PasswordChar = '*';
            tab.Controls.Add(txtOldPassword);

            tab.Controls.Add(CreateLabel("New Password", 290, 165));
            txtNewPassword = CreateTextBox(290, 195, 500);
            txtNewPassword.PasswordChar = '*';
            tab.Controls.Add(txtNewPassword);

            tab.Controls.Add(CreateLabel("Confirm Password", 290, 275));
            txtConfirmPassword = CreateTextBox(290, 305, 500);
            txtConfirmPassword.PasswordChar = '*';
            tab.Controls.Add(txtConfirmPassword);

            Button btnConfirm = CreateButton("Confirm", Color.MediumSeaGreen, new Point(345, 385), new Size(180, 44));
            Button btnCancel = CreateButton("Cancel", Color.DimGray, new Point(555, 385), new Size(180, 44));
            btnConfirm.Click += btnChangePassword_Click;
            btnCancel.Click += delegate
            {
                txtOldPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
            };
            tab.Controls.AddRange(new Control[] { btnConfirm, btnCancel });
            return tab;
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        TextBox CreateTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(width, 30),
                Location = new Point(x, y)
            };
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

        void RefreshAll()
        {
            LoadDoctorsGrid();
            LoadPatientsGrid();
            LoadStatistics();
        }

        void LoadDoctorsGrid()
        {
            dgvDoctors.Rows.Clear();
            for (int i = 0; i < DB.DoctorCount; i++)
            {
                Doctor doctor = DB.Doctors[i];
                if (doctor == null) continue;
                dgvDoctors.Rows.Add(doctor.DoctorID, doctor.Username, doctor.GetName(), doctor.Specialty, doctor.GetPhone(), doctor.Age, doctor.ExperienceYears, doctor.CertificationsCount);
            }
        }

        void LoadPatientsGrid()
        {
            dgvPatients.Rows.Clear();
            for (int i = 0; i < DB.PatientCount; i++)
            {
                Patient patient = DB.Patients[i];
                if (patient == null) continue;
                dgvPatients.Rows.Add(patient.PatientID, patient.GetName(), patient.Age, patient.Gender, patient.GetPhone(), DB.CountAppointmentsForPatient(patient.PatientID));
            }
        }

        void LoadStatistics()
        {
            int pending = 0;
            int completed = 0;
            int cancelled = 0;
            for (int i = 0; i < DB.AppointmentCount; i++)
            {
                if (DB.Appointments[i] == null) continue;
                if (DB.Appointments[i].Status == AppointmentStatus.Pending) pending++;
                else if (DB.Appointments[i].Status == AppointmentStatus.Completed) completed++;
                else if (DB.Appointments[i].Status == AppointmentStatus.Cancelled) cancelled++;
            }

            lblStats.Text =
                Environment.NewLine +
                "Total Doctors: " + DB.DoctorCount + Environment.NewLine + 
                "Total Patients: " + DB.PatientCount + Environment.NewLine + 
                "Total Appointments: " + DB.AppointmentCount + Environment.NewLine + 
                "Pending Appointments: " + pending + Environment.NewLine + 
                "Completed Appointments: " + completed + Environment.NewLine + 
                "Cancelled Appointments: " + cancelled + Environment.NewLine + 
                "Admin Password Protected: Yes" + Environment.NewLine +
                "Data Storage: Local text files";
        }

        void btnAddDoctor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidFullName(txtDoctorName.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid doctor full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidTextOnly(txtDoctorSpecialty.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid specialization.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidEgyptianPhone(txtDoctorPhone.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtDoctorAge.Text.Trim(), out int age) || age < 25 || age > 75)
            {
                MessageBox.Show("Doctor age must be between 25 and 75.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtDoctorExperience.Text.Trim(), out int exp) || exp < 0)
            {
                MessageBox.Show("Please enter a valid experience value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtDoctorCertifications.Text.Trim(), out int certs) || certs < 0)
            {
                MessageBox.Show("Please enter a valid certifications value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDoctorPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullName = txtDoctorName.Text.Trim();
            if (!fullName.StartsWith("Dr. ", StringComparison.OrdinalIgnoreCase))
                fullName = "Dr. " + fullName;

            Doctor doctor = new Doctor();
            doctor.CreateDoctor(DB.GenerateMixedID(), txtDoctorUsername.Text.Trim(), fullName, txtDoctorSpecialty.Text.Trim(), txtDoctorPhone.Text.Trim(), txtDoctorPassword.Text, age, exp, certs);
            DB.Doctors[DB.DoctorCount++] = doctor;
            DB.Save();
            ClearDoctorInputs();
            RefreshAll();
            MessageBox.Show("Doctor added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void btnEditDoctor_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string doctorId = dgvDoctors.SelectedRows[0].Cells["colDoctorID"].Value?.ToString() ?? "";
            int index = DB.FindDoctorByID(doctorId);
            if (index == -1)
            {
                MessageBox.Show("Doctor not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Doctor doctor = DB.Doctors[index];
            using (DoctorEditForm form = new DoctorEditForm(doctor))
            {
                form.ShowDialog();
            }
            DB.Save();
            RefreshAll();
        }

        void btnDeleteDoctor_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string doctorId = dgvDoctors.SelectedRows[0].Cells["colDoctorID"].Value?.ToString() ?? "";
            int index = DB.FindDoctorByID(doctorId);
            if (index == -1)
            {
                MessageBox.Show("Doctor not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Delete selected doctor and all related appointments?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            DB.DeleteDoctorByIndex(index);
            DB.Save();
            RefreshAll();
        }

        void btnDeletePatient_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string patientId = dgvPatients.SelectedRows[0].Cells["colPatientID"].Value?.ToString() ?? "";
            int index = DB.FindPatientByID(patientId);
            if (index == -1)
            {
                MessageBox.Show("Patient not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Delete selected patient and all related appointments?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            DB.DeletePatientByIndex(index);
            DB.Save();
            RefreshAll();
        }

        void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text != DB.AdminPassword)
            {
                MessageBox.Show("Old password is incorrect.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("New password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password confirmation does not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DB.AdminPassword = txtNewPassword.Text;
            DB.Save();
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            MessageBox.Show("Admin password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void ClearDoctorInputs()
        {
            txtDoctorUsername.Clear();
            txtDoctorName.Clear();
            txtDoctorSpecialty.Clear();
            txtDoctorPhone.Clear();
            txtDoctorAge.Clear();
            txtDoctorExperience.Clear();
            txtDoctorCertifications.Clear();
            txtDoctorPassword.Clear();
        }
    }
}
