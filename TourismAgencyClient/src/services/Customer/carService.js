import apiClient from '../apiService';
const carService ={
 getAvailableCars: async (startDate, endDate) => {
    try {
      const response = await apiClient.get('/api/Customer/CustomerDashboard/AvailableCars',{
        params: { startDate, endDate }
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching available cars:', error);
      throw error;
    }
  }

};
export default carService;

