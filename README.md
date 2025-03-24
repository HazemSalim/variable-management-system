
# Variable Management System

## Overview
The **Variable Management System** is an application designed to handle the creation, updating, deletion, and management of variables in a centralized system. This system allows users to interact with variables through a set of APIs, providing flexibility and scalability for different use cases.

The application utilizes **SignalR** for real-time notifications, providing immediate feedback to clients when operations are performed on the variables. Additionally, it integrates **Swagger** for easy access to API documentation and testing.

## Features
- **Create Variables**: Allows you to add new variables to the system.
- **Update Variables**: Modify the value of existing variables.
- **Delete Variables**: Delete variables from the system.
- **SignalR Real-time Communication**: Provides real-time updates to connected clients when variables are created, updated, or deleted.
- **Swagger API Documentation**: Provides an interactive UI to explore and test the API endpoints.

## Prerequisites
Before running the application, ensure that the following tools are installed:

- .NET 8 or later
- Visual Studio Code or any .NET-compatible IDE
- SQL Server or any database solution for storing variables
- [Swagger](https://swagger.io/) for API documentation (pre-integrated in the app)
- SignalR (for real-time notifications)

## Setup Instructions

1. **Clone the Repository**:
   ```
   git clone https://github.com/HazemSalim/variable-management-system.git
   cd variable-management-system
   ```

2. **Install Dependencies**:
   - Open the project in your preferred IDE or editor.
   - Restore the required NuGet packages:
     ```
     dotnet restore
     ```

3. **Configure the Database**:
   - Ensure your database is set up and configured.
   - Update the connection string in `appsettings.json` if necessary.

4. **Run the Application**:
   - Start the application using the following command:
     ```
     dotnet run
     ```

5. **Access Swagger UI**:
   - After the application starts, open your browser and go to `https://localhost:44360/swagger/index.html` to access the Swagger API documentation.
   - Swagger allows you to interact with the API directly from your browser, making it easy to test endpoints like creating, updating, and deleting variables.

6. **Using SignalR**:
   - SignalR is used for real-time communication between the server and connected clients.
   - After performing actions like creating, updating, or deleting variables, connected clients will automatically receive updates via SignalR.
   - You can test SignalR functionality by opening multiple tabs or browsers to simulate different clients.
   - After the application starts, open your browser and go to ` https://localhost:44360/index.html` to access the SignalR.

7. **Using the HTML Page**:
   - The HTML page serves as a basic front-end interface for interacting with the API.
   - It allows users to create, update, and delete variables via form inputs and view the results in real-time via SignalR.
   - Open the `index.html` file in a browser to start using the HTML interface.
   - The HTML page communicates with the backend API and updates the UI based on real-time updates.

## Why Do We Need This App?
The **Variable Management System** provides an easy-to-use and scalable solution for managing variables. It supports multiple use cases, such as:

- Centralized management of configuration values.
- Real-time updates for monitoring systems.
- Interaction between various services and clients through the API and SignalR.

This application is ideal for use in systems that require real-time variable updates and can be extended for additional functionality, such as user authentication, logging, and more.

## Conclusion
This app demonstrates the use of modern technologies like **SignalR** for real-time communication and **Swagger** for documenting and testing APIs. It is a scalable solution for managing variables and integrating with different systems through a RESTful API.
