import React, { useState, useEffect, useMemo } from 'react';
import tripPlanService from '../../services/TripSupervisor/tripPlanService';
import regionService from '../../services/TripSupervisor/regionService';
import tripService from '../../services/TripSupervisor/tripService';
import DashboardHeader from '../../components/DashboardHeader';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import ErrorMessage from '../../components/ErrorMessage';
import SearchBar from '../../components/SearchBar';
import './ManagementPage.css'; // Shared styles for management pages
import TripPlanForm from '../../components/TripPlanForm';

/**
 * Calculates the duration between two dates
 * @param {string} startDateStr - The start date string (e.g., "2024-06-11").
 * @param {string} endDateStr - The end date string (e.g., "2024-06-15").
 * @returns {string|null} The formatted duration string "d.hh:mm:ss" or null if dates are invalid.
 */
const calculateDuration = (startDateStr, endDateStr) => {
    const startDate = new Date(startDateStr);
    const endDate = new Date(endDateStr);

    // Validate dates
    if (isNaN(startDate.getTime()) || isNaN(endDate.getTime()) || endDate < startDate) {
        return null;
    }

    const diffInMs = endDate.getTime() - startDate.getTime();

    const days = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diffInMs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diffInMs % (1000 * 60 * 60)) / (1000 * 60));

    // Format "d.hh:mm"
    return `${days}.${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}`;
};

/**
 * TripPlansManagementPage component.
 * A placeholder page for managing trip plans.
 */

