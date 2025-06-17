import { Routes, Route, Navigate } from 'react-router-dom';
import AuthPage from '../pages/Customer/AuthPage';
import CustomerDashboard from '../pages/Customer/CustomerDashboard';
import TripSupervisorDashboard from '../pages/TripSupervisor/TripSupervisorDashboard';
import CarSupervisorDashboard from '../pages/CarSupervisor/CarSupervisorDashboard';
import NotFound from '../pages/NotFound';
import PrivateRoute from './PrivateRoutes';
import Home from '../pages/Home';

import RegionManagementPage from '../pages/TripSupervisor/RegionManagementPage';
import TripManagementPage from '../pages/TripSupervisor/TripManagementPage';
import TripPlanManagementPage from '../pages/TripSupervisor/TripPlanManagementPage';
import TripBookingManagementPage from '../pages/TripSupervisor/TripBookingManagementPage';

import CategoryManagementPage from '../pages/CarSupervisor/CategoryManagementPage';
import CarManagementPage from '../pages/CarSupervisor/CarManagementPage';
import CarBookingManagementPage from '../pages/CarSupervisor/CarBookingManagementPage';

import TripPlanListPage from '../pages/Customer/TripPlanListPage';
import TripPlanDetailPage from '../pages/Customer/TripPlanDetailPage';

import CarBookingPage from '../pages/Customer/CarBookingPage';


export default function AppRoutes() {
  return (
    <Routes>

      <Route path="/home" element={<Home/>} />
      <Route path="/login" element={<AuthPage login={true}/>} />
      <Route path="/register" element={<AuthPage login={false}/>} />

      {/* Customer Routes */}
      <Route element={<PrivateRoute allowedRoles={['Customer']} />}>
        <Route path="/dashboard" element={<CustomerDashboard />} />
        <Route path="/trip-booking" element={<TripPlanListPage />} />
        <Route path="/trip-plans/:id" element={<TripPlanDetailPage />} />
        <Route path="/car-booking" element={<CarBookingPage />} /> {/* Placeholder */}
      </Route>

      {/* Trip Supervisor Routes */}
      <Route element={<PrivateRoute allowedRoles={['TripSupervisor', 'Admin']} />}>
        <Route path="/trip-dashboard" element={<TripSupervisorDashboard />} />
        <Route path="/trip-supervisor/regions" element={<RegionManagementPage />} />
        <Route path="/trip-supervisor/trips" element={<TripManagementPage />} />
        <Route path="/trip-supervisor/trip-plans" element={<TripPlanManagementPage />} />
        <Route path="/trip-supervisor/trip-bookings" element={<TripBookingManagementPage />} />
        <Route path="/posts" element={<div>Posts Management Page</div>} /> {/* Placeholder for Posts */}
      </Route>

      {/* Car Supervisor Routes */}
      <Route element={<PrivateRoute allowedRoles={['CarSupervisor', 'Admin']} />}>
        <Route path="/car-dashboard" element={<CarSupervisorDashboard />} />
        <Route path="/car-supervisor/categories" element={<CategoryManagementPage />} />
        <Route path="/car-supervisor/cars" element={<CarManagementPage />} />
        <Route path="/car-supervisor/car-bookings" element={<CarBookingManagementPage />} />
        {/* Posts page is common, so it can be handled by either or a common route */}
      </Route>

      {/* Admin specific dashboard - assuming there is one, otherwise admin uses other dashboards */}
      <Route element={<PrivateRoute allowedRoles={['Admin']} />}>
        <Route path="/admin-dashboard" element={<div>Admin Specific Dashboard</div>} /> {/* Placeholder for Admin Dashboard */}
      </Route>


      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}