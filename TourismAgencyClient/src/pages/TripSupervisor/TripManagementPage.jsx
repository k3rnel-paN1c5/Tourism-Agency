import React, { useState, useEffect } from 'react';
import tripService  from '../../services/TripSupervisor/tripService';

import DashboardHeader from '../../components/DashboardHeader';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import TripForm from '../../components/TripForm';
import './ManagementPage.css'; // Shared styles for management pages

/**
 * TripManagementPage component.
 * Provides functionality to manage trips (CRUD operations).
 */
const TripManagementPage = () => {
    const [trips, setTrips] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedTrip, setSelectedTrip] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        // Load both trips and regions when the component mounts
        const loadData = async () => {
            setIsLoading(true);
            try {
                const tripsData = await tripService.getTrips();
                setTrips(tripsData);
            } catch (error) {
                console.error("Failed to load data:", error);
            }
            setIsLoading(false);
        };
        loadData();
    }, []);
    
    const loadTrips = async () => {
        const data = await tripService.getTrips();
        setTrips(data);
    };

    /**
     * Handles opening the modal for creating a new trip.
     */
    const handleCreate = () => {
        setSelectedTrip(null);
        setIsModalOpen(true);
    };

    /**
     * Handles opening the modal for editing an existing trip.
     * @param {object} trip - The trip to edit.
     */
    const handleEdit = (trip) => {
        setSelectedTrip(trip);
        setIsModalOpen(true);
    };

    /**
     * Handles deleting a trip.
     * @param {string|number} id - The ID of the trip to delete.
     */
    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this trip?')) {
            try {
                await tripService.deleteTrip(id);
                loadTrips(); // Refresh the list
            } catch (error) {
                console.error("Failed to delete trip:", error);
            }
        }
    };

    /**
     * Handles saving a new or updated trip.
     * @param {object} tripData - The data of the trip to save.
     */
    const handleSave = async (tripData) => {
        try {
            if (selectedTrip) {
                await tripService.updateTrip(selectedTrip.id, tripData);
            } else {
                await tripService.createTrip(tripData);
            }
            loadTrips(); // Refresh the list
            setIsModalOpen(false);
        } catch (error) {
            console.error("Failed to save trip:", error);
        }
    };

    // Define columns for the DataTable
    const tripColumns = [
        { header: 'ID', key: 'id' },
        { header: 'Name', key: 'name' },
        { header: 'Description', key: 'description' },
        { header: 'Available', key: 'isAvailable' },
        { header: 'Private', key: 'isPrivate' },
    ];

    return (
        <div className="management-page">
            <DashboardHeader title="Manage Trips" />
            <main className="management-content">
                <div className="actions-bar">
                    <button onClick={handleCreate} className="btn-add-new">Add New Trip</button>
                </div>
                {isLoading ? (
                    <p>Loading trips...</p>
                ) : (
                    <DataTable
                        columns={tripColumns}
                        data={trips}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                    />
                )}
                {isModalOpen && (
                    <Modal onClose={() => setIsModalOpen(false)}>
                        <TripForm
                            trip={selectedTrip}
                            onSave={handleSave}
                            onCancel={() => setIsModalOpen(false)}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default TripManagementPage;
