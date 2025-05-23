import { Routes, Route, Navigate } from 'react-router-dom';
import AuthPage from '../pages/Customer/AuthPage';
import Dashboard from '../pages/Customer/Dashboard';
import NotFound from '../pages/NotFound';


export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<AuthPage />} />
      <Route path="/register" element={<AuthPage />} />
      <Route path="/dashboard" element={<Dashboard />} />
      <Route path="/" element={<Navigate to="/login" />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}