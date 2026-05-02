using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class MainForm : Form
    {
        Button btnBookAppointment;
        Button btnStaffLogin;
        Button btnPatientRegistration;
        Button btnExit;

        public MainForm()
        {
            Text = "Hospital Management System";
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

            //
            // New Image
            //

            Label lblTitle = new Label
            {
                Text = "Hospital Management System",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(440, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblHint = new Label
            {
                Text = "What would you like to do?",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(700, 30),
                Location = new Point(220, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnBookAppointment = new Button
            {
                Text = "Book Appointment",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(300, 70),
                Location = new Point(420, 255)
            };
            
            Button btnPatientRegistration = new Button
            {
                Text = "Patient Login",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(300, 70),
                Location = new Point(420, 340)
            };

            Button btnStaffLogin = new Button
            {
                Text = "Staff Login",
                BackColor = Color.Wheat,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(930, 20)
            };
            
            Button btnExit = new Button
            {
                Text = "Exit",
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(170, 46),
                Location = new Point(488, 560)
            };

            btnBookAppointment.Click += btnBookAppointment_Click;
            btnStaffLogin.Click += btnStaffLogin_Click;
            btnPatientRegistration.Click += btnPatientRegistration_Click;
            btnExit.Click += btnExit_Click;

            panel.Controls.AddRange(new Control[]
            {
                lblTitle, lblHint, btnBookAppointment, btnStaffLogin, btnPatientRegistration, btnExit
            });

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
                Size = new Size(280, 55),
                Location = location
            };
        }

        void btnBookAppointment_Click(object sender, EventArgs e)
        {
            using (BookingEntryForm form = new BookingEntryForm())
            {
                Hide();
                form.ShowDialog();
                Show();
            }
        }

        void btnStaffLogin_Click(object sender, EventArgs e)
        {
            using (StaffLoginForm form = new StaffLoginForm())
            {
                Hide();
                form.ShowDialog();
                Show();
            }
        }

        void btnPatientRegistration_Click(object sender, EventArgs e)
        {
            using (PatientLoginForm form = new PatientLoginForm())
            {
                Hide();
                form.ShowDialog();
                Show();
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to save data and exit the application?",
                "Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DB.Save();
                Application.Exit();
            }
        }
    }
}
