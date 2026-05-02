using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class PatientRegistrationForm : Form
    {
        TextBox txtFullName;
        TextBox txtAge;
        ComboBox cmbGender;
        TextBox txtPhone;
        TextBox txtPassword;

        public Patient RegisteredPatient { get; private set; }

        public PatientRegistrationForm()
        {
            Text = "Patient Registration";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(760, 560),
                Location = new Point(220, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitle = new Label
            {
                Text = "Patient Registration",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(560, 40),
                Location = new Point(100, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblSubtitle = new Label
            {
                Text = "Enter patient information to create a new account",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(560, 24),
                Location = new Point(100, 72),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblSubtitle);

            int labelX = 120;
            int inputX = 270;
            int y = 150;
            int gap = 65;

            panel.Controls.Add(CreateLabel("Full Name", labelX, y));
            txtFullName = CreateTextBox(inputX, y);
            panel.Controls.Add(txtFullName);

            y += gap;
            panel.Controls.Add(CreateLabel("Age", labelX, y));
            txtAge = CreateTextBox(inputX, y);
            panel.Controls.Add(txtAge);

            y += gap;
            panel.Controls.Add(CreateLabel("Gender", labelX, y));
            cmbGender = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Size = new Size(340, 31),
                Location = new Point(inputX, y - 3)
            };
            cmbGender.Items.AddRange(new object[] { "Male", "Female" });
            panel.Controls.Add(cmbGender);

            y += gap;
            panel.Controls.Add(CreateLabel("Phone", labelX, y));
            txtPhone = CreateTextBox(inputX, y);
            panel.Controls.Add(txtPhone);

            y += gap;
            panel.Controls.Add(CreateLabel("Password", labelX, y));
            txtPassword = CreateTextBox(inputX, y);
            txtPassword.PasswordChar = '*';
            panel.Controls.Add(txtPassword);

            Button btnRegister = CreateButton("Register", Color.MediumSeaGreen, new Point(190, 480));
            Button btnBack = CreateButton("Back", Color.DimGray, new Point(400, 480));

            btnRegister.Click += btnRegister_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnRegister, btnBack });
            Controls.Add(panel);
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(x, y + 3),
                AutoSize = true
            };
        }

        TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(340, 30),
                Location = new Point(x, y)
            };
        }

        Button CreateButton(string text, Color color, Point location)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(170, 46),
                Location = location
            };
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string ageText = txtAge.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string gender = cmbGender.Text.Trim();

            if (!ValidationHelper.IsValidFullName(fullName))
            {
                MessageBox.Show("Please enter a valid full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (!int.TryParse(ageText, out int age) || age <= 0)
            {
                MessageBox.Show("Age must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return;
            }

            if (gender != "Male" && gender != "Female")
            {
                MessageBox.Show("Gender must be exactly Male or Female.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGender.Focus();
                return;
            }

            if (!ValidationHelper.IsValidEgyptianPhone(phone))
            {
                MessageBox.Show("Phone must be 11 digits and start with 010, 011, 012, or 015.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (DB.PatientCount >= DB.Patients.Length)
            {
                MessageBox.Show("Patient storage is full.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Patient patient = new Patient();
            patient.CreatePatient(DB.GenerateMixedID(), fullName, age, gender, phone, password);
            DB.Patients[DB.PatientCount++] = patient;
            DB.Save();

            RegisteredPatient = patient;

            MessageBox.Show("Registration successful.\nYour ID is: " + patient.PatientID, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
