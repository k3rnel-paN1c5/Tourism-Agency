import React, { useState, useEffect } from 'react';
import ErrorMessage from './ErrorMessage';
import "./Form.css";

const RegionForm = ({ onSubmit, initialData, isLoading }) => {
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
    <>
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="regionName" className="form-group">Region Name</label>
          <input
            id="regionName"
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            maxLength="100"
            required
            className="form-input"
            placeholder="e.g., Coastal Area" />
        </div>
        <button type="submit" className="auth-button" disabled={isLoading}>
          {isLoading ? <div className="spinner" /> : (initialData ? 'Update Region' : 'Create Region')}
        </button>
      </form>
    </>
  );
};

export default RegionForm;