import React, { useState } from 'react';
import authService from '../../services/authService';
import '../../components/auth/Register.css';
import './EmployeeRegisterForm.css';

const EmployeeRegisterForm = () => {
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: '',
        empRole: 0,
    });
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match');
            return;
        }
        formData.empRole = formData.empRole == 0 ? 'CarSupervisor' : (formData.empRole == 1 ? 'TripSupervisor'  : 'Manager');
        setLoading(true);

        try {
            // This object structure now perfectly matches the working Swagger request.
            const submissionData = {
                email: formData.email,
                password: formData.password,
                confirmPassword: formData.confirmPassword,
                empRole: formData.empRole,
            };
            
            await authService.registerEmployee(submissionData);

            setSuccess('Employee registered successfully!');
            setFormData({
                email: '',
                password: '',
                confirmPassword: '',
                empRole: 0,
            });
        } catch (err) {
            // Display a more specific error message from the server if available.
            const errorInfo = err.response?.data?.title || err.message || 'An unknown error occurred.';
            setError(errorInfo);
            console.error("Registration failed:", err.response || err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="auth-form">
            {error && <p className="error-message">{error}</p>}
            {success && <p className="success-message">{success}</p>}
            <div className="input-group">
                <label className="input-label" htmlFor="email">Email Address</label>
                <input
                    id="email"
                    type="email"
                    name="email"
                    className="auth-input"
                    placeholder="e.g., mail@example.com"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className="grid-cols-2">
                <div className="input-group">
                    <label className="input-label" htmlFor="password">Password</label>
                    <input
                        id="password"
                        type="password"
                        name="password"
                        className="auth-input"
                        placeholder="Enter your password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="input-group">
                    <label className="input-label" htmlFor="confirmPassword">Confirm Password</label>
                    <input
                        id="confirmPassword"
                        type="password"
                        name="confirmPassword"
                        className="auth-input"
                        placeholder="Confirm your password"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        required
                    />
                </div>
            </div>
            <div className="input-group">
                <label className="input-label" htmlFor="empRole">Employee Role</label>
                <select
                    id="empRole"
                    name="empRole"
                    className="auth-input"
                    value={formData.empRole}
                    onChange={handleChange}
                >
                    <option value={0}>Car Supervisor</option>
                    <option value={1}>Trip Supervisor</option>
                    <option value={2}>Manager</option>
                </select>
            </div>
            <button type="submit" className="auth-button" disabled={loading}>
                {loading ? <div className="spinner"></div> : 'Register Employee'}
            </button>
        </form>
    );
};

export default EmployeeRegisterForm;