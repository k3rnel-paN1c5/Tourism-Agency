import { useState, useEffect } from 'react';
import authService from '../../services/authService';
import DashboardHeader from '../../components/shared/DashboardHeader'
import TripBookingSection from '../../components/booking/TripBookingSection';
import CarBookingSection from '../../components/booking/CarBookingSection';
import PaymentSection from '../../components/payment/PaymentSection';
import bookingService from '../../services/bookingService';
import paymentService from '../../services/paymentService';
import './Dashboard.css';

export default function CustomerDashboard() {
  const [user, setUser] = useState(null);
  const [tripBookings, setTripBookings] = useState([]);
  const [carBookings, setCarBookings] = useState([]);
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [activeTab, setActiveTab] = useState('bookings');

  const handleLogout = async () => {
    try {
      await authService.logout();
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const token = localStorage.getItem('token');
        console.log('Token exists:', !!token);

        if (!token) {
          setError('No authentication token found. Please login again.');
          return;
        }

        const [tripData, carData, paymentData] = await Promise.all([
          bookingService.getTripBookings(),
          bookingService.getCarBookings(),
          paymentService.getAllPayments()
        ]);
        
        setTripBookings(tripData);
        setCarBookings(carData);
        setPayments(paymentData);
        setError(null);
      } catch (error) {
        console.error('Error fetching data:', error);
        setError('Failed to load data. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handlePaymentUpdate = (updatedPayment) => {
    setPayments(prevPayments => 
      prevPayments.map(payment => 
        payment.id === updatedPayment.id ? updatedPayment : payment
      )
    );
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
        <DashboardHeader 
          title="Customer Dashboard" 
          subtitle="Manage your bookings and payments" 
        />

        <div className="dashboard-tabs">
          <button 
            className={`tab-button ${activeTab === 'bookings' ? 'active' : ''}`}
            onClick={() => setActiveTab('bookings')}
          >
            Bookings
          </button>
          <button 
            className={`tab-button ${activeTab === 'payments' ? 'active' : ''}`}
            onClick={() => setActiveTab('payments')}
          >
            Payments
          </button>
        </div>

        <div className="dashboard-content">
          {activeTab === 'bookings' && (
            <>
              <TripBookingSection bookings={tripBookings} />
              <CarBookingSection bookings={carBookings} />
            </>
          )}
          
          {activeTab === 'payments' && (
            <PaymentSection 
              payments={payments} 
              onPaymentUpdate={handlePaymentUpdate}
            />
          )}
        </div>
      </div>
    </div>
  );
}