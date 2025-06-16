import React, { useState, useEffect } from 'react';
import tripPlanCarService from '../../services/TripSupervisor/tripPlanCarService';
import carService from '../../services/CarSupervisor/carService';
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
            const [available, allTripPlanCars, allCars] = await Promise.all([
                tripPlanCarService.getAvailableCars(tripPlan.startDate, tripPlan.endDate),
                tripPlanCarService.getTripPlanCars(),
                carService.getCars()
            ]);

            const carMap = new Map(allCars.map(car => [car.id, car]));
            
            const associated = allTripPlanCars.filter(tpc => tpc.tripPlanId === tripPlan.id);
            const associatedCarIds = new Set(associated.map(tpc => tpc.carId));
            setAssociatedCars(associated);
            setAssociatedCars(associated.map(assoc => ({
                ...assoc,
                carModel: carMap.get(assoc.carId)?.model || 'Unknown Car'
            })));
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
            await tripPlanCarService.AddTripPlanCartoPlan(newTripPlanCar);
            loadCars();
        } catch (err) {
            setError('Failed to add car to the trip plan.');
            console.error(err);
        }
    };

    const handleRemoveCar = async (tripPlanCarId) => {
        setError(null);
        try {
            await tripPlanCarService.RemoveTripPlanCarFromPlan(tripPlanCarId);
            loadCars();
        } catch (err) {
            setError('Failed to remove car from the trip plan.');
            console.error(err);
        }
    };

    return (
        <div className="trip-plan-car-form">
            <h4>Manage Cars for Trip Plan: {tripPlan.tripName}</h4>
            <ErrorMessage message={error} onClear={() => setError(null)} />
            {isLoading ? (
                <p>Loading cars...</p>
            ) : (
                <div style={{ display: 'flex', justifyContent: 'space-between', gap: '2rem' }}>
                    <div style={{ flex: 1 }}>
                        <h5>Associated Cars</h5>
                        {associatedCars.length > 0 ? (
                            <ul>
                                {associatedCars.map(assoc => (
                                    <li key={assoc.id} style={{ marginBottom: '10px' }}>
                                        {assoc.carModel}
                                        <button onClick={() => handleRemoveCar(assoc.id)} style={{ marginLeft: '10px' }}>Remove</button>
                                    </li>
                                ))}
                            </ul>
                        ) : <p>No cars associated yet.</p>}
                    </div>
                    <div style={{ flex: 1 }}>
                        <h5>Available Cars</h5>
                        {availableCars.length > 0 ? (
                            <ul>
                                {availableCars.map(car => (
                                    <li key={car.id} style={{ marginBottom: '10px' }}>
                                        {car.model}
                                        <button onClick={() => handleAddCar(car.id)} style={{ marginLeft: '10px' }}>Add</button>
                                    </li>
                                ))}
                            </ul>
                        ) : <p>No available cars for this period.</p>}
                    </div>
                </div>
            )}
            <button onClick={onClose} style={{ marginTop: '20px' }}>Close</button>
        </div>
    );
};

export default TripPlanCarForm;