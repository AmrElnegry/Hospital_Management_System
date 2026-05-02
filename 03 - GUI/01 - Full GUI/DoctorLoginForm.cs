using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class DoctorLoginForm : Form
    {
        TextBox txtUsername;
        TextBox txtPassword;

        public DoctorLoginForm()
        {
            Text = "Doctor Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(760, 440),
                Location = new Point(220, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Doctor Login",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(560, 45),
                Location = new Point(100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(CreateLabel("Username", 145, 145));
            txtUsername = CreateTextBox(145, 175);
            panel.Controls.Add(txtUsername);

            panel.Controls.Add(CreateLabel("Password", 145, 245));
            txtPassword = CreateTextBox(145, 275);
            txtPassword.PasswordChar = '*';
            panel.Controls.Add(txtPassword);

            Button btnLogin = CreateButton("Login", Color.MediumSeaGreen, new Point(145, 350));
            Button btnBack = CreateButton("Back", Color.DimGray, new Point(355, 350));

            btnLogin.Click += btnLogin_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnLogin, btnBack });
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
                Size = new Size(180, 48),
                Location = location
            };
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            int doctorIndex = DB.FindDoctorByLogin(username, password);
            if (doctorIndex == -1)
            {
                MessageBox.Show("Doctor credentials not recognized.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (DoctorForm form = new DoctorForm(DB.Doctors[doctorIndex]))
            {
                Hide();
                form.ShowDialog();
                Close();
            }
        }
    }
}
