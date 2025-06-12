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
    localStorage.setItem('role', JSON.stringify(response.data.role)); //
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
    localStorage.removeItem('role');
    // Redirect to login page
    window.location.href = '/login';
  } catch (error) {
    console.error('Logout failed:', error);
    // Even if the API call fails, clear local storage and redirect
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    window.location.href = '/login';
  }
};

const getCurrentRole = () => {
  const token = localStorage.getItem('token');
  const role = localStorage.getItem('role');
  if (token && role) {
    return {'token': token, 'roles': JSON.parse(role)}
  }
};

const isAuthenticated = () => {
  return !!localStorage.getItem('token');
};

export default {
  login,
  register,
  logout,
  getCurrentRole,
  isAuthenticated,
};