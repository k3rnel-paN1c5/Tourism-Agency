import React, { useState, useEffect } from 'react';
import ErrorMessage from './ErrorMessage';

const TripForm = ({ onSubmit, initialData, isLoading, error }) => {
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
        isAvailable: initialData.isAvailable !== undefined ? initialData.isAvailable : true,
        isPrivate: initialData.isPrivate !== undefined ? initialData.isPrivate : false,
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
  let submitData;
  const handleSubmit = (e) => {
    e.preventDefault();
    if (initialData && initialData.id) {

      submitData = formData
      submitData.id = initialData.id;
    }

    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form">
      {error && <ErrorMessage message={error} />}
      <div className="input-group">
        <label htmlFor="tripName" className="input-label">Trip Name</label>
        <input id="tripName" type="text" name="name" value={formData.name} onChange={handleChange} required className="auth-input" placeholder="e.g., Mountain Adventure" />
      </div>
      <div className="input-group">
        <label htmlFor="tripSlug" className="input-label">Trip Slug</label>
        <input id="tripSlug" type="text" name="slug" value={formData.slug} onChange={handleChange} required className="auth-input" placeholder="e.g., best-mountain-adventure" />
      </div>
      <div className="input-group">
        <label htmlFor="tripDescription" className="input-label">Description</label>
        <textarea id="tripDescription" name="description" value={formData.description} onChange={handleChange} required rows="3" className="auth-input" placeholder="Describe the trip" />
      </div>
      <div className="form-row">
        <div className="checkbox-group">
          <input
            id="isAvailable"
            name="isAvailable"
            type="checkbox"
            checked={formData.isAvailable}
            onChange={handleChange}
          />
          <label htmlFor="isAvailable">Available for Booking</label>
        </div>

        <div className="checkbox-group">
          <input
            id="isPrivate"
            name="isPrivate"
            type="checkbox"
            checked={formData.isPrivate}
            onChange={handleChange}
          />
          <label htmlFor="isPrivate">Private Trip</label>
        </div>
      </div>

      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip' : 'Create Trip')}
      </button>
    </form>
  );
};

export default TripForm;