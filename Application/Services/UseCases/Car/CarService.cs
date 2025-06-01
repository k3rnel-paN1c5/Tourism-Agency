using Application.DTOs.Car;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;



namespace Application.Services.UseCases
{
    public class CarService : ICarService
    {
        private readonly IRepository<Car, int> _repo;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICarBookingService _carBookingService;
        private readonly ITripPlanService _tripPlanService;

        public CarService(IMapper mapper, IRepository<Car, int> repo, ICategoryService categoryService, ICarBookingService carBookingService, ITripPlanService tripPlanService)
        {

            _mapper = mapper;
            _repo = repo;
            _categoryService = categoryService;
            _carBookingService = carBookingService;
            _tripPlanService = tripPlanService;
        }

        public async Task<GetCarDTO> CreateCarAsync(CreateCarDTO dto)
        {
            var category = _categoryService.GetCategoryByIdAsync(dto.CategoryId)
                ?? throw new Exception($"Category {dto.CategoryId} was not found");

            var car = _mapper.Map<Car>(dto);

            car.Category = _mapper.Map<Category>(category); //remove later if found unnecessary 


            await _repo.AddAsync(car);
            await _repo.SaveAsync();

            return _mapper.Map<GetCarDTO>(car);

        }

        public async Task UpdateCarAsync(UpdateCarDTO dto)
        {
            var category = _categoryService.GetCategoryByIdAsync(dto.CategoryId)
                ?? throw new Exception($"Category {dto.CategoryId} was not found");

            var car = _mapper.Map<Car>(dto);
            car.Category = _mapper.Map<Category>(category);
            _repo.Update(car);
            await _repo.SaveAsync();


        }


        public async Task DeleteCarAsync(int id)
        {
            await _repo.DeleteByIdAsync(id);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<GetCarDTO>> GetAllCarsAsync()
        {
            var cars = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<GetCarDTO>>(cars);

        }

        public async Task<GetCarDTO> GetCarByIdAsync(int id)
        {
            var car = await _repo.GetByIdAsync(id)
                ?? throw new Exception($"Category {id} was not found");
            return _mapper.Map<GetCarDTO>(car);
        }

        public async Task<IEnumerable<GetCarDTO>> GetCarsByCategoryAsync(int categoryId)
        {
            var cars = await _repo.GetAllByPredicateAsync(c => c.CategoryId.Equals(categoryId))
                ?? throw new Exception($"No cars with category ID {categoryId} were found");
            return _mapper.Map<IEnumerable<GetCarDTO>>(cars);

        }

        public async Task<IEnumerable<GetCarDTO>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                throw new ArgumentException("Start date must be before end date.");
            }

            var allCars = await _repo.GetAllAsync();
            var bookedCarIds = new HashSet<int>();

            var CarBookings = await _carBookingService.GetCarBookingsByDateIntervalAsync(startDate, endDate);
            foreach (var b in CarBookings)
            {
                bookedCarIds.Add(b.CarId);
            }
            var TripPlans = await _tripPlanService.GetTripPlansByDateIntervalAsync(startDate, endDate);
            foreach (var tp in TripPlans)
            {
                if (tp.TripPlanCars != null)
                {
                    foreach (var tpc in tp.TripPlanCars)
                    {
                        bookedCarIds.Add(tpc.CarId);
                    }
                }
            }
            var availableCars = allCars.Where(car => !bookedCarIds.Contains(car.Id));

            return _mapper.Map<IEnumerable<GetCarDTO>>(availableCars);
        }
    }
}
