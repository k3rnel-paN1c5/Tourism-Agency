import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5050',
});

export default apiClient;