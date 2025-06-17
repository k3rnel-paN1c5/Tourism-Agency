import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext';
import './NavBar.css';

const Navbar = () => {
  const { currentRole, logout } = useContext(AuthContext);

  const handleLogout = () => {
    logout();
  };

  return (
    <nav className="navbar">
      <ul className="navbar-nav">
        {currentRole && currentRole === 'Customer' && (
          <>
            <li className="nav-item">
              <Link to="/dashboard" className="nav-link">Customer Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-booking" className="nav-link">Trip Booking</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-booking" className="nav-link">Car Booking</Link>
            </li>
          </>
        )}

        {currentRole && currentRole === 'TripSupervisor' && (
          <>
            <li className="nav-item">
              <Link to="/trip-dashboard" className="nav-link">Trip Supervisor Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/regions" className="nav-link">Region Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trips" className="nav-link">Trip Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trip-plans" className="nav-link">Trip Plan Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trip-bookings" className="nav-link">Trip Booking Management</Link>
            </li>
             <li className="nav-item">
              <Link to="/posts-management" className="nav-link">Posts</Link>
            </li>
          </>
        )}

        {currentRole && currentRole === 'CarSupervisor' && (
          <>
            <li className="nav-item">
              <Link to="/car-dashboard" className="nav-link">Car Supervisor Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-supervisor/cars" className="nav-link">Car Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-supervisor/categories" className="nav-link">Car Category Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/posts-management" className="nav-link">Posts</Link>
            </li>
          </>
        )}

        {currentRole && currentRole === 'Admin' && (
          <>
            <li className="nav-item">
              <Link to="/admin-dashboard" className="nav-link">Admin Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-booking" className="nav-link">Trip Booking</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-booking" className="nav-link">Car Booking</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-dashboard" className="nav-link">Trip Supervisor Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/regions" className="nav-link">Region Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trips" className="nav-link">Trip Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trip-plans" className="nav-link">Trip Plan Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/trip-supervisor/trip-bookings" className="nav-link">Trip Booking Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-dashboard" className="nav-link">Car Supervisor Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-supervisor/cars" className="nav-link">Car Management</Link>
            </li>
            <li className="nav-item">
              <Link to="/car-supervisor/categories" className="nav-link">Car Category Management</Link>
            </li>
             <li className="nav-item">
              <Link to="/posts-management" className="nav-link">Posts</Link>
            </li>
          </>
        )}

        {currentRole ? (
          <li className="nav-item">
            <button onClick={handleLogout} className="nav-link logout-btn">Logout</button>
          </li>
        ) : (
          <>
            <li className="nav-item">
              <Link to="/login" className="nav-link">Login</Link>
            </li>
            <li className="nav-item">
              <Link to="/register" className="nav-link">Register</Link>
            </li>
          </>
        )}
      </ul>
    </nav>
  );
};

export default Navbar;