import apiClient from '../apiService';

const categoryService = {
  getCategories: async () => {
    try {
      const response = await apiClient.get('/api/Customer/CustomerDashboard/Categories'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  }
};
export default categoryService;