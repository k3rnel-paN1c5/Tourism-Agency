import React, { useState, useEffect, useMemo } from 'react';
import regionService from '../../services/TripSupervisor/regionService';
import DashboardHeader from '../../components/DashboardHeader';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import ErrorMessage from '../../components/ErrorMessage';
import RegionForm from '../../components/RegionForm';
import SearchBar from '../../components/SearchBar';
import './ManagementPage.css'; // Shared styles for management pages


/**
 * RegionManagementPage component.
 * Provides functionality to manage regions (CRUD operations).
 */
const RegionManagementPage = () => {
    const [regions, setRegions] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedRegion, setSelectedRegion] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formError, setFormError] = useState(null);
     const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        loadRegions();
    }, []);

    /**
     * Fetches all regions from the service and updates the state.
     */
    const loadRegions = async () => {
        setIsLoading(true);
        setError(null);
        setFormError(null);
        try {
            const data = await regionService.getRegions();
            setRegions(data);
        } catch (error) {
            setError("Failed to load regions. Please try again later.");
            console.error("Failed to load regions:", error);
            // Here you could set an error message in the state and display it
        }
        setIsLoading(false);
    };

    /**
     * Handles opening the modal for creating a new region.
     */
    const handleCreate = () => {
        setError(null);
        setFormError(null);
        setSelectedRegion(null);
        setIsModalOpen(true);
    };

    /**
     * Handles opening the modal for editing an existing region.
     * @param {object} region - The region to edit.
     */
    const handleEdit = (region) => {
        setError(null);
        setFormError(null);
        setSelectedRegion(region);
        setIsModalOpen(true);
    };

    /**
     * Handles deleting a region.
     * @param {string|number} id - The ID of the region to delete.
     */
    const handleDelete = async (id) => {
        // A simple confirmation dialog. For a better UX, consider a custom modal confirmation.
        setError(null);
        setFormError(null);
        try {
            await regionService.deleteRegion(id);
            loadRegions(); // Refresh the list after deletion
        } catch (error) {
            setError("Failed to delete region, Region may be used by a trip plan")
            console.error("Failed to delete region:", error);
        }

    };

    /**
     * Handles saving a new or updated region.
     * @param {object} regionData - The data of the region to save.
     */
    const handleSave = async (regionData) => {
        setError(null);
        setFormError(null);
        try {
            if (selectedRegion) {
                await regionService.updateRegion(selectedRegion.id, regionData);
            } else {
                await regionService.createRegion(regionData);
            }
            loadRegions(); // Refresh the list after saving
            setIsModalOpen(false);
        } catch (error) {
            setFormError("Failed to save region. Name already exist");
            console.error("Failed to save region:", error);
        }
    };



    const filteredRegions = useMemo(() => {
        if (!searchQuery) {
            return regions;
        }
        return regions.filter(region =>
            region.name.toLowerCase().includes(searchQuery.toLowerCase())
        );
    }, [regions, searchQuery]);

    const regionColumns = [
        { header: 'ID', key: 'id' },
        { header: 'Name', key: 'name' },
    ];
    
    return (
        <div className="management-page">

            <DashboardHeader title="Manage Regions" />
            <main className="management-content">
                <div className="actions-bar">
                    <SearchBar 
                        onSearch={setSearchQuery}
                        placeholder="Search by region name..."
                    />
                    <button onClick={handleCreate} className="btn-add-new">Add New Region</button>
                </div>

                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading regions...</p>
                ) : (
                    <DataTable
                        columns={regionColumns}
                        data={filteredRegions}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                    />
                )}
                {isModalOpen && (
                    <Modal
                        isOpen={isModalOpen}
                        onClose={() => setIsModalOpen(false)}
                        title="Creating a new region">
                            <ErrorMessage message={formError} onClear={() => setFormError(null)} />
                        <RegionForm
                            onSubmit={handleSave}
                            initialData={selectedRegion}
                            onSave={handleSave}
                            onCancel={() => setIsModalOpen(false)}
                        />
                    </Modal>
                )}
            </main>
        </div>
    );
};

export default RegionManagementPage;
