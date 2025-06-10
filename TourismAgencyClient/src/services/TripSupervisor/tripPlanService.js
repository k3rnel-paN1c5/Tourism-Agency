import apiClient from '../apiService';

const tripPlanService = {
  getTripPlans: async () => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/TripPlans'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching trip Plans:', error);
      throw error;
    }
  },
  getTripPlanById: async (id) => {
    try {
      const response = await apiClient.get(`/api/TripSupervisor/TripSupervisorDashboard/TripPlans/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching trip plan ${id}:`, error);
      throw error;
    }
  },

  createTripPlan: async (tripPlanData) => {
    try {
      const response = await apiClient.post('/api/TripSupervisor/TripSupervisorDashboard/TripPlans', tripPlanData); 
      return response.data;
    } catch (error) {
      console.error('Error creating trip plan:', error);
      throw error;
    }
  },

  updateTripPlan: async (id, tripPlanData) => {
    try {
      const response = await apiClient.put(`/api/TripSupervisor/TripSupervisorDashboard/TripPlans/${id}`, tripPlanData); 
      return response.data;
    } catch (error) {
      console.error(`Error updating trip plan ${id}:`, error);
      throw error;
    }
  },

   deleteTripPlan: async (id) => {
    try {
      const response = await apiClient.delete(`/api/TripSupervisor/TripSupervisorDashboard/TripPlans/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error deleting trip plan ${id}:`, error);
      throw error;
    }
  },
};

export default tripPlanService;