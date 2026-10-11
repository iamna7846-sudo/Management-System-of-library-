# Library Management System

## Project Overview

A desktop application developed using C# Windows Forms.

## Technologies

* C#
* .NET 10
* Windows Forms
* Visual Studio

## Features

* Admin login and password verification
* Password hashing using PBKDF2
* Book management
* Student management
* Duplicate ID validation
* Local data persistence

## Architecture

* Models: Book and Student classes
* Data: BookRepository and StudentRepository
* Presentation: Login, Dashboard, Books, and Students forms
* Event-Driven Architecture: UI events trigger application actions
* Authentication: AuthService and PasswordHelper

## How to Run

1. Open the project in Visual Studio.
2. Build the solution.
3. Run the application.
4. Create an admin account on first use, if prompted.
5. Log in using your saved credentials.

## Data Storage

Books and students are stored in local text files. Shared cloud storage is not implemented.

## Security

Passwords are stored as salted PBKDF2 hashes. The project still needs further security testing before production use.

## Future Improvements

* Cloud database
* Password reset
* Role-based access control
* Book issue and return system
