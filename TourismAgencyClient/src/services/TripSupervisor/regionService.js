import apiClient from '../apiService';

const regionService = {
  getRegions: async () => {
    try {
      const response = await apiClient.get('/api/TripSupervisor/TripSupervisorDashboard/Regions'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching regions:', error);
      throw error;
    }
  },

  getRegionById: async (id) => {
    try {
      const response = await apiClient.get(`/api/TripSupervisor/TripSupervisorDashboard/Regions/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching region ${id}:`, error);
      throw error;
    }
  },

  createRegion: async (regionData) => {
    try {
      const response = await apiClient.post('/api/TripSupervisor/TripSupervisorDashboard/Regions', regionData); 
      return response.data;
    } catch (error) {
      console.error('Error creating region:', error);
      throw error;
    }
  },

  updateRegion: async (id, regionData) => {
    try {
      const response = await apiClient.put(`/api/TripSupervisor/TripSupervisorDashboard/Regions/${id}`, regionData); 
      return response.data;
    } catch (error) {
      console.error(`Error updating region ${id}:`, error);
      throw error;
    }
  },

  deleteRegion: async (id) => {
    try {
      const response = await apiClient.delete(`/api/TripSupervisor/TripSupervisorDashboard/Regions/${id}`); 
      return response.data;
    } catch (error){
      console.error(`Error deleting region ${id}:`, error);
      throw error;
    }
  },
};

export default regionService;