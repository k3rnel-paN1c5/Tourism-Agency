import apiClient from '../apiService';

const categoryService = {
  getCategories: async () => {
    try {
      const response = await apiClient.get('/api/CarSupervisor/CarSupervisorDashboard/Categories'); 
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },

  getCategoryById: async (id) => {
    try {
      const response = await apiClient.get(`/api/CarSupervisor/CarSupervisorDashboard/Categories/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with id: ${id}:`, error);
      throw error;
    }
  },

  createCategory: async (categoryData) => {
    try {
      const response = await apiClient.post('/api/CarSupervisor/CarSupervisorDashboard/Categories', categoryData); 
      return response.data;
    } catch (error) {
      console.error('Error creating category:', error);
      throw error;
    }
  },

  updateCategory: async (id, categoryData) => {
    try {
      const response = await apiClient.put(`/api/CarSupervisor/CarSupervisorDashboard/Categories/${id}`, categoryData); 
      return response.data;
    } catch (error) {
      console.error(`Error updating category with id: ${id}:`, error);
      throw error;
    }
  },

  deleteCategory: async (id) => {
    try {
      const response = await apiClient.delete(`/api/CarSupervisor/CarSupervisorDashboard/Categories/${id}`); 
      return response.data;
    } catch (error){
      console.error(`Error deleting category with id: ${id}:`, error);
      throw error;
    }
  },
};

export default categoryService;