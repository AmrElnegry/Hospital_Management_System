using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class DoctorEditForm : Form
    {
        readonly Doctor doctor;
        TextBox txtUsername;
        TextBox txtName;
        TextBox txtSpecialty;
        TextBox txtPhone;
        TextBox txtAge;
        TextBox txtExperience;
        TextBox txtCertifications;
        TextBox txtPassword;

        public DoctorEditForm(Doctor doctorToEdit)
        {
            doctor = doctorToEdit;

            Text = "Edit Doctor";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 620);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Edit Doctor",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(500, 40),
                Location = new Point(200, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            int x = 130;
            int y = 55;
            int gap = 60;

            panel.Controls.Add(CreateLabel("Username", x, y));
            txtUsername = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtUsername);

            y += gap;
            panel.Controls.Add(CreateLabel("Full Name", x, y));
            txtName = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtName);

            y += gap;
            panel.Controls.Add(CreateLabel("Specialization", x, y));
            txtSpecialty = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtSpecialty);

            y += gap;
            panel.Controls.Add(CreateLabel("Phone", x, y));
            txtPhone = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtPhone);

            y += gap;
            panel.Controls.Add(CreateLabel("Age", x, y));
            txtAge = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtAge);

            y += gap;
            panel.Controls.Add(CreateLabel("Experience", x, y));
            txtExperience = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtExperience);

            y += gap;
            panel.Controls.Add(CreateLabel("Certifications", x, y));
            txtCertifications = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtCertifications);

            y += gap;
            panel.Controls.Add(CreateLabel("Password", x, y));
            txtPassword = CreateTextBox(x, y + 28);
            panel.Controls.Add(txtPassword);

            Button btnConfirm = new Button
            {
                Text = "Confirm",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 48),
                Location = new Point(250, 545)
            };
            btnConfirm.Click += btnConfirm_Click;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 48),
                Location = new Point(470, 545)
            };
            btnCancel.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnConfirm, btnCancel });
            Controls.Add(panel);

            txtUsername.Text = doctor.Username;
            txtName.Text = doctor.GetName();
            txtSpecialty.Text = doctor.Specialty;
            txtPhone.Text = doctor.GetPhone();
            txtAge.Text = doctor.Age.ToString();
            txtExperience.Text = doctor.ExperienceYears.ToString();
            txtCertifications.Text = doctor.CertificationsCount.ToString();
            txtPassword.Text = doctor.Password;
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        TextBox CreateTextBox(int x, int y)
        {
            return new TextBox { Font = new Font("Segoe UI", 10), Size = new Size(630, 30), Location = new Point(x, y) };
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidFullName(txtName.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidTextOnly(txtSpecialty.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid specialization.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidationHelper.IsValidEgyptianPhone(txtPhone.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtAge.Text.Trim(), out int age) || age < 25 || age > 75 ||
                !int.TryParse(txtExperience.Text.Trim(), out int exp) || exp < 0 ||
                !int.TryParse(txtCertifications.Text.Trim(), out int certs) || certs < 0)
            {
                MessageBox.Show("Doctor age must be 25-75 and other numeric values must be valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();
            if (!name.StartsWith("Dr. ", StringComparison.OrdinalIgnoreCase))
                name = "Dr. " + name;

            doctor.Username = txtUsername.Text.Trim();
            doctor.SetName(name);
            doctor.Specialty = txtSpecialty.Text.Trim();
            doctor.SetPhone(txtPhone.Text.Trim());
            doctor.Age = age;
            doctor.ExperienceYears = exp;
            doctor.CertificationsCount = certs;
            doctor.Password = txtPassword.Text;

            MessageBox.Show("Doctor updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
