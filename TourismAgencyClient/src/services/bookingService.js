import apiClient from './apiService';

const bookingService = {  
  //sshow availlable plans to customer

  getUpcomingPlans:async () => {
    try {
      const response = await apiClient.get('/api/Customer/CustomerDashboard/TripPlans');
      return response.data; 
    } catch (error) {
      console.error('Error fetching upcoming trip plans:', error);
      throw error;
    }
  },
  getTripPlanById: async (id) => {
    try {
      const response = await apiClient.get(`/api/Customer/CustomerDashboard/TripPlans/${id}`); 
      return response.data;
    } catch (error) {
      console.error(`Error fetching trip plan ${id}:`, error);
      throw error;
    }
  },
  // Trip Bookings
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
  CancelTripBooking: async (id) => {
    try {
      const response = await apiClient.put(`/api/Customer/CustomerDashboard/CancelTripBooking/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error Canceling trip booking:', error);
      throw error;
    }
  },
  
  // Car Bookings
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

  createCarBooking: async (bookingData) => {
    try {
      const response = await apiClient.post('/api/Customer/CustomerDashboard/CarBooking', bookingData);
      return response.data;
    } catch (error) {
      console.error('Error creating car booking:', error);
      throw error;
    }
  },

  updateCarBooking: async (id, bookingData) => {
    try {
      const response = await apiClient.put(`/api/Customer/CustomerDashboard/CarBooking/${id}`, bookingData);
      return response.data;
    } catch (error) {
      console.error('Error updating car booking:', error);
      throw error;
    }
  }
};

export default bookingService; 