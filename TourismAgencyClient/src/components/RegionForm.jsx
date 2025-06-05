import React, { useState, useEffect } from 'react';

const RegionForm = ({ onSubmit, initialData, isLoading }) => {
  const [name, setName] = useState('');

  useEffect(() => {
    if (initialData) {
      setName(initialData.name || '');
      setDescription(initialData.description || '');
    } else {
      setName('');
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ name });
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form"> 
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
      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Region' : 'Create Region')}
      </button>
    </form>
  );
};

export default RegionForm;