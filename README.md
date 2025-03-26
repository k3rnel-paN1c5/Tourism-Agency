# Tourism Services Website Development - ASP.Net Core

## Project Objective

This project aims to develop a service-oriented website for a travel agency that provides car rental and diverse tourism booking services using **ASP.Net Core**. It includes creating both backend and frontend interfaces integrated with a **RESTful API** system. The focus is on enhancing user interaction with the platform through an integrated booking system. The project also incorporates strong **token-based authentication** mechanisms such as **JWT**, utilizes the **MVC architectural pattern** for flexibility and maintainability, and adopts best design practices like the **Repository Pattern**.

---

## Project Description

### 1. Car Booking Services:
- Enable users to specify search criteria (e.g., pickup and drop-off locations, booking date, car type, passenger count) to view suitable car options.
- Provide booking options for hourly or daily rentals, including special offers and discounts.
- Support multiple payment systems (e.g., pay-on-arrival or prepaid via credit card).
- Allow users to upload photos at car pickup and return for documentation purposes.

### 2. Tourism Trip Services:
- Allow users to book trips under two categories: private tours and group tours.
- Specify group tour schedules with the option for advance booking and payment of fees.
- Offer a tourist guide booking service for private trips upon request.

### 3. Website Management:
- Develop an admin dashboard for managing users and roles (e.g., Admin, Trip Supervisor, Booking Supervisor).
- Provide functionality to add, edit, and delete trips and cars.
- Include helpful statistics for decision-making.

### 4. Blog System:
- Enable users to upload articles and pages related to the website.
- Ensure adherence to **SEO standards** for increased search engine visibility.

---

## Technology Stack
- **ASP.Net Core** for creating RESTful APIs.
- **HTML**, **CSS**, **JavaScript**, **Bootstrap**, with optional frameworks such as **React** or **Angular** for the user interface.
- **SQL Server** or any other suitable database for data storage.

---

## Educational Goals
- Strengthen students' skills in web application development using **ASP.Net Core**.
- Teach how to design and implement RESTful APIs.
- Foster understanding of modern web technologies for building interactive user interfaces.
- Apply security principles for secure payment and information storage.
- Enhance database management and design skills.

---

## Requirements and Standards
- Adherence to **best programming practices (Code Quality)**.
- Proper handling of exceptions and errors.
- Implementation of a secure login and registration system.
- Responsive designs to ensure compatibility across various devices.

---

## Repository Usage

To use this repository, follow these steps:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/emadof85/miniProjSession.git
2. **Navigate to the Project Directory**:
   ```bash
   cd miniProjSession
3. **Set Up the Environment**:
   - Install the required development tools such as **Visual Studio** or any other compatible IDE.
   - Ensure **ASP.Net Core SDK** and **SQL Server** are installed on your machine.
4. **Configure the Database**:
   - Update the connection string in the project's configuration file (e.g., appsettings.json) to match your database setup.
   - Apply migrations to set up the database schema:
     ```bash
     dotnet ef database update -c IdentityAppDbContext
     dotnet ef database update -c TourAgencyDbContext
5. **Build and run the project**.
    
