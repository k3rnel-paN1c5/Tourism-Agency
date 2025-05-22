import React from 'react';
import Register from '../components/auth/Register.component';
const RegisterPage: React.FC = () => {
  return (
    <div className="page register-page">
      <div className="page-content">
        <Register />
      </div>
    </div>
  );
};

export default RegisterPage;