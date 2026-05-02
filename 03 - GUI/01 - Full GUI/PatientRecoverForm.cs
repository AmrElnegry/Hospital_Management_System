using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Hospital
{
    public class PatientRecoverForm : Form
    {
        TextBox txtName;
        TextBox txtPassword;
        public string RecoveredId { get; private set; }

        public PatientRecoverForm()
        {
            Text = "Recover Patient ID";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(800, 420);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.AliceBlue;

            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = "Recover Patient ID",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Size = new Size(500, 40),
                Location = new Point(150, 35),
                TextAlign = ContentAlignment.MiddleCenter
            });

            panel.Controls.Add(CreateLabel("Full Name", 120, 120));
            txtName = CreateTextBox(120, 150);
            panel.Controls.Add(txtName);

            panel.Controls.Add(CreateLabel("Password", 120, 215));
            txtPassword = CreateTextBox(120, 245);
            txtPassword.PasswordChar = '*';
            panel.Controls.Add(txtPassword);

            Button btnRecover = CreateButton("Recover ID", Color.MediumSeaGreen, new Point(205, 320));
            Button btnBack = CreateButton("Back", Color.DimGray, new Point(415, 320));
            btnRecover.Click += btnRecover_Click;
            btnBack.Click += delegate { Close(); };

            panel.Controls.AddRange(new Control[] { btnRecover, btnBack });
            Controls.Add(panel);
        }

        Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        TextBox CreateTextBox(int x, int y)
        {
            return new TextBox { Font = new Font("Segoe UI", 11), Size = new Size(540, 34), Location = new Point(x, y) };
        }

        Button CreateButton(string text, Color color, Point location)
        {
            return new Button { Text = text, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(180, 48), Location = location };
        }

        void btnRecover_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string password = txtPassword.Text;

            if (!ValidationHelper.IsValidFullName(name))
            {
                MessageBox.Show("Please enter a valid full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            int index = DB.FindPatientByNameAndPassword(name, password);
            if (index == -1)
            {
                MessageBox.Show("Patient record not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RecoveredId = DB.Patients[index].PatientID;
            MessageBox.Show("Your patient ID is: " + RecoveredId, "Recovered", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
