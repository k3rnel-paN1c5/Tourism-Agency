import React, { useState, useEffect } from 'react';
// Assuming you'll fetch regions for a dropdown/select
// import regionService from '../services/regionService';

const TripForm = ({ onSubmit, initialData, isLoading }) => {
  const [formData, setFormData] = useState({
    name: '',
    slug: '',
    description: '',
    isAvailable: true,
    isPrivate: false,
  });


  useEffect(() => {
    if (initialData) {
      setFormData({
        name: initialData.name || '',
        slug: initialData.slug || '',
        description: initialData.description || '',
        isAvailable: initialData.isAvailable || true,
        isPrivate: initialData.isPrivate || false,
      });
    } else {
      setFormData({
        name: '',
        slug: '',
        description: '',
        isAvailable: true,
        isPrivate: false,
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form">
      <div className="input-group">
        <label htmlFor="tripName" className="input-label">Trip Name</label>
        <input id="tripName" type="text" name="name" value={formData.name} onChange={handleChange} required className="auth-input" placeholder="e.g., Mountain Adventure"/>
      </div>
      <div className="input-group">
        <label htmlFor="tripSlug" className="input-label">Trip Slug</label>
        <input id="tripSlug" type="text" name="slug" value={formData.slug} onChange={handleChange} required className="auth-input" placeholder="e.g., best-mountain-adventure"/>
      </div>
      <div className="input-group">
        <label htmlFor="tripDescription" className="input-label">Description</label>
        <textarea id="tripDescription" name="description" value={formData.description} onChange={handleChange} rows="3" className="auth-input" placeholder="Describe the trip"/>
      </div>
      <div className="grid grid-cols-2 gap-4"> 
        <div className="input-group">
          <label htmlFor="isAvailable" className="input-label">Available</label>
          <input id="isAvailable" type="checkbox" name="isAvailable" value={formData.isAvailable} onChange={handleChange} required className="auth-input"/>
        </div>
        <div className="input-group">
          <label htmlFor="isPrivate" className="input-label">Private Trip</label>
          <input id="isPrivate" type="checkbox" name="isPrivate" value={formData.isPrivate} onChange={handleChange} required className="auth-input"/>
        </div>
      </div>

      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip' : 'Create Trip')}
      </button>
    </form>
  );
};

export default TripForm;