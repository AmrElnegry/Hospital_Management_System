using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class StaffLoginForm : Form
    {
        public StaffLoginForm()
        {
            Text = "Staff Login";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(900, 560),
                Location = new Point(150, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Staff Login",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(700, 45),
                Location = new Point(100, 40),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Choose the portal you want to enter",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(700, 25),
                Location = new Point(100, 92),
                TextAlign = ContentAlignment.MiddleCenter
            });

            Button btnAdmin = CreateButton("Admin", Color.DodgerBlue, new Point(310, 210));
            Button btnDoctor = CreateButton("Doctor", Color.MediumSeaGreen, new Point(310, 300));

            Button btnBack = new Button
            {
                Text = "Back",
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(180, 45),
                Location = new Point(360, 410)
            };

            btnAdmin.Click += delegate
            {
                using (AdminLoginForm form = new AdminLoginForm())
                {
                    Hide();
                    form.ShowDialog();
                    Show();
                }
            };

            btnDoctor.Click += delegate
            {
                using (DoctorLoginForm form = new DoctorLoginForm())
                {
                    Hide();
                    form.ShowDialog();
                    Show();
                }
            };

            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnAdmin, btnDoctor, btnBack });
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
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(280, 58),
                Location = location
            };
        }
    }
}
