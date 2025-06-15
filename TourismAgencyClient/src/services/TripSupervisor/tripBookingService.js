import apiClient from '../apiService';

const tripBookingService = {
  getTripBookings: async () => {
    try {
      const response = await apiClient.get('/api/Customer/CustomerDashboard/TripBooking');
      return response.data; 
    } catch (error) {
      console.error('Error fetching trip bookings:', error);
      throw error;
    }
  },

  getTripBookingById: async (id) => {
    try {
      const response = await apiClient.get(`/api/Customer/CustomerDashboard/TripBooking/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching trip booking:', error);
      throw error;
    }
  },

  /**
   * Accepts a trip booking.
   * @param {number} id - The ID of the trip booking to accept.
   */
  acceptTripBooking: async (id) => {
    try {
      const response = await apiClient.put(`/api/TripSupervisor/TripSupervisorDashboard/Accept/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error accepting trip booking ${id}:`, error);
      throw error;
    }
  },
  rejectTripBooking: async (id) => {
    try {
      const response = await apiClient.put(`/api/TripSupervisor/TripSupervisorDashboard/Cancel/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error rejecting trip booking ${id}:`, error);
      throw error;
    }
  },

  createTripBooking: async (bookingData) => {
    try {
      const response = await apiClient.post('/api/Customer/CustomerDashboard/TripBooking', bookingData);
      return response.data;
    } catch (error) {
      console.error('Error creating trip booking:', error);
      throw error;
    }
  },

  updateTripBooking: async (id, bookingData) => {
    try {
      const response = await apiClient.put(`/api/Customer/CustomerDashboard/TripBooking/${id}`, bookingData);
      return response.data;
    } catch (error) {
      console.error('Error updating trip booking:', error);
      throw error;
    }
  },
};

export default tripBookingService; 