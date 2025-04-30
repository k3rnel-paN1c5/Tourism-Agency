using DTO.Booking;
using DTO.ImageShot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CarBooking
{
    public class CreateCarBookingDTO
    {
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }


        // the employee id is added when the booking is reviewd by an employee
        // [Required(ErrorMessage = "Employee ID is required")]
        // public string EmployeeId { get; set; } = string.Empty;

        //? Should this be an input or just the seats of the car? 
        [Required(ErrorMessage = "Passenger count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger count must be positive")]
        public int NumOfPassengers { get; set; }

        [Required(ErrorMessage = "Car ID is required")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Pickup location is required")]
        public string PickUpLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dropoff location is required")]
        public string DropOffLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Driver option is required")]
        public bool WithDriver { get; set; }

        // No need for the images when creating the booking entity
        // public List<CreateImageShotDTO> ImageShots { get; set; } = [];
    }



}

