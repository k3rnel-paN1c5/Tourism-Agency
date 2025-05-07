using System;
using Application.DTOs.TripPlan;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
namespace Application.Services.UseCases;

public class TripPlanService : ITripPlanService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TripPlan, int>  _repo;
    private readonly IRegionService _regionService;
    private readonly ITripService _tripService;
    //todo
    // private readonly ICarService _carService;
    public TripPlanService(IMapper mapper, IRepository<TripPlan, int>  repo, IRegionService regionService, ITripService tripService){
        _mapper = mapper;
        _repo = repo;
        _regionService = regionService;
        _tripService = tripService;
    }
    public async Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO dto)
    {
        //todo: link cars with TripPlanCar

        var region = _regionService.GetRegionByIdAsync(dto.RegionId) 
            ?? throw new Exception($"Region {dto.RegionId} was not found");
        var trip = _tripService.GetTripByIdAsync(dto.TripId)
            ?? throw new  Exception($"Trip {dto.TripId} was not found");
        
        // Logic Validation:
        if(dto.EndDate <= dto.StartDate)
            throw new Exception("Invalid Date Range");
        if(dto.StartDate < DateTime.Now)
            throw new Exception("Invalid Startng Date");
        
        var tripPlan = _mapper.Map<TripPlan>(dto);
        //todo: stops and included services
        tripPlan.Region = _mapper.Map<Region>(region); //? should I do this?
        tripPlan.Trip = _mapper.Map<Trip>(trip);
        await _repo.AddAsync(tripPlan);
        await _repo.SaveAsync();
        return _mapper.Map<GetTripPlanDTO>(tripPlan);

    }   
    public async Task UpdateTripPlanAsync(UpdateTripPlanDTO dto)
    {
        //todo: link cars with TripPlanCar

        var region = _regionService.GetRegionByIdAsync(dto.RegionId) 
            ?? throw new Exception($"Region {dto.RegionId} was not found");
        var trip = _tripService.GetTripByIdAsync(dto.TripId)
            ?? throw new  Exception($"Trip {dto.TripId} was not found");
        
        // Logic Validation:
        if(dto.EndDate <= dto.StartDate)
            throw new Exception("Invalid Date Range");
        if(dto.StartDate < DateTime.Now)
            throw new Exception("Invalid Startng Date");
        
        var tripPlan = _mapper.Map<TripPlan>(dto);
        //todo: stops and included services
        tripPlan.Region = _mapper.Map<Region>(region); //? should I do this?
        tripPlan.Trip = _mapper.Map<Trip>(trip);
        _repo.Update(tripPlan);
        await _repo.SaveAsync();

    }
    public async Task DeleteTripPlanAsync(int id)
    {
        _repo.DeleteByIdAsync(id);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<GetTripPlanDTO>> GetAllTripsAsync()
    {
        var tripPlans = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetTripPlanDTO>>(tripPlans);
    }

    public async Task<GetTripPlanDTO> GetTripByIdAsync(int id)
    {
        var tripPlan = await _repo.GetByIdAsync(id)
            ?? throw new Exception($"Trip {id} was not found");
        return _mapper.Map<GetTripPlanDTO>(tripPlan);
    }

}
