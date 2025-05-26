import axios from 'axios';

// Use VITE_API_URL from .env.development.local or Docker Compose env
const API_URL = import.meta.env.VITE_API_URL;

const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  },
  withCredentials: false
});

// Request interceptor
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
       console.log('Request with token:', config.url); // Debug log
    }
    else{
       console.warn('No token found for request:', config.url); // Debug log
    }
    return config;
  },
  (error) => {
    console.error('Request interceptor error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor
apiClient.interceptors.response.use(
  (response) => {
    console.log('Successful response:', response.config.url, response.status); // Debug log
    return response;
  },
  (error) => {
    console.error('Response error:', error.config?.url, error.response?.status, error.response?.data);
    if (error.response?.status === 401) {
      // Handle unauthorized access
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default apiClient;