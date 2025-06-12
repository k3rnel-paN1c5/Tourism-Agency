import React, { createContext, useState, useEffect } from 'react';
import authService from '../services/authService';

export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [currentRole, setCurrenteRole] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const res = authService.getCurrentRole();
    if (res && res['roles']) {
      setCurrenteRole(res['roles']);
      setIsAuthenticated(true);
    }
  }, []);

  const login = async (email, password, rememberMe) => {
    const data = await authService.login(email, password, rememberMe);
    if (data.role) { // Assuming data contains a user object upon successful login
      setCurrenteRole(data.role);
      setIsAuthenticated(true);
    }
    return data;
  };

  const logout = async () => {
    await authService.logout();
    setCurrenteRole(null);
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ currentRole, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};