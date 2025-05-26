import apiClient from './apiService';

const login = async (email, password, rememberMe) => {
  const response = await apiClient.post('/api/customer/login', {
    email,
    password,
    rememberMe,
  });
  console.log(response.data.token);
  if (response.data.token !== null) {
    localStorage.setItem('token', response.data.token);
    return response.data;
  }
  throw new Error('No token received from server');
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
  });
  
  if (response.data.Token) {
    localStorage.setItem('token', response.data.Token);
    return response.data;
  }
  throw new Error('No token received from server');
};

const logout = async () => {
  try {
    await apiClient.post('/api/customer/logout');
  } catch (error) {
    console.error('Logout failed:', error);
  } finally {
    // Clear all auth-related data
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    // Redirect to login page
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