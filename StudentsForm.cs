
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Libraray_manangement_system.Data;
using Libraray_manangement_system.Models;

namespace Libraray_manangement_system
{
    public partial class StudentsForm : Form
    {
        private TextBox studentIdBox;
        private TextBox studentNameBox;
        private TextBox departmentBox;
        private TextBox searchBox;
        private ListBox studentsList;

        private readonly StudentRepository studentRepository =
            new StudentRepository();

        public StudentsForm()
        {
            InitializeComponent();

            Text = "Manage Students";
            Size = new Size(650, 530);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label heading = new Label();
            heading.Text = "Manage Library Students";
            heading.Font = new Font("Arial", 18, FontStyle.Bold);
            heading.AutoSize = true;
            heading.Location = new Point(155, 20);
            Controls.Add(heading);

            Label idLabel = new Label();
            idLabel.Text = "Student ID:";
            idLabel.Location = new Point(50, 90);
            idLabel.AutoSize = true;
            Controls.Add(idLabel);

            studentIdBox = new TextBox();
            studentIdBox.Location = new Point(160, 85);
            studentIdBox.Width = 200;
            Controls.Add(studentIdBox);

            Label nameLabel = new Label();
            nameLabel.Text = "Student Name:";
            nameLabel.Location = new Point(50, 135);
            nameLabel.AutoSize = true;
            Controls.Add(nameLabel);

            studentNameBox = new TextBox();
            studentNameBox.Location = new Point(160, 130);
            studentNameBox.Width = 200;
            Controls.Add(studentNameBox);

            Label departmentLabel = new Label();
            departmentLabel.Text = "Department:";
            departmentLabel.Location = new Point(50, 180);
            departmentLabel.AutoSize = true;
            Controls.Add(departmentLabel);

            departmentBox = new TextBox();
            departmentBox.Location = new Point(160, 175);
            departmentBox.Width = 200;
            Controls.Add(departmentBox);

            Button addButton = new Button();
            addButton.Text = "Add Student";
            addButton.Location = new Point(160, 220);
            addButton.Size = new Size(110, 35);
            Controls.Add(addButton);

            Button deleteButton = new Button();
            deleteButton.Text = "Delete Selected";
            deleteButton.Location = new Point(290, 220);
            deleteButton.Size = new Size(130, 35);
            Controls.Add(deleteButton);

            studentsList = new ListBox();
            studentsList.Location = new Point(50, 280);
            studentsList.Size = new Size(520, 125);
            Controls.Add(studentsList);

            Label searchLabel = new Label();
            searchLabel.Text = "Search by ID or Name:";
            searchLabel.Location = new Point(50, 430);
            searchLabel.AutoSize = true;
            Controls.Add(searchLabel);

            searchBox = new TextBox();
            searchBox.Location = new Point(210, 425);
            searchBox.Width = 160;
            Controls.Add(searchBox);

            Button searchButton = new Button();
            searchButton.Text = "Search";
            searchButton.Location = new Point(390, 423);
            searchButton.Size = new Size(100, 30);
            Controls.Add(searchButton);

            addButton.Click += AddButton_Click;
            deleteButton.Click += DeleteButton_Click;
            searchButton.Click += SearchButton_Click;

            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                studentsList.Items.Clear();

                List<Student> students = studentRepository.GetAll();

                foreach (Student student in students)
                {
                    studentsList.Items.Add(
                        "ID: " + student.StudentId +
                        " | Name: " + student.Name +
                        " | Department: " + student.Department);
                }
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                MessageBox.Show("Could not load students: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string id = studentIdBox.Text.Trim();
            string name = studentNameBox.Text.Trim();
            string department = departmentBox.Text.Trim();

            if (id == "" || name == "" || department == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            Student student = new Student
            {
                StudentId = id,
                Name = name,
                Department = department
            };

            try
            {
                studentRepository.Add(student);
                LoadStudents();

                studentIdBox.Clear();
                studentNameBox.Clear();
                departmentBox.Clear();

                MessageBox.Show("Student added successfully!");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                MessageBox.Show("Could not save student: " + ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (studentsList.SelectedItem == null)
            {
                MessageBox.Show("Please select a student first.");
                return;
            }

            string selectedStudent = studentsList.SelectedItem.ToString() ?? "";
            int separatorIndex = selectedStudent.IndexOf(" | ");

            if (separatorIndex < 0)
            {
                MessageBox.Show("Could not identify the selected student.");
                return;
            }

            string studentId = selectedStudent.Substring(4, separatorIndex - 4);

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                studentRepository.Delete(studentId);
                LoadStudents();
                MessageBox.Show("Student deleted successfully!");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                MessageBox.Show("Could not delete student: " + ex.Message);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchText = searchBox.Text.Trim();

            if (searchText == "")
            {
                MessageBox.Show("Please enter Student ID or Name.");
                return;
            }

            for (int i = 0; i < studentsList.Items.Count; i++)
            {
                string student = studentsList.Items[i].ToString() ?? "";

                if (student.IndexOf(
                    searchText,
                    StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    studentsList.SelectedIndex = i;
                    MessageBox.Show("Student found:\n" + student);
                    return;
                }
            }

            studentsList.ClearSelected();
            MessageBox.Show("Student not found.");
        }
    }
}

