using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class SpecialtySelectionForm : Form
    {
        readonly Patient currentPatient;
        ListBox lstSpecialties;
        public string SelectedSpecialty { get; private set; }

        public SpecialtySelectionForm(Patient patient)
        {
            currentPatient = patient;

            Text = "Specialty Selection";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 730);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Size = new Size(900, 580),
                Location = new Point(150, 70),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Specialty Selection",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(760, 40),
                Location = new Point(70, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Choose a medical specialty to continue booking",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Size = new Size(760, 23),
                Location = new Point(70, 65),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Patient: " + currentPatient.GetName() + " | ID: " + currentPatient.PatientID,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                Size = new Size(760, 24),
                Location = new Point(70, 95),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(new Label
            {
                Text = "Available Specialties",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(70, 140)
            });

            lstSpecialties = new ListBox
            {
                Font = new Font("Segoe UI", 10),
                ItemHeight = 23,
                Size = new Size(760, 290),
                Location = new Point(70, 175)
            };
            panel.Controls.Add(lstSpecialties);

            Button btnSelect = new Button { Text = "Select Specialty", BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(250, 500) };
            Button btnBack = new Button { Text = "Back", BackColor = Color.DimGray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(220, 46), Location = new Point(500, 500) };

            btnSelect.Click += btnSelect_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnSelect, btnBack });
            Controls.Add(panel);

            LoadSpecialties();
        }

        void LoadSpecialties()
        {
            lstSpecialties.Items.Clear();
            string[] specialties = new string[100];
            DB.GetDistinctSpecialties(specialties, out int count);
            for (int i = 0; i < count; i++)
                lstSpecialties.Items.Add(specialties[i]);

            if (lstSpecialties.Items.Count > 0)
                lstSpecialties.SelectedIndex = 0;
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            if (lstSpecialties.SelectedItem == null)
            {
                MessageBox.Show("Please select a specialty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedSpecialty = lstSpecialties.SelectedItem.ToString();
            using (DoctorSelectionForm form = new DoctorSelectionForm(currentPatient, SelectedSpecialty))
            {
                Hide();
                form.ShowDialog();
                Close();
            }
        }
    }
}
