import apiClient from '../apiService';

const tripService = {
  getTrips: async () => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/Trips'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching trips:', error);
      throw error;
    }
  },
  getAvailableTrips: async () => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/AvailableTrips'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching trips:', error);
      throw error;
    }
  },

  getTripById: async (id) => {
    try {
      const response = await apiClient.get(`/api/TripSupervisor/TripSupervisorDashboard/Trips/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching trip ${id}:`, error);
      throw error;
    }
  },

  createTrip: async (tripData) => {
    try {
      const response = await apiClient.post('/api/TripSupervisor/TripSupervisorDashboard/Trips', tripData); 
      return response.data;
    } catch (error) {
      console.error('Error creating trip:', error);
      throw error;
    }
  },

  updateTrip: async (id, tripData) => {
    try {
      const response = await apiClient.put(`/api/TripSupervisor/TripSupervisorDashboard/Trips/${id}`, tripData); 
      return response.data;
    } catch (error) {
      console.error(`Error updating trip ${id}:`, error);
      throw error;
    }
  },

   deleteTrip: async (id) => {
    try {
      const response = await apiClient.delete(`/api/TripSupervisor/TripSupervisorDashboard/Trips/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error deleting trip ${id}:`, error);
      throw error;
    }
  },
};

export default tripService;