import { Routes, Route, Navigate } from 'react-router-dom';
import AuthPage from '../pages/Customer/AuthPage';
import CustomerDashboard from '../pages/Customer/CustomerDashboard';
import TripSupervisorDashboard from '../pages/TripSupervisor/TripSupervisorDashboard';
import NotFound from '../pages/NotFound';


export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<AuthPage />} />
      <Route path="/register" element={<AuthPage />} />
      <Route path="/dashboard" element={<CustomerDashboard />} />
      <Route path="/TripDashboard" element={<TripSupervisorDashboard />} />
      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}