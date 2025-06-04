import React, { useState, useEffect, useCallback } from 'react';
import DashboardHeader from '../../components/DashboardHeader';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import RegionForm from '../../components/RegionForm';
import TripForm from '../../components/TripForm';
import regionService from '../../services/TripSupervisor/regionService';
import tripService from '../../services/TripSupervisor/tripService';
import './TripSupervisorDashboard.css'; 

export default function TripSupervisorDashboard() {
  const [regions, setRegions] = useState([]);
  const [trips, setTrips] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const [isRegionModalOpen, setIsRegionModalOpen] = useState(false);
  const [isTripModalOpen, setIsTripModalOpen] = useState(false);
  const [editingRegion, setEditingRegion] = useState(null);
  const [editingTrip, setEditingTrip] = useState(null);

  const fetchRegions = useCallback(async () => {
    try {
      setIsLoading(true);
      const data = await regionService.getRegions();
      setRegions(Array.isArray(data) ? data : []);
      setError(null);
    } catch (err) {
      setError('Failed to load regions.');
      console.error(err);
      setRegions([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const fetchTrips = useCallback(async () => {
    try {
      setIsLoading(true);
      const data = await tripService.getTrips();
      setTrips(Array.isArray(data) ? data : []);
      setError(null);
    } catch (err) {
      setError('Failed to load trips.');
      console.error(err);
      setTrips([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchRegions();
    fetchTrips();
  }, [fetchRegions, fetchTrips]);

  // Region Actions
  const handleCreateRegion = () => {
    setEditingRegion(null);
    setIsRegionModalOpen(true);
  };

  const handleEditRegion = (region) => {
    setEditingRegion(region);
    setIsRegionModalOpen(true);
  };

  const handleDeleteRegion = async (regionId) => {
    if (window.confirm('Are you sure you want to delete this region?')) {
      try {
        setIsLoading(true);
        await regionService.deleteRegion(regionId);
        fetchRegions(); // Refresh
      } catch (err) {
        setError('Failed to delete region.');
        console.error(err);
      } finally {
        setIsLoading(false);
      }
    }
  };

  const handleRegionFormSubmit = async (regionData) => {
    try {
      setIsLoading(true);
      if (editingRegion) {
        await regionService.updateRegion(editingRegion.id, regionData);
      } else {
        await regionService.createRegion(regionData);
      }
      setIsRegionModalOpen(false);
      fetchRegions(); // Refresh
    } catch (err) {
      setError(`Failed to ${editingRegion ? 'update' : 'create'} region.`);
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  // Trip Actions
  const handleCreateTrip = () => {
    setEditingTrip(null);
    setIsTripModalOpen(true);
  };

  const handleEditTrip = (trip) => {
    setEditingTrip(trip);
    setIsTripModalOpen(true);
  };

  const handleDeleteTrip = async (tripId) => {
     if (window.confirm('Are you sure you want to delete this trip?')) {
      try {
        setIsLoading(true);
        await tripService.deleteTrip(tripId);
        fetchTrips(); // Refresh
      } catch (err) {
        setError('Failed to delete trip.');
        console.error(err);
      } finally {
        setIsLoading(false);
      }
    }
  };

  const handleTripFormSubmit = async (tripData) => {
     try {
      setIsLoading(true);
      // Ensure numeric fields are numbers
      const payload = {
        ...tripData,
        price: parseFloat(tripData.price),
        maxParticipants: parseInt(tripData.maxParticipants, 10),
        regionId: parseInt(tripData.regionId, 10),
        // currentParticipants is usually managed by backend or through bookings
      };

      if (editingTrip) {
        await tripService.updateTrip(editingTrip.id, payload);
      } else {
        await tripService.createTrip(payload);
      }
      setIsTripModalOpen(false);
      fetchTrips(); // Refresh
    } catch (err) {
      setError(`Failed to ${editingTrip ? 'update' : 'create'} trip.`);
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };


  const regionColumns = [
    { header: 'ID', key: 'id' },
    { header: 'Name', key: 'name' },
    { header: 'Description', key: 'description' },
  ];

  const tripColumns = [
    { header: 'ID', key: 'id' },
    { header: 'Name', key: 'name' },
    { header: 'Region', key: 'regionName' }, // Assuming you map regionId to regionName when fetching/displaying
    { header: 'Start Date', key: 'startDateFormatted' }, // Assuming you format dates
    { header: 'End Date', key: 'endDateFormatted' },
    { header: 'Price', key: 'price' },
    { header: 'Status', key: 'status' },
  ];
  
  // Helper to format dates and map region names for trips display
  const processedTrips = trips.map(trip => ({
    ...trip,
    startDateFormatted: trip.startDate ? new Date(trip.startDate).toLocaleDateString() : 'N/A',
    endDateFormatted: trip.endDate ? new Date(trip.endDate).toLocaleDateString() : 'N/A',
    regionName: regions.find(r => r.id === trip.regionId)?.name || 'Unknown',
  }));


  return (
    <div className="trip-supervisor-dashboard-container">
      <div className="dashboard-wrapper"> {/* Reusing .dashboard-wrapper for consistent max-width and centering */}
        <DashboardHeader title="Trip Supervisor Dashboard" subtitle="Manage Regions and Trips" />

        {error && <p className="error-message">{error}</p>}
        {isLoading && <p className="loading-message">Loading data...</p>}

        <DataTable
          title="Regions"
          columns={regionColumns}
          data={regions}
          onEdit={handleEditRegion}
          onDelete={handleDeleteRegion}
          onCreate={handleCreateRegion}
          createLabel="Add New Region"
        />

        <DataTable
          title="Trips"
          columns={tripColumns}
          data={processedTrips}
          onEdit={handleEditTrip}
          onDelete={handleDeleteTrip}
          onCreate={handleCreateTrip}
          createLabel="Add New Trip"
        />
        
        {/* Modals */}
        <Modal isOpen={isRegionModalOpen} onClose={() => setIsRegionModalOpen(false)} title={editingRegion ? 'Edit Region' : 'Create New Region'}>
          <RegionForm onSubmit={handleRegionFormSubmit} initialData={editingRegion} isLoading={isLoading} />
        </Modal>

        <Modal isOpen={isTripModalOpen} onClose={() => setIsTripModalOpen(false)} title={editingTrip ? 'Edit Trip' : 'Create New Trip'}>
          <TripForm onSubmit={handleTripFormSubmit} initialData={editingTrip} isLoading={isLoading} regions={regions}/>
        </Modal>

      </div>
    </div>
  );
}