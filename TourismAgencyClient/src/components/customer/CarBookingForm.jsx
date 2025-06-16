import { useState } from 'react';
import ErrorMessage from '../shared/ErrorMessage';
import './CarBookingForm.css';

const CarBookingForm = ({ car, startDate, endDate, onSubmit, onCancel }) => {
    const [formData, setFormData] = useState({
        pickUpLocation: '',
        dropOffLocation: '',
        numOfPassengers: 1,
        withDriver: false,
        notes: ''
    });
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        // Basic validation
        if (!formData.pickUpLocation || !formData.dropOffLocation) {
            setError("Pickup and Drop-off locations are required.");
            return;
        }

        const bookingData = {
            carId: car.id,
            startDate,
            endDate,
            ...formData
        };

        try {
            await onSubmit(bookingData);
        } catch (err) {
            setError(err.response?.data?.message || 'An unexpected error occurred. Please try again.');
        }
    };

    return (
        <form onSubmit={handleSubmit} className="car-booking-form">
            <div className="booking-summary">
                <h4>Booking Summary</h4>
                <p><strong>Car:</strong> {car.model}</p>
                <p><strong>From:</strong> {startDate}</p>
                <p><strong>To:</strong> {endDate}</p>
            </div>

            {error && <ErrorMessage message={error} />}

            <div className="form-grid">
                <div className="form-group">
                    <label htmlFor="pickUpLocation">Pickup Location</label>
                    <input type="text" id="pickUpLocation" name="pickUpLocation" value={formData.pickUpLocation} onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label htmlFor="dropOffLocation">Drop-off Location</label>
                    <input type="text" id="dropOffLocation" name="dropOffLocation" value={formData.dropOffLocation} onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label htmlFor="numOfPassengers">Passengers</label>
                    <input type="number" id="numOfPassengers" name="numOfPassengers" min="1" max={car.seats} value={formData.numOfPassengers} onChange={handleChange} required />
                </div>
                 <div className="form-group-checkbox">
                    <input type="checkbox" id="withDriver" name="withDriver" checked={formData.withDriver} onChange={handleChange} />
                    <label htmlFor="withDriver">Include Driver</label>
                </div>
            </div>

            <div className="form-group">
                <label htmlFor="notes">Notes / Special Requests</label>
                <textarea id="notes" name="notes" rows="3" value={formData.notes} onChange={handleChange}></textarea>
            </div>

            <div className="form-actions">
                <button type="button" onClick={onCancel} className="btn-cancel">Cancel</button>
                <button type="submit" className="btn-confirm">Confirm Booking</button>
            </div>
        </form>
    );
};

export default CarBookingForm;
