import React from "react";
import DashboardHeader from "../../components/DashboardHeader";
import DashboardBlock from "../../components/DashboardBlock"; // Import the new component
import "./CarSupervisorDashboard.css";

/**
 * CarSupervisorDashboard component.
 * Displays a dashboard with navigation blocks for managing different aspects of Cars.
 */
const CarSupervisorDashboard = () => {
  return (
    <div className="dashboard">
      <DashboardHeader title="Car Supervisor Dashboard" />
      <main className="dashboard-main-content">
        <div className="dashboard-blocks">
          {/* Use the new DashboardBlock component */}
          <DashboardBlock
            title="Manage Categories"
            description="View, add, edit, and delete categories."
            linkTo="/car-supervisor/categories"
          />
          <DashboardBlock
            title="Manage Cars"
            description="View, add, edit, and delete cars."
            linkTo="/car-supervisor/cars"
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

export default CarSupervisorDashboard;
