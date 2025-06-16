import React, { useState, useContext } from 'react';
import { AuthContext } from '../../context/AuthContext'; // Import AuthContext
import { useNavigate } from 'react-router-dom';
import './Login.css'

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { login } = useContext(AuthContext);

  const handleLogin = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      await login(email, password, rememberMe);
      navigate('/home');
    } catch (error) {
      setIsLoading(false);
      alert('Login failed: ' + (error.response?.data?.message || 'Unknown error'));
    }
  };

  return (
     <form onSubmit={handleLogin} className="auth-form">
      <h2>Welcome Back</h2>
      <p className="subtitle">Sign in to continue</p>
      
      <div className="input-group">
        <label htmlFor="email" className="input-label">Email Address</label>
        <input
          id="email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
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
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          className="auth-input"
          placeholder="••••••••"
        />
      </div>
      
      <div className="options">
        <label className="checkbox-container">
          <input
            type="checkbox"
            checked={rememberMe}
            onChange={(e) => setRememberMe(e.target.checked)}
          />
          <span className="checkmark"></span>
          Remember Me
        </label>
        
        <a href="#" className="forgot-link">Forgot Password?</a>
      </div>
      
      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? (
          <div className="spinner"></div>
        ) : (
          'Sign In'
        )}
      </button>
      
      <div className="divider"/>
      
      <button type="button" className="social-button">
        Sign in with E-class
      </button>
    </form>
  );
}