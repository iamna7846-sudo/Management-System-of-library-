
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Libraray_manangement_system.Data;
using Libraray_manangement_system.Models;

namespace Libraray_manangement_system
{
    public partial class BooksForm : Form
    {
        private TextBox bookIdBox;
        private TextBox titleBox;
        private TextBox authorBox;
        private TextBox searchBox;
        private ListBox booksList;

        // Data layer repository
        private readonly BookRepository bookRepository =
            new BookRepository();

        public BooksForm()
        {
            InitializeComponent();

            Text = "Manage Books";
            Size = new Size(650, 570);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label heading = new Label();
            heading.Text = "Manage Library Books";
            heading.Font = new Font("Arial", 18, FontStyle.Bold);
            heading.AutoSize = true;
            heading.Location = new Point(150, 20);
            Controls.Add(heading);

            Label idLabel = new Label();
            idLabel.Text = "Book ID:";
            idLabel.Location = new Point(50, 90);
            idLabel.AutoSize = true;
            Controls.Add(idLabel);

            bookIdBox = new TextBox();
            bookIdBox.Location = new Point(150, 85);
            bookIdBox.Width = 200;
            Controls.Add(bookIdBox);

            Label titleLabel = new Label();
            titleLabel.Text = "Book Title:";
            titleLabel.Location = new Point(50, 135);
            titleLabel.AutoSize = true;
            Controls.Add(titleLabel);

            titleBox = new TextBox();
            titleBox.Location = new Point(150, 130);
            titleBox.Width = 200;
            Controls.Add(titleBox);

            Label authorLabel = new Label();
            authorLabel.Text = "Author:";
            authorLabel.Location = new Point(50, 180);
            authorLabel.AutoSize = true;
            Controls.Add(authorLabel);

            authorBox = new TextBox();
            authorBox.Location = new Point(150, 175);
            authorBox.Width = 200;
            Controls.Add(authorBox);

            Button addButton = new Button();
            addButton.Text = "Add Book";
            addButton.Location = new Point(150, 220);
            addButton.Size = new Size(110, 35);
            Controls.Add(addButton);

            Button deleteButton = new Button();
            deleteButton.Text = "Delete Selected";
            deleteButton.Location = new Point(280, 220);
            deleteButton.Size = new Size(130, 35);
            Controls.Add(deleteButton);

            booksList = new ListBox();
            booksList.Location = new Point(50, 280);
            booksList.Size = new Size(520, 125);
            Controls.Add(booksList);

            Label searchLabel = new Label();
            searchLabel.Text = "Search by ID or Title:";
            searchLabel.Location = new Point(50, 435);
            searchLabel.AutoSize = true;
            Controls.Add(searchLabel);

            searchBox = new TextBox();
            searchBox.Location = new Point(220, 430);
            searchBox.Width = 200;
            Controls.Add(searchBox);

            Button searchButton = new Button();
            searchButton.Text = "Search";
            searchButton.Location = new Point(440, 428);
            searchButton.Size = new Size(90, 30);
            Controls.Add(searchButton);

            // Event-driven frontend
            addButton.Click += AddButton_Click;
            deleteButton.Click += DeleteButton_Click;
            searchButton.Click += SearchButton_Click;

            LoadBooks();
        }

        // Search a book
        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchText = searchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Please enter a Book ID or Title.");
                return;
            }

            for (int i = 0; i < booksList.Items.Count; i++)
            {
                string book = booksList.Items[i].ToString() ?? "";

                if (book.IndexOf(
                    searchText,
                    StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    booksList.SelectedIndex = i;
                    MessageBox.Show("Book found:\n" + book);
                    return;
                }
            }

            booksList.ClearSelected();
            MessageBox.Show("Book not found.");
        }

        // Delete through repository
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (booksList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a book to delete.");
                return;
            }

            string selectedBook =
                booksList.SelectedItem?.ToString() ?? "";

            string bookId = "";

            // Extract Book ID from displayed text
            if (selectedBook.StartsWith(
                "ID: ",
                StringComparison.OrdinalIgnoreCase))
            {
                int separatorIndex = selectedBook.IndexOf(" | ");

                if (separatorIndex > 4)
                {
                    bookId = selectedBook.Substring(
                        4, separatorIndex - 4);
                }
            }

            if (string.IsNullOrWhiteSpace(bookId))
            {
                MessageBox.Show("Could not identify the selected Book ID.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this book?\n\n"
                + selectedBook,
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bookRepository.Delete(bookId);

                // Refresh list using repository
                LoadBooks();

                MessageBox.Show("Book deleted successfully!");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Book could not be deleted because of a file error.");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Permission denied while deleting the book.");
            }
        }

        // Load books through repository
        private void LoadBooks()
        {
            try
            {
                booksList.Items.Clear();

                List<Book> books = bookRepository.GetAll();

                foreach (Book book in books)
                {
                    string displayBook =
                        "ID: " + book.BookId +
                        " | Title: " + book.Title +
                        " | Author: " + book.Author;

                    booksList.Items.Add(displayBook);
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Could not load books. Please check the file.",
                    "File Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Permission denied while loading books.",
                    "Access Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Add through repository
        private void AddButton_Click(object sender, EventArgs e)
        {
            string id = bookIdBox.Text.Trim();
            string title = titleBox.Text.Trim();
            string author = authorBox.Text.Trim();

            if (id == "" || title == "" || author == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            Book newBook = new Book
            {
                BookId = id,
                Title = title,
                Author = author
            };

            try
            {
                bookRepository.Add(newBook);

                LoadBooks();

                bookIdBox.Clear();
                titleBox.Clear();
                authorBox.Clear();

                MessageBox.Show("Book added and saved successfully!");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Book could not be saved. Please try again.",
                    "File Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Permission denied while saving books.",
                    "Access Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
