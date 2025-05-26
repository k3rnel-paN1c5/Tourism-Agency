import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import TripBookingList from '../../components/TripBookingList';
import CarBookingList from '../../components/CarBookingList';
import apiClient from '../../services/apiService';
import './Dashboard.css';

export default function Dashboard() {
  const [tripBookings, setTripBookings] = useState([]);
  const [carBookings, setCarBookings] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBookings = async () => {
      try {
        setIsLoading(true);
        // Check if user is authenticated
        const token = localStorage.getItem('token');
        if (!token) {
          navigate('/login');
          return;
        }

        // Fetch trip bookings
        try {
          const tripResponse = await apiClient.get('/api/customer/customerdashboard/tripbooking');
          setTripBookings(tripResponse.data);
        } catch (tripError) {
          console.error('Error fetching trip bookings:', tripError);
          setTripBookings([]);
        }

        // Fetch car bookings
        try {
          const carResponse = await apiClient.get('/api/customer/customerdashboard/carbooking');
          setCarBookings(carResponse.data);
        } catch (carError) {
          console.error('Error fetching car bookings:', carError);
          setCarBookings([]);
        }

        setError(null);
      } catch (err) {
        console.error('Dashboard error:', err);
        if (err.response?.status === 401) {
          localStorage.removeItem('token');
          navigate('/login');
        } else {
          setError('Failed to fetch bookings. Please try again later.');
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchBookings();
  }, [navigate]);

  if (isLoading) {
    return (
      <div className="dashboard-container">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="dashboard-container">
        <div className="error-message">{error}</div>
      </div>
    );
  }

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Welcome to Your Dashboard</h1>
        <p>View and manage your bookings</p>
      </div>

      <div className="dashboard-content">
        <TripBookingList bookings={tripBookings} />
        <CarBookingList bookings={carBookings} />
      </div>
    </div>
  );
}
