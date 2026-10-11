using System;
using System.Windows.Forms;

namespace Libraray_manangement_system
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }


    private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_StyleChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthService authService = new AuthService();

            // Check whether an admin account exists
            if (!authService.HasAdmin())
            {
                using (AdminSetupForm setupForm = new AdminSetupForm())
                {
                    setupForm.ShowDialog();
                }

                // Stop if admin setup was cancelled
                if (!authService.HasAdmin())
                {
                    return;
                }
            }

            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            // Verify username and password
            if (authService.Login(username, password))
            {
                MessageBox.Show("Login Successful!");

                DashboardForm dashboard = new DashboardForm(this);
                dashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }
        }
    }


}
