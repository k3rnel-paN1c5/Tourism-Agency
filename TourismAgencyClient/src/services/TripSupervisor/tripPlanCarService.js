import apiClient from '../apiService';

const tripPlanCarService = {
    getAvailableCars: async (startDate, endDate) => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/AvailableCars',{
        params: { startDate, endDate }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching available cars:', error);
      throw error;
    }
  },
    getCars: async (startDate, endDate) => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/Cars');
      return response.data;
    } catch (error) {
      console.error('Error fetching all cars:', error);
      throw error;
    }
  },
  getTripPlanCars: async () => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/TripPlanCars');
      return response.data;
    } catch (error) {
      console.error('Error fetching trip plan cars:', error);
      throw error;
    }
  },
  getTripPlanCarById: async (id) => {
    try {
      const response = await apiClient.get(`/api/TripSupervisor/TripSupervisorDashboard/TripPlanCars/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching trip plan car ${id}:`, error);
      throw error;
    }
  },

  AddTripPlanCartoPlan: async (tripPlanCarData) => {
    try {
      const response = await apiClient.post('/api/TripSupervisor/TripSupervisorDashboard/AddTripPlanCar', tripPlanCarData);
      return response.data;
    } catch (error) {
      console.error('Error adding trip plan car:', error);
      throw error;
    }
  },
  RemoveTripPlanCarFromPlan: async (id) => {
    try {
      const response = await apiClient.post(`/api/TripSupervisor/TripSupervisorDashboard/RemoveTripPlanCar/${id}`, id);
      return response.data;
    } catch (error) {
      console.error('Error removing trip plan car:', error);
      throw error;
    }
  },

  createTripPlanCar: async (tripPlanCarData) => {
    try {
      const response = await apiClient.post('/api/TripSupervisor/TripSupervisorDashboard/TripPlanCars', tripPlanCarData);
      return response.data;
    } catch (error) {
      console.error('Error creating trip plan car:', error);
      throw error;
    }
  },

  deleteTripPlanCar: async (id) => {
    try {
      const response = await apiClient.delete(`/api/TripSupervisor/TripSupervisorDashboard/TripPlanCars/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting trip plan car ${id}:`, error);
      throw error;
    }
  },
};

export default tripPlanCarService;