using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class PatientLoginForm : Form
    {
        TextBox txtId;
        TextBox txtPassword;

        public PatientLoginForm()
        {
            Text = "Patient Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(760, 460),
                Location = new Point(220, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Patient Login",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(560, 45),
                Location = new Point(100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(CreateLabel("Patient ID", 145, 130));
            txtId = CreateTextBox(145, 160);
            panel.Controls.Add(txtId);

            panel.Controls.Add(CreateLabel("Password", 145, 225));
            txtPassword = CreateTextBox(145, 255);
            txtPassword.PasswordChar = '*';
            panel.Controls.Add(txtPassword);

            Button btnLogin = new Button
            {
                Text = "Login",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(470, 40),
                Location = new Point(145, 320)
            };

            Button btnForgot = new Button
            {
                Text = "Forgot ID",
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(145, 380)
            };

            Button btnBack = new Button
            {
                Text = "Back",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(395, 380)
            };

            btnLogin.Click += btnLogin_Click;
            btnForgot.Click += delegate
            {
                using (PatientRecoverForm form = new PatientRecoverForm())
                {
                    form.ShowDialog();
                    if (!string.IsNullOrWhiteSpace(form.RecoveredId))
                    {
                        txtId.Text = form.RecoveredId;
                        txtPassword.Focus();
                    }
                }
            };
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnLogin, btnForgot, btnBack });
            Controls.Add(panel);
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(470, 34),
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
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(160, 48),
                Location = location
            };
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter patient ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            int patientIndex = DB.FindPatientByID(id);
            if (patientIndex == -1 || DB.Patients[patientIndex].Password != password)
            {
                MessageBox.Show("Invalid patient ID or password.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (PatientDashboardForm form = new PatientDashboardForm(DB.Patients[patientIndex]))
            {
                Hide();
                form.ShowDialog();
                Close();
            }
        }
    }
}
