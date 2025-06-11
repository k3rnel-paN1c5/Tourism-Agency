using Application.DTOs.Car;
using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;



namespace Application.Services.UseCases
{
    /// <summary>
    /// Provides business logic for managing Cars.
    /// </summary>
    public class CarService : ICarService
    {

        private readonly IRepository<Car, int> _repo;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CarService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarBookingService"/> class.
        /// </summary>
        /// <param name="repo">The repository for CarBooking entities.</param>
        /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
        /// <param name="categoryService">The service for category-related operations.</param>
        /// <param name="logger">The logger for this service.</param>
        public CarService(IMapper mapper, IRepository<Car, int> repo, ICategoryService categoryService, ILogger<CarService> logger)
        {

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<GetCarDTO> CreateCarAsync(CreateCarDTO dto)
        {

            if (dto is null)
            {
                _logger.LogError("CreateCarAsync: Input DTO is null.");
                throw new ArgumentNullException(nameof(dto), "Car creation DTO cannot be null.");
            }
            _logger.LogInformation("Attempting to create car with category id: {CategoryId}", dto.CategoryId);
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(dto.CategoryId).ConfigureAwait(false);
                if (category is null)
                {
                    _logger.LogError("Category with Id {CategoryId} was not found", dto.CategoryId);
                    throw new KeyNotFoundException($"Category with ID {dto.CategoryId} was not found.");
                }

                var car = _mapper.Map<Car>(dto);

                await _repo.AddAsync(car).ConfigureAwait(false);
                await _repo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Car '{Id}' created successfully.", car.Id);
                GetCarDTO getCarDTO = _mapper.Map<GetCarDTO>(car);

                return getCarDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating car.");
                throw;
            }
            
        }

        /// <inheritdoc />
        public async Task UpdateCarAsync(UpdateCarDTO dto)
        {

            if (dto is null)
            {
                _logger.LogError("UpdateCarAsync: Input DTO is null for car ID {CarId}.", dto?.Id);
                throw new ArgumentNullException(nameof(dto), "Car update DTO cannot be null.");
            }
            _logger.LogInformation("Attempting to update car with ID: {CarId}", dto.Id);

            try
            {
                var existingCar = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false);

                if (existingCar is null)
                {
                    _logger.LogWarning("Car with ID {CarId} was not found for update.", dto.Id);
                    throw new KeyNotFoundException($"Car with ID {dto.Id} was not found.");
                }

                var category = await _categoryService.GetCategoryByIdAsync(dto.CategoryId).ConfigureAwait(false);
                if (category is null)
                {
                    _logger.LogError("Category with id {id} was not found", dto.CategoryId);
                    throw new KeyNotFoundException($"Category with ID {dto.CategoryId} was not found.");
                }

                _mapper.Map(dto, existingCar);


                _repo.Update(existingCar);
                await _repo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Car '{Id}' updated successfully.", existingCar.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating car with ID {Id}.", dto.Id);
                throw;
            }


        }

        /// <inheritdoc />
        public async Task DeleteCarAsync(int id)
        {
            _logger.LogInformation("Attempting to delete car with ID: {CarId}", id);
            try
            {
                var car = await _repo.GetByIdAsync(id).ConfigureAwait(false);
                if (car is null)
                {
                    _logger.LogWarning("Car with ID {CarId} was not found for deletion.", id);
                    throw new KeyNotFoundException($"Car with ID {id} was not found.");
                }

                _repo.Delete(car);
                await _repo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Car '{Id}' deleted successfully.", car.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Car with ID {Id}.", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetCarDTO>> GetAllCarsAsync()
        {
            _logger.LogInformation("Attempting to retrieve all cars.");
            try
            {
                var cars = await _repo.GetAllAsync().ConfigureAwait(false);
                var carCount = cars?.Count() ?? 0;
                _logger.LogInformation("Retrieved {Count} cars.", carCount);
                return _mapper.Map<IEnumerable<GetCarDTO>>(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all cars.");
                throw;
            }

        }

        /// <inheritdoc />
        public async Task<GetCarDTO> GetCarByIdAsync(int id)
        {
            _logger.LogInformation("Attempting to retrieve car with ID: {CarId}", id);
            try
            {
                var car = await _repo.GetByIdAsync(id).ConfigureAwait(false);
                if (car is null)
                {
                    _logger.LogWarning("Car with ID {CarId} was not found.", id);
                    throw new KeyNotFoundException($"Car with ID {id} was not found.");
                }
                _logger.LogInformation("Car ID: {Id} retrieved successfully.", id);
                return _mapper.Map<GetCarDTO>(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving car with ID {Id}.", id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetCarDTO>> GetCarsByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Attempting to retrieve car with CategoryID: {Id}", categoryId);
            try
            {
                var cars = await _repo.GetAllByPredicateAsync(c => c.CategoryId.Equals(categoryId));
                if (cars is null)
                {
                    _logger.LogWarning("Cars with CategoryId: {Id} were not found.", categoryId);
                    throw new KeyNotFoundException($"No cars with category ID {categoryId} were found");
                }
                _logger.LogInformation("Cars with CategoryId: {Id} retrieved successfully.", categoryId);
                return _mapper.Map<IEnumerable<GetCarDTO>>(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving car with CategoryID {Id}.", categoryId);
                throw;
            }
            
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetCarDTO>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Attempting to retrieve cars booked between {StartDate} and {EndDate}",startDate , endDate);
            try
            {
                var availableCars = await _repo.GetAllByPredicateAsync(car =>

                           !car.CarBookings.Any(cb =>
                           cb.Booking.StartDate < endDate &&
                           cb.Booking.EndDate > startDate)
                           &&

                           !car.TripPlanCars.Any(tpc =>
                           tpc.TripPlan.StartDate < endDate &&
                           tpc.TripPlan.EndDate > startDate));
                _logger.LogInformation("Cars retrieved successfully.");
                return _mapper.Map<IEnumerable<GetCarDTO>>(availableCars);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving cars booked between {StartDate} and {EndDate}", startDate, endDate);
                throw;
            }
        }
    }
}
