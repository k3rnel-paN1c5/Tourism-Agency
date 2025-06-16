import apiClient from './apiService';

const paymentService = {
  // Get all payments or filter by status
  getAllPayments: async (status = null) => {
    try {
      const url = status ? `/api/payments?status=${status}` : '/api/payments';
      const response = await apiClient.get(url);
      return response.data;
    } catch (error) {
      console.error('Error fetching payments:', error);
      throw error;
    }
  },

  // Get payment by ID
  getPaymentById: async (id) => {
    try {
      const response = await apiClient.get(`/api/payments/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching payment with id ${id}:`, error);
      throw error;
    }
  },

  // Get payment by booking ID
  getPaymentByBookingId: async (bookingId) => {
    try {
      const response = await apiClient.get(`/api/payments/bookings/${bookingId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching payment for booking ${bookingId}:`, error);
      throw error;
    }
  },

  // Get detailed payment information
  getPaymentDetails: async (id) => {
    try {
      const response = await apiClient.get(`/api/payments/${id}/details`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching payment details for id ${id}:`, error);
      throw error;
    }
  },

  // Create a new payment
  createPayment: async (paymentData) => {
    try {
      const response = await apiClient.post('/api/payments', paymentData);
      return response.data;
    } catch (error) {
      console.error('Error creating payment:', error);
      throw error;
    }
  },

  // Process a payment
  processPayment: async (id, processPaymentData) => {
    try {
      const response = await apiClient.post(`/api/payments/${id}/process`, processPaymentData);
      return response.data;
    } catch (error) {
      console.error(`Error processing payment ${id}:`, error);
      throw error;
    }
  },

  // Process a refund
  processRefund: async (id, refundData) => {
    try {
      const response = await apiClient.post(`/api/payments/${id}/refunds`, refundData);
      return response.data;
    } catch (error) {
      console.error(`Error processing refund for payment ${id}:`, error);
      throw error;
    }
  },

  // Get payments by status
  getPaymentsByStatus: async (status) => {
    try {
      const response = await apiClient.get(`/api/payments?status=${status}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching payments with status ${status}:`, error);
      throw error;
    }
  }
};

export default paymentService;