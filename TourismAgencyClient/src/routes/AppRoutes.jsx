import { Routes, Route, Navigate } from 'react-router-dom';
import AuthPage from '../pages/Customer/AuthPage';
import CustomerDashboard from '../pages/Customer/CustomerDashboard';
import TripSupervisorDashboard from '../pages/TripSupervisor/TripSupervisorDashboard';
import CarSupervisorDashboard from '../pages/CarSupervisor/CarSupervisorDashboard';
import NotFound from '../pages/NotFound';

import RegionManagementPage from '../pages/TripSupervisor/RegionManagementPage';
import TripManagementPage from '../pages/TripSupervisor/TripManagementPage';
import TripPlanManagementPage from '../pages/TripSupervisor/TripPlanManagementPage';
import TripBookingManagementPage from '../pages/TripSupervisor/TripBookingManagementPage';

import CategoryManagementPage from '../pages/CarSupervisor/CategoryManagementPage';
import CarManagementPage from '../pages/CarSupervisor/CarManagementPage';


export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<AuthPage />} />
      <Route path="/register" element={<AuthPage />} />
      <Route path="/dashboard" element={<CustomerDashboard />} />

      <Route path="/trip-dashboard" element={<TripSupervisorDashboard />} />
      <Route path="/trip-supervisor/regions" element={<RegionManagementPage />} />
      <Route path="/trip-supervisor/trips" element={<TripManagementPage />} />
      <Route path="/trip-supervisor/trip-plans" element={<TripPlanManagementPage />} />
      <Route path="/trip-supervisor/trip-bookings" element={<TripBookingManagementPage />} />

      <Route path="/car-dashboard" element={<CarSupervisorDashboard />} />
      <Route path="/car-supervisor/categories" element={<CategoryManagementPage />} />
      <Route path="/car-supervisor/cars" element={<CarManagementPage />} />

      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}