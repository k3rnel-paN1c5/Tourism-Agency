import React, { useState, useEffect, useCallback } from 'react';
import regionService from '../services/TripSupervisor/regionService';
import tripService from '../services/TripSupervisor/tripService';
import './Form.css'
import './Login.css'
const TripPlanForm = ({ onSubmit, initialData, isLoading, error }) => {
  const [formData, setFormData] = useState({
    tripId: '',
    regionId: '',
    startDate: '',
    endDate: '',
    includedServices: '',
    stops: '',
    mealsPlan: '',
    hotelStays: '',
  });
  const [trips, setTrips] = useState([]);
  const [regions, setRegions] = useState([]);
  const [Loading, setLoading] = useState(true);

  const fetchDropdownData = useCallback(async () => {
    try {
      const [tripsData, regionsData] = await Promise.all([
        tripService.getAvailableTrips(),
        regionService.getRegions()
      ]);
      setTrips(tripsData);
      setRegions(regionsData);
    } catch (err) {
      setError(err.message || 'Failed to load data for the form.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDropdownData();
  }, [fetchDropdownData]);

  useEffect(() => {
    let startDate =   '';
    let endDate =  '';
    if (initialData) {
      // Format dates for the input fields
       startDate = initialData.startDate ? new Date(initialData.startDate).toISOString().split('T')[0] : '';
       endDate = initialData.endDate ? new Date(initialData.endDate).toISOString().split('T')[0] : '';
      setFormData({
        tripId: initialData.tripId || '',
        regionId: initialData.regionId || '',
        startDate,
        endDate,
        includedServices: initialData.includedServices || '',
        stops: initialData.stops || '',
        mealsPlan: initialData.mealsPlan || '',
        hotelStays: initialData.hotelStays || '',
      });
    } else {
      setFormData({
        tripId: '',
        regionId: '',
        startDate,
        endDate,
        includedServices: '',
        stops: '',
        mealsPlan: '',
        hotelStays: '',
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    let submitData;


    if (!formData.tripId || !formData.regionId || !formData.startDate || !formData.endDate) {
      return;
    }

    try {
      submitData = formData
      if (initialData && initialData.id) {

      submitData = formData
      submitData.id = initialData.id;
    }

    onSubmit(submitData);
    } catch (err) {
      console.error(err);
    }
  };
  if (Loading) {
    return <div>Loading form...</div>;
  }
  return (
    <form onSubmit={handleSubmit} className="auth-form">
      <h3>{initialData ? 'Edit Trip Plan' : 'Add New Trip Plan'}</h3>
      {error && <ErrorMessage message={error} />}

      <div className="form-grid">
        <div className="input-group">
          <label htmlFor="regionId">Region</label>
          <select id="regionId" name="regionId" value={formData.regionId} onChange={handleChange} required>
            <option value="">Select a Region</option>
            {regions.map(region => (
              <option key={region.id} value={region.id}>{region.name}</option>
            ))}
          </select>
        </div>

        <div className="input-group">
          <label htmlFor="tripId" className="input-label">Trip</label>
          <select id="tripId" name="tripId" value={formData.tripId} onChange={handleChange} required>
            <option value="">Select a Trip</option>
            {trips.map(trip => (
              <option key={trip.id} value={trip.id}>{trip.name}</option>
            ))}
          </select>
        </div>

        <div className="input-group">
          <label htmlFor="startDate" className="input-label">Start Date</label>
          <input type="date" id="startDate" name="startDate" value={formData.startDate} onChange={handleChange} required />
        </div>

        <div className="input-group">
          <label htmlFor="endDate" className="input-label">End Date</label>
          <input type="date" id="endDate" name="endDate" value={formData.endDate} onChange={handleChange} required />
        </div>

        <div className="input-group form-group-full">
          <label htmlFor="includedServices" className="input-label">Included Services</label>
          <textarea id="includedServices" name="includedServices" value={formData.includedServices} onChange={handleChange} />
        </div>

        <div className="input-group form-group-full">
          <label htmlFor="stops" className="input-label">Stops</label>
          <textarea id="stops" name="stops" value={formData.stops} onChange={handleChange} />
        </div>

        <div className="input-group" >
          <label htmlFor="mealsPlan" className="input-label">Meals Plan</label>
          <input type="text" id="mealsPlan" name="mealsPlan" value={formData.mealsPlan} onChange={handleChange} />
        </div>

        <div className="input-group">
          <label htmlFor="hotelStays" className="input-label">Hotel Stays</label>
          <input type="text" id="hotelStays" name="hotelStays" value={formData.hotelStays} onChange={handleChange} />
        </div>
      </div>

      <button type="submit" className="auth-button" disabled={isLoading}>
        {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip' : 'Create Trip')}
      </button>
    </form>
  );
};

export default TripPlanForm;