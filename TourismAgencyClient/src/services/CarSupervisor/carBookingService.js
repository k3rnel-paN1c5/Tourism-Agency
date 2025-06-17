import apiClient from '../apiService';

const carBookingService = {
  getCarBookings: async () => {
    try {
      const response = await apiClient.get('/api/Customer/CustomerDashboard/CarBooking');
      return response.data; 
    } catch (error) {
      console.error('Error fetching car bookings:', error);
      throw error;
    }
  },

  getCarBookingById: async (id) => {
    try {
      const response = await apiClient.get(`/api/Customer/CustomerDashboard/CarBooking/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching car booking:', error);
      throw error;
    }
  },

  /**
   * Accepts a car booking.
   * @param {number} id - The ID of the car booking to accept.
   */
  acceptCarBooking: async (id) => {
    try {
      const response = await apiClient.put(`/api/CarSupervisor/CarSupervisorDashboard/Accept/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error accepting car booking ${id}:`, error);
      throw error;
    }
  },
  rejectCarBooking: async (id) => {
    try {
      const response = await apiClient.put(`/api/CarSupervisor/CarSupervisorDashboard/Cancel/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error rejecting car booking ${id}:`, error);
      throw error;
    }
  },

 
};

export default carBookingService; 