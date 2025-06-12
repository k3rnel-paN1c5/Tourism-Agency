import React, { useState, useEffect } from 'react';

const CategoryForm = ({ onSubmit, initialData, isLoading, error }) => {
  const [title, setTitle] = useState('');

  useEffect(() => {
    if (initialData) {
      setTitle(initialData.title || '');
    } else {
      setTitle('');
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const submissionData = { title };
    

    if (initialData && initialData.id) {
      submissionData.id = initialData.id;
    }
    
    onSubmit(submissionData);
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form"> 
     {error && <p className="error-message">{error}</p>}
      <div className="input-group">
        <label htmlFor="categoryTitle" className="input-label">Category Title</label>
        <input
          id="categoryTitle"
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          maxLength="100"
          required
          className="auth-input"
          placeholder="e.g., Mini Van"
        />
      </div>
      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Category' : 'Create Category')}
      </button>
    </form>
  );
};

export default CategoryForm;