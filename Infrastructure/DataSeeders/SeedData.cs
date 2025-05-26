
using Domain.Entities;
namespace Infrastructure.DataSeeders;
public static class SeedData
{
    public static List<Category> GetCategories()
    {
        return new List<Category>
        {
            new Category {Id = 1, Title = "Sedan"},
            new Category {Id = 2, Title = "SUV" },
            new Category {Id = 3, Title = "Sports"}
        };
    }

    public static List<Car> GetCars()
    {
        return new List<Car>
        {
            new Car
            {
                Id = 1,
                Model = "Toyota Camry",
                Seats = 5,
                Color = "Silver",
                Image = "toyota-camry.jpg",
                Pph = 15.00m,
                Ppd = 80.00m,
                Mbw = 1.00m,
                CategoryId = 1 // Sedan
            },
            new Car
            {
                Id = 2,
                Model = "Honda CR-V",
                Seats = 7,
                Color = "White",
                Image = "honda-cr-v.jpg",
                Pph = 20.00m,
                Ppd = 100.00m,
                Mbw = 2.00m,
                CategoryId = 2 // SUV
            },
            new Car
            {
                Id = 3,
                Model = "Ford Mustang",
                Seats = 4,
                Color = "Red",
                Image = "ford-mustang.jpg",
                Pph = 30.00m,
                Ppd = 150.00m,
                Mbw = 3.00m,
                CategoryId = 3 // Sports
            }
        };
    }
    public static List<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer
            {
                UserId = "user1", // Unique identifier (e.g., corresponds to ASP.NET Identity UserId)
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+1234567890",
                Whatsapp = "+1234567890",
                Country = "USA"
            },
            new Customer
            {
                UserId = "user2",
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "+0987654321",
                Whatsapp = "+0987654321",
                Country = "Canada"
            },new Customer
            {
                UserId = "user3",
                FirstName = "Alice",
                LastName = "Johnson",
                PhoneNumber = "+1122334455",
                Whatsapp = "+1122334455",
                Country = "UK"
            }
        };

    }
    public static List<Employee> GetEmployees()
    {
        return new List<Employee>
        {
            new Employee
            {
                UserId = "emp1", // Unique identifier (e.g., corresponds to ASP.NET Identity UserId)
                HireDate = new DateTime(2020, 5, 1) // Hired on May 1, 2020
            },
            new Employee
            {
                UserId = "emp2",
                HireDate = new DateTime(2018, 8, 15) // Hired on August 15, 2018
            },
            new Employee
            {
                UserId = "emp3",
                HireDate = new DateTime(2022, 3, 10) // Hired on March 10, 2022
            }
        };
    }
}
