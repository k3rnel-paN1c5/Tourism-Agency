import React, { useState, useEffect } from 'react';
// Assuming you'll fetch regions for a dropdown/select
// import regionService from '../services/regionService';

const TripForm = ({ onSubmit, initialData, isLoading, regions }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    startDate: '',
    endDate: '',
    price: '',
    regionId: '', // Assuming trips are linked to regions
    maxParticipants: '',
    currentParticipants: '',
    status: 'Planned', // Default status, or make it a select
  });

  // const [regions, setRegions] = useState([]); // If fetching regions here
  // useEffect(() => {
  //   const loadRegions = async () => {
  //     try {
  //       const data = await regionService.getRegions();
  //       setRegions(data || []);
  //     } catch (error) {
  //       console.error("Failed to load regions for trip form", error);
  //     }
  //   };
  //   loadRegions();
  // }, []);


  useEffect(() => {
    if (initialData) {
      setFormData({
        name: initialData.name || '',
        description: initialData.description || '',
        startDate: initialData.startDate ? new Date(initialData.startDate).toISOString().split('T')[0] : '',
        endDate: initialData.endDate ? new Date(initialData.endDate).toISOString().split('T')[0] : '',
        price: initialData.price || '',
        regionId: initialData.regionId || '',
        maxParticipants: initialData.maxParticipants || '',
        currentParticipants: initialData.currentParticipants || '0',
        status: initialData.status || 'Planned',
      });
    } else {
      setFormData({
        name: '',
        description: '',
        startDate: '',
        endDate: '',
        price: '',
        regionId: '',
        maxParticipants: '',
        currentParticipants: '0',
        status: 'Planned',
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
        <label htmlFor="tripDescription" className="input-label">Description</label>
        <textarea id="tripDescription" name="description" value={formData.description} onChange={handleChange} rows="3" className="auth-input" placeholder="Describe the trip"/>
      </div>
      <div className="grid grid-cols-2 gap-4"> {/* Using grid like in Register.css */}
        <div className="input-group">
          <label htmlFor="startDate" className="input-label">Start Date</label>
          <input id="startDate" type="date" name="startDate" value={formData.startDate} onChange={handleChange} required className="auth-input"/>
        </div>
        <div className="input-group">
          <label htmlFor="endDate" className="input-label">End Date</label>
          <input id="endDate" type="date" name="endDate" value={formData.endDate} onChange={handleChange} required className="auth-input"/>
        </div>
      </div>
      <div className="grid grid-cols-2 gap-4">
        <div className="input-group">
          <label htmlFor="price" className="input-label">Price ($)</label>
          <input id="price" type="number" name="price" value={formData.price} onChange={handleChange} required className="auth-input" placeholder="e.g., 500"/>
        </div>
        <div className="input-group">
          <label htmlFor="regionId" className="input-label">Region</label>
          <select id="regionId" name="regionId" value={formData.regionId} onChange={handleChange} required className="auth-input">
            <option value="">Select Region</option>
            {regions && regions.map(region => (
              <option key={region.id} value={region.id}>{region.name}</option>
            ))}
          </select>
        </div>
      </div>
       <div className="grid grid-cols-2 gap-4">
        <div className="input-group">
          <label htmlFor="maxParticipants" className="input-label">Max Participants</label>
          <input id="maxParticipants" type="number" name="maxParticipants" value={formData.maxParticipants} onChange={handleChange} required className="auth-input" placeholder="e.g., 20"/>
        </div>
         <div className="input-group">
          <label htmlFor="status" className="input-label">Status</label>
          <select id="status" name="status" value={formData.status} onChange={handleChange} required className="auth-input">
            <option value="Planned">Planned</option>
            <option value="Active">Active</option>
            <option value="Completed">Completed</option>
            <option value="Cancelled">Cancelled</option>
          </select>
        </div>
      </div>
      {/* currentParticipants might be read-only or managed by backend logic primarily */}
      {initialData && (
        <div className="input-group">
            <label htmlFor="currentParticipants" className="input-label">Current Participants</label>
            <input id="currentParticipants" type="number" name="currentParticipants" value={formData.currentParticipants} readOnly className="auth-input"/>
        </div>
      )}


      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip' : 'Create Trip')}
      </button>
    </form>
  );
};

export default TripForm;