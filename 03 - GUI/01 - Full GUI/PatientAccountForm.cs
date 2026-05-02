using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class PatientAccountForm : Form
    {
        Patient currentPatient;
        TextBox txtName;
        TextBox txtAge;
        TextBox txtId;
        TextBox txtPassword;
        Button btnConfirmEdit;
        Button btnCancel;
        Button btnDelete;
        public bool AccountDeleted { get; private set; }

        public PatientAccountForm(Patient patient)
        {
            currentPatient = patient;

            Text = "Manage Account";
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
                Text = "Manage Account",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(560, 45),
                Location = new Point(290, 35),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(CreateLabel("Name", 310, 130));
            txtName = CreateTextBox(310, 160);
            panel.Controls.Add(txtName);

            panel.Controls.Add(CreateLabel("Age", 310, 220));
            txtAge = CreateTextBox(310, 250);
            panel.Controls.Add(txtAge);

            panel.Controls.Add(CreateLabel("ID", 310, 310));
            txtId = CreateTextBox(310, 340);
            txtId.ReadOnly = true;
            panel.Controls.Add(txtId);

            panel.Controls.Add(CreateLabel("Password", 310, 400));
            txtPassword = CreateTextBox(310, 430);
            panel.Controls.Add(txtPassword);

            btnConfirmEdit = CreateButton("Confirm Edit", Color.MediumSeaGreen, new Point(275, 545));
            btnDelete = CreateButton("Delete Account", Color.Crimson, new Point(480, 545));
            btnCancel = CreateButton("Cancel", Color.DimGray, new Point(685, 545));

            btnConfirmEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnCancel.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnConfirmEdit, btnDelete, btnCancel });
            Controls.Add(panel);

            LoadPatient();
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        TextBox CreateTextBox(int x, int y)
        {
            return new TextBox { Font = new Font("Segoe UI", 11), Size = new Size(520, 34), Location = new Point(x, y) };
        }

        Button CreateButton(string text, Color color, Point location)
        {
            return new Button { Text = text, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(180, 48), Location = location };
        }

        void LoadPatient()
        {
            int index = DB.FindPatientByID(currentPatient.PatientID);
            if (index == -1)
            {
                MessageBox.Show("Account not found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            currentPatient = DB.Patients[index];
            txtName.Text = currentPatient.GetName();
            txtAge.Text = currentPatient.Age.ToString();
            txtId.Text = currentPatient.PatientID;
            txtPassword.Text = currentPatient.Password;
        }

        void btnEdit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string ageText = txtAge.Text.Trim();
            string password = txtPassword.Text;

            if (!ValidationHelper.IsValidFullName(name))
            {
                MessageBox.Show("Please enter a valid full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!int.TryParse(ageText, out int age) || age <= 0)
            {
                MessageBox.Show("Age must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            currentPatient.SetName(name);
            currentPatient.Age = age;
            currentPatient.Password = password;
            DB.Save();
            LoadPatient();
            MessageBox.Show("Account updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            int index = DB.FindPatientByID(currentPatient.PatientID);
            if (index == -1)
            {
                MessageBox.Show("Account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DB.DeletePatientByIndex(index);
            DB.Save();
            AccountDeleted = true;
            MessageBox.Show("Account deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
