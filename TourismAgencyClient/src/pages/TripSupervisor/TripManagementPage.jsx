import React, { useState, useEffect, useMemo } from 'react';
import tripService from '../../services/TripSupervisor/tripService';

import DashboardHeader from '../../components/shared/DashboardHeader';
import DataTable from '../../components/shared/DataTable';
import Modal from '../../components/shared/Modal';
import ErrorMessage from '../../components/shared/ErrorMessage';
import TripForm from '../../components/tripSupervisor/TripForm';
import SearchBar from '../../components/shared/SearchBar';
import '../shared/ManagementPage.css';

/**
 * TripManagementPage component.
 * Provides functionality to manage trips (CRUD operations).
 */
const TripManagementPage = () => {
    const [trips, setTrips] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedTrip, setSelectedTrip] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formError, setFormError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [availabilityFilter, setAvailabilityFilter] = useState('available'); // 'all', 'available', 'unavailable'
    const [privacyFilter, setPrivacyFilter] = useState('public'); // 'all', 'private', 'public'

    useEffect(() => {
        loadTrips();
    }, []);

    const loadTrips = async () => {
        setIsLoading(true);
        setError(null);
        setFormError(null);
        try {
            const tripsData = await tripService.getTrips();
            setTrips(tripsData);
        } catch (error) {
            setError("Failed to load regions. Please Try again later.")
            console.error("Failed to load data:", error);
        }
        setIsLoading(false);
    };


    /**
     * Handles opening the modal for creating a new trip.
     */
    const handleCreate = () => {
        setError(null);
        setFormError(null);
        setSelectedTrip(null);
        setIsModalOpen(true);
    };

    /**
     * Handles opening the modal for editing an existing trip.
     * @param {object} trip - The trip to edit.
     */
    const handleEdit = (trip) => {
        setError(null);
        setFormError(null);
        setSelectedTrip(trip);
        setIsModalOpen(true);
    };

    /**
     * Handles deleting a trip.
     * @param {string|number} id - The ID of the trip to delete.
     */
    const handleDelete = async (id) => {
        setError(null);
        setFormError(null);
        try {
            await tripService.deleteTrip(id);
            loadTrips(); // Refresh the list
        } catch (error) {
            setError("Failed to delete Trip, trip may be used by a trip plan")
            console.error("Failed to delete trip:", error);
        }

    };

    /**
     * Handles saving a new or updated trip.
     * @param {object} tripData - The data of the trip to save.
     */
    const handleSave = async (tripData) => {
        setError(null);
        setFormError(null);
        try {
            if (selectedTrip) {
                await tripService.updateTrip(selectedTrip.id, tripData);
            } else {
                await tripService.createTrip(tripData);
            }
            loadTrips(); // Refresh the list
            setIsModalOpen(false);
        } catch (error) {
            setFormError("Failed to save region. Name already exist");
            console.error("Failed to save trip:", error);
        }
    };

    const filteredTrips = useMemo(() => {
        if (!searchQuery && availabilityFilter === 'all' && privacyFilter === 'all') {
            return trips;
        }
        return trips.filter(trip =>
            searchQuery ?
            trip.name.toLowerCase().includes(searchQuery.toLowerCase()) : true)
                .filter(trip => {
                    // Filter by availability
                    if (availabilityFilter === 'available') return trip.isAvailable;
                    if (availabilityFilter === 'unavailable') return !trip.isAvailable;
                    return true; // 'all' returns all trips
                })
                .filter(trip => {
                    // Filter by privacy
                    if (privacyFilter === 'private') return trip.isPrivate;
                    if (privacyFilter === 'public') return !trip.isPrivate;
                    return true; // 'all' returns all trips
                });
    }, [trips, searchQuery, availabilityFilter, privacyFilter]);

    // Define columns for the DataTable
    const tripColumns = [
        { header: 'Number', key: 'id' },
        { header: 'Name', key: 'name' },
        { header: 'Description', key: 'description' },
        { header: 'Available', key: 'isAvailable' },
        { header: 'Private', key: 'isPrivate' },
    ];

    return (
        <div className="management-page">
            <DashboardHeader title="Manage Trips" />
            <main className="management-content">
                <div className="toolbar">
                    <SearchBar
                        onSearch={setSearchQuery}
                        placeholder="Search by trip name..."
                    />
                    <div className="filters">
                        <div className="filter-group">
                            <label htmlFor="availability-filter">Availability:</label>
                            <select id="availability-filter" value={availabilityFilter}  onChange={(e) => setAvailabilityFilter(e.target.value)}>
                                <option value="all">All</option>
                                <option value="available">Available</option>
                                <option value="unavailable">Unavailable</option>
                            </select>
                        </div>
                        <div className="filter-group">
                            <label htmlFor="privacy-filter">Type:</label>
                            <select id="privacy-filter" value={privacyFilter} onChange={(e) => setPrivacyFilter(e.target.value)}>
                                <option value="all">All</option>
                                <option value="private">Private</option>
                                <option value="public">Public</option>
                            </select>
                        </div>
                    </div>
                    <button onClick={handleCreate} className="btn-add-new">Add New Trip</button>
                </div>
                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading trips...</p>
                ) : (
                    <DataTable
                        title="Trips"
                        columns={tripColumns}
                        data={filteredTrips}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                    />
                )}
                {isModalOpen && (
                    <Modal
                        isOpen={isModalOpen}
                        onClose={() => setIsModalOpen(false)}
                        title={selectedTrip ? "Update Trip" : "Creating a new Trip"}>
                        <ErrorMessage message={formError} onClear={() => setFormError(null)} />
                        <TripForm
                            onSubmit={handleSave}
                            initialData={selectedTrip}
                            isLoading={isLoading}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default TripManagementPage;
