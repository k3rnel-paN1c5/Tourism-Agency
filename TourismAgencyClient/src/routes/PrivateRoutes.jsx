import React, { useContext } from 'react';
import { Route, Navigate, Outlet } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const PrivateRoute = ({ allowedRoles }) => {
  const { isAuthenticated, currentRole } = useContext(AuthContext);

  if (!isAuthenticated) {
    // Not authenticated, redirect to login page
    return <Navigate to="/login" replace />;
  }

  if (allowedRoles && !allowedRoles.includes(currentRole)) {
    // Authenticated but unauthorized role, redirect to a forbidden page or dashboard
    return <Navigate to="/dashboard" replace />; // Redirect to a common dashboard or specific error page
  }

  return <Outlet />; // User is authenticated and authorized, render child routes
};

export default PrivateRoute;