import React from 'react';
import DashboardHeader from '../../components/shared/DashboardHeader';
import DashboardBlock from '../../components/shared/DashboardBlock'; // Import the new component
import './TripSupervisorDashboard.css';

/**
 * TripSupervisorDashboard component.
 * Displays a dashboard with navigation blocks for managing different aspects of trips.
 */
const TripSupervisorDashboard = () => {
    return (
        <div className="dashboard">
            <DashboardHeader title="Trip Supervisor Dashboard" />
            <main className="dashboard-main-content">
                <div className="dashboard-blocks">
                    {/* Use the new DashboardBlock component */}
                    <DashboardBlock
                        title="Manage Regions"
                        description="View, add, edit, and delete regions."
                        linkTo="/trip-supervisor/regions"
                    />
                    <DashboardBlock
                        title="Manage Trips"
                        description="View, add, edit, and delete trips."
                        linkTo="/trip-supervisor/trips"
                    />
                    <DashboardBlock
                        title="Manage Trip Plans"
                        description="View, add, edit, and delete trip plans."
                        linkTo="/trip-supervisor/trip-plans"
                    />
                    <DashboardBlock
                        title="Manage Trip Bookings"
                        description="View, Accept, Reject Trip bookings"
                        linkTo="/trip-supervisor/trip-bookings"
                    />
                    <DashboardBlock
                        title="Future Use"
                        description="This block is reserved for future features."
                        isEmpty={true}
                    />
                </div>
            </main>
        </div>
    );
};

export default TripSupervisorDashboard;