const TripPlanManagementPage = () => {
    const [plans, setPlans] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedPlan, setSelectedPlan] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formError, setFormError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [regions, setRegions] = useState([]);
    const [trips, setTrips] = useState([]); 
    const [tripFilter, setTripFilter] = useState('');
    const [regionFilter, setRegionFilter] = useState('');
    const [startDateFilter, setStartDateFilter] = useState('');
    const [endDateFilter, setEndDateFilter] = useState('');

    useEffect(() => {
        loadPlans();
    }, []);

    /**
     * Fetches all plans from the service and updates the state.
     */
    const loadPlans = async () => {
        setIsLoading(true);
        setError(null);
        setFormError(null);
        try {
            const data = await tripPlanService.getTripPlans();
            const regionsData = await regionService.getRegions();
            const tripsData = await tripService.getTrips();
            setPlans(data);
            setRegions(regionsData);
            setTrips(tripsData);
                    console.log(data);
        } catch (error) {
            setError("Failed to load Data. Please try again later.");
            console.error("Failed to load plans:", error);
            // Here you could set an error message in the state and display it
        }
        setIsLoading(false);
    };

    /**
     * Handles opening the modal for creating a new plan.
     */
    const handleCreate = () => {
        setError(null);
        setFormError(null);
        setSelectedPlan(null);
        setIsModalOpen(true);
    };

    /**
     * Handles opening the modal for editing an existing plan.
     * @param {object} plan - The plan to edit.
     */
    const handleEdit = (plan) => {
        setError(null);
        setFormError(null);
        setSelectedPlan(plan);
        setIsModalOpen(true);
    };

    /**
     * Handles deleting a plan.
     * @param {string|number} id - The ID of the plan to delete.
     */
    const handleDelete = async (id) => {
        setError(null);
        setFormError(null);
        try {
            await tripPlanService.deleteTripPlan(id);
            loadPlans(); // Refresh the list after deletion
        } catch (error) {
            setError("Failed to delete plan" + error)
            console.error("Failed to delete region:", error);
        }

    };

    /**
     * Handles saving a new or updated plan.
     * @param {object} planData - The data of the plan to save.
     */
    const handleSave = async (planData) => {
        setError(null);
        setFormError(null);
        try {
            if (selectedPlan) {
                await tripPlanService.updateTripPlan(selectedPlan.id, planData);
            } else {
                await tripPlanService.createTripPlan(planData);
            }
            loadPlans(); // Refresh the list after saving
            setIsModalOpen(false);
        } catch (error) {
            setFormError(error.response.data.details);
            console.log("Failed to save plan:", error);
        }
    };



    const processedPlans = useMemo(() => {
        let filteredPlans = plans;
        const regionMap = new Map(regions.map(region => [region.id, region.name]));
        const tripMap = new Map(trips.map(trip => [trip.id, trip.name]));


        if (searchQuery) {
            filteredPlans = filteredPlans.filter(plan =>
                (tripMap.get(plan.tripId) || '').toLowerCase().includes(searchQuery.toLowerCase()) ||
                (regionMap.get(plan.regionId) || '').toLowerCase().includes(searchQuery.toLowerCase())
            );
        }

        if (tripFilter) {
            filteredPlans = filteredPlans.filter(plan => plan.tripId == tripFilter);
        }

        if (regionFilter) {
            filteredPlans = filteredPlans.filter(plan => plan.regionId == regionFilter);
        }

        if (startDateFilter) {
            filteredPlans = filteredPlans.filter(plan => new Date(plan.startDate) >= new Date(startDateFilter));
        }

        if (endDateFilter) {
            filteredPlans = filteredPlans.filter(plan => new Date(plan.endDate) <= new Date(endDateFilter));
        }

        return filteredPlans.map(plan => ({
            ...plan,
            regionName: regionMap.get(plan.regionId) || 'N/A',
            tripName: tripMap.get(plan.tripId) || 'N/A',
            startDate: new Date(plan.startDate).toLocaleString(),
            endDate: new Date(plan.endDate).toLocaleString(),
            duration: calculateDuration(plan.startDate, plan.endDate),
        }));
    }, [plans, regions, trips, searchQuery, tripFilter, regionFilter, startDateFilter, endDateFilter]);

    const planColumns = [
        { header: 'Number', key: 'id' },
        { header: 'Trip', key: 'tripName' },
        { header: 'Region', key: 'regionName' },
        { header: 'Start Date', key: 'startDate' },
        { header: 'End Date', key: 'endDate' },
        { header: 'Duration', key: 'duration' },
        { header: 'Price', key: 'price' },
        { header: 'Included Services', key: 'includedServices' },
        { header: 'Stops', key: 'stops' },
        { header: 'Meals Plan', key: 'mealsPlan' },
        { header: 'Hotel Stays', key: 'hotelStays' },
    ];
    
    return (
        <div className="management-page">

            <DashboardHeader title="Manage Plans" />
            <main className="management-content">
                <div className="toolbar">
                    <SearchBar 
                        onSearch={setSearchQuery}
                        placeholder="Search by plan..."
                    />
                    <div className="filters">
                        <div className="form-group">
                            <label htmlFor="trip-filter" className="form-group">Trip:</label>
                            <select id="trip-filter" className="form-input" value={tripFilter} onChange={(e) => setTripFilter(e.target.value)}>
                                <option value="">All</option>
                                {trips.map(trip => (
                                    <option key={trip.id} value={trip.id}>{trip.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="form-group">
                            <label htmlFor="region-filter" className="form-group">Region:</label>
                            <select id="region-filter" className="form-input" value={regionFilter} onChange={(e) => setRegionFilter(e.target.value)}>
                                <option value="">All</option>
                                {regions.map(region => (
                                    <option key={region.id} value={region.id}>{region.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="form-group">
                            <label htmlFor="start-date-filter" className="form-group">Start Date:</label>
                            <input type="date" id="start-date-filter" className="form-input" value={startDateFilter} onChange={(e) => setStartDateFilter(e.target.value)} />
                        </div>
                        <div className="form-group">
                            <label htmlFor="end-date-filter" className="form-group">End Date:</label>
                            <input type="date" id="end-date-filter" className="form-input" value={endDateFilter} onChange={(e) => setEndDateFilter(e.target.value)} />
                        </div>
                    </div>
                    <button onClick={handleCreate} className="btn-add-new">Add New Plan</button>
                </div>

                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading Plans...</p>
                ) : (
                    <DataTable
                        title ="Trip Plans"
                        columns={planColumns}
                        data={processedPlans}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                    />
                )}
                {isModalOpen && (
                    <Modal
                        isOpen={isModalOpen}
                        onClose={() => setIsModalOpen(false)}
                        title={selectedPlan ? "Update Plan":"Creating a new plan"}>
                        <ErrorMessage message={formError} onClear={() => setFormError(null)} />
                        <TripPlanForm
                            onSubmit={handleSave}
                            initialData={selectedPlan}
                            isLoading={isLoading}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default TripPlanManagementPage;
