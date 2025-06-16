import React, { useState, useEffect } from 'react';
import carService from '../../services/CarSupervisor/carService';
import tripPlanCarService from '../../services/TripSupervisor/tripPlanCarService';
import ErrorMessage from '../shared/ErrorMessage';

const TripPlanCarForm = ({ tripPlan, onClose }) => {
    const [availableCars, setAvailableCars] = useState([]);
    const [associatedCars, setAssociatedCars] = useState([]);
    const [error, setError] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        loadCars();
    }, [tripPlan]);

    const loadCars = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const [available, allTripPlanCars] = await Promise.all([
                carService.getAvailableCars(),
                tripPlanCarService.getTripPlanCars()
            ]);

            const associated = allTripPlanCars.filter(tpc => tpc.tripPlanId === tripPlan.id);
            const associatedCarIds = new Set(associated.map(tpc => tpc.carId));

            setAssociatedCars(associated);
            setAvailableCars(available.filter(car => !associatedCarIds.has(car.id)));
        } catch (err) {
            setError('Failed to load cars. Please try again later.');
            console.error(err);
        }
        setIsLoading(false);
    };

    const handleAddCar = async (carId) => {
        setError(null);
        try {
            const newTripPlanCar = {
                tripPlanId: tripPlan.id,
                carId: carId,
                startDate: tripPlan.startDate,
                endDate: tripPlan.endDate,
            };
            await tripPlanCarService.createTripPlanCar(newTripPlanCar);
            loadCars(); // Refresh lists
        } catch (err) {
            setError('Failed to add car to the trip plan.');
            console.error(err);
        }
    };

    const handleRemoveCar = async (tripPlanCarId) => {
        setError(null);
        try {
            await tripPlanCarService.deleteTripPlanCar(tripPlanCarId);
            loadCars(); // Refresh lists
        } catch (err) {
            setError('Failed to remove car from the trip plan.');
            console.error(err);
        }
    };

    return (
        <div className="trip-plan-car-form">
            <h4>Manage Cars for Trip Plan: {tripPlan.id}</h4>
            <ErrorMessage message={error} onClear={() => setError(null)} />
            {isLoading ? (
                <p>Loading cars...</p>
            ) : (
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <div>
                        <h5>Associated Cars</h5>
                        <ul>
                            {associatedCars.map(car => (
                                <li key={car.id}>
                                    Car ID: {car.carId}
                                    <button onClick={() => handleRemoveCar(car.id)} style={{ marginLeft: '10px' }}>Remove</button>
                                </li>
                            ))}
                        </ul>
                    </div>
                    <div>
                        <h5>Available Cars</h5>
                        <ul>
                            {availableCars.map(car => (
                                <li key={car.id}>
                                    {car.brand} {car.model} ({car.year})
                                    <button onClick={() => handleAddCar(car.id)} style={{ marginLeft: '10px' }}>Add</button>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            )}
            <button onClick={onClose} style={{ marginTop: '20px' }}>Close</button>
        </div>
    );
};

export default TripPlanCarForm;