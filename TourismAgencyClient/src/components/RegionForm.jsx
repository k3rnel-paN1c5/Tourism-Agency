import React, { useState, useEffect } from 'react';

// Using general .auth-form, .input-group, .input-label, .auth-input, .auth-button styles
// from existing CSS (Login.css/Register.css) adapted via Modal.css

const RegionForm = ({ onSubmit, initialData, isLoading }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');

  useEffect(() => {
    if (initialData) {
      setName(initialData.name || '');
      setDescription(initialData.description || '');
    } else {
      setName('');
      setDescription('');
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ name, description });
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form"> {/* Reusing auth-form styling */}
      <div className="input-group">
        <label htmlFor="regionName" className="input-label">Region Name</label>
        <input
          id="regionName"
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          className="auth-input"
          placeholder="e.g., Coastal Area"
        />
      </div>
      <div className="input-group">
        <label htmlFor="regionDescription" className="input-label">Description</label>
        <textarea
          id="regionDescription"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          rows="4"
          className="auth-input" // Style textarea similarly
          placeholder="Describe the region"
        />
      </div>
      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Region' : 'Create Region')}
      </button>
    </form>
  );
};

export default RegionForm;