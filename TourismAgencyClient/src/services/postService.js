import apiClient from './apiService';

// For roles: Admin, TripSupervisor, CarSupervisor
const managementApiClient = apiClient; 

// --- Public Routes ---

// Fetch all published posts
export const getPublishedPosts = () => {
  return apiClient.get('/api/Post/published');
};

// Fetch a single published post by ID
export const getPublicPost = (id) => {
  return apiClient.get(`/api/Post/${id}`);
};


// --- Management (Authenticated) Routes ---

// Fetch all posts for management
export const getAllPosts = () => {
  return managementApiClient.get('/api/Common/CommonDashboard/Posts');
};

// Fetch all post types
export const getPostTypes = () => {
    return managementApiClient.get('/api/Common/CommonDashboard/PostTypes');
};

// Create a new post
export const createPost = (postData) => {
  return managementApiClient.post('/api/Common/CommonDashboard/Post', postData);
};

// Update a post
export const updatePost = (id, postData) => {
  return managementApiClient.put(`/api/Common/CommonDashboard/Post/${id}`, postData);
};

// Delete a post
export const deletePost = (id) => {
  return managementApiClient.delete(`/api/Common/CommonDashboard/Post/${id}`);
};

// ... other management functions from CommonDashboardController