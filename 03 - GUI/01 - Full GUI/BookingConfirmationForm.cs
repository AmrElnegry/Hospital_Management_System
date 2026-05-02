using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class BookingConfirmationForm : Form
    {
        public BookingConfirmationForm(Appointment appointment, Doctor doctor)
        {
            Text = "Booking Confirmation";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(760, 450),
                Location = new Point(220, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Appointment Confirmed",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(560, 45),
                Location = new Point(100, 35),
                TextAlign = ContentAlignment.MiddleCenter
            });

            int y = 125;
            int gap = 55;
            panel.Controls.Add(CreateInfo("Appointment ID", appointment.AppointmentID, y));
            y += gap;
            panel.Controls.Add(CreateInfo("Doctor Name", doctor.GetName(), y));
            y += gap;
            panel.Controls.Add(CreateInfo("Specialization", doctor.Specialty, y));
            y += gap;
            panel.Controls.Add(CreateInfo("Date / Time", appointment.AppointmentDate.ToString("dd/MM/yyyy HH:mm"), y));

            Button btnOk = new Button
            {
                Text = "OK",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 48),
                Location = new Point(290, 360)
            };
            btnOk.Click += delegate { Close(); };
            panel.Controls.Add(btnOk);

            Controls.Add(panel);
        }

        Control CreateInfo(string title, string value, int y)
        {
            Panel panel = new Panel { Location = new Point(120, y), Size = new Size(520, 34) };
            panel.Controls.Add(new Label { Text = title + ":", Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true, Location = new Point(0, 5) });
            panel.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 11), AutoSize = true, Location = new Point(180, 5) });
            return panel;
        }
    }
}
