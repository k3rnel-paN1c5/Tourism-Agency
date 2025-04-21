
using DataAccess.Entities;
namespace DataAccess.SeedData;
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
}
