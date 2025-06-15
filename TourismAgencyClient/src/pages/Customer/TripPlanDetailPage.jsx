import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import DashboardHeader from '../../components/shared/DashboardHeader';
import bookingService from '../../services/bookingService';
import ErrorMessage from '../../components/shared/ErrorMessage';
import './TripPlanDetailPage.css';

// Reusable Booking Form
const TripDetailBookingForm = ({ onSubmit }) => {
    const [formData, setFormData] = useState({
        numOfPassengers: 1,
        withGuide: false,
    });

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
        <form onSubmit={handleSubmit} className="detail-booking-form">
            <div className="form-group">
                <label htmlFor="numOfPassengers">Number of Passengers</label>
                <input 
                    type="number"
                    id="numOfPassengers"
                    name="numOfPassengers"
                    value={formData.numOfPassengers}
                    onChange={handleChange}
                    min="1"
                    required
                />
            </div>
            <div className="form-group checkbox-group">
                <input 
                    type="checkbox"
                    id="withGuide"
                    name="withGuide"
                    checked={formData.withGuide}
                    onChange={handleChange}
                />
                <label htmlFor="withGuide">Include a Guide?</label>
            </div>
            <button type="submit" className="submit-booking-btn">Book this trip!</button>
        </form>
    );
};


const TripPlanDetailPage = () => {
    const { id } = useParams();
    const [plan, setPlan] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState('');

    useEffect(() => {
        const loadTripPlan = async () => {
            setIsLoading(true);
            try {
                const planData = await bookingService.getTripPlanById(id);
                setPlan(planData);
            } catch (err) {
                setError('Failed to load trip plan details.');
                console.error(err);
            } finally {
                setIsLoading(false);
            }
        };
        loadTripPlan();
    }, [id]);

    const handleBookingSubmit = async (bookingData) => {
        setError(null);
        setSuccessMessage('');
        try {
            await bookingService.createTripBooking({ ...bookingData, tripPlanId: id , startDate:plan.startDate, endDate:plan.endDate,});
            setSuccessMessage('Trip booked successfully!');
        } catch (err) {
            setError('Failed to book the trip. Please try again.');
            console.error('Booking failed:', err);
        }
    };

    if (isLoading) return <p className="loading-message">Loading details...</p>;
    if (error && !plan) return <ErrorMessage message={error} />;
    if (!plan) return <p>Trip plan not found.</p>;

    console.log("********");
    console.log(plan);
    return (
        
        <div className="trip-plan-detail-page">
            <DashboardHeader title={plan.trip.name} subtitle={ "An unforgettable journey"} />
            <main className="trip-plan-detail-content">
                <div className="detail-layout">
                    <div className="plan-info">
                        <h2>Trip Details</h2>
                        <p>{plan.trip.description || "Detailed description coming soon."}</p>
                        <ul>
                            <li><strong>Start Date:</strong> {plan.startDate}</li>
                            <li><strong>End Date:</strong> {plan.endDate}</li>
                            <li><strong>Duration:</strong> {plan.duration || 'N/A'}</li>
                            <li><strong>Hotel Stays:</strong> {plan.hotelStays}</li>
                            <li><strong>Included Services:</strong> {plan.includedServices}</li>
                            <li><strong>Meals Plan:</strong> {plan.mealsPlan}</li>
                            <li><strong>Region:</strong> {plan.region.name}</li>
                            <li><strong>Stops:</strong>{plan.stops}</li>
                            <li><strong>Price:</strong> ${plan.price}</li>
                            {/* You can add more trip plan details here as they become available */}
                        </ul>
                    </div>
                    <div className="booking-section">
                        <h2>Book This Trip</h2>
                        {error && <ErrorMessage message={error} onClear={() => setError(null)} />}
                        {successMessage && <div className="success-message">{successMessage}</div>}
                        <TripDetailBookingForm onSubmit={handleBookingSubmit}/>
                    </div>
                </div>
            </main>
        </div>
    );
};

export default TripPlanDetailPage;
