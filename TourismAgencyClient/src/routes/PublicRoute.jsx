import { Navigate, Outlet } from 'react-router-dom';
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
            return '/'; // Fallback to home
    }
};

/**
 * A route component that is only accessible to unauthenticated users.
 * If a logged-in user tries to access it, they are redirected to their dashboard.
 */
const PublicRoute = () => {
    const { isAuthenticated, currentRole } = useContext(AuthContext);

    // if (isLoading) {
    //     // You can render a loading spinner here while checking auth state
    //     return <div>Loading...</div>; 
    // }

    // If the user is authenticated, redirect them away from the public page
    return isAuthenticated ? <Navigate to={getDashboardPath(currentRole)} /> : <Outlet />;
};

export default PublicRoute;