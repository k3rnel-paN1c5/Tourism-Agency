import React from 'react';
import { authService } from '../services/auth.service';
import { useNavigate } from 'react-router-dom';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const user = authService.getCurrentUser();

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  return (
    <div className="home-page">
      <h1>Welcome to Tourism Agency</h1>
      {user ? (
        <div>
          <p>Hello, {user.email}!</p>
          <button onClick={handleLogout}>Logout</button>
        </div>
      ) : (
        <div>
          <p>Please login to access all features</p>
          <button onClick={() => navigate('/login')}>Login</button>
        </div>
      )}
    </div>
  );
};

export default HomePage;