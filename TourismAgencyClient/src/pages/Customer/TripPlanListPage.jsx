import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import DashboardHeader from '../../components/shared/DashboardHeader';
import bookingService from '../../services/bookingService';
import ErrorMessage from '../../components/shared/ErrorMessage';
import './TripPlanListPage.css';

// Reusable TripPlanCard component
const TripPlanCard = ({ plan }) => {
    return (
        <Link to={`/trip-plans/${plan.id}`} className="trip-plan-card">
            <div className="trip-plan-card-image">
                {/* <img src={`https://picsum.photos/seed/${plan.id}/400/300`} alt={plan.tripName} /> */}
            </div>
            <div className="trip-plan-card-content">
                <h3 className="trip-plan-card-title">{plan.trip.name}</h3>
                <p className="trip-plan-card-description">{plan.trip.description || 'No description available.'}</p>
                <div className="trip-plan-card-footer">
                    <span className="trip-plan-card-price">${plan.price}</span>
                    <span className="trip-plan-card-link">View Details &rarr;</span>
                </div>
            </div>
        </Link>
    );
};


const TripPlanListPage = () => {
    const [tripPlans, setTripPlans] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadTripPlans = async () => {
            setIsLoading(true);
            try {
                const plans = await bookingService.getUpcomingPlans();
                setTripPlans(plans);
            } catch (err) {
                setError('Failed to load available trip plans. Please try again later.');
                console.error('Failed to load trip plans:', err);
            } finally {
                setIsLoading(false);
            }
        };
        loadTripPlans();
    }, []);

    return (
        <div className="trip-plan-list-page">
            <DashboardHeader 
                title="Explore Our Trip Plans" 
                subtitle="Select a plan to see more details and book your next adventure."
            />
            <main className="trip-plan-list-content">
                {error && <ErrorMessage message={error} onClear={() => setError(null)} />}
                
                {isLoading ? (
                    <p className="loading-message">Loading trip plans...</p>
                ) : (
                    <div className="trip-plans-grid">
                        {tripPlans.map(plan => (
                            <TripPlanCard key={plan.id} plan={plan} />
                        ))}
                    </div>
                )}
            </main>
        </div>
    );
};

export default TripPlanListPage;

