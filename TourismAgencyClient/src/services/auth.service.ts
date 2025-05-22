import api from './api';
import type { LoginRequest, LoginResponse, RegisterRequest } from '../types/auth.types';

export const authService = {
  login: async (credentials: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/CustomerAuth/login', credentials);
    // Store token in localStorage
    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify({
        userId: response.data.userId,
        email: response.data.email,
        role: response.data.role
      }));
    }
    return response.data;
  },

  register: async (userData: RegisterRequest): Promise<any> => {
    const response = await api.post('/CustomerAuth/register', userData);
    return response.data;
  },

  logout: (): void => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getCurrentUser: (): { userId?: string; email?: string; role?: string } | null => {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      return JSON.parse(userStr);
    }
    return null;
  },

  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('token');
  }
};