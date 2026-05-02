using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class AdminLoginForm : Form
    {
        TextBox txtPassword;

        public AdminLoginForm()
        {
            Text = "Admin Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(700, 400),
                Location = new Point(250, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Admin Login",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(500, 45),
                Location = new Point(100, 40),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Admin Password",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(140, 145)
            });

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                PasswordChar = '*',
                Size = new Size(420, 34),
                Location = new Point(140, 175)
            };
            panel.Controls.Add(txtPassword);

            Button btnLogin = CreateButton("Login", Color.MediumSeaGreen, new Point(155, 255));
            Button btnBack = CreateButton("Back", Color.DimGray, new Point(365, 255));
            btnLogin.Click += btnLogin_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnLogin, btnBack });
            Controls.Add(panel);
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
            if (txtPassword.Text == DB.AdminPassword)
            {
                using (AdminForm form = new AdminForm())
                {
                    Hide();
                    form.ShowDialog();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Incorrect admin password.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
