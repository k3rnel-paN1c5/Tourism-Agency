import { useState, useEffect } from 'react';
import authService from '../../services/authService';
import TripBookingList from '../../components/TripBookingList';
import CarBookingList from '../../components/CarBookingList';
import bookingService from '../../services/bookingService';
import './Dashboard.css';

export default function Dashboard() {
  const [user, setUser] = useState(null);
  const [tripBookings, setTripBookings] = useState([]);
  const [carBookings, setCarBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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
    const fetchBookings = async () => {
      try {
        setLoading(true);
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

    fetchBookings();
  }, []);

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Welcome, {user?.firstName || 'User'}</h1>
        <button onClick={handleLogout} className="logout-button">
          Logout
        </button>
      </div>
      
      <div className="dashboard-content">
        <div className="bookings-section">
          <div className="section-header">
            <h2>Your Trip Bookings</h2>
            <button className="add-button">Book New Trip</button>
          </div>
          <TripBookingList bookings={tripBookings} />
        </div>

        <div className="bookings-section">
          <div className="section-header">
            <h2>Your Car Bookings</h2>
            <button className="add-button">Book New Car</button>
          </div>
          <CarBookingList bookings={carBookings} />
        </div>
      </div>
    </div>
  );
}