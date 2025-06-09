import React, { useState, useEffect } from 'react';

const RegionForm = ({ onSubmit, initialData, isLoading, error }) => {
  const [name, setName] = useState('');

  useEffect(() => {
    if (initialData) {
      setName(initialData.name || '');
    } else {
      setName('');
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const submissionData = { name };
    

    if (initialData && initialData.id) {
      submissionData.id = initialData.id;
    }
    
    onSubmit(submissionData);
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form"> 
     {error && <p className="error-message">{error}</p>}
      <div className="input-group">
        <label htmlFor="regionName" className="input-label">Region Name</label>
        <input
          id="regionName"
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          maxLength="100"
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