import apiClient from '../apiService';

const carService = {
  getCars: async () => {
    try {
      const response = await apiClient.get('/api/CarSupervisor/CarSupervisorDashboard/Cars'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching cars:', error);
      throw error;
    }
  },
  getAvailableCars: async () => {
    try {
      const response = await apiClient.get('/api/CarSupervisor/CarSupervisorDashboard/AvailableCars'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching available cars:', error);
      throw error;
    }
  },

  getCarById: async (id) => {
    try {
      const response = await apiClient.get(`/api/CarSupervisor/CarSupervisorDashboard/car/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching car with id: ${id}:`, error);
      throw error;
    }
  },

  createCar: async (carData) => {
    try {
      const response = await apiClient.post('/api/CarSupervisor/CarSupervisorDashboard/Car', carData); 
      return response.data;
    } catch (error) {
      console.error('Error creating car:', error);
      throw error;
    }
  },

  updateCar: async (id, carData) => {
    try {
      const response = await apiClient.put(`/api/CarSupervisor/CarSupervisorDashboard/Cars/${id}`, carData); 
      return response.data;
    } catch (error) {
      console.error(`Error updating car with id: ${id}:`, error);
      throw error;
    }
  },

   deleteCar: async (id) => {
    try {
      const response = await apiClient.delete(`/api/CarSupervisor/CarSupervisorDashboard/Cars/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error deleting car with id:  ${id}:`, error);
      throw error;
    }
  },
};

export default carService;