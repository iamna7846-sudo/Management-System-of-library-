using System;
using System.Drawing;
using System.Windows.Forms;

namespace Libraray_manangement_system
{
    public partial class AdminSetupForm : Form
    {
        private readonly AuthService authService = new AuthService();


    private TextBox usernameBox = new TextBox();
        private TextBox passwordBox = new TextBox();
        private TextBox confirmPasswordBox = new TextBox();

        public AdminSetupForm()
        {
            InitializeComponent();

            Text = "Create Admin Account";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterScreen;

            Label usernameLabel = new Label
            {
                Text = "Username",
                Location = new Point(30, 25),
                AutoSize = true
            };

            usernameBox.Location = new Point(150, 22);
            usernameBox.Width = 190;

            Label passwordLabel = new Label
            {
                Text = "Password (minimum 12 characters)",
                Location = new Point(30, 65),
                AutoSize = true
            };

            passwordBox.Location = new Point(150, 90);
            passwordBox.Width = 190;
            passwordBox.UseSystemPasswordChar = true;

            Label confirmLabel = new Label
            {
                Text = "Confirm Password",
                Location = new Point(30, 130),
                AutoSize = true
            };

            confirmPasswordBox.Location = new Point(150, 155);
            confirmPasswordBox.Width = 190;
            confirmPasswordBox.UseSystemPasswordChar = true;

            Button createButton = new Button
            {
                Text = "Create Admin",
                Location = new Point(150, 200),
                Width = 140
            };

            createButton.Click += CreateButton_Click;

            Controls.Add(usernameLabel);
            Controls.Add(usernameBox);
            Controls.Add(passwordLabel);
            Controls.Add(passwordBox);
            Controls.Add(confirmLabel);
            Controls.Add(confirmPasswordBox);
            Controls.Add(createButton);
        }

        private void CreateButton_Click(object? sender, EventArgs e)
        {
            if (passwordBox.Text != confirmPasswordBox.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                authService.CreateAdmin(
                    usernameBox.Text.Trim(),
                    passwordBox.Text);

                MessageBox.Show(
                    "Admin account created successfully. You can now log in.");

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Admin Setup");
            }
        }
    }


}
