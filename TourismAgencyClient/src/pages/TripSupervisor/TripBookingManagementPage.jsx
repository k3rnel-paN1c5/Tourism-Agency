import React, { useState, useEffect, useMemo, useCallback } from 'react';
import tripBookingService from '../../services/TripSupervisor/tripBookingService';
import tripPlanService from '../../services/TripSupervisor/tripPlanService';
import DashboardHeader from '../../components/shared/DashboardHeader';
import DataTable from '../../components/shared/DataTable';
import ErrorMessage from '../../components/shared/ErrorMessage';
import '../shared/ManagementPage.css';

const TripBookingManagementPage = () => {
    const [bookings, setBookings] = useState([]);
    const [tripPlans, setTripPlans] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filters, setFilters] = useState({
        status: '0', // Default to 'Pending'
        tripPlanId: '',
        startDate: '',
        endDate: '',
    });

    const loadData = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const [bookingsData, tripPlansData] = await Promise.all([
                tripBookingService.getTripBookings(),
                tripPlanService.getTripPlans(),
            ]);
            setBookings(bookingsData);
            setTripPlans(tripPlansData);
        } catch (err) {
            setError("Failed to load data. Please try again later.");
            console.error("Failed to load data:", err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters(prev => ({ ...prev, [name]: value }));
    };

    const handleAction = async (action, id) => {
        setError(null);
        try {
            if (action === 'accept') {
                await tripBookingService.acceptTripBooking(id);
            } else if (action === 'reject') {
                await tripBookingService.rejectTripBooking(id);
            }
            loadData(); // Refresh data after action
        } catch (err) {
            setError(`Failed to ${action} booking. Please try again.`);
            console.error(`Failed to ${action} booking:`, err);
        }
    };

    const tripPlanMap = useMemo(() => new Map(tripPlans.map(plan => [plan.id, plan.trip.name])), [tripPlans]);
    const getStatusText = (status) => {
        switch (status) {
            case 0: return 'Pending';
            case 1: return 'Rejected';
            case 2: return 'Confirmed';
            default: return 'Unknown';
        }
    };

    const filteredBookings = useMemo(() => {
        
        return bookings
            .filter(booking => {
                if (filters.status && booking.status.toString() !== filters.status) return false;
                if (filters.tripPlanId && booking.tripPlanId.toString() !== filters.tripPlanId) return false;
                if (filters.startDate && new Date(booking.startDate) < new Date(filters.startDate)) return false;
                if (filters.endDate && new Date(booking.endDate) > new Date(filters.endDate)) return false;
                return true;
            })
            .map(booking => ({
                ...booking,
                tripPlanName: tripPlanMap.get(booking.tripPlanId) || 'N/A',
                statusText: getStatusText(booking.status),
                startDate: new Date(booking.startDate).toLocaleDateString(),
                endDate: new Date(booking.endDate).toLocaleDateString(),
            }));
    }, [bookings, filters, tripPlanMap]);

    const columns = [
        { header: 'ID', key: 'id' },
        { header: 'Customer ID', key: 'customerId' },
        { header: 'Trip Plan', key: 'tripPlanName' },
        { header: 'Start Date', key: 'startDate' },
        { header: 'End Date', key: 'endDate' },
        { header: 'Status', key: 'statusText' },
        { header: 'Passengers', key: 'numOfPassengers' },
        { header: 'With Guide', key: 'withGuide' },
        {
            header: 'Actions',
            key: 'actions',
            render: (item) => (
                item.status === 0 ? ( // Only show actions for Pending bookings
                    <div className="actions-cell">
                        <button onClick={() => handleAction('accept', item.id)} className="action-button edit-button">Accept</button>
                        <button onClick={() => handleAction('reject', item.id)} className="action-button delete-button">Reject</button>
                    </div>
                ) : null
            ),
        },
    ];
    return (
        <div className="management-page">
            <DashboardHeader title="Manage Trip Bookings" subtitle="Review, accept, or reject trip bookings" />
            <main className="management-content">
                <div className="toolbar">
                    <div className="filters">
                        <div className="filter-group">
                            <label htmlFor="status-filter">Status:</label>
                            <select id="status-filter" name="status" value={filters.status} onChange={handleFilterChange}>
                                <option value="">All</option>
                                <option value="0">Pending</option>
                                <option value="2">Confirmed</option>
                                <option value="1">Rejected</option>
                            </select>
                        </div>
                        <div className="filter-group">
                            <label htmlFor="trip-plan-filter">Trip Plan:</label>
                            <select id="trip-plan-filter" name="tripPlanId" value={filters.tripPlanId} onChange={handleFilterChange}>
                                <option value="">All</option>
                                {tripPlans.map(plan => (
                                    <option key={plan.id} value={plan.id}>{plan.trip.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="filter-group">
                            <label htmlFor="start-date-filter">Start Date:</label>
                            <input type="date" id="start-date-filter" name="startDate" value={filters.startDate} onChange={handleFilterChange} className="form-input" />
                        </div>
                        <div className="filter-group">
                            <label htmlFor="end-date-filter">End Date:</label>
                            <input type="date" id="end-date-filter" name="endDate" value={filters.endDate} onChange={handleFilterChange} className="form-input" />
                        </div>
                    </div>
                </div>

                <ErrorMessage message={error} onClear={() => setError(null)} />

                {isLoading ? (
                    <p>Loading bookings...</p>
                ) : (
                    <DataTable
                        title="Trip Bookings"
                        columns={columns}
                        data={filteredBookings}
                    />
                )}
            </main>
        </div>
    );
};

export default TripBookingManagementPage;