import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';

/**
 * Returns the dashboard path based on the user's role.
 * @param {string} role - The role of the user.
 * @returns {string} The path to the corresponding dashboard.
 */
const getDashboardPath = (role) => {
    switch (role) {
        case 'Customer':
            return '/dashboard';
        case 'TripSupervisor':
            return '/trip-dashboard';
        case 'CarSupervisor':
            return '/car-dashboard';
        case 'Admin':
            return '/admin-dashboard';
        default:
            // Default path if role is unknown
            return '/login';
    }
};

/**
 * The main landing component. It redirects users based on their authentication status and role.
 */
const Home = () => {
    const { isAuthenticated, currentRole } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
            if (isAuthenticated) {
                // If user is logged in, redirect to their specific dashboard
                navigate(getDashboardPath(currentRole));
            } else {
                // If no user, redirect to the login page
                navigate('/login');
            }
    }, [isAuthenticated, currentRole, navigate]);

    // Display a loading indicator to prevent flashes of content
    return <div>Loading...</div>;
};

export default Home;
