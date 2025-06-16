import React, { useState, useEffect, useCallback } from 'react';
import regionService from '../../services/TripSupervisor/regionService';
import tripService from '../../services/TripSupervisor/tripService';
import TripPlanCarForm from './TripPlanCarForm';
import "../shared/Form.css";
const TripPlanForm = ({ onSubmit, initialData, isLoading, isCarModal }) => {
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
  const [Loading, setLoading] = useState(true);
  const [isCarModalOpen, setIsCarModalOpen] = useState(isCarModal);

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
    let startDate = '';
    let endDate = '';
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
        price: initialData.price || '',
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
    <>
      <form onSubmit={handleSubmit} className="form">
        {!initialData &&
          <div className="form-group">
            <label htmlFor="regionId" className="form-group">Region</label>
            <select id="regionId" name="regionId" className="form-input" value={formData.regionId} onChange={handleChange} required>
              <option value="">Select a Region</option>
              {regions.map(region => (
                <option key={region.id} value={region.id}>{region.name}</option>
              ))}
            </select>
          </div>
        }
        {!initialData &&
          <div className="form-group">
            <label htmlFor="tripId" className="form-group">Trip</label>
            <select id="tripId" name="tripId" className="form-input" value={formData.tripId} onChange={handleChange} required>
              <option value="">Select a Trip</option>
              {trips.map(trip => (
                <option key={trip.id} value={trip.id}>{trip.name}</option>
              ))}
            </select>
          </div>
        }
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

        <button  className="auth-button" disabled={isLoading} onClick={() => {setIsCarModalOpen(true)}}>
          Manage Cars
        </button>
        <button type="submit" className="auth-button" disabled={isLoading}>
          {isLoading ? <div className="spinner" /> : (initialData ? 'Update Trip' : 'Create Trip')}
        </button>
      </form>
      <div>
        {isCarModalOpen && formData.id && formData.startDate && formData.endDate && (
          <Modal
            isOpen={isCarModalOpen}
            onClose={() => setIsCarModalOpen(false)}
            title={`Manage Cars for Trip Plan ${formData.id}`}>
            <TripPlanCarForm
              tripPlan={selectedPlan}
              onClose={() => setIsCarModalOpen(false)}
            />
          </Modal>
        )}
      </div>
    </>

  );
};

export default TripPlanForm;