import React, { useState } from 'react';
import authService from '../../services/authService';
import { useNavigate } from 'react-router-dom';
import './Register.css'

export default function Register() {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    whatsapp: '',
    country: '',
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      await authService.register(
        formData.email,
        formData.password,
        formData.confirmPassword,
        formData.firstName,
        formData.lastName,
        formData.phoneNumber,
        formData.whatsapp,
        formData.country
      );
      navigate('/dashboard');
    } catch (error) {
      alert('Registration failed: ' + (error.response?.data?.message || 'Unknown error'));
    }
  };

  return (
   <form onSubmit={handleSubmit} className="auth-form">
      <h2>Create Account</h2>
      <p className="subtitle">Join us today</p>
      
      <div className="grid grid-cols-2 gap-4">
        <div className="input-group">
          <label htmlFor="firstName" className="input-label">First Name</label>
          <input
            id="firstName"
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
            className="auth-input"
            placeholder="John"
          />
        </div>
        
        <div className="input-group">
          <label htmlFor="lastName" className="input-label">Last Name</label>
          <input
            id="lastName"
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
            className="auth-input"
            placeholder="Doe"
          />
        </div>
      </div>
      <div className="input-group">
        <label htmlFor="email" className="input-label">Email Address</label>
        <input
          id="email"
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          required
          className="auth-input"
          placeholder="you@example.com"
        />
      </div>
      
      <div className="input-group">
        <label htmlFor="password" className="input-label">Password</label>
        <input
          id="password"
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          required
          className="auth-input"
          placeholder="••••••••"
        />
      </div>
       <div className="input-group">
        <label htmlFor="confirmPassword" className="input-label">Confirm Password</label>
        <input
          id="confirmPassword"
          type="password"
          name="confirmPassword"
          value={formData.confirmPassword}
          onChange={handleChange}
          required
          className="auth-input"
          placeholder="••••••••"
        />
      </div>
      
      <div className="grid grid-cols-2 gap-4">
        <div className="input-group">
          <label htmlFor="phoneNumber" className="input-label">Phone Number</label>
          <input
            id="phoneNumber"
            type="text"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
            pattern="[0-9]{12}"
            className="auth-input"
            placeholder="123456789012"
          />
        </div>
         <div className="input-group">
          <label htmlFor="country" className="input-label">Country</label>
          <input
            id="country"
            type="text"
            name="country"
            value={formData.country}
            onChange={handleChange}
            required
            className="auth-input"
            placeholder="USA"
          />
        </div>
      </div>
      
      <div className="input-group">
        <label htmlFor="whatsapp" className="input-label">WhatsApp (optional)</label>
        <input
          id="whatsapp"
          type="text"
          name="whatsapp"
          value={formData.whatsapp}
          onChange={handleChange}
          pattern="[0-9]{14}"
          className="auth-input"
          placeholder="12345678901234"
        />
      </div>
      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? (
          <div className="spinner"></div>
        ) : (
          'Create Account'
        )}
      </button>
      
      <div className="divider">
        <span>or continue with</span>
      </div>
      
      <button type="button" className="social-button">
        Sign up with E-class
      </button>
    </form>
  );
}