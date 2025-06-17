import React from 'react';
import EmployeeRegisterForm from '../../components/auth/EmployeeRegisterForm';
import '../../pages/Customer/AuthPage.css';

const EmployeeRegistrationPage = () => {
    return (
        <div className="auth-container">
            <div className="background-shapes">
                <div className="shape circle-1"></div>
                <div className="shape circle-2"></div>
                <div className="shape blob-1"></div>
                <div className="shape blob-2"></div>
            </div>
            <div className="auth-card">
                <div className="logo-container">
                    <h1>Employee Registration</h1>
                    <EmployeeRegisterForm />
                </div>
            </div>
        </div>
    );
};

export default EmployeeRegistrationPage;