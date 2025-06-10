import React, { useState, useEffect, useMemo } from 'react';
import tripPlanService from '../../services/TripSupervisor/tripPlanService';
import DashboardHeader from '../../components/DashboardHeader';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import ErrorMessage from '../../components/ErrorMessage';
import SearchBar from '../../components/SearchBar';
import './ManagementPage.css'; // Shared styles for management pages
import TripPlanForm from '../../components/TripPlanForm';
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
            setPlans(data);
        } catch (error) {
            setError("Failed to load Plans. Please try again later.");
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
            setFormError("Failed to save plan" + error);
            console.error("Failed to save plan:", error);
        }
    };



    const filterdPlans = useMemo(() => {
        if (!searchQuery) {
            return plans;
        }
        return plans;
    }, [plans, searchQuery]);

    const planColumns = [
        { header: 'Number', key: 'id' },
        { header: 'Trip', key: 'tripId' },
        { header: 'Region', key: 'regionId' },
        { header: 'Start Date', key: 'startDate' },
        { header: 'End Date', key: 'endDate' },
        { header: 'Duration', key: 'duration' },
        { header: 'Included Services', key: 'includedServices' },
        { header: 'Stops', key: 'stops' },
        { header: 'Meals Plan', key: 'mealsPlan' },
        { header: 'Hotel Stays', key: 'hotelStays' },
    ];
    
    return (
        <div className="management-page">

            <DashboardHeader title="Manage Regions" />
            <main className="management-content">
                <div className="toolbar">
                    <SearchBar 
                        onSearch={setSearchQuery}
                        placeholder="Search by plan..."
                    />
                    <button onClick={handleCreate} className="btn-add-new">Add New Plan</button>
                </div>

                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading Plans...</p>
                ) : (
                    <DataTable
                        title ="Trip Plans"
                        columns={planColumns}
                        data={filterdPlans}
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
                            onSave={handleSave}
                            onCancel={() => setIsModalOpen(false)}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default TripPlanManagementPage;
