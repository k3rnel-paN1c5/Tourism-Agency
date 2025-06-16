import { useState, useEffect } from 'react';
import authService from '../../services/authService';
import DashboardHeader from '../../components/shared/DashboardHeader'
import TripBookingSection from '../../components/booking/TripBookingSection';
import CarBookingSection from '../../components/booking/CarBookingSection';
import PaymentSection from '../../components/payment/PaymentSection';
import bookingService from '../../services/bookingService';
import paymentService from '../../services/paymentService';
import Modal from '../../components/shared/Modal'
import './Dashboard.css';

export default function CustomerDashboard() {
  const [tripBookings, setTripBookings] = useState([]);
  const [carBookings, setCarBookings] = useState([]);
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [activeTab, setActiveTab] = useState('bookings');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedBooking, setSelectedBooking] = useState(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      const token = localStorage.getItem('token');
      console.log('Token exists:', !!token);

      if (!token) {
        setError('No authentication token found. Please login again.');
        return;
      }

      const [tripData, carData] = await Promise.all([
        bookingService.getTripBookings(),
        bookingService.getCarBookings()
      ]);
      
      setTripBookings(tripData);
      setCarBookings(carData);
      setError(null);
    } catch (error) {
      console.error('Error fetching bookings:', error);
      setError('Failed to load bookings. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const fetchPayments = async () => {
    try {
      const paymentData = await paymentService.getAllPayments();
      setPayments(paymentData);
    } catch (error) {
      console.error('Error fetching payments:', error);
      // Don't set error here as payments might not exist yet
    }
  };

  useEffect(() => {
    fetchData();
    if (activeTab === 'payments') {
      fetchPayments();
    }
  }, [activeTab]);

  const handlePaymentUpdate = (updatedPayment) => {
    setPayments(prevPayments => 
      prevPayments.map(payment => 
        payment.id === updatedPayment.id ? updatedPayment : payment
      )
    );
  };

  const handleOpenModal = async (booking) => {
    setSelectedBooking(booking);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedBooking(null);
  };

  const handleCancel = async (booking) => {
    try {
      await bookingService.CancelTripBooking(booking.id);
      setIsModalOpen(false);
      fetchData();

    }
    catch (err) {
      console.error("Cancelation failed", err);

    }
  }
  const handlePayment = async () => {
    // if (!paymentDetails) {
    //   setPaymentError("Payment details are not available.");
    //   return;
    // }
    try {
      // This is a placeholder for a real payment flow.
      // You would typically integrate a payment gateway here.
      // const processPaymentDto = {
      // paymentId: paymentDetails.id,
      // transactionMethodId: 1, // Assuming '1' is a valid transaction method like 'Credit Card'
      // amount: paymentDetails.amount
      // };
      // await paymentService.processPayment(paymentDetails.id, processPaymentDto);
      handleCloseModal();
      fetchBookings(); // Refetch bookings to show the updated status
    } catch (err) {
      console.error("Cancellation failed", err);
    }
  };

  const handleTabChange = (tab) => {
    setActiveTab(tab);
    if (tab === 'payments') {
      fetchPayments();
    }
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="dashboard-container">
      <div className="dashboard-wrapper">
        <DashboardHeader title="Customer Dashboard" subtitle="Manage your trip and car bookings" />

        <TripBookingSection bookings={tripBookings} onBookingClick={handleOpenModal} />
        <CarBookingSection bookings={carBookings} />
      </div>

      <Modal isOpen={isModalOpen} onClose={handleCloseModal} title="Booking Details">
        {selectedBooking && (
          <div className="booking-modal-content">
            <h3>{selectedBooking.tripName || selectedBooking.carName}</h3>
            <div className="booking-details-grid">
              <div className="detail-item">
                <strong>Start Date:</strong> 
                <span>{selectedBooking.startDate}</span>
              </div>
              <div className="detail-item">
                <strong>End Date:</strong> 
                <span>{selectedBooking.endDate}</span>
              </div>
              <div className="detail-item">
                <strong>Status:</strong> 
                <span className={`status-${selectedBooking.status}`}>
                  {selectedBooking.status === 0 ? 'Pending' : 
                   selectedBooking.status === 2 ? 'Confirmed' : 'Canceled'}
                </span>
              </div>
              <div className="detail-item">
                <strong>Amount:</strong> 
                <span className="amount">${selectedBooking.amountDue}</span>
              </div>
            </div>

            <div className="modal-actions">
              {selectedBooking.status === 0 && (
                <button 
                  onClick={() => handleCancel(selectedBooking)} 
                  className="btn btn-danger"
                >
                  Cancel Booking
                </button>
              )}
              <button 
                onClick={handleCloseModal} 
                className="btn btn-secondary"
              >
                Close
              </button>
            </div>
          </div>
        )}
      </Modal>
    </div>
  );
}