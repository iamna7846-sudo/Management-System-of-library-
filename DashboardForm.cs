
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Libraray_manangement_system
{
    public partial class DashboardForm : Form
    {
        private Form1 loginForm;
        public DashboardForm(Form1 form)
        {
            InitializeComponent();
            loginForm = form;


            this.Text = "Library Management Dashboard";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

           
            Label title = new Label();
            title.Text = "Library Management System";
            title.Font = new Font("Arial", 22, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(180, 40);
            this.Controls.Add(title);

           
            Button booksButton = new Button();
            booksButton.Text = "Manage Books";
            booksButton.Size = new Size(200, 50);
            booksButton.Location = new Point(280, 130);
            this.Controls.Add(booksButton);

            booksButton.Click += (sender, e) =>
            {
                BooksForm booksForm = new BooksForm();
                booksForm.ShowDialog();
            };


            Button studentsButton = new Button();
            studentsButton.Text = "Manage Students";
            studentsButton.Size = new Size(200, 50);
            studentsButton.Location = new Point(280, 200);
            this.Controls.Add(studentsButton);



            studentsButton.Click += (sender, e) =>
            {
                StudentsForm studentsForm = new StudentsForm();
                studentsForm.ShowDialog();
            };


            Button logoutButton = new Button();
            logoutButton.Text = "Logout";
            logoutButton.Size = new Size(200, 50);
            logoutButton.Location = new Point(280, 270);
            this.Controls.Add(logoutButton);



            logoutButton.Click += (sender, e) =>
            {
                loginForm.Show();
                this.Close();
            };
        }
    }
}