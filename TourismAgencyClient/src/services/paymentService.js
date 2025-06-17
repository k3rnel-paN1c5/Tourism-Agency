import apiClient from './apiService';



const paymentService = {
  // Get all payments for the current customer
  getAllPayments: async () => {
    try {


      const response = await apiClient.get('/api/Customer/CustomerDashboard/Payments');
      return response.data; 
    } catch (error) {
      console.error('Error fetching payments:', error);
      throw error;
    }
  },
  
  getPaymentById: async (id) => {
    try {

      const response = await apiClient.get(`/api/Customer/CustomerDashboard/Payments/${id}`);
      return response.data;
    } catch (error) {

      console.error(`Error fetching payment with id ${id}:`, error);
      throw error;
    }
  },

  // Get payment by booking ID
  getPaymentByBookingId: async (bookingId) => {
    try {

      const response = await apiClient.get(`/api/Customer/CustomerDashboard/Payments/Booking/${bookingId}`);
      return response.data;
    } catch (error) {

      console.error(`Error fetching payment for booking ${bookingId}:`, error);
      throw error;
    }
  },



  // Create a payment
  createPayment: async (paymentData) => {
    try {

      const response = await apiClient.post('/api/Customer/CustomerDashboard/Payments', paymentData);
      return response.data;
    } catch (error) {

      console.error('Error creating payment:', error);
      throw error;
    }
  },



  // Update a payment
  updatePayment: async (id, paymentData) => {
    try {

      const response = await apiClient.put(`/api/Customer/CustomerDashboard/Payments/${id}`, paymentData);
      return response.data;
    } catch (error) {

      console.error('Error updating payment:', error);
      throw error;
    }
  },



  // Cancel a payment
  cancelPayment: async (id) => {
    try {

      const response = await apiClient.put(`/api/Customer/CustomerDashboard/Payments/${id}/Cancel`);
      return response.data;
    } catch (error) {

      console.error(`Error cancelling payment with id ${id}:`, error);
      throw error;
    }
  },



  // Process a payment
  processPayment: async (id, processPaymentData) => {
    try {
      const response = await apiClient.post(`/api/Customer/CustomerDashboard/Payments/${id}/Process`, processPaymentData);
      return response.data;
    } catch (error) {

      console.error(`Error processing payment with id ${id}:`, error);
      throw error;
    }
  },



  // Get payments by status
  getPaymentsByStatus: async (status) => {
    try {

      const response = await apiClient.get(`/api/Customer/CustomerDashboard/Payments?status=${status}`);
      return response.data;
    } catch (error) {

      console.error(`Error fetching payments with status ${status}:`, error);
      throw error;
    }
  },


  // Process refund
  processRefund: async (id, refundData) => {
    try {

      const response = await apiClient.post(`/api/Customer/CustomerDashboard/Payments/${id}/Refund`, refundData);
      return response.data;
    } catch (error) {

      console.error(`Error processing refund for payment ${id}:`, error);
      throw error;
    }
  }

};

export default paymentService;