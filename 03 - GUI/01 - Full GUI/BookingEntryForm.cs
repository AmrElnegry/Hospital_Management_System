using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class BookingEntryForm : Form
    {
        TextBox txtPatientId;
        TextBox txtFullName;
        TextBox txtPassword;

        public Patient SelectedPatient { get; private set; }

        public BookingEntryForm()
        {
            Text = "Booking Entry";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(980, 610),
                Location = new Point(110, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Appointment Entry",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(760, 40),
                Location = new Point(110, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Login using patient ID, or register a new patient",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(760, 23),
                Location = new Point(110, 65),
                TextAlign = ContentAlignment.MiddleCenter
            });

            GroupBox grpLogin = new GroupBox
            {
                Text = "Login with ID",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(760, 130),
                Location = new Point(110, 110)
            };

            grpLogin.Controls.Add(new Label { Text = "Patient ID", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(28, 34) });
            txtPatientId = new TextBox { Font = new Font("Segoe UI", 10), CharacterCasing = CharacterCasing.Upper, Size = new Size(520, 30), Location = new Point(157, 31) };
            Button btnLogin = new Button { Text = "Login", BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(180, 40), Location = new Point(497, 76) };
            btnLogin.Click += btnLoginWithId_Click;
            grpLogin.Controls.AddRange(new Control[] { txtPatientId, btnLogin });

            GroupBox grpRecover = new GroupBox
            {
                Text = "Forget ID",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(760, 190),
                Location = new Point(110, 275)
            };

            grpRecover.Controls.Add(new Label { Text = "Full Name", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(28, 41) });
            txtFullName = new TextBox { Font = new Font("Segoe UI", 10), Size = new Size(520, 30), Location = new Point(157, 38) };
            grpRecover.Controls.Add(new Label { Text = "Password", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Location = new Point(28, 80) });
            txtPassword = new TextBox { Font = new Font("Segoe UI", 10), PasswordChar = '*', Size = new Size(520, 30), Location = new Point(157, 77) };
            Button btnRecover = new Button { Text = "Recover ID", BackColor = Color.SteelBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(180, 40), Location = new Point(497, 122) };
            btnRecover.Click += btnForgetId_Click;
            grpRecover.Controls.AddRange(new Control[] { txtFullName, txtPassword, btnRecover });

            Button btnNewRegistration = new Button { Text = "New Registration", BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 45), Location = new Point(250, 520) };
            Button btnBack = new Button { Text = "Back", BackColor = Color.DimGray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 45), Location = new Point(510, 520) };

            btnNewRegistration.Click += btnNewRegistration_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { grpLogin, grpRecover, btnNewRegistration, btnBack });
            Controls.Add(panel);
        }

        void btnLoginWithId_Click(object sender, EventArgs e)
        {
            string id = txtPatientId.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter the patient ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPatientId.Focus();
                return;
            }

            int index = DB.FindPatientByID(id);
            if (index == -1)
            {
                MessageBox.Show("ID not recognized in the system.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SelectedPatient = DB.Patients[index];
            using (SpecialtySelectionForm form = new SpecialtySelectionForm(SelectedPatient))
            {
                Hide();
                form.ShowDialog();
                Show();
            }
        }

        void btnNewRegistration_Click(object sender, EventArgs e)
        {
            using (PatientRegistrationForm form = new PatientRegistrationForm())
            {
                form.ShowDialog();
                if (form.RegisteredPatient != null)
                {
                    SelectedPatient = form.RegisteredPatient;
                    using (SpecialtySelectionForm nextForm = new SpecialtySelectionForm(SelectedPatient))
                    {
                        Hide();
                        nextForm.ShowDialog();
                        Close();
                    }
                }
            }
        }

        void btnForgetId_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string password = txtPassword.Text;

            if (!ValidationHelper.IsValidFullName(fullName))
            {
                MessageBox.Show("Please enter a valid full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter the password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            int index = DB.FindPatientByNameAndPassword(fullName, password);
            if (index == -1)
            {
                MessageBox.Show("Identity not found. Please check your credentials.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SelectedPatient = DB.Patients[index];
            txtPatientId.Text = SelectedPatient.PatientID;

            MessageBox.Show("ID recovered successfully.\nYour ID is: " + SelectedPatient.PatientID, "Recovered", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (SpecialtySelectionForm form = new SpecialtySelectionForm(SelectedPatient))
            {
                Hide();
                form.ShowDialog();
                Close();
            }
        }
    }
}
