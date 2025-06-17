import React, { useState, useEffect, useCallback } from 'react';
import regionService from '../../services/TripSupervisor/regionService';
import tripService from '../../services/TripSupervisor/tripService';
import Modal from '../shared/Modal';
import TripPlanCarForm from './TripPlanCarForm';
import "../shared/Form.css";

const TripPlanForm = ({ onSubmit, initialData, isLoading }) => {
  
  const [formData, setFormData] = useState({
    tripId: '',
    regionId: '',
    startDate: '',
    endDate: '',
    includedServices: '',
    stops: '',
    mealsPlan: '',
    hotelStays: '',
    price: '',
  });
  const [trips, setTrips] = useState([]);
  const [regions, setRegions] = useState([]);
  const [error, setError] = useState(null);
  const [formLoading, setFormLoading] = useState(true);
  const [isCarModalOpen, setIsCarModalOpen] = useState(false);

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
      setFormLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDropdownData();
  }, [fetchDropdownData]);

  useEffect(() => {
    let formatedStartDate = '';
    let formatedEndDate = '';
    if (initialData) {
      formatedStartDate = initialData.startDate ? new Date(initialData.startDate).toISOString().slice(0, 16):'';
      formatedEndDate = initialData.endDate ? new Date(initialData.endDate).toISOString().slice(0, 16):'';

      setFormData({
        tripId: initialData.tripId || '',
        regionId: initialData.regionId || '',
        startDate: formatedStartDate,
        endDate: formatedEndDate,
        includedServices: initialData.includedServices || '',
        stops: initialData.stops || '',
        mealsPlan: initialData.mealsPlan || '',
        hotelStays: initialData.hotelStays || '',
        price: initialData.price || '',
      });
    } else {
      setFormData({
        tripId: '',
        regionId: '',
        startDate: '',
        endDate: '',
        includedServices: '',
        stops: '',
        mealsPlan: '',
        hotelStays: '',
        price: '',
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!formData.tripId || !formData.regionId || !formData.startDate || !formData.endDate) {
      return;
    }
    
    let submitData = { ...formData };
    if (initialData && initialData.id) {
      submitData.id = initialData.id;
    }
    onSubmit(submitData);
  };

  const handleCarManagement = () => {
    setIsCarModalOpen(true);
  }

  if (formLoading) {
    return <div>Loading form...</div>;
  }

  return (
    <>
      <form onSubmit={handleSubmit} className="form">
        {!initialData && (
          <>
            <div className="form-group">
              <label htmlFor="regionId" className="form-group">Region</label>
              <select id="regionId" name="regionId" className="form-input" value={formData.regionId} onChange={handleChange} required>
                <option value="">Select a Region</option>
                {regions.map(region => (
                  <option key={region.id} value={region.id}>{region.name}</option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label htmlFor="tripId" className="form-group">Trip</label>
              <select id="tripId" name="tripId" className="form-input" value={formData.tripId} onChange={handleChange} required>
                <option value="">Select a Trip</option>
                {trips.map(trip => (
                  <option key={trip.id} value={trip.id}>{trip.name}</option>
                ))}
              </select>
            </div>
          </>
        )}
        <div className="form-group">
          <label htmlFor="startDate" className="form-group">Start Date</label>
          <input type="datetime-local" id="startDate" name="startDate" className='form-input' value={formData.startDate} onChange={handleChange} required />
        </div>
        <div className="form-group">
          <label htmlFor="endDate" className="form-group">End Date</label>
          <input type="datetime-local" id="endDate" name="endDate" className='form-input' value={formData.endDate} onChange={handleChange} required />
        </div>
        <div className="form-group">
          <label htmlFor="price" className="form-group">Price</label>
          <input type="number" id="price" name="price" className='form-input' value={formData.price} onChange={handleChange} required />
        </div>
        <div className="form-group form-group-full">
          <label htmlFor="includedServices" className="form-group">Included Services</label>
          <textarea id="includedServices" name="includedServices" className='form-input' value={formData.includedServices} onChange={handleChange} />
        </div>
        <div className="form-group form-group-full">
          <label htmlFor="stops" className="form-group">Stops</label>
          <textarea id="stops" name="stops" className='form-input' value={formData.stops} onChange={handleChange} />
        </div>
        <div className="form-group" >
          <label htmlFor="mealsPlan" className="form-group">Meals Plan</label>
          <input type="text" id="mealsPlan" name="mealsPlan" className='form-input' value={formData.mealsPlan} onChange={handleChange} />
        </div>
        <div className="form-group">
          <label htmlFor="hotelStays" className="form-group">Hotel Stays</label>
          <input type="text" id="hotelStays" name="hotelStays" className='form-input' value={formData.hotelStays} onChange={handleChange} />
        </div>

        <button type="button" className="auth-button" disabled={!initialData} onClick={handleCarManagement}>
          Manage Cars
        </button>
        <button type="submit" className="auth-button" disabled={isLoading}>
          {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip Plan' : 'Create Trip Plan')}
        </button>
      </form>
      {isCarModalOpen && (
        <Modal
          isOpen={isCarModalOpen}
          onClose={() => setIsCarModalOpen(false)}
          title={`Manage Cars for Trip Plan ${initialData.id}`}>
          <TripPlanCarForm
            tripPlan={initialData}
            onClose={() => setIsCarModalOpen(false)}
          />
        </Modal>
      )}
    </>
  );
};

export default TripPlanForm;