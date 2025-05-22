import React from 'react';
import Login from '../components/auth/Login.component';

const LoginPage: React.FC = () => {
  return (
    <div className="page login-page">
      <div className="page-content">
        <Login />
      </div>
    </div>
  );
};

export default LoginPage;