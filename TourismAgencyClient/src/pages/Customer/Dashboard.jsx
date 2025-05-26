import { useState, useEffect } from 'react';
import authService from '../../services/authService';
import DashboardHeader from '../../components/DashboardHeader'
import TripBookingSection from '../../components/TripBookingSection';
import CarBookingSection from '../../components/CarBookingSection';
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

    fetchBookings();
    console.log(tripBookings);
  }, []);

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

        <TripBookingSection bookings={tripBookings} />
        <CarBookingSection bookings={carBookings} />
      </div>
    </div>
  );
}