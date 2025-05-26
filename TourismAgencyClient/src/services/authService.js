import apiClient from './apiService';

const login = async (email, password, rememberMe) => {
  const response = await apiClient.post('/api/customer/login', {
    email,
    password,
    rememberMe,
  },
  {
    headers : {
    'Content-Type': 'application/json',
  }
  });
  if (response.data.token) {
    localStorage.setItem('token', response.data.token);
  }
  return response.data;
};

const register = async (
  email,
  password,
  confirmPassword,
  firstName,
  lastName,
  phoneNumber,
  whatsapp,
  country
) => {
  const response = await apiClient.post('/api/customer/register', {
    email,
    password,
    confirmPassword,
    firstName,
    lastName,
    phoneNumber,
    whatsapp,
    country,
  }, 
  {
    headers : {
    'Content-Type': 'application/json',
  }});
  return response.data;
};

const logout = async () => {
  try {
    await apiClient.post('/api/customer/logout');
    // Clear all auth-related data
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    // Redirect to login page
    window.location.href = '/login';
  } catch (error) {
    console.error('Logout failed:', error);
    // Even if the API call fails, clear local storage and redirect
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = '/login';
  }
};

const getCurrentUser = () => {
  return localStorage.getItem('token');
};

const isAuthenticated = () => {
  return !!localStorage.getItem('token');
};

export default {
  login,
  register,
  logout,
  getCurrentUser,
  isAuthenticated,
};